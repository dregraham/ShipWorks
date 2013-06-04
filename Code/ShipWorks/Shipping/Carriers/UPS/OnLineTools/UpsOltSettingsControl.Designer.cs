namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    partial class UpsOltSettingsControl
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
            this.accountControl = new ShipWorks.Shipping.Carriers.UPS.UpsAccountManagerControl();
            this.labelAccounts = new System.Windows.Forms.Label();
            this.optionsControl = new ShipWorks.Shipping.Carriers.UPS.OnLineTools.UpsOltOptionsControl();
            this.insuranceProviderChooser = new ShipWorks.Shipping.Insurance.InsuranceProviderChooser();
            this.label1 = new System.Windows.Forms.Label();
            this.pennyOneLink = new ShipWorks.UI.Controls.LinkControl();
            this.pennyOne = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // accountControl
            // 
            this.accountControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.accountControl.Location = new System.Drawing.Point(12, 98);
            this.accountControl.Name = "accountControl";
            this.accountControl.Size = new System.Drawing.Size(405, 168);
            this.accountControl.TabIndex = 1;
            // 
            // labelAccounts
            // 
            this.labelAccounts.AutoSize = true;
            this.labelAccounts.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelAccounts.Location = new System.Drawing.Point(9, 79);
            this.labelAccounts.Name = "labelAccounts";
            this.labelAccounts.Size = new System.Drawing.Size(84, 13);
            this.labelAccounts.TabIndex = 1;
            this.labelAccounts.Text = "UPS Accounts";
            // 
            // optionsControl
            // 
            this.optionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.optionsControl.Location = new System.Drawing.Point(8, 4);
            this.optionsControl.Name = "optionsControl";
            this.optionsControl.Size = new System.Drawing.Size(287, 70);
            this.optionsControl.TabIndex = 0;
            // 
            // insuranceProviderChooser
            // 
            this.insuranceProviderChooser.CarrierMessage = "(UPS Declared Value is not insurance)";
            this.insuranceProviderChooser.CarrierProviderName = "UPS Declared Value";
            this.insuranceProviderChooser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.insuranceProviderChooser.Location = new System.Drawing.Point(32, 293);
            this.insuranceProviderChooser.Name = "insuranceProviderChooser";
            this.insuranceProviderChooser.Size = new System.Drawing.Size(407, 30);
            this.insuranceProviderChooser.TabIndex = 6;
            this.insuranceProviderChooser.ProviderChanged += new System.EventHandler(this.OnInsuranceProviderChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label1.Location = new System.Drawing.Point(9, 274);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Shipment Protection";
            // 
            // pennyOneLink
            // 
            this.pennyOneLink.AutoSize = true;
            this.pennyOneLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pennyOneLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.pennyOneLink.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.pennyOneLink.Location = new System.Drawing.Point(326, 325);
            this.pennyOneLink.Name = "pennyOneLink";
            this.pennyOneLink.Size = new System.Drawing.Size(65, 13);
            this.pennyOneLink.TabIndex = 8;
            this.pennyOneLink.Text = "(Learn why)";
            this.pennyOneLink.Click += new System.EventHandler(this.OnLinkPennyOne);
            // 
            // pennyOne
            // 
            this.pennyOne.AutoSize = true;
            this.pennyOne.Location = new System.Drawing.Point(34, 324);
            this.pennyOne.Name = "pennyOne";
            this.pennyOne.Size = new System.Drawing.Size(298, 17);
            this.pennyOne.TabIndex = 7;
            this.pennyOne.Text = "Use ShipWorks Insurance for the first $100 of coverage.";
            this.pennyOne.UseVisualStyleBackColor = true;
            // 
            // UpsOltSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pennyOneLink);
            this.Controls.Add(this.pennyOne);
            this.Controls.Add(this.insuranceProviderChooser);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.optionsControl);
            this.Controls.Add(this.labelAccounts);
            this.Controls.Add(this.accountControl);
            this.Name = "UpsOltSettingsControl";
            this.Size = new System.Drawing.Size(423, 363);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UpsAccountManagerControl accountControl;
        private System.Windows.Forms.Label labelAccounts;
        private UpsOltOptionsControl optionsControl;
        private Insurance.InsuranceProviderChooser insuranceProviderChooser;
        private System.Windows.Forms.Label label1;
        private UI.Controls.LinkControl pennyOneLink;
        private System.Windows.Forms.CheckBox pennyOne;
    }
}
