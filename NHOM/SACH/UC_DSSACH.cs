using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using NHOM.KHACHHANG;
using NHOM.User;

namespace NHOM.SACH
{
    public partial class UC_DSSACH : UserControl
    {
        // Chuỗi kết nối đến cơ sở dữ liệu
        string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=QL_Sach_DA;User ID=sa;Password='123'";
        string tendangnhap;
       int mahd;

        public UC_DSSACH(string tendangnhap,int mahd)
        {
            InitializeComponent();
            this.tendangnhap = tendangnhap;
            this.mahd = mahd;
        }


        private void LoadDataToTreeView()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Lấy dữ liệu thể loại
                    string query = "SELECT MaTheLoai, TenTheLoai FROM TheLoai";  // Lấy MaTheLoai và TenTheLoai
                    SqlCommand cmd = new SqlCommand(query, conn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    treeViewSach.Nodes.Clear();  // Xóa tất cả các mục hiện tại trong TreeView

                    while (reader.Read())
                    {
                        string maTheLoai = reader["MaTheLoai"].ToString();
                        string tenTheLoai = reader["TenTheLoai"].ToString();

                      

                        if (string.IsNullOrEmpty(tenTheLoai))
                        {
                            MessageBox.Show("Tên thể loại trống!");
                            continue;  // Bỏ qua nếu tên thể loại rỗng
                        }

                        // Kiểm tra phông chữ của TreeView và xử lý mã hóa Unicode nếu cần
                        treeViewSach.Font = new Font("Segoe UI", 10);  // Đảm bảo TreeView sử dụng phông chữ hỗ trợ Unicode

                        // Tạo một node cho thể loại
                        TreeNode treeNode = new TreeNode(tenTheLoai.Normalize(NormalizationForm.FormC));

                        // Lấy sách thuộc thể loại này
                        LoadBooksForCategory(treeNode, maTheLoai);

                        // Thêm node thể loại vào TreeView
                        treeViewSach.Nodes.Add(treeNode);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu vào TreeView: " + ex.Message);
                }
            }
        }

