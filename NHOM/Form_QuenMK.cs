using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;
using SendGrid;
using SendGrid.Helpers.Mail;



namespace NHOM
{
    public partial class Form_QuenMK : Form
    {
        string connectionString = @"Data Source=ADMIN-PC\SQLEXPRESS;Initial Catalog=QL_Sach_DA;User ID=sa;Password='123'";
        int otp;
        Random Random = new Random();
        public Form_QuenMK()
        {
            InitializeComponent();
        }

        private void Form_QuenMK_Load(object sender, EventArgs e)
        {
            string email = textBoxEmail.Text;
            textBoxMKMOI.Visible = false;
            LabelMKMOI.Visible = false;
            buttonLUUMKMOI.Visible = false;
            textBoxResetToken.Enabled = false;

        }



       
         

            // Kiểm tra email có tồn tại trong cơ sở dữ liệu
            public bool CheckIfEmailExists(string email)
            {
                string query = "SELECT COUNT(*) FROM NGUOIDUNG WHERE Email = @Email";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = email });
                        connection.Open();
                        int count = (int)command.ExecuteScalar();
                        return count > 0; // Trả về true nếu email tồn tại trong cơ sở dữ liệu
                    }
                }
            }

            // Tạo mã reset
            public string GenerateResetToken()
            {
                return Guid.NewGuid().ToString(); // Tạo mã duy nhất
            }

        // Nút để gửi yêu cầu mã OTP
       

            // Xử lý sự kiện khi người dùng nhấn nút xác nhận mã reset
            private async void buttonSubmitReset_Click(object sender, EventArgs e)
            {
                if (otp.ToString().Equals(textBoxResetToken.Text))
                {

                    MessageBox.Show("Xác minh thành công. Bạn có thể nhập mật khẩu mới.");
                // Hiển thị các điều khiển mật khẩu mới
                textBoxMKMOI.Visible = true;
                LabelMKMOI.Visible = true;
                buttonLUUMKMOI.Visible = true;
              
            }
                else
                {
                    MessageBox.Show("Mã xác minh không đúng. Vui lòng thử lại.");
                     textBoxMKMOI.Clear();
                        LabelMKMOI.Text = string.Empty;
            }
            }

        private void buttonLayMk_Click(object sender, EventArgs e)
        {
            textBoxResetToken.Enabled = true;
        }

        private void buttonLayMk_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra email và gửi mã OTP
                string email = textBoxEmail.Text;

                if (CheckIfEmailExists(email))
                {
                    otp = Random.Next(100000, 999999); // Tạo mã OTP ngẫu nhiên có 6 chữ số
                    var fromAdress = new MailAddress("truongmyhoa561@gmail.com");
                    var toAdress = new MailAddress(email);
                    const string fromPass = "oaxmvslqbausexko"; // Mật khẩu ứng dụng Gmail
                    const string subject = "Mã khôi phục mật khẩu";
                    string body = $"Mã khôi phục mật khẩu của bạn là: {otp}";

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAdress.Address, fromPass),
                        Timeout = 20000
                    };

                    using (var message = new MailMessage(fromAdress, toAdress)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        // Gửi email bất đồng bộ
                        smtp.Send(message);
                    }

                    MessageBox.Show("Mã đã được gửi đi. Vui lòng kiểm tra email của bạn.");
                    textBoxResetToken.Enabled = true;

                }
                else
                {
                    MessageBox.Show("Email không tồn tại trong hệ thống.");
                }
            }
            catch (Exception ex)
            {
                // Bắt lỗi chi tiết và hiển thị
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}");
            }
        }

        private void buttonLUUMKMOI_Click(object sender, EventArgs e)
        {
           
            string email = textBoxEmail.Text; // TextBox cho địa chỉ email
            string newPassword = textBoxMKMOI.Text; // TextBox cho mật khẩu mới

           
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Tìm kiếm người dùng dựa trên email
                string getUserQuery = @"
            SELECT MaNguoiDung 
            FROM NGUOIDUNG 
            WHERE Email = @Email";

                int userId;

                using (SqlCommand command = new SqlCommand(getUserQuery, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    var result = command.ExecuteScalar();

                    if (result != null)
                    {
                        userId = Convert.ToInt32(result);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy người dùng với địa chỉ email này.");
                        return;
                    }
                }

                // Cập nhật mật khẩu trong bảng DANGNHAP
                string updatePasswordQuery = @"
            UPDATE DANGNHAP 
            SET Password = @NewPassword 
            WHERE MaNguoiDung = @MaNguoiDung";

                using (SqlCommand command = new SqlCommand(updatePasswordQuery, connection))
                {
                    command.Parameters.AddWithValue("@NewPassword", newPassword);
                    command.Parameters.AddWithValue("@MaNguoiDung", userId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Mật khẩu đã được cập nhật thành công!");
                        textBoxResetToken.Clear();
                        textBoxMKMOI.Clear();
                        textBoxEmail.Clear();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Có lỗi xảy ra khi cập nhật mật khẩu.");
                    }
                }
            
        }

    }
    }








    }



