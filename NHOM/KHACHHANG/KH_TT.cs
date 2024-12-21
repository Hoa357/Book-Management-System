using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NHOM.User
{
    public partial class KH_TT : Form
    {
        int mahd;
        public KH_TT(int mahd)
        {
            InitializeComponent();
            this.mahd = mahd;
        }

            string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=QL_SACH_DA;User ID=sa;Password='123'";

            private void txtSDT_TextChanged(object sender, EventArgs e)
            {
                string sdt = txtSDT.Text.Trim(); // Lấy giá trị số điện thoại từ textbox và loại bỏ khoảng trắng

                if (sdt.Length == 10 && sdt.All(char.IsDigit)) // Kiểm tra nếu số điện thoại hợp lệ
                {
                    // Truy vấn cơ sở dữ liệu để lấy thông tin khách hàng
                    string query = "SELECT MaKH, TenKH FROM KhachHang WHERE SDT = @SDT";
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@SDT", sdt);

                        try
                        {
                            conn.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                // Hiển thị mã khách hàng và tên khách hàng lên các label
                                labelMa.Text = "Mã khách hàng là:" + reader["MaKH"].ToString();
                                labelTen.Text = "Tên khách hàng là:" + reader["TenKH"].ToString();

                                // Sau khi tìm thấy khách hàng, cập nhật mã khách hàng vào hóa đơn
                                int maKH = Convert.ToInt32(reader["MaKH"]);
                                UpdateMaKhachHangTrongHoaDon(maKH); // Cập nhật mã khách hàng vào hóa đơn
                            }
                            else
                            {
                                // Nếu không tìm thấy khách hàng, xóa nội dung các label
                                labelMa.Text = "Không tìm thấy";
                                labelTen.Text = "Không tìm thấy";
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Có lỗi khi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    // Nếu số điện thoại không hợp lệ, xóa nội dung các label
                    labelMa.Text = "";
                    labelTen.Text = "";
                }
            }

            private void UpdateMaKhachHangTrongHoaDon(int maKH)
            {
                string query = "UPDATE HOADON SET MaKH = @MaKH WHERE SoHD = @SoHD";  // Cập nhật vào bảng HOADON dựa trên mã hóa đơn

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaKH", maKH);
                        cmd.Parameters.AddWithValue("@SoHD", mahd); // Dùng mã đơn hàng (mahd) để xác định hóa đơn cần cập nhật

                        try
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery(); // Thực thi câu lệnh
                            MessageBox.Show("Cập nhật mã khách hàng thành công vào bảng HOADON!");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Có lỗi khi cập nhật mã khách hàng vào bảng HOADON: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

        private void KH_TT_Load(object sender, EventArgs e)
        {

        }
    }
    }
