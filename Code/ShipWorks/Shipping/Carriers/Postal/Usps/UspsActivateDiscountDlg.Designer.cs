namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    partial class UspsActivateDiscountDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UspsActivateDiscountDlg));
            this.close = new System.Windows.Forms.Button();
            this.convertToExpeditedControl = new ShipWorks.Shipping.Carriers.Postal.Usps.UspsConvertAccountToExpeditedControl();
            this.signUpForExpeditedControl = new ShipWorks.Shipping.Carriers.Postal.Usps.UspsAutomaticDiscountControl();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(353, 447);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 1;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // convertToExpeditedControl
            // 
            this.convertToExpeditedControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.convertToExpeditedControl.DescriptionText = resources.GetString("convertToExpeditedControl.DescriptionText");
            this.convertToExpeditedControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.convertToExpeditedControl.LinkText = "Click here to add these discounted rates from IntuiShip through your existing Sta" +
    "mps.com account at no additional cost.";
            this.convertToExpeditedControl.Location = new System.Drawing.Point(3, 225);
            this.convertToExpeditedControl.Name = "convertToExpeditedControl";
            this.convertToExpeditedControl.Size = new System.Drawing.Size(425, 193);
            this.convertToExpeditedControl.TabIndex = 2;
            // 
            // signUpForExpeditedControl
            // 
            this.signUpForExpeditedControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.signUpForExpeditedControl.DiscountText = resources.GetString("signUpForExpeditedControl.DiscountText");
            this.signUpForExpeditedControl.HeaderText = "Postage Discount";
            this.signUpForExpeditedControl.Location = new System.Drawing.Point(3, 12);
            this.signUpForExpeditedControl.Name = "signUpForExpeditedControl";
            this.signUpForExpeditedControl.Size = new System.Drawing.Size(425, 182);
            this.signUpForExpeditedControl.TabIndex = 0;
            this.signUpForExpeditedControl.UseExpeditedOptionText = "Automatically save postage costs on domestic and international Priority and Expre" +
    "ss shipments:";
            // 
            // UspsActivateDiscountDlg
            // 
            this.AcceptButton = this.close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(443, 478);
            this.Controls.Add(this.convertToExpeditedControl);
            this.Controls.Add(this.close);
            this.Controls.Add(this.signUpForExpeditedControl);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UspsActivateDiscountDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Activate Postage Discount";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button close;
        protected UspsAutomaticDiscountControl signUpForExpeditedControl;
        protected UspsConvertAccountToExpeditedControl convertToExpeditedControl;
    }
}