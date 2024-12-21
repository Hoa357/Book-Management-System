using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace NHOM.SACH
{
    public partial class UC_InfoBook : UserControl
    {
        // Chuỗi kết nối đến cơ sở dữ liệu
        string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=QL_Sach_DA;User ID=sa;Password='123'";
        string tendangnhap;
        public UC_InfoBook(string tendangnhap)
        {
            InitializeComponent();
            this.tendangnhap = tendangnhap;
        }

        // Phương thức load dữ liệu vào TreeView
        // Phương thức load dữ liệu vào TreeView
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
                    treeView1.Nodes.Clear();  // Xóa tất cả các mục hiện tại trong TreeView

                    while (reader.Read())
                    {
                        string maTheLoai = reader["MaTheLoai"].ToString();
                        string tenTheLoai = reader["TenTheLoai"].ToString();

                        // Tạo một node mới cho mỗi MaTheLoai
                        TreeNode treeNode = new TreeNode(tenTheLoai.Normalize(NormalizationForm.FormC));

                        // Lấy sách thuộc thể loại này
                        LoadBooksForCategory(treeNode, maTheLoai);

                        // Thêm node vào TreeView
                        treeView1.Nodes.Add(treeNode);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu vào TreeView: " + ex.Message);
                }
            }
        }

        // Phương thức load sách cho một thể loại
        private void LoadBooksForCategory(TreeNode theLoaiNode, string maTheLoai)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Lấy dữ liệu sách thuộc thể loại
                    string query = "SELECT TenSach, NamXuatBan, SoLuongTon, DonGiaBan FROM SACH WHERE MaTheLoai = @MaTheLoai";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string tenSach = reader["TenSach"].ToString();
                        int namXuatBan = Convert.ToInt32(reader["NamXuatBan"]);
                        int soLuong = Convert.ToInt32(reader["SoLuongTon"]);
                        double donGia = Convert.ToDouble(reader["DonGiaBan"]);

                        // Tạo node cho sách
                        string sachInfo = $"{tenSach} - {namXuatBan} - {soLuong} - {donGia:C}";
                        TreeNode sachNode = new TreeNode(sachInfo);

                        // Thêm sách vào node thể loại
                        theLoaiNode.Nodes.Add(sachNode);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu sách: " + ex.Message);
                }
            }
        }

        // Phương thức load dữ liệu vào ComboBox
        private void LoadDataToComboBox()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT MaTheLoai, TenTheLoai FROM TheLoai";  // Lấy MaTheLoai và TenTheLoai
                    SqlCommand cmd = new SqlCommand(query, conn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    comboBoxTL.Items.Clear();  // Xóa tất cả các mục hiện tại trong ComboBox

                    while (reader.Read())
                    {
                        string maTheLoai = reader["MaTheLoai"].ToString();
                        string tenTheLoai = reader["TenTheLoai"].ToString();

                        // Thêm MaTheLoai vào ComboBox
                        comboBoxTL.Items.Add(maTheLoai);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu vào ComboBox: " + ex.Message);
                }
            }
        }

        // Sự kiện load UserControl
        private void UC_InfoBook_Load(object sender, EventArgs e)
        {
            LoadDataToTreeView();  // Gọi phương thức để tải dữ liệu vào TreeView
            LoadDataToComboBox();
            // Gọi phương thức để tải dữ liệu vào ComboBox
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = e.Node;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {



            // Lấy giá trị từ các điều khiển
            string tenSach = txtTenSach.Text;
            DateTime namXuatBan = dateTimePicker1.Value;
            string maTheLoai = comboBoxTL.SelectedItem.ToString();
            int soLuong = int.Parse(numericUpDown1.Text);
            double donGia = double.Parse(textBoxDGia.Text);
            string NhaXuatBan = textBoxNXB.Text;

            // Kiểm tra xem sách đã tồn tại chưa
            bool bookExists = CheckIfBookExists(maTheLoai, tenSach, NhaXuatBan, namXuatBan);

            if (bookExists)
            {
                MessageBox.Show("Sách này đã tồn tại trong cơ sở dữ liệu.");
                return;  // Dừng không thêm sách vào cơ sở dữ liệu
            }

            // Lấy tên thể loại từ mã thể loại (tìm trong cơ sở dữ liệu)
            string tenTheLoai = GetTenTheLoai(maTheLoai);  // Phương thức lấy tên thể loại từ mã thể loại

            if (string.IsNullOrEmpty(tenTheLoai))
            {
                MessageBox.Show("Không tìm thấy thể loại với mã đã chọn.");
                return;
            }

            // Tạo node cho TreeView theo mã thể loại
            TreeNode theLoaiNode = null;
            bool found = false;

            // Kiểm tra xem đã có node thể loại trong TreeView chưa
            foreach (TreeNode node in treeView1.Nodes)
            {
                if (node.Text == tenTheLoai)  // Kiểm tra theo tên thể loại
                {
                    theLoaiNode = node;
                    found = true;
                    break;
                }
            }

            // Nếu chưa có, tạo node mới cho thể loại
            if (!found)
            {
                theLoaiNode = new TreeNode(tenTheLoai); // Tạo node mới với tên thể loại
                treeView1.Nodes.Add(theLoaiNode);
            }

            // Tạo node con cho sách với các thông tin chi tiết
            string sachInfo = $"{maTheLoai} - {tenSach}  - {soLuong} - {donGia}";
            TreeNode sachNode = new TreeNode(sachInfo); // Tạo node với thông tin sách

            // Thêm node con vào node thể loại
            theLoaiNode.Nodes.Add(sachNode);
            treeView1.Refresh();

            // Thêm sách vào cơ sở dữ liệu
            AddBookToDatabase(maTheLoai, tenSach, NhaXuatBan, namXuatBan, soLuong, donGia);


        }

        // Phương thức để lấy tên thể loại từ mã thể loại
        private string GetTenTheLoai(string maTheLoai)
        {
            string tenTheLoai = string.Empty;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT TenTheLoai FROM TheLoai WHERE MaTheLoai = @MaTheLoai";  // Truy vấn tên thể loại
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);  // Thêm tham số mã thể loại

                    object result = cmd.ExecuteScalar();  // Dùng ExecuteScalar để lấy giá trị đơn (Tên thể loại)

                    if (result != null)
                    {
                        tenTheLoai = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lấy tên thể loại: " + ex.Message);
                }
            }

            return tenTheLoai;
        }

        // Phương thức thêm sách vào cơ sở dữ liệu
        // Phương thức thêm sách vào cơ sở dữ liệu
        private void AddBookToDatabase(string maTheLoai, string tenSach, string nhaXuatBan, DateTime namXuatBan, int soLuong, double donGia)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Truy vấn thêm sách và lấy mã sách tự động tạo ra
                    string query = "INSERT INTO SACH (MaTheLoai, TenSach, NhaXuatBan, NamXuatBan, SoLuongTon, DonGiaBan) " +
                                   "VALUES (@MaTheLoai, @TenSach, @NhaXuatBan, @NamXuatBan, @SoLuongTon, @DonGiaBan); " +
                                   "SELECT SCOPE_IDENTITY();";  // Lấy mã sách vừa được thêm

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);
                    cmd.Parameters.AddWithValue("@TenSach", tenSach);
                    cmd.Parameters.AddWithValue("@NhaXuatBan", nhaXuatBan);
                    cmd.Parameters.AddWithValue("@NamXuatBan", namXuatBan.Year);  // Chỉ lấy năm từ DateTime
                    cmd.Parameters.AddWithValue("@SoLuongTon", soLuong);
                    cmd.Parameters.AddWithValue("@DonGiaBan", donGia);

                    // Thực thi câu lệnh và lấy mã sách vừa thêm
                    int maSach = Convert.ToInt32(cmd.ExecuteScalar());

                    MessageBox.Show("Sách đã được thêm vào cơ sở dữ liệu và TreeView.");

                    // Gọi hàm để thêm node sách vào TreeView sau khi đã có mã sách
                    AddBookNodeToTreeView(maSach, maTheLoai, tenSach, soLuong, donGia);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm sách vào cơ sở dữ liệu: " + ex.Message);
                }
            }
        }

        // Phương thức thêm node sách vào TreeView
        private void AddBookNodeToTreeView(int maSach, string maTheLoai, string tenSach, int soLuong, double donGia)
        {
            // Tìm hoặc tạo node thể loại
            string tenTheLoai = GetTenTheLoai(maTheLoai);
            TreeNode theLoaiNode = null;

            foreach (TreeNode node in treeView1.Nodes)
            {
                if (node.Text == tenTheLoai)
                {
                    theLoaiNode = node;
                    break;
                }
            }
            if (theLoaiNode == null)
            {
                theLoaiNode = new TreeNode(tenTheLoai);
                treeView1.Nodes.Add(theLoaiNode);
            }

            // Thêm node sách vào TreeView với mã sách trong Tag
            string sachInfo = $"{maTheLoai} - {tenSach} - {soLuong} - {donGia:C}";
            TreeNode sachNode = new TreeNode(sachInfo) { Tag = maSach };  // Lưu mã sách vào Tag
            theLoaiNode.Nodes.Add(sachNode);
            treeView1.Refresh();
        }


        private bool CheckIfBookExists(string maTheLoai, string tenSach, string nhaXuatBan, DateTime namXuatBan)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM SACH WHERE MaTheLoai = @MaTheLoai " +
                                   "AND TenSach = @TenSach AND NhaXuatBan = @NhaXuatBan " +
                                   "AND NamXuatBan = @NamXuatBan";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);
                    cmd.Parameters.AddWithValue("@TenSach", tenSach);
                    cmd.Parameters.AddWithValue("@NhaXuatBan", nhaXuatBan);
                    cmd.Parameters.AddWithValue("@NamXuatBan", namXuatBan.Year);  // Chỉ lấy năm từ DateTime

                    int count = (int)cmd.ExecuteScalar();  // Kiểm tra số lượng sách thỏa mãn điều kiện

                    return count > 0;  // Nếu có ít nhất 1 sách trùng lặp, trả về true
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi kiểm tra sự tồn tại của sách: " + ex.Message);
                    return false;
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có node nào được chọn không
            if (treeView1.SelectedNode != null)
            {
                TreeNode selectedNode = treeView1.SelectedNode;

                // Kiểm tra xem Tag có phải là null không
                if (selectedNode.Tag == null)
                {
                    MessageBox.Show("Không thể xóa, mã sách không được xác định.");
                    return;
                }

                // Hiển thị hộp thoại xác nhận
                DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa node này?",
                                                            "Xác nhận xóa",
                                                            MessageBoxButtons.YesNo,
                                                            MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.Yes)
                {
                    int maSach = Convert.ToInt32(selectedNode.Tag); // Lấy mã sách từ Tag

                    if (DeleteBookFromDatabase(maSach))
                    {
                        selectedNode.Remove();
                        MessageBox.Show("Node đã được xóa và dữ liệu đã được cập nhật trong cơ sở dữ liệu.");
                    }
                    else
                    {
                        MessageBox.Show("Lỗi khi xóa dữ liệu trong cơ sở dữ liệu.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một node để xóa.");
            }
        }

        // Phương thức để xóa sách khỏi cơ sở dữ liệu theo mã sách
        private bool DeleteBookFromDatabase(int maSach)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM SACH WHERE MaSach = @MaSach";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaSach", maSach);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa dữ liệu từ cơ sở dữ liệu: " + ex.Message);
                    return false;
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
           
               
                Uc_TT_NhapSach cT_1PN = new Uc_TT_NhapSach(tendangnhap);
                cT_1PN.Show();


            
        }
    }
}




    
