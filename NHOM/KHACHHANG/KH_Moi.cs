using NHOM.KHACHHANG;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;



namespace NHOM.User
{
    public partial class KH_Moi : Form
    {
       
        int mahd;
        string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=QL_Sach_DA;User ID=sa;Password='123'"; // Chuỗi kết nối cơ sở dữ liệu

        public KH_Moi(int mahd)
        {
            InitializeComponent();
           
            this.mahd = mahd;
        }
        int ma = -1;

        private void button1_Click(object sender, EventArgs e)
        {
            /*
           
                // Lấy thông tin từ UserControl "UC_CAPNHAT_TT"
                UC_CAPNHAT_TT userControl = panel1.Controls["UC_CAPNHAT_TT"] as UC_CAPNHAT_TT;

                if (userControl == null)
                {
                    userControl = new UC_CAPNHAT_TT();
                    userControl.Name = "UC_CAPNHAT_TT";
                    userControl.Dock = DockStyle.Fill;
                    panel1.Controls.Clear();  // Xóa tất cả các điều khiển hiện có
                    panel1.Controls.Add(userControl);  // Thêm UserControl vào panel
                }

                // Lấy dữ liệu từ UserControl
                string tenKhachHang = userControl.GetTenKhachHang();
                string gioiTinh = userControl.GetGioiTinh();
                string sdt = userControl.GetSdt();
                string diaChi = userControl.GetDiaChi();

                if (sdt.Length != 10 || !sdt.All(char.IsDigit))
                {
                    MessageBox.Show("Số điện thoại phải có 10 chữ số.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrEmpty(tenKhachHang) || string.IsNullOrEmpty(gioiTinh) || string.IsNullOrEmpty(sdt))
                {
                    MessageBox.Show("Vui lòng nhập thông tin khách hàng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra số điện thoại đã tồn tại trong CSDL chưa
                string checkQuery = "SELECT COUNT(*) FROM KhachHang WHERE SDT = @SDT";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@SDT", sdt);

                    try
                    {
                        conn.Open();
                        int count = (int)checkCmd.ExecuteScalar();  // Kiểm tra số điện thoại đã tồn tại

                        if (count > 0)
                        {
                            MessageBox.Show("Số điện thoại này đã tồn tại trong cơ sở dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Nếu số điện thoại chưa tồn tại, thêm dữ liệu vào CSDL
                        string insertQuery = @"
                INSERT INTO KhachHang (TenKH, GioiTinh, SDT, DiaChi)
                OUTPUT INSERTED.MaKH  -- Lấy mã khách hàng vừa thêm
                VALUES (@TenKhachHang, @GioiTinh, @SDT, @DiaChi)";
                        SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                        insertCmd.Parameters.AddWithValue("@TenKhachHang", tenKhachHang);
                        insertCmd.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                        insertCmd.Parameters.AddWithValue("@SDT", sdt);
                        insertCmd.Parameters.AddWithValue("@DiaChi", diaChi);

                        // Lấy mã khách hàng vừa được thêm
                        object result = insertCmd.ExecuteScalar();
                        if (result != null)
                        {
                        ma = Convert.ToInt32(result);
                        MessageBox.Show($"Thêm khách hàng thành công! Mã khách hàng: {ma}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK; // Đóng Form với trạng thái OK
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Không thể thêm khách hàng. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        userControl.ClearTextBox();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Có lỗi khi kết nối đến cơ sở dữ liệu : " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            
            */
          
                // Lấy thông tin từ UserControl
                UC_CAPNHAT_TT userControl = panel1.Controls["UC_CAPNHAT_TT"] as UC_CAPNHAT_TT;

                if (userControl == null)
                {
                    userControl = new UC_CAPNHAT_TT();
                    userControl.Name = "UC_CAPNHAT_TT";
                    userControl.Dock = DockStyle.Fill;
                    panel1.Controls.Clear();
                    panel1.Controls.Add(userControl);
                }

                // Lấy dữ liệu từ UserControl
                string tenKhachHang = userControl.GetTenKhachHang();
                string gioiTinh = userControl.GetGioiTinh();
                string sdt = userControl.GetSdt();
                string diaChi = userControl.GetDiaChi();

                if (!sdt.All(char.IsDigit))
                {
                    MessageBox.Show("Số điện thoại phải có 10 chữ số.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrEmpty(tenKhachHang) || string.IsNullOrEmpty(gioiTinh) || string.IsNullOrEmpty(sdt))
                {
                    MessageBox.Show("Vui lòng nhập thông tin khách hàng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kết nối cơ sở dữ liệu
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        // Kiểm tra số điện thoại đã tồn tại chưa
                        string checkQuery = "SELECT COUNT(*) FROM KhachHang WHERE SDT = @SDT";
                        using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                        {
                            checkCmd.Parameters.AddWithValue("@SDT", sdt);
                            int count = (int)checkCmd.ExecuteScalar();

                            if (count > 0)
                            {
                                MessageBox.Show("Số điện thoại này đã tồn tại trong cơ sở dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                        

                        // Thêm khách hàng mới vào bảng KhachHang
                        string insertQuery = @"
            INSERT INTO KhachHang (TenKH, GioiTinh, SDT, DiaChi)
            OUTPUT INSERTED.MaKH
            VALUES (@TenKhachHang, @GioiTinh, @SDT, @DiaChi)";
                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@TenKhachHang", tenKhachHang);
                            insertCmd.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                            insertCmd.Parameters.AddWithValue("@SDT", sdt);
                            insertCmd.Parameters.AddWithValue("@DiaChi", diaChi);

                            object result = insertCmd.ExecuteScalar();
                            if (result != null)
                            {
                                int ma = Convert.ToInt32(result);  // Mã khách hàng mới
                                MessageBox.Show($"Thêm khách hàng thành công! Mã khách hàng: {ma}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Cập nhật mã khách hàng vào bảng HOADONMA
                                UpdateMaKhachHangTrongHoaDon(ma);

                                // Đóng form nếu cần
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Không thể thêm khách hàng. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            private void UpdateMaKhachHangTrongHoaDon(int maKH)
            {
                string query = "UPDATE HOADON SET MaKH = @MaKH WHERE SOHD = @MaDH";  // Cập nhật vào bảng HOADONMA thay vì HOADON

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaKH", maKH);
                        cmd.Parameters.AddWithValue("@MaDH", mahd); // Dùng mã đơn hàng (mahd) để xác định hóa đơn cần cập nhật

                        try
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery(); // Thực thi câu lệnh
                            MessageBox.Show("Cập nhật mã khách hàng thành công vào bảng HOADONMA!");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Có lỗi khi cập nhật mã khách hàng vào bảng HOADONMA: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            






        }


        private void KH_Moi_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
