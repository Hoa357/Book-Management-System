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
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NHOM.User
{
    public partial class UC_Home : UserControl
    {
        private string tendangnhap;
        private int ma;
        private int mahd;
        public UC_Home(string tendangnhap, int ma)
        {
            InitializeComponent();
          
            LoadChart(SeriesChartType.Line);
            this.tendangnhap = tendangnhap;// Giả sử bạn có một phương thức LoadChart
            this.ma = ma;
        }

        string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=QL_SACH_DA;User ID=sa;Password='123'";

        private Random random = new Random();
        private List<SeriesChartType> chartTypes = new List<SeriesChartType>
    {
        SeriesChartType.Bar,
        SeriesChartType.Column,
        SeriesChartType.Line,
       
        SeriesChartType.Area,
        SeriesChartType.Spline
    };
        private void LoadChart(SeriesChartType chartType)
        {

            chart1.Series.Clear();
            var           
                series = new Series("Doanh số")
            {
                ChartType = chartType
            };

            // Thêm dữ liệu vào biểu đồ
            series.Points.AddXY("Tháng 1", 50);
            series.Points.AddXY("Tháng 2", 30);
            series.Points.AddXY("Tháng 3", 20);
            series.Points.AddXY("Tháng 4", 10);
            series.Points.AddXY("Tháng 5", 20);
            series.Points.AddXY("Tháng 6", 50);
            series.Points.AddXY("Tháng 7", 30);
            series.Points.AddXY("Tháng 8", 20);
            series.Points.AddXY("Tháng 9", 10);
            series.Points.AddXY("Tháng 10", 20);
            series.Points.AddXY("Tháng 11", 0);
            series.Points.AddXY("Tháng 12", 0);

            chart1.Series.Add(series);

        }

        

       
        private void button2_Click(object sender, EventArgs e)
        {
            // Chọn ngẫu nhiên kiểu dáng từ danh sách
            chart1.Series.Clear();
            int index = random.Next(chartTypes.Count);
            SeriesChartType randomChartType = chartTypes[index];

            // Gọi LoadChart với kiểu dáng ngẫu nhiên
            LoadChart(randomChartType);
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

       

     
      
     
        private void NHAP_SACH_Click(object sender, EventArgs e)
        {
            // Khởi tạo UserControl
            SACH.UC_NhapSach uC_Nhap = new SACH.UC_NhapSach(tendangnhap, ma);

            // Lấy Form chính chứa Panel mong muốn
            Form mainForm = this.FindForm();

            if (mainForm != null)
            {
                // Tìm tất cả các Panel trong Form chính
                Panel mainPanel = mainForm.Controls.OfType<Panel>().FirstOrDefault();

                if (mainPanel != null)
                {
                    // Xóa các UserControl hiện tại trong mainPanel
                    mainPanel.Controls.Clear();

                    // Thêm UserControl mới vào mainPanel
                    mainPanel.Controls.Add(uC_Nhap);

                    // Đảm bảo UserControl chiếm toàn bộ diện tích của mainPanel
                    uC_Nhap.Dock = DockStyle.Fill;
                    uC_Nhap.Visible = true; // Đảm bảo UserControl được hiển thị
                }
                else
                {
                    MessageBox.Show("Không tìm thấy bất kỳ Panel nào trong Form chính.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy Form cha.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HOADON_Click(object sender, EventArgs e)
        {

        }

       private void CHITIET_HD_Click(object sender, EventArgs e)
        {
            // Khởi tạo UserControl
            SACH.UC_HD_CT uC_HD_CT = new SACH.UC_HD_CT(tendangnhap, mahd);

            // Lấy Form chính chứa Panel mong muốn
            Form mainForm = this.FindForm();

            if (mainForm != null)
            {
                // Tìm tất cả các Panel trong Form chính
                Panel mainPanel = mainForm.Controls.OfType<Panel>().FirstOrDefault();

                if (mainPanel != null)
                {
                    // Xóa các UserControl hiện tại trong mainPanel
                    mainPanel.Controls.Clear();

                    // Thêm UserControl mới vào mainPanel
                    mainPanel.Controls.Add(uC_HD_CT);

                    // Đảm bảo UserControl chiếm toàn bộ diện tích của mainPanel
                    uC_HD_CT.Dock = DockStyle.Fill;
                    uC_HD_CT.Visible = true; // Đảm bảo UserControl được hiển thị
                }
                else
                {
                    MessageBox.Show("Không tìm thấy bất kỳ Panel nào trong Form chính.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy Form cha.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
      
        private void CT_NHAP_SACH_Click(object sender, EventArgs e)
        {
            // Khởi tạo UserControl
            SACH.Uc_TT_NhapSach uC_Nhap_CT = new SACH.Uc_TT_NhapSach(tendangnhap);

            // Lấy Form chính chứa Panel mong muốn
            Form mainForm = this.FindForm();

            if (mainForm != null)
            {
                // Tìm tất cả các Panel trong Form chính
                Panel mainPanel = mainForm.Controls.OfType<Panel>().FirstOrDefault();

                if (mainPanel != null)
                {
                    // Xóa các UserControl hiện tại trong mainPanel
                    mainPanel.Controls.Clear();

                    // Thêm UserControl mới vào mainPanel
                    mainPanel.Controls.Add(uC_Nhap_CT);

                    // Đảm bảo UserControl chiếm toàn bộ diện tích của mainPanel
                    uC_Nhap_CT.Dock = DockStyle.Fill;
                    uC_Nhap_CT.Visible = true; // Đảm bảo UserControl được hiển thị
                }
                else
                {
                    MessageBox.Show("Không tìm thấy bất kỳ Panel nào trong Form chính.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy Form cha.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void phieuThuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
            // Khởi tạo UserControl
            KHACHHANG.UC_PhieuTien phieuTien = new KHACHHANG.UC_PhieuTien();

            // Lấy Form chính chứa Panel mong muốn
            Form mainForm = this.FindForm();

            if (mainForm != null)
            {
                // Tìm tất cả các Panel trong Form chính
                Panel mainPanel = mainForm.Controls.OfType<Panel>().FirstOrDefault();

                if (mainPanel != null)
                {
                    // Xóa các UserControl hiện tại trong mainPanel
                    mainPanel.Controls.Clear();

                    // Thêm UserControl mới vào mainPanel
                    mainPanel.Controls.Add(phieuTien);

                    // Đảm bảo UserControl chiếm toàn bộ diện tích của mainPanel
                    phieuTien.Dock = DockStyle.Fill;
                    phieuTien.Visible = true; // Đảm bảo UserControl được hiển thị
                }
                else
                {
                    MessageBox.Show("Không tìm thấy bất kỳ Panel nào trong Form chính.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy Form cha.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            */

            
        }

        private void tìmKiếmToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
                // Khởi tạo UserControl
                SACH.UC_DSSACH uC_ttSach = new SACH.UC_DSSACH(tendangnhap, mahd);

                // Lấy Form chính chứa Panel mong muốn
                Form mainForm = this.FindForm();

                if (mainForm != null)
                {
                    // Tìm tất cả các Panel trong Form chính
                    Panel mainPanel = mainForm.Controls.OfType<Panel>().FirstOrDefault();

                    if (mainPanel != null)
                    {
                        // Xóa các UserControl hiện tại trong mainPanel
                        mainPanel.Controls.Clear();

                        // Thêm UserControl mới vào mainPanel
                        mainPanel.Controls.Add(uC_ttSach);

                        // Đảm bảo UserControl chiếm toàn bộ diện tích của mainPanel
                        uC_ttSach.Dock = DockStyle.Fill;
                        uC_ttSach.Visible = true; // Đảm bảo UserControl được hiển thị
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy bất kỳ Panel nào trong Form chính.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy Form cha.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            

        }

        private void nhậpSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void label_accountK_Click(object sender, EventArgs e)
        {
           
                // Kết nối tới cơ sở dữ liệu và lấy số lượng khách hàng
                int soLuongKhachHang = GetCustomerCount();

              
         
        }

        private int GetTotalBooksSoldToday()
        {
            int totalBooksSold = 0;

            // Câu truy vấn SQL
            string query = @"
        SELECT 
ISNULL(SUM(CT.SoLuong), 0)  AS TongSoLuongSachDaBan          


        FROM 
            [QL_SACH_DA].[dbo].[CT_HoaDon] AS CT
        JOIN 
            [QL_SACH_DA].[dbo].[HOADON] AS HD
            ON CT.MaHoaDon = HD.SoHD
        WHERE 
            CAST(HD.NgayHD AS DATE) = CAST(GETDATE() AS DATE)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    totalBooksSold = (int?)cmd.ExecuteScalar() ?? 0;  // Trả về 0 nếu không có kết quả
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy số lượng sách đã bán trong ngày: " + ex.Message);
            }

            return totalBooksSold;
        }


        private int GetTotalBooksReceivedToday()
        {
            int totalBooksReceived = 0;

            // Câu truy vấn SQL
            string query = @"
            SELECT 
    ISNULL(SUM(CT.SoLuongNhap), 0) AS TongSoLuongSachNhap
FROM 
    [QL_SACH_DA].[dbo].[CT_NHAPSACH] AS CT
JOIN 
    [QL_SACH_DA].[dbo].[NHAPSACH] AS NS
    ON CT.MaNS = NS.MaNS
WHERE 
    NS.NgayNS = GETDATE()";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    totalBooksReceived = (int?)cmd.ExecuteScalar() ?? 0;  // Trả về 0 nếu không có kết quả
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy số lượng sách nhập trong ngày: " + ex.Message);
            }

            return totalBooksReceived;
        }

        private void UC_Home_Load(object sender, EventArgs e)
        {
            // Kết nối tới cơ sở dữ liệu và lấy số lượng khách hàng
            label_accountK.Text  = GetCustomerCount().ToString();
            labelSachDaMua.Text = GetTotalBooksSoldToday().ToString();
            labelSLSACHNHAP.Text = GetTotalBooksReceivedToday().ToString();

        }

        // Hàm đếm số lượng khách hàng
        private int GetCustomerCount()
        {
            int customerCount = 0;

            // Câu truy vấn SQL đếm số lượng khách hàng
            string query = "SELECT COUNT(*)  FROM KHACHHANG";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))  // connectionString là chuỗi kết nối của bạn
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    customerCount = (int)cmd.ExecuteScalar();  // Dùng ExecuteScalar để lấy kết quả số lượng
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đếm khách hàng: " + ex.Message);
            }

            return customerCount;
        }

        private void thôngTinKháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // Khởi tạo UserControl
            KHACHHANG.UC_TT_KH uC_kh = new KHACHHANG.UC_TT_KH(tendangnhap);

                // Lấy Form chính chứa Panel mong muốn
                Form mainForm = this.FindForm();

                if (mainForm != null)
                {
                    // Tìm tất cả các Panel trong Form chính
                    Panel mainPanel = mainForm.Controls.OfType<Panel>().FirstOrDefault();

                    if (mainPanel != null)
                    {
                        // Xóa các UserControl hiện tại trong mainPanel
                        mainPanel.Controls.Clear();

                        // Thêm UserControl mới vào mainPanel
                        mainPanel.Controls.Add(uC_kh);

                    // Đảm bảo UserControl chiếm toàn bộ diện tích của mainPanel
                    uC_kh.Dock = DockStyle.Fill;
                        uC_kh.Visible = true; // Đảm bảo UserControl được hiển thị
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy bất kỳ Panel nào trong Form chính.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy Form cha.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            


        }

        private void xemDanhSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
           SACH.UC_UpdatesSach uC_UPDATES = new SACH.UC_UpdatesSach(tendangnhap, mahd);
          

         
              
                // Lấy Form chính chứa Panel mong muốn
                Form mainForm = this.FindForm();

                if (mainForm != null)
                {
                    // Tìm tất cả các Panel trong Form chính
                    Panel mainPanel = mainForm.Controls.OfType<Panel>().FirstOrDefault();

                    if (mainPanel != null)
                    {
                        // Xóa các UserControl hiện tại trong mainPanel
                        mainPanel.Controls.Clear();

                        // Thêm UserControl mới vào mainPanel
                        mainPanel.Controls.Add(uC_UPDATES);

                    // Đảm bảo UserControl chiếm toàn bộ diện tích của mainPanel
                    uC_UPDATES.Dock = DockStyle.Fill;
                        uC_UPDATES.Visible = true; // Đảm bảo UserControl được hiển thị
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy bất kỳ Panel nào trong Form chính.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy Form cha.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            

        }

        private void lậpPhiếuThuTiềnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

               


            

        }

        private void báoCáoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Khởi tạo UserControl
            User.UC_HD uC_kh = new User.UC_HD(tendangnhap, ma);

            // Lấy Form chính chứa Panel mong muốn
            Form mainForm = this.FindForm();

            if (mainForm != null)
            {
                // Tìm tất cả các Panel trong Form chính
                Panel mainPanel = mainForm.Controls.OfType<Panel>().FirstOrDefault();

                if (mainPanel != null)
                {
                    // Xóa các UserControl hiện tại trong mainPanel
                    mainPanel.Controls.Clear();

                    // Thêm UserControl mới vào mainPanel
                    mainPanel.Controls.Add(uC_kh);

                    // Đảm bảo UserControl chiếm toàn bộ diện tích của mainPanel
                    uC_kh.Dock = DockStyle.Fill;
                    uC_kh.Visible = true; // Đảm bảo UserControl được hiển thị
                }
                else
                {
                    MessageBox.Show("Không tìm thấy bất kỳ Panel nào trong Form chính.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy Form cha.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void quảnLýToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Khởi tạo UserControl
            User.UC_Accountcs uC_account = new User.UC_Accountcs(tendangnhap);

            // Lấy Form chính chứa Panel mong muốn
            Form mainForm = this.FindForm();

            if (mainForm != null)
            {
                // Tìm tất cả các Panel trong Form chính
                Panel mainPanel = mainForm.Controls.OfType<Panel>().FirstOrDefault();

                if (mainPanel != null)
                {
                    // Xóa các UserControl hiện tại trong mainPanel
                    mainPanel.Controls.Clear();

                    // Thêm UserControl mới vào mainPanel
                    mainPanel.Controls.Add(uC_account);

                    // Đảm bảo UserControl chiếm toàn bộ diện tích của mainPanel
                    uC_account.Dock = DockStyle.Fill;
                    uC_account.Visible = true; // Đảm bảo UserControl được hiển thị
                }
                else
                {
                    MessageBox.Show("Không tìm thấy bất kỳ Panel nào trong Form chính.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy Form cha.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }

}
