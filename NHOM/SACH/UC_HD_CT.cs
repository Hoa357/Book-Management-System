using NHOM.User;
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

namespace NHOM.SACH
{
    public partial class UC_HD_CT : UserControl
    {
        String tendangnhap;
        int mahd;
        string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=QL_Sach_DA;User ID=sa;Password='123'";
      
        public UC_HD_CT(string tendangnhap, int mahd)
        {
            InitializeComponent();
            this.tendangnhap = tendangnhap;
            this.mahd = mahd;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

            Form form = new Form();
            UC_HD ucForm2 = new UC_HD(tendangnhap, mahd);

            // Thêm UserControl vào Form
            form.Controls.Add(ucForm2);

            // Đặt kích thước Form phù hợp
            form.Size = ucForm2.Size;
            ucForm2.Dock = DockStyle.Fill;

            // Hiển thị Form
            form.ShowDialog(); // Hoặc dùng form.Show() nếu không cần chờ

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            // Lấy mã hóa đơn được chọn từ comboBox1
            string selectedMaHoaDon = comboBox1.SelectedItem.ToString();

           

            // Tạo câu truy vấn kết hợp hai bảng để tìm kiếm thông tin theo mã hóa đơn
            string query = @"
        SELECT 
            hd.SoHD AS [Số Hóa Đơn], 
            hd.MaKH AS [Mã Khách Hàng], 
            hd.NgayHD AS [Ngày Hóa Đơn], 
            hd.TongTien AS [Tổng Tiền], 
            hd.SoTienTra AS [Số Tiền Trả], 
            hd.MaNguoiNhap AS [Mã Người Nhập], 
            hd.MaGiam AS [Mã Giảm Giá], 
            cthd.MaSach AS [Mã Sách], 
            cthd.SoLuong AS [Số Lượng], 
            cthd.DonGia AS [Đơn Giá], 
            cthd.ThanhTien AS [Thành Tiền]
        FROM HOADON hd
        INNER JOIN CT_HoaDon cthd ON hd.SoHD = cthd.MaHoaDon
        WHERE hd.SoHD = @MaHoaDon";

            // Tạo một DataTable để chứa dữ liệu
            DataTable dataTable = new DataTable();

            try
            {
                // Sử dụng SqlConnection và SqlDataAdapter để nạp dữ liệu
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Gán giá trị tham số
                        command.Parameters.AddWithValue("@MaHoaDon", selectedMaHoaDon);

                        // Sử dụng SqlDataAdapter để điền dữ liệu vào DataTable
                        SqlDataAdapter adapter = new SqlDataAdapter(command);

                        // Mở kết nối
                        connection.Open();

                        // Điền dữ liệu vào DataTable
                        adapter.Fill(dataTable);

                        // Đặt DataTable làm nguồn dữ liệu của dataGridView1
                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi nếu có
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        
        

    

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void UC_HD_CT_Load(object sender, EventArgs e)
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
    }
}

