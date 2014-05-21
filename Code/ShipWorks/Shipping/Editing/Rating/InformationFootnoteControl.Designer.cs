namespace ShipWorks.Shipping.Editing.Rating
{
    partial class InformationFootnoteControl
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
            this.informationMessageLabel = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // informationMessageLabel
            // 
            this.informationMessageLabel.AutoSize = true;
            this.informationMessageLabel.Location = new System.Drawing.Point(25, 7);
            this.informationMessageLabel.Name = "informationMessageLabel";
            this.informationMessageLabel.Size = new System.Drawing.Size(182, 13);
            this.informationMessageLabel.TabIndex = 9;
            this.informationMessageLabel.Text = "Select a different carrier to Get Rates";
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.information16;
            this.pictureBox.Location = new System.Drawing.Point(4, 5);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(16, 16);
            this.pictureBox.TabIndex = 11;
            this.pictureBox.TabStop = false;
            // 
            // InformationFootnoteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.informationMessageLabel);
            this.Name = "InformationFootnoteControl";
            this.Size = new System.Drawing.Size(396, 28);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label informationMessageLabel;
    }
}
