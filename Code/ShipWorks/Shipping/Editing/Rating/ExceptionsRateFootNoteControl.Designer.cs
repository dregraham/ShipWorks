namespace ShipWorks.Shipping.Editing.Rating
{
    partial class ExceptionsRateFootnoteControl
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
            this.errorMessage = new System.Windows.Forms.Label();
            this.exceptionsLink = new System.Windows.Forms.LinkLabel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // errorMessage
            // 
            this.errorMessage.AutoSize = true;
            this.errorMessage.Location = new System.Drawing.Point(25, 7);
            this.errorMessage.Name = "errorMessage";
            this.errorMessage.Size = new System.Drawing.Size(206, 13);
            this.errorMessage.TabIndex = 6;
            this.errorMessage.Text = "Some errors occurred while getting rates.";
            // 
            // exceptionsLink
            // 
            this.exceptionsLink.AutoSize = true;
            this.exceptionsLink.Location = new System.Drawing.Point(237, 7);
            this.exceptionsLink.Name = "exceptionsLink";
            this.exceptionsLink.Size = new System.Drawing.Size(64, 13);
            this.exceptionsLink.TabIndex = 7;
            this.exceptionsLink.TabStop = true;
            this.exceptionsLink.Text = "More info...";
            this.exceptionsLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnClickExceptionsLink);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.error16;
            this.pictureBox.Location = new System.Drawing.Point(4, 5);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(16, 16);
            this.pictureBox.TabIndex = 8;
            this.pictureBox.TabStop = false;
            // 
            // ExceptionsRateFootnoteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.exceptionsLink);
            this.Controls.Add(this.errorMessage);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ExceptionsRateFootnoteControl";
            this.Size = new System.Drawing.Size(396, 28);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label errorMessage;
        private System.Windows.Forms.LinkLabel exceptionsLink;
        private System.Windows.Forms.PictureBox pictureBox;

    }
}
