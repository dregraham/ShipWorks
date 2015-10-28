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
            this.SuspendLayout();
            // 
            // carrierNamesMessageLabel
            // 
            this.carrierNamesMessageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.carrierNamesMessageLabel.AutoSize = true;
            this.carrierNamesMessageLabel.Location = new System.Drawing.Point(12, 9);
            this.carrierNamesMessageLabel.MaximumSize = new System.Drawing.Size(400, 0);
            this.carrierNamesMessageLabel.MinimumSize = new System.Drawing.Size(100, 0);
            this.carrierNamesMessageLabel.Name = "carrierNamesMessageLabel";
            this.carrierNamesMessageLabel.Size = new System.Drawing.Size(246, 13);
            this.carrierNamesMessageLabel.TabIndex = 10;
            this.carrierNamesMessageLabel.Text = "Terms and conditions have not been accepted for:";
            // 
            // howToFixMessageLabel
            // 
            this.howToFixMessageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.howToFixMessageLabel.AutoSize = true;
            this.howToFixMessageLabel.Location = new System.Drawing.Point(12, 84);
            this.howToFixMessageLabel.MaximumSize = new System.Drawing.Size(330, 0);
            this.howToFixMessageLabel.Name = "howToFixMessageLabel";
            this.howToFixMessageLabel.Size = new System.Drawing.Size(330, 13);
            this.howToFixMessageLabel.TabIndex = 13;
            this.howToFixMessageLabel.Text = "Accepting these terms and conditions can be done through Amazon.";
            // 
            // carriersLabel
            // 
            this.carriersLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.carriersLabel.AutoSize = true;
            this.carriersLabel.Location = new System.Drawing.Point(26, 25);
            this.carriersLabel.MaximumSize = new System.Drawing.Size(400, 0);
            this.carriersLabel.MinimumSize = new System.Drawing.Size(100, 0);
            this.carriersLabel.Name = "carriersLabel";
            this.carriersLabel.Size = new System.Drawing.Size(100, 13);
            this.carriersLabel.TabIndex = 14;
            this.carriersLabel.Text = "Carriers";
            // 
            // AmazonCarrierTermsAndConditionsNotAcceptedDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(504, 106);
            this.Controls.Add(this.carriersLabel);
            this.Controls.Add(this.howToFixMessageLabel);
            this.Controls.Add(this.carrierNamesMessageLabel);
            this.Name = "AmazonCarrierTermsAndConditionsNotAcceptedDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Terms and conditions not accepted";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label carrierNamesMessageLabel;
        private System.Windows.Forms.Label howToFixMessageLabel;
        private System.Windows.Forms.Label carriersLabel;
    }
}