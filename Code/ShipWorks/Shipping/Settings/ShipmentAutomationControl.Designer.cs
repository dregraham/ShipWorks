namespace ShipWorks.Shipping.Settings
{
    partial class ShipmentAutomationControl
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
            this.delayTimeOfDay = new System.Windows.Forms.DateTimePicker();
            this.delayDelivery = new System.Windows.Forms.CheckBox();
            this.labelVoidStatus = new System.Windows.Forms.Label();
            this.labelTemplate = new System.Windows.Forms.Label();
            this.voidActionBox = new System.Windows.Forms.CheckBox();
            this.labelActionVoiding = new System.Windows.Forms.Label();
            this.emailActionBox = new System.Windows.Forms.CheckBox();
            this.labelActionEmail = new System.Windows.Forms.Label();
            this.voidActionStatus = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.emailActionTemplate = new ShipWorks.Templates.Controls.TemplateComboBox();
            this.panelEmailSettings = new System.Windows.Forms.Panel();
            this.panelEmailSetup = new System.Windows.Forms.Panel();
            this.emailAccounts = new System.Windows.Forms.Button();
            this.labelAutoEmails = new System.Windows.Forms.Label();
            this.panelEmailVoid = new System.Windows.Forms.Panel();
            this.shipActionBox = new System.Windows.Forms.CheckBox();
            this.shipActionStatus = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelShipStatus = new System.Windows.Forms.Label();
            this.panelEmailSettings.SuspendLayout();
            this.panelEmailSetup.SuspendLayout();
            this.panelEmailVoid.SuspendLayout();
            this.SuspendLayout();
            // 
            // delayTimeOfDay
            // 
            this.delayTimeOfDay.CustomFormat = "h:mm tt";
            this.delayTimeOfDay.Enabled = false;
            this.delayTimeOfDay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.delayTimeOfDay.Location = new System.Drawing.Point(218, 49);
            this.delayTimeOfDay.Name = "delayTimeOfDay";
            this.delayTimeOfDay.ShowUpDown = true;
            this.delayTimeOfDay.Size = new System.Drawing.Size(82, 21);
            this.delayTimeOfDay.TabIndex = 36;
            this.delayTimeOfDay.Value = new System.DateTime(2009, 1, 29, 8, 0, 0, 0);
            // 
            // delayDelivery
            // 
            this.delayDelivery.AutoSize = true;
            this.delayDelivery.Enabled = false;
            this.delayDelivery.Location = new System.Drawing.Point(24, 51);
            this.delayDelivery.Name = "delayDelivery";
            this.delayDelivery.Size = new System.Drawing.Size(196, 17);
            this.delayDelivery.TabIndex = 35;
            this.delayDelivery.Text = "Delay delivery until the ship date at";
            this.delayDelivery.UseVisualStyleBackColor = true;
            this.delayDelivery.CheckedChanged += new System.EventHandler(this.OnChangeEmailDelayDelivery);
            // 
            // labelVoidStatus
            // 
            this.labelVoidStatus.AutoSize = true;
            this.labelVoidStatus.Location = new System.Drawing.Point(42, 188);
            this.labelVoidStatus.Name = "labelVoidStatus";
            this.labelVoidStatus.Size = new System.Drawing.Size(42, 13);
            this.labelVoidStatus.TabIndex = 8;
            this.labelVoidStatus.Text = "Status:";
            // 
            // labelTemplate
            // 
            this.labelTemplate.AutoSize = true;
            this.labelTemplate.Enabled = false;
            this.labelTemplate.Location = new System.Drawing.Point(21, 26);
            this.labelTemplate.Name = "labelTemplate";
            this.labelTemplate.Size = new System.Drawing.Size(55, 13);
            this.labelTemplate.TabIndex = 33;
            this.labelTemplate.Text = "Template:";
            // 
            // voidActionBox
            // 
            this.voidActionBox.AutoSize = true;
            this.voidActionBox.Checked = true;
            this.voidActionBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.voidActionBox.Location = new System.Drawing.Point(24, 165);
            this.voidActionBox.Name = "voidActionBox";
            this.voidActionBox.Size = new System.Drawing.Size(300, 17);
            this.voidActionBox.TabIndex = 7;
            this.voidActionBox.Text = "Automatically update the local order status after voiding.";
            this.voidActionBox.UseVisualStyleBackColor = true;
            this.voidActionBox.CheckedChanged += new System.EventHandler(this.OnChangeActionVoiding);
            // 
            // labelActionVoiding
            // 
            this.labelActionVoiding.AutoSize = true;
            this.labelActionVoiding.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelActionVoiding.Location = new System.Drawing.Point(4, 93);
            this.labelActionVoiding.Name = "labelActionVoiding";
            this.labelActionVoiding.Size = new System.Drawing.Size(76, 13);
            this.labelActionVoiding.TabIndex = 3;
            this.labelActionVoiding.Text = "Local Status";
            // 
            // emailActionBox
            // 
            this.emailActionBox.AutoSize = true;
            this.emailActionBox.Location = new System.Drawing.Point(3, 3);
            this.emailActionBox.Name = "emailActionBox";
            this.emailActionBox.Size = new System.Drawing.Size(243, 17);
            this.emailActionBox.TabIndex = 28;
            this.emailActionBox.Text = "Automatically send an email after processing.";
            this.emailActionBox.UseVisualStyleBackColor = true;
            this.emailActionBox.CheckedChanged += new System.EventHandler(this.OnChangeActionEmail);
            // 
            // labelActionEmail
            // 
            this.labelActionEmail.AutoSize = true;
            this.labelActionEmail.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelActionEmail.Location = new System.Drawing.Point(4, 0);
            this.labelActionEmail.Name = "labelActionEmail";
            this.labelActionEmail.Size = new System.Drawing.Size(37, 13);
            this.labelActionEmail.TabIndex = 1;
            this.labelActionEmail.Text = "Email";
            // 
            // voidActionStatus
            // 
            this.voidActionStatus.Location = new System.Drawing.Point(87, 185);
            this.voidActionStatus.MaxLength = 32767;
            this.voidActionStatus.Name = "voidActionStatus";
            this.voidActionStatus.Size = new System.Drawing.Size(244, 21);
            this.voidActionStatus.TabIndex = 9;
            // 
            // emailActionTemplate
            // 
            this.emailActionTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.emailActionTemplate.Enabled = false;
            this.emailActionTemplate.FormattingEnabled = true;
            this.emailActionTemplate.Location = new System.Drawing.Point(78, 23);
            this.emailActionTemplate.Name = "emailActionTemplate";
            this.emailActionTemplate.Size = new System.Drawing.Size(244, 21);
            this.emailActionTemplate.TabIndex = 29;
            // 
            // panelEmailSettings
            // 
            this.panelEmailSettings.Controls.Add(this.emailActionBox);
            this.panelEmailSettings.Controls.Add(this.delayTimeOfDay);
            this.panelEmailSettings.Controls.Add(this.emailActionTemplate);
            this.panelEmailSettings.Controls.Add(this.delayDelivery);
            this.panelEmailSettings.Controls.Add(this.labelTemplate);
            this.panelEmailSettings.Location = new System.Drawing.Point(19, 16);
            this.panelEmailSettings.Name = "panelEmailSettings";
            this.panelEmailSettings.Size = new System.Drawing.Size(327, 76);
            this.panelEmailSettings.TabIndex = 37;
            // 
            // panelEmailSetup
            // 
            this.panelEmailSetup.Controls.Add(this.emailAccounts);
            this.panelEmailSetup.Controls.Add(this.labelAutoEmails);
            this.panelEmailSetup.Location = new System.Drawing.Point(19, 15);
            this.panelEmailSetup.Name = "panelEmailSetup";
            this.panelEmailSetup.Size = new System.Drawing.Size(327, 77);
            this.panelEmailSetup.TabIndex = 2;
            // 
            // emailAccounts
            // 
            this.emailAccounts.Location = new System.Drawing.Point(15, 48);
            this.emailAccounts.Name = "emailAccounts";
            this.emailAccounts.Size = new System.Drawing.Size(110, 23);
            this.emailAccounts.TabIndex = 1;
            this.emailAccounts.Text = "Email Accounts...";
            this.emailAccounts.UseVisualStyleBackColor = true;
            this.emailAccounts.Click += new System.EventHandler(this.OnEmailAccounts);
            // 
            // labelAutoEmails
            // 
            this.labelAutoEmails.Location = new System.Drawing.Point(2, 4);
            this.labelAutoEmails.Name = "labelAutoEmails";
            this.labelAutoEmails.Size = new System.Drawing.Size(322, 43);
            this.labelAutoEmails.TabIndex = 0;
            this.labelAutoEmails.Text = "No email accounts have been setup in ShipWorks.  You must setup an email account " +
                "before you can enable automatic shipping emails.";
            // 
            // panelEmailVoid
            // 
            this.panelEmailVoid.Controls.Add(this.shipActionBox);
            this.panelEmailVoid.Controls.Add(this.shipActionStatus);
            this.panelEmailVoid.Controls.Add(this.labelShipStatus);
            this.panelEmailVoid.Controls.Add(this.labelActionEmail);
            this.panelEmailVoid.Controls.Add(this.labelActionVoiding);
            this.panelEmailVoid.Controls.Add(this.panelEmailSetup);
            this.panelEmailVoid.Controls.Add(this.voidActionBox);
            this.panelEmailVoid.Controls.Add(this.panelEmailSettings);
            this.panelEmailVoid.Controls.Add(this.voidActionStatus);
            this.panelEmailVoid.Controls.Add(this.labelVoidStatus);
            this.panelEmailVoid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEmailVoid.Location = new System.Drawing.Point(0, 0);
            this.panelEmailVoid.Name = "panelEmailVoid";
            this.panelEmailVoid.Size = new System.Drawing.Size(345, 271);
            this.panelEmailVoid.TabIndex = 1;
            // 
            // shipActionBox
            // 
            this.shipActionBox.AutoSize = true;
            this.shipActionBox.Checked = true;
            this.shipActionBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.shipActionBox.Location = new System.Drawing.Point(24, 113);
            this.shipActionBox.Name = "shipActionBox";
            this.shipActionBox.Size = new System.Drawing.Size(317, 17);
            this.shipActionBox.TabIndex = 4;
            this.shipActionBox.Text = "Automatically update the local order status after processing.";
            this.shipActionBox.UseVisualStyleBackColor = true;
            this.shipActionBox.CheckedChanged += new System.EventHandler(this.OnChangeActionProcessing);
            // 
            // shipActionStatus
            // 
            this.shipActionStatus.Location = new System.Drawing.Point(87, 133);
            this.shipActionStatus.MaxLength = 32767;
            this.shipActionStatus.Name = "shipActionStatus";
            this.shipActionStatus.Size = new System.Drawing.Size(244, 21);
            this.shipActionStatus.TabIndex = 6;
            // 
            // labelShipStatus
            // 
            this.labelShipStatus.AutoSize = true;
            this.labelShipStatus.Location = new System.Drawing.Point(42, 136);
            this.labelShipStatus.Name = "labelShipStatus";
            this.labelShipStatus.Size = new System.Drawing.Size(42, 13);
            this.labelShipStatus.TabIndex = 5;
            this.labelShipStatus.Text = "Status:";
            // 
            // ShipmentAutomationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelEmailVoid);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "ShipmentAutomationControl";
            this.Size = new System.Drawing.Size(345, 271);
            this.panelEmailSettings.ResumeLayout(false);
            this.panelEmailSettings.PerformLayout();
            this.panelEmailSetup.ResumeLayout(false);
            this.panelEmailVoid.ResumeLayout(false);
            this.panelEmailVoid.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker delayTimeOfDay;
        private System.Windows.Forms.CheckBox delayDelivery;
        private System.Windows.Forms.Label labelVoidStatus;
        private System.Windows.Forms.Label labelTemplate;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox voidActionStatus;
        private System.Windows.Forms.CheckBox voidActionBox;
        private System.Windows.Forms.Label labelActionVoiding;
        private ShipWorks.Templates.Controls.TemplateComboBox emailActionTemplate;
        private System.Windows.Forms.CheckBox emailActionBox;
        private System.Windows.Forms.Label labelActionEmail;
        private System.Windows.Forms.Panel panelEmailSettings;
        private System.Windows.Forms.Panel panelEmailSetup;
        private System.Windows.Forms.Button emailAccounts;
        private System.Windows.Forms.Label labelAutoEmails;
        private System.Windows.Forms.Panel panelEmailVoid;
        private System.Windows.Forms.CheckBox shipActionBox;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox shipActionStatus;
        private System.Windows.Forms.Label labelShipStatus;
    }
}
