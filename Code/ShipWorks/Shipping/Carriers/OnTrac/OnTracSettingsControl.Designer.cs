﻿using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    partial class OnTracSettingsControl
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
            this.managerLabel = new System.Windows.Forms.Label();
            this.accountManager = new ShipWorks.Shipping.Carriers.OnTrac.OnTracAccountManagerControl();
            this.optionsControl = new ShipWorks.Shipping.Carriers.OnTrac.OnTracOptionsControl();
            this.pennyOneLink = new ShipWorks.UI.Controls.LinkControl();
            this.pennyOne = new System.Windows.Forms.CheckBox();
            this.labelShipmentProtection = new System.Windows.Forms.Label();
            this.insuranceProviderChooser = new ShipWorks.Shipping.Insurance.InsuranceProviderChooser();
            this.excludedServiceControl = new ShipWorks.Shipping.Carriers.OnTrac.OnTracServicePickerControl();
            this.excludedPackageControl = new ShipWorks.Shipping.Carriers.OnTrac.OnTracPackagePickerControl();
            this.shippingCutoff = new ShipWorks.Shipping.Editing.ShippingDateCutoffControl();
            this.SuspendLayout();
            // 
            // managerLabel
            // 
            this.managerLabel.AutoSize = true;
            this.managerLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.managerLabel.Location = new System.Drawing.Point(9, 84);
            this.managerLabel.Name = "managerLabel";
            this.managerLabel.Size = new System.Drawing.Size(102, 13);
            this.managerLabel.TabIndex = 2;
            this.managerLabel.Text = "OnTrac Accounts";
            // 
            // accountManager
            // 
            this.accountManager.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountManager.Location = new System.Drawing.Point(27, 104);
            this.accountManager.Name = "accountManager";
            this.accountManager.Size = new System.Drawing.Size(407, 168);
            this.accountManager.TabIndex = 1;
            // 
            // optionsControl
            // 
            this.optionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsControl.Location = new System.Drawing.Point(6, 3);
            this.optionsControl.Name = "optionsControl";
            this.optionsControl.Size = new System.Drawing.Size(428, 47);
            this.optionsControl.TabIndex = 0;
            // 
            // pennyOneLink
            // 
            this.pennyOneLink.AutoSize = true;
            this.pennyOneLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pennyOneLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.pennyOneLink.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.pennyOneLink.Location = new System.Drawing.Point(323, 629);
            this.pennyOneLink.Name = "pennyOneLink";
            this.pennyOneLink.Size = new System.Drawing.Size(65, 13);
            this.pennyOneLink.TabIndex = 10;
            this.pennyOneLink.Text = "(Learn why)";
            this.pennyOneLink.Click += new System.EventHandler(this.OnLinkPennyOne);
            // 
            // pennyOne
            // 
            this.pennyOne.AutoSize = true;
            this.pennyOne.Location = new System.Drawing.Point(27, 628);
            this.pennyOne.Name = "pennyOne";
            this.pennyOne.Size = new System.Drawing.Size(298, 17);
            this.pennyOne.TabIndex = 9;
            this.pennyOne.Text = "Use ShipWorks Insurance for the first $100 of coverage.";
            this.pennyOne.UseVisualStyleBackColor = true;
            // 
            // labelShipmentProtection
            // 
            this.labelShipmentProtection.AutoSize = true;
            this.labelShipmentProtection.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelShipmentProtection.Location = new System.Drawing.Point(9, 577);
            this.labelShipmentProtection.Name = "labelShipmentProtection";
            this.labelShipmentProtection.Size = new System.Drawing.Size(123, 13);
            this.labelShipmentProtection.TabIndex = 12;
            this.labelShipmentProtection.Text = "Shipment Protection";
            // 
            // insuranceProviderChooser
            // 
            this.insuranceProviderChooser.CarrierMessage = "(OnTrac Declared Value is not insurance)";
            this.insuranceProviderChooser.CarrierProviderName = "OnTrac Declared Value";
            this.insuranceProviderChooser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceProviderChooser.Location = new System.Drawing.Point(26, 596);
            this.insuranceProviderChooser.Name = "insuranceProviderChooser";
            this.insuranceProviderChooser.Size = new System.Drawing.Size(407, 30);
            this.insuranceProviderChooser.TabIndex = 13;
            // 
            // excludedServiceControl
            // 
            this.excludedServiceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.excludedServiceControl.Location = new System.Drawing.Point(9, 285);
            this.excludedServiceControl.Name = "excludedServiceControl";
            this.excludedServiceControl.Size = new System.Drawing.Size(422, 148);
            this.excludedServiceControl.TabIndex = 14;
            // 
            // excludedPackageControl
            // 
            this.excludedPackageControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.excludedPackageControl.Location = new System.Drawing.Point(9, 431);
            this.excludedPackageControl.Name = "excludedPackageControl";
            this.excludedPackageControl.Size = new System.Drawing.Size(422, 148);
            this.excludedPackageControl.TabIndex = 15;
            // 
            // shippingCutoff
            // 
            this.shippingCutoff.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.shippingCutoff.Location = new System.Drawing.Point(24, 52);
            this.shippingCutoff.Name = "shippingCutoff";
            this.shippingCutoff.Size = new System.Drawing.Size(467, 22);
            this.shippingCutoff.TabIndex = 62;
            // 
            // OnTracSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.shippingCutoff);
            this.Controls.Add(this.excludedPackageControl);
            this.Controls.Add(this.excludedServiceControl);
            this.Controls.Add(this.insuranceProviderChooser);
            this.Controls.Add(this.labelShipmentProtection);
            this.Controls.Add(this.pennyOneLink);
            this.Controls.Add(this.managerLabel);
            this.Controls.Add(this.accountManager);
            this.Controls.Add(this.pennyOne);
            this.Controls.Add(this.optionsControl);
            this.Name = "OnTracSettingsControl";
            this.Size = new System.Drawing.Size(499, 656);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OnTracOptionsControl optionsControl;
        private OnTracAccountManagerControl accountManager;
        private System.Windows.Forms.Label managerLabel;
        private UI.Controls.LinkControl pennyOneLink;
        private System.Windows.Forms.CheckBox pennyOne;
        private System.Windows.Forms.Label labelShipmentProtection;
        private Insurance.InsuranceProviderChooser insuranceProviderChooser;
        private OnTracServicePickerControl excludedServiceControl;
        private OnTracPackagePickerControl excludedPackageControl;
        private Editing.ShippingDateCutoffControl shippingCutoff;
    }
}
