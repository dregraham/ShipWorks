namespace ShipWorks.Shipping.Carriers.Postal.Usps.Express1
{
    partial class Express1UspsServiceControl
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
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory1 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            this.sectionFrom = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.originControl = new ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl();
            this.panelTop = new System.Windows.Forms.Panel();
            this.linkManageUspsAccounts = new ShipWorks.UI.Controls.LinkControl();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.uspsAccount = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelStampsValidation = new System.Windows.Forms.Label();
            this.requireFullAddressValidation = new System.Windows.Forms.CheckBox();
            this.memo = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelMemo = new System.Windows.Forms.Label();
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
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom.ContentPanel)).BeginInit();
            this.sectionFrom.ContentPanel.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // insuranceControl
            // 
            this.insuranceControl.Location = new System.Drawing.Point(14, 277);
            this.insuranceControl.Size = new System.Drawing.Size(376, 50);
            // 
            // sectionExpress
            // 
            this.sectionExpress.Location = new System.Drawing.Point(3, 550);
            this.sectionExpress.Size = new System.Drawing.Size(378, 24);
            // 
            // sectionRecipient
            // 
            // 
            // sectionRecipient.ContentPanel
            // 
            this.sectionRecipient.ContentPanel.Controls.Add(this.labelStampsValidation);
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
            this.sectionReturns.Location = new System.Drawing.Point(3, 579);
            this.sectionReturns.Size = new System.Drawing.Size(378, 24);

            this.sectionLabelOptions.Size = new System.Drawing.Size(378, 24);
            // 
            // sectionShipment
            // 
            // 
            // sectionShipment.ContentPanel
            // 
            this.sectionShipment.ContentPanel.Controls.Add(this.memo);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelMemo);
            this.sectionShipment.Location = new System.Drawing.Point(3, 161);
            this.sectionShipment.Size = new System.Drawing.Size(378, 360);
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
            this.originControl.Location = new System.Drawing.Point(0, 51);
            this.originControl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.originControl.Name = "originControl";
            this.originControl.Size = new System.Drawing.Size(371, 465);
            this.originControl.TabIndex = 1;
            this.originControl.OriginChanged += new System.EventHandler(this.OnOriginChanged);
            // 
            // panelTop
            // 
            this.panelTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTop.BackColor = System.Drawing.Color.Transparent;
            this.panelTop.Controls.Add(this.linkManageUspsAccounts);
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Controls.Add(this.uspsAccount);
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(371, 50);
            this.panelTop.TabIndex = 5;
            // 
            // linkManageStampsAccounts
            // 
            this.linkManageUspsAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkManageUspsAccounts.AutoSize = true;
            this.linkManageUspsAccounts.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkManageUspsAccounts.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkManageUspsAccounts.ForeColor = System.Drawing.Color.Blue;
            this.linkManageUspsAccounts.Location = new System.Drawing.Point(300, 28);
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
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Stamps.com";
            // 
            // uspsAccount
            // 
            this.uspsAccount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uspsAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uspsAccount.FormattingEnabled = true;
            this.uspsAccount.Location = new System.Drawing.Point(79, 25);
            this.uspsAccount.Name = "uspsAccount";
            this.uspsAccount.PromptText = "(Multiple Values)";
            this.uspsAccount.Size = new System.Drawing.Size(215, 21);
            this.uspsAccount.TabIndex = 3;
            this.uspsAccount.SelectedValueChanged += new System.EventHandler(this.OnOriginChanged);
            // 
            // labelStampsValidation
            // 
            this.labelStampsValidation.AutoSize = true;
            this.labelStampsValidation.BackColor = System.Drawing.Color.Transparent;
            this.labelStampsValidation.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStampsValidation.Location = new System.Drawing.Point(7, 336);
            this.labelStampsValidation.Name = "labelStampsValidation";
            this.labelStampsValidation.Size = new System.Drawing.Size(136, 13);
            this.labelStampsValidation.TabIndex = 72;
            this.labelStampsValidation.Text = "Stamps.com Validation";
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
            // memo
            // 
            this.memo.Location = new System.Drawing.Point(90, 241);
            this.memo.MaxLength = 32767;
            this.memo.Name = "memo";
            this.memo.Size = new System.Drawing.Size(210, 21);
            this.memo.TabIndex = 73;
            this.memo.TokenSuggestionFactory = commonTokenSuggestionsFactory1;
            // 
            // labelMemo
            // 
            this.labelMemo.BackColor = System.Drawing.Color.Transparent;
            this.labelMemo.Location = new System.Drawing.Point(41, 241);
            this.labelMemo.Name = "labelMemo";
            this.labelMemo.Size = new System.Drawing.Size(39, 21);
            this.labelMemo.TabIndex = 74;
            this.labelMemo.Text = "Memo:";
            this.labelMemo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // StampsServiceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.sectionFrom);
            this.Name = "Express1UspsServiceControl";
            this.Size = new System.Drawing.Size(384, 633);
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
            ((System.ComponentModel.ISupportInitialize)(this.sectionLabelOptions.ContentPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionLabelOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment.ContentPanel)).EndInit();
            this.sectionShipment.ContentPanel.ResumeLayout(false);
            this.sectionShipment.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment)).EndInit();
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
        private System.Windows.Forms.Label labelStampsValidation;
        private System.Windows.Forms.CheckBox requireFullAddressValidation;
        private Templates.Tokens.TemplateTokenTextBox memo;
        private System.Windows.Forms.Label labelMemo;
    }
}
