using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NHOM
{
    public partial class SignUpcs : Form
    {
        string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=QL_Sach_DA;User ID=sa;Password='123'";

        public SignUpcs()
        {
            InitializeComponent();
        }

        private void labelDN_Click(object sender, EventArgs e)
        {
            Login formDN = new Login();
            formDN.Show();
            this.Hide();
        }

        private void buttonDK_Click(object sender, EventArgs e)
        {
            string fullName = txtTen.Text;
            string email = txtEmail.Text;
            string phone = txtSđt.Text;
            string password = txtMK.Text;

            // Kiểm tra nếu có trường nào trống
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!");
                return;
            }

            // Kiểm tra nếu email đã tồn tại
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();  // Mở kết nối

                    // Kiểm tra nếu tên đăng nhập đã tồn tại
                    if (IsUserNameExists(connection, fullName))
                    {
                        MessageBox.Show("Tên đăng nhập đã tồn tại!");
                        return;
                    }

                    // Kiểm tra nếu email đã tồn tại
                    if (IsEmailExists(connection, email))
                    {
                        MessageBox.Show("Email này đã được đăng ký!");
                        return;
                    }
                  


                    // Thực hiện INSERT vào bảng NGUOIDUNG
                    int userId = InsertUser(connection, fullName, email, phone);

                    if (userId > 0)
                    {
                        // Lưu mật khẩu vào bảng DANGNHAP
                        bool isLoginInserted = InsertLogin(connection, fullName, password, userId);
                       

                        if (isLoginInserted)
                        {
                            

                            MessageBox.Show("Đăng ký thành công! Vui lòng đăng nhập.");
                            // Mở form đăng nhập và ẩn form đăng ký
                            Login loginForm = new Login();
                            this.Hide();
                            loginForm.Show();
                        }
                        else
                        {
                            MessageBox.Show("Có lỗi xảy ra trong quá trình đăng ký.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Có lỗi xảy ra trong quá trình đăng ký.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message);
                }
            }
        }

        private bool IsUserNameExists(SqlConnection connection, string userName)
        {
            string query = "SELECT COUNT(*) FROM DANGNHAP WHERE UserName = @UserName";
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@UserName", userName);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        private bool IsEmailExists(SqlConnection connection, string email)
        {
            string query = "SELECT COUNT(*) FROM NGUOIDUNG WHERE Email = @Email";
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        private int InsertUser(SqlConnection connection, string fullName, string email, string phone)
        {
          

            string query = "INSERT INTO NGUOIDUNG (UserName, Email, SDT, NgaySinh,NgayDangKy, DiaChi, TrangThai) " +
                           "VALUES (@HoTen, @Email, @SDT, @NgaySinh,@NgayDangKy, @DiaChi, @TrangThai);" +
                           "SELECT CAST(SCOPE_IDENTITY() AS INT);";  // Đảm bảo trả về kiểu INT
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
             

                cmd.Parameters.AddWithValue("@HoTen", fullName);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@SDT", phone);
                cmd.Parameters.AddWithValue("@NgaySinh", DBNull.Value);  // Có thể dùng null nếu trường có thể null
                cmd.Parameters.AddWithValue("@NgayDangKy", DateTime.Now);
                cmd.Parameters.AddWithValue("@DiaChi", DBNull.Value);    // Cũng vậy cho trường này
                cmd.Parameters.AddWithValue("@TrangThai", "Active");
               

                object result = cmd.ExecuteScalar();
               

                return result != null ? Convert.ToInt32(result) : 0; // Chuyển đổi đúng kiểu dữ liệu
            }
        }

        private bool InsertLogin(SqlConnection connection, string userName, string password, int userId)
        {
           

            string query = "INSERT INTO DANGNHAP (UserName, Password, MaNguoiDung) VALUES (@UserName, @Password, @MaNguoiDung)";
          

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@Password", password);  // Không mã hóa mật khẩu nữa
                cmd.Parameters.AddWithValue("@MaNguoiDung", userId);
                
                return cmd.ExecuteNonQuery() > 0; // Chắc chắn kiểu dữ liệu là đúng
            }
        }

        // Kiểm tra định dạng email
        private bool IsValidEmail(string email)
        {
            // Biểu thức chính quy để kiểm tra định dạng email
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(emailPattern);
            return regex.IsMatch(email);
        }

        // Kiểm tra số điện thoại
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return phoneNumber.Length == 10 && phoneNumber.All(char.IsDigit);
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            string email = txtEmail.Text;

            // Kiểm tra nếu TextBox trống
            if (string.IsNullOrEmpty(email))
            {
                error.Clear(); // Xóa thông báo lỗi nếu trống
            }
            else if (!IsValidEmail(email))
            {
                error.SetError(txtEmail, "Email không hợp lệ!");
            }
            else
            {
                error.Clear(); // Xóa thông báo lỗi nếu hợp lệ
            }
        }

        private void txtSđt_Leave(object sender, EventArgs e)
        {
            string phoneNumber = txtSđt.Text;

            if (string.IsNullOrEmpty(phoneNumber))
            {
                error.Clear(); // Xóa thông báo lỗi nếu trống
            }
            else if (!IsValidPhoneNumber(phoneNumber))
            {
                error.SetError(txtSđt, "Số điện thoại không hợp lệ!");
            }
            else
            {
                error.Clear(); // Xóa thông báo lỗi nếu hợp lệ
            }
        }

        private void txtMK_Leave(object sender, EventArgs e)
        {

        }

        private void txtSđt_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
