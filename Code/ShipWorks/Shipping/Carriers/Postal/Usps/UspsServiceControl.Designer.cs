namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    partial class UspsServiceControl
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
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory3 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory2 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory1 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            this.sectionFrom = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.originControl = new ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl();
            this.panelTop = new System.Windows.Forms.Panel();
            this.rateShop = new System.Windows.Forms.CheckBox();
            this.linkManageUspsAccounts = new ShipWorks.UI.Controls.LinkControl();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.uspsAccount = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.hidePostalLabel = new System.Windows.Forms.Label();
            this.hidePostage = new System.Windows.Forms.CheckBox();
            this.labelUspsValidation = new System.Windows.Forms.Label();
            this.requireFullAddressValidation = new System.Windows.Forms.CheckBox();
            this.memo1 = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelMemo1 = new System.Windows.Forms.Label();
            this.memo3 = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelMemo3 = new System.Windows.Forms.Label();
            this.memo2 = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelMemo2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.sectionExpress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionExpress.ContentPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient.ContentPanel)).BeginInit();
            this.sectionRecipient.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns.ContentPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment.ContentPanel)).BeginInit();
            this.sectionShipment.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionLabelOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionLabelOptions.ContentPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom.ContentPanel)).BeginInit();
            this.sectionFrom.ContentPanel.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // insuranceControl
            // 
            this.insuranceControl.Location = new System.Drawing.Point(21, 351);
            this.insuranceControl.Size = new System.Drawing.Size(376, 50);
            // 
            // sectionExpress
            // 
            this.sectionExpress.Location = new System.Drawing.Point(3, 496);
            this.sectionExpress.Size = new System.Drawing.Size(378, 24);
            // 
            // sectionRecipient
            // 
            // 
            // sectionRecipient.ContentPanel
            // 
            this.sectionRecipient.ContentPanel.Controls.Add(this.residentialDetermination);
            this.sectionRecipient.ContentPanel.Controls.Add(this.labelAddress);
            this.sectionRecipient.ContentPanel.Controls.Add(this.labelResidentialCommercial);
            this.sectionRecipient.ContentPanel.Controls.Add(this.personControl);
            this.sectionRecipient.ContentPanel.Controls.Add(this.labelUspsValidation);
            this.sectionRecipient.ContentPanel.Controls.Add(this.requireFullAddressValidation);
            this.sectionRecipient.ExpandedHeight = 459;
            this.sectionRecipient.Location = new System.Drawing.Point(3, 34);
            this.sectionRecipient.Size = new System.Drawing.Size(378, 24);
            this.sectionRecipient.TabIndex = 1;
            // 
            // personControl
            // 
            this.personControl.Size = new System.Drawing.Size(371, 330);
            // 
            // labelResidentialCommercial
            // 
            this.labelResidentialCommercial.Location = new System.Drawing.Point(10, 380);
            // 
            // labelAddress
            // 
            this.labelAddress.Location = new System.Drawing.Point(23, 403);
            // 
            // residentialDetermination
            // 
            this.residentialDetermination.Location = new System.Drawing.Point(79, 400);
            // 
            // sectionReturns
            // 
            this.sectionReturns.Location = new System.Drawing.Point(3, 525);
            this.sectionReturns.Size = new System.Drawing.Size(378, 24);
            // 
            // sectionShipment
            // 
            // 
            // sectionShipment.ContentPanel
            // 
            this.sectionShipment.ContentPanel.Controls.Add(this.insuranceControl);
            this.sectionShipment.ContentPanel.Controls.Add(this.service);
            this.sectionShipment.ContentPanel.Controls.Add(this.memo2);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelMemo2);
            this.sectionShipment.ContentPanel.Controls.Add(this.memo3);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelMemo3);
            this.sectionShipment.ContentPanel.Controls.Add(this.hidePostalLabel);
            this.sectionShipment.ContentPanel.Controls.Add(this.hidePostage);
            this.sectionShipment.ContentPanel.Controls.Add(this.memo1);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelMemo1);
            this.sectionShipment.Location = new System.Drawing.Point(3, 63);
            this.sectionShipment.Size = new System.Drawing.Size(378, 428);
            // 
            // sectionLabelOptions
            // 
            this.sectionLabelOptions.Location = new System.Drawing.Point(3, 554);
            this.sectionLabelOptions.Size = new System.Drawing.Size(378, 24);
            // 
            // sectionFrom
            // 
            this.sectionFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionFrom.Collapsed = true;
            // 
            // sectionFrom.ContentPanel
            // 
            this.sectionFrom.ContentPanel.Controls.Add(this.originControl);
            this.sectionFrom.ContentPanel.Controls.Add(this.panelTop);
            this.sectionFrom.ExpandedHeight = 541;
            this.sectionFrom.ExtraText = "";
            this.sectionFrom.Location = new System.Drawing.Point(3, 5);
            this.sectionFrom.Name = "sectionFrom";
            this.sectionFrom.SectionName = "From";
            this.sectionFrom.SettingsKey = "6306b47c-8029-44bc-8b97-9b9eb001a61a";
            this.sectionFrom.Size = new System.Drawing.Size(378, 24);
            this.sectionFrom.TabIndex = 0;
            // 
            // originControl
            // 
            this.originControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.originControl.BackColor = System.Drawing.Color.Transparent;
            this.originControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.originControl.Location = new System.Drawing.Point(0, 74);
            this.originControl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.originControl.Name = "originControl";
            this.originControl.Size = new System.Drawing.Size(371, 488);
            this.originControl.TabIndex = 0;
            this.originControl.OriginChanged += new System.EventHandler(this.OnOriginChanged);
            // 
            // panelTop
            // 
            this.panelTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTop.BackColor = System.Drawing.Color.Transparent;
            this.panelTop.Controls.Add(this.rateShop);
            this.panelTop.Controls.Add(this.linkManageUspsAccounts);
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Controls.Add(this.uspsAccount);
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(371, 73);
            this.panelTop.TabIndex = 5;
            // 
            // rateShop
            // 
            this.rateShop.AutoSize = true;
            this.rateShop.BackColor = System.Drawing.Color.Transparent;
            this.rateShop.Location = new System.Drawing.Point(79, 29);
            this.rateShop.Name = "rateShop";
            this.rateShop.Size = new System.Drawing.Size(341, 17);
            this.rateShop.TabIndex = 6;
            this.rateShop.Text = "Automatically use the USPS account with the least expensive rate";
            this.rateShop.UseVisualStyleBackColor = false;
            this.rateShop.CheckedChanged += new System.EventHandler(this.OnRateShopChanged);
            // 
            // linkManageUspsAccounts
            // 
            this.linkManageUspsAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkManageUspsAccounts.AutoSize = true;
            this.linkManageUspsAccounts.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkManageUspsAccounts.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkManageUspsAccounts.ForeColor = System.Drawing.Color.Blue;
            this.linkManageUspsAccounts.Location = new System.Drawing.Point(300, 51);
            this.linkManageUspsAccounts.Name = "linkManageUspsAccounts";
            this.linkManageUspsAccounts.Size = new System.Drawing.Size(57, 13);
            this.linkManageUspsAccounts.TabIndex = 5;
            this.linkManageUspsAccounts.Text = "Manage...";
            this.linkManageUspsAccounts.Click += new System.EventHandler(this.OnManageUspsAccounts);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Account:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "USPS";
            // 
            // uspsAccount
            // 
            this.uspsAccount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uspsAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uspsAccount.FormattingEnabled = true;
            this.uspsAccount.Location = new System.Drawing.Point(79, 48);
            this.uspsAccount.Name = "uspsAccount";
            this.uspsAccount.PromptText = "(Multiple Values)";
            this.uspsAccount.Size = new System.Drawing.Size(215, 21);
            this.uspsAccount.TabIndex = 3;
            this.uspsAccount.SelectedValueChanged += new System.EventHandler(this.OnOriginChanged);
            // 
            // hidePostalLabel
            // 
            this.hidePostalLabel.AutoSize = true;
            this.hidePostalLabel.BackColor = System.Drawing.Color.Transparent;
            this.hidePostalLabel.Location = new System.Drawing.Point(39, 328);
            this.hidePostalLabel.Name = "hidePostalLabel";
            this.hidePostalLabel.Size = new System.Drawing.Size(45, 13);
            this.hidePostalLabel.TabIndex = 19;
            this.hidePostalLabel.Text = "Stealth:";
            // 
            // hidePostage
            // 
            this.hidePostage.AutoSize = true;
            this.hidePostage.BackColor = System.Drawing.Color.Transparent;
            this.hidePostage.Location = new System.Drawing.Point(90, 327);
            this.hidePostage.Name = "hidePostage";
            this.hidePostage.Size = new System.Drawing.Size(89, 17);
            this.hidePostage.TabIndex = 20;
            this.hidePostage.Text = "Hide Postage";
            this.hidePostage.UseVisualStyleBackColor = false;
            // 
            // labelUspsValidation
            // 
            this.labelUspsValidation.AutoSize = true;
            this.labelUspsValidation.BackColor = System.Drawing.Color.Transparent;
            this.labelUspsValidation.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUspsValidation.Location = new System.Drawing.Point(7, 336);
            this.labelUspsValidation.Name = "labelUspsValidation";
            this.labelUspsValidation.Size = new System.Drawing.Size(95, 13);
            this.labelUspsValidation.TabIndex = 72;
            this.labelUspsValidation.Text = "USPS Validation";
            // 
            // requireFullAddressValidation
            // 
            this.requireFullAddressValidation.AutoSize = true;
            this.requireFullAddressValidation.BackColor = System.Drawing.Color.Transparent;
            this.requireFullAddressValidation.Location = new System.Drawing.Point(79, 355);
            this.requireFullAddressValidation.Name = "requireFullAddressValidation";
            this.requireFullAddressValidation.Size = new System.Drawing.Size(170, 17);
            this.requireFullAddressValidation.TabIndex = 73;
            this.requireFullAddressValidation.Text = "Require full address validation";
            this.requireFullAddressValidation.UseVisualStyleBackColor = false;
            // 
            // memo1
            // 
            this.memo1.Location = new System.Drawing.Point(90, 241);
            this.memo1.MaxLength = 32767;
            this.memo1.Name = "memo1";
            this.memo1.Size = new System.Drawing.Size(210, 21);
            this.memo1.TabIndex = 14;
            this.memo1.TokenSuggestionFactory = commonTokenSuggestionsFactory3;
            // 
            // labelMemo1
            // 
            this.labelMemo1.BackColor = System.Drawing.Color.Transparent;
            this.labelMemo1.Location = new System.Drawing.Point(30, 241);
            this.labelMemo1.Name = "labelMemo1";
            this.labelMemo1.Size = new System.Drawing.Size(54, 21);
            this.labelMemo1.TabIndex = 13;
            this.labelMemo1.Text = "Memo 1:";
            this.labelMemo1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // memo3
            // 
            this.memo3.Location = new System.Drawing.Point(90, 295);
            this.memo3.MaxLength = 32767;
            this.memo3.Name = "memo3";
            this.memo3.Size = new System.Drawing.Size(210, 21);
            this.memo3.TabIndex = 18;
            this.memo3.TokenSuggestionFactory = commonTokenSuggestionsFactory2;
            // 
            // labelMemo3
            // 
            this.labelMemo3.BackColor = System.Drawing.Color.Transparent;
            this.labelMemo3.Location = new System.Drawing.Point(31, 295);
            this.labelMemo3.Name = "labelMemo3";
            this.labelMemo3.Size = new System.Drawing.Size(53, 21);
            this.labelMemo3.TabIndex = 17;
            this.labelMemo3.Text = "Memo 3:";
            this.labelMemo3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // memo2
            // 
            this.memo2.Location = new System.Drawing.Point(90, 268);
            this.memo2.MaxLength = 32767;
            this.memo2.Name = "memo2";
            this.memo2.Size = new System.Drawing.Size(210, 21);
            this.memo2.TabIndex = 16;
            this.memo2.TokenSuggestionFactory = commonTokenSuggestionsFactory1;
            // 
            // labelMemo2
            // 
            this.labelMemo2.BackColor = System.Drawing.Color.Transparent;
            this.labelMemo2.Location = new System.Drawing.Point(35, 268);
            this.labelMemo2.Name = "labelMemo2";
            this.labelMemo2.Size = new System.Drawing.Size(49, 21);
            this.labelMemo2.TabIndex = 15;
            this.labelMemo2.Text = "Memo 2:";
            this.labelMemo2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // UspsServiceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.sectionFrom);
            this.Name = "UspsServiceControl";
            this.Size = new System.Drawing.Size(384, 633);
            this.Controls.SetChildIndex(this.sectionLabelOptions, 0);
            this.Controls.SetChildIndex(this.sectionReturns, 0);
            this.Controls.SetChildIndex(this.sectionExpress, 0);
            this.Controls.SetChildIndex(this.sectionShipment, 0);
            this.Controls.SetChildIndex(this.sectionRecipient, 0);
            this.Controls.SetChildIndex(this.sectionFrom, 0);
            ((System.ComponentModel.ISupportInitialize)(this.sectionExpress.ContentPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionExpress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient.ContentPanel)).EndInit();
            this.sectionRecipient.ContentPanel.ResumeLayout(false);
            this.sectionRecipient.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns.ContentPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment.ContentPanel)).EndInit();
            this.sectionShipment.ContentPanel.ResumeLayout(false);
            this.sectionShipment.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionLabelOptions.ContentPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionLabelOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom.ContentPanel)).EndInit();
            this.sectionFrom.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionFrom;
        private ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl originControl;
        private ShipWorks.UI.Controls.MultiValueComboBox uspsAccount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label label2;
        private ShipWorks.UI.Controls.LinkControl linkManageUspsAccounts;
        private System.Windows.Forms.Label labelUspsValidation;
        private System.Windows.Forms.CheckBox requireFullAddressValidation;
        private Templates.Tokens.TemplateTokenTextBox memo1;
        private System.Windows.Forms.Label labelMemo1;
        private System.Windows.Forms.CheckBox hidePostage;
        private System.Windows.Forms.Label hidePostalLabel;
        private System.Windows.Forms.CheckBox rateShop;
        private Templates.Tokens.TemplateTokenTextBox memo2;
        private System.Windows.Forms.Label labelMemo2;
        private Templates.Tokens.TemplateTokenTextBox memo3;
        private System.Windows.Forms.Label labelMemo3;
    }
}
