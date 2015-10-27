namespace ShipWorks.Shipping.Carriers.Amazon
{
    partial class AmazonCarrierTermsAndConditionsNotAcceptedFootnoteControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AmazonCarrierTermsAndConditionsNotAcceptedFootnoteControl));
            this.carrierNamesMessageLabel = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.howToFixMessageLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // carrierNamesMessageLabel
            // 
            this.carrierNamesMessageLabel.AutoSize = true;
            this.carrierNamesMessageLabel.Location = new System.Drawing.Point(25, 7);
            this.carrierNamesMessageLabel.Name = "carrierNamesMessageLabel";
            this.carrierNamesMessageLabel.Size = new System.Drawing.Size(263, 13);
            this.carrierNamesMessageLabel.TabIndex = 9;
            this.carrierNamesMessageLabel.Text = "Terms and conditions have not been accepted for {0}.";
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(4, 5);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(16, 16);
            this.pictureBox.TabIndex = 11;
            this.pictureBox.TabStop = false;
            // 
            // howToFixMessageLabel
            // 
            this.howToFixMessageLabel.AutoSize = true;
            this.howToFixMessageLabel.Location = new System.Drawing.Point(25, 24);
            this.howToFixMessageLabel.Name = "howToFixMessageLabel";
            this.howToFixMessageLabel.Size = new System.Drawing.Size(330, 13);
            this.howToFixMessageLabel.TabIndex = 12;
            this.howToFixMessageLabel.Text = "Accepting these terms and conditions can be done through Amazon.";
            // 
            // AmazonCarrierTermsAndConditionsNotAcceptedFootnoteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.howToFixMessageLabel);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.carrierNamesMessageLabel);
            this.Name = "AmazonCarrierTermsAndConditionsNotAcceptedFootnoteControl";
            this.Size = new System.Drawing.Size(396, 44);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label carrierNamesMessageLabel;
        private System.Windows.Forms.Label howToFixMessageLabel;
    }
}
