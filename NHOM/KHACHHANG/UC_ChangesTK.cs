using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NHOM.KHACHHANG
{
    public partial class UC_ChangesTK : UserControl
    {
        public UC_ChangesTK()
        {
            InitializeComponent();
        }

        private void txtMK_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Thay thế tên server và thông tin kết nối của bạn ở đây
            string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=QLNS;User ID=sa;Password='123'";



            string tenDangNhap = txtTK.Text;
            string matKhau = txtMK.Text;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    // Mở kết nối
                    conn.Open();

                    // Tạo lệnh SQL để kiểm tra đăng nhập
                    using (SqlCommand cmd = new SqlCommand("KtraDangNhap", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.AddWithValue("@UserName", tenDangNhap);
                        cmd.Parameters.AddWithValue("@Passwd", matKhau);

                        // Tham số đầu ra để lưu kết quả kiểm tra
                        SqlParameter paramKetQua = new SqlParameter("@KetQua", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(paramKetQua);

                        // Thực thi stored procedure
                        cmd.ExecuteNonQuery();

                        // Lấy giá trị từ tham số đầu ra
                        int ketQua = (int)paramKetQua.Value;

                        if (ketQua == 1)
                        {
                            // Đăng nhập thành công
                            MessageBox.Show("Bạn có muốn sửa thông tin tài khảon không!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            string TenDangNhap = txtTK.Text.Trim();
                            // Hiển thị FormDashBoard


                            txtMK.Clear();
                            txtTK.Clear();
                        }
                        else
                        {
                            // Đăng nhập thất bại
                            MessageBox.Show("Tài khoản hoặc mật khẩu không đúng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtMK.Clear();
                            txtTK.Focus();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
