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
            this.insuranceProviderChooser = new ShipWorks.Shipping.Insurance.InsuranceProviderChooser();
            this.labelShipmentProtection = new System.Windows.Forms.Label();
            this.hiddenInsurancePanel = new System.Windows.Forms.Panel();
            this.labelShipWorksInsurance = new System.Windows.Forms.Label();
            this.hiddenInsurancePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // managerLabel
            // 
            this.managerLabel.AutoSize = true;
            this.managerLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.managerLabel.Location = new System.Drawing.Point(3, 77);
            this.managerLabel.Name = "managerLabel";
            this.managerLabel.Size = new System.Drawing.Size(102, 13);
            this.managerLabel.TabIndex = 2;
            this.managerLabel.Text = "OnTrac Accounts";
            // 
            // accountManager
            // 
            this.accountManager.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountManager.Location = new System.Drawing.Point(26, 98);
            this.accountManager.Name = "accountManager";
            this.accountManager.Size = new System.Drawing.Size(407, 168);
            this.accountManager.TabIndex = 1;
            // 
            // optionsControl
            // 
            this.optionsControl.Location = new System.Drawing.Point(6, 3);
            this.optionsControl.Name = "optionsControl";
            this.optionsControl.Size = new System.Drawing.Size(349, 75);
            this.optionsControl.TabIndex = 0;
            // 
            // pennyOneLink
            // 
            this.pennyOneLink.AutoSize = true;
            this.pennyOneLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pennyOneLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.pennyOneLink.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.pennyOneLink.Location = new System.Drawing.Point(320, 299);
            this.pennyOneLink.Name = "pennyOneLink";
            this.pennyOneLink.Size = new System.Drawing.Size(65, 13);
            this.pennyOneLink.TabIndex = 10;
            this.pennyOneLink.Text = "(Learn why)";
            this.pennyOneLink.Click += new System.EventHandler(this.OnLinkPennyOne);
            // 
            // pennyOne
            // 
            this.pennyOne.AutoSize = true;
            this.pennyOne.Location = new System.Drawing.Point(26, 299);
            this.pennyOne.Name = "pennyOne";
            this.pennyOne.Size = new System.Drawing.Size(298, 17);
            this.pennyOne.TabIndex = 9;
            this.pennyOne.Text = "Use ShipWorks Insurance for the first $100 of coverage.";
            this.pennyOne.UseVisualStyleBackColor = true;
            // 
            // insuranceProviderChooser
            // 
            this.insuranceProviderChooser.CarrierMessage = "(OnTrac Declared Value is not insurance)";
            this.insuranceProviderChooser.CarrierProviderName = "OnTrac Declared Value";
            this.insuranceProviderChooser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceProviderChooser.Location = new System.Drawing.Point(26, 19);
            this.insuranceProviderChooser.Name = "insuranceProviderChooser";
            this.insuranceProviderChooser.Size = new System.Drawing.Size(407, 30);
            this.insuranceProviderChooser.TabIndex = 8;
            this.insuranceProviderChooser.ProviderChanged += new System.EventHandler(this.OnInsuranceProviderChanged);
            // 
            // labelShipmentProtection
            // 
            this.labelShipmentProtection.AutoSize = true;
            this.labelShipmentProtection.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelShipmentProtection.Location = new System.Drawing.Point(3, 0);
            this.labelShipmentProtection.Name = "labelShipmentProtection";
            this.labelShipmentProtection.Size = new System.Drawing.Size(123, 13);
            this.labelShipmentProtection.TabIndex = 7;
            this.labelShipmentProtection.Text = "Shipment Protection";
            // 
            // hiddenInsurancePanel
            // 
            this.hiddenInsurancePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hiddenInsurancePanel.Controls.Add(this.labelShipmentProtection);
            this.hiddenInsurancePanel.Controls.Add(this.insuranceProviderChooser);
            this.hiddenInsurancePanel.Location = new System.Drawing.Point(0, 329);
            this.hiddenInsurancePanel.Margin = new System.Windows.Forms.Padding(0);
            this.hiddenInsurancePanel.Name = "hiddenInsurancePanel";
            this.hiddenInsurancePanel.Size = new System.Drawing.Size(456, 57);
            this.hiddenInsurancePanel.TabIndex = 11;
            this.hiddenInsurancePanel.Visible = false;
            // 
            // labelShipWorksInsurance
            // 
            this.labelShipWorksInsurance.AutoSize = true;
            this.labelShipWorksInsurance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelShipWorksInsurance.Location = new System.Drawing.Point(3, 278);
            this.labelShipWorksInsurance.Name = "labelShipWorksInsurance";
            this.labelShipWorksInsurance.Size = new System.Drawing.Size(127, 13);
            this.labelShipWorksInsurance.TabIndex = 12;
            this.labelShipWorksInsurance.Text = "ShipWorks Insurance";
            // 
            // OnTracSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelShipWorksInsurance);
            this.Controls.Add(this.hiddenInsurancePanel);
            this.Controls.Add(this.pennyOneLink);
            this.Controls.Add(this.managerLabel);
            this.Controls.Add(this.accountManager);
            this.Controls.Add(this.pennyOne);
            this.Controls.Add(this.optionsControl);
            this.Name = "OnTracSettingsControl";
            this.Size = new System.Drawing.Size(456, 386);
            this.hiddenInsurancePanel.ResumeLayout(false);
            this.hiddenInsurancePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OnTracOptionsControl optionsControl;
        private OnTracAccountManagerControl accountManager;
        private System.Windows.Forms.Label managerLabel;
        private UI.Controls.LinkControl pennyOneLink;
        private System.Windows.Forms.CheckBox pennyOne;
        private Insurance.InsuranceProviderChooser insuranceProviderChooser;
        private System.Windows.Forms.Label labelShipmentProtection;
        private System.Windows.Forms.Panel hiddenInsurancePanel;
        private System.Windows.Forms.Label labelShipWorksInsurance;
    }
}
