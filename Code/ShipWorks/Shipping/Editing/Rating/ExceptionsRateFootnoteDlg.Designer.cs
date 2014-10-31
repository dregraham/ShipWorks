using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Editing.Rating
{
    partial class ExceptionsRateFootnoteDlg
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ok = new System.Windows.Forms.Button();
            this.helpImage = new System.Windows.Forms.PictureBox();
            this.errorMessage = new ShipWorks.Shipping.Editing.Rating.ExceptionsRateFootnoteErrorControl();
            ((System.ComponentModel.ISupportInitialize)(this.helpImage)).BeginInit();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(336, 102);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 2;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOkClicked);
            // 
            // helpImage
            // 
            this.helpImage.Image = global::ShipWorks.Properties.Resources.help2;
            this.helpImage.Location = new System.Drawing.Point(12, 12);
            this.helpImage.Name = "helpImage";
            this.helpImage.Size = new System.Drawing.Size(32, 32);
            this.helpImage.TabIndex = 7;
            this.helpImage.TabStop = false;
            // 
            // errorMessage
            // 
            this.errorMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.errorMessage.Location = new System.Drawing.Point(50, 20);
            this.errorMessage.Name = "errorMessage";
            this.errorMessage.Size = new System.Drawing.Size(361, 67);
            this.errorMessage.TabIndex = 6;
            // 
            // ExceptionsRateFootnoteDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 137);
            this.ControlBox = false;
            this.Controls.Add(this.helpImage);
            this.Controls.Add(this.errorMessage);
            this.Controls.Add(this.ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ExceptionsRateFootnoteDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ShipWorks";
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.helpImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private ExceptionsRateFootnoteErrorControl errorMessage;
        private System.Windows.Forms.PictureBox helpImage;
    }
}