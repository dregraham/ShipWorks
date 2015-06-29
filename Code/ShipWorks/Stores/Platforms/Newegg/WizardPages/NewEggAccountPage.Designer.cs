namespace ShipWorks.Stores.Platforms.Newegg.WizardPages
{
    partial class NeweggAccountPage
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
            this.storeSettingsControl = new ShipWorks.Stores.Platforms.Newegg.NeweggStoreCredentialsControl();
            this.SuspendLayout();
            // 
            // storeSettingsControl
            // 
            this.storeSettingsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storeSettingsControl.Location = new System.Drawing.Point(16, 0);
            this.storeSettingsControl.Marketplace = 0;
            this.storeSettingsControl.Name = "storeSettingsControl";
            this.storeSettingsControl.SecretKey = "";
            this.storeSettingsControl.SellerId = "";
            this.storeSettingsControl.Size = new System.Drawing.Size(367, 154);
            this.storeSettingsControl.TabIndex = 0;
            // 
            // NeweggAccountPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.storeSettingsControl);
            this.Name = "NeweggAccountPage";
            this.Size = new System.Drawing.Size(383, 166);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);

        }

        #endregion

        private NeweggStoreCredentialsControl storeSettingsControl;

    }
}
