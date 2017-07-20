namespace ShipWorks.Stores.UI.Platforms.ChannelAdvisor
{
    partial class ChannelAdvisorAccountSettingsControl
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
            this.upgradeButton = new System.Windows.Forms.Button();
            this.soapPanel = new System.Windows.Forms.Panel();
            this.restSettingsControl = new ShipWorks.Stores.UI.Platforms.ChannelAdvisor.ChannelAdvisorRestAccountSettingsControl();
            this.soapSettingsControl = new ShipWorks.Stores.UI.Platforms.ChannelAdvisor.ChannelAdvisorSoapAccountSettingsControl();
            this.soapPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // upgradeButton
            //
            this.upgradeButton.Location = new System.Drawing.Point(187, 179);
            this.upgradeButton.Name = "upgradeButton";
            this.upgradeButton.Size = new System.Drawing.Size(134, 23);
            this.upgradeButton.TabIndex = 1;
            this.upgradeButton.Text = "Upgrade to REST API";
            this.upgradeButton.UseVisualStyleBackColor = true;
            this.upgradeButton.Click += new System.EventHandler(this.OnClickUpgrade);
            //
            // soapPanel
            //
            this.soapPanel.Controls.Add(this.upgradeButton);
            this.soapPanel.Controls.Add(this.soapSettingsControl);
            this.soapPanel.Location = new System.Drawing.Point(0, 0);
            this.soapPanel.Name = "soapPanel";
            this.soapPanel.Size = new System.Drawing.Size(598, 230);
            this.soapPanel.TabIndex = 0;
            //
            // restSettingsControl
            //
            this.restSettingsControl.Enabled = false;
            this.restSettingsControl.Location = new System.Drawing.Point(0, 0);
            this.restSettingsControl.Name = "restSettingsControl";
            this.restSettingsControl.Size = new System.Drawing.Size(598, 173);
            this.restSettingsControl.TabIndex = 3;
            this.restSettingsControl.Visible = false;
            //
            // soapSettingsControl
            //
            this.soapSettingsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.soapSettingsControl.Location = new System.Drawing.Point(0, 0);
            this.soapSettingsControl.Name = "soapSettingsControl";
            this.soapSettingsControl.Size = new System.Drawing.Size(598, 224);
            this.soapSettingsControl.TabIndex = 0;
            //
            // ChannelAdvisorAccountSettingsControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.restSettingsControl);
            this.Controls.Add(this.soapPanel);
            this.Name = "ChannelAdvisorAccountSettingsControl";
            this.Size = new System.Drawing.Size(600, 233);
            this.soapPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ChannelAdvisorSoapAccountSettingsControl soapSettingsControl;
        private System.Windows.Forms.Button upgradeButton;
        private System.Windows.Forms.Panel soapPanel;
        private ChannelAdvisorRestAccountSettingsControl restSettingsControl;
    }
}
