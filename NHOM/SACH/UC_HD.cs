using NHOM.KHACHHANG;
using NHOM.SACH;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NHOM.User
{
    public partial class UC_HD : UserControl
    {
        String ten;
        string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=QL_SACH_DA;User ID=sa;Password='123'";
        int mahd;



        public UC_HD(string ten, int mahd)
        {
            InitializeComponent();
            this.ten = ten;
            this.mahd = mahd;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private decimal TinhGG(int mahd)
        {
            decimal tongTien = TinhTT(mahd);
            decimal tienGiam = 0;
            decimal tongTienCuoi = tongTien;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Lấy mã giảm giá từ bảng HOADON
                    string queryMaGiam = "SELECT MaGiam FROM HOADON WHERE SoHD = @MaHD";
                    SqlCommand cmdMaGiam = new SqlCommand(queryMaGiam, conn);
                    cmdMaGiam.Parameters.AddWithValue("@MaHD", mahd);

                    object maGiamObj = cmdMaGiam.ExecuteScalar();
                    if (maGiamObj != DBNull.Value && maGiamObj != null)
                    {
                        string maGiam = maGiamObj.ToString();

                        // Lấy thông tin giảm giá từ bảng DISCOUNT
                        string queryDiscount = "SELECT PhanTram FROM DISCOUNT WHERE MaGiam = @MaGiam";
                        SqlCommand cmdDiscount = new SqlCommand(queryDiscount, conn);
                        cmdDiscount.Parameters.AddWithValue("@MaGiam", maGiam);

                        object phanTramObj = cmdDiscount.ExecuteScalar();
                        if (phanTramObj != DBNull.Value && phanTramObj != null)
                        {
                            decimal phanTram = Convert.ToDecimal(phanTramObj);
                            tienGiam = tongTien * (phanTram / 100);
                            tongTienCuoi = tongTien - tienGiam;

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi khi xử lý giảm giá: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Cập nhật tổng tiền cuối vào bảng HOADON
                string updateQuery = "UPDATE HOADON SET TongTien = @TongTien WHERE SoHD = @MaHD";
                SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                updateCmd.Parameters.AddWithValue("@TongTien", tongTienCuoi);
                updateCmd.Parameters.AddWithValue("@MaHD", mahd);
                updateCmd.ExecuteNonQuery();
            }




            return tienGiam; // Trả về tổng tiền cuối sau khi giảm
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

        private void button3_Click(object sender, EventArgs e)
        {
             // Kiểm tra xem hóa đơn đã được tạo chưa
                if (mahd <= 0)
                {
                    MessageBox.Show("Hãy tạo hóa đơn trước khi thêm sản phẩm!");
                    return;
                }

                // Lấy dữ liệu từ giao diện
                string maSach = txtMaSach.Text.Trim();
                string tenSach = txtTenSach.Text.Trim();
                int soLuong = (int)numericUpDownSL.Value;

                // Kiểm tra thông tin hợp lệ
                if (string.IsNullOrEmpty(maSach) || string.IsNullOrEmpty(tenSach) || soLuong <= 0)
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin sản phẩm!");
                    return;
                }

                // Lấy giá tiền từ cơ sở dữ liệu
                decimal giaTien = LayGiaTheoMa(maSach);

                if (giaTien <= 0)
                {
                    MessageBox.Show("Không tìm thấy giá cho mã sách này!");
                    return;
                }

                // Kiểm tra số lượng tồn kho
                int soLuongTon = 0;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        // Truy vấn lấy số lượng tồn của sách
                        string querySoLuongTon = "SELECT SoLuongTon FROM SACH WHERE MaSach = @MaSach";
                        SqlCommand cmdSoLuongTon = new SqlCommand(querySoLuongTon, conn);
                        cmdSoLuongTon.Parameters.AddWithValue("@MaSach", maSach);

                        object result = cmdSoLuongTon.ExecuteScalar();
                        if (result != DBNull.Value && result != null)
                        {
                            soLuongTon = Convert.ToInt32(result);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy thông tin sách này!");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi lấy thông tin sách: " + ex.Message);
                        return;
                    }
                }

                // Kiểm tra nếu số lượng mua lớn hơn số lượng tồn
                if (soLuong > soLuongTon)
                {
                    MessageBox.Show("Số lượng mua không thể vượt quá số lượng tồn kho (" + soLuongTon + " còn lại).");
                    return;
                }

                // Tính thành tiền
                decimal thanhTien = soLuong * giaTien;

                // Thêm dữ liệu vào bảng CT_HoaDon và cập nhật số lượng tồn
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlTransaction transaction = conn.BeginTransaction();

                        try
                        {
                            // Thêm sản phẩm vào CT_HoaDon
                            string insertQuery = "INSERT INTO CT_HoaDon (MaHoaDon, MaSach, SoLuong, DonGia, ThanhTien) " +
                                                 "VALUES (@MaHoaDon, @MaSach, @SoLuong, @DonGia, @ThanhTien)";

                            using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn, transaction))
                            {
                                insertCmd.Parameters.AddWithValue("@MaHoaDon", mahd);
                                insertCmd.Parameters.AddWithValue("@MaSach", maSach);
                                insertCmd.Parameters.AddWithValue("@SoLuong", soLuong);
                                insertCmd.Parameters.AddWithValue("@DonGia", giaTien);
                                insertCmd.Parameters.AddWithValue("@ThanhTien", thanhTien);

                                insertCmd.ExecuteNonQuery();
                            }

                            // Cập nhật số lượng tồn kho trong bảng SACH
                            string updateQuery = "UPDATE SACH SET SoLuongTon = SoLuongTon - @SoLuong WHERE MaSach = @MaSach";

                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn, transaction))
                            {
                                updateCmd.Parameters.AddWithValue("@SoLuong", soLuong);
                                updateCmd.Parameters.AddWithValue("@MaSach", maSach);

                                int rowsAffected = updateCmd.ExecuteNonQuery();
                                if (rowsAffected <= 0)
                                {
                                    throw new Exception("Không tìm thấy sách hoặc số lượng tồn không hợp lệ.");
                                }
                            }

                            // Xác nhận giao dịch
                            transaction.Commit();


                        // Thêm sản phẩm vào ListView
                        ListViewItem item = new ListViewItem();

                        // Cột STT (được hiển thị từ `Text` của `ListViewItem`)
                      //  item.Checked = false; // Checkbox
                        item.Text = (listView1.Items.Count + 1).ToString(); // Thứ tự (STT)

                        // Thêm các cột tiếp theo (SubItems)
                        item.SubItems.Add(maSach);         // Cột Mã Sách
                        item.SubItems.Add(tenSach);        // Cột Tên Sách
                        item.SubItems.Add(soLuong.ToString()); // Cột Số Lượng
                        item.SubItems.Add(giaTien.ToString("N0")); // Cột Giá (định dạng số)

                      
                            // Thêm item vào ListView
                            listView1.Items.Add(item);

                            // Hiển thị thông báo thành công
                            MessageBox.Show("Thêm sản phẩm vào hóa đơn và cập nhật số lượng tồn thành công!");

                            // Xóa dữ liệu nhập cũ
                            txtMaSach.Clear();
                            txtTenSach.Clear();
                            numericUpDownSL.Value = 0;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback(); // Nếu lỗi, hoàn tác thay đổi
                            MessageBox.Show($"Lỗi khi thêm sản phẩm hoặc cập nhật dữ liệu: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thêm sản phẩm: {ex.Message}");
                }

                // Cập nhật các thông tin tổng tiền và giảm giá
                labelTT.Text = TinhTT(mahd).ToString();
            




        }

        // Hàm giả lập lấy giá tiền theo mã sách từ CSDL
        private decimal LayGiaTheoMa(string maSach)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT  DonGiaBan FROM Sach WHERE MaSach = @MaSach";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSach", maSach);
                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToDecimal(result) : 0;
                    }
                }
            }
            catch (Exception)
            {
                return 0; // Trả về 0 nếu xảy ra lỗi
            }

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (DOANHTHU uf = new DOANHTHU())
            {
                {
                    uf.ShowDialog();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            UC_Home home2 = new UC_Home(ten, mahd);
            this.Controls.Clear();
            home2.Dock = DockStyle.Fill;
            this.Controls.Add(home2);
        }







        // Hàm để lấy tên thể loại từ mã sách
        private string GetTheLoaiFromMaSach(string maSach)
        {
            string theLoai = string.Empty;

            string query = "SELECT t.TenTheLoai FROM THELOAI t " +
                           "JOIN SACH s ON t.MaTheLoai = s.MaTheLoai WHERE s.MaSach = @MaSach";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MaSach", maSach);

                try
                {
                    connection.Open();
                    theLoai = command.ExecuteScalar()?.ToString();  // Lấy tên thể loại từ cơ sở dữ liệu
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }

            return theLoai;



        }



        private void UC_Sale_Load(object sender, EventArgs e)
        {

            txtMaSach.Enabled = false;
            txtTheLoai.Enabled = false;
            txtTheLoai.Enabled = false;
            txtTenSach.Enabled = false;
            numericUpDownSL.Enabled = false;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void domainUpDownSLSach_SelectedItemChanged(object sender, EventArgs e)
        {

            // Lấy số lượng từ DomainUpDown
            int soLuong = Convert.ToInt32(numericUpDownSL.Value);

            // Cập nhật số lượng vào TextBox nếu cần (hoặc bạn có thể làm thêm các thao tác khác)
            numericUpDownSL.Text = soLuong.ToString();



        }


        private void UC_Sale_Load_1(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        //  mã sách 

        private void txtMaSach_TextChanged(object sender, EventArgs e)
        {


            string maSach = txtMaSach.Text.Trim(); // Lấy mã sách người dùng nhập

            if (string.IsNullOrEmpty(maSach))
            {
                // Nếu mã sách trống, xóa thông tin trong các textbox và ảnh
                txtTenSach.Text = string.Empty;
                txtTheLoai.Text = string.Empty;
                pictureBox.Image = null; // Xóa ảnh hiện tại
                return;
            }


            // Truy vấn lấy thông tin tên sách, thể loại và ảnh
            string query = @"SELECT S.TenSach, TL.TenTheLoai, S.Images
                     FROM SACH S
                     LEFT JOIN THELOAI TL ON S.MaTheLoai = TL.MaTheLoai
                     WHERE S.MaSach = @MaSach";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Thêm tham số @MaSach
                        command.Parameters.AddWithValue("@MaSach", maSach);

                        // Thực thi lệnh và đọc dữ liệu
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // Nếu tìm thấy sách
                            {
                                // Hiển thị tên sách và thể loại
                                txtTenSach.Text = reader["TenSach"].ToString();
                                txtTheLoai.Text = reader["TenTheLoai"].ToString();

                              

                            }
                            else
                            {
                                // Nếu không tìm thấy sách với mã đã nhập
                                txtTenSach.Text = string.Empty;
                                txtTheLoai.Text = string.Empty;
                                pictureBox.Image = null; // Xóa ảnh hiện tại
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Hiển thị lỗi nếu có
                MessageBox.Show($"Lỗi: {ex.Message}");
            }



        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {


            // Lấy thông tin người dùng hiện tại
            string username = ten;
            int maNguoiDung = GetMaNguoiDung(username);

            if (maNguoiDung == -1)
            {
                MessageBox.Show("Không tìm thấy người dùng trong hệ thống.");
                return;
            }

            DateTime ngayHoaDon = DateTime.Now;

            // Câu lệnh SQL để thêm hóa đơn và lấy mã hóa đơn mới
            string queryInsertHoaDon = @"
        INSERT INTO HOADON (NgayHD, MaNguoiNhap) 
        OUTPUT INSERTED.SoHD 
        VALUES (@NgayHD, @MaNguoiNhap)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(queryInsertHoaDon, connection))
                    {
                        command.Parameters.AddWithValue("@NgayHD", ngayHoaDon);
                        command.Parameters.AddWithValue("@MaNguoiNhap", maNguoiDung);

                        // Lấy mã hóa đơn mới được tạo
                        int maHoaDonMoi = (int)command.ExecuteScalar();
                        mahd = maHoaDonMoi; // Lưu lại mã hóa đơn vào biến toàn cục
                        MessageBox.Show($"Hóa đơn đã được tạo thành công với mã: {maHoaDonMoi}");

                        // Cập nhật các control nếu cần
                        txtMaSach.Clear();
                        txtTenSach.Clear();
                        numericUpDownSL.Value = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo hóa đơn: {ex.Message}");
            }




        }

        private int GetMaNguoiDung(string username)
        {
            // Lấy mã người dùng từ bảng NGUOIDUNG dựa trên tên đăng nhập
            string query = "SELECT MaNguoiDung FROM NGUOIDUNG WHERE UserName = @UserName";
            int maNguoiDung = -1;  // Mặc định là -1 nếu không tìm thấy người dùng

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", username);
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        maNguoiDung = Convert.ToInt32(result); // Trả về mã người dùng
                    }
                }
            }

            return maNguoiDung;
        }

        private string GetLatestMaHoaDon()
        {
            // Lấy mã hóa đơn mới nhất từ bảng HOADON
            string query = "SELECT MAX(SoHD) FROM HOADON";  // Lấy SoHD mới nhất từ bảng HOADON
            string maHoaDon = string.Empty;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        maHoaDon = result.ToString();
                    }
                }
            }

            return maHoaDon;
        }

        private void UpdateMaKhachHangTrongHoaDon(int maKH)
        {
            string query = "UPDATE HOADON SET MaKH = @MaKH WHERE SoHD = @SoHD";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaKH", maKH);
                    cmd.Parameters.AddWithValue("@SoHD", mahd); // Dùng mahd để xác định hóa đơn cần cập nhật

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery(); // Thực thi câu lệnh
                        MessageBox.Show("Cập nhật mã khách hàng thành công!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Có lỗi khi cập nhật hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        // khách hàng mới 
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (radioButton1.Checked)
            {
                // Hiển thị form KH_Moi và truyền mã hóa đơn (mahd)
                using (KH_Moi formKH = new KH_Moi(mahd))
                {
                    if (formKH.ShowDialog() == DialogResult.OK)
                    {
                        // Không cần làm gì thêm, vì formKH đã cập nhật mã khách hàng và mã hóa đơn trong cơ sở dữ liệu
                        MessageBox.Show("Cập nhật khách hàng và hóa đơn thành công!");
                    }
                }

                // Lấy tên khách hàng từ mã hóa đơn (mahd)
                string tenKH = GetTenKHFromMaHD(mahd);


            }
            labelTenKH.Text = LayTenKhachHang(mahd);
            labelGG.Text = TinhGG(mahd).ToString();
            labelThanhTien.Text = LayTongTien(mahd).ToString();

        }

        private string LayTenKhachHang(int soHD)
        {
            string tenKhachHang = string.Empty;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Truy vấn trực tiếp để lấy TenKH bằng JOIN
                    string query = @"
                SELECT KH.TenKH 
                FROM HOADON HD
                JOIN KHACHHANG KH ON HD.MaKH = KH.MaKH
                WHERE HD.SoHD = @SoHD";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SoHD", soHD);

                        object tenKHObj = cmd.ExecuteScalar();

                        if (tenKHObj != null && tenKHObj != DBNull.Value)
                        {
                            tenKhachHang = tenKHObj.ToString();
                        }
                        else
                        {
                            tenKhachHang = "Không tìm thấy tên khách hàng";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi khi lấy tên khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return tenKhachHang;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                // Hiển thị form KH_Moi và truyền mã hóa đơn (mahd)
                using (KH_TT formKHTT = new KH_TT(mahd))
                {
                    if (formKHTT.ShowDialog() == DialogResult.OK)
                    {
                        // Không cần làm gì thêm, vì formKH đã cập nhật mã khách hàng và mã hóa đơn trong cơ sở dữ liệu
                        MessageBox.Show("Cập nhật khách hàng và hóa đơn thành công!");
                    }
                }

                // Tự động bỏ chọn CheckBox sau khi hoàn thành
                radioButton2.Checked = false;
            }
            labelTenKH.Text = LayTenKhachHang(mahd);
            labelGG.Text = TinhGG(mahd).ToString();
            labelThanhTien.Text = LayTongTien(mahd).ToString();
        }

        private string GetTenKHFromMaHD(int mahd)
        {

            string tenKH = string.Empty;

            // Câu truy vấn kết hợp để lấy TenKH từ MaHD
            string query = @"
        SELECT KH.TenKH
        FROM HOADON HD
        JOIN KHACHHANG KH ON HD.MAKH = KH.MAKH
        WHERE HD.SoHD = @MaDH";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaDH", mahd);

                try
                {
                    conn.Open();
                    // Thực thi truy vấn và lấy tên khách hàng
                    object result = cmd.ExecuteScalar();

                    if (result != DBNull.Value && result != null)
                    {
                        tenKH = result.ToString(); // Lấy tên khách hàng
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi khi lấy tên khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return tenKH;


        }

        private decimal TinhTT(int mahd)
        {
            // Câu truy vấn SQL để lấy tổng tiền từ bảng CT_HD
            string query = "SELECT SUM(SoLuong * DonGia) AS TongTien FROM CT_HoaDon WHERE MaHoaDon = @MaDH";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaDH", mahd);

                try
                {
                    conn.Open();
                    // Thực thi truy vấn và lấy kết quả
                    object result = cmd.ExecuteScalar();

                    if (result != DBNull.Value)
                    {
                        return Convert.ToDecimal(result); // Trả về tổng tiền dưới dạng decimal
                    }
                    else
                    {
                        return 0; // Nếu không có dữ liệu, trả về 0
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi khi tính tổng tiền: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                }
            }
        }



        private void UpdatePaymentMethod(int pttt)
        {
            // Chuỗi kết nối với cơ sở dũ liệu 
            string query = "UPDATE CT_HOADON SET Pttt = @Pttt WHERE MaHoaDon = @SoHD";  // Cập nhật Pttt cho mã hóa đơn cụ thể

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Mở kết nối đến cơ sở dữ liệu
                connection.Open();

                // Tạo đối tượng SqlCommand để thực thi câu lệnh SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Thêm tham số vào câu lệnh SQL
                    command.Parameters.AddWithValue("@Pttt", pttt);
                    command.Parameters.AddWithValue("@SoHD", mahd);  // `mahd` là mã hóa đơn cần cập nhật

                    // Thực thi câu lệnh SQL
                    command.ExecuteNonQuery();
                }
            }
        }


        private void UC_Sale_Load_2(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;

           // Đặt chế độ hiển thị chi tiết
            listView1.View = View.Details;

            // Xóa các cột cũ nếu có
            listView1.Columns.Clear();

            // Thêm các cột theo đúng thứ tự
            listView1.Columns.Add("STT", 50, HorizontalAlignment.Center);
            listView1.Columns.Add("Mã", 70, HorizontalAlignment.Left);
            listView1.Columns.Add("Tên Sách", 150, HorizontalAlignment.Left);
            listView1.Columns.Add("SL", 50, HorizontalAlignment.Center);
            listView1.Columns.Add("Giá", 100, HorizontalAlignment.Right);

            // Bật checkbox để chọn hàng
          listView1.CheckBoxes = true;
        }

        private void labelTenKH_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged_2(object sender, EventArgs e)
        {

        }

        private void labelGG_Click(object sender, EventArgs e)
        {



        }

        // xoá hết 
        private void btn_Add_toCart_Click(object sender, EventArgs e)
        {

                // Kiểm tra nếu mã hóa đơn không hợp lệ
                if (mahd <= 0)
                {
                    MessageBox.Show("Mã hóa đơn không hợp lệ!");
                    return;
                }

                // Kết nối cơ sở dữ liệu
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        SqlTransaction transaction = conn.BeginTransaction();

                        try
                        {
                            // Cập nhật số lượng tồn trong bảng SACH (thêm số lượng vào)
                            string updateSachQuery = @"
                UPDATE SACH
                SET SoLuongTon = SoLuongTon + (SELECT SoLuong FROM CT_HoaDon WHERE MaHoaDon = @MaHD AND SACH.MaSach = CT_HoaDon.MaSach)
                WHERE MaSach IN (SELECT MaSach FROM CT_HoaDon WHERE MaHoaDon = @MaHD)";

                            using (SqlCommand cmdUpdateSach = new SqlCommand(updateSachQuery, conn, transaction))
                            {
                                cmdUpdateSach.Parameters.AddWithValue("@MaHD", mahd);
                                cmdUpdateSach.ExecuteNonQuery();
                            }

                            // Xóa dữ liệu trong bảng CT_HoaDon
                            string deleteCTHDQuery = "DELETE FROM CT_HoaDon WHERE MaHoaDon = @MaHD";
                            using (SqlCommand cmdDeleteCTHD = new SqlCommand(deleteCTHDQuery, conn, transaction))
                            {
                                cmdDeleteCTHD.Parameters.AddWithValue("@MaHD", mahd);
                                cmdDeleteCTHD.ExecuteNonQuery();
                            }

                            // Xóa dữ liệu trong bảng HOADON
                            string deleteHoaDonQuery = "DELETE FROM HOADON WHERE SoHD = @MaHD";
                            using (SqlCommand cmdDeleteHoaDon = new SqlCommand(deleteHoaDonQuery, conn, transaction))
                            {
                                cmdDeleteHoaDon.Parameters.AddWithValue("@MaHD", mahd);
                                cmdDeleteHoaDon.ExecuteNonQuery();
                            }

                            // Commit giao dịch sau khi thực hiện các thay đổi thành công
                            transaction.Commit();

                            // Thông báo cho người dùng
                            MessageBox.Show("Đã xóa hóa đơn và cập nhật lại số lượng sách thành công!");

                            // Xóa toàn bộ dữ liệu trong ListView
                            listView1.Items.Clear();  // Sử dụng cách xóa sạch ListView
                        }
                        catch (Exception ex)
                        {
                            // Nếu có lỗi, hoàn tác giao dịch
                            transaction.Rollback();
                            MessageBox.Show($"Lỗi khi xóa hóa đơn hoặc cập nhật sách: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi kết nối cơ sở dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            


        }

        private void numericUpDownSL_ValueChanged(object sender, EventArgs e)
        {

        }

        private void panelLeft_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Thanh toán thanh công !");

          FormDHReport form = new FormDHReport();
           form.Show();

        }

        private void radioButtonTT_Onlien_CheckedChanged(object sender, EventArgs e)
        {
          
                // Cập nhật Pttt = 0 vào cơ sở dữ liệu
                UpdatePaymentMethod(0);  // 0 tương ứng với thanh toán offline
           

        }





        private void radioOnline_CheckedChanged(object sender, EventArgs e)
        {

           
                // Cập nhật Pttt = 0 vào cơ sở dữ liệu
                UpdatePaymentMethod(1);  // 0 tương ứng với thanh toán offline
            
            if (radioOnline.Checked)
            {
                Form1 qrForm = new Form1(mahd);
                qrForm.ShowDialog();  // Dùng ShowDialog thay vì Show
            }
        }

       
              
        // Hàm xóa sách khỏi cơ sở dữ liệu
        private void XoaSachKhoiCSDL(string maSach, int soLuongXoa)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        // 1. Cập nhật lại số lượng tồn kho trong bảng SACH
                        string updateSachQuery = "UPDATE SACH SET SoLuongTon = SoLuongTon + @SoLuongXoa WHERE MaSach = @MaSach";
                        using (SqlCommand cmdUpdateSach = new SqlCommand(updateSachQuery, conn, transaction))
                        {
                            cmdUpdateSach.Parameters.AddWithValue("@SoLuongXoa", soLuongXoa);
                            cmdUpdateSach.Parameters.AddWithValue("@MaSach", maSach);
                            cmdUpdateSach.ExecuteNonQuery();
                        }

                        // 2. Xóa sản phẩm khỏi bảng CT_HoaDon
                        string deleteCTHDQuery = "DELETE FROM CT_HoaDon WHERE MaSach = @MaSach";
                        using (SqlCommand cmdDeleteCTHD = new SqlCommand(deleteCTHDQuery, conn, transaction))
                        {
                            cmdDeleteCTHD.Parameters.AddWithValue("@MaSach", maSach);
                            cmdDeleteCTHD.ExecuteNonQuery();
                        }

                        // Commit giao dịch sau khi thực hiện các thay đổi thành công
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Nếu có lỗi, hoàn tác giao dịch
                        transaction.Rollback();
                        throw new Exception("Lỗi khi xóa sách hoặc cập nhật cơ sở dữ liệu: " + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi kết nối cơ sở dữ liệu: " + ex.Message);
                }
            }
        }

        private void LoadListView()
        {
            listView1.Items.Clear();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM CT_HoaDon"; // Thay đổi query phù hợp
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ListViewItem item = new ListViewItem(reader["MaSach"].ToString());
                        item.SubItems.Add(reader["TenSach"].ToString());
                        item.SubItems.Add(reader["SoLuong"].ToString());
                        item.SubItems.Add(reader["DonGia"].ToString());

                        listView1.Items.Add(item);
                    }
                }
            }
        }

    

        private void xoáToolStripMenuItem_Click(object sender, EventArgs e)
        {
               // Kiểm tra xem người dùng đã chọn dòng nào chưa
                if (listView1.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn một sản phẩm để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy mục đã chọn từ ListView
                ListViewItem selectedItem = listView1.SelectedItems[0];

                // Lấy thông tin từ các cột
                string maSach = selectedItem.SubItems[1].Text; // Mã sách (giả sử mã sách ở cột thứ 2)
                int soLuongXoa = int.Parse(selectedItem.SubItems[3].Text); // Số lượng (giả sử ở cột thứ 4)

                // Hiển thị hộp thoại xác nhận
                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa sách với mã {maSach} không?",
                                                      "Xác nhận xóa",
                                                      MessageBoxButtons.OKCancel,
                                                      MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    try
                    {
                        // Xóa sách khỏi cơ sở dữ liệu
                        XoaSachKhoiCSDL(maSach, soLuongXoa);

                        // Xóa mục khỏi ListView
                        listView1.Items.Remove(selectedItem);

                        // Thông báo thành công
                        MessageBox.Show("Đã xóa sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa sách: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            


        }

        private void sửaToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // Kiểm tra xem có mục nào được chọn không
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy dòng được chọn
            ListViewItem selectedItem = listView1.SelectedItems[0];

            // Đưa dữ liệu từ ListView vào các TextBox và NumericUpDown
            txtMaSach.Text = selectedItem.SubItems[1].Text;  // Mã sách
            txtTenSach.Text = selectedItem.SubItems[2].Text; // Tên sách
            numericUpDownSL.Value = int.Parse(selectedItem.SubItems[3].Text); // Số lượng

            // Thêm nút "Cập nhật" (hoặc xử lý theo cách khác nếu bạn muốn cập nhật)
            buttonCN.Enabled = true; // Kích hoạt nút cập nhật
        

        }

        private void buttonCN_Click(object sender, EventArgs e)
        {
       
            // Lấy dữ liệu đã sửa từ các TextBox và NumericUpDown
            string maSach = txtMaSach.Text.Trim();
            string tenSach = txtTenSach.Text.Trim();
            int soLuongMuaMoi = (int)numericUpDownSL.Value;

            // Kiểm tra thông tin hợp lệ
            if (string.IsNullOrEmpty(maSach) || string.IsNullOrEmpty(tenSach) || soLuongMuaMoi <= 0)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin sản phẩm!");
                return;
            }

            // Lấy số lượng cũ đã bán (cần lấy từ bảng CT_HoaDon)
            int soLuongCuaSanPham = 0;
            decimal giaTien = LayGiaTheoMa(maSach);

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Lấy số lượng cũ đã bán từ bảng CT_HoaDon
                    string querySoLuongCuaSanPham = "SELECT SoLuong FROM CT_HoaDon WHERE MaHoaDon = @MaHoaDon AND MaSach = @MaSach";
                    SqlCommand cmdSoLuong = new SqlCommand(querySoLuongCuaSanPham, conn);
                    cmdSoLuong.Parameters.AddWithValue("@MaHoaDon", mahd);
                    cmdSoLuong.Parameters.AddWithValue("@MaSach", maSach);

                    object result = cmdSoLuong.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        soLuongCuaSanPham = Convert.ToInt32(result);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy thông tin sản phẩm trong hóa đơn.");
                        return;
                    }

                    // Cập nhật thông tin vào bảng CT_HoaDon
                    string updateQuery = "UPDATE CT_HoaDon SET SoLuong = @SoLuong, ThanhTien = @ThanhTien WHERE MaHoaDon = @MaHoaDon AND MaSach = @MaSach";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@SoLuong", soLuongMuaMoi);
                    updateCmd.Parameters.AddWithValue("@ThanhTien", soLuongMuaMoi * giaTien); // Tính lại thành tiền
                    updateCmd.Parameters.AddWithValue("@MaHoaDon", mahd); // Mã hóa đơn hiện tại
                    updateCmd.Parameters.AddWithValue("@MaSach", maSach); // Mã sách

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                    {
                        MessageBox.Show("Không thể cập nhật dữ liệu vào CT_HoaDon.");
                        return;
                    }

                    // Lấy số lượng tồn kho hiện tại của sách
                    string querySoLuongTon = "SELECT SoLuongTon FROM SACH WHERE MaSach = @MaSach";
                    SqlCommand cmdSoLuongTon = new SqlCommand(querySoLuongTon, conn);
                    cmdSoLuongTon.Parameters.AddWithValue("@MaSach", maSach);

                    object resultSoLuongTon = cmdSoLuongTon.ExecuteScalar();

                    int soLuongTon = Convert.ToInt32(resultSoLuongTon);

                    // Kiểm tra nếu số lượng mua lớn hơn số lượng tồn
                    if (soLuongCuaSanPham > soLuongTon)
                    {
                        MessageBox.Show("Số lượng mua không thể vượt quá số lượng tồn kho (" + soLuongTon + " còn lại).");
                        return;
                    }

                    // Cập nhật lại số lượng tồn kho trong bảng SACH
                    // Tính lại số lượng tồn kho: (soLuongTon + soLuongCuaSanPham) - soLuongMuaMoi
                    string updateSoLuongTonQuery = "UPDATE SACH SET SoLuongTon = @SoLuongTon WHERE MaSach = @MaSach";
                    SqlCommand updateSoLuongTonCmd = new SqlCommand(updateSoLuongTonQuery, conn);
                    updateSoLuongTonCmd.Parameters.AddWithValue("@SoLuongTon", soLuongTon + soLuongCuaSanPham - soLuongMuaMoi);
                    updateSoLuongTonCmd.Parameters.AddWithValue("@MaSach", maSach);

                    int rowsAffected2 = updateSoLuongTonCmd.ExecuteNonQuery();
                    if (rowsAffected2 <= 0)
                    {
                        MessageBox.Show("Không thể cập nhật số lượng tồn kho.");
                        return;
                    }

                    // Hiển thị thông báo thành công
                    MessageBox.Show("Cập nhật thông tin sản phẩm và cơ sở dữ liệu thành công!");

                    // Cập nhật lại ListView
                    ListViewItem selectedItem = listView1.SelectedItems[0];
                    selectedItem.SubItems[1].Text = maSach;
                    selectedItem.SubItems[2].Text = tenSach;
                    selectedItem.SubItems[3].Text = soLuongMuaMoi.ToString();

                    // Xóa dữ liệu nhập cũ
                    txtMaSach.Clear();
                    txtTenSach.Clear();
                    numericUpDownSL.Value = 0;

                    // Tắt nút Cập nhật
                    buttonCN.Enabled = false;
                    // Cập nhật các thông tin tổng tiền và giảm giá
                    labelTT.Text = TinhTT(mahd).ToString();


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật dữ liệu: {ex.Message}");
            }
        



    }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form form = new Form();
            UC_HD_CT ucForm = new UC_HD_CT(ten, mahd);

            // Thêm UserControl vào Form
            form.Controls.Add(ucForm);

            // Đặt kích thước Form phù hợp
            form.Size = ucForm.Size;
            ucForm.Dock = DockStyle.Fill;

            // Hiển thị Form
            form.ShowDialog(); // Hoặc dùng form.Show() nếu không cần chờ
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            UC_Home home2 = new UC_Home(ten, mahd);

            // Xóa tất cả các control trên form chính
            this.Controls.Clear();

            // Thêm UserControl mới
            home2.Dock = DockStyle.Fill;
            this.Controls.Add(home2);
        }
    }
}





