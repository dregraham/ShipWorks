namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    partial class UspsConvertExistingAccountDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UspsConvertExistingAccountDlg));
            this.close = new System.Windows.Forms.Button();
            this.learnMore = new System.Windows.Forms.LinkLabel();
            this.convertAccountLink = new System.Windows.Forms.LinkLabel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.labelConversionDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(353, 205);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 1;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // learnMore
            // 
            this.learnMore.AutoSize = true;
            this.learnMore.Location = new System.Drawing.Point(29, 119);
            this.learnMore.Name = "learnMore";
            this.learnMore.Size = new System.Drawing.Size(69, 13);
            this.learnMore.TabIndex = 9;
            this.learnMore.TabStop = true;
            this.learnMore.Text = "(Learn more)";
            this.learnMore.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLearnMore);
            // 
            // linkLabel1
            // 
            this.convertAccountLink.Location = new System.Drawing.Point(29, 148);
            this.convertAccountLink.Name = "convertAccountLink";
            this.convertAccountLink.Size = new System.Drawing.Size(378, 32);
            this.convertAccountLink.TabIndex = 8;
            this.convertAccountLink.TabStop = true;
            this.convertAccountLink.Text = "Click here to add these discounted rates from IntuiShip through your existing Stamps.com account at no additional cost.";
            this.convertAccountLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnStartSaving);
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeader.Location = new System.Drawing.Point(10, 12);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(105, 13);
            this.labelHeader.TabIndex = 7;
            this.labelHeader.Text = "Postage Discount";
            // 
            // labelConversionDescription
            // 
            this.labelConversionDescription.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelConversionDescription.Location = new System.Drawing.Point(29, 37);
            this.labelConversionDescription.Name = "labelConversionDescription";
            this.labelConversionDescription.Size = new System.Drawing.Size(378, 82);
            this.labelConversionDescription.TabIndex = 6;
            this.labelConversionDescription.Text = resources.GetString("labelConversionDescription.Text");
            // 
            // UspsConvertExistingAccountDlg
            // 
            this.AcceptButton = this.close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(443, 236);
            this.Controls.Add(this.learnMore);
            this.Controls.Add(this.convertAccountLink);
            this.Controls.Add(this.labelHeader);
            this.Controls.Add(this.labelConversionDescription);
            this.Controls.Add(this.close);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UspsConvertExistingAccountDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Activate Postage Discount";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button close;
        private System.Windows.Forms.LinkLabel learnMore;
        private System.Windows.Forms.LinkLabel convertAccountLink;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Label labelConversionDescription;
    }
}