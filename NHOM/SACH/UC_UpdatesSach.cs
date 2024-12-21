using NHOM.User;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NHOM.SACH
{
    public partial class UC_UpdatesSach : UserControl
    {
        string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=QL_SACH_DA;User ID=sa;Password='123'";
        string ten;
        int mahd;
        public UC_UpdatesSach(string ten,int mahd)
        {
            InitializeComponent();
            this.ten = ten;
            this.mahd = mahd;
        }   

        private void btnSua_Click(object sender, EventArgs e)
        {
            dataGridViewBooksDetail.Enabled = true;
            btnLuu.Enabled = true;
            var result = MessageBox.Show("Bạn có muốn sửa thông tin sách không?", "Xác nhận sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Cho phép chỉnh sửa DataGridView
                dataGridViewBooksDetail.ReadOnly = false;

                // Kích hoạt nút lưu để cập nhật cơ sở dữ liệu
                btnLuu.Enabled = true;
                btnSua.Enabled = false;  // Disable nút sửa trong lúc chỉnh sửa
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu có thay đổi
            bool isChanged = false;

            // Duyệt qua các dòng trong DataGridView và cập nhật thông tin
            foreach (DataGridViewRow row in dataGridViewBooksDetail.Rows)
            {
                // Bỏ qua các dòng mới hoặc dòng chưa chỉnh sửa
                if (row.IsNewRow) continue;
                MessageBox.Show("row" + row);
                // Lấy dữ liệu từ từng dòng (Lấy giá trị mới nhất từ DataGridView)
                string maSach = row.Cells["Mã sách"].Value?.ToString();
                string tenSach = row.Cells["Tên sách"].Value?.ToString();
                int soLuongTon = int.TryParse(row.Cells["Số lượng tồn"].Value?.ToString(), out int tempSL) ? tempSL : 0;
                string nhaXuatBan = row.Cells["Nhà xuất bản"].Value?.ToString();
                int namXuatBan = int.TryParse(row.Cells["Năm xuất bản"].Value?.ToString(), out int tempNamXB) ? tempNamXB : 0;

                // Kiểm tra mã sách
                if (string.IsNullOrWhiteSpace(maSach))
                {
                    MessageBox.Show("Mã sách không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }

                // Nếu có thay đổi, thực hiện cập nhật
                if (!string.IsNullOrWhiteSpace(tenSach) || soLuongTon > 0 || !string.IsNullOrWhiteSpace(nhaXuatBan) || namXuatBan > 0)
                {
                    isChanged = true;
                    // Cập nhật thông tin trong bảng SACH
                    string updateSachQuery = @"
                        UPDATE SACH
                        SET TenSach = @TenSach, 
                            NamXuatBan = @NamXB, 
                            NhaXuatBan = @NhaXuatBan,
                            SoLuongTon = @SoLuongTon
                        WHERE MaSach = @MaSach";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        try
                        {
                            conn.Open();
                            using (SqlCommand cmd = new SqlCommand(updateSachQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("@MaSach", maSach);
                                cmd.Parameters.AddWithValue("@TenSach", tenSach);
                                cmd.Parameters.AddWithValue("@NamXB", namXuatBan);
                                cmd.Parameters.AddWithValue("@SoLuongTon", soLuongTon);
                                cmd.Parameters.AddWithValue("@NhaXuatBan", nhaXuatBan);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi khi cập nhật thông tin sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }

            // Nếu có thay đổi, xác nhận và khóa lại DataGridView
            if (isChanged)
            {
                MessageBox.Show("Cập nhật thông tin sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridViewBooksDetail.ReadOnly = true;
                btnLuu.Enabled = false;
                btnSua.Enabled = true;

                // Load lại dữ liệu vào DataGridView sau khi lưu thành công
                LoadData();
            }
            else
            {
                MessageBox.Show("Không có thay đổi nào được thực hiện.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UC_UpdatesSach_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            dataGridViewBooksDetail.Enabled = false;
            btnLuu.Enabled = false;
            string query = "SELECT MaSach AS 'Mã sách', " +
                           "MaTheLoai AS 'Mã Thể Loại', " +
                           "TenSach AS 'Tên sách', " +
                           "NhaXuatBan AS 'Nhà Xuất Bản', " +
                           "NamXuatBan AS 'Năm Xuất Bản', " +
                           "SoLuongTon AS 'Số Lượng Tồn', " +
                           "DonGiaBan AS 'Đơn Giá'" +
                           " FROM SACH";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();

                try
                {
                    // Lấy dữ liệu từ cơ sở dữ liệu và điền vào DataTable
                    dataAdapter.Fill(dataTable);

                    // Gán DataTable vào DataGridView
                    dataGridViewBooksDetail.DataSource = dataTable;

                    dataGridViewBooksDetail.Columns["Mã sách"].MinimumWidth = 50;
                    // Tự động điều chỉnh kích thước cột sao cho vừa với nội dung
                    dataGridViewBooksDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


                    // Làm cho cột "Mã sách" không thể chỉnh sửa
                    dataGridViewBooksDetail.ReadOnly = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi danh sách sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
           
                // Kiểm tra nếu có dòng được chọn trong DataGridView
                if (dataGridViewBooksDetail.SelectedRows.Count > 0)
                {
                    // Lấy mã sách của dòng đang được chọn
                    string maSach = dataGridViewBooksDetail.SelectedRows[0].Cells["Mã sách"].Value.ToString();

                    // Hiển thị thông báo xác nhận khi xóa
                    var result = MessageBox.Show("Bạn có muốn xóa sách này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            // Cập nhật cơ sở dữ liệu để xóa sách
                            using (SqlConnection conn = new SqlConnection(connectionString))
                            {
                                conn.Open();

                                // Bắt đầu giao dịch
                                using (SqlTransaction transaction = conn.BeginTransaction())
                                {
                                    try
                                    {
                                        // Xóa thông tin sách trong bảng SACH
                                        string deleteSachQuery = @"
                                DELETE FROM SACH
                                WHERE MaSach = @MaSach";

                                        using (SqlCommand cmd = new SqlCommand(deleteSachQuery, conn, transaction))
                                        {
                                            cmd.Parameters.AddWithValue("@MaSach", maSach);
                                            cmd.ExecuteNonQuery();
                                        }

                                        // Xóa thông tin sách trong bảng CT_NHAPSACH (nếu có)
                                        string deleteCTNhapQuery = @"
                                DELETE FROM CT_NHAPSACH
                                WHERE MaSach = @MaSach";

                                        using (SqlCommand cmd = new SqlCommand(deleteCTNhapQuery, conn, transaction))
                                        {
                                            cmd.Parameters.AddWithValue("@MaSach", maSach);
                                            cmd.ExecuteNonQuery();
                                        }

                                        // Cam kết giao dịch
                                        transaction.Commit();

                                        // Xóa dòng khỏi DataGridView
                                        dataGridViewBooksDetail.Rows.Remove(dataGridViewBooksDetail.SelectedRows[0]);

                                        MessageBox.Show("Xóa sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    catch (Exception ex)
                                    {
                                        // Nếu có lỗi, hoàn lại giao dịch
                                        transaction.Rollback();
                                        MessageBox.Show("Lỗi khi xóa sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một dòng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            

        }

        private void button2_Click(object sender, EventArgs e)
        {

            UC_Home home2 = new UC_Home(ten,mahd);

            // Xóa tất cả các control trên form chính
            this.Controls.Clear();

            // Thêm UserControl mới
            home2.Dock = DockStyle.Fill;
            this.Controls.Add(home2);

        }

        private void dataGridViewBooksDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
