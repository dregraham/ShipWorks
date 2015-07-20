using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    partial class UspsSettingsControl
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
            this.labelAccountType = new System.Windows.Forms.Label();
            this.accountControl = new ShipWorks.Shipping.Carriers.Postal.Usps.UspsAccountManagerControl();
            this.optionsControl = new ShipWorks.Shipping.Carriers.Postal.Usps.UspsOptionsControl();
            this.express1Options = new ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Express1UspsSingleSourceControl();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.express1SettingsControl = new ShipWorks.Shipping.Carriers.Postal.Express1.AutomaticExpress1ControlBase();
            this.panelInsurance = new System.Windows.Forms.Panel();
            this.insuranceProviderChooser = new ShipWorks.Shipping.Insurance.InsuranceProviderChooser();
            this.labelShipmentProtection = new System.Windows.Forms.Label();
            this.servicePicker = new ShipWorks.Shipping.Carriers.Postal.PostalServicePickerControl();
            this.packagePicker = new ShipWorks.Shipping.Carriers.Postal.PostalPackagePickerControl();
            this.panelBottom.SuspendLayout();
            this.panelInsurance.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelAccountType
            // 
            this.labelAccountType.AutoSize = true;
            this.labelAccountType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAccountType.Location = new System.Drawing.Point(3, 5);
            this.labelAccountType.Name = "labelAccountType";
            this.labelAccountType.Size = new System.Drawing.Size(91, 13);
            this.labelAccountType.TabIndex = 0;
            this.labelAccountType.Text = "USPS Accounts";
            // 
            // accountControl
            // 
            this.accountControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountControl.Location = new System.Drawing.Point(19, 21);
            this.accountControl.Name = "accountControl";
            this.accountControl.Size = new System.Drawing.Size(459, 104);
            this.accountControl.TabIndex = 1;
            this.accountControl.UspsResellerType = ShipWorks.Shipping.Carriers.Postal.Usps.UspsResellerType.None;
            // 
            // optionsControl
            // 
            this.optionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsControl.Location = new System.Drawing.Point(0, -1);
            this.optionsControl.Name = "optionsControl";
            this.optionsControl.ShipmentTypeCode = ShipWorks.Shipping.ShipmentTypeCode.Usps;
            this.optionsControl.Size = new System.Drawing.Size(435, 54);
            this.optionsControl.TabIndex = 4;
            // 
            // express1Options
            // 
            this.express1Options.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.express1Options.Location = new System.Drawing.Point(4, 59);
            this.express1Options.Name = "express1Options";
            this.express1Options.Size = new System.Drawing.Size(421, 41);
            this.express1Options.TabIndex = 5;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.labelAccountType);
            this.panelBottom.Controls.Add(this.accountControl);
            this.panelBottom.Location = new System.Drawing.Point(5, 278);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(486, 131);
            this.panelBottom.TabIndex = 6;
            // 
            // express1SettingsControl
            // 
            this.express1SettingsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.express1SettingsControl.Location = new System.Drawing.Point(5, 112);
            this.express1SettingsControl.Name = "express1SettingsControl";
            this.express1SettingsControl.Size = new System.Drawing.Size(468, 160);
            this.express1SettingsControl.TabIndex = 7;
            // 
            // panelInsurance
            // 
            this.panelInsurance.Controls.Add(this.insuranceProviderChooser);
            this.panelInsurance.Controls.Add(this.labelShipmentProtection);
            this.panelInsurance.Location = new System.Drawing.Point(5, 828);
            this.panelInsurance.Name = "panelInsurance";
            this.panelInsurance.Size = new System.Drawing.Size(386, 50);
            this.panelInsurance.TabIndex = 8;
            // 
            // insuranceProviderChooser
            // 
            this.insuranceProviderChooser.CarrierMessage = "(UPS Declared Value is not insurance)";
            this.insuranceProviderChooser.CarrierProviderName = "Stamps.com Insurance";
            this.insuranceProviderChooser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceProviderChooser.Location = new System.Drawing.Point(20, 23);
            this.insuranceProviderChooser.Name = "insuranceProviderChooser";
            this.insuranceProviderChooser.Size = new System.Drawing.Size(193, 30);
            this.insuranceProviderChooser.TabIndex = 10;
            // 
            // labelShipmentProtection
            // 
            this.labelShipmentProtection.AutoSize = true;
            this.labelShipmentProtection.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelShipmentProtection.Location = new System.Drawing.Point(3, 4);
            this.labelShipmentProtection.Name = "labelShipmentProtection";
            this.labelShipmentProtection.Size = new System.Drawing.Size(123, 13);
            this.labelShipmentProtection.TabIndex = 9;
            this.labelShipmentProtection.Text = "Shipment Protection";
            // 
            // servicePicker
            // 
            this.servicePicker.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.servicePicker.Location = new System.Drawing.Point(9, 412);
            this.servicePicker.Name = "servicePicker";
            this.servicePicker.Size = new System.Drawing.Size(486, 200);
            this.servicePicker.TabIndex = 9;
            // 
            // postalServiceTypeServicePickerControl1
            // 
            this.packagePicker.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.packagePicker.Location = new System.Drawing.Point(9, 622);
            this.packagePicker.Name = "postalServiceTypeServicePickerControl1";
            this.packagePicker.Size = new System.Drawing.Size(486, 200);
            this.packagePicker.TabIndex = 10;
            // 
            // UspsSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.packagePicker);
            this.Controls.Add(this.servicePicker);
            this.Controls.Add(this.panelInsurance);
            this.Controls.Add(this.express1SettingsControl);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.express1Options);
            this.Controls.Add(this.optionsControl);
            this.Name = "UspsSettingsControl";
            this.Size = new System.Drawing.Size(499, 893);
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.panelInsurance.ResumeLayout(false);
            this.panelInsurance.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelAccountType;
        private UspsAccountManagerControl accountControl;
        private UspsOptionsControl optionsControl;
        private Express1UspsSingleSourceControl express1Options;
        private System.Windows.Forms.Panel panelBottom;
        private AutomaticExpress1ControlBase express1SettingsControl;
        private System.Windows.Forms.Panel panelInsurance;
        private Insurance.InsuranceProviderChooser insuranceProviderChooser;
        private System.Windows.Forms.Label labelShipmentProtection;
        private PostalServicePickerControl servicePicker;
        private PostalPackagePickerControl packagePicker;
    }
}
