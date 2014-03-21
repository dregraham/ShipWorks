namespace ShipWorks.Shipping.Carriers.BestRate.Footnote
{
    partial class CounterRatesInvalidStoreAddressFootnoteControl
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
            this.storeAddressLink = new System.Windows.Forms.LinkLabel();
            this.errorMessage = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // storeAddressLink
            // 
            this.storeAddressLink.AutoSize = true;
            this.storeAddressLink.Location = new System.Drawing.Point(245, 5);
            this.storeAddressLink.Name = "storeAddressLink";
            this.storeAddressLink.Size = new System.Drawing.Size(86, 13);
            this.storeAddressLink.TabIndex = 4;
            this.storeAddressLink.TabStop = true;
            this.storeAddressLink.Text = "Enter address...";
            this.storeAddressLink.Click += new System.EventHandler(this.OnShowAddress);
            // 
            // errorMessage
            // 
            this.errorMessage.AutoSize = true;
            this.errorMessage.Location = new System.Drawing.Point(25, 5);
            this.errorMessage.Name = "errorMessage";
            this.errorMessage.Size = new System.Drawing.Size(219, 13);
            this.errorMessage.TabIndex = 3;
            this.errorMessage.Text = "ShipWorks needs your address to get rates.";
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.flag_red;
            this.pictureBox.Location = new System.Drawing.Point(4, 4);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(16, 16);
            this.pictureBox.TabIndex = 5;
            this.pictureBox.TabStop = false;
            // 
            // CounterRatesInvalidStoreAddressFootnoteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.storeAddressLink);
            this.Controls.Add(this.errorMessage);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "CounterRatesInvalidStoreAddressFootnoteControl";
            this.Size = new System.Drawing.Size(396, 20);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.LinkLabel storeAddressLink;
        private System.Windows.Forms.Label errorMessage;
    }
}
