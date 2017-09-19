namespace ShipWorks.Stores.Platforms.Etsy
{
    partial class EtsyTokenManageControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tokenButton = new System.Windows.Forms.Button();
            this.statusText = new System.Windows.Forms.Label();
            this.statusPicture = new System.Windows.Forms.PictureBox();
            this.panelStatus = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize) (this.statusPicture)).BeginInit();
            this.panelStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // tokenButton
            // 
            this.tokenButton.Location = new System.Drawing.Point(3, 3);
            this.tokenButton.Name = "tokenButton";
            this.tokenButton.Size = new System.Drawing.Size(137, 23);
            this.tokenButton.TabIndex = 1;
            this.tokenButton.Text = "Create Login Token...";
            this.tokenButton.UseVisualStyleBackColor = true;
            this.tokenButton.Click += new System.EventHandler(this.OnAuthorizeShipWorks);
            // 
            // statusText
            // 
            this.statusText.AutoSize = true;
            this.statusText.ForeColor = System.Drawing.Color.DimGray;
            this.statusText.Location = new System.Drawing.Point(20, 6);
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(286, 13);
            this.statusText.TabIndex = 18;
            this.statusText.Text = "Connected to \'ShipWorks HandmadeStuff\' as \'interapptive\'";
            // 
            // statusPicture
            // 
            this.statusPicture.Image = global::ShipWorks.Properties.Resources.check16;
            this.statusPicture.Location = new System.Drawing.Point(3, 4);
            this.statusPicture.Name = "statusPicture";
            this.statusPicture.Size = new System.Drawing.Size(16, 16);
            this.statusPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.statusPicture.TabIndex = 17;
            this.statusPicture.TabStop = false;
            // 
            // panelStatus
            // 
            this.panelStatus.Controls.Add(this.statusText);
            this.panelStatus.Controls.Add(this.statusPicture);
            this.panelStatus.Location = new System.Drawing.Point(143, 3);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(358, 23);
            this.panelStatus.TabIndex = 19;
            // 
            // EtsyTokenManageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelStatus);
            this.Controls.Add(this.tokenButton);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "EtsyTokenManageControl";
            this.Size = new System.Drawing.Size(504, 33);
            ((System.ComponentModel.ISupportInitialize) (this.statusPicture)).EndInit();
            this.panelStatus.ResumeLayout(false);
            this.panelStatus.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button tokenButton;
        private System.Windows.Forms.Label statusText;
        private System.Windows.Forms.PictureBox statusPicture;
        private System.Windows.Forms.Panel panelStatus;
    }
}
