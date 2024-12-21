using NHOM.User;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;
using Excel = Microsoft.Office.Interop.Excel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace NHOM.SACH
{
    public partial class CT_1PN : Form
    {
        private readonly string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=QL_SACH_DA;User ID=sa;Password='123'";
        private readonly string tendangnhap;
        string maNhap;

        public CT_1PN(string tendangnhap, string maNhap)
        {
            InitializeComponent();
            this.tendangnhap = tendangnhap;
            this.maNhap = maNhap;
        }

        private void CT_1PN_Load(object sender, EventArgs e)
        {

            LoadGridViewNHAP(maNhap);
            labelMA_PNHAPSACH.Text = "Mã phiếu nhập: " + maNhap.ToString();
            dataGridViewBooksDetail.ReadOnly = true;
        }

        // Biến toàn cục lưu trữ DataTable
        private DataTable dataTable;

        private void LoadGridViewNHAP(string maNS)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                  SELECT 
     SACH.MaSach as 'Mã sách',
     TenSach as 'Tên sách',
     SoLuongNhap as 'Số lượng',
     DonGiaNhap as 'Đơn giá nhập',
     NhaXuatBan as 'Nhà xuất bản',
     NamXuatBan  as 'Năm xuất bản'
 FROM 
     CT_NHAPSACH
 JOIN SACH ON CT_NHAPSACH.MaSach = SACH.MaSach
 WHERE 
     MaNS = @maNS";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maNS", maNS);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    dataTable = new DataTable(); // Tạo một DataTable mới
                    da.Fill(dataTable);

                    dataGridViewBooksDetail.DataSource = dataTable; // Gán DataTable cho DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dataGridViewBooksDetail.Rows.Count > 0)
            {
                // Cho phép chỉnh sửa
                dataGridViewBooksDetail.ReadOnly = false;
                MessageBox.Show("Bạn có thể chỉnh sửa thông tin trực tiếp trên bảng. Nhấn nút 'Lưu' để lưu thay đổi.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để chỉnh sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLUU_SUA_Click(object sender, EventArgs e)
        {

            dataGridViewBooksDetail.ReadOnly = false;

            try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        // Bắt đầu giao dịch
                        using (SqlTransaction transaction = conn.BeginTransaction())
                        {
                            try
                            {
                                foreach (DataGridViewRow row in dataGridViewBooksDetail.Rows)
                                {
                                    // Bỏ qua các dòng mới hoặc dòng chưa chỉnh sửa
                                    if (row.IsNewRow) continue;

                                    // Lấy dữ liệu từ từng dòng (Lấy giá trị mới nhất từ DataGridView)
                                    string maSach = row.Cells["Mã sách"].Value?.ToString();
                                    string tenSach = row.Cells["Tên sách"].Value?.ToString();
                                    int soLuongNhap = int.TryParse(row.Cells["Số lượng"].Value?.ToString(), out int tempSL) ? tempSL : 0;
                                    double donGiaNhap = double.TryParse(row.Cells["Đơn giá nhập"].Value?.ToString(), out double tempGia) ? tempGia : 0;
                                    string nhaXuatBan = row.Cells["Nhà xuất bản"].Value?.ToString();
                                    int namXuatBan = int.TryParse(row.Cells["Năm xuất bản"].Value?.ToString(), out int tempNamXB) ? tempNamXB : 0;

                                  
                                    // Kiểm tra mã sách
                                    if (string.IsNullOrWhiteSpace(maSach))
                                    {
                                        MessageBox.Show("Mã sách không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        continue;
                                    }

                                    // Cập nhật thông tin trong bảng SACH
                                    string updateSachQuery = @"
                            UPDATE SACH
                            SET TenSach = @TenSach, 
                                NamXuatBan = @NamXB, 
                                NhaXuatBan = @NhaXuatBan,
                                SoLuongTon = @SoLuongTon
                            WHERE MaSach = @MaSach";

                                    using (SqlCommand cmd = new SqlCommand(updateSachQuery, conn, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@MaSach", maSach);
                                        cmd.Parameters.AddWithValue("@TenSach", tenSach);
                                        cmd.Parameters.AddWithValue("@NamXB", namXuatBan);
                                        cmd.Parameters.AddWithValue("@SoLuongTon", soLuongNhap);
                                        cmd.Parameters.AddWithValue("@NhaXuatBan", nhaXuatBan);
                                        cmd.ExecuteNonQuery();
                                    }

                                    // Cập nhật thông tin trong bảng CT_NHAPSACH
                                    string updateCTNhapQuery = @"
                            UPDATE CT_NHAPSACH
                            SET SoLuongNhap = @SoLuong, 
                                DonGiaNhap = @DonGiaNhap
                            WHERE MaSach = @MaSach";

                                    using (SqlCommand cmd = new SqlCommand(updateCTNhapQuery, conn, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@MaSach", maSach);
                                        cmd.Parameters.AddWithValue("@SoLuong", soLuongNhap);
                                        cmd.Parameters.AddWithValue("@DonGiaNhap", donGiaNhap);
                                        cmd.ExecuteNonQuery();
                                    }
                                }

                                // Xác nhận giao dịch
                                transaction.Commit();
                                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Khóa DataGridView sau khi lưu
                                dataGridViewBooksDetail.ReadOnly = true;

                                // Cập nhật lại dữ liệu hiển thị sau khi lưu thành công
                                LoadGridViewNHAP(maNhap); // Đây là hàm bạn cần gọi để load lại dữ liệu vào DataGridView
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                MessageBox.Show("Lỗi khi lưu thay đổi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
          
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        // Bắt đầu giao dịch
                        using (SqlTransaction transaction = conn.BeginTransaction())
                        {
                            try
                            {
                                foreach (DataGridViewRow row in dataGridViewBooksDetail.Rows)
                                {
                                    // Bỏ qua các dòng mới hoặc dòng chưa chỉnh sửa
                                    if (row.IsNewRow) continue;

                                    // Lấy dữ liệu từ từng dòng
                                    string maSach = row.Cells["Mã sách"].Value?.ToString();
                                    string tenSach = row.Cells["Tên sách"].Value?.ToString();
                                    int soLuongNhap = int.TryParse(row.Cells["Số lượng"].Value?.ToString(), out int tempSL) ? tempSL : 0;
                                    double donGiaNhap = double.TryParse(row.Cells["Đơn giá nhập"].Value?.ToString(), out double tempGia) ? tempGia : 0;
                                    string nhaXuatBan = row.Cells["Nhà xuất bản"].Value?.ToString();
                                    int namXuatBan = int.TryParse(row.Cells["Năm xuất bản"].Value?.ToString(), out int tempNamXB) ? tempNamXB : 0;

                                    // Kiểm tra mã sách
                                    if (string.IsNullOrWhiteSpace(maSach))
                                    {
                                        MessageBox.Show("Mã sách không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        continue;
                                    }

                                    // Cập nhật thông tin trong bảng SACH
                                    string updateSachQuery = @"
                            UPDATE SACH
                            SET TenSach = @TenSach, 
                                NamXuatBan = @NamXB, 
                                NhaXuatBan = @NhaXuatBan,
                                SoLuongTon = @SoLuongTon
                            WHERE MaSach = @MaSach";
                                    using (SqlCommand cmd = new SqlCommand(updateSachQuery, conn, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@MaSach", maSach);
                                        cmd.Parameters.AddWithValue("@TenSach", tenSach);
                                        cmd.Parameters.AddWithValue("@NamXB", namXuatBan);
                                        cmd.Parameters.AddWithValue("@SoLuongTon", soLuongNhap);
                                        cmd.Parameters.AddWithValue("@NhaXuatBan", nhaXuatBan);
                                        cmd.ExecuteNonQuery();
                                    }

                                    // Cập nhật thông tin trong bảng CT_NHAPSACH
                                    string updateCTNhapQuery = @"
                            UPDATE CT_NHAPSACH
                            SET SoLuongNhap = @SoLuong, 
                                DonGiaNhap = @DonGiaNhap
                            WHERE MaSach = @MaSach";
                                    using (SqlCommand cmd = new SqlCommand(updateCTNhapQuery, conn, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@MaSach", maSach);
                                        cmd.Parameters.AddWithValue("@SoLuong", soLuongNhap);
                                        cmd.Parameters.AddWithValue("@DonGiaNhap", donGiaNhap);
                                        cmd.ExecuteNonQuery();
                                    }

                                    // Cập nhật lại thành tiền trong bảng CT_NHAPSACH
                                    double thanhTien = soLuongNhap * donGiaNhap;
                                    string updateThanhTienQuery = @"
                            UPDATE CT_NHAPSACH
                            SET ThanhTien = @ThanhTien
                            WHERE MaSach = @MaSach";
                                    using (SqlCommand cmd = new SqlCommand(updateThanhTienQuery, conn, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@MaSach", maSach);
                                        cmd.Parameters.AddWithValue("@ThanhTien", thanhTien);
                                        cmd.ExecuteNonQuery();
                                    }
                                }

                                // Cập nhật lại tổng tiền trong bảng PHIEUNHAP
                                string updateTongTienQuery = @"
                        UPDATE PHIEUNHAP
                        SET TongTien = (
                            SELECT SUM(ThanhTien) 
                            FROM CT_NHAPSACH 
                            WHERE MaNS = @MaNS)
                        WHERE MaPN = @MaPN";
                                using (SqlCommand cmd = new SqlCommand(updateTongTienQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@MaNS", maNhap);  // Mã phiếu nhập
                                    cmd.Parameters.AddWithValue("@MaPN", maNhap);  // Mã phiếu nhập (cùng mã với MaNS)
                                    cmd.ExecuteNonQuery();
                                }

                                // Xác nhận giao dịch
                                transaction.Commit();
                                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Khóa DataGridView sau khi lưu
                                dataGridViewBooksDetail.ReadOnly = true;

                                // Cập nhật lại dữ liệu hiển thị
                                LoadGridViewNHAP(maNhap);
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                MessageBox.Show("Lỗi khi lưu thay đổi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
           


        }
        // lấy tên dăng nhập 
        private string LayHoTenTheoUsername(string tendangnhap)
        {
            string hoTen = string.Empty; // Biến để lưu trữ HoTen

            try
            {

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Mở kết nối
                    conn.Open();

                    // Câu lệnh SQL để lấy HoTen từ Username
                    string query = "SELECT HoTen FROM NguoiDung WHERE Username = @Username";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Thêm parameter vào câu lệnh SQL
                        cmd.Parameters.AddWithValue("@Username", tendangnhap);

                        // Thực thi câu lệnh và lấy kết quả
                        var result = cmd.ExecuteScalar(); // Dùng ExecuteScalar để lấy giá trị duy nhất

                        if (result != null)
                        {
                            hoTen = result.ToString(); // Gán HoTen nếu tìm thấy
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi truy vấn cơ sở dữ liệu: " + ex.Message);
            }

            return hoTen; // Trả về HoTen
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
       
            // Tạo một đối tượng ứng dụng Excel mới
            Excel.Application exApp = new Excel.Application();
            Excel.Workbook exBook = exApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            Excel.Worksheet exSheet = (Excel.Worksheet)exBook.Worksheets[1];

            // Thiết lập tiêu đề cho bảng tính
            Excel.Range shopName = (Excel.Range)exSheet.Cells[1, 1];
            shopName.Font.Size = 20;
            shopName.Font.Bold = true;
            shopName.Value = "CỬA HÀNG SÁCH";

            Excel.Range shopAddress = (Excel.Range)exSheet.Cells[2, 1];
            shopAddress.Font.Size = 14;
            shopAddress.Font.Bold = true;
            shopAddress.Value = "Địa chỉ: 140 Lê Trọng Tấn, Tân Phú, Tp.Hồ Chí Minh";

            // Thông tin người tạo và ngày tháng
            exSheet.Range["A6:B7"].Font.Size = 13;
            exSheet.get_Range("A6:B7").Font.Bold = true;
            exSheet.get_Range("A6").Value = "Người tạo:";
            exSheet.get_Range("B6").Value = LayHoTenTheoUsername(tendangnhap);
            exSheet.get_Range("A7").Value = "Ngày:";
            exSheet.get_Range("B7").Value = DateTime.Now.ToString("dd-MM-yyyy");

            // In tiêu đề cho bảng dữ liệu
            exSheet.get_Range("A10:L10").Font.Size = 13;
            exSheet.get_Range("A10:L10").Font.Bold = true;
            exSheet.get_Range("A10:L10").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            exSheet.get_Range("A10").Value = "STT";
            exSheet.get_Range("B10").Value = "Mã sách";
            exSheet.get_Range("C10").Value = "Tên sách";
            exSheet.get_Range("D10").Value = "Số lượng";
            exSheet.get_Range("E10").Value = "Đơn giá nhập";
            exSheet.get_Range("F10").Value = "Nhà xuất bản";
            exSheet.get_Range("G10").Value = "Năm xuất bản";

            // Thiết lập màu nền cho tiêu đề bảng
            exSheet.get_Range("A10:L10").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);

            // In dữ liệu từ DataGridView
            int row = 11; // Bắt đầu từ dòng 11
            foreach (DataGridViewRow gridRow in dataGridViewBooksDetail.Rows)
            {
                // Bỏ qua dòng trống
                if (gridRow.IsNewRow) continue;

                exSheet.Range["A" + row].Value = row - 10; // STT
                exSheet.Range["B" + row].Value = gridRow.Cells["Mã sách"].Value?.ToString();
                exSheet.Range["C" + row].Value = gridRow.Cells["Tên sách"].Value?.ToString();
                exSheet.Range["D" + row].Value = gridRow.Cells["Số lượng"].Value?.ToString();
                exSheet.Range["E" + row].Value = gridRow.Cells["Đơn giá nhập"].Value?.ToString();
                exSheet.Range["F" + row].Value = gridRow.Cells["Nhà xuất bản"].Value?.ToString();
                exSheet.Range["G" + row].Value = gridRow.Cells["Năm xuất bản"].Value?.ToString();

                row++; // Tăng dòng lên 1 để ghi vào dòng tiếp theo
            }

            // Đặt tên cho bảng tính và kích hoạt workbook
            exSheet.Name = "DSSP";
            exBook.Activate();

            // Lưu file Excel
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Excel WorkBook|*.xlsx|All Files|*.*";
            saveDialog.FilterIndex = 1;
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                exBook.SaveAs(saveDialog.FileName.ToLower());
            }

            // Đóng ứng dụng Excel
            exApp.Quit();
        }


    

}
}
