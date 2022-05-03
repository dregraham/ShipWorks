namespace ShipWorks.Stores.Platforms.Amazon.WizardPages
{
    partial class AmazonMwsCountryPage
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
            this.storeSettingsControl = new ShipWorks.Stores.Platforms.Amazon.AmazonMwsCountryControl();
            this.SuspendLayout();
            // 
            // storeSettingsControl
            // 
            this.storeSettingsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storeSettingsControl.Location = new System.Drawing.Point(3, 1);
            this.storeSettingsControl.Name = "storeSettingsControl";
            this.storeSettingsControl.Size = new System.Drawing.Size(287, 68);
            this.storeSettingsControl.TabIndex = 0;
            // 
            // AmazonMwsCountryPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.storeSettingsControl);
            this.Description = "Enter the information about your Amazon account.";
            this.Name = "AmazonMwsCountryPage";
            this.Size = new System.Drawing.Size(293, 81);
            this.Title = "Amazon Store Setup";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);

        }

        #endregion

        private AmazonMwsCountryControl storeSettingsControl;

    }
}
