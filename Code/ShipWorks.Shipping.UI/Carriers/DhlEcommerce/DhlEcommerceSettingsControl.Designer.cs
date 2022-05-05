using ShipWorks.Shipping.Carriers;

namespace ShipWorks.Shipping.UI.Carriers.DhlEcommerce
{
    partial class DhlEcommerceSettingsControl
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
            this.carrierAccountManagerControl = new ShipWorks.Shipping.Carriers.CarrierAccountManagerControl();
            this.label1 = new System.Windows.Forms.Label();
            this.insuranceProviderChooser = new ShipWorks.Shipping.Insurance.InsuranceProviderChooser();
            this.pennyOne = new System.Windows.Forms.CheckBox();
            this.pennyOneLink = new ShipWorks.UI.Controls.LinkControl();
            this.shippingCutoff = new ShipWorks.Shipping.Editing.ShippingDateCutoffControl();
            this.labelsLabel = new System.Windows.Forms.Label();
            this.requestedLabelFormatOptionControl = new ShipWorks.Shipping.Editing.RequestedLabelFormatOptionControl();
            this.excludedServiceControl = new ShipWorks.Shipping.UI.Carriers.DhlEcommerce.DhlEcommerceServicePickerControl();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // carrierAccountManagerControl
            // 
            this.carrierAccountManagerControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.carrierAccountManagerControl.Location = new System.Drawing.Point(25, 100);
            this.carrierAccountManagerControl.Name = "carrierAccountManagerControl";
            this.carrierAccountManagerControl.Size = new System.Drawing.Size(462, 168);
            this.carrierAccountManagerControl.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "DHL eCommerce Accounts";
            // 
            // insuranceProviderChooser
            // 
            this.insuranceProviderChooser.CarrierMessage = "(DHL Declared Value is not insurance)";
            this.insuranceProviderChooser.CarrierProviderName = "DHL Declared Value";
            this.insuranceProviderChooser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceProviderChooser.Location = new System.Drawing.Point(24, 501);
            this.insuranceProviderChooser.Name = "insuranceProviderChooser";
            this.insuranceProviderChooser.Size = new System.Drawing.Size(407, 30);
            this.insuranceProviderChooser.TabIndex = 21;
            this.insuranceProviderChooser.ProviderChanged += new System.EventHandler(this.OnInsuranceProviderChanged);
            // 
            // pennyOne
            // 
            this.pennyOne.AutoSize = true;
            this.pennyOne.Location = new System.Drawing.Point(25, 534);
            this.pennyOne.Name = "pennyOne";
            this.pennyOne.Size = new System.Drawing.Size(298, 17);
            this.pennyOne.TabIndex = 22;
            this.pennyOne.Text = "Use ShipWorks Insurance for the first $100 of coverage.";
            this.pennyOne.UseVisualStyleBackColor = true;
            // 
            // pennyOneLink
            // 
            this.pennyOneLink.AutoSize = true;
            this.pennyOneLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pennyOneLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.pennyOneLink.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.pennyOneLink.Location = new System.Drawing.Point(317, 535);
            this.pennyOneLink.Name = "pennyOneLink";
            this.pennyOneLink.Size = new System.Drawing.Size(65, 13);
            this.pennyOneLink.TabIndex = 23;
            this.pennyOneLink.Text = "(Learn why)";
            this.pennyOneLink.Click += new System.EventHandler(this.OnLinkPennyOne);
            // 
            // shippingCutoff
            // 
            this.shippingCutoff.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.shippingCutoff.Location = new System.Drawing.Point(22, 50);
            this.shippingCutoff.Name = "shippingCutoff";
            this.shippingCutoff.Size = new System.Drawing.Size(467, 22);
            this.shippingCutoff.TabIndex = 16;
            // 
            // labelsLabel
            // 
            this.labelsLabel.AutoSize = true;
            this.labelsLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelsLabel.Location = new System.Drawing.Point(6, 8);
            this.labelsLabel.Name = "labelsLabel";
            this.labelsLabel.Size = new System.Drawing.Size(43, 13);
            this.labelsLabel.TabIndex = 17;
            this.labelsLabel.Text = "Labels";
            // 
            // requestedLabelFormatOptionControl
            // 
            this.requestedLabelFormatOptionControl.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.requestedLabelFormatOptionControl.Location = new System.Drawing.Point(22, 24);
            this.requestedLabelFormatOptionControl.Name = "requestedLabelFormatOptionControl";
            this.requestedLabelFormatOptionControl.Size = new System.Drawing.Size(375, 23);
            this.requestedLabelFormatOptionControl.TabIndex = 18;
            // 
            // excludedServiceControl
            // 
            this.excludedServiceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.excludedServiceControl.Location = new System.Drawing.Point(7, 281);
            this.excludedServiceControl.Name = "excludedServiceControl";
            this.excludedServiceControl.Size = new System.Drawing.Size(384, 198);
            this.excludedServiceControl.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 482);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Shipment Protection";
            // 
            // DhlEcommerceSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.pennyOneLink);
            this.Controls.Add(this.pennyOne);
            this.Controls.Add(this.insuranceProviderChooser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.excludedServiceControl);
            this.Controls.Add(this.requestedLabelFormatOptionControl);
            this.Controls.Add(this.labelsLabel);
            this.Controls.Add(this.shippingCutoff);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.carrierAccountManagerControl);
            this.Name = "DhlEcommerceSettingsControl";
            this.Size = new System.Drawing.Size(500, 570);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CarrierAccountManagerControl carrierAccountManagerControl;
        private System.Windows.Forms.Label label1;
        private Editing.ShippingDateCutoffControl shippingCutoff;
        private System.Windows.Forms.Label labelsLabel;
        private Editing.RequestedLabelFormatOptionControl requestedLabelFormatOptionControl;
        private UI.Carriers.DhlEcommerce.DhlEcommerceServicePickerControl excludedServiceControl;



        private Shipping.Insurance.InsuranceProviderChooser insuranceProviderChooser;
        private System.Windows.Forms.CheckBox pennyOne;
        private ShipWorks.UI.Controls.LinkControl pennyOneLink;
        
        private System.Windows.Forms.Label label2;
    }
}
