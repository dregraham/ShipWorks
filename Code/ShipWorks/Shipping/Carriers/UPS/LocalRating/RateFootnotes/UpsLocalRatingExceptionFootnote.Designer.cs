namespace ShipWorks.Shipping.Carriers.UPS.LocalRating.RateFootnotes
{
    partial class UpsLocalRatingExceptionFootnote
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
            this.label1 = new System.Windows.Forms.Label();
            this.moreInfoLink = new System.Windows.Forms.LinkLabel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(349, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ShipWorks did not find any UPS rates for this shipment using local rating.";
            // 
            // moreInfoLink
            // 
            this.moreInfoLink.AutoSize = true;
            this.moreInfoLink.Location = new System.Drawing.Point(380, 4);
            this.moreInfoLink.Name = "moreInfoLink";
            this.moreInfoLink.Size = new System.Drawing.Size(61, 13);
            this.moreInfoLink.TabIndex = 1;
            this.moreInfoLink.TabStop = true;
            this.moreInfoLink.Text = "More Info...";
            this.moreInfoLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnMoreInfoClicked);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.warning16;
            this.pictureBox.Location = new System.Drawing.Point(3, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(16, 16);
            this.pictureBox.TabIndex = 3;
            this.pictureBox.TabStop = false;
            // 
            // UpsLocalRatingExceptionFootnote
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.moreInfoLink);
            this.Controls.Add(this.label1);
            this.Name = "UpsLocalRatingExceptionFootnote";
            this.Size = new System.Drawing.Size(444, 22);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel moreInfoLink;
        private System.Windows.Forms.PictureBox pictureBox;
    }
}
