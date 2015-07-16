namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    partial class UpsOltSettingsControl
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
            this.accountControl = new ShipWorks.Shipping.Carriers.UPS.UpsAccountManagerControl();
            this.labelAccounts = new System.Windows.Forms.Label();
            this.optionsControl = new ShipWorks.Shipping.Carriers.UPS.OnLineTools.UpsOltOptionsControl();
            this.insuranceProviderChooser = new ShipWorks.Shipping.Insurance.InsuranceProviderChooser();
            this.label1 = new System.Windows.Forms.Label();
            this.pennyOneLink = new ShipWorks.UI.Controls.LinkControl();
            this.pennyOne = new System.Windows.Forms.CheckBox();
            this.upsMailInnovationsOptions = new ShipWorks.Shipping.Carriers.UPS.WorldShip.UpsMailInnovationsOptionsControl();
            this.labelInfo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panelExclusionConfiguration = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // accountControl
            // 
            this.accountControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountControl.Location = new System.Drawing.Point(27, 79);
            this.accountControl.Name = "accountControl";
            this.accountControl.Size = new System.Drawing.Size(405, 168);
            this.accountControl.TabIndex = 1;
            // 
            // labelAccounts
            // 
            this.labelAccounts.AutoSize = true;
            this.labelAccounts.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAccounts.Location = new System.Drawing.Point(8, 60);
            this.labelAccounts.Name = "labelAccounts";
            this.labelAccounts.Size = new System.Drawing.Size(84, 13);
            this.labelAccounts.TabIndex = 1;
            this.labelAccounts.Text = "UPS Accounts";
            // 
            // optionsControl
            // 
            this.optionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsControl.Location = new System.Drawing.Point(8, 4);
            this.optionsControl.Name = "optionsControl";
            this.optionsControl.Size = new System.Drawing.Size(409, 52);
            this.optionsControl.TabIndex = 0;
            // 
            // insuranceProviderChooser
            // 
            this.insuranceProviderChooser.CarrierMessage = "(UPS Declared Value is not insurance)";
            this.insuranceProviderChooser.CarrierProviderName = "UPS Declared Value";
            this.insuranceProviderChooser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceProviderChooser.Location = new System.Drawing.Point(26, 280);
            this.insuranceProviderChooser.Name = "insuranceProviderChooser";
            this.insuranceProviderChooser.Size = new System.Drawing.Size(407, 30);
            this.insuranceProviderChooser.TabIndex = 6;
            this.insuranceProviderChooser.ProviderChanged += new System.EventHandler(this.OnInsuranceProviderChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 261);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Shipment Protection";
            // 
            // pennyOneLink
            // 
            this.pennyOneLink.AutoSize = true;
            this.pennyOneLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pennyOneLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.pennyOneLink.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.pennyOneLink.Location = new System.Drawing.Point(319, 312);
            this.pennyOneLink.Name = "pennyOneLink";
            this.pennyOneLink.Size = new System.Drawing.Size(65, 13);
            this.pennyOneLink.TabIndex = 8;
            this.pennyOneLink.Text = "(Learn why)";
            this.pennyOneLink.Click += new System.EventHandler(this.OnLinkPennyOne);
            // 
            // pennyOne
            // 
            this.pennyOne.AutoSize = true;
            this.pennyOne.Location = new System.Drawing.Point(27, 311);
            this.pennyOne.Name = "pennyOne";
            this.pennyOne.Size = new System.Drawing.Size(298, 17);
            this.pennyOne.TabIndex = 7;
            this.pennyOne.Text = "Use ShipWorks Insurance for the first $100 of coverage.";
            this.pennyOne.UseVisualStyleBackColor = true;
            // 
            // upsMailInnovationsOptions
            // 
            this.upsMailInnovationsOptions.Location = new System.Drawing.Point(24, 375);
            this.upsMailInnovationsOptions.Name = "upsMailInnovationsOptions";
            this.upsMailInnovationsOptions.Size = new System.Drawing.Size(150, 23);
            this.upsMailInnovationsOptions.TabIndex = 11;
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelInfo.Location = new System.Drawing.Point(9, 354);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(292, 13);
            this.labelInfo.TabIndex = 10;
            this.labelInfo.Text = "Select the services that are enabled on your UPS accounts.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 339);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "UPS Services";
            // 
            // panelExclusionConfiguration
            // 
            this.panelExclusionConfiguration.Location = new System.Drawing.Point(11, 404);
            this.panelExclusionConfiguration.Name = "panelExclusionConfiguration";
            this.panelExclusionConfiguration.Size = new System.Drawing.Size(421, 200);
            this.panelExclusionConfiguration.TabIndex = 12;
            // 
            // UpsOltSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelExclusionConfiguration);
            this.Controls.Add(this.upsMailInnovationsOptions);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pennyOneLink);
            this.Controls.Add(this.pennyOne);
            this.Controls.Add(this.insuranceProviderChooser);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.optionsControl);
            this.Controls.Add(this.labelAccounts);
            this.Controls.Add(this.accountControl);
            this.Name = "UpsOltSettingsControl";
            this.Size = new System.Drawing.Size(440, 617);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UpsAccountManagerControl accountControl;
        private System.Windows.Forms.Label labelAccounts;
        private UpsOltOptionsControl optionsControl;
        private Insurance.InsuranceProviderChooser insuranceProviderChooser;
        private System.Windows.Forms.Label label1;
        private UI.Controls.LinkControl pennyOneLink;
        private System.Windows.Forms.CheckBox pennyOne;
        private WorldShip.UpsMailInnovationsOptionsControl upsMailInnovationsOptions;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelExclusionConfiguration;
    }
}
