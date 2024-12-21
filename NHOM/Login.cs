using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NHOM
{
    public partial class Login : Form
    {

        // Kết nối với cơ sở dữ liệu
        string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=QL_Sach_DA;User ID=sa;Password='123'";
        private int mahd;
        public Login()
        {
            InitializeComponent();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            txtMK.PasswordChar = '*';
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
            private void button1_Click(object sender, EventArgs e)
            {
                string tenDangNhap = txtTen.Text.Trim();
                string matKhau = txtMK.Text.Trim();

                // Kiểm tra các trường nhập liệu trống
                if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ tên đăng nhập và mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

              

               
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        // Mở kết nối
                        conn.Open();

                        // Tạo lệnh SQL để kiểm tra đăng nhập
                        using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM DANGNHAP WHERE UserName = @UserName AND Password = @Password", conn))
                        {
                            // Thêm tham số vào câu lệnh SQL
                            cmd.Parameters.AddWithValue("@UserName", tenDangNhap);
                            cmd.Parameters.AddWithValue("@Password", matKhau);

                            // Thực thi câu lệnh SQL và lấy kết quả
                            int result = (int)cmd.ExecuteScalar();

                            if (result > 0)
                            {
                                // Đăng nhập thành công
                                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Sau khi đăng nhập thành công, mở Form chính (Ví dụ: FormDashBoard)
                                using (FormDashBoard fd = new FormDashBoard(tenDangNhap, mahd))
                                {
                                    fd.ShowDialog();
                                }

                                // Xóa thông tin đăng nhập sau khi thành công
                                txtMK.Clear();
                                txtTen.Clear();
                            }
                            else
                            {
                                // Đăng nhập thất bại
                                MessageBox.Show("Tài khoản hoặc mật khẩu không đúng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtMK.Clear();
                                txtTen.Focus();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Thông báo lỗi kết nối cơ sở dữ liệu
                        MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        
        
        private void pictureBox3_Click(object sender, EventArgs e)
        {

            SignUpcs formDK = new SignUpcs();
            formDK.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Form_QuenMK fromQMK = new Form_QuenMK();
            fromQMK.Show();
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
           
                if (checkBox1.Checked)
                {
                    // Hiển thị mật khẩu
                    txtMK.PasswordChar = '\0'; // '\0' có nghĩa là không có ký tự nào được hiển thị
                }
                else
                {
                    // Ẩn mật khẩu, hiển thị dấu '*'
                    txtMK.PasswordChar = '*';
               }
            

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {
            SignUpcs fromQMK = new SignUpcs();
            fromQMK.Show();
        }
    }




}