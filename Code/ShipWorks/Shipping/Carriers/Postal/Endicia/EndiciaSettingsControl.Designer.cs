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
            this.optionsControl = new ShipWorks.Shipping.Carriers.Postal.Endicia.EndiciaOptionsControl();
            this.accountControl = new ShipWorks.Shipping.Carriers.Postal.Endicia.EndiciaAccountManagerControl();
            this.labelAccountType = new System.Windows.Forms.Label();
            this.endiciaOptions = new ShipWorks.Shipping.Carriers.Postal.Endicia.EndiciaAutomaticExpress1Control();
            this.express1Options = new Express1EndiciaSingleSourceControl();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.insuranceProviderChooser = new ShipWorks.Shipping.Insurance.InsuranceProviderChooser();
            this.labelShipmentProtection = new System.Windows.Forms.Label();
            this.panelInsurance = new System.Windows.Forms.Panel();
            this.panelBottom.SuspendLayout();
            this.panelInsurance.SuspendLayout();
            this.SuspendLayout();
            // 
            // optionsControl
            // 
            this.optionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.optionsControl.Location = new System.Drawing.Point(0, -1);
            this.optionsControl.Name = "optionsControl";
            this.optionsControl.Size = new System.Drawing.Size(454, 170);
            this.optionsControl.TabIndex = 0;
            // 
            // accountControl
            // 
            this.accountControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.accountControl.Location = new System.Drawing.Point(15, 21);
            this.accountControl.Name = "accountControl";
            this.accountControl.Size = new System.Drawing.Size(449, 113);
            this.accountControl.TabIndex = 2;
            // 
            // labelAccountType
            // 
            this.labelAccountType.AutoSize = true;
            this.labelAccountType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelAccountType.Location = new System.Drawing.Point(12, 5);
            this.labelAccountType.Name = "labelAccountType";
            this.labelAccountType.Size = new System.Drawing.Size(101, 13);
            this.labelAccountType.TabIndex = 1;
            this.labelAccountType.Text = "Endicia Accounts";
            // 
            // endiciaOptions
            // 
            this.endiciaOptions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.endiciaOptions.Location = new System.Drawing.Point(5, 215);
            this.endiciaOptions.Name = "endiciaOptions";
            this.endiciaOptions.Size = new System.Drawing.Size(454, 160);
            this.endiciaOptions.TabIndex = 3;
            // 
            // express1Options
            // 
            this.express1Options.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.express1Options.Location = new System.Drawing.Point(5, 160);
            this.express1Options.Name = "express1Options";
            this.express1Options.Size = new System.Drawing.Size(421, 49);
            this.express1Options.TabIndex = 4;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.panelInsurance);
            this.panelBottom.Controls.Add(this.labelAccountType);
            this.panelBottom.Controls.Add(this.accountControl);
            this.panelBottom.Location = new System.Drawing.Point(0, 381);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(473, 211);
            this.panelBottom.TabIndex = 5;
            // 
            // insuranceProviderChooser
            // 
            this.insuranceProviderChooser.CarrierMessage = "(UPS Declared Value is not insurance)";
            this.insuranceProviderChooser.CarrierProviderName = "Endicia Insurance";
            this.insuranceProviderChooser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.insuranceProviderChooser.Location = new System.Drawing.Point(26, 24);
            this.insuranceProviderChooser.Name = "insuranceProviderChooser";
            this.insuranceProviderChooser.Size = new System.Drawing.Size(193, 30);
            this.insuranceProviderChooser.TabIndex = 10;
            // 
            // labelShipmentProtection
            // 
            this.labelShipmentProtection.AutoSize = true;
            this.labelShipmentProtection.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelShipmentProtection.Location = new System.Drawing.Point(3, 6);
            this.labelShipmentProtection.Name = "labelShipmentProtection";
            this.labelShipmentProtection.Size = new System.Drawing.Size(123, 13);
            this.labelShipmentProtection.TabIndex = 9;
            this.labelShipmentProtection.Text = "Shipment Protection";
            // 
            // panelInsurance
            // 
            this.panelInsurance.Controls.Add(this.insuranceProviderChooser);
            this.panelInsurance.Controls.Add(this.labelShipmentProtection);
            this.panelInsurance.Location = new System.Drawing.Point(10, 139);
            this.panelInsurance.Name = "panelInsurance";
            this.panelInsurance.Size = new System.Drawing.Size(265, 56);
            this.panelInsurance.TabIndex = 6;
            // 
            // EndiciaSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.express1Options);
            this.Controls.Add(this.endiciaOptions);
            this.Controls.Add(this.optionsControl);
            this.Name = "EndiciaSettingsControl";
            this.Size = new System.Drawing.Size(473, 719);
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
        private EndiciaAutomaticExpress1Control endiciaOptions;
        private Express1.Express1EndiciaSingleSourceControl express1Options;
        private System.Windows.Forms.Panel panelBottom;
        private Insurance.InsuranceProviderChooser insuranceProviderChooser;
        private System.Windows.Forms.Label labelShipmentProtection;
        private System.Windows.Forms.Panel panelInsurance;
    }
}
