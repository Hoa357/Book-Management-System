using NHOM.SACH;
using NHOM.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NHOM
{
    public partial class Form_KH : Form
    {
        int PanelWidth;
        bool isCollapsed;
        string tenDangNhap;
        private int ma;
        public Form_KH(string tenDangNhap)
        {
            InitializeComponent();
            
            timerTime.Start();
            PanelWidth = panelLeft.Width;
            isCollapsed = false;
            UC_Home uC_Home = new UC_Home(tenDangNhap, ma);
            AddControlsToPanel(uC_Home);
            // Hiển thị tên đăng nhập trong labelWelcome
            labelWelcome.Text = "Xin chào: " + tenDangNhap;
        }

        private void moveSidePanel(Control btn)
        {
            panelSide.Top = btn.Top;
            panelSide.Height = btn.Height;
        }

        private void AddControlsToPanel(Control c)
        {

            if (c != null)
            {
                c.Dock = DockStyle.Fill;  // Đảm bảo Control chiếm toàn bộ không gian của panel
                panelCus.Controls.Clear();  // Xóa các control hiện có
                panelCus.Controls.Add(c);  // Thêm control mới vào panel
            }
            else
            {
                MessageBox.Show("Control không hợp lệ!");
            }


        }

        private void Form_KH_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (isCollapsed)
            {
                panelLeft.Width = PanelWidth + 10;
                if (panelLeft.Width > PanelWidth)
                {
                    timer1.Stop();
                    isCollapsed = false;
                    this.Refresh();
                }

            }
            else
            {
                panelLeft.Width = panelLeft.Width - 10;
                if (panelLeft.Width <= 75)
                {
                    timer1.Stop();
                    isCollapsed = true;
                    this.Refresh();
                }
            }
        }

        private void timerTime_Tick(object sender, EventArgs e)
        {
           
                DateTime dt = DateTime.Now;
                labelTimer.Text = dt.ToString("HH:MM:ss");
            
        }

        private void labelTimer_Click(object sender, EventArgs e)
        {

            DateTime dt = DateTime.Now;
            labelTimer.Text = dt.ToString("HH:mm:ss");
        }

        private void buttonSale_Click(object sender, EventArgs e)
        {
            moveSidePanel(buttonSale);
            UC_HD us = new UC_HD(tenDangNhap, ma);
            AddControlsToPanel(us);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            moveSidePanel(buttonHome);
        }

        private void btnCart_Click(object sender, EventArgs e)
        {
            moveSidePanel(ButtonPur);
        }

        private void buttonUser_Click(object sender, EventArgs e)
        {
            moveSidePanel(buttonUser);

            UC_Accountcs account = new UC_Accountcs(tenDangNhap);
            AddControlsToPanel(account);

        }

        private void buttonSignOut_Click(object sender, EventArgs e)
        {
            moveSidePanel(buttonSignOut);
            this.Dispose();
        }

        private void labelWelcome_Click(object sender, EventArgs e)
        {

          
        }
    }
}
