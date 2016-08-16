namespace ShipWorks.Shipping.Carriers.Amazon
{
    partial class AmazonCarrierTermsAndConditionsNotAcceptedDialog
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
            this.carrierNamesMessageLabel = new System.Windows.Forms.Label();
            this.howToFixMessageLabel = new System.Windows.Forms.Label();
            this.carriersLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.infoPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.infoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // carrierNamesMessageLabel
            // 
            this.carrierNamesMessageLabel.AutoSize = true;
            this.carrierNamesMessageLabel.Location = new System.Drawing.Point(102, 14);
            this.carrierNamesMessageLabel.MaximumSize = new System.Drawing.Size(400, 0);
            this.carrierNamesMessageLabel.MinimumSize = new System.Drawing.Size(100, 0);
            this.carrierNamesMessageLabel.Name = "carrierNamesMessageLabel";
            this.carrierNamesMessageLabel.Size = new System.Drawing.Size(312, 13);
            this.carrierNamesMessageLabel.TabIndex = 10;
            this.carrierNamesMessageLabel.Text = "Terms and conditions have not been accepted for these carriers:";
            // 
            // howToFixMessageLabel
            // 
            this.howToFixMessageLabel.Location = new System.Drawing.Point(102, 46);
            this.howToFixMessageLabel.Name = "howToFixMessageLabel";
            this.howToFixMessageLabel.Size = new System.Drawing.Size(325, 33);
            this.howToFixMessageLabel.TabIndex = 13;
            this.howToFixMessageLabel.Text = "Please accept the terms and conditions for these carriers using your\r\nAmazon Sell" +
    "er Central Account";
            // 
            // carriersLabel
            // 
            this.carriersLabel.AutoSize = true;
            this.carriersLabel.Location = new System.Drawing.Point(126, 25);
            this.carriersLabel.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.carriersLabel.MinimumSize = new System.Drawing.Size(100, 0);
            this.carriersLabel.Name = "carriersLabel";
            this.carriersLabel.Size = new System.Drawing.Size(100, 13);
            this.carriersLabel.TabIndex = 14;
            this.carriersLabel.Text = "Carriers";
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.Location = new System.Drawing.Point(444, 77);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 22);
            this.okButton.TabIndex = 15;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // infoPictureBox
            // 
            this.infoPictureBox.BackgroundImage = global::ShipWorks.Properties.Resources.amazon_large;
            this.infoPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.infoPictureBox.Location = new System.Drawing.Point(12, 46);
            this.infoPictureBox.Name = "infoPictureBox";
            this.infoPictureBox.Size = new System.Drawing.Size(79, 33);
            this.infoPictureBox.TabIndex = 17;
            this.infoPictureBox.TabStop = false;
            // 
            // AmazonCarrierTermsAndConditionsNotAcceptedDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.okButton;
            this.ClientSize = new System.Drawing.Size(527, 104);
            this.ControlBox = false;
            this.Controls.Add(this.howToFixMessageLabel);
            this.Controls.Add(this.infoPictureBox);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.carriersLabel);
            this.Controls.Add(this.carrierNamesMessageLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AmazonCarrierTermsAndConditionsNotAcceptedDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Terms and conditions not accepted";
            ((System.ComponentModel.ISupportInitialize)(this.infoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label carrierNamesMessageLabel;
        private System.Windows.Forms.Label howToFixMessageLabel;
        private System.Windows.Forms.Label carriersLabel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.PictureBox infoPictureBox;
    }
}