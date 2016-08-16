namespace ShipWorks.Stores.Platforms.Yahoo.EmailIntegration.WizardPages
{
    partial class YahooOnlineUpdatePage
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
            this.labelStatusUpdates = new System.Windows.Forms.Label();
            this.labelTrackingPasswordInfo = new System.Windows.Forms.Label();
            this.enableTrackingPassword = new System.Windows.Forms.CheckBox();
            this.labelTrackingPassword = new System.Windows.Forms.Label();
            this.trackingPassword = new System.Windows.Forms.TextBox();
            this.panelOnlineUpdate = new System.Windows.Forms.Panel();
            this.labelInfo2 = new System.Windows.Forms.Label();
            this.linkHelp = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            this.panelOnlineUpdate.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelStatusUpdates
            // 
            this.labelStatusUpdates.AutoSize = true;
            this.labelStatusUpdates.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelStatusUpdates.Location = new System.Drawing.Point(25, 11);
            this.labelStatusUpdates.Name = "labelStatusUpdates";
            this.labelStatusUpdates.Size = new System.Drawing.Size(94, 13);
            this.labelStatusUpdates.TabIndex = 0;
            this.labelStatusUpdates.Text = "Status Updates";
            // 
            // labelTrackingPasswordInfo
            // 
            this.labelTrackingPasswordInfo.Location = new System.Drawing.Point(28, 30);
            this.labelTrackingPasswordInfo.Name = "labelTrackingPasswordInfo";
            this.labelTrackingPasswordInfo.Size = new System.Drawing.Size(443, 37);
            this.labelTrackingPasswordInfo.TabIndex = 1;
            this.labelTrackingPasswordInfo.Text = "ShipWorks can update Yahoo! order status and shipment tracking information.  You " +
                "must enable this feature in your Yahoo! store and get your \"Email Tracking Passw" +
                "ord\".";
            // 
            // enableTrackingPassword
            // 
            this.enableTrackingPassword.AutoSize = true;
            this.enableTrackingPassword.Checked = true;
            this.enableTrackingPassword.CheckState = System.Windows.Forms.CheckState.Checked;
            this.enableTrackingPassword.Location = new System.Drawing.Point(31, 68);
            this.enableTrackingPassword.Name = "enableTrackingPassword";
            this.enableTrackingPassword.Size = new System.Drawing.Size(458, 17);
            this.enableTrackingPassword.TabIndex = 2;
            this.enableTrackingPassword.Text = "I want to be able to update Yahoo! order status and tracking information from Shi" +
                "pWorks.";
            this.enableTrackingPassword.UseVisualStyleBackColor = true;
            this.enableTrackingPassword.CheckedChanged += new System.EventHandler(this.OnEnableTrackingPasswordChanged);
            // 
            // labelTrackingPassword
            // 
            this.labelTrackingPassword.AutoSize = true;
            this.labelTrackingPassword.Location = new System.Drawing.Point(24, 26);
            this.labelTrackingPassword.Name = "labelTrackingPassword";
            this.labelTrackingPassword.Size = new System.Drawing.Size(172, 13);
            this.labelTrackingPassword.TabIndex = 3;
            this.labelTrackingPassword.Text = "Yahoo! \"Email Tracking Password\":";
            // 
            // trackingPassword
            // 
            this.trackingPassword.Location = new System.Drawing.Point(202, 23);
            this.trackingPassword.Name = "trackingPassword";
            this.trackingPassword.Size = new System.Drawing.Size(224, 21);
            this.trackingPassword.TabIndex = 4;
            // 
            // panelOnlineUpdate
            // 
            this.panelOnlineUpdate.Controls.Add(this.labelInfo2);
            this.panelOnlineUpdate.Controls.Add(this.trackingPassword);
            this.panelOnlineUpdate.Controls.Add(this.labelTrackingPassword);
            this.panelOnlineUpdate.Controls.Add(this.linkHelp);
            this.panelOnlineUpdate.Location = new System.Drawing.Point(26, 87);
            this.panelOnlineUpdate.Name = "panelOnlineUpdate";
            this.panelOnlineUpdate.Size = new System.Drawing.Size(461, 49);
            this.panelOnlineUpdate.TabIndex = 5;
            // 
            // labelInfo2
            // 
            this.labelInfo2.AutoSize = true;
            this.labelInfo2.Location = new System.Drawing.Point(70, 3);
            this.labelInfo2.Name = "labelInfo2";
            this.labelInfo2.Size = new System.Drawing.Size(301, 13);
            this.labelInfo2.TabIndex = 5;
            this.labelInfo2.Text = "to learn how to enable and get your email tracking password.";
            // 
            // linkHelp
            // 
            this.linkHelp.AutoSize = true;
            this.linkHelp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkHelp.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkHelp.ForeColor = System.Drawing.Color.Blue;
            this.linkHelp.Location = new System.Drawing.Point(21, 3);
            this.linkHelp.Name = "linkHelp";
            this.linkHelp.Size = new System.Drawing.Size(53, 13);
            this.linkHelp.TabIndex = 4;
            this.linkHelp.Text = "Click here";
            // 
            // YahooOnlineUpdatePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelOnlineUpdate);
            this.Controls.Add(this.enableTrackingPassword);
            this.Controls.Add(this.labelTrackingPasswordInfo);
            this.Controls.Add(this.labelStatusUpdates);
            this.Description = "Enter the following information about your online store.";
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "YahooOnlineUpdatePage";
            this.Size = new System.Drawing.Size(495, 344);
            this.Title = "Store Setup";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.panelOnlineUpdate.ResumeLayout(false);
            this.panelOnlineUpdate.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelStatusUpdates;
        private System.Windows.Forms.Label labelTrackingPasswordInfo;
        private System.Windows.Forms.CheckBox enableTrackingPassword;
        private System.Windows.Forms.Label labelTrackingPassword;
        private System.Windows.Forms.TextBox trackingPassword;
        private System.Windows.Forms.Panel panelOnlineUpdate;
        private System.Windows.Forms.Label labelInfo2;
        private ShipWorks.ApplicationCore.Interaction.HelpLink linkHelp;
    }
}
