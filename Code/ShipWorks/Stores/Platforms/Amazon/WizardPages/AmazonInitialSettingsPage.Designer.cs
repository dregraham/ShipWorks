namespace ShipWorks.Stores.Platforms.Amazon.WizardPages
{
    partial class AmazonInitialSettingsPage
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
            this.storeCountryControl = new ShipWorks.Stores.Platforms.Amazon.AmazonCountryControl();
            this.storeInitialDownloadDaysControl = new ShipWorks.Stores.Platforms.Amazon.AmazonInitialDownloadDaysControl();
            this.SuspendLayout();
            // 
            // storeSettingsControl
            // 
            this.storeCountryControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storeCountryControl.Location = new System.Drawing.Point(11, 3);
            this.storeCountryControl.Name = "storeCountryControl";
            this.storeCountryControl.Size = new System.Drawing.Size(287, 66);
            this.storeCountryControl.TabIndex = 0;
            // 
            // storeInitialDownloadDaysControl
            // 
            this.storeInitialDownloadDaysControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storeInitialDownloadDaysControl.Location = new System.Drawing.Point(0, 75);
            this.storeInitialDownloadDaysControl.Name = "storeInitialDownloadDaysControl";
            this.storeInitialDownloadDaysControl.Size = new System.Drawing.Size(266, 69);
            this.storeInitialDownloadDaysControl.TabIndex = 1;
            // 
            // AmazonInitialSettingsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.storeCountryControl);
            this.Controls.Add(this.storeInitialDownloadDaysControl);
            this.Description = "Select the information about your Amazon account.";
            this.Name = "AmazonInitialSettingsPage";
            this.Size = new System.Drawing.Size(286, 148);
            this.Title = "Amazon Store Setup";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);

        }

        #endregion

        private AmazonCountryControl storeCountryControl;
        private AmazonInitialDownloadDaysControl storeInitialDownloadDaysControl;
    }
}
