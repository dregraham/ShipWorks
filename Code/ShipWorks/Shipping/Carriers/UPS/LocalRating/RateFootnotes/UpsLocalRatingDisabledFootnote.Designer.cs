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
            this.accountSettingsLink = new System.Windows.Forms.LinkLabel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // accountSettingsLink
            // 
            this.accountSettingsLink.AutoSize = true;
            this.accountSettingsLink.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.accountSettingsLink.LinkArea = new System.Windows.Forms.LinkArea(0, 19);
            this.accountSettingsLink.Location = new System.Drawing.Point(305, 4);
            this.accountSettingsLink.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.accountSettingsLink.Name = "accountSettingsLink";
            this.accountSettingsLink.Size = new System.Drawing.Size(48, 18);
            this.accountSettingsLink.TabIndex = 1;
            this.accountSettingsLink.TabStop = true;
            this.accountSettingsLink.Text = "Enable...";
            this.accountSettingsLink.UseCompatibleTextRendering = true;
            this.accountSettingsLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnClickLink);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.information16;
            this.pictureBox.Location = new System.Drawing.Point(4, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(16, 16);
            this.pictureBox.TabIndex = 4;
            this.pictureBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(274, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Local rating needs to be enabled to compare UPS rates.";
            // 
            // UpsLocalRatingDisabledFootnote
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.accountSettingsLink);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "UpsLocalRatingDisabledFootnote";
            this.Size = new System.Drawing.Size(390, 21);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.LinkLabel accountSettingsLink;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label label1;
    }
}
