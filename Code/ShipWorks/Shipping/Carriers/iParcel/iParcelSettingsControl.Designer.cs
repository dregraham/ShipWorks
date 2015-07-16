namespace ShipWorks.Shipping.Carriers.iParcel
{
    partial class iParcelSettingsControl
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
            this.optionsControl = new ShipWorks.Shipping.Carriers.iParcel.iParcelOptionsControl();
            this.insuranceProtectionPanel = new System.Windows.Forms.Panel();
            this.pennyOneLink = new ShipWorks.UI.Controls.LinkControl();
            this.labelShipWorksInsurance = new System.Windows.Forms.Label();
            this.pennyOne = new System.Windows.Forms.CheckBox();
            this.labelShipmentProtection = new System.Windows.Forms.Label();
            this.insuranceProviderChooser = new ShipWorks.Shipping.Insurance.InsuranceProviderChooser();
            this.accountManager = new ShipWorks.Shipping.Carriers.iParcel.iParcelAccountManagerControl();
            this.managerLabel = new System.Windows.Forms.Label();
            this.servicePicker = new ShipWorks.Shipping.Carriers.iParcel.iParcelServiceTypeServicePickerControl();
            this.insuranceProtectionPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // optionsControl
            // 
            this.optionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsControl.Location = new System.Drawing.Point(6, 3);
            this.optionsControl.Name = "optionsControl";
            this.optionsControl.Size = new System.Drawing.Size(427, 71);
            this.optionsControl.TabIndex = 0;
            // 
            // insuranceProtectionPanel
            // 
            this.insuranceProtectionPanel.Controls.Add(this.pennyOneLink);
            this.insuranceProtectionPanel.Controls.Add(this.labelShipWorksInsurance);
            this.insuranceProtectionPanel.Controls.Add(this.pennyOne);
            this.insuranceProtectionPanel.Controls.Add(this.labelShipmentProtection);
            this.insuranceProtectionPanel.Controls.Add(this.insuranceProviderChooser);
            this.insuranceProtectionPanel.Location = new System.Drawing.Point(0, 487);
            this.insuranceProtectionPanel.Margin = new System.Windows.Forms.Padding(0);
            this.insuranceProtectionPanel.Name = "insuranceProtectionPanel";
            this.insuranceProtectionPanel.Size = new System.Drawing.Size(456, 106);
            this.insuranceProtectionPanel.TabIndex = 18;
            // 
            // pennyOneLink
            // 
            this.pennyOneLink.AutoSize = true;
            this.pennyOneLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pennyOneLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.pennyOneLink.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.pennyOneLink.Location = new System.Drawing.Point(320, 24);
            this.pennyOneLink.Name = "pennyOneLink";
            this.pennyOneLink.Size = new System.Drawing.Size(65, 13);
            this.pennyOneLink.TabIndex = 19;
            this.pennyOneLink.Text = "(Learn why)";
            // 
            // labelShipWorksInsurance
            // 
            this.labelShipWorksInsurance.AutoSize = true;
            this.labelShipWorksInsurance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelShipWorksInsurance.Location = new System.Drawing.Point(8, 3);
            this.labelShipWorksInsurance.Name = "labelShipWorksInsurance";
            this.labelShipWorksInsurance.Size = new System.Drawing.Size(127, 13);
            this.labelShipWorksInsurance.TabIndex = 20;
            this.labelShipWorksInsurance.Text = "ShipWorks Insurance";
            // 
            // pennyOne
            // 
            this.pennyOne.AutoSize = true;
            this.pennyOne.Location = new System.Drawing.Point(27, 23);
            this.pennyOne.Name = "pennyOne";
            this.pennyOne.Size = new System.Drawing.Size(298, 17);
            this.pennyOne.TabIndex = 18;
            this.pennyOne.Text = "Use ShipWorks Insurance for the first $100 of coverage.";
            this.pennyOne.UseVisualStyleBackColor = true;
            // 
            // labelShipmentProtection
            // 
            this.labelShipmentProtection.AutoSize = true;
            this.labelShipmentProtection.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelShipmentProtection.Location = new System.Drawing.Point(8, 51);
            this.labelShipmentProtection.Name = "labelShipmentProtection";
            this.labelShipmentProtection.Size = new System.Drawing.Size(123, 13);
            this.labelShipmentProtection.TabIndex = 7;
            this.labelShipmentProtection.Text = "Shipment Protection";
            // 
            // insuranceProviderChooser
            // 
            this.insuranceProviderChooser.CarrierMessage = "";
            this.insuranceProviderChooser.CarrierProviderName = "i-parcel Insurance";
            this.insuranceProviderChooser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceProviderChooser.Location = new System.Drawing.Point(26, 67);
            this.insuranceProviderChooser.Name = "insuranceProviderChooser";
            this.insuranceProviderChooser.Size = new System.Drawing.Size(407, 30);
            this.insuranceProviderChooser.TabIndex = 8;
            this.insuranceProviderChooser.ProviderChanged += new System.EventHandler(this.OnInsuranceProviderChanged);
            // 
            // accountManager
            // 
            this.accountManager.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountManager.Location = new System.Drawing.Point(27, 99);
            this.accountManager.Name = "accountManager";
            this.accountManager.Size = new System.Drawing.Size(400, 168);
            this.accountManager.TabIndex = 19;
            // 
            // managerLabel
            // 
            this.managerLabel.AutoSize = true;
            this.managerLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.managerLabel.Location = new System.Drawing.Point(8, 78);
            this.managerLabel.Name = "managerLabel";
            this.managerLabel.Size = new System.Drawing.Size(105, 13);
            this.managerLabel.TabIndex = 20;
            this.managerLabel.Text = "i-parcel Accounts";
            // 
            // servicePicker
            // 
            this.servicePicker.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.servicePicker.Location = new System.Drawing.Point(9, 280);
            this.servicePicker.Name = "servicePicker";
            this.servicePicker.Size = new System.Drawing.Size(447, 200);
            this.servicePicker.TabIndex = 21;
            // 
            // iParcelSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.servicePicker);
            this.Controls.Add(this.managerLabel);
            this.Controls.Add(this.accountManager);
            this.Controls.Add(this.insuranceProtectionPanel);
            this.Controls.Add(this.optionsControl);
            this.Name = "iParcelSettingsControl";
            this.Size = new System.Drawing.Size(458, 605);
            this.insuranceProtectionPanel.ResumeLayout(false);
            this.insuranceProtectionPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private iParcelOptionsControl optionsControl;
        private System.Windows.Forms.Panel insuranceProtectionPanel;
        private System.Windows.Forms.Label labelShipmentProtection;
        private Insurance.InsuranceProviderChooser insuranceProviderChooser;
        private iParcelAccountManagerControl accountManager;
        private System.Windows.Forms.Label managerLabel;
        private UI.Controls.LinkControl pennyOneLink;
        private System.Windows.Forms.Label labelShipWorksInsurance;
        private System.Windows.Forms.CheckBox pennyOne;
        private iParcelServiceTypeServicePickerControl servicePicker;
    }
}
