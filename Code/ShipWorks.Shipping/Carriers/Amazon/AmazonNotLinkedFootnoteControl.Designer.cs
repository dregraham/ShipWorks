namespace ShipWorks.Shipping.Carriers.Amazon
{
    partial class AmazonNotLinkedFootnoteControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AmazonNotLinkedFootnoteControl));
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.infoLink = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(4, 5);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(16, 16);
            this.pictureBox.TabIndex = 12;
            this.pictureBox.TabStop = false;
            // 
            // infoLink
            // 
            this.infoLink.AutoSize = true;
            this.infoLink.LinkArea = new System.Windows.Forms.LinkArea(51, 4);
            this.infoLink.Location = new System.Drawing.Point(25, 7);
            this.infoLink.Name = "infoLink";
            this.infoLink.Size = new System.Drawing.Size(269, 17);
            this.infoLink.TabIndex = 14;
            this.infoLink.TabStop = true;
            this.infoLink.Text = "Shipworks could not retrieve {0} rates. For more info, click here.";
            this.infoLink.UseCompatibleTextRendering = true;
            this.infoLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnClickInfoLink);
            // 
            // AmazonUspsNotLinkedFootnoteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.infoLink);
            this.Controls.Add(this.pictureBox);
            this.Name = "AmazonNotLinkedFootnoteControl";
            this.Size = new System.Drawing.Size(396, 28);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.LinkLabel infoLink;
    }
}
