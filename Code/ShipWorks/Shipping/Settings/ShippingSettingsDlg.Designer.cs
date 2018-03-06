namespace ShipWorks.Shipping.Settings
{
    partial class ShippingSettingsDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.close = new System.Windows.Forms.Button();
            this.optionControl = new ShipWorks.UI.Controls.OptionControl();
            this.optionPageGeneral = new ShipWorks.UI.Controls.OptionPage();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageProviders = new System.Windows.Forms.TabPage();
            this.panelActiveProviders = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panelProviders = new ShipWorks.Shipping.Settings.ShippingTypeCheckBoxesControl();
            this.labelProvidersInfo = new System.Windows.Forms.Label();
            this.panelDefaultProvider = new System.Windows.Forms.Panel();
            this.providerRulesControl = new ShipWorks.Shipping.Settings.ShippingProviderControl();
            this.label5 = new System.Windows.Forms.Label();
            this.labelDefaultProvider = new System.Windows.Forms.Label();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.originControl = new ShipWorks.Shipping.Settings.Origin.ShippingOriginManagerControl();
            this.label1 = new System.Windows.Forms.Label();
            this.blankPhone = new System.Windows.Forms.TextBox();
            this.labelBlankPhone = new System.Windows.Forms.Label();
            this.radioBlankPhoneUseSpecified = new System.Windows.Forms.RadioButton();
            this.radioBlankPhoneUseShipper = new System.Windows.Forms.RadioButton();
            this.labelBlankPhoneInfo = new System.Windows.Forms.Label();
            this.optionControl.SuspendLayout();
            this.optionPageGeneral.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageProviders.SuspendLayout();
            this.panelActiveProviders.SuspendLayout();
            this.panelDefaultProvider.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(656, 534);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 0;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // optionControl
            // 
            this.optionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.optionControl.Controls.Add(this.optionPageGeneral);
            this.optionControl.Location = new System.Drawing.Point(12, 12);
            this.optionControl.MenuListWidth = 150;
            this.optionControl.Name = "optionControl";
            this.optionControl.SelectedIndex = 0;
            this.optionControl.Size = new System.Drawing.Size(719, 516);
            this.optionControl.TabIndex = 1;
            this.optionControl.Text = "optionControl1";
            this.optionControl.Deselecting += new ShipWorks.UI.Controls.OptionControlCancelEventHandler(this.OnOptionPageDeselecting);
            this.optionControl.Selecting += new ShipWorks.UI.Controls.OptionControlCancelEventHandler(this.OnOptionPageSelecting);
            // 
            // optionPageGeneral
            // 
            this.optionPageGeneral.Controls.Add(this.tabControl);
            this.optionPageGeneral.Location = new System.Drawing.Point(163, 0);
            this.optionPageGeneral.Name = "optionPageGeneral";
            this.optionPageGeneral.Size = new System.Drawing.Size(556, 516);
            this.optionPageGeneral.TabIndex = 1;
            this.optionPageGeneral.Text = "General";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageProviders);
            this.tabControl.Controls.Add(this.tabPageSettings);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(556, 516);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageProviders
            // 
            this.tabPageProviders.AutoScroll = true;
            this.tabPageProviders.Controls.Add(this.panelActiveProviders);
            this.tabPageProviders.Controls.Add(this.panelDefaultProvider);
            this.tabPageProviders.Location = new System.Drawing.Point(4, 22);
            this.tabPageProviders.Name = "tabPageProviders";
            this.tabPageProviders.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProviders.Size = new System.Drawing.Size(548, 490);
            this.tabPageProviders.TabIndex = 0;
            this.tabPageProviders.Text = "Providers";
            this.tabPageProviders.UseVisualStyleBackColor = true;
            // 
            // panelActiveProviders
            // 
            this.panelActiveProviders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelActiveProviders.Controls.Add(this.label2);
            this.panelActiveProviders.Controls.Add(this.panelProviders);
            this.panelActiveProviders.Controls.Add(this.labelProvidersInfo);
            this.panelActiveProviders.Location = new System.Drawing.Point(10, 169);
            this.panelActiveProviders.Name = "panelActiveProviders";
            this.panelActiveProviders.Size = new System.Drawing.Size(536, 249);
            this.panelActiveProviders.TabIndex = 28;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Active Providers";
            // 
            // panelProviders
            // 
            this.panelProviders.Location = new System.Drawing.Point(29, 34);
            this.panelProviders.Name = "panelProviders";
            this.panelProviders.Size = new System.Drawing.Size(307, 100);
            this.panelProviders.TabIndex = 1;
            this.panelProviders.ChangeEnabledShipmentTypes += new System.EventHandler(this.OnChangeEnabledShipmentTypes);
            // 
            // labelProvidersInfo
            // 
            this.labelProvidersInfo.AutoSize = true;
            this.labelProvidersInfo.Location = new System.Drawing.Point(26, 17);
            this.labelProvidersInfo.Name = "labelProvidersInfo";
            this.labelProvidersInfo.Size = new System.Drawing.Size(310, 13);
            this.labelProvidersInfo.TabIndex = 0;
            this.labelProvidersInfo.Text = "The following providers will be available in the shipping window:";
            // 
            // panelDefaultProvider
            // 
            this.panelDefaultProvider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDefaultProvider.Controls.Add(this.providerRulesControl);
            this.panelDefaultProvider.Controls.Add(this.label5);
            this.panelDefaultProvider.Controls.Add(this.labelDefaultProvider);
            this.panelDefaultProvider.Location = new System.Drawing.Point(10, 12);
            this.panelDefaultProvider.Name = "panelDefaultProvider";
            this.panelDefaultProvider.Size = new System.Drawing.Size(536, 151);
            this.panelDefaultProvider.TabIndex = 2;
            // 
            // providerRulesControl
            // 
            this.providerRulesControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.providerRulesControl.AutoScroll = true;
            this.providerRulesControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.providerRulesControl.Location = new System.Drawing.Point(23, 34);
            this.providerRulesControl.Name = "providerRulesControl";
            this.providerRulesControl.Size = new System.Drawing.Size(510, 114);
            this.providerRulesControl.TabIndex = 2;
            this.providerRulesControl.SizeChanged += new System.EventHandler(this.OnProviderRulesSizeChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(166, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "When a shipment is first created:";
            // 
            // labelDefaultProvider
            // 
            this.labelDefaultProvider.AutoSize = true;
            this.labelDefaultProvider.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDefaultProvider.Location = new System.Drawing.Point(0, 0);
            this.labelDefaultProvider.Name = "labelDefaultProvider";
            this.labelDefaultProvider.Size = new System.Drawing.Size(99, 13);
            this.labelDefaultProvider.TabIndex = 0;
            this.labelDefaultProvider.Text = "Default Provider";
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.Controls.Add(this.originControl);
            this.tabPageSettings.Controls.Add(this.label1);
            this.tabPageSettings.Controls.Add(this.blankPhone);
            this.tabPageSettings.Controls.Add(this.labelBlankPhone);
            this.tabPageSettings.Controls.Add(this.radioBlankPhoneUseSpecified);
            this.tabPageSettings.Controls.Add(this.radioBlankPhoneUseShipper);
            this.tabPageSettings.Controls.Add(this.labelBlankPhoneInfo);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings.Size = new System.Drawing.Size(548, 490);
            this.tabPageSettings.TabIndex = 1;
            this.tabPageSettings.Text = "Global Settings";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // originControl
            // 
            this.originControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.originControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.originControl.Location = new System.Drawing.Point(30, 160);
            this.originControl.Name = "originControl";
            this.originControl.Size = new System.Drawing.Size(451, 116);
            this.originControl.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 139);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Origin Addresses";
            // 
            // blankPhone
            // 
            this.blankPhone.Location = new System.Drawing.Point(71, 108);
            this.blankPhone.Name = "blankPhone";
            this.blankPhone.Size = new System.Drawing.Size(131, 21);
            this.blankPhone.TabIndex = 4;
            // 
            // labelBlankPhone
            // 
            this.labelBlankPhone.AutoSize = true;
            this.labelBlankPhone.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBlankPhone.Location = new System.Drawing.Point(10, 12);
            this.labelBlankPhone.Name = "labelBlankPhone";
            this.labelBlankPhone.Size = new System.Drawing.Size(132, 13);
            this.labelBlankPhone.TabIndex = 0;
            this.labelBlankPhone.Text = "Blank Recipient Phone";
            // 
            // radioBlankPhoneUseSpecified
            // 
            this.radioBlankPhoneUseSpecified.AutoSize = true;
            this.radioBlankPhoneUseSpecified.Location = new System.Drawing.Point(52, 86);
            this.radioBlankPhoneUseSpecified.Name = "radioBlankPhoneUseSpecified";
            this.radioBlankPhoneUseSpecified.Size = new System.Drawing.Size(183, 17);
            this.radioBlankPhoneUseSpecified.TabIndex = 3;
            this.radioBlankPhoneUseSpecified.TabStop = true;
            this.radioBlankPhoneUseSpecified.Text = "Use the specified phone number:";
            this.radioBlankPhoneUseSpecified.UseVisualStyleBackColor = true;
            this.radioBlankPhoneUseSpecified.CheckedChanged += new System.EventHandler(this.OnChangeBlankPhoneOption);
            // 
            // radioBlankPhoneUseShipper
            // 
            this.radioBlankPhoneUseShipper.AutoSize = true;
            this.radioBlankPhoneUseShipper.Location = new System.Drawing.Point(52, 63);
            this.radioBlankPhoneUseShipper.Name = "radioBlankPhoneUseShipper";
            this.radioBlankPhoneUseShipper.Size = new System.Drawing.Size(202, 17);
            this.radioBlankPhoneUseShipper.TabIndex = 2;
            this.radioBlankPhoneUseShipper.TabStop = true;
            this.radioBlankPhoneUseShipper.Text = "Use the phone number of the sender";
            this.radioBlankPhoneUseShipper.UseVisualStyleBackColor = true;
            this.radioBlankPhoneUseShipper.CheckedChanged += new System.EventHandler(this.OnChangeBlankPhoneOption);
            // 
            // labelBlankPhoneInfo
            // 
            this.labelBlankPhoneInfo.Location = new System.Drawing.Point(27, 31);
            this.labelBlankPhoneInfo.Name = "labelBlankPhoneInfo";
            this.labelBlankPhoneInfo.Size = new System.Drawing.Size(369, 31);
            this.labelBlankPhoneInfo.TabIndex = 1;
            this.labelBlankPhoneInfo.Text = "Some carriers require a recipient phone number for certain services.  When none i" +
    "s provided:";
            // 
            // ShippingSettingsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(743, 569);
            this.Controls.Add(this.optionControl);
            this.Controls.Add(this.close);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(759, 607);
            this.Name = "ShippingSettingsDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Shipping Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.optionControl.ResumeLayout(false);
            this.optionPageGeneral.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageProviders.ResumeLayout(false);
            this.panelActiveProviders.ResumeLayout(false);
            this.panelActiveProviders.PerformLayout();
            this.panelDefaultProvider.ResumeLayout(false);
            this.panelDefaultProvider.PerformLayout();
            this.tabPageSettings.ResumeLayout(false);
            this.tabPageSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button close;
        private ShipWorks.UI.Controls.OptionControl optionControl;
        private ShipWorks.UI.Controls.OptionPage optionPageGeneral;
        private System.Windows.Forms.Label labelProvidersInfo;
        private ShippingTypeCheckBoxesControl panelProviders;
        private System.Windows.Forms.Panel panelDefaultProvider;
        private System.Windows.Forms.Label labelBlankPhone;
        private System.Windows.Forms.Label labelBlankPhoneInfo;
        private System.Windows.Forms.RadioButton radioBlankPhoneUseShipper;
        private System.Windows.Forms.RadioButton radioBlankPhoneUseSpecified;
        private System.Windows.Forms.TextBox blankPhone;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageProviders;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.Label labelDefaultProvider;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private ShippingProviderControl providerRulesControl;
        private ShipWorks.Shipping.Settings.Origin.ShippingOriginManagerControl originControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelActiveProviders;
    }
}