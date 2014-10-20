namespace ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.NotQualified
{
    partial class UspsRateNotQualifiedFootnote
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
            this.label = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(27, 6);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(322, 13);
            this.label.TabIndex = 4;
            this.label.Text = "The selected packaging option does not qualify for a rate discount.";
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.information16;
            this.pictureBox.Location = new System.Drawing.Point(6, 4);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(16, 16);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 5;
            this.pictureBox.TabStop = false;
            // 
            // UspsRateNotQualifiedFootnote
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.label);
            this.Name = "UspsRateNotQualifiedFootnote";
            this.Size = new System.Drawing.Size(416, 24);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label label;
    }
}
