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
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.additionalRatesAvailableLabel = new System.Windows.Forms.Label();
            this.openDialogLink = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
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
            // additionalRatesAvailableLabel
            // 
            this.additionalRatesAvailableLabel.AutoSize = true;
            this.additionalRatesAvailableLabel.Location = new System.Drawing.Point(27, 5);
            this.additionalRatesAvailableLabel.Name = "additionalRatesAvailableLabel";
            this.additionalRatesAvailableLabel.Size = new System.Drawing.Size(209, 13);
            this.additionalRatesAvailableLabel.TabIndex = 12;
            this.additionalRatesAvailableLabel.Text = "Additional rates are available. For more info";
            // 
            // openDialogLink
            // 
            this.openDialogLink.AutoSize = true;
            this.openDialogLink.Location = new System.Drawing.Point(242, 5);
            this.openDialogLink.Name = "openDialogLink";
            this.openDialogLink.Size = new System.Drawing.Size(53, 13);
            this.openDialogLink.TabIndex = 13;
            this.openDialogLink.TabStop = true;
            this.openDialogLink.Text = "click here";
            this.openDialogLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.openDialogLink_LinkClicked);
            // 
            // AmazonCarrierTermsAndConditionsNotAcceptedFootnoteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.openDialogLink);
            this.Controls.Add(this.additionalRatesAvailableLabel);
            this.Controls.Add(this.pictureBox);
            this.Name = "AmazonCarrierTermsAndConditionsNotAcceptedFootnoteControl";
            this.Size = new System.Drawing.Size(396, 25);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label additionalRatesAvailableLabel;
        private System.Windows.Forms.LinkLabel openDialogLink;
    }
}
