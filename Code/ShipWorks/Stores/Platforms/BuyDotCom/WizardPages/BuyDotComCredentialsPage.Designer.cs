namespace ShipWorks.Stores.Platforms.BuyDotCom.WizardPages
{
    partial class BuyDotComCredentialsPage
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
            this.settingsControl = new ShipWorks.Stores.Platforms.BuyDotCom.BuyDotComAccountSettingsControl();
            this.SuspendLayout();
            // 
            // settingsControl
            // 
            this.settingsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.settingsControl.Location = new System.Drawing.Point(2, 2);
            this.settingsControl.Name = "settingsControl";
            this.settingsControl.Size = new System.Drawing.Size(483, 180);
            this.settingsControl.TabIndex = 0;
            // 
            // BuyDotComCredentialsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.settingsControl);
            this.Name = "BuyDotComCredentialsPage";
            this.Size = new System.Drawing.Size(509, 199);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);

        }

        #endregion

        private BuyDotComAccountSettingsControl settingsControl;

    }
}
