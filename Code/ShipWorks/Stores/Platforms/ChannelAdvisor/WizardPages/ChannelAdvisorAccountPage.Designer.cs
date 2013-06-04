namespace ShipWorks.Stores.Platforms.ChannelAdvisor.WizardPages
{
    partial class ChannelAdvisorAccountPage
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
            this.accountSettings = new ShipWorks.Stores.Platforms.ChannelAdvisor.ChannelAdvisorAccountSettingsControl();
            this.SuspendLayout();
            // 
            // accountSettings
            // 
            this.accountSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountSettings.Location = new System.Drawing.Point(17, 12);
            this.accountSettings.Name = "accountSettings";
            this.accountSettings.Size = new System.Drawing.Size(502, 270);
            this.accountSettings.TabIndex = 1;
            // 
            // ChannelAdvisorAccountPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.accountSettings);
            this.Name = "ChannelAdvisorAccountPage";
            this.Size = new System.Drawing.Size(522, 282);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);

        }

        #endregion

        private ChannelAdvisorAccountSettingsControl accountSettings;
    }
}
