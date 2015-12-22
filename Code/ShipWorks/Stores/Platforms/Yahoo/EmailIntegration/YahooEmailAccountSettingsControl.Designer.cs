namespace ShipWorks.Stores.Platforms.Yahoo.EmailIntegration
{
    partial class YahooEmailAccountSettingsControl
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
            this.labelInfo2 = new System.Windows.Forms.Label();
            this.trackingPassword = new System.Windows.Forms.TextBox();
            this.labelTrackingPassword = new System.Windows.Forms.Label();
            this.linkHelp = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            this.labelTrackingPasswordInfo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.emailAccountControl = new YahooEmailAccountControl();
            this.SuspendLayout();
            // 
            // labelStatusUpdates
            // 
            this.labelStatusUpdates.AutoSize = true;
            this.labelStatusUpdates.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelStatusUpdates.Location = new System.Drawing.Point(6, 107);
            this.labelStatusUpdates.Name = "labelStatusUpdates";
            this.labelStatusUpdates.Size = new System.Drawing.Size(94, 13);
            this.labelStatusUpdates.TabIndex = 8;
            this.labelStatusUpdates.Text = "Status Updates";
            // 
            // labelInfo2
            // 
            this.labelInfo2.AutoSize = true;
            this.labelInfo2.Location = new System.Drawing.Point(73, 161);
            this.labelInfo2.Name = "labelInfo2";
            this.labelInfo2.Size = new System.Drawing.Size(301, 13);
            this.labelInfo2.TabIndex = 13;
            this.labelInfo2.Text = "to learn how to enable and get your email tracking password.";
            // 
            // trackingPassword
            // 
            this.trackingPassword.Location = new System.Drawing.Point(200, 181);
            this.trackingPassword.Name = "trackingPassword";
            this.trackingPassword.Size = new System.Drawing.Size(224, 21);
            this.trackingPassword.TabIndex = 12;
            // 
            // labelTrackingPassword
            // 
            this.labelTrackingPassword.AutoSize = true;
            this.labelTrackingPassword.Location = new System.Drawing.Point(22, 184);
            this.labelTrackingPassword.Name = "labelTrackingPassword";
            this.labelTrackingPassword.Size = new System.Drawing.Size(172, 13);
            this.labelTrackingPassword.TabIndex = 10;
            this.labelTrackingPassword.Text = "Yahoo! \"Email Tracking Password\":";
            // 
            // linkHelp
            // 
            this.linkHelp.AutoSize = true;
            this.linkHelp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkHelp.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkHelp.ForeColor = System.Drawing.Color.Blue;
            this.linkHelp.Location = new System.Drawing.Point(22, 160);
            this.linkHelp.Name = "linkHelp";
            this.linkHelp.Size = new System.Drawing.Size(53, 13);
            this.linkHelp.TabIndex = 11;
            this.linkHelp.Text = "Click here";
            // 
            // labelTrackingPasswordInfo
            // 
            this.labelTrackingPasswordInfo.Location = new System.Drawing.Point(7, 125);
            this.labelTrackingPasswordInfo.Name = "labelTrackingPasswordInfo";
            this.labelTrackingPasswordInfo.Size = new System.Drawing.Size(443, 37);
            this.labelTrackingPasswordInfo.TabIndex = 9;
            this.labelTrackingPasswordInfo.Text = "ShipWorks can update Yahoo! order status and shipment tracking information.  You " +
                "must enable this feature in your Yahoo! store and get your \"Email Tracking Passw" +
                "ord\".";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label1.Location = new System.Drawing.Point(6, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Downloading";
            // 
            // emailAccountControl
            // 
            this.emailAccountControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.emailAccountControl.Location = new System.Drawing.Point(15, 24);
            this.emailAccountControl.Name = "emailAccountControl";
            this.emailAccountControl.Size = new System.Drawing.Size(309, 76);
            this.emailAccountControl.TabIndex = 15;
            // 
            // YahooAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.emailAccountControl);
            this.Controls.Add(this.labelInfo2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelStatusUpdates);
            this.Controls.Add(this.trackingPassword);
            this.Controls.Add(this.labelTrackingPassword);
            this.Controls.Add(this.linkHelp);
            this.Controls.Add(this.labelTrackingPasswordInfo);
            this.Name = "YahooAccountSettingsControl";
            this.Size = new System.Drawing.Size(513, 356);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelStatusUpdates;
        private System.Windows.Forms.Label labelInfo2;
        private System.Windows.Forms.TextBox trackingPassword;
        private System.Windows.Forms.Label labelTrackingPassword;
        private ShipWorks.ApplicationCore.Interaction.HelpLink linkHelp;
        private System.Windows.Forms.Label labelTrackingPasswordInfo;
        private System.Windows.Forms.Label label1;
        private YahooEmailAccountControl emailAccountControl;
    }
}
