namespace ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion
{
    partial class UspsRatePromotionFootnote
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
            this.linkControl = new ShipWorks.UI.Controls.LinkControl();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // linkControl
            // 
            this.linkControl.AutoSize = true;
            this.linkControl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkControl.ForeColor = System.Drawing.Color.Blue;
            this.linkControl.Location = new System.Drawing.Point(184, 9);
            this.linkControl.Name = "linkControl";
            this.linkControl.Size = new System.Drawing.Size(103, 13);
            this.linkControl.TabIndex = 5;
            this.linkControl.Text = "Activate Discount...";
            this.linkControl.Click += new System.EventHandler(this.OnActivateDiscount);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.star_green;
            this.pictureBox.Location = new System.Drawing.Point(7, 7);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(16, 16);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 4;
            this.pictureBox.TabStop = false;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(28, 9);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(150, 13);
            this.label.TabIndex = 3;
            this.label.Text = "You qualify for a rate discount.";
            // 
            // UspsRatePromotionFootnote
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.label);
            this.Controls.Add(this.linkControl);
            this.Name = "UspsRatePromotionFootnote";
            this.Size = new System.Drawing.Size(370, 29);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Controls.LinkControl linkControl;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label label;
    }
}
