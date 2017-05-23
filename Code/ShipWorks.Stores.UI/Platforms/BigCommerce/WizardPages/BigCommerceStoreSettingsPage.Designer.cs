namespace ShipWorks.Stores.UI.Platforms.BigCommerce.WizardPages
{
    partial class BigCommerceStoreSettingsPage
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
            this.storeSettingsControl = new ShipWorks.Stores.UI.Platforms.BigCommerce.BigCommerceStoreSettingsControl();
            this.downloadCriteriaControl = new BigCommerceDownloadCriteriaControl();
            this.SuspendLayout();
            // 
            // storeSettingsControl
            // 
            this.storeSettingsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storeSettingsControl.Location = new System.Drawing.Point(8, 0);
            this.storeSettingsControl.Name = "storeSettingsControl";
            this.storeSettingsControl.Size = new System.Drawing.Size(508, 60);
            this.storeSettingsControl.TabIndex = 2;
            // 
            // storeSettingsControl
            // 
            this.downloadCriteriaControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadCriteriaControl.Location = new System.Drawing.Point(8, 65);
            this.downloadCriteriaControl.Name = "downloadCriteriaControl";
            this.downloadCriteriaControl.Size = new System.Drawing.Size(508, 150);
            this.downloadCriteriaControl.TabIndex = 3;
            // 
            // BigCommerceStoreSettingsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.storeSettingsControl);
            this.Controls.Add(this.downloadCriteriaControl);
            this.Name = "BigCommerceStoreSettingsPage";
            this.Size = new System.Drawing.Size(544, 424);
            this.Title = "BigCommerce Store Settings";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.ResumeLayout(false);

        }

        #endregion

        private BigCommerceStoreSettingsControl storeSettingsControl;
        private BigCommerceDownloadCriteriaControl downloadCriteriaControl;
    }
}
