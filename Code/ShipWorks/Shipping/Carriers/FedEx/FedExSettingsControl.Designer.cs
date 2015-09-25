namespace ShipWorks.Shipping.Carriers.FedEx
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
            this.fimsLabel = new System.Windows.Forms.Label();
            this.servicePicker = new ShipWorks.Shipping.Carriers.FedEx.FedExServicePickerControl();
            this.packagePicker = new ShipWorks.Shipping.Carriers.FedEx.FedExPackagePickerControl();
            this.enableFims = new System.Windows.Forms.CheckBox();
            this.fimsUsernameLabel = new System.Windows.Forms.Label();
            this.fimsUsername = new System.Windows.Forms.TextBox();
            this.fimsPassword = new System.Windows.Forms.TextBox();
            this.fimsPasswordLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // shippersControl
            // 
            this.shippersControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shippersControl.Location = new System.Drawing.Point(27, 149);
            this.shippersControl.Name = "shippersControl";
            this.shippersControl.Size = new System.Drawing.Size(407, 168);
            this.shippersControl.TabIndex = 2;
            // 
            // optionsControl
            // 
            this.optionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsControl.Location = new System.Drawing.Point(6, 3);
            this.optionsControl.Name = "optionsControl";
            this.optionsControl.Size = new System.Drawing.Size(439, 124);
            this.optionsControl.TabIndex = 0;
            // 
            // labelShippers
            // 
            this.labelShippers.AutoSize = true;
            this.labelShippers.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelShippers.Location = new System.Drawing.Point(8, 128);
            this.labelShippers.Name = "labelShippers";
            this.labelShippers.Size = new System.Drawing.Size(95, 13);
            this.labelShippers.TabIndex = 1;
            this.labelShippers.Text = "FedEx Accounts";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 846);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Shipment Protection";
            // 
            // insuranceProviderChooser
            // 
            this.insuranceProviderChooser.CarrierMessage = "(FedEx Declared Value is not insurance)";
            this.insuranceProviderChooser.CarrierProviderName = "FedEx Declared Value";
            this.insuranceProviderChooser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceProviderChooser.Location = new System.Drawing.Point(26, 865);
            this.insuranceProviderChooser.Name = "insuranceProviderChooser";
            this.insuranceProviderChooser.Size = new System.Drawing.Size(407, 30);
            this.insuranceProviderChooser.TabIndex = 4;
            this.insuranceProviderChooser.ProviderChanged += new System.EventHandler(this.OnInsuranceProviderChanged);
            // 
            // pennyOne
            // 
            this.pennyOne.AutoSize = true;
            this.pennyOne.Location = new System.Drawing.Point(27, 898);
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
            this.pennyOneLink.Location = new System.Drawing.Point(319, 899);
            this.pennyOneLink.Name = "pennyOneLink";
            this.pennyOneLink.Size = new System.Drawing.Size(65, 13);
            this.pennyOneLink.TabIndex = 6;
            this.pennyOneLink.Text = "(Learn why)";
            this.pennyOneLink.Click += new System.EventHandler(this.OnLinkPennyOne);
            // 
            // fimsLabel
            // 
            this.fimsLabel.AutoSize = true;
            this.fimsLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fimsLabel.Location = new System.Drawing.Point(8, 331);
            this.fimsLabel.Name = "fimsLabel";
            this.fimsLabel.Size = new System.Drawing.Size(227, 13);
            this.fimsLabel.TabIndex = 7;
            this.fimsLabel.Text = "FIMS (FedEx International MailService)";
            // 
            // servicePicker
            // 
            this.servicePicker.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.servicePicker.Location = new System.Drawing.Point(9, 429);
            this.servicePicker.Name = "servicePicker";
            this.servicePicker.Size = new System.Drawing.Size(421, 200);
            this.servicePicker.TabIndex = 13;
            // 
            // packagePicker
            // 
            this.packagePicker.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.packagePicker.Location = new System.Drawing.Point(9, 639);
            this.packagePicker.Name = "packagePicker";
            this.packagePicker.Size = new System.Drawing.Size(421, 200);
            this.packagePicker.TabIndex = 14;
            // 
            // enableFims
            // 
            this.enableFims.AutoSize = true;
            this.enableFims.Location = new System.Drawing.Point(27, 353);
            this.enableFims.Name = "enableFims";
            this.enableFims.Size = new System.Drawing.Size(117, 17);
            this.enableFims.TabIndex = 8;
            this.enableFims.Text = "Use FIMS services.";
            this.enableFims.UseVisualStyleBackColor = true;
            this.enableFims.CheckedChanged += new System.EventHandler(this.OnEnableFimsCheckedChanged);
            // 
            // fimsUsernameLabel
            // 
            this.fimsUsernameLabel.AutoSize = true;
            this.fimsUsernameLabel.Location = new System.Drawing.Point(45, 376);
            this.fimsUsernameLabel.Name = "fimsUsernameLabel";
            this.fimsUsernameLabel.Size = new System.Drawing.Size(86, 13);
            this.fimsUsernameLabel.TabIndex = 9;
            this.fimsUsernameLabel.Text = "FIMS Username:";
            // 
            // fimsUsername
            // 
            this.fimsUsername.Location = new System.Drawing.Point(135, 374);
            this.fimsUsername.Name = "fimsUsername";
            this.fimsUsername.Size = new System.Drawing.Size(201, 21);
            this.fimsUsername.TabIndex = 10;
            // 
            // fimsPassword
            // 
            this.fimsPassword.Location = new System.Drawing.Point(135, 401);
            this.fimsPassword.Name = "fimsPassword";
            this.fimsPassword.Size = new System.Drawing.Size(201, 21);
            this.fimsPassword.TabIndex = 12;
            // 
            // fimsPasswordLabel
            // 
            this.fimsPasswordLabel.AutoSize = true;
            this.fimsPasswordLabel.Location = new System.Drawing.Point(47, 403);
            this.fimsPasswordLabel.Name = "fimsPasswordLabel";
            this.fimsPasswordLabel.Size = new System.Drawing.Size(84, 13);
            this.fimsPasswordLabel.TabIndex = 11;
            this.fimsPasswordLabel.Text = "FIMS Password:";
            // 
            // FedExSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.fimsPassword);
            this.Controls.Add(this.fimsPasswordLabel);
            this.Controls.Add(this.packagePicker);
            this.Controls.Add(this.servicePicker);
            this.Controls.Add(this.fimsUsername);
            this.Controls.Add(this.fimsUsernameLabel);
            this.Controls.Add(this.enableFims);
            this.Controls.Add(this.fimsLabel);
            this.Controls.Add(this.pennyOneLink);
            this.Controls.Add(this.pennyOne);
            this.Controls.Add(this.insuranceProviderChooser);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelShippers);
            this.Controls.Add(this.optionsControl);
            this.Controls.Add(this.shippersControl);
            this.Name = "FedExSettingsControl";
            this.Size = new System.Drawing.Size(452, 922);
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
        private System.Windows.Forms.Label fimsLabel;
        private FedExServicePickerControl servicePicker;
        private FedExPackagePickerControl packagePicker;
        private System.Windows.Forms.CheckBox enableFims;
        private System.Windows.Forms.Label fimsUsernameLabel;
        private System.Windows.Forms.TextBox fimsUsername;
        private System.Windows.Forms.TextBox fimsPassword;
        private System.Windows.Forms.Label fimsPasswordLabel;
    }
}
