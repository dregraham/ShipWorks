namespace ShipWorks.Shipping.Carriers.BestRate
{
    partial class BrokerExceptionsRateFootnoteControl
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
            this.moreInfoLink = new System.Windows.Forms.LinkLabel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // errorMessage
            // 
            this.errorMessage.AutoSize = true;
            this.errorMessage.Location = new System.Drawing.Point(29, 4);
            this.errorMessage.Name = "errorMessage";
            this.errorMessage.Size = new System.Drawing.Size(241, 13);
            this.errorMessage.TabIndex = 0;
            this.errorMessage.Text = "ShipWorks could not get rates from all providers.";
            // 
            // moreInfoLink
            // 
            this.moreInfoLink.AutoSize = true;
            this.moreInfoLink.Location = new System.Drawing.Point(267, 4);
            this.moreInfoLink.Name = "moreInfoLink";
            this.moreInfoLink.Size = new System.Drawing.Size(64, 13);
            this.moreInfoLink.TabIndex = 1;
            this.moreInfoLink.TabStop = true;
            this.moreInfoLink.Text = "More info...";
            this.moreInfoLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnMoreInfoClick);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.flag_red;
            this.pictureBox.Location = new System.Drawing.Point(4, 4);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(16, 16);
            this.pictureBox.TabIndex = 2;
            this.pictureBox.TabStop = false;
            // 
            // BrokerExceptionsRateFootnoteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.moreInfoLink);
            this.Controls.Add(this.errorMessage);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "BrokerExceptionsRateFootnoteControl";
            this.Size = new System.Drawing.Size(362, 20);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label errorMessage;
        private System.Windows.Forms.LinkLabel moreInfoLink;
        private System.Windows.Forms.PictureBox pictureBox;

    }
}
