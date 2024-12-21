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

namespace NHOM
{
    public partial class DOANHTHU : Form
    {
        private readonly string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS;Initial Catalog=QL_SACH_DA;User ID=sa;Password='123'";

        public DOANHTHU()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
         
            // Lấy ngày được chọn từ MonthCalendar
            DateTime selectedDate = e.Start.Date; // Lấy ngày bắt đầu từ phạm vi ngày chọn

            // Chuỗi kết nối cơ sở dữ liệu (cập nhật theo thông tin của bạn)
         
            // Câu truy vấn SQL để lấy tổng tiền trong ngày được chọn
            string query = @"
        SELECT SUM(TongTien) AS TotalRevenue
        FROM HOADON
        WHERE CAST(NgayHD AS DATE) = @SelectedDate";

            // Kết nối tới cơ sở dữ liệu và thực hiện truy vấn
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Thêm tham số cho ngày được chọn
                    command.Parameters.AddWithValue("@SelectedDate", selectedDate);

                    try
                    {
                        connection.Open();

                        // Thực thi truy vấn và lấy kết quả
                        object result = command.ExecuteScalar();

                        // Kiểm tra kết quả và hiển thị vào TextBox
                        if (result != DBNull.Value)
                        {
                            txtTongTienNgay.Text = Convert.ToDecimal(result).ToString("N2"); // Hiển thị định dạng số
                        }
                        else
                        {
                            txtTongTienNgay.Text = "0.00"; // Không có dữ liệu cho ngày này
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
                    }
                }
            }
        

    }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query = @"select TenTheLoai from THELOAI ";
            string selectedTL = comboBox1.SelectedItem.ToString();
            // Kết nối tới cơ sở dữ liệu và thực hiện truy vấn
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Thêm tham số cho ngày được chọn
                    command.Parameters.AddWithValue("@selectedTL", selectedTL);

                    try
                    {
                        connection.Open();

                        // Thực thi truy vấn và lấy kết quả
                        object result = command.ExecuteScalar();

                       
                        
                            comboBox1.SelectedItem = selectedTL;
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
                    }
                }
            }

        }
    }
}
