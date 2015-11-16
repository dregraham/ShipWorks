namespace ShipWorks.Stores.Platforms.Brightpearl
{
    partial class BrightpearlAddStoreWizardPage
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.accountSettingsControl = new ShipWorks.Stores.Platforms.Brightpearl.BrightpearlStoreAccountSettingsControl();
            this.helpLink = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            this.helpLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // accountSettingsControl
            // 
            this.accountSettingsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountSettingsControl.Location = new System.Drawing.Point(12, 10);
            this.accountSettingsControl.Name = "accountSettingsControl";
            this.accountSettingsControl.Size = new System.Drawing.Size(482, 188);
            this.accountSettingsControl.TabIndex = 1;
            // 
            // helpLink
            // 
            this.helpLink.AutoSize = true;
            this.helpLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.helpLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.helpLink.ForeColor = System.Drawing.Color.Blue;
            this.helpLink.Location = new System.Drawing.Point(202, 157);
            this.helpLink.Name = "helpLink";
            this.helpLink.Size = new System.Drawing.Size(51, 13);
            this.helpLink.TabIndex = 2;
            this.helpLink.Text = "click here";
            this.helpLink.Url = "http://www.interapptive.com/shipworks/help";
            // 
            // helpLabel
            // 
            this.helpLabel.AutoSize = true;
            this.helpLabel.Location = new System.Drawing.Point(9, 157);
            this.helpLabel.Name = "helpLabel";
            this.helpLabel.Size = new System.Drawing.Size(193, 13);
            this.helpLabel.TabIndex = 3;
            this.helpLabel.Text = "For help adding your Brightpearl store,";
            // 
            // BrightpearlAddStoreWizardPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.helpLabel);
            this.Controls.Add(this.helpLink);
            this.Controls.Add(this.accountSettingsControl);
            this.Name = "BrightpearlAddStoreWizardPage";
            this.Size = new System.Drawing.Size(563, 203);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrightpearlStoreAccountSettingsControl accountSettingsControl;
        private ApplicationCore.Interaction.HelpLink helpLink;
        private System.Windows.Forms.Label helpLabel;
    }
}