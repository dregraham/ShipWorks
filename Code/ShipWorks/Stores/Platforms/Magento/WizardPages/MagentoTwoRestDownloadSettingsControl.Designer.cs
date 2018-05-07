namespace ShipWorks.Stores.Platforms.Magento.WizardPages
{
    partial class MagentoTwoRestDownloadSettingsControl
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
            this.downloadOrderStatus = new System.Windows.Forms.CheckBox();
            this.downloadOrderStatusLabel1 = new System.Windows.Forms.Label();
            this.downloadOrderStatusLabel2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // downloadOrderStatus
            // 
            this.downloadOrderStatus.AutoSize = true;
            this.downloadOrderStatus.Checked = false;
            this.downloadOrderStatus.CheckState = System.Windows.Forms.CheckState.Unchecked;
            this.downloadOrderStatus.Location = new System.Drawing.Point(12, 8);
            this.downloadOrderStatus.Name = "downloadOrderStatus";
            this.downloadOrderStatus.Size = new System.Drawing.Size(235, 17);
            this.downloadOrderStatus.TabIndex = 2;
            this.downloadOrderStatus.Text = "Download online order status of split orders";
            this.downloadOrderStatus.UseVisualStyleBackColor = true;
            this.downloadOrderStatus.CheckedChanged += new System.EventHandler(this.Save);
            // 
            // downloadOrderStatusLabel1
            // 
            this.downloadOrderStatusLabel1.AutoSize = true;
            this.downloadOrderStatusLabel1.Location = new System.Drawing.Point(31, 28);
            this.downloadOrderStatusLabel1.Name = "downloadOrderStatusLabel1";
            this.downloadOrderStatusLabel1.Size = new System.Drawing.Size(417, 13);
            this.downloadOrderStatusLabel1.TabIndex = 3;
            this.downloadOrderStatusLabel1.Text = "When you split orders associated with this store, ShipWorks will continue to down" +
    "load";
            // 
            // downloadOrderStatusLabel2
            // 
            this.downloadOrderStatusLabel2.AutoSize = true;
            this.downloadOrderStatusLabel2.Location = new System.Drawing.Point(31, 41);
            this.downloadOrderStatusLabel2.Name = "downloadOrderStatusLabel2";
            this.downloadOrderStatusLabel2.Size = new System.Drawing.Size(251, 13);
            this.downloadOrderStatusLabel2.TabIndex = 4;
            this.downloadOrderStatusLabel2.Text = "the online order status for each of the split orders.";
            // 
            // MagentoTwoRestDownloadSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.downloadOrderStatusLabel2);
            this.Controls.Add(this.downloadOrderStatusLabel1);
            this.Controls.Add(this.downloadOrderStatus);
            this.Name = "MagentoTwoRestDownloadSettingsControl";
            this.Size = new System.Drawing.Size(452, 60);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox downloadOrderStatus;
        private System.Windows.Forms.Label downloadOrderStatusLabel1;
        private System.Windows.Forms.Label downloadOrderStatusLabel2;
    }
}
