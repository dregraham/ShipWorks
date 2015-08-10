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
            this.accountManagerControl = new ShipWorks.Shipping.Carriers.Amazon.AmazonAccountManagerControl();
            this.managerLabel = new System.Windows.Forms.Label();
            this.labelShipmentProtection = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.insuranceProviderChooser = new ShipWorks.Shipping.Insurance.InsuranceProviderChooser();
            this.amazonOptionsControl1 = new ShipWorks.Shipping.Carriers.Amazon.AmazonOptionsControl();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // accountManagerControl
            // 
            this.accountManagerControl.AccountManager = null;
            this.accountManagerControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountManagerControl.Location = new System.Drawing.Point(19, 21);
            this.accountManagerControl.Name = "accountManagerControl";
            this.accountManagerControl.Size = new System.Drawing.Size(400, 168);
            this.accountManagerControl.TabIndex = 0;
            // 
            // managerLabel
            // 
            this.managerLabel.AutoSize = true;
            this.managerLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.managerLabel.Location = new System.Drawing.Point(5, 5);
            this.managerLabel.Name = "managerLabel";
            this.managerLabel.Size = new System.Drawing.Size(108, 13);
            this.managerLabel.TabIndex = 21;
            this.managerLabel.Text = "Amazon Accounts";
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
            // panel1
            // 
            this.panel1.Controls.Add(this.accountManagerControl);
            this.panel1.Controls.Add(this.managerLabel);
            this.panel1.Location = new System.Drawing.Point(5, 47);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(425, 195);
            this.panel1.TabIndex = 23;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.insuranceProviderChooser);
            this.panel2.Controls.Add(this.labelShipmentProtection);
            this.panel2.Location = new System.Drawing.Point(5, 248);
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
            this.Controls.Add(this.panel1);
            this.Name = "AmazonSettingsControl";
            this.Size = new System.Drawing.Size(445, 359);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AmazonAccountManagerControl accountManagerControl;
        private System.Windows.Forms.Label managerLabel;
        private System.Windows.Forms.Label labelShipmentProtection;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Insurance.InsuranceProviderChooser insuranceProviderChooser;
        private AmazonOptionsControl amazonOptionsControl1;
    }
}
