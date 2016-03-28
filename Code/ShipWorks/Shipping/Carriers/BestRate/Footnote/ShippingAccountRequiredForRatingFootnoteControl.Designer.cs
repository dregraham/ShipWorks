namespace ShipWorks.Shipping.Carriers.BestRate.Footnote
{
    partial class ShippingAccountRequiredForRatingFootnoteControl
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
            this.linkControl = new ShipWorks.UI.Controls.LinkControl();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(22, 7);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(215, 13);
            this.label.TabIndex = 6;
            this.label.Text = "A shipping account is required to view rates.";
            // 
            // linkControl
            // 
            this.linkControl.AutoSize = true;
            this.linkControl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkControl.ForeColor = System.Drawing.Color.Blue;
            this.linkControl.Location = new System.Drawing.Point(237, 7);
            this.linkControl.Name = "linkControl";
            this.linkControl.Size = new System.Drawing.Size(130, 13);
            this.linkControl.TabIndex = 8;
            this.linkControl.Text = "Add a shipping account...";
            this.linkControl.Click += new System.EventHandler(this.OnAddShippingAccount);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.information16;
            this.pictureBox.Location = new System.Drawing.Point(3, 5);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(16, 16);
            this.pictureBox.TabIndex = 12;
            this.pictureBox.TabStop = false;
            // 
            // ShippingAccountRequiredForRatingFootnoteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.linkControl);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.label);
            this.Name = "ShippingAccountRequiredForRatingFootnoteControl";
            this.Size = new System.Drawing.Size(449, 27);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label;
        private UI.Controls.LinkControl linkControl;
        private System.Windows.Forms.PictureBox pictureBox;
    }
}
