namespace ShipWorks.Stores.UI.Platforms.ShopSite
{
    partial class ShopSiteAccountSettingsControl
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
            this.shopSiteAccountSettings = new ShipWorks.Stores.UI.Platforms.ShopSite.ShopSiteAccountSettings();
            this.SuspendLayout();
            //
            // accountSettingsElementHost
            //
            this.accountSettingsElementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accountSettingsElementHost.Location = new System.Drawing.Point(0, 0);
            this.accountSettingsElementHost.Name = "accountSettingsElementHost";
            this.accountSettingsElementHost.Size = new System.Drawing.Size(494, 440);
            this.accountSettingsElementHost.TabIndex = 0;
            this.accountSettingsElementHost.Text = "accountSettingsElementHost";
            this.accountSettingsElementHost.Child = this.shopSiteAccountSettings;
            //
            // ShopSiteAccountSettingsControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.accountSettingsElementHost);
            this.Name = "ShopSiteAccountSettingsControl";
            this.Size = new System.Drawing.Size(494, 340);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost accountSettingsElementHost;
        private ShipWorks.Stores.UI.Platforms.ShopSite.ShopSiteAccountSettings shopSiteAccountSettings;
    }
}
