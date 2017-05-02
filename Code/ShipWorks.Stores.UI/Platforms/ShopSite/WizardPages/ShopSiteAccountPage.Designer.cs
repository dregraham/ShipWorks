namespace ShipWorks.Stores.UI.Platforms.ShopSite.WizardPages
{
    partial class ShopSiteAccountPage
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
            this.accountSettingsControl = new ShipWorks.Stores.UI.Platforms.ShopSite.ShopSiteAccountSettingsControl();
            this.SuspendLayout();
            //
            // accountSettingsControl
            //
            this.accountSettingsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.accountSettingsControl.Location = new System.Drawing.Point(3, 3);
            this.accountSettingsControl.Name = "accountSettingsControl";
            this.accountSettingsControl.Size = new System.Drawing.Size(516, 245);
            this.accountSettingsControl.TabIndex = 0;
            //
            // ShopSiteAccountPage
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.accountSettingsControl);
            this.Description = "Enter the following information about your online store.";
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "ShopSiteAccountPage";
            this.Size = new System.Drawing.Size(522, 264);
            this.Title = "Store Setup";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);

        }

        #endregion

        private ShopSiteAccountSettingsControl accountSettingsControl;

    }
}
