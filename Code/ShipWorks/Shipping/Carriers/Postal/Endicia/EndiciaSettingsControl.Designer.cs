using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    partial class EndiciaSettingsControl
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
            this.panelBottom = new System.Windows.Forms.Panel();
            this.panelInsurance = new System.Windows.Forms.Panel();
            this.insuranceProviderChooser = new ShipWorks.Shipping.Insurance.InsuranceProviderChooser();
            this.labelShipmentProtection = new System.Windows.Forms.Label();
            this.labelAccountType = new System.Windows.Forms.Label();
            this.accountControl = new ShipWorks.Shipping.Carriers.Postal.Endicia.EndiciaAccountManagerControl();
            this.express1PostageDiscountSettingsControl = new ShipWorks.Shipping.Carriers.Postal.Express1.AutomaticExpress1ControlBase();
            this.express1Options = new ShipWorks.Shipping.Carriers.Postal.Endicia.Express1.Express1EndiciaSingleSourceControl();
            this.optionsControl = new ShipWorks.Shipping.Carriers.Postal.Endicia.EndiciaOptionsControl();
            this.panelExclusionConfiguration = new System.Windows.Forms.Panel();
            this.panelBottom.SuspendLayout();
            this.panelInsurance.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.labelAccountType);
            this.panelBottom.Controls.Add(this.accountControl);
            this.panelBottom.Location = new System.Drawing.Point(0, 357);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(479, 200);
            this.panelBottom.TabIndex = 5;
            // 
            // panelInsurance
            // 
            this.panelInsurance.Controls.Add(this.insuranceProviderChooser);
            this.panelInsurance.Controls.Add(this.labelShipmentProtection);
            this.panelInsurance.Location = new System.Drawing.Point(0, 769);
            this.panelInsurance.Name = "panelInsurance";
            this.panelInsurance.Size = new System.Drawing.Size(386, 56);
            this.panelInsurance.TabIndex = 6;
            // 
            // insuranceProviderChooser
            // 
            this.insuranceProviderChooser.CarrierMessage = "(UPS Declared Value is not insurance)";
            this.insuranceProviderChooser.CarrierProviderName = "Endicia Insurance";
            this.insuranceProviderChooser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceProviderChooser.Location = new System.Drawing.Point(20, 25);
            this.insuranceProviderChooser.Name = "insuranceProviderChooser";
            this.insuranceProviderChooser.Size = new System.Drawing.Size(193, 30);
            this.insuranceProviderChooser.TabIndex = 10;
            // 
            // labelShipmentProtection
            // 
            this.labelShipmentProtection.AutoSize = true;
            this.labelShipmentProtection.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelShipmentProtection.Location = new System.Drawing.Point(3, 6);
            this.labelShipmentProtection.Name = "labelShipmentProtection";
            this.labelShipmentProtection.Size = new System.Drawing.Size(123, 13);
            this.labelShipmentProtection.TabIndex = 9;
            this.labelShipmentProtection.Text = "Shipment Protection";
            // 
            // labelAccountType
            // 
            this.labelAccountType.AutoSize = true;
            this.labelAccountType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAccountType.Location = new System.Drawing.Point(8, 5);
            this.labelAccountType.Name = "labelAccountType";
            this.labelAccountType.Size = new System.Drawing.Size(101, 13);
            this.labelAccountType.TabIndex = 1;
            this.labelAccountType.Text = "Endicia Accounts";
            // 
            // accountControl
            // 
            this.accountControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountControl.Location = new System.Drawing.Point(24, 23);
            this.accountControl.Name = "accountControl";
            this.accountControl.Size = new System.Drawing.Size(449, 113);
            this.accountControl.TabIndex = 2;
            // 
            // express1PostageDiscountSettingsControl
            // 
            this.express1PostageDiscountSettingsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.express1PostageDiscountSettingsControl.Location = new System.Drawing.Point(5, 196);
            this.express1PostageDiscountSettingsControl.Name = "express1PostageDiscountSettingsControl";
            this.express1PostageDiscountSettingsControl.Size = new System.Drawing.Size(454, 163);
            this.express1PostageDiscountSettingsControl.TabIndex = 6;
            // 
            // express1Options
            // 
            this.express1Options.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.express1Options.Location = new System.Drawing.Point(4, 149);
            this.express1Options.Name = "express1Options";
            this.express1Options.Size = new System.Drawing.Size(421, 41);
            this.express1Options.TabIndex = 4;
            // 
            // optionsControl
            // 
            this.optionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsControl.Location = new System.Drawing.Point(0, -1);
            this.optionsControl.Name = "optionsControl";
            this.optionsControl.Reseller = ShipWorks.Shipping.Carriers.Postal.Endicia.EndiciaReseller.None;
            this.optionsControl.Size = new System.Drawing.Size(454, 144);
            this.optionsControl.TabIndex = 0;
            // 
            // panelExclusionConfiguration
            // 
            this.panelExclusionConfiguration.Location = new System.Drawing.Point(0, 563);
            this.panelExclusionConfiguration.Name = "panelExclusionConfiguration";
            this.panelExclusionConfiguration.Size = new System.Drawing.Size(486, 200);
            this.panelExclusionConfiguration.TabIndex = 10;
            // 
            // EndiciaSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panelExclusionConfiguration);
            this.Controls.Add(this.panelInsurance);
            this.Controls.Add(this.express1PostageDiscountSettingsControl);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.express1Options);
            this.Controls.Add(this.optionsControl);
            this.Name = "EndiciaSettingsControl";
            this.Size = new System.Drawing.Size(516, 841);
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.panelInsurance.ResumeLayout(false);
            this.panelInsurance.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private EndiciaOptionsControl optionsControl;
        private EndiciaAccountManagerControl accountControl;
        private System.Windows.Forms.Label labelAccountType;
        private Express1.Express1EndiciaSingleSourceControl express1Options;
        private System.Windows.Forms.Panel panelBottom;
        private Insurance.InsuranceProviderChooser insuranceProviderChooser;
        private System.Windows.Forms.Label labelShipmentProtection;
        private System.Windows.Forms.Panel panelInsurance;
        private Postal.Express1.AutomaticExpress1ControlBase express1PostageDiscountSettingsControl;
        private System.Windows.Forms.Panel panelExclusionConfiguration;
    }
}
