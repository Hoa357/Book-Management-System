using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NHOM.KHACHHANG
{
    public partial class Form1 : Form
    {
        string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=QL_Sach_DA;User ID=sa;Password='123'";
        int mahd;
        public Form1(int mahd)
        {
            InitializeComponent();
            this.mahd = mahd;   
        }
        private decimal LayTongTien(int soHD)
        {
            decimal tongTien = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Câu truy vấn để lấy tổng tiền từ bảng HOADON
                    string query = "SELECT TongTien FROM HOADON WHERE SoHD = @SoHD";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SoHD", soHD);

                    // Thực thi truy vấn và lấy kết quả
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        tongTien = Convert.ToDecimal(result);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy hóa đơn hoặc tổng tiền chưa được nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi khi lấy tổng tiền: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return tongTien;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            buttonXUATHD.Enabled = false;
            txtTenTaiKhoan.Enabled = false;
            txtSTK.Enabled = false;

            textBoxTT.Text =  LayTongTien(mahd).ToString();
            using (WebClient client = new WebClient())
            {
                var htmlData = client.DownloadData("https://api.vietqr.io/v2/banks");
                var bankRawJson = Encoding.UTF8.GetString(htmlData);
                var listBankData = JsonConvert.DeserializeObject<Bank>(bankRawJson);

                if (listBankData != null && listBankData.data != null && listBankData.data.Any())
                {
                    // Nếu có dữ liệu trong danh sách
                    cb_nganhang.DataSource = listBankData.data;   // list banks
                    cb_nganhang.DisplayMember = "customer_name";
                    cb_nganhang.ValueMember = "bin";
                    cb_nganhang.SelectedIndex = 5;

                    cb_template.Items.Add("compact");
                    cb_template.Items.Add("compact2");
                    cb_template.Items.Add("qr_only");
                    cb_template.SelectedIndex = 0; // Chọn giá trị mặc định là compact

                }
                else
                {
                    MessageBox.Show("Không có dữ liệu ngân hàng.");
                }
            }
        }

        
        public Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            return image;
        }
        public static Image stringToImage(string binaryString)
        {
            try
            {
                // Chuyển đổi chuỗi nhị phân thành mảng byte
                byte[] imageBytes = Encoding.UTF8.GetBytes(binaryString);

                // Tạo MemoryStream từ mảng byte
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    // Tạo đối tượng Image từ MemoryStream
                    Image image = Image.FromStream(ms);
                    return image;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi tai mã code: " + ex.Message);
            }
        }
        private async void buttonQR_Click(object sender, EventArgs e)
        {
            buttonXUATHD.Enabled = true;
            try
            {
                if (string.IsNullOrWhiteSpace(txtSTK.Text) ||
                    string.IsNullOrWhiteSpace(txtTenTaiKhoan.Text) ||
                    string.IsNullOrWhiteSpace(textBoxTT.Text) ||
                    cb_nganhang.SelectedValue == null ||
                    string.IsNullOrWhiteSpace(cb_template.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                var apiRequest = new APIRequest
                {
                    acqId = Convert.ToInt32(cb_nganhang.SelectedValue),
                    accountNo = long.Parse(txtSTK.Text),
                    accountName = txtTenTaiKhoan.Text,
                    amount = Convert.ToInt32(textBoxTT.Text),
                    format = "text",
                    template = cb_template.Text
                };


                var urlApi = $"https://img.vietqr.io";
                var jsonRequest = JsonConvert.SerializeObject(apiRequest);

                var client = new RestClient(urlApi);
                //   var request = new RestRequest($"image/{cb_nganhang.SelectedValue}-{txtSTK.Text}-{cb_template.Text}.png?amount={Convert.ToInt32(textBoxTT.Text)}&addInfo={Uri.EscapeDataString(mahd)}&accountName={txtTenTaiKhoan.Text}");
                var request = new RestRequest($"image/{cb_nganhang.SelectedValue}-{txtSTK.Text}-{cb_template.Text}.png?amount={Convert.ToInt32(textBoxTT.Text)}&addInfo={mahd}&accountName={txtTenTaiKhoan.Text}");

                var response = await client.ExecuteGetAsync(request);

                if (response.IsSuccessful)
                {
                    var content = response.Content;
                    MemoryStream ms = new MemoryStream(response.RawBytes);

                    Image image = Image.FromStream(ms, true, true);
                    pictureBox1.Image = image;
                    

                }
                else
                {
                    MessageBox.Show($"Lỗi API: {response.ErrorMessage}");
                    Console.WriteLine(response.Content);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void cb_nganhang_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSTK.Text = "0843322497";
            txtTenTaiKhoan.Text = "Cao Thien Chi";
        }
    }
}
    
