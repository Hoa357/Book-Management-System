using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NHOM.KHACHHANG
{
    public partial class UC_CAPNHAT_TT : UserControl
    {
        public UC_CAPNHAT_TT()
        {
            InitializeComponent();
        }

        private void UC_CAPNHAT_TT_Load(object sender, EventArgs e)
        {
            
                // Thêm các giá trị "Nam" và "Nữ" vào ComboBox
                comboBoxGT.Items.Add("Nam");
                comboBoxGT.Items.Add("Nữ");

                // Nếu bạn muốn chọn mặc định "Nam", có thể đặt SelectedIndex
                comboBoxGT.SelectedIndex = 0; // Chọn "Nam" là giá trị mặc định
            

        }

        public void ClearTextBox()
        {
            textBoxTenKH.Clear();  // Xóa nội dung của TextBox
            textBoxDC.Clear();
            txtSDT.Clear();
            comboBoxGT.SelectedIndex = 0;

        }
        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        public string GetTenKhachHang()
        {
            return textBoxTenKH.Text;  // Giả sử txtTenKhachHang là TextBox trong UC_CAPNHAT_TT
        }
        
        public string GetGioiTinh()
        {
            return comboBoxGT.SelectedItem.ToString();  // Giả sử comboBoxGT là ComboBox trong UC_CAPNHAT_TT
        }
        public string GetSdt()
        {
            return txtSDT.Text;  
        }

        public string GetDiaChi()
        {
            return textBoxDC.Text;  
        }

        public void SetTenKhachHang(string tenKhachHang)
        {
            textBoxTenKH.Text = tenKhachHang;  // Giả sử txtTenKH là TextBox trong UC_CAPNHAT_TT
        }

        public void SetDiaChi(string diaChi)
        {
            textBoxDC.Text = diaChi;  // Giả sử txtDiaChi là TextBox trong UC_CAPNHAT_TT
        }

        public void SetGioiTinh(string gioiTinh)
        {
            comboBoxGT.SelectedItem = gioiTinh;  // Giả sử cboGioiTinh là ComboBox trong UC_CAPNHAT_TT
        }

        public void SetSdt(string sdt)
        {
            txtSDT.Text = sdt;  // Giả sử txtSdt là TextBox trong UC_CAPNHAT_TT
        }









    }
}
