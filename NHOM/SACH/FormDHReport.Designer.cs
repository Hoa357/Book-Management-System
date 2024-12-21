namespace NHOM.SACH
{
    partial class FormDHReport
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
            this.dhReportViewer = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // dhReportViewer
            // 
            this.dhReportViewer.LocalReport.ReportEmbeddedResource = "NHOM.SACH.DHSACH.rdlc";
            this.dhReportViewer.Location = new System.Drawing.Point(-2, -4);
            this.dhReportViewer.Name = "dhReportViewer";
            this.dhReportViewer.ServerReport.BearerToken = null;
            this.dhReportViewer.Size = new System.Drawing.Size(960, 586);
            this.dhReportViewer.TabIndex = 0;
            // 
            // FormDHReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 576);
            this.Controls.Add(this.dhReportViewer);
            this.Name = "FormDHReport";
            this.Text = "FormDHReport";
            this.Load += new System.EventHandler(this.FormDHReport_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer dhReportViewer;
    }
}