using NHOM.User;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NHOM.KHACHHANG
{
    public partial class UC_TT_KH : UserControl
    {
        string ten;
        string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=QL_Sach_DA;User ID=sa;Password='123'"; // Chuỗi kết nối cơ sở dữ liệu
        private int mahd; 
        public UC_TT_KH(string ten)
        {
            InitializeComponent();
            this.ten = ten;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            textBoxTenKH.Text = string.Empty;
            textBoxSDTKH.Text = string.Empty;
            textBoxDC.Text = string.Empty;
            // Lấy thông tin từ UserControl "UC_CAPNHAT_TT"
            UC_CAPNHAT_TT userControl = panelNHAP_TT.Controls["UC_CAPNHAT_TT"] as UC_CAPNHAT_TT;

            if (userControl == null)
            {
                userControl = new UC_CAPNHAT_TT();
                userControl.Name = "UC_CAPNHAT_TT";
                userControl.Dock = DockStyle.Fill;
                panelNHAP_TT.Controls.Clear();  // Xóa tất cả các điều khiển hiện có
                panelNHAP_TT.Controls.Add(userControl);  // Thêm UserControl vào panel
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

            if(tenKhachHang == null || gioiTinh == null || sdt == null)
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
                        // Nếu số điện thoại đã tồn tại, hiển thị thông báo lỗi và không thực hiện thêm
                        MessageBox.Show("Số điện thoại này đã tồn tại trong cơ sở dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;  // Dừng lại ở đây, không thêm dữ liệu nữa
                    }

                    // Nếu số điện thoại chưa tồn tại, thêm dữ liệu vào CSDL
                    string insertQuery = "INSERT INTO KhachHang (TenKH, GioiTinh, SDT, DiaChi) VALUES (@TenKhachHang, @GioiTinh, @SDT, @DiaChi)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@TenKhachHang", tenKhachHang);
                    insertCmd.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                    insertCmd.Parameters.AddWithValue("@SDT", sdt);
                    insertCmd.Parameters.AddWithValue("@DiaChi", diaChi);

                    // Thực thi câu lệnh SQL để thêm dữ liệu vào CSDL
                    insertCmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Cập nhật DataGridView
                    LoadData();

                    userControl.ClearTextBox();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi khi kết nối đến cơ sở dữ liệu : " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadData()
        {
            // Chuỗi truy vấn SQL để lấy dữ liệu từ bảng KHACHHANG
            string query = "SELECT TOP 1000 MaKH AS 'Mã khách hàng', TenKH AS 'Tên khách hàng', DiaChi AS 'Địa chỉ', GioiTinh AS 'Giới tính', SDT AS 'Số điện thoại' FROM KHACHHANG";

            // Kết nối với cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();

                try
                {
                    // Lấy dữ liệu từ cơ sở dữ liệu và điền vào DataTable
                    dataAdapter.Fill(dataTable);

                    // Gán DataTable vào DataGridView để hiển thị dữ liệu
                    dataGridViewKH.DataSource = dataTable;
                    dataGridViewKH.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lấy dữ liệu khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UC_TT_KH_Load(object sender, EventArgs e)
        {
            // Tải dữ liệu khi UserControl được load
            LoadData();

            // Ẩn/Hiện các nút khi load
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            MessageBox.Show("vo day ha");
            if (dataGridViewKH.SelectedRows.Count > 0)
            {
                string maKH = dataGridViewKH.SelectedRows[0].Cells["Mã khách hàng"].Value?.ToString();

                MessageBox.Show("ma " + maKH);

                if (string.IsNullOrEmpty(maKH))
                {
                    MessageBox.Show("Mã khách hàng không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng {maKH}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            string deleteQuery = "DELETE FROM KHACHHANG WHERE MaKH = @MaKH";
                            using (SqlCommand cmd = new SqlCommand(deleteQuery, connection))
                            {
                                cmd.Parameters.AddWithValue("@MaKH", maKH);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Xóa khách hàng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();  // Tải lại dữ liệu sau khi xóa
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            MessageBox.Show("day a");
        }

        private void btnSua_Click(object sender, EventArgs e)
        {

            if (dataGridViewKH.SelectedRows.Count > 0)
            {
                // Lấy mã khách hàng từ dòng được chọn
                string maKH = dataGridViewKH.SelectedRows[0].Cells["Mã khách hàng"].Value?.ToString();

                if (string.IsNullOrEmpty(maKH))
                {
                    MessageBox.Show("Mã khách hàng không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Hiển thị thông tin khách hàng vào các TextBox trong UC_CAPNHAT_TT
                UC_CAPNHAT_TT userControl = panelNHAP_TT.Controls["UC_CAPNHAT_TT"] as UC_CAPNHAT_TT;

                if (userControl == null)
                {
                    // Nếu không tìm thấy UC_CAPNHAT_TT trong panel, tạo mới
                    userControl = new UC_CAPNHAT_TT();
                    userControl.Name = "UC_CAPNHAT_TT";
                    userControl.Dock = DockStyle.Fill;
                    panelNHAP_TT.Controls.Clear();
                    panelNHAP_TT.Controls.Add(userControl);
                }

                // Cập nhật thông tin vào UserControl
                userControl.SetTenKhachHang(dataGridViewKH.SelectedRows[0].Cells["Tên khách hàng"].Value?.ToString());
                userControl.SetDiaChi(dataGridViewKH.SelectedRows[0].Cells["Địa chỉ"].Value?.ToString());
                userControl.SetGioiTinh(dataGridViewKH.SelectedRows[0].Cells["Giới tính"].Value?.ToString());
                userControl.SetSdt(dataGridViewKH.SelectedRows[0].Cells["Số điện thoại"].Value?.ToString());
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }




        }

        private void button2_Click(object sender, EventArgs e)
        {
            UC_Home home2 = new UC_Home(ten, mahd);
            this.Controls.Clear();
            home2.Dock = DockStyle.Fill;
            this.Controls.Add(home2);
        }

        private void btnMo_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnMo.Enabled = false;
            btnXoa.Enabled = true;

            UC_CAPNHAT_TT userControl = panelNHAP_TT.Controls["UC_CAPNHAT_TT"] as UC_CAPNHAT_TT;

            if (userControl == null)
            {
                userControl = new UC_CAPNHAT_TT();
                userControl.Name = "UC_CAPNHAT_TT";
                userControl.Dock = DockStyle.Fill;
                panelNHAP_TT.Controls.Clear();
                panelNHAP_TT.Controls.Add(userControl);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            UC_CAPNHAT_TT userControl = panelNHAP_TT.Controls["UC_CAPNHAT_TT"] as UC_CAPNHAT_TT;

            if (userControl == null)
            {
                userControl = new UC_CAPNHAT_TT();
                userControl.Name = "UC_CAPNHAT_TT";
                userControl.Dock = DockStyle.Fill;
                panelNHAP_TT.Controls.Clear();  // Xóa tất cả các điều khiển hiện có
                panelNHAP_TT.Controls.Add(userControl);  // Thêm UserControl vào panel
            }

            // Lấy dữ liệu từ UserControl
            string tenKhachHang = userControl.GetTenKhachHang();
            string gioiTinh = userControl.GetGioiTinh();
            string sdt = userControl.GetSdt();
            string diaChi = userControl.GetDiaChi();

            // Lấy mã khách hàng từ dòng được chọn trong DataGridView
            string maKH = dataGridViewKH.SelectedRows[0].Cells["Mã khách hàng"].Value?.ToString();

            if (string.IsNullOrEmpty(maKH))
            {
                MessageBox.Show("Mã khách hàng không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Kết nối tới cơ sở dữ liệu và cập nhật thông tin
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Câu truy vấn cập nhật thông tin khách hàng
                    string updateQuery = @"
            UPDATE KHACHHANG
            SET TenKH = @TenKH, DiaChi = @DiaChi, GioiTinh = @gt, SDT = @SDT
            WHERE MaKH = @MaKH";  // Đảm bảo @MaKH được đưa vào trong câu truy vấn

                    using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                    {
                        // Thêm các tham số vào câu lệnh SQL
                        cmd.Parameters.AddWithValue("@TenKH", tenKhachHang);
                        cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                        cmd.Parameters.AddWithValue("@gt", gioiTinh);
                        cmd.Parameters.AddWithValue("@SDT", sdt);
                        cmd.Parameters.AddWithValue("@MaKH", maKH);  // Thêm tham số @MaKH

                        // Thực thi câu lệnh cập nhật
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Cập nhật thông tin khách hàng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();  // Tải lại dữ liệu sau khi sửa
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật thông tin khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxTenKH_TextChanged(object sender, EventArgs e)
        {
          
                // Khi người dùng nhập vào textBoxTenKH, tiến hành tìm kiếm
                string searchQuery = textBoxTenKH.Text.Trim();

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    // Tìm kiếm trong cơ sở dữ liệu
                    SearchCustomers(searchQuery);
                }
                else
                {
                    // Nếu không nhập gì, hiển thị tất cả khách hàng
                    LoadData();
                }
            
        }

        private void SearchCustomers(string searchQuery)
        {
            string query = "SELECT TOP 1000 MaKH AS 'Mã khách hàng', TenKH AS 'Tên khách hàng', DiaChi AS 'Địa chỉ', GioiTinh AS 'Giới tính', SDT AS 'Số điện thoại' FROM KHACHHANG " +
                           "WHERE TenKH LIKE @searchQuery";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // Gán dữ liệu vào DataGridView
                dataGridViewKH.DataSource = dataTable;
            }
        }

       
        // Sự kiện TextChanged cho số điện thoại
        private void textBoxSDTKH_TextChanged(object sender, EventArgs e)
        {
            // Lấy chuỗi nhập vào ô tìm kiếm (số điện thoại)
            string searchQuery = textBoxSDTKH.Text.Trim();

            // Kiểm tra nếu chuỗi không rỗng
            if (!string.IsNullOrEmpty(searchQuery))
            {
                // Tiến hành tìm kiếm theo số điện thoại
                SearchCustomersByPhone(searchQuery);
            }
            else
            {
                // Nếu không có gì nhập vào, hiển thị tất cả khách hàng
                LoadData();
            }
        }

        // Hàm tìm kiếm khách hàng theo số điện thoại
        private void SearchCustomersByPhone(string searchQuery)
        {
            // Câu lệnh SQL tìm kiếm theo số điện thoại
            string query = "SELECT TOP 1000 MaKH AS 'Mã khách hàng', TenKH AS 'Tên khách hàng', DiaChi AS 'Địa chỉ', GioiTinh AS 'Giới tính', SDT AS 'Số điện thoại' " +
                           "FROM KHACHHANG " +
                           "WHERE SDT LIKE @searchQuery";

            // Kết nối cơ sở dữ liệu và thực hiện truy vấn
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // Gán kết quả tìm kiếm vào DataGridView
                dataGridViewKH.DataSource = dataTable;
            }
        }

        private void textBoxDC_TextChanged(object sender, EventArgs e)
        {
           
                // Lấy chuỗi nhập vào ô tìm kiếm (địa chỉ)
                string searchQuery = textBoxDC.Text.Trim();

                // Kiểm tra nếu chuỗi không rỗng
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    // Tiến hành tìm kiếm theo địa chỉ
                    SearchCustomersByAddress(searchQuery);
                }
                else
                {
                    // Nếu không có gì nhập vào, hiển thị tất cả khách hàng
                    LoadData();
                }
        }

            // Hàm tìm kiếm khách hàng theo địa chỉ
            private void SearchCustomersByAddress(string searchQuery)
            {
                // Câu lệnh SQL tìm kiếm theo địa chỉ
                string query = "SELECT TOP 1000 MaKH AS 'Mã khách hàng', TenKH AS 'Tên khách hàng', DiaChi AS 'Địa chỉ', GioiTinh AS 'Giới tính', SDT AS 'Số điện thoại'" +
                               "FROM KHACHHANG " +
                               "WHERE DiaChi LIKE @searchQuery";

                // Kết nối cơ sở dữ liệu và thực hiện truy vấn
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");

                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Gán kết quả tìm kiếm vào DataGridView
                    dataGridViewKH.DataSource = dataTable;
                }
            }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        // Hàm tải tất cả khách hàng (nếu không có tìm kiếm)

    }
}
