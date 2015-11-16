namespace ShipWorks.Stores.Platforms.OrderDynamics.WizardPages
{
    partial class OrderDynamicsAccountPage
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
            this.accountSettingsControl = new ShipWorks.Stores.Platforms.OrderDynamics.OrderDynamicsAccountSettingsControl();
            this.helpLabel = new System.Windows.Forms.Label();
            this.helpLink = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            this.SuspendLayout();
            // 
            // accountSettingsControl
            // 
            this.accountSettingsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountSettingsControl.Location = new System.Drawing.Point(27, 15);
            this.accountSettingsControl.Name = "accountSettingsControl";
            this.accountSettingsControl.Size = new System.Drawing.Size(480, 188);
            this.accountSettingsControl.TabIndex = 0;
            // 
            // helpLabel
            // 
            this.helpLabel.AutoSize = true;
            this.helpLabel.Location = new System.Drawing.Point(31, 169);
            this.helpLabel.Name = "helpLabel";
            this.helpLabel.Size = new System.Drawing.Size(214, 13);
            this.helpLabel.TabIndex = 5;
            this.helpLabel.Text = "For help adding your OrderDynamics store,";
            // 
            // helpLink
            // 
            this.helpLink.AutoSize = true;
            this.helpLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.helpLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.helpLink.ForeColor = System.Drawing.Color.Blue;
            this.helpLink.Location = new System.Drawing.Point(242, 169);
            this.helpLink.Name = "helpLink";
            this.helpLink.Size = new System.Drawing.Size(51, 13);
            this.helpLink.TabIndex = 4;
            this.helpLink.Text = "click here";
            this.helpLink.Url = "http://www.interapptive.com/shipworks/help";
            // 
            // OrderDynamicsAccountPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.helpLabel);
            this.Controls.Add(this.helpLink);
            this.Controls.Add(this.accountSettingsControl);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "OrderDynamicsAccountPage";
            this.Size = new System.Drawing.Size(522, 206);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OrderDynamicsAccountSettingsControl accountSettingsControl;
        private System.Windows.Forms.Label helpLabel;
        private ApplicationCore.Interaction.HelpLink helpLink;
    }
}
