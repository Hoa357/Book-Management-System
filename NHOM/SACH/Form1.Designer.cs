namespace NHOM.KHACHHANG
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtSTK = new System.Windows.Forms.TextBox();
            this.txtTenTaiKhoan = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_template = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_nganhang = new System.Windows.Forms.ComboBox();
            this.buttonQR = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBoxTT = new System.Windows.Forms.TextBox();
            this.buttonXUATHD = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSTK
            // 
            this.txtSTK.Location = new System.Drawing.Point(673, 62);
            this.txtSTK.Name = "txtSTK";
            this.txtSTK.Size = new System.Drawing.Size(190, 26);
            this.txtSTK.TabIndex = 37;
            this.txtSTK.Text = "0721000584901";
            // 
            // txtTenTaiKhoan
            // 
            this.txtTenTaiKhoan.Location = new System.Drawing.Point(188, 157);
            this.txtTenTaiKhoan.Name = "txtTenTaiKhoan";
            this.txtTenTaiKhoan.Size = new System.Drawing.Size(181, 26);
            this.txtTenTaiKhoan.TabIndex = 36;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(75, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 20);
            this.label5.TabIndex = 35;
            this.label5.Text = "Tên tài khoản";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(75, 229);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 20);
            this.label4.TabIndex = 34;
            this.label4.Text = "Số tiền";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(553, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 20);
            this.label3.TabIndex = 32;
            this.label3.Text = "Template";
            // 
            // cb_template
            // 
            this.cb_template.FormattingEnabled = true;
            this.cb_template.Location = new System.Drawing.Point(673, 123);
            this.cb_template.Name = "cb_template";
            this.cb_template.Size = new System.Drawing.Size(121, 28);
            this.cb_template.TabIndex = 31;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(544, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 20);
            this.label2.TabIndex = 30;
            this.label2.Text = "Số tài khoản";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(75, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 20);
            this.label1.TabIndex = 29;
            this.label1.Text = "Ngân hàng";
            // 
            // cb_nganhang
            // 
            this.cb_nganhang.FormattingEnabled = true;
            this.cb_nganhang.Location = new System.Drawing.Point(188, 94);
            this.cb_nganhang.Name = "cb_nganhang";
            this.cb_nganhang.Size = new System.Drawing.Size(278, 28);
            this.cb_nganhang.TabIndex = 28;
            this.cb_nganhang.SelectedIndexChanged += new System.EventHandler(this.cb_nganhang_SelectedIndexChanged);
            // 
            // buttonQR
            // 
            this.buttonQR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(170)))));
            this.buttonQR.ForeColor = System.Drawing.Color.White;
            this.buttonQR.Location = new System.Drawing.Point(131, 374);
            this.buttonQR.Name = "buttonQR";
            this.buttonQR.Size = new System.Drawing.Size(195, 65);
            this.buttonQR.TabIndex = 27;
            this.buttonQR.Text = "TẠO MÃ QR";
            this.buttonQR.UseVisualStyleBackColor = false;
            this.buttonQR.Click += new System.EventHandler(this.buttonQR_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(611, 206);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(383, 368);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 38;
            this.pictureBox1.TabStop = false;
            // 
            // textBoxTT
            // 
            this.textBoxTT.Location = new System.Drawing.Point(188, 229);
            this.textBoxTT.Name = "textBoxTT";
            this.textBoxTT.Size = new System.Drawing.Size(181, 26);
            this.textBoxTT.TabIndex = 39;
            // 
            // buttonXUATHD
            // 
            this.buttonXUATHD.BackColor = System.Drawing.Color.Green;
            this.buttonXUATHD.FlatAppearance.BorderSize = 0;
            this.buttonXUATHD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonXUATHD.Font = new System.Drawing.Font("Century Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonXUATHD.ForeColor = System.Drawing.Color.White;
            this.buttonXUATHD.Location = new System.Drawing.Point(131, 528);
            this.buttonXUATHD.Name = "buttonXUATHD";
            this.buttonXUATHD.Size = new System.Drawing.Size(195, 46);
            this.buttonXUATHD.TabIndex = 40;
            this.buttonXUATHD.Text = "XUẤT HOÁ ĐƠN";
            this.buttonXUATHD.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1092, 652);
            this.Controls.Add(this.buttonXUATHD);
            this.Controls.Add(this.textBoxTT);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtSTK);
            this.Controls.Add(this.txtTenTaiKhoan);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cb_template);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cb_nganhang);
            this.Controls.Add(this.buttonQR);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtSTK;
        private System.Windows.Forms.TextBox txtTenTaiKhoan;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cb_template;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cb_nganhang;
        private System.Windows.Forms.Button buttonQR;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBoxTT;
        private System.Windows.Forms.Button buttonXUATHD;
    }
}