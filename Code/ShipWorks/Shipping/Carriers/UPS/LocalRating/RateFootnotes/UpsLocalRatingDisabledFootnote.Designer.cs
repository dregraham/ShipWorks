namespace ShipWorks.Shipping.Carriers.UPS.LocalRating.RateFootnotes
{
    partial class UpsLocalRatingDisabledFootnote
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
            this.footnote = new System.Windows.Forms.Label();
            this.accountSettingsLink = new System.Windows.Forms.LinkLabel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // footnote
            // 
            this.footnote.AutoSize = true;
            this.footnote.Location = new System.Drawing.Point(25, 4);
            this.footnote.Name = "footnote";
            this.footnote.Size = new System.Drawing.Size(452, 13);
            this.footnote.TabIndex = 0;
            this.footnote.Text = "UPS local rating must be enabled to compare UPS rates. To setup local rating for " +
    "this account,";
            // 
            // accountSettingsLink
            // 
            this.accountSettingsLink.AutoSize = true;
            this.accountSettingsLink.Location = new System.Drawing.Point(474, 4);
            this.accountSettingsLink.Name = "accountSettingsLink";
            this.accountSettingsLink.Size = new System.Drawing.Size(56, 13);
            this.accountSettingsLink.TabIndex = 1;
            this.accountSettingsLink.TabStop = true;
            this.accountSettingsLink.Text = "click here.";
            this.accountSettingsLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnClickLink);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.information16;
            this.pictureBox.Location = new System.Drawing.Point(3, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(16, 16);
            this.pictureBox.TabIndex = 4;
            this.pictureBox.TabStop = false;
            // 
            // UpsLocalRatingDisabledFootnote
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.accountSettingsLink);
            this.Controls.Add(this.footnote);
            this.Name = "UpsLocalRatingDisabledFootnote";
            this.Size = new System.Drawing.Size(533, 21);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label footnote;
        private System.Windows.Forms.LinkLabel accountSettingsLink;
        private System.Windows.Forms.PictureBox pictureBox;
    }
}
