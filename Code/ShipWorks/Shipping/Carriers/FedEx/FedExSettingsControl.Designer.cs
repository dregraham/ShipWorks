﻿namespace ShipWorks.Shipping.Carriers.FedEx
{
    partial class FedExSettingsControl
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
            this.shippersControl = new ShipWorks.Shipping.Carriers.FedEx.FedExAccountManagerControl();
            this.optionsControl = new ShipWorks.Shipping.Carriers.FedEx.FedExOptionsControl();
            this.labelShippers = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.insuranceProviderChooser = new ShipWorks.Shipping.Insurance.InsuranceProviderChooser();
            this.pennyOne = new System.Windows.Forms.CheckBox();
            this.pennyOneLink = new ShipWorks.UI.Controls.LinkControl();
            this.SuspendLayout();
            // 
            // shippersControl
            // 
            this.shippersControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.shippersControl.Location = new System.Drawing.Point(26, 164);
            this.shippersControl.Name = "shippersControl";
            this.shippersControl.Size = new System.Drawing.Size(407, 168);
            this.shippersControl.TabIndex = 2;
            // 
            // optionsControl
            // 
            this.optionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.optionsControl.Location = new System.Drawing.Point(6, 3);
            this.optionsControl.Name = "optionsControl";
            this.optionsControl.Size = new System.Drawing.Size(439, 137);
            this.optionsControl.TabIndex = 0;
            // 
            // labelShippers
            // 
            this.labelShippers.AutoSize = true;
            this.labelShippers.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelShippers.Location = new System.Drawing.Point(3, 143);
            this.labelShippers.Name = "labelShippers";
            this.labelShippers.Size = new System.Drawing.Size(95, 13);
            this.labelShippers.TabIndex = 1;
            this.labelShippers.Text = "FedEx Accounts";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label1.Location = new System.Drawing.Point(3, 346);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Shipment Protection";
            // 
            // insuranceProviderChooser
            // 
            this.insuranceProviderChooser.CarrierMessage = "(FedEx Declared Value is not insurance)";
            this.insuranceProviderChooser.CarrierProviderName = "FedEx Declared Value";
            this.insuranceProviderChooser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.insuranceProviderChooser.Location = new System.Drawing.Point(26, 365);
            this.insuranceProviderChooser.Name = "insuranceProviderChooser";
            this.insuranceProviderChooser.Size = new System.Drawing.Size(407, 30);
            this.insuranceProviderChooser.TabIndex = 4;
            this.insuranceProviderChooser.ProviderChanged += new System.EventHandler(this.OnInsuranceProviderChanged);
            // 
            // pennyOne
            // 
            this.pennyOne.AutoSize = true;
            this.pennyOne.Location = new System.Drawing.Point(28, 398);
            this.pennyOne.Name = "pennyOne";
            this.pennyOne.Size = new System.Drawing.Size(298, 17);
            this.pennyOne.TabIndex = 5;
            this.pennyOne.Text = "Use ShipWorks Insurance for the first $100 of coverage.";
            this.pennyOne.UseVisualStyleBackColor = true;
            // 
            // pennyOneLink
            // 
            this.pennyOneLink.AutoSize = true;
            this.pennyOneLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pennyOneLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.pennyOneLink.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.pennyOneLink.Location = new System.Drawing.Point(320, 399);
            this.pennyOneLink.Name = "pennyOneLink";
            this.pennyOneLink.Size = new System.Drawing.Size(65, 13);
            this.pennyOneLink.TabIndex = 6;
            this.pennyOneLink.Text = "(Learn why)";
            this.pennyOneLink.Click += new System.EventHandler(this.OnLinkPennyOne);
            // 
            // FedExSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.pennyOneLink);
            this.Controls.Add(this.pennyOne);
            this.Controls.Add(this.insuranceProviderChooser);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelShippers);
            this.Controls.Add(this.optionsControl);
            this.Controls.Add(this.shippersControl);
            this.Name = "FedExSettingsControl";
            this.Size = new System.Drawing.Size(445, 446);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FedExAccountManagerControl shippersControl;
        private FedExOptionsControl optionsControl;
        private System.Windows.Forms.Label labelShippers;
        private System.Windows.Forms.Label label1;
        private Insurance.InsuranceProviderChooser insuranceProviderChooser;
        private System.Windows.Forms.CheckBox pennyOne;
        private UI.Controls.LinkControl pennyOneLink;
    }
}
