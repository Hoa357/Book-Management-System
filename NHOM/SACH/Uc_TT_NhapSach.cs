using NHOM.User;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NHOM.SACH
{
    public partial class Uc_TT_NhapSach : UserControl
    {
        private readonly string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=QL_SACH_DA;User ID=sa;Password='123'";
        private readonly string tendangnhap;
        private int maNhapSach; // Khai báo biến toàn cục
        private int mahd;
        public Uc_TT_NhapSach(string tendangnhap)
        {
            InitializeComponent();
            this.tendangnhap = tendangnhap;


        }

        private void Uc_TT_NhapSach_Load(object sender, EventArgs e)
        {
            LoadTheLoaiIntoComboBox();

            txtTenSach.Enabled = false;
            comboBoxTheLoai.Enabled = false;
            txtNhaXuatBan.Enabled = false;
            textBoxNamXuatBan.Enabled = false;
            numericUpDownSLNhap.Enabled = false;
            txtGiaNhap.Enabled = false;
            pictureBoxAnh.Enabled = false;
            pictureBox1.Enabled = true;

        }

        // Load danh sách thể loại vào ComboBox
        private void LoadTheLoaiIntoComboBox()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    var query = "SELECT MaTheLoai FROM THELOAI";
                    var cmd = new SqlCommand(query, conn);
                    var reader = cmd.ExecuteReader();

                    comboBoxTheLoai.Items.Clear();
                    while (reader.Read())
                    {
                        string maTheLoai = reader["MaTheLoai"].ToString();

                        comboBoxTheLoai.Items.Add($"{maTheLoai}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải danh sách thể loại: " + ex.Message);
                }
            }
        }

        // Load dữ liệu vào DataGridView
        private void LoadDataIntoDataGridView(int maNhapSach)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    var query = @"
            SELECT c.MaSach as N'Mã sách', 
       s.TenSach as N'Tên sách', 
       c.SoLuongNhap as N'Sl nhập', 
       c.DonGiaNhap as N'Đơn giá', 
       (c.SoLuongNhap * c.DonGiaNhap) AS ThanhTien,
       s.MaTheLoai as N'Mã thể loại',
       s.NhaXuatBan as N'Nhà xb',
       s.NamXuatBan as N'Năm xb',
       s.Images as N'Hình ảnh'
