namespace ShipWorks.Stores.UI.Platforms.BigCommerce
{
    partial class BigCommerceAccountSettingsControl
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
            if (disposing)
            {
                components?.Dispose();
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
            this.accountSettingsElementHost = new System.Windows.Forms.Integration.ElementHost();
            this.bigCommerceAccountSettings1 = new ShipWorks.Stores.UI.Platforms.BigCommerce.BigCommerceAccountSettings();
            this.SuspendLayout();
            //
            // accountSettingsElementHost
            //
            this.accountSettingsElementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accountSettingsElementHost.Location = new System.Drawing.Point(0, 0);
            this.accountSettingsElementHost.Name = "accountSettingsElementHost";
            this.accountSettingsElementHost.Size = new System.Drawing.Size(463, 200);
            this.accountSettingsElementHost.TabIndex = 0;
            this.accountSettingsElementHost.Text = "elementHost1";
            this.accountSettingsElementHost.Child = this.bigCommerceAccountSettings1;
            //
            // BigCommerceAccountSettingsControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.accountSettingsElementHost);
            this.Name = "BigCommerceAccountSettingsControl";
            this.Size = new System.Drawing.Size(463, 200);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost accountSettingsElementHost;
        private BigCommerceAccountSettings bigCommerceAccountSettings1;
    }
}