        // Hàm lấy sách cho từng thể loại
        private void LoadBooksForCategory(TreeNode categoryNode, string maTheLoai)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Lấy dữ liệu sách thuộc thể loại này
                    string query = "SELECT TenSach, SoLuongTon, DonGiaBan FROM SACH WHERE MaTheLoai = @MaTheLoai";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);

                    SqlDataReader reader = cmd.ExecuteReader();

                    // Duyệt qua từng sách
                    while (reader.Read())
                    {
                        string tenSach = reader["TenSach"].ToString();
                        int soLuongTon = Convert.ToInt32(reader["SoLuongTon"]);
                        double donGiaBan = Convert.ToDouble(reader["DonGiaBan"]);

                        // Kiểm tra nếu tên sách không rỗng
                        if (!string.IsNullOrEmpty(tenSach))
                        {
                            string donGiaBanFormatted = donGiaBan.ToString("C0", CultureInfo.GetCultureInfo("vi-VN"));
                            // Tạo chuỗi hiển thị cho sách với thông tin đầy đủ
                            string nodeText = $"{tenSach} - SL: {soLuongTon} - Giá: {donGiaBanFormatted}";

                            // Tạo một node con cho sách và thêm vào node thể loại
                            TreeNode bookNode = new TreeNode(nodeText.Normalize(NormalizationForm.FormC));
                            categoryNode.Nodes.Add(bookNode);
                        }
                    }

                    reader.Close();
                    categoryNode.Expand();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải sách thuộc thể loại: " + ex.Message);
            }
        }

        private void UC_DSSACH_Load(object sender, EventArgs e)
        {
            LoadTheLoaiIntoComboBox();
            LoadDataToTreeView();
        }

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

        private void timTheoLoai_Click(object sender, EventArgs e)
        {

          
                try
                {
                    // Lấy mã thể loại từ ComboBox
                    string maTheLoai = comboBoxTheLoai.SelectedItem?.ToString(); // Hoặc .SelectedValue tùy thuộc vào kiểu dữ liệu của ComboBox

                    if (string.IsNullOrEmpty(maTheLoai))
                    {
                        MessageBox.Show("Vui lòng chọn mã thể loại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Lấy dữ liệu sách theo mã thể loại từ cơ sở dữ liệu
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        // Truy vấn dữ liệu từ bảng SACH theo mã thể loại
                        string query = @"
                SELECT MaSach, TenSach, SoLuongTon 
                FROM SACH
                WHERE MaTheLoai = @MaTheLoai"; // Lọc sách theo mã thể loại

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);

                        SqlDataReader reader = cmd.ExecuteReader();

                        // Làm mới TreeView
                        treeViewSach.Nodes.Clear();

                        // Kiểm tra nếu có kết quả
                        if (reader.HasRows)
                        {
                            // Tạo một node cha cho thể loại
                            TreeNode nodeTheLoai = new TreeNode("Thể loại: " + maTheLoai);
                            treeViewSach.Nodes.Add(nodeTheLoai);

                            while (reader.Read())
                            {
                                // Lấy thông tin sách
                                string maSach = reader["MaSach"].ToString();
                                string tenSach = reader["TenSach"].ToString();
                                int soLuongTon = Convert.ToInt32(reader["SoLuongTon"]);

                                // Tạo node con cho mỗi sách
                                string nodeText = $"{tenSach} - {soLuongTon}";
                                TreeNode nodeSach = new TreeNode(nodeText);
                              

                                // Thêm node con vào node thể loại
                                nodeTheLoai.Nodes.Add(nodeSach);
                            }

                            // Mở rộng tất cả các node trong TreeView
                            treeViewSach.ExpandAll();
                        }
                        else
                        {
                            MessageBox.Show("Không có sách nào thuộc thể loại này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            


        }

        private void radioButtonNhoHon1990_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonNhoHon1990.Checked)
            {
                int nam;
                if (int.TryParse(textBoxNamXB.Text, out nam))  // Kiểm tra xem năm có hợp lệ không
                {
                    LoadSachTheoNamXuatBan(nam, true);  // Tìm sách có năm xuất bản nhỏ hơn giá trị nhập vào từ TextBox
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập một năm hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void radioButtonLonHon1990_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLonHon1990.Checked)
            {
                int nam;
                if (int.TryParse(textBoxNamXB.Text, out nam))  // Kiểm tra xem năm có hợp lệ không
                {
                    LoadSachTheoNamXuatBan(nam, false);  // Tìm sách có năm xuất bản lớn hơn giá trị nhập vào từ TextBox
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập một năm hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void LoadSachTheoNamXuatBan(int namXuatBan, bool isBefore)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Câu truy vấn SQL tùy thuộc vào cờ isBefore
                    string query = isBefore
                        ? "SELECT TenSach, NamXuatBan, DonGiaBan FROM SACH WHERE NamXuatBan < @NamXuatBan"
                        : "SELECT TenSach, NamXuatBan, DonGiaBan FROM SACH WHERE NamXuatBan > @NamXuatBan";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@NamXuatBan", namXuatBan);

                    SqlDataReader reader = cmd.ExecuteReader();

                    // Làm sạch TreeView trước khi thêm dữ liệu mới
                    treeViewSach.Nodes.Clear();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string tenSach = reader["TenSach"].ToString();
                            int namXuatBanResult = Convert.ToInt32(reader["NamXuatBan"]);
                            double donGiaBan = Convert.ToDouble(reader["DonGiaBan"]);

                            // Tạo chuỗi hiển thị trong TreeNode
                            string nodeText = $"{tenSach} - {namXuatBanResult} - {donGiaBan}";

                            // Thêm node vào TreeView
                            TreeNode nodeSach = new TreeNode(nodeText);
                            treeViewSach.Nodes.Add(nodeSach);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không có sách nào thỏa mãn điều kiện tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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




        private void LoadSachTheoTenSach(string keyword)
        {
            try
            {
                // Kiểm tra nếu từ khóa trống hoặc null thì không thực hiện tìm kiếm
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    MessageBox.Show("Vui lòng nhập tên sách cần tìm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Câu truy vấn SQL để tìm sách có tên gần giống với từ khóa
                    string query = "SELECT TenSach, NamXuatBan, DonGiaBan FROM SACH WHERE TenSach LIKE @Keyword";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");  // Sử dụng LIKE với từ khóa

                    SqlDataReader reader = cmd.ExecuteReader();

                    // Xóa TreeView trước khi thêm các kết quả mới
                    treeViewSach.Nodes.Clear();

                    // Kiểm tra nếu có kết quả
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string tenSach = reader["TenSach"].ToString();
                            int namXuatBan = Convert.ToInt32(reader["NamXuatBan"]);
                            double donGiaBan = Convert.ToDouble(reader["DonGiaBan"]);

                            // Tạo chuỗi hiển thị trong TreeNode
                            string nodeText = $"{tenSach} - Năm xuất bản: {namXuatBan} - Giá bán: {donGiaBan.ToString("C")}";

                            // Tạo node sách với thông tin
                            TreeNode nodeSach = new TreeNode(nodeText)
                            {
                                Tag = new { TenSach = tenSach, NamXuatBan = namXuatBan, DonGiaBan = donGiaBan }  // Lưu thêm thông tin vào Tag
                            };

                            // Thêm node vào TreeView
                            treeViewSach.Nodes.Add(nodeSach);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không có sách nào thỏa mãn điều kiện tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi nếu có
                MessageBox.Show("Lỗi khi tìm kiếm sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBoxTLTL_NAM_Click(object sender, EventArgs e)
        {
            
                try
                {
                    // Lấy mã thể loại từ ComboBox
                    string maTheLoai = comboBoxTheLoai.SelectedItem?.ToString();
                    int namXuatBan = 0;

                    if (!int.TryParse(textBoxNamXB.Text, out namXuatBan) || namXuatBan <= 0)
                    {
                        MessageBox.Show("Vui lòng nhập năm xuất bản hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (string.IsNullOrEmpty(maTheLoai))
                    {
                        MessageBox.Show("Vui lòng chọn mã thể loại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string query = @"
                SELECT MaSach, TenSach, SoLuongTon, DonGiaBan 
                FROM SACH
                WHERE MaTheLoai = @MaTheLoai AND NamXuatBan < @NamXuatBan";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);
                        cmd.Parameters.AddWithValue("@NamXuatBan", namXuatBan);

                        SqlDataReader reader = cmd.ExecuteReader();

                        treeViewSach.Nodes.Clear();

                        if (reader.HasRows)
                        {
                            TreeNode nodeTheLoai = new TreeNode("Thể loại: " + maTheLoai);
                            treeViewSach.Nodes.Add(nodeTheLoai);

                            while (reader.Read())
                            {
                                string maSach = reader["MaSach"].ToString();
                                string tenSach = reader["TenSach"].ToString();
                                int soLuongTon = Convert.ToInt32(reader["SoLuongTon"]);
                                double donGiaBan = Convert.ToDouble(reader["DonGiaBan"]);

                                // Đảm bảo rằng tên sách và các chuỗi văn bản được xử lý đúng Unicode
                                string donGiaBanFormatted = donGiaBan.ToString("C0", CultureInfo.GetCultureInfo("vi-VN"));
                                string nodeText = $"{tenSach} - Số lượng tồn: {soLuongTon} - Đơn giá: {donGiaBanFormatted}";

                                TreeNode nodeSach = new TreeNode(nodeText);

                                // Thêm node con vào node thể loại
                                nodeTheLoai.Nodes.Add(nodeSach);
                            }

                            treeViewSach.ExpandAll();
                        }
                        else
                        {
                            MessageBox.Show("Không có sách nào thuộc thể loại này và năm xuất bản nhỏ hơn " + namXuatBan, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            

        }

        private void treeViewSach_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

           
             
            

        }

        private void textBoxSach_TextChanged(object sender, EventArgs e)
        {
            
                string keyword = textBoxSach.Text.Trim(); // Lấy giá trị từ TextBox và loại bỏ khoảng trắng thừa

                // Kiểm tra nếu người dùng không nhập gì thì không tìm kiếm
                if (string.IsNullOrEmpty(keyword))
                {
                    treeViewSach.Nodes.Clear(); // Xóa TreeView nếu không có từ khóa
                    return;
                }

                // Tìm sách với từ khóa trong TextBox
                FilterSach(keyword);
            

        }

        private void FilterSach(string keyword)
        {
            treeViewSach.Nodes.Clear();  // Xóa toàn bộ các node trong TreeView trước khi thêm lại

            // Giả sử bạn có một danh sách các sách được tải vào trước đó
            List<TreeNode> allBooksNodes = GetAllBooksNodes();  // Lấy tất cả các node sách

            // Duyệt qua danh sách tất cả các node và thêm vào TreeView nếu tên sách chứa từ khóa
            foreach (var node in allBooksNodes)
            {
                if (node.Text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    treeViewSach.Nodes.Add(node);  // Thêm vào TreeView nếu tên sách khớp với từ khóa
                }
            }
        }

        private List<TreeNode> GetAllBooksNodes()
        {
            List<TreeNode> allNodes = new List<TreeNode>();

            try
            {
                // Kết nối cơ sở dữ liệu
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Truy vấn lấy tất cả các tên sách và thông tin cần thiết
                    string query = "SELECT MaSach, TenSach, SoLuongTon, DonGiaBan FROM Sach";  // Truy vấn lấy thông tin sách
                    SqlCommand cmd = new SqlCommand(query, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())  // Đảm bảo SqlDataReader được đóng đúng cách
                    {
                        // Lặp qua tất cả kết quả truy vấn và thêm vào TreeNode
                        while (reader.Read())
                        {
                            string maSach = reader["MaSach"].ToString();
                            string tenSach = reader["TenSach"].ToString();
                            int soLuongTon = reader["SoLuongTon"] != DBNull.Value ? Convert.ToInt32(reader["SoLuongTon"]) : 0;
                            double donGiaBan = reader["DonGiaBan"] != DBNull.Value ? Convert.ToDouble(reader["DonGiaBan"]) : 0;

                            // Tạo TreeNode cho mỗi sách với các thông tin cần thiết
                            string nodeText = $"{tenSach} - Số lượng tồn: {soLuongTon} - Giá bán: {donGiaBan:C}";  // Hiển thị thông tin

                            TreeNode treeNode = new TreeNode(nodeText);  // Tạo TreeNode với text là tên sách và các thông tin khác
                            treeNode.Tag = maSach;  // Lưu thông tin MaSach vào Tag nếu cần

                            allNodes.Add(treeNode);  // Thêm node vào danh sách
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return allNodes;
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        // Lập phiếu nhập 
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form form = new Form();
            UC_NhapSach ucFormNS = new UC_NhapSach(tendangnhap, mahd);

            // Thêm UserControl vào Form
            form.Controls.Add(ucFormNS);

            // Đặt kích thước Form phù hợp
            form.Size = ucFormNS.Size;
            ucFormNS.Dock = DockStyle.Fill;

            // Hiển thị Form
            form.ShowDialog(); // Hoặc dùng form.Show() nếu không cần chờ


        }

        // Lập hoá đơn
        private void pictureBox3_Click(object sender, EventArgs e)
        {
           
            Form form = new Form();
            UC_HD ucForm = new UC_HD(tendangnhap, mahd);

            // Thêm UserControl vào Form
            form.Controls.Add(ucForm);

            // Đặt kích thước Form phù hợp
            form.Size = ucForm.Size;
            ucForm.Dock = DockStyle.Fill;

            // Hiển thị Form
            form.ShowDialog(); // Hoặc dùng form.Show() nếu không cần chờ
        

    }
}
}





