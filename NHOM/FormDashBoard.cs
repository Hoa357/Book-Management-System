using NHOM.SACH;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace NHOM
{
    public partial class FormDashBoard : Form
    {
        int PanelWidth;
        bool isCollapsed;
        private string tenDangNhap;

        private int mahd;
   
        public FormDashBoard(string tenDangNhap, int mahd)
        {
            InitializeComponent();
            timerTime.Start();
            PanelWidth = panelLeft.Width;
            isCollapsed = false;
        
            UC_Home UCH = new UC_Home(tenDangNhap,mahd);
            AddControlsToPanel(UCH);

            // Hiển thị tên đăng nhập trong labelWelcome
            labelWelcome.Text = "Xin chào: " + tenDangNhap;
            this.tenDangNhap = tenDangNhap;
      
        }
      
      
        private void FormDashBoard_Load(object sender, EventArgs e)
        {
            UC_ChangeMK ucChangeMK = new UC_ChangeMK(tenDangNhap);
                      // Thêm UC_ChangeMK vào form
            ucChangeMK.Dock = DockStyle.Fill; // Để UC chiếm toàn bộ không gian
            this.Controls.Add(ucChangeMK);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            moveSidePanel(btnSale);
            UC_HD us = new UC_HD(tenDangNhap,mahd);
            AddControlsToPanel(us);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(isCollapsed)
            {
                panelLeft.Width = PanelWidth + 10;
                if(panelLeft.Width > PanelWidth)
                {
                    timer1.Stop();
                    isCollapsed = false;
                    this.Refresh();
                }    

            }    
            else
            {
                panelLeft.Width = panelLeft.Width - 10;
                if(panelLeft.Width <= 75)
                {
                    timer1.Stop();
                    isCollapsed = true;
                    this.Refresh();
                }    
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }
        private void moveSidePanel(Control btn)
        {
            panelSide.Top = btn.Top;
            panelSide.Height = btn.Height;
        }

        private void AddControlsToPanel (Control c)
        {
            c.Dock = DockStyle.Fill;
            panelControls.Controls.Clear();
            panelControls.Controls.Add(c);
        }

            private void btnHome_Click(object sender, EventArgs e)
        {

                moveSidePanel(btnHome);
                UC_Home UCH_Main = new UC_Home(tenDangNhap, mahd);
                AddControlsToPanel(UCH_Main);
           
        }

        private void btnCart_Click(object sender, EventArgs e)
        {
            moveSidePanel(btnCart);
            UC_NhapSach up = new UC_NhapSach(tenDangNhap, mahd);
            AddControlsToPanel(up);
        }

        private void btnMoney_Click(object sender, EventArgs e)
        {
         
            moveSidePanel(btnReport);
            DOANHTHU dt = new DOANHTHU();
            dt.ShowDialog();

        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            moveSidePanel(btnUser);
            UC_ChangeMK changMK = new UC_ChangeMK(tenDangNhap);
            AddControlsToPanel(changMK);

        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            moveSidePanel(btnSetting);
            
            UC_Accountcs account = new UC_Accountcs(tenDangNhap);
            AddControlsToPanel(account);
            
           
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            
        }

        private void timerTime_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            labelTimer.Text = dt.ToString("HH:MM:ss");
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            this.Dispose();

        }

        private void panelControls_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            moveSidePanel(btnSignOut);
            this.Dispose();

        }

        private void labelWelcome_Click(object sender, EventArgs e)
        {
         
        }

        private void thôngTinĐầuSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void panelSide_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void labelTimer_Click(object sender, EventArgs e)
        {

        }
    }
}
