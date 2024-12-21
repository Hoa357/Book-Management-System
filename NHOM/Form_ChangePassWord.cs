using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NHOM
{
    public partial class Form_ChangePassWord : Form
    {
        // Lưu trữ tên người dùng (UserName) sau khi đăng nhập thành công
        public string ten; // Tên người dùng
        string connectionString = @"Data Source=ADMIN-PC\SQLEXPRESS;Initial Catalog=QL_SACH_DA;User ID=sa;Password='123'";

        public Form_ChangePassWord(string ten)
        {
            InitializeComponent();
            this.ten = ten; // Khởi tạo tên người dùng (UserName) khi load form
        }

        private void Form_ChangePassWord_Load(object sender, EventArgs e)
        {
          
            textBoxMKCU.Enabled = false;
            textBoxMKMOI.Enabled = false;
            txtXN_MKM.Enabled = false;

            
              
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            // Kích hoạt các trường mật khẩu khi nhấn nút đổi mật khẩu
            textBoxMKCU.Enabled = true;
            textBoxMKMOI.Enabled = true;
            txtXN_MKM.Enabled = true;

            // Xóa các trường mật khẩu
            textBoxMKCU.Text = string.Empty;
            textBoxMKMOI.Text = string.Empty;
            txtXN_MKM.Text = string.Empty;

            // Hiển thị thông báo xác nhận
            if (MessageBox.Show("Bạn có chắc muốn sửa mật khẩu không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                labelPrompt.Text = "Vui lòng nhập mật khẩu cũ để xác nhận";
                labelPrompt.Visible = true;
            }
            else
            {
                return;
            }
        }





        private void buttonLUUMK_Click(object sender, EventArgs e)
        {
            string oldPassword = textBoxMKCU.Text;
            string newPassword = textBoxMKMOI.Text;
            string confirmPassword = txtXN_MKM.Text;

            // Kiểm tra xem các trường mật khẩu có trống hay không
            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin mật khẩu.");
                return;
            }

            // Kiểm tra mật khẩu cũ có khớp với mật khẩu hiện tại trong cơ sở dữ liệu
            if (!IsCorrectOldPassword(oldPassword))
            {
                MessageBox.Show("Mật khẩu cũ không đúng.");
                return;
            }

            // Kiểm tra mật khẩu mới và mật khẩu xác nhận có khớp không
            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Mật khẩu mới và mật khẩu xác nhận không khớp.");
                return;
            }

            // Cập nhật mật khẩu mới vào cơ sở dữ liệu
            try
            {
                UpdatePassword(newPassword);
                MessageBox.Show("Mật khẩu đã được cập nhật thành công.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi cập nhật mật khẩu: " + ex.Message);
            }
        }

        // Kiểm tra mật khẩu cũ với mật khẩu trong cơ sở dữ liệu
        private bool IsCorrectOldPassword(string oldPassword)
        {
            try
            {
             

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Passwd FROM DANGNHAP WHERE UserName = @UserName";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Thêm tham số cho UserName
                    cmd.Parameters.AddWithValue("@UserName", ten);
                    MessageBox.Show("UserNamela: " + ten);
                    // Thực hiện truy vấn và lấy kết quả
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        string currentPassword = result.ToString(); // Lấy mật khẩu hiện tại từ cơ sở dữ liệu

                        // Kiểm tra mật khẩu cũ nhập vào có khớp với mật khẩu trong cơ sở dữ liệu không
                        if (currentPassword == oldPassword)
                        {
                            return true; // Mật khẩu đúng
                        }
                        else
                        {
                            MessageBox.Show("Mật khẩu cũ không đúng");
                            return false; // Mật khẩu cũ không đúng
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy người dùng với ID này.");
                        return false; // Không tìm thấy người dùng
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi kiểm tra mật khẩu cũ: " + ex.Message);
                return false;
            }
        }

        // Cập nhật mật khẩu mới vào cơ sở dữ liệu
        private void UpdatePassword(string newPassword)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE DANGNHAP SET Passwd = @NewPassword WHERE UserName = @UserName";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@NewPassword", newPassword);
                    cmd.Parameters.AddWithValue("@UserName", ten); // Sử dụng tên người dùng để cập nhật mật khẩu

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception("Không tìm thấy người dùng để cập nhật mật khẩu.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi cập nhật mật khẩu: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form_QuenMK fc = new Form_QuenMK();
            fc.ShowDialog();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                // Hiển thị mật khẩu
                textBoxMKCU.PasswordChar = '*';
         
            }
            else
            {
                textBoxMKCU.PasswordChar = '\0';
               
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                // Hiển thị mật khẩu
                textBoxMKMOI.PasswordChar = '*';

            }
            else
            {
                textBoxMKMOI.PasswordChar = '\0';

            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                // Hiển thị mật khẩu
                txtXN_MKM.PasswordChar = '*';

            }
            else
            {
                txtXN_MKM.PasswordChar = '\0';

            }
        }
    }
}
