namespace ShipWorks.Shipping.Insurance
{
    partial class InsuranceBenefitsDlg
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
            this.close = new System.Windows.Forms.Button();
            this.labelVisit = new System.Windows.Forms.Label();
            this.labelRates = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.pictureBoxFooter = new System.Windows.Forms.PictureBox();
            this.pictureBoxHeader = new System.Windows.Forms.PictureBox();
            this.enableInsurance = new System.Windows.Forms.CheckBox();
            this.labelInsurance = new System.Windows.Forms.Label();
            this.linkInsurance = new ShipWorks.UI.Controls.LinkControl();
            this.panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxFooter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxHeader)).BeginInit();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.close.Location = new System.Drawing.Point(345, 17);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 0;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // labelVisit
            // 
            this.labelVisit.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelVisit.AutoSize = true;
            this.labelVisit.Location = new System.Drawing.Point(11, 22);
            this.labelVisit.Name = "labelVisit";
            this.labelVisit.Size = new System.Drawing.Size(26, 13);
            this.labelVisit.TabIndex = 18;
            this.labelVisit.Text = "Visit";
            // 
            // labelRates
            // 
            this.labelRates.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelRates.AutoSize = true;
            this.labelRates.Location = new System.Drawing.Point(186, 22);
            this.labelRates.Name = "labelRates";
            this.labelRates.Size = new System.Drawing.Size(111, 13);
            this.labelRates.TabIndex = 19;
            this.labelRates.Text = "for rates and policies.";
            // 
            // panelBottom
            // 
            this.panelBottom.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBottom.Controls.Add(this.pictureBoxFooter);
            this.panelBottom.Controls.Add(this.labelRates);
            this.panelBottom.Controls.Add(this.close);
            this.panelBottom.Controls.Add(this.labelVisit);
            this.panelBottom.Controls.Add(this.linkInsurance);
            this.panelBottom.Location = new System.Drawing.Point(0, 46);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(433, 52);
            this.panelBottom.TabIndex = 20;
            // 
            // pictureBoxFooter
            // 
            this.pictureBoxFooter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBoxFooter.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxFooter.Name = "pictureBoxFooter";
            this.pictureBoxFooter.Size = new System.Drawing.Size(433, 10);
            this.pictureBoxFooter.TabIndex = 20;
            this.pictureBoxFooter.TabStop = false;
            // 
            // pictureBoxHeader
            // 
            this.pictureBoxHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBoxHeader.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxHeader.Name = "pictureBoxHeader";
            this.pictureBoxHeader.Size = new System.Drawing.Size(433, 10);
            this.pictureBoxHeader.TabIndex = 21;
            this.pictureBoxHeader.TabStop = false;
            // 
            // enableInsurance
            // 
            this.enableInsurance.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.enableInsurance.ForeColor = System.Drawing.Color.DarkGreen;
            this.enableInsurance.Location = new System.Drawing.Point(14, 13);
            this.enableInsurance.Name = "enableInsurance";
            this.enableInsurance.Size = new System.Drawing.Size(419, 31);
            this.enableInsurance.TabIndex = 22;
            this.enableInsurance.Text = "Insure this package for only $0.54 with ShipWorks Insurance and save $1.80 over t" +
                "he alternative.";
            this.enableInsurance.UseVisualStyleBackColor = true;
            // 
            // labelInsurance
            // 
            this.labelInsurance.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelInsurance.ForeColor = System.Drawing.Color.Green;
            this.labelInsurance.Location = new System.Drawing.Point(14, 15);
            this.labelInsurance.Name = "labelInsurance";
            this.labelInsurance.Size = new System.Drawing.Size(415, 28);
            this.labelInsurance.TabIndex = 23;
            this.labelInsurance.Text = "Insure this package for only $0.54 with ShipWorks Insurance and save $1.80 over t" +
                "he alternative.";
            this.labelInsurance.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // linkInsurance
            // 
            this.linkInsurance.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkInsurance.AutoSize = true;
            this.linkInsurance.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkInsurance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkInsurance.ForeColor = System.Drawing.Color.Blue;
            this.linkInsurance.Location = new System.Drawing.Point(35, 22);
            this.linkInsurance.Name = "linkInsurance";
            this.linkInsurance.Size = new System.Drawing.Size(155, 13);
            this.linkInsurance.TabIndex = 17;
            this.linkInsurance.Text = "www.shipworks.com/insurance";
            this.linkInsurance.Click += new System.EventHandler(this.OnClickRatesPolicies);
            // 
            // InsuranceBenefitsDlg
            // 
            this.AcceptButton = this.close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(433, 100);
            this.Controls.Add(this.labelInsurance);
            this.Controls.Add(this.enableInsurance);
            this.Controls.Add(this.pictureBoxHeader);
            this.Controls.Add(this.panelBottom);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InsuranceBenefitsDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ShipWorks Insurance";
            this.Load += new System.EventHandler(this.OnLoad);
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxFooter)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxHeader)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button close;
        private UI.Controls.LinkControl linkInsurance;
        private System.Windows.Forms.Label labelVisit;
        private System.Windows.Forms.Label labelRates;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.PictureBox pictureBoxFooter;
        private System.Windows.Forms.PictureBox pictureBoxHeader;
        private System.Windows.Forms.CheckBox enableInsurance;
        private System.Windows.Forms.Label labelInsurance;
    }
}