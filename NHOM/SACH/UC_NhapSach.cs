using NHOM.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NHOM.SACH
{
    public partial class UC_NhapSach : UserControl
    {
        string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=QL_SACH_DA;User ID=sa;Password='123'";

        private readonly string tendangnhap;
        private int mahd;
        public UC_NhapSach(string tendangnhap,int mahd)
        {
            InitializeComponent();
            this.tendangnhap = tendangnhap;
            this.mahd = mahd;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
        private void LoadMaNSToComboBox()
        {
            string query = "SELECT MaNS FROM NHAPSACH"; // Truy vấn lấy tất cả MaNS hiện có

            cbxMaNhapSach.Items.Clear(); // Xóa các item cũ trong comboBox3

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Thêm MaNS vào comboBox3
                                cbxMaNhapSach.Items.Add(reader["MaNS"].ToString());
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


        private void UC_NhapSach_Load(object sender, EventArgs e)
        {
            LoadMaNSToComboBox();
            LoadGridView();
        }


        private void LoadGridView()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Query 1: Tổng số lượng nhập theo MaNS
                    string query1 = @"
                SELECT 
                    MaNS, 
                    SUM(SoLuongNhap) AS [Tổng Số Lượng Nhập]
                FROM 
                    CT_NHAPSACH
                GROUP BY 
                    MaNS";

                    // Query 2: Thông tin phiếu nhập
                    string query2 = @"
                SELECT 
                    NS.MaNS AS [Mã Phiếu Nhập],
                    NS.NgayNS AS [Ngày Nhập],
                    NS.TongTien AS [Tổng Tiền]
                FROM 
                    NHAPSACH AS NS";

                    // Query 3: Tên người dùng liên quan
                    string query3 = @"
                SELECT 
                    NHAPSACH.MaNS,
                    NGUOIDUNG.HoTen AS [Tên Người Nhập]
                FROM 
                    NGUOIDUNG
                INNER JOIN 
                    NHAPSACH ON NGUOIDUNG.MaNguoiDung = NHAPSACH.MaNguoiDung";

                    // Load dữ liệu từ từng query
                    var da1 = new SqlDataAdapter(new SqlCommand(query1, conn));
                    var da2 = new SqlDataAdapter(new SqlCommand(query2, conn));
                    var da3 = new SqlDataAdapter(new SqlCommand(query3, conn));

                    var dt1 = new DataTable();
                    var dt2 = new DataTable();
                    var dt3 = new DataTable();

                    da1.Fill(dt1); // Tổng số lượng nhập
                    da2.Fill(dt2); // Thông tin phiếu nhập
                    da3.Fill(dt3); // Tên người dùng

                    // Kết hợp dữ liệu từ các DataTable
                    var combinedTable = CombineDataTables(dt1, dt2, dt3);

                    // Hiển thị vào DataGridView
                    dataGridViewTTnhapSach.DataSource = combinedTable;

                    // Định dạng cột
                    FormatGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        // Hàm định dạng DataGridView
        private void FormatGridView()
        {
            if (dataGridViewTTnhapSach.Columns.Contains("Tổng Số Lượng Nhập"))
                dataGridViewTTnhapSach.Columns["Tổng Số Lượng Nhập"].DefaultCellStyle.Format = "N0";
            if (dataGridViewTTnhapSach.Columns.Contains("Tổng Tiền"))
                dataGridViewTTnhapSach.Columns["Tổng Tiền"].DefaultCellStyle.Format = "N0";
            dataGridViewTTnhapSach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

        // Hàm kết hợp các DataTable
        private DataTable CombineDataTables(DataTable dt1, DataTable dt2, DataTable dt3)
        {
            var resultTable = dt2.Copy(); // Sao chép cấu trúc của bảng dt2

            // Thêm cột từ các bảng khác
            resultTable.Columns.Add("Tổng Số Lượng Nhập", typeof(int));
            resultTable.Columns.Add("Tên Người Nhập", typeof(string));

            foreach (DataRow row in resultTable.Rows)
            {
                var maNS = row["Mã Phiếu Nhập"].ToString();

                // Lấy dữ liệu từ dt1
                var dt1Row = dt1.AsEnumerable().FirstOrDefault(r => r["MaNS"].ToString() == maNS);
                row["Tổng Số Lượng Nhập"] = dt1Row?["Tổng Số Lượng Nhập"] ?? DBNull.Value;

                // Lấy dữ liệu từ dt3
                var dt3Row = dt3.AsEnumerable().FirstOrDefault(r => r["MaNS"].ToString() == maNS);
                row["Tên Người Nhập"] = dt3Row?["Tên Người Nhập"] ?? DBNull.Value;
            }

            return resultTable;
        }


        private void btnTaoPhieuMoi_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn muốn nhập phiếu hàng mới!","Thông Báo",
                MessageBoxButtons.OKCancel,
                 MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
               
                Uc_TT_NhapSach ct_nhapsach = new Uc_TT_NhapSach(tendangnhap);
               
                this.Controls.Clear();
                ct_nhapsach.Dock = DockStyle.Fill;
                this.Controls.Add(ct_nhapsach);
              
            }
           
            else
            {
                // Xử lý khi người dùng nhấn Cancel
                MessageBox.Show("Bạn đã hủy thao tác.");
            }
         }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {

           
         }

        public event Action<string> OnMaNhapSelected;

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            string maNhap = cbxMaNhapSach.SelectedItem.ToString();

           
            OnMaNhapSelected?.Invoke(maNhap);
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }


        private void CalculateTotalAmount()
        {
            // Lấy giá trị MaNS từ ComboBox
            string maNS = cbxMaNhapSach.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(maNS))
            {
                MessageBox.Show("Vui lòng chọn mã nhập sách.");
                return;
            }

            // Biến để lưu tổng tiền và ngày giờ nhập
            double totalAmount = 0;
            DateTime? ngayNhap = null;

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Truy vấn để lấy tổng tiền và ngày nhập từ bảng NHAPSACH theo MaNS
                    string query = @"
                SELECT TongTien, NgayNS
                FROM NHAPSACH
                WHERE MaNS = @MaNS";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Thêm tham số cho câu truy vấn
                        cmd.Parameters.AddWithValue("@MaNS", maNS);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Lấy tổng tiền
                                if (!reader.IsDBNull(0)) // Cột đầu tiên: TongTien
                                {
                                    totalAmount = reader.GetFieldValue<double>(0);
                                }

                                // Lấy ngày nhập
                                if (!reader.IsDBNull(1)) // Cột thứ hai: NgayNhap
                                {
                                    ngayNhap = reader.GetFieldValue<DateTime>(1);
                                }
                            }
                        }
                    }
                }

                // Cập nhật tổng tiền vào TextBox
                textBoxTongTien.Text = totalAmount.ToString("N2"); // Định dạng số với 2 chữ số thập phân

                // Cập nhật ngày nhập vào DateTimePicker
                if (ngayNhap.HasValue)
                {
                    dateTimePicker2.Value = ngayNhap.Value; // Set ngày nhập vào DateTimePicker
                }
                else
                {
                    MessageBox.Show("Không tìm thấy ngày nhập cho mã nhập sách này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (InvalidCastException ice)
            {
                MessageBox.Show("Lỗi kiểu dữ liệu trong cơ sở dữ liệu: " + ice.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tính tổng tiền và lấy ngày nhập: " + ex.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
                // Kiểm tra nếu chưa chọn mã nhập sách
                if (cbxMaNhapSach.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn mã nhập sách.");
                }
                else
                {
                    // Lấy mã nhập sách từ ComboBox và chuyển nó thành chuỗi
                    string maNhap = cbxMaNhapSach.SelectedItem.ToString();

                   // Khởi tạo form CT_1PN với tham số mã nhập sách
                    CT_1PN cT_1PN = new CT_1PN(tendangnhap, maNhap);
                    cT_1PN.Show();
                 
                   CalculateTotalAmount();
                 
            }


            
        }


        private void button2_Click(object sender, EventArgs e)
        {
            UC_Home home3 = new UC_Home(tendangnhap, mahd);

            this.Controls.Clear();
        
            home3.Dock = DockStyle.Fill;
            this.Controls.Add(home3);
        }

        private void dataGridViewTTnhapSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void LoadGridViewTK(DateTime? filterDate = null)
        {
            checkBoxTimKiem.Checked = false;
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Query 1: Tổng số lượng nhập theo MaNS
                    string query1 = @"
            SELECT 
                MaNS, 
                SUM(SoLuongNhap) AS [Tổng Số Lượng Nhập]
            FROM 
                CT_NHAPSACH
            GROUP BY 
                MaNS";

                    // Query 2: Thông tin phiếu nhập
                    string query2 = @"
            SELECT 
                NS.MaNS AS [Mã Phiếu Nhập],
                NS.NgayNS AS [Ngày Nhập],
                NS.TongTien AS [Tổng Tiền]
            FROM 
                NHAPSACH AS NS";

                    // Query 3: Tên người dùng liên quan
                    string query3 = @"
            SELECT 
                NHAPSACH.MaNS,
                NGUOIDUNG.HoTen AS [Tên Người Nhập]
            FROM 
                NGUOIDUNG
            INNER JOIN 
                NHAPSACH ON NGUOIDUNG.MaNguoiDung = NHAPSACH.MaNguoiDung";

                    // Thêm điều kiện lọc theo ngày nếu filterDate có giá trị
                    if (filterDate.HasValue)
                    {
                        query2 += " WHERE CAST(NS.NgayNS AS DATE) = @FilterDate";
                    }

                    // Load dữ liệu từ từng query
                    var cmd1 = new SqlCommand(query1, conn);
                    var cmd2 = new SqlCommand(query2, conn);
                    var cmd3 = new SqlCommand(query3, conn);

                    // Thêm tham số vào query 2 nếu có ngày lọc
                    if (filterDate.HasValue)
                    {
                        cmd2.Parameters.AddWithValue("@FilterDate", filterDate.Value);
                    }

                    var da1 = new SqlDataAdapter(cmd1);
                    var da2 = new SqlDataAdapter(cmd2);
                    var da3 = new SqlDataAdapter(cmd3);

                    var dt1 = new DataTable();
                    var dt2 = new DataTable();
                    var dt3 = new DataTable();

                    da1.Fill(dt1); // Tổng số lượng nhập
                    da2.Fill(dt2); // Thông tin phiếu nhập
                    da3.Fill(dt3); // Tên người dùng

                    // Kết hợp dữ liệu từ các DataTable
                    var combinedTable = CombineDataTables(dt1, dt2, dt3);

                    // Hiển thị vào DataGridView
                    dataGridViewTTnhapSach.DataSource = combinedTable;

                    // Định dạng cột
                    FormatGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

    
        private void cb_Tk_CheckedChanged(object sender, EventArgs e)
        {
           
                if (checkBoxTimKiem.Checked)
                {
                    // Tìm kiếm theo ngày nhập
                    DateTime selectedDate = dateTimePicker1.Value.Date; // Lấy ngày từ DateTimePicker
                    LoadGridViewTK(selectedDate);
                  
               }
                else
                {
                    // Hiển thị lại toàn bộ dữ liệu
                    LoadGridViewTK(null);
              
            }
            
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
           
                // Xác nhận từ người dùng
                DialogResult result = MessageBox.Show("Bạn có muốn xóa phiếu nhập này không? Lưu ý: Dữ liệu sẽ bị xóa hoàn toàn.",
                                                      "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                // Nếu người dùng nhấn Yes, tiến hành xóa
                if (result == DialogResult.Yes)
                {
                // Lấy mã phiếu nhập từ DataGridView (giả sử bạn đang chọn phiếu nhập trong DataGridView)
                // Kiểm tra nếu chưa chọn mã nhập sách
                if (cbxMaNhapSach.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn mã nhập sách.");
                }
                else 
                {
                    string maNhap = cbxMaNhapSach.SelectedItem.ToString();

                    if (!string.IsNullOrEmpty(maNhap))
                        {
                            // Kết nối đến cơ sở dữ liệu
                           
                            using (SqlConnection conn = new SqlConnection(connectionString))
                            {
                                try
                                {
                                    conn.Open();

                                    // Bắt đầu một giao dịch (Transaction)
                                    using (SqlTransaction transaction = conn.BeginTransaction())
                                    {
                                        try
                                        {
                                            // Xóa dữ liệu liên quan trong bảng CT_NHAPSACH (Chi tiết phiếu nhập sách)
                                            string deleteCTNhapSachQuery = "DELETE FROM CT_NHAPSACH WHERE MaNS = @MaNS";
                                            using (SqlCommand cmd = new SqlCommand(deleteCTNhapSachQuery, conn, transaction))
                                            {
                                                cmd.Parameters.AddWithValue("@MaNS", maNhap);
                                                cmd.ExecuteNonQuery();
                                            }

                                            // Xóa dữ liệu liên quan trong bảng SACH (nếu có dữ liệu liên quan)
                                            string deleteSACGQuery = "DELETE FROM SACH WHERE MaNS = @MaNS";
                                            using (SqlCommand cmd = new SqlCommand(deleteSACGQuery, conn, transaction))
                                            {
                                                cmd.Parameters.AddWithValue("@MaNS", maNhap);
                                                cmd.ExecuteNonQuery();
                                            }

                                            // Cuối cùng, xóa phiếu nhập trong bảng NHAPSACH
                                            string deleteNhapSachQuery = "DELETE FROM NHAPSACH WHERE MaNS = @MaNS";
                                            using (SqlCommand cmd = new SqlCommand(deleteNhapSachQuery, conn, transaction))
                                            {
                                                cmd.Parameters.AddWithValue("@MaNS", maNhap);
                                                cmd.ExecuteNonQuery();
                                            }

                                            // Nếu không có lỗi, commit giao dịch
                                            transaction.Commit();

                                            // Thông báo người dùng đã xóa thành công
                                            MessageBox.Show("Đã xóa phiếu nhập và các dữ liệu liên quan.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        catch (Exception ex)
                                        {
                                            // Nếu có lỗi, rollback giao dịch
                                            transaction.Rollback();
                                            MessageBox.Show($"Lỗi khi xóa dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Lỗi kết nối cơ sở dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Không có mã phiếu nhập để xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    
                }
            

        }

        private void btnDong_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
