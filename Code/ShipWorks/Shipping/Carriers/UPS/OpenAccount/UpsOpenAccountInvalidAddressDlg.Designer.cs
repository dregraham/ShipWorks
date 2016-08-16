namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    partial class UpsOpenAccountInvalidAddressDlg
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
            this.okButton = new System.Windows.Forms.Button();
            this.labelInstructions = new System.Windows.Forms.Label();
            this.addressLine1 = new System.Windows.Forms.Label();
            this.addressLine2 = new System.Windows.Forms.Label();
            this.country = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.labelButtonInstructions = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OkButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(268, 141);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // labelInstructions
            // 
            this.labelInstructions.Location = new System.Drawing.Point(12, 9);
            this.labelInstructions.Name = "labelInstructions";
            this.labelInstructions.Size = new System.Drawing.Size(331, 23);
            this.labelInstructions.TabIndex = 1;
            this.labelInstructions.Text = "label1";
            // 
            // addressLine1
            // 
            this.addressLine1.AutoSize = true;
            this.addressLine1.Location = new System.Drawing.Point(28, 32);
            this.addressLine1.Name = "addressLine1";
            this.addressLine1.Size = new System.Drawing.Size(70, 13);
            this.addressLine1.TabIndex = 2;
            this.addressLine1.Text = "addressLine1";
            // 
            // addressLine2
            // 
            this.addressLine2.AutoSize = true;
            this.addressLine2.Location = new System.Drawing.Point(28, 55);
            this.addressLine2.Name = "addressLine2";
            this.addressLine2.Size = new System.Drawing.Size(70, 13);
            this.addressLine2.TabIndex = 3;
            this.addressLine2.Text = "addressLine2";
            // 
            // country
            // 
            this.country.AutoSize = true;
            this.country.Location = new System.Drawing.Point(28, 77);
            this.country.Name = "country";
            this.country.Size = new System.Drawing.Size(44, 13);
            this.country.TabIndex = 4;
            this.country.Text = "country";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(187, 141);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // labelButtonInstructions
            // 
            this.labelButtonInstructions.Location = new System.Drawing.Point(12, 102);
            this.labelButtonInstructions.Name = "labelButtonInstructions";
            this.labelButtonInstructions.Size = new System.Drawing.Size(331, 33);
            this.labelButtonInstructions.TabIndex = 6;
            this.labelButtonInstructions.Text = "Click OK to Accept the new address, or click Cancel and go back and enter a valid" +
    " address.";
            // 
            // UpsOpenAccountInvalidAddressDlg
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(355, 176);
            this.Controls.Add(this.labelButtonInstructions);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.country);
            this.Controls.Add(this.addressLine2);
            this.Controls.Add(this.addressLine1);
            this.Controls.Add(this.labelInstructions);
            this.Controls.Add(this.okButton);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpsOpenAccountInvalidAddressDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ups Invalid Address";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label labelInstructions;
        private System.Windows.Forms.Label addressLine1;
        private System.Windows.Forms.Label addressLine2;
        private System.Windows.Forms.Label country;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label labelButtonInstructions;
    }
}