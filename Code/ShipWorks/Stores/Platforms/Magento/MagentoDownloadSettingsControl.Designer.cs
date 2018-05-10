namespace ShipWorks.Stores.Platforms.Magento
{
    partial class MagentoDownloadSettingsControl
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
            this.downloadOrderStatusLabel2 = new System.Windows.Forms.Label();
            this.downloadOrderStatusLabel1 = new System.Windows.Forms.Label();
            this.downloadOrderStatus = new System.Windows.Forms.CheckBox();
            this.downloadSettingsControl = new ShipWorks.Stores.Management.DownloadSettingsControl();
            this.SuspendLayout();
            // 
            // downloadOrderStatusLabel2
            // 
            this.downloadOrderStatusLabel2.AutoSize = true;
            this.downloadOrderStatusLabel2.Location = new System.Drawing.Point(20, 117);
            this.downloadOrderStatusLabel2.Name = "downloadOrderStatusLabel2";
            this.downloadOrderStatusLabel2.Size = new System.Drawing.Size(239, 13);
            this.downloadOrderStatusLabel2.TabIndex = 7;
            this.downloadOrderStatusLabel2.Text = "the online order status for each of the split orders.";
            // 
            // downloadOrderStatusLabel1
            // 
            this.downloadOrderStatusLabel1.AutoSize = true;
            this.downloadOrderStatusLabel1.Location = new System.Drawing.Point(20, 103);
            this.downloadOrderStatusLabel1.Name = "downloadOrderStatusLabel1";
            this.downloadOrderStatusLabel1.Size = new System.Drawing.Size(410, 13);
            this.downloadOrderStatusLabel1.TabIndex = 6;
            this.downloadOrderStatusLabel1.Text = "When you split orders associated with this store, ShipWorks will continue to down" +
    "load";
            // 
            // downloadOrderStatus
            // 
            this.downloadOrderStatus.AutoSize = true;
            this.downloadOrderStatus.Location = new System.Drawing.Point(3, 83);
            this.downloadOrderStatus.Name = "downloadOrderStatus";
            this.downloadOrderStatus.Size = new System.Drawing.Size(228, 17);
            this.downloadOrderStatus.TabIndex = 5;
            this.downloadOrderStatus.Text = "Download online order status for split orders";
            this.downloadOrderStatus.UseVisualStyleBackColor = true;
            this.downloadOrderStatus.CheckedChanged += new System.EventHandler(this.OnDownloadOrderStatusCheckedChanged);
            // 
            // downloadSettingsControl
            // 
            this.downloadSettingsControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadSettingsControl.Location = new System.Drawing.Point(0, 0);
            this.downloadSettingsControl.Name = "downloadSettingsControl";
            this.downloadSettingsControl.Size = new System.Drawing.Size(544, 73);
            this.downloadSettingsControl.TabIndex = 0;
            // 
            // MagentoDownloadSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.downloadOrderStatusLabel2);
            this.Controls.Add(this.downloadOrderStatusLabel1);
            this.Controls.Add(this.downloadOrderStatus);
            this.Controls.Add(this.downloadSettingsControl);
            this.Name = "MagentoDownloadSettingsControl";
            this.Size = new System.Drawing.Size(544, 133);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Management.DownloadSettingsControl downloadSettingsControl;
        private System.Windows.Forms.Label downloadOrderStatusLabel2;
        private System.Windows.Forms.Label downloadOrderStatusLabel1;
        private System.Windows.Forms.CheckBox downloadOrderStatus;
    }
}
