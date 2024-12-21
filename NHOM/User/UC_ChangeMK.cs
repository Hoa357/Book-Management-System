using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace NHOM.User
{
    public partial class UC_ChangeMK : UserControl
    {
        string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=QL_SACH_DA;User ID=sa;Password='123'";

      
        // Lưu tên tài khoản hoặc UserID sau khi đăng nhập thành công
        private string ten;  // Biến lưu 
       
        public UC_ChangeMK(string ten)
        {
            InitializeComponent();
         
            this.ten = ten;
        }

       

        private void GetUserInfo(string tenTK)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT UserName,Hoten ,DiaChi, Email, SDT FROM NGUOIDUNG WHERE UserName = @TenTaiKhoan";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TenTaiKhoan", tenTK);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Lấy các giá trị từ cơ sở dữ liệu
                                string hoTen = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : null;
                                string TenThat = reader["Hoten"] != DBNull.Value ? reader["HoTen"].ToString() : null;
                                string email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null;
                                string sdt = reader["SDT"] != DBNull.Value ? reader["SDT"].ToString() : null;
                                string dc = reader["DiaChi"] != DBNull.Value ? reader["SDT"].ToString() : null;
                                // Hiển thị thông tin vào các TextBox
                                textBoxTenTK.Text = string.IsNullOrEmpty(hoTen) ? "Vui lòng cập nhật thông tin" : hoTen;
                                textBoxEmail.Text = string.IsNullOrEmpty(email) ? "Vui lòng cập nhật thông tin" : email;
                                textBoxSDT.Text = string.IsNullOrEmpty(sdt) ? "Vui lòng cập nhật thông tin" : sdt;
                                textBoxDC.Text = string.IsNullOrEmpty(sdt) ? "Vui lòng cập nhật thông tin" : dc;
                                // Mật khẩu hiển thị dấu "***"
                                textBoxMK.PasswordChar = '*';
                                // Chuyển các TextBox sang chế độ chỉ đọc (không cho sửa)
                                textBoxTenTK.Enabled = false;
                                textBoxEmail.Enabled = false;
                                textBoxSDT.Enabled = false;
                                textBoxMK.Enabled = false;
                                textBoxDC.Enabled = false;
                            }
                            else
                            {
                                // Nếu không có dữ liệu trả về
                                textBoxTenTK.Text = "Vui lòng cập nhật thông tin";
                                textBoxEmail.Text = "Vui lòng cập nhật thông tin";
                                textBoxSDT.Text = "Vui lòng cập nhật thông tin";
                                textBoxMK.PasswordChar= '*';
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi kết nối cơ sở dữ liệu: " + ex.Message);
                }
            }
        }

        private void UC_ChangeMK_Load(object sender, EventArgs e)
        {
           

            // Truy vấn để lấy thông tin người dùng từ currentUsername
            GetUserInfo(ten);
          

        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        // Kiểm tra số điện thoại có đúng 10 chữ số không
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return phoneNumber.Length == 10 && phoneNumber.All(char.IsDigit);
        }


        private bool IsUsernameUnique(string username)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM NGUOIDUNG WHERE HoTen = @TenTaiKhoan";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TenTaiKhoan", username);
                    int count = (int)command.ExecuteScalar();
                    return count == 0; // Trả về true nếu tên tài khoản là duy nhất
                }
            }
        }

        // Cập nhật thông tin người dùng trong cơ sở dữ liệu

        private void UpdateUserName(string newTenTK,string tenthat, string email, string sdt, string diaChi)
        {
          
            // Kiểm tra tên tài khoản cũ (ten) có giá trị hợp lệ
            if (string.IsNullOrWhiteSpace(ten))  // Đảm bảo rằng ten không phải null hoặc rỗng
            {
                MessageBox.Show("Tên tài khoản cũ không hợp lệ.");
                
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
              
                try
                {
                    connection.Open();
                   
                    // Cập nhật thông tin người dùng trong bảng NGUOIDUNG, sử dụng UserName từ DANGNHAP
                    string queryUser = "UPDATE NGUOIDUNG SET UserName = @NewTenTaiKhoan,Hoten = @Hoten, Email = @Email, SDT = @SDT, DiaChi = @DiaChi WHERE UserName = @OldTenTaiKhoan";
                    using (SqlCommand commandUser = new SqlCommand(queryUser, connection))
                    {
                     
                        // Thêm các tham số vào câu lệnh SQL
                        commandUser.Parameters.AddWithValue("@NewTenTaiKhoan", newTenTK);  // Cập nhật tên tài khoản mới
                        commandUser.Parameters.AddWithValue("@Hoten", tenthat);
                        commandUser.Parameters.AddWithValue("@Email", email);
                        commandUser.Parameters.AddWithValue("@SDT", sdt);
                        commandUser.Parameters.AddWithValue("@DiaChi", diaChi);
                        commandUser.Parameters.AddWithValue("@OldTenTaiKhoan", ten);  // Sử dụng tên tài khoản cũ từ form đăng nhập

                        int rowsAffectedUser = commandUser.ExecuteNonQuery();
                        if (rowsAffectedUser == 0)
                        {
                            MessageBox.Show("Không thể cập nhật thông tin người dùng trong bảng NGUOIDUNG.");
                            return;
                        }
                    }

                    // Cập nhật họ tên trong bảng DANGNHAP
                    string queryLogin = "UPDATE DANGNHAP SET UserName = @NewTenTaiKhoan WHERE UserName = @OldTenTaiKhoan";
                    using (SqlCommand commandLogin = new SqlCommand(queryLogin, connection))
                    {
                    
                        // Thêm các tham số vào câu lệnh SQL
                        commandLogin.Parameters.AddWithValue("@NewTenTaiKhoan", newTenTK);  // Cập nhật UserName trong bảng DANGNHAP
                        commandLogin.Parameters.AddWithValue("@OldTenTaiKhoan", ten);  // Dùng tên tài khoản hiện tại để xác định người dùng

                        int rowsAffectedLogin = commandLogin.ExecuteNonQuery();
                        if (rowsAffectedLogin == 0)
                        {
                            MessageBox.Show("Không thể cập nhật tên tài khoản trong bảng DANGNHAP.");
                            return;
                        }
                    }

                    MessageBox.Show("Cập nhật tên tài khoản và thông tin người dùng thành công.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi kết nối cơ sở dữ liệu: " + ex.Message);
                }
            }
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {

         
                // Enable text boxes for editing user information
                textBoxTenTK.Enabled = true;
                textBoxEmail.Enabled = true;
                textBoxSDT.Enabled = true;
                textBoxMK.Enabled = true;
                textBoxDC.Enabled = true;

                // Prompt the user to confirm if they want to update personal information
                if (MessageBox.Show("Bạn có muốn sửa thông tin cá nhân không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    // Retrieve values from input fields
                    string tenTK = textBoxTenTK.Text;
                    string email = textBoxEmail.Text;
                    string sdt = textBoxSDT.Text;
                    string diaChi = textBoxDC.Text;
                    string hoten = textBoxHoten.Text;


                    // Display the option to change password
                    labelTHAYMK.Text = "Thay đổi mật khẩu";
                    labelTHAYMK.Visible = true;
                }
                else
                {
                    // Cancel the operation and return
                    return;
                }
            


        }

        private void labelTHAYMK_Click(object sender, EventArgs e)
        {
            Form_ChangePassWord f_c = new Form_ChangePassWord(ten);
            f_c.ShowDialog();
        }



        private void textBoxTenTK_Leave(object sender, EventArgs e)
        {
            
                string ten = textBoxTenTK.Text;

                // Kiểm tra nếu TextBox trống
                if (string.IsNullOrEmpty(ten))
                {
                  error.SetError(textBoxTenTK, "Vui lòng nhập tên tài khoản");
                }
                else if (!IsUsernameUnique(ten))
                {
                    error.SetError(textBoxTenTK, "Đã có tên này trong hệ thống");
                }
                else
                {
                    error.Clear(); // Xóa thông báo lỗi nếu hợp lệ
                }
            }

        private void textBoxSDT_Leave(object sender, EventArgs e)
        {
            string sdt = textBoxSDT.Text;

            // Kiểm tra nếu TextBox trống
            if (!IsValidPhoneNumber(sdt))
            {
                error.SetError( textBoxSDT, "Số điện thoại phải đủ 10 số");
            }
            else
            {
                error.Clear(); // Xóa thông báo lỗi nếu hợp lệ
            }
        }

        private void textBoxEmail_Leave(object sender, EventArgs e)
        {
            string em = textBoxEmail.Text;
            if (string.IsNullOrEmpty(em))
            {
                error.SetError(textBoxTenTK, "Vui lòng nhập tên tài khoản");
            }
            else if (!IsValidEmail(em))
            {
                error.SetError(textBoxTenTK, "Email không hợp lệ !");
            }
            else
            {
                error.Clear(); 
            }
        }

        private void buttonLuu_Click(object sender, EventArgs e)
        {
           
                // Enable text boxes for editing user information
                textBoxTenTK.Enabled = true;
                textBoxEmail.Enabled = true;
                textBoxSDT.Enabled = true;
                textBoxMK.Enabled = true;
                textBoxDC.Enabled = true;
                textBoxHoten.Enabled = true;

                // Retrieve values from input fields
                string tenTK = textBoxTenTK.Text;
                string email = textBoxEmail.Text;
                string sdt = textBoxSDT.Text;
                string diaChi = textBoxDC.Text;
                string hoten = textBoxHoten.Text;
                // Check for any invalid input before updating
                if (string.IsNullOrWhiteSpace(tenTK))
                {
                    MessageBox.Show("Tên tài khoản không được để trống.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                {
                    MessageBox.Show("Email không hợp lệ.");
                    return;
                }

                /*
                if (!IsValidPhoneNumber(sdt))
                {
                    MessageBox.Show("Số điện thoại phải có 10 chữ số.");
                    return;
                }
                */

                if (string.IsNullOrWhiteSpace(diaChi))
                {
                    MessageBox.Show("Địa chỉ không được để trống.");
                    return;
                }

                // If all fields are valid, update information in the database
                try
                {
           
                    UpdateUserName(tenTK,hoten, email, sdt, diaChi);
                    MessageBox.Show("Thông tin đã được cập nhật thành công.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra khi cập nhật thông tin: " + ex.Message);
                }

               
            

                // Display the option to change password
                labelTHAYMK.Text = "Thay đổi mật khẩu";
                labelTHAYMK.Visible = true;
            }

          

        }
    }











