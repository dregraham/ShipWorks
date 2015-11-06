using ShipWorks.Shipping.Carriers.Amazon;

namespace ShipWorks.Shipping.UI.Carriers.Amazon
{
    partial class AmazonSettingsControl
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
            this.labelShipmentProtection = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.insuranceProviderChooser = new ShipWorks.Shipping.Insurance.InsuranceProviderChooser();
            this.amazonOptionsControl1 = new ShipWorks.Shipping.Carriers.Amazon.AmazonOptionsControl();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelShipmentProtection
            // 
            this.labelShipmentProtection.AutoSize = true;
            this.labelShipmentProtection.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelShipmentProtection.Location = new System.Drawing.Point(5, 5);
            this.labelShipmentProtection.Name = "labelShipmentProtection";
            this.labelShipmentProtection.Size = new System.Drawing.Size(123, 13);
            this.labelShipmentProtection.TabIndex = 22;
            this.labelShipmentProtection.Text = "Shipment Protection";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.insuranceProviderChooser);
            this.panel2.Controls.Add(this.labelShipmentProtection);
            this.panel2.Location = new System.Drawing.Point(3, 52);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(425, 57);
            this.panel2.TabIndex = 24;
            // 
            // insuranceProviderChooser
            // 
            this.insuranceProviderChooser.CarrierMessage = "";
            this.insuranceProviderChooser.CarrierProviderName = "Amazon Insurance";
            this.insuranceProviderChooser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceProviderChooser.Location = new System.Drawing.Point(19, 21);
            this.insuranceProviderChooser.Name = "insuranceProviderChooser";
            this.insuranceProviderChooser.Size = new System.Drawing.Size(375, 30);
            this.insuranceProviderChooser.TabIndex = 25;
            // 
            // amazonOptionsControl1
            // 
            this.amazonOptionsControl1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.amazonOptionsControl1.Location = new System.Drawing.Point(2, 0);
            this.amazonOptionsControl1.Name = "amazonOptionsControl1";
            this.amazonOptionsControl1.Size = new System.Drawing.Size(377, 46);
            this.amazonOptionsControl1.TabIndex = 25;
            // 
            // AmazonSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.amazonOptionsControl1);
            this.Controls.Add(this.panel2);
            this.Name = "AmazonSettingsControl";
            this.Size = new System.Drawing.Size(445, 119);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label labelShipmentProtection;
        private System.Windows.Forms.Panel panel2;
        private Insurance.InsuranceProviderChooser insuranceProviderChooser;
        private AmazonOptionsControl amazonOptionsControl1;
    }
}
