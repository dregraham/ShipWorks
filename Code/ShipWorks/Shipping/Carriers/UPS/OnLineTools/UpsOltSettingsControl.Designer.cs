namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    partial class UpsOltSettingsControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.shippingCutoff = new ShipWorks.Shipping.Editing.ShippingDateCutoffControl();
            this.oneBalanceUpsBannerControl = new ShipWorks.Shipping.Carriers.UPS.OneBalanceUpsBannerControl();
            this.panel = new System.Windows.Forms.Panel();
            this.panelMailInnovations = new System.Windows.Forms.Panel();
            this.upsMailInnovationsOptions = new ShipWorks.Shipping.Carriers.UPS.WorldShip.UpsMailInnovationsOptionsControl();
            this.labelInfo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.servicesPanel = new System.Windows.Forms.Panel();
            this.shipmentRestrictionsHeader = new System.Windows.Forms.Label();
            this.noDims = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.insuranceProviderChooser = new ShipWorks.Shipping.Insurance.InsuranceProviderChooser();
            this.pennyOne = new System.Windows.Forms.CheckBox();
            this.pennyOneLink = new ShipWorks.UI.Controls.LinkControl();
            this.servicePicker = new ShipWorks.Shipping.Carriers.UPS.OnLineTools.UpsServiceTypeServicePickerControl();
            this.upsPackagingTypeServicePickerControl = new ShipWorks.Shipping.Carriers.UPS.OnLineTools.UpsPackagingTypePickerControl();
            this.panel.SuspendLayout();
            this.panelMailInnovations.SuspendLayout();
            this.servicesPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // accountControl
            // 
            this.accountControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountControl.Location = new System.Drawing.Point(22, 102);
            this.accountControl.Name = "accountControl";
            this.accountControl.Size = new System.Drawing.Size(405, 168);
            this.accountControl.TabIndex = 1;
            // 
            // labelAccounts
            // 
            this.labelAccounts.AutoSize = true;
            this.labelAccounts.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAccounts.Location = new System.Drawing.Point(3, 83);
            this.labelAccounts.Name = "labelAccounts";
            this.labelAccounts.Size = new System.Drawing.Size(84, 13);
            this.labelAccounts.TabIndex = 1;
            this.labelAccounts.Text = "UPS Accounts";
            // 
            // optionsControl
            // 
            this.optionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsControl.Location = new System.Drawing.Point(3, 2);
            this.optionsControl.Name = "optionsControl";
            this.optionsControl.Size = new System.Drawing.Size(388, 49);
            this.optionsControl.TabIndex = 0;
            // 
            // shippingCutoff
            // 
            this.shippingCutoff.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.shippingCutoff.Location = new System.Drawing.Point(19, 49);
            this.shippingCutoff.Name = "shippingCutoff";
            this.shippingCutoff.Size = new System.Drawing.Size(467, 22);
            this.shippingCutoff.TabIndex = 14;
            // 
            // oneBalanceUpsBannerControl
            // 
            this.oneBalanceUpsBannerControl.Location = new System.Drawing.Point(0, 0);
            this.oneBalanceUpsBannerControl.Margin = new System.Windows.Forms.Padding(10);
            this.oneBalanceUpsBannerControl.Name = "oneBalanceUpsBannerControl";
            this.oneBalanceUpsBannerControl.Size = new System.Drawing.Size(503, 90);
            this.oneBalanceUpsBannerControl.TabIndex = 15;
            // 
            // panel
            // 
            this.panel.Controls.Add(this.panelMailInnovations);
            this.panel.Controls.Add(this.servicesPanel);
            this.panel.Controls.Add(this.optionsControl);
            this.panel.Controls.Add(this.accountControl);
            this.panel.Controls.Add(this.shippingCutoff);
            this.panel.Controls.Add(this.labelAccounts);
            this.panel.Location = new System.Drawing.Point(4, 89);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(570, 940);
            this.panel.TabIndex = 16;
            // 
            // panelMailInnovations
            // 
            this.panelMailInnovations.Controls.Add(this.upsMailInnovationsOptions);
            this.panelMailInnovations.Controls.Add(this.labelInfo);
            this.panelMailInnovations.Controls.Add(this.label2);
            this.panelMailInnovations.Location = new System.Drawing.Point(-3, 273);
            this.panelMailInnovations.Name = "panelMailInnovations";
            this.panelMailInnovations.Size = new System.Drawing.Size(471, 65);
            this.panelMailInnovations.TabIndex = 17;
            // 
            // upsMailInnovationsOptions
            // 
            this.upsMailInnovationsOptions.Location = new System.Drawing.Point(22, 38);
            this.upsMailInnovationsOptions.Name = "upsMailInnovationsOptions";
            this.upsMailInnovationsOptions.Size = new System.Drawing.Size(150, 23);
            this.upsMailInnovationsOptions.TabIndex = 14;
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelInfo.Location = new System.Drawing.Point(7, 17);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(292, 13);
            this.labelInfo.TabIndex = 13;
            this.labelInfo.Text = "Select the services that are enabled on your UPS accounts.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "UPS Services";
            // 
            // servicesPanel
            // 
            this.servicesPanel.Controls.Add(this.shipmentRestrictionsHeader);
            this.servicesPanel.Controls.Add(this.noDims);
            this.servicesPanel.Controls.Add(this.label1);
            this.servicesPanel.Controls.Add(this.insuranceProviderChooser);
            this.servicesPanel.Controls.Add(this.pennyOne);
            this.servicesPanel.Controls.Add(this.pennyOneLink);
            this.servicesPanel.Controls.Add(this.servicePicker);
            this.servicesPanel.Controls.Add(this.upsPackagingTypeServicePickerControl);
            this.servicesPanel.Location = new System.Drawing.Point(5, 344);
            this.servicesPanel.Name = "servicesPanel";
            this.servicesPanel.Size = new System.Drawing.Size(564, 570);
            this.servicesPanel.TabIndex = 16;
            // 
            // shipmentRestrictionsHeader
            // 
            this.shipmentRestrictionsHeader.AutoSize = true;
            this.shipmentRestrictionsHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shipmentRestrictionsHeader.Location = new System.Drawing.Point(-1, 530);
            this.shipmentRestrictionsHeader.Name = "shipmentRestrictionsHeader";
            this.shipmentRestrictionsHeader.Size = new System.Drawing.Size(123, 13);
            this.shipmentRestrictionsHeader.TabIndex = 18;
            this.shipmentRestrictionsHeader.Text = "Shipment Restrictions";
            // 
            // noDims
            // 
            this.noDims.AutoSize = true;
            this.noDims.Location = new System.Drawing.Point(17, 550);
            this.noDims.Name = "noDims";
            this.noDims.Size = new System.Drawing.Size(298, 17);
            this.noDims.TabIndex = 19;
            this.noDims.Text = "Allow UPS to provide rates and process labels when no dimensions are entered.";
            this.noDims.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(-1, 437);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Shipment Protection";
            // 
            // insuranceProviderChooser
            // 
            this.insuranceProviderChooser.CarrierMessage = "(UPS Declared Value is not insurance)";
            this.insuranceProviderChooser.CarrierProviderName = "UPS Declared Value";
            this.insuranceProviderChooser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceProviderChooser.Location = new System.Drawing.Point(16, 456);
            this.insuranceProviderChooser.Name = "insuranceProviderChooser";
            this.insuranceProviderChooser.Size = new System.Drawing.Size(407, 30);
            this.insuranceProviderChooser.TabIndex = 15;
            // 
            // pennyOne
            // 
            this.pennyOne.AutoSize = true;
            this.pennyOne.Location = new System.Drawing.Point(17, 491);
            this.pennyOne.Name = "pennyOne";
            this.pennyOne.Size = new System.Drawing.Size(290, 17);
            this.pennyOne.TabIndex = 16;
            this.pennyOne.Text = "Use ShipWorks Insurance for the first $100 of coverage.";
            this.pennyOne.UseVisualStyleBackColor = true;
            // 
            // pennyOneLink
            // 
            this.pennyOneLink.AutoSize = true;
            this.pennyOneLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pennyOneLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.pennyOneLink.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.pennyOneLink.Location = new System.Drawing.Point(313, 491);
            this.pennyOneLink.Name = "pennyOneLink";
            this.pennyOneLink.Size = new System.Drawing.Size(65, 13);
            this.pennyOneLink.TabIndex = 17;
            this.pennyOneLink.Text = "(Learn why)";
            // 
            // servicePicker
            // 
            this.servicePicker.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.servicePicker.Location = new System.Drawing.Point(-1, 3);
            this.servicePicker.Name = "servicePicker";
            this.servicePicker.Size = new System.Drawing.Size(421, 200);
            this.servicePicker.TabIndex = 12;
            // 
            // upsPackagingTypeServicePickerControl
            // 
            this.upsPackagingTypeServicePickerControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.upsPackagingTypeServicePickerControl.Location = new System.Drawing.Point(-1, 220);
            this.upsPackagingTypeServicePickerControl.Name = "upsPackagingTypeServicePickerControl";
            this.upsPackagingTypeServicePickerControl.Size = new System.Drawing.Size(421, 200);
            this.upsPackagingTypeServicePickerControl.TabIndex = 13;
            // 
            // UpsOltSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.oneBalanceUpsBannerControl);
            this.Controls.Add(this.panel);
            this.Name = "UpsOltSettingsControl";
            this.Size = new System.Drawing.Size(366, 722);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.panelMailInnovations.ResumeLayout(false);
            this.panelMailInnovations.PerformLayout();
            this.servicesPanel.ResumeLayout(false);
            this.servicesPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UpsAccountManagerControl accountControl;
        private System.Windows.Forms.Label labelAccounts;
        private UpsOltOptionsControl optionsControl;
        private Editing.ShippingDateCutoffControl shippingCutoff;
        private OneBalanceUpsBannerControl oneBalanceUpsBannerControl;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Panel servicesPanel;
        private System.Windows.Forms.Label shipmentRestrictionsHeader;
        private System.Windows.Forms.CheckBox noDims;
        private System.Windows.Forms.Label label1;
        private Insurance.InsuranceProviderChooser insuranceProviderChooser;
        private System.Windows.Forms.CheckBox pennyOne;
        private UI.Controls.LinkControl pennyOneLink;
        private UpsServiceTypeServicePickerControl servicePicker;
        private UpsPackagingTypePickerControl upsPackagingTypeServicePickerControl;
        private System.Windows.Forms.Panel panelMailInnovations;
        private WorldShip.UpsMailInnovationsOptionsControl upsMailInnovationsOptions;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Label label2;
    }
}