FROM CT_NHAPSACH c
JOIN SACH s ON c.MaSach = s.MaSach
WHERE c.MaNS = @MaNS;
"; // Chỉ lấy dữ liệu có MaNS khớp

                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaNS", maNhapSach);

                    var adapter = new SqlDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridViewBooks.DataSource = dt;
                    dataGridViewBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    // Tính tổng tiền từ cột ThanhTien
                    double totalAmount = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        totalAmount += Convert.ToDouble(row["ThanhTien"]);
                    }

                    // Hiển thị tổng tiền
                    label1TongTien.Text = totalAmount.ToString("N2"); // Format tiền tệ
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
                }
            }
        }

        // // Hàm lấy tên thể loại từ mã thể loại
        private string GetTenTheLoai(string maTheLoai)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT TenTheLoai FROM THELOAI WHERE MaTheLoai = @MaTheLoai";
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);

                    var result = cmd.ExecuteScalar();
                    return result != null ? result.ToString() : null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lấy tên thể loại: " + ex.Message);
                    return null;
                }
            }
        }


        // Hàm lấy mã người nhập
        private int GetMaNguoiNhap(string tenDangNhap)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    var query = "SELECT MaNguoiDung FROM NGUOIDUNG WHERE UserName = @TenDangNhap";
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);

                    var result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
                catch
                {
                    return -1;
                }
            }
        }



        // Hàm lấy mã sách
        private int AddBookToSACH(string maTheLoai, string tenSach, string nhaXuatBan, int namXuatBan, int soLuong, double donGia, string imageName)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    var query = "INSERT INTO SACH (MaTheLoai, TenSach, NhaXuatBan, NamXuatBan, SoLuongTon, DonGiaBan, Images) " +
                                "VALUES (@MaTheLoai, @TenSach, @NhaXuatBan, @NamXuatBan, @SoLuongTon, @DonGiaBan, @HinhAnh); " +
                                "SELECT SCOPE_IDENTITY();"; // Lấy mã sách tự động tăng

                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);
                    cmd.Parameters.AddWithValue("@TenSach", tenSach);
                    cmd.Parameters.AddWithValue("@NhaXuatBan", nhaXuatBan);
                    cmd.Parameters.AddWithValue("@NamXuatBan", namXuatBan);
                    cmd.Parameters.AddWithValue("@SoLuongTon", soLuong);
                    cmd.Parameters.AddWithValue("@DonGiaBan", donGia);
                    cmd.Parameters.AddWithValue("@HinhAnh", imageName);

                    // Thực thi và lấy giá trị mã sách
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1; // Trả về mã sách
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm vào sách: " + ex.Message);
                    return -1;
                }
            }
        }


        // Thêm sách và phiếu nhập

        private void buttonAnh_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png)|*.jpg;*.jpeg;*.png";
                openFileDialog.Title = "Chọn ảnh";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Hiển thị ảnh mới trong PictureBox
                    pictureBoxAnh.ImageLocation = openFileDialog.FileName;

                    // Lưu đường dẫn ảnh vào Tag để xử lý khi lưu
                    pictureBoxAnh.Tag = openFileDialog.FileName;
                }

            }

        }

        private void pictureBoxTAO_PN_MOI_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn muốn nhập 1 đơn hàng mới!", "Thông Báo", MessageBoxButtons.OKCancel,
            MessageBoxIcon.Question
            );

            if (result == DialogResult.OK)
            {
                try
                {
                    // Tạo phiếu nhập mới trong cơ sở dữ liệu và lấy mã nhập sách
                    int newMaNhapSach = CreateNewPhieuNhap();
                    if (newMaNhapSach <= 0)
                    {
                        MessageBox.Show("Không thể tạo phiếu nhập mới.");
                        return;
                    }



                    maNhapSach = newMaNhapSach;


                    // Kích hoạt các trường nhập liệu và nút bấm
                    txtTenSach.Enabled = true;
                    comboBoxTheLoai.Enabled = true;
                    txtNhaXuatBan.Enabled = true;
                    textBoxNamXuatBan.Enabled = true;
                    numericUpDownSLNhap.Enabled = true;
                    txtGiaNhap.Enabled = true;
                    pictureBoxAnh.Enabled = true;
                    pictureBox1.Enabled = true;
                    btnLuu.Enabled = true;
                    btnXoa.Enabled = true;
                    btnSua.Enabled = true;
                    btnLuu.Enabled = true;
                    pictureBoxAnh.Enabled = true;

                    MessageBox.Show("Tạo phiếu nhập mới thành công.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tạo phiếu nhập mới: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Bạn đã hủy thao tác.");
            }
        }

        private void labelTTien_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {


            UC_Home home2 = new UC_Home(tendangnhap, mahd);

            // Xóa tất cả các control trên form chính
            this.Controls.Clear();

            // Thêm UserControl mới
            home2.Dock = DockStyle.Fill;
            this.Controls.Add(home2);



        }

        private void UpdateTotalAmount()
        {
            double totalAmount = 0;

            // Lặp qua các hàng trong DataGridView để tính tổng tiền
            foreach (DataGridViewRow row in dataGridViewBooks.Rows)
            {
                if (row.Cells["ThanhTien"].Value != null)
                {
                    totalAmount += Convert.ToDouble(row.Cells["ThanhTien"].Value);
                }
            }

            // Cập nhật label với tổng tiền đã định dạng
            label1TongTien.Text = totalAmount.ToString("N2");
        }

        private void btnSua_Click(object sender, EventArgs e)
        {

            if (dataGridViewBooks.SelectedRows.Count > 0)
            {
                try
                {
                    // Lấy dòng được chọn
                    DataGridViewRow selectedRow = dataGridViewBooks.SelectedRows[0];

                    // Gán dữ liệu vào các trường thông tin
                    txtTenSach.Text = selectedRow.Cells["Tên sách"].Value?.ToString() ?? string.Empty;
                    comboBoxTheLoai.SelectedItem = selectedRow.Cells["Mã thể loại"].Value?.ToString() ?? string.Empty;
                    txtNhaXuatBan.Text = selectedRow.Cells["Nhà xb"].Value?.ToString() ?? string.Empty;
                    textBoxNamXuatBan.Text = selectedRow.Cells["Năm xb"].Value?.ToString() ?? string.Empty;
                    numericUpDownSLNhap.Value = Convert.ToDecimal(selectedRow.Cells["Sl nhập"].Value ?? 0);
                    txtGiaNhap.Text = selectedRow.Cells["Đơn giá"].Value?.ToString() ?? string.Empty;

                    // Xử lý hình ảnh
                    string imagePath = selectedRow.Cells["Hình ảnh"].Value?.ToString(); // Đường dẫn ảnh đầy đủ

                    if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                    {
                        pictureBoxAnh.ImageLocation = imagePath;
                        pictureBoxAnh.Tag = imagePath; // Lưu đường dẫn vào Tag để sử dụng sau
                    }
                    else
                    {
                        pictureBoxAnh.Image = null; // Xóa ảnh nếu không tìm thấy
                        MessageBox.Show("Không tìm thấy hình ảnh hoặc đường dẫn không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi sửa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn dòng cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        



        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
         
            try
            {
                if (dataGridViewBooks.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow row in dataGridViewBooks.SelectedRows)
                    {
                        // Lấy mã sách từ dòng được chọn
                        int maSach = Convert.ToInt32(row.Cells["Mã sách"].Value); // Đảm bảo tên cột chính xác

                        using (var conn = new SqlConnection(connectionString))
                        {
                            conn.Open();

                            // Xóa dữ liệu trong bảng CT_NHAPSACH trước
                            string deleteCTNhapSachQuery = "DELETE FROM CT_NHAPSACH WHERE MaSach = @MaSach";
                            using (var cmdCT = new SqlCommand(deleteCTNhapSachQuery, conn))
                            {
                                cmdCT.Parameters.AddWithValue("@MaSach", maSach);
                                cmdCT.ExecuteNonQuery();
                            }

                            // Xóa dữ liệu trong bảng SACH
                            string deleteSachQuery = "DELETE FROM SACH WHERE MaSach = @MaSach";
                            using (var cmdSach = new SqlCommand(deleteSachQuery, conn))
                            {
                                cmdSach.Parameters.AddWithValue("@MaSach", maSach);
                                cmdSach.ExecuteNonQuery();
                            }
                        }

                        // Xóa dòng khỏi DataGridView
                        if (!row.IsNewRow)
                        {
                            dataGridViewBooks.Rows.RemoveAt(row.Index);
                            UpdateTotalAmount();
                        }
                    }
                    MessageBox.Show("Xóa thành công.");
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn dòng cần xóa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message);
            }

        
        }


        private void ClearTextBoxes()
        {
            txtTenSach.Clear();
            txtNhaXuatBan.Clear();
            textBoxNamXuatBan.Clear();
            txtGiaNhap.Clear();
            numericUpDownSLNhap.Value = 0;
            comboBoxTheLoai.SelectedItem = null;
        }


      

       

        private int CreateNewPhieuNhap()
        {
            int maNhapSach = 0;

            using (var conn = new SqlConnection(connectionString))
            {
                try
                {

                    conn.Open();

                    // Lệnh SQL thêm phiếu nhập mới
                    string query = "INSERT INTO NHAPSACH (NgayNS, MaNguoiDung) OUTPUT INSERTED.MaNS VALUES (@NgayNS, @MaNguoiDung)";
                    var cmd = new SqlCommand(query, conn);


                    cmd.Parameters.AddWithValue("@NgayNS", DateTime.Today);
                    cmd.Parameters.AddWithValue("@MaNguoiDung", GetMaNguoiNhap(tendangnhap));

                    // Thực thi lệnh và kiểm tra kết quả
                    var result = cmd.ExecuteScalar();

                    if (result == null)
                    {
                        MessageBox.Show("Không nhận được mã nhập sách từ cơ sở dữ liệu.");
                        return 0;
                    }

                    maNhapSach = Convert.ToInt32(result);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tạo phiếu nhập mới: " + ex.Message);
                }
            }

            return maNhapSach;
        }

       

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
          
            try
            {
                string tenSach = txtTenSach.Text.Trim();
                string maTheLoai = comboBoxTheLoai.SelectedItem?.ToString();
                string nhaXuatBan = txtNhaXuatBan.Text.Trim();

                // Kiểm tra dữ liệu nhập vào
                int soLuongNhap = (int)numericUpDownSLNhap.Value;
                if (!double.TryParse(txtGiaNhap.Text.Trim(), out double donGiaNhap))
                {
                    MessageBox.Show("Đơn giá nhập phải là số.");
                    return;
                }

                int maNguoiNhap = GetMaNguoiNhap(tendangnhap);
                if (maNguoiNhap == -1)
                {
                    MessageBox.Show("Không tìm thấy mã người nhập.");
                    return;
                }

                if (!int.TryParse(textBoxNamXuatBan.Text.Trim(), out int namXuatBan))
                {
                    MessageBox.Show("Năm xuất bản phải là số.");
                    return;
                }

                if (soLuongNhap <= 0)
                {
                    MessageBox.Show("Số lượng nhập phải lớn hơn 0.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy đường dẫn đầy đủ của ảnh
                string imagePath = null;
                if (!string.IsNullOrEmpty(pictureBoxAnh.ImageLocation))
                {
                    imagePath = pictureBoxAnh.ImageLocation; // Lấy full đường dẫn
                }

                // Tính đơn giá mới dựa trên thể loại
                double dongiaMoi = donGiaNhap;
                if (maTheLoai != null)
                {
                    switch (maTheLoai.ToUpper())
                    {
                        case "T":
                            dongiaMoi = donGiaNhap + donGiaNhap * 0.25;
                            break;
                        case "KT":
                            dongiaMoi = donGiaNhap + donGiaNhap * 0.35;
                            break;
                        case "LT":
                            dongiaMoi = donGiaNhap + donGiaNhap * 0.15;
                            break;
                        case "KNS":
                            dongiaMoi = donGiaNhap + donGiaNhap * 0.20;
                            break;
                        default:
                            dongiaMoi = donGiaNhap + donGiaNhap * 0.10;
                            break;
                    }
                }

                // Thêm sách vào bảng SACH và lấy mã sách
                int maSach = AddBookToSACH(maTheLoai, tenSach, nhaXuatBan, namXuatBan, soLuongNhap, dongiaMoi, imagePath);
                if (maSach <= 0)
                {
                    MessageBox.Show("Thêm sách thất bại.");
                    return;
                }

                // Thêm chi tiết phiếu nhập vào bảng CT_NHAPSACH
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var query = "INSERT INTO CT_NHAPSACH (MaNS, MaSach, SoLuongNhap, DonGiaNhap) " +
                                "VALUES (@MaNS, @MaSach, @SoLuongNhap, @DonGiaNhap)";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNS", maNhapSach);
                        cmd.Parameters.AddWithValue("@MaSach", maSach);
                        cmd.Parameters.AddWithValue("@SoLuongNhap", soLuongNhap);
                        cmd.Parameters.AddWithValue("@DonGiaNhap", donGiaNhap);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Load lại dữ liệu và reset form
                LoadDataIntoDataGridView(maNhapSach);
                MessageBox.Show("Thêm thành công.");

                txtGiaNhap.Text = string.Empty;
                txtNhaXuatBan.Text = string.Empty;
                txtTenSach.Text = string.Empty;
                comboBoxTheLoai.SelectedItem = null;
                pictureBoxAnh.Image = null;
                numericUpDownSLNhap.Value = numericUpDownSLNhap.Minimum;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm: " + ex.Message);
            }
        


    }

        private void btnLUU_SUA_Click_2(object sender, EventArgs e)
        {
         
            btnLUU_SUA.Enabled = true;
            if (dataGridViewBooks.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow selectedRow = dataGridViewBooks.SelectedRows[0];
                    string maSach = selectedRow.Cells[0].Value?.ToString();
                    if (string.IsNullOrEmpty(maSach))
                    {
                        MessageBox.Show("Mã sách không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Lấy dữ liệu từ giao diện
                    string tenSach = txtTenSach.Text.Trim();
                    string maTheLoai = comboBoxTheLoai.SelectedItem?.ToString();
                    string nhaXuatBan = txtNhaXuatBan.Text.Trim();
                    if (!int.TryParse(textBoxNamXuatBan.Text.Trim(), out int namXuatBan))
                    {
                        MessageBox.Show("Năm xuất bản phải là số hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int soLuongNhap = (int)numericUpDownSLNhap.Value;
                    if (!decimal.TryParse(txtGiaNhap.Text.Trim(), out decimal donGiaNhap))
                    {
                        MessageBox.Show("Đơn giá nhập phải là số hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string imagePath = selectedRow.Cells[8].Value?.ToString();

                    // Thực hiện cập nhật
                    using (var conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        var updateQuery = @"
                    UPDATE SACH
                    SET TenSach = @TenSach, 
                        MaTheLoai = @MaTheLoai, 
                        NhaXuatBan = @NhaXuatBan, 
                        NamXuatBan = @NamXuatBan,
                        Images = @Images
                    WHERE MaSach = @MaSach;

                    UPDATE CT_NHAPSACH
                    SET SoLuongNhap = @SoLuongNhap, 
                        DonGiaNhap = @DonGiaNhap
                    WHERE MaSach = @MaSach AND MaNS = @MaNS;
                ";

                        using (var cmd = new SqlCommand(updateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@MaSach", maSach);
                            cmd.Parameters.AddWithValue("@TenSach", tenSach);
                            cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);
                            cmd.Parameters.AddWithValue("@NhaXuatBan", nhaXuatBan);
                            cmd.Parameters.AddWithValue("@NamXuatBan", namXuatBan);
                            cmd.Parameters.AddWithValue("@SoLuongNhap", soLuongNhap);
                            cmd.Parameters.AddWithValue("@DonGiaNhap", donGiaNhap);
                            cmd.Parameters.AddWithValue("@Images", imagePath);
                            cmd.Parameters.AddWithValue("@MaNS", maNhapSach);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Tải lại dữ liệu
                    LoadDataIntoDataGridView(maNhapSach);

                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật: " + ex.Message);
                }
            }
        }

        
        private void btnLuu_Click_1(object sender, EventArgs e)
        {

            try
            {
                using (var conn = new SqlConnection(connectionString)) // Kết nối tới cơ sở dữ liệu
                {
                    conn.Open();

                    // Truy vấn SQL để cập nhật tổng tiền
                    var updateQuery = @"
                UPDATE NHAPSACH
  SET TongTien = (
      SELECT SUM(CT_NHAPSACH.TongTien)
      FROM CT_NHAPSACH
      WHERE CT_NHAPSACH.MaNS = NHAPSACH.MaNS
  )
  WHERE EXISTS (
      SELECT 1
      FROM CT_NHAPSACH
      WHERE CT_NHAPSACH.MaNS = NHAPSACH.MaNS
  );
            ";

                    using (var cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.ExecuteNonQuery(); // Thực thi truy vấn
                    }

                    // Lấy tổng tiền từ tất cả các phiếu nhập sách
                    var sumQuery = "SELECT SUM(TongTien) FROM NHAPSACH";
                    using (var cmd = new SqlCommand(sumQuery, conn))
                    {
                        var totalAmount = cmd.ExecuteScalar(); // Lấy tổng tiền
                        if (totalAmount != DBNull.Value)
                        {
                            label1TongTien.Text = Convert.ToDecimal(totalAmount).ToString("N2"); // Cập nhật label
                        }
                        else
                        {
                            label1TongTien.Text = "0.00"; // Nếu không có tổng tiền
                        }
                    }

                    MessageBox.Show("Cập nhật tổng tiền thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật tổng tiền: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        

    }
}
}


    


   




