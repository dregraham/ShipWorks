namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    partial class SingleAccountMarketingDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SingleAccountMarketingDlg));
            this.close = new System.Windows.Forms.Button();
            this.expeditedDiscountControl = new ShipWorks.Shipping.Carriers.Postal.Usps.UspsAutomaticDiscountControl();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(373, 186);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 13;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // expeditedDiscountControl
            // 
            this.expeditedDiscountControl.DiscountText = resources.GetString("expeditedDiscountControl.DiscountText");
            this.expeditedDiscountControl.HeaderText = "Single Account Marketing Header Goes Here";
            this.expeditedDiscountControl.Location = new System.Drawing.Point(13, 12);
            this.expeditedDiscountControl.Name = "expeditedDiscountControl";
            this.expeditedDiscountControl.Size = new System.Drawing.Size(435, 164);
            this.expeditedDiscountControl.TabIndex = 14;
            this.expeditedDiscountControl.UseExpeitedOptionText = "TBD: the intent is to use this to capture/record whether the user opted into the " +
    "single account";
            // 
            // SingleAccountMarketingDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 221);
            this.Controls.Add(this.expeditedDiscountControl);
            this.Controls.Add(this.close);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SingleAccountMarketingDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Activate Postage Discount";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button close;
        private UspsAutomaticDiscountControl expeditedDiscountControl;
    }
}