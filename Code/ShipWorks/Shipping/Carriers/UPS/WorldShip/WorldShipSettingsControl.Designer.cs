namespace ShipWorks.Shipping.Carriers.UPS.WorldShip
{
    partial class WorldShipSettingsControl
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
            this.labelAccounts = new System.Windows.Forms.Label();
            this.accountControl = new ShipWorks.Shipping.Carriers.UPS.UpsAccountManagerControl();
            this.optionsControl = new ShipWorks.Shipping.Carriers.UPS.WorldShip.WorldShipOptionsControl();
            this.upsMailInnovationsOptions = new ShipWorks.Shipping.Carriers.UPS.WorldShip.UpsMailInnovationsOptionsControl();
            this.labelInfo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panelExclusionConfiguration = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // labelAccounts
            // 
            this.labelAccounts.AutoSize = true;
            this.labelAccounts.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAccounts.Location = new System.Drawing.Point(8, 206);
            this.labelAccounts.Name = "labelAccounts";
            this.labelAccounts.Size = new System.Drawing.Size(84, 13);
            this.labelAccounts.TabIndex = 1;
            this.labelAccounts.Text = "UPS Accounts";
            // 
            // accountControl
            // 
            this.accountControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountControl.Location = new System.Drawing.Point(27, 227);
            this.accountControl.Name = "accountControl";
            this.accountControl.Size = new System.Drawing.Size(404, 168);
            this.accountControl.TabIndex = 2;
            // 
            // optionsControl
            // 
            this.optionsControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsControl.Location = new System.Drawing.Point(8, 3);
            this.optionsControl.Name = "optionsControl";
            this.optionsControl.Size = new System.Drawing.Size(407, 192);
            this.optionsControl.TabIndex = 0;
            // 
            // upsMailInnovationsOptions
            // 
            this.upsMailInnovationsOptions.Location = new System.Drawing.Point(24, 447);
            this.upsMailInnovationsOptions.Name = "upsMailInnovationsOptions";
            this.upsMailInnovationsOptions.Size = new System.Drawing.Size(150, 46);
            this.upsMailInnovationsOptions.TabIndex = 14;
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelInfo.Location = new System.Drawing.Point(9, 426);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(292, 13);
            this.labelInfo.TabIndex = 13;
            this.labelInfo.Text = "Select the services that are enabled on your UPS accounts.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 408);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "UPS Services";
            // 
            // panelExclusionConfiguration
            // 
            this.panelExclusionConfiguration.Location = new System.Drawing.Point(11, 479);
            this.panelExclusionConfiguration.Name = "panelExclusionConfiguration";
            this.panelExclusionConfiguration.Size = new System.Drawing.Size(421, 200);
            this.panelExclusionConfiguration.TabIndex = 15;
            // 
            // WorldShipSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panelExclusionConfiguration);
            this.Controls.Add(this.upsMailInnovationsOptions);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.optionsControl);
            this.Controls.Add(this.labelAccounts);
            this.Controls.Add(this.accountControl);
            this.Name = "WorldShipSettingsControl";
            this.Size = new System.Drawing.Size(468, 704);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAccounts;
        private UpsAccountManagerControl accountControl;
        private WorldShipOptionsControl optionsControl;
        private UpsMailInnovationsOptionsControl upsMailInnovationsOptions;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelExclusionConfiguration;
    }
}
