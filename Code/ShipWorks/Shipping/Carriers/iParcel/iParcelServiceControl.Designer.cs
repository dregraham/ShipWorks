namespace ShipWorks.Shipping.Carriers.iParcel
{
    partial class iParcelServiceControl
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
            this.panelTop = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.accountLabel = new System.Windows.Forms.Label();
            this.iParcelAccount = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.originControl = new ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl();
            this.service = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelService = new System.Windows.Forms.Label();
            this.referenceCustomer = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelReference = new System.Windows.Forms.Label();
            this.sectionOptions = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.isDeliveryDutyPaid = new System.Windows.Forms.CheckBox();
            this.labelDeliveryDutyPaid = new System.Windows.Forms.Label();
            this.emailTrack = new System.Windows.Forms.CheckBox();
            this.labelNotification = new System.Windows.Forms.Label();
            this.sectionReference = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.packageControl = new ShipWorks.Shipping.Carriers.iParcel.iParcelPackageControl();
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
            ((System.ComponentModel.ISupportInitialize)(this.sectionOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionOptions.ContentPanel)).BeginInit();
            this.sectionOptions.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReference)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReference.ContentPanel)).BeginInit();
            this.sectionReference.ContentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // sectionRecipient
            // 
            this.sectionRecipient.Location = new System.Drawing.Point(3, 34);
            this.sectionRecipient.Size = new System.Drawing.Size(389, 24);
            // 
            // personControl
            // 
            this.personControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.personControl.Size = new System.Drawing.Size(378, 330);
            // 
            // residentialDetermination
            // 
            this.residentialDetermination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.residentialDetermination.Size = new System.Drawing.Size(289, 21);
            // 
            // sectionReturns
            // 
            this.sectionReturns.Location = new System.Drawing.Point(3, 420);
            this.sectionReturns.Size = new System.Drawing.Size(389, 24);
            // 
            // sectionShipment
            // 
            // 
            // sectionShipment.ContentPanel
            // 
            this.sectionShipment.ContentPanel.Controls.Add(this.packageControl);
            this.sectionShipment.ContentPanel.Controls.Add(this.service);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelService);
            this.sectionShipment.Location = new System.Drawing.Point(3, 63);
            this.sectionShipment.Size = new System.Drawing.Size(389, 352);
            // 
            // sectionFrom
            // 
            this.sectionFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionFrom.Collapsed = true;
            // 
            // sectionFrom.ContentPanel
            // 
            this.sectionFrom.ContentPanel.Controls.Add(this.panelTop);
            this.sectionFrom.ContentPanel.Controls.Add(this.originControl);
            this.sectionFrom.ExpandedHeight = 513;
            this.sectionFrom.ExtraText = "";
            this.sectionFrom.Location = new System.Drawing.Point(3, 5);
            this.sectionFrom.Name = "sectionFrom";
            this.sectionFrom.SectionName = "From";
            this.sectionFrom.SettingsKey = "6306b47c-8029-44bc-8b97-9b9eb001a61a";
            this.sectionFrom.Size = new System.Drawing.Size(389, 24);
            this.sectionFrom.TabIndex = 4;
            // 
            // panelTop
            // 
            this.panelTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTop.BackColor = System.Drawing.Color.Transparent;
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.accountLabel);
            this.panelTop.Controls.Add(this.iParcelAccount);
            this.panelTop.Location = new System.Drawing.Point(1, 3);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(383, 50);
            this.panelTop.TabIndex = 3;
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
            // accountLabel
            // 
            this.accountLabel.AutoSize = true;
            this.accountLabel.BackColor = System.Drawing.Color.Transparent;
            this.accountLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountLabel.Location = new System.Drawing.Point(3, 7);
            this.accountLabel.Name = "accountLabel";
            this.accountLabel.Size = new System.Drawing.Size(99, 13);
            this.accountLabel.TabIndex = 2;
            this.accountLabel.Text = "i-parcel Account";
            // 
            // iParcelAccount
            // 
            this.iParcelAccount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.iParcelAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.iParcelAccount.FormattingEnabled = true;
            this.iParcelAccount.Location = new System.Drawing.Point(79, 25);
            this.iParcelAccount.Name = "iParcelAccount";
            this.iParcelAccount.PromptText = "(Multiple Values)";
            this.iParcelAccount.Size = new System.Drawing.Size(290, 21);
            this.iParcelAccount.TabIndex = 0;
            this.iParcelAccount.SelectedIndexChanged += new System.EventHandler(this.OnOriginChanged);
            // 
            // originControl
            // 
            this.originControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.originControl.BackColor = System.Drawing.Color.Transparent;
            this.originControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.originControl.Location = new System.Drawing.Point(1, 55);
            this.originControl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.originControl.Name = "originControl";
            this.originControl.Size = new System.Drawing.Size(383, 430);
            this.originControl.TabIndex = 2;
            this.originControl.OriginChanged += new System.EventHandler(this.OnOriginChanged);
            // 
            // service
            // 
            this.service.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.service.FormattingEnabled = true;
            this.service.Location = new System.Drawing.Point(110, 10);
            this.service.Name = "service";
            this.service.PromptText = "(Multiple Values)";
            this.service.Size = new System.Drawing.Size(175, 21);
            this.service.TabIndex = 3;
            // 
            // labelService
            // 
            this.labelService.AutoSize = true;
            this.labelService.BackColor = System.Drawing.Color.Transparent;
            this.labelService.Location = new System.Drawing.Point(58, 13);
            this.labelService.Name = "labelService";
            this.labelService.Size = new System.Drawing.Size(46, 13);
            this.labelService.TabIndex = 2;
            this.labelService.Text = "Service:";
            // 
            // referenceCustomer
            // 
            this.referenceCustomer.Location = new System.Drawing.Point(86, 8);
            this.referenceCustomer.MaxLength = 32767;
            this.referenceCustomer.Name = "referenceCustomer";
            this.referenceCustomer.Size = new System.Drawing.Size(210, 21);
            this.referenceCustomer.TabIndex = 3;
            this.referenceCustomer.TokenSuggestionFactory = commonTokenSuggestionsFactory1;
            // 
            // labelReference
            // 
            this.labelReference.AutoSize = true;
            this.labelReference.BackColor = System.Drawing.Color.Transparent;
            this.labelReference.Location = new System.Drawing.Point(8, 11);
            this.labelReference.Name = "labelReference";
            this.labelReference.Size = new System.Drawing.Size(72, 13);
            this.labelReference.TabIndex = 2;
            this.labelReference.Text = "Reference #:";
            // 
            // sectionOptions
            // 
            this.sectionOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // sectionOptions.ContentPanel
            // 
            this.sectionOptions.ContentPanel.Controls.Add(this.isDeliveryDutyPaid);
            this.sectionOptions.ContentPanel.Controls.Add(this.labelDeliveryDutyPaid);
            this.sectionOptions.ContentPanel.Controls.Add(this.emailTrack);
            this.sectionOptions.ContentPanel.Controls.Add(this.labelNotification);
            this.sectionOptions.ExtraText = "";
            this.sectionOptions.Location = new System.Drawing.Point(3, 478);
            this.sectionOptions.Name = "sectionOptions";
            this.sectionOptions.SectionName = "Options";
            this.sectionOptions.SettingsKey = "{2740f860-1d14-453e-a511-8f62ad1e7dcc}";
            this.sectionOptions.Size = new System.Drawing.Size(389, 93);
            this.sectionOptions.TabIndex = 7;
            // 
            // isDeliveryDutyPaid
            // 
            this.isDeliveryDutyPaid.AutoSize = true;
            this.isDeliveryDutyPaid.BackColor = System.Drawing.Color.White;
            this.isDeliveryDutyPaid.Location = new System.Drawing.Point(84, 38);
            this.isDeliveryDutyPaid.Name = "isDeliveryDutyPaid";
            this.isDeliveryDutyPaid.Size = new System.Drawing.Size(114, 17);
            this.isDeliveryDutyPaid.TabIndex = 8;
            this.isDeliveryDutyPaid.Text = "Duty Delivery Paid";
            this.isDeliveryDutyPaid.UseVisualStyleBackColor = false;
            // 
            // labelDeliveryDutyPaid
            // 
            this.labelDeliveryDutyPaid.AutoSize = true;
            this.labelDeliveryDutyPaid.BackColor = System.Drawing.Color.Transparent;
            this.labelDeliveryDutyPaid.Location = new System.Drawing.Point(44, 38);
            this.labelDeliveryDutyPaid.Name = "labelDeliveryDutyPaid";
            this.labelDeliveryDutyPaid.Size = new System.Drawing.Size(34, 13);
            this.labelDeliveryDutyPaid.TabIndex = 7;
            this.labelDeliveryDutyPaid.Text = "Duty:";
            // 
            // emailTrack
            // 
            this.emailTrack.AutoSize = true;
            this.emailTrack.BackColor = System.Drawing.Color.White;
            this.emailTrack.Location = new System.Drawing.Point(84, 15);
            this.emailTrack.Name = "emailTrack";
            this.emailTrack.Size = new System.Drawing.Size(94, 17);
            this.emailTrack.TabIndex = 5;
            this.emailTrack.Text = "Track By Email";
            this.emailTrack.UseVisualStyleBackColor = false;
            // 
            // labelNotification
            // 
            this.labelNotification.AutoSize = true;
            this.labelNotification.BackColor = System.Drawing.Color.Transparent;
            this.labelNotification.Location = new System.Drawing.Point(8, 15);
            this.labelNotification.Name = "labelNotification";
            this.labelNotification.Size = new System.Drawing.Size(70, 13);
            this.labelNotification.TabIndex = 1;
            this.labelNotification.Text = "Notifications:";
            // 
            // sectionReference
            // 
            this.sectionReference.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionReference.Collapsed = true;
            // 
            // sectionReference.ContentPanel
            // 
            this.sectionReference.ContentPanel.Controls.Add(this.referenceCustomer);
            this.sectionReference.ContentPanel.Controls.Add(this.labelReference);
            this.sectionReference.ExpandedHeight = 70;
            this.sectionReference.ExtraText = "";
            this.sectionReference.Location = new System.Drawing.Point(3, 449);
            this.sectionReference.Name = "sectionReference";
            this.sectionReference.SectionName = "Reference";
            this.sectionReference.SettingsKey = "{2740f860-1d14-453e-a511-8f62ad1e7dcc}";
            this.sectionReference.Size = new System.Drawing.Size(389, 24);
            this.sectionReference.TabIndex = 8;
            // 
            // packageControl
            // 
            this.packageControl.BackColor = System.Drawing.Color.White;
            this.packageControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.packageControl.Location = new System.Drawing.Point(2, 37);
            this.packageControl.Name = "packageControl";
            this.packageControl.Size = new System.Drawing.Size(412, 292);
            this.packageControl.TabIndex = 4;
            this.packageControl.RateCriteriaChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            this.packageControl.Resize += new System.EventHandler(this.OnPackageControlSizeChanged);
            // 
            // iParcelServiceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.sectionFrom);
            this.Controls.Add(this.sectionReference);
            this.Controls.Add(this.sectionOptions);
            this.Name = "iParcelServiceControl";
            this.Size = new System.Drawing.Size(395, 795);
            this.Controls.SetChildIndex(this.sectionOptions, 0);
            this.Controls.SetChildIndex(this.sectionReference, 0);
            this.Controls.SetChildIndex(this.sectionReturns, 0);
            this.Controls.SetChildIndex(this.sectionShipment, 0);
            this.Controls.SetChildIndex(this.sectionRecipient, 0);
            this.Controls.SetChildIndex(this.sectionFrom, 0);
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
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom.ContentPanel)).EndInit();
            this.sectionFrom.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionOptions.ContentPanel)).EndInit();
            this.sectionOptions.ContentPanel.ResumeLayout(false);
            this.sectionOptions.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReference.ContentPanel)).EndInit();
            this.sectionReference.ContentPanel.ResumeLayout(false);
            this.sectionReference.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReference)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Controls.CollapsibleGroupControl sectionFrom;
        private Settings.Origin.ShipmentOriginControl originControl;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label accountLabel;
        private UI.Controls.MultiValueComboBox iParcelAccount;
        private UI.Controls.MultiValueComboBox service;
        private System.Windows.Forms.Label labelService;
        private Templates.Tokens.TemplateTokenTextBox referenceCustomer;
        private System.Windows.Forms.Label labelReference;
        private UI.Controls.CollapsibleGroupControl sectionOptions;
        private UI.Controls.CollapsibleGroupControl sectionReference;
        private System.Windows.Forms.Label labelNotification;
        private System.Windows.Forms.CheckBox emailTrack;
        private iParcelPackageControl packageControl;
        private System.Windows.Forms.CheckBox isDeliveryDutyPaid;
        private System.Windows.Forms.Label labelDeliveryDutyPaid;
    }
}
