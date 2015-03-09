namespace ShipWorks.Stores.Content.Panels
{
    partial class MapPanel
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
            this.googleImage = new System.Windows.Forms.PictureBox();
            this.errorLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.googleImage)).BeginInit();
            this.SuspendLayout();
            // 
            // googleImage
            // 
            this.googleImage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.googleImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.googleImage.Location = new System.Drawing.Point(0, 0);
            this.googleImage.Name = "googleImage";
            this.googleImage.Size = new System.Drawing.Size(326, 174);
            this.googleImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.googleImage.TabIndex = 4;
            this.googleImage.TabStop = false;
            this.googleImage.Click += new System.EventHandler(this.OnGoogleImageClick);
            // 
            // errorLabel
            // 
            this.errorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.errorLabel.Location = new System.Drawing.Point(4, 4);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(322, 170);
            this.errorLabel.TabIndex = 5;
            // 
            // MapPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.googleImage);
            this.Controls.Add(this.errorLabel);
            this.Name = "MapPanel";
            this.Size = new System.Drawing.Size(326, 174);
            this.SizeChanged += new System.EventHandler(this.OnSizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.googleImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox googleImage;
        private System.Windows.Forms.Label errorLabel;
    }
}
