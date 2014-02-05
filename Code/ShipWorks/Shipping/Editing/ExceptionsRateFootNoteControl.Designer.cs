namespace ShipWorks.Shipping.Editing
{
    partial class ExceptionsRateFootNoteControl
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
            this.exceptionsLink = new System.Windows.Forms.LinkLabel();
            this.errorMessage = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // exceptionsLink
            // 
            this.exceptionsLink.AutoSize = true;
            this.exceptionsLink.Location = new System.Drawing.Point(237, 2);
            this.exceptionsLink.Name = "exceptionsLink";
            this.exceptionsLink.Size = new System.Drawing.Size(94, 13);
            this.exceptionsLink.TabIndex = 7;
            this.exceptionsLink.TabStop = true;
            this.exceptionsLink.Text = "More information...";
            this.exceptionsLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnClickExceptionsLink);
            // 
            // errorMessage
            // 
            this.errorMessage.AutoSize = true;
            this.errorMessage.Location = new System.Drawing.Point(32, 2);
            this.errorMessage.Name = "errorMessage";
            this.errorMessage.Size = new System.Drawing.Size(199, 13);
            this.errorMessage.TabIndex = 6;
            this.errorMessage.Text = "Some errors occurred while getting rates.";
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.flag_red;
            this.pictureBox.Location = new System.Drawing.Point(7, 2);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(16, 16);
            this.pictureBox.TabIndex = 8;
            this.pictureBox.TabStop = false;
            // 
            // ExceptionsRateFootNoteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.exceptionsLink);
            this.Controls.Add(this.errorMessage);
            this.Name = "ExceptionsRateFootNoteControl";
            this.Size = new System.Drawing.Size(396, 20);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.LinkLabel exceptionsLink;
        private System.Windows.Forms.Label errorMessage;
    }
}
