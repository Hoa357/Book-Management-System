namespace NHOM.User
{
    partial class KH_TT
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
            this.labelKH1 = new System.Windows.Forms.Label();
            this.txtSDT = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.labelTen = new System.Windows.Forms.Label();
            this.labelMa = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelKH1
            // 
            this.labelKH1.AutoSize = true;
            this.labelKH1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelKH1.ForeColor = System.Drawing.Color.Navy;
            this.labelKH1.Location = new System.Drawing.Point(207, 59);
            this.labelKH1.Name = "labelKH1";
            this.labelKH1.Size = new System.Drawing.Size(377, 32);
            this.labelKH1.TabIndex = 4;
            this.labelKH1.Text = "KHÁCH HÀNG THÂN THIẾT ";
            // 
            // txtSDT
            // 
            this.txtSDT.Location = new System.Drawing.Point(345, 178);
            this.txtSDT.Name = "txtSDT";
            this.txtSDT.Size = new System.Drawing.Size(252, 26);
            this.txtSDT.TabIndex = 45;
            this.txtSDT.TextChanged += new System.EventHandler(this.txtSDT_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Century", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(163, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(160, 27);
            this.label3.TabIndex = 39;
            this.label3.Text = "Số điện thoại";
            // 
            // labelTen
            // 
            this.labelTen.AutoSize = true;
            this.labelTen.Location = new System.Drawing.Point(310, 247);
            this.labelTen.Name = "labelTen";
            this.labelTen.Size = new System.Drawing.Size(13, 20);
            this.labelTen.TabIndex = 46;
            this.labelTen.Text = ".";
            // 
            // labelMa
            // 
            this.labelMa.AutoSize = true;
            this.labelMa.Location = new System.Drawing.Point(325, 286);
            this.labelMa.Name = "labelMa";
            this.labelMa.Size = new System.Drawing.Size(17, 20);
            this.labelMa.TabIndex = 47;
            this.labelMa.Text = "..";
            // 
            // KH_TT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 395);
            this.Controls.Add(this.labelMa);
            this.Controls.Add(this.labelTen);
            this.Controls.Add(this.txtSDT);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelKH1);
            this.Name = "KH_TT";
            this.Text = "KH_TT";
            this.Load += new System.EventHandler(this.KH_TT_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelKH1;
        private System.Windows.Forms.TextBox txtSDT;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelTen;
        private System.Windows.Forms.Label labelMa;
    }
}