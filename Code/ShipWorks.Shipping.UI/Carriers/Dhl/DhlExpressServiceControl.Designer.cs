namespace ShipWorks.Shipping.Carriers.Dhl
{
    partial class DhlExpressServiceControl
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
            this.cutoffDateDisplay = new ShipWorks.Shipping.Editing.ShippingDateCutoffDisplayControl();
            this.sectionFrom = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.panelTop = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.accountLabel = new System.Windows.Forms.Label();
            this.DhlExpressAccount = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.originControl = new ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl();
            this.service = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelShipDate = new System.Windows.Forms.Label();
            this.shipDate = new ShipWorks.UI.Controls.MultiValueDateTimePicker();
            this.labelService = new System.Windows.Forms.Label();
            this.sectionOptions = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.saturdayDelivery = new System.Windows.Forms.CheckBox();
            this.labelSaturdayDelivery = new System.Windows.Forms.Label();
            this.dutyPaid = new System.Windows.Forms.CheckBox();
            this.labelDutyPaid = new System.Windows.Forms.Label();
            this.labelNonMachinable = new System.Windows.Forms.Label();
            this.nonMachinable = new System.Windows.Forms.CheckBox();
            this.packageControl = new ShipWorks.Shipping.Carriers.Dhl.DhlExpressPackageControl();
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
            ((System.ComponentModel.ISupportInitialize)(this.sectionOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionOptions.ContentPanel)).BeginInit();
            this.sectionOptions.ContentPanel.SuspendLayout();
            this.SuspendLayout();
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
            this.sectionReturns.Location = new System.Drawing.Point(3, 369);
            this.sectionReturns.Size = new System.Drawing.Size(389, 24);
            // 
            // sectionShipment
            // 
            // 
            // sectionShipment.ContentPanel
            // 
            this.sectionShipment.ContentPanel.Controls.Add(this.packageControl);
            this.sectionShipment.ContentPanel.Controls.Add(this.service);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelShipDate);
            this.sectionShipment.ContentPanel.Controls.Add(this.shipDate);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelService);
            this.sectionShipment.ContentPanel.Controls.Add(this.cutoffDateDisplay);
            this.sectionShipment.Location = new System.Drawing.Point(3, 63);
            this.sectionShipment.Size = new System.Drawing.Size(389, 301);
            // 
            // sectionLabelOptions
            // 
            this.sectionLabelOptions.Collapsed = false;
            this.sectionLabelOptions.Location = new System.Drawing.Point(3, 516);
            this.sectionLabelOptions.Size = new System.Drawing.Size(389, 62);
            // 
            // cutoffDateDisplay
            // 
            this.cutoffDateDisplay.AutoSize = true;
            this.cutoffDateDisplay.BackColor = System.Drawing.Color.White;
            this.cutoffDateDisplay.Caption = "Shipments processed after 5:00 PM today will have a ship date of the next valid s" +
    "hipping day.\r\nTo update this setting, go to Manage > Shipping Settings > DHL Exp" +
    "ress > Settings.";
            this.cutoffDateDisplay.Location = new System.Drawing.Point(257, 45);
            this.cutoffDateDisplay.Name = "cutoffDateDisplay";
            this.cutoffDateDisplay.ShipmentType = ShipWorks.Shipping.ShipmentTypeCode.DhlExpress;
            this.cutoffDateDisplay.Size = new System.Drawing.Size(113, 15);
            this.cutoffDateDisplay.TabIndex = 6;
            this.cutoffDateDisplay.Title = "Shipment cutoff time";
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
            this.panelTop.Controls.Add(this.DhlExpressAccount);
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
            this.accountLabel.Size = new System.Drawing.Size(125, 13);
            this.accountLabel.TabIndex = 2;
            this.accountLabel.Text = "DHL Express Account";
            // 
            // DhlExpressAccount
            // 
            this.DhlExpressAccount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DhlExpressAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DhlExpressAccount.FormattingEnabled = true;
            this.DhlExpressAccount.Location = new System.Drawing.Point(79, 25);
            this.DhlExpressAccount.Name = "DhlExpressAccount";
            this.DhlExpressAccount.PromptText = "(Multiple Values)";
            this.DhlExpressAccount.Size = new System.Drawing.Size(290, 21);
            this.DhlExpressAccount.TabIndex = 0;
            this.DhlExpressAccount.SelectedIndexChanged += new System.EventHandler(this.OnOriginChanged);
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
            this.service.Location = new System.Drawing.Point(108, 10);
            this.service.Name = "service";
            this.service.PromptText = "(Multiple Values)";
            this.service.Size = new System.Drawing.Size(175, 21);
            this.service.TabIndex = 3;
            this.service.SelectedIndexChanged += new System.EventHandler(this.OnServiceChanged);
            // 
            // labelShipDate
            // 
            this.labelShipDate.AutoSize = true;
            this.labelShipDate.BackColor = System.Drawing.Color.Transparent;
            this.labelShipDate.Location = new System.Drawing.Point(46, 45);
            this.labelShipDate.Name = "labelShipDate";
            this.labelShipDate.Size = new System.Drawing.Size(56, 13);
            this.labelShipDate.TabIndex = 3;
            this.labelShipDate.Text = "Ship date:";
            // 
            // shipDate
            // 
            this.shipDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.shipDate.Location = new System.Drawing.Point(108, 41);
            this.shipDate.Name = "shipDate";
            this.shipDate.Size = new System.Drawing.Size(144, 21);
            this.shipDate.TabIndex = 4;
            // 
            // labelService
            // 
            this.labelService.AutoSize = true;
            this.labelService.BackColor = System.Drawing.Color.Transparent;
            this.labelService.Location = new System.Drawing.Point(56, 13);
            this.labelService.Name = "labelService";
            this.labelService.Size = new System.Drawing.Size(46, 13);
            this.labelService.TabIndex = 2;
            this.labelService.Text = "Service:";
            // 
            // sectionOptions
            // 
            this.sectionOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // sectionOptions.ContentPanel
            // 
            this.sectionOptions.ContentPanel.Controls.Add(this.saturdayDelivery);
            this.sectionOptions.ContentPanel.Controls.Add(this.labelSaturdayDelivery);
            this.sectionOptions.ContentPanel.Controls.Add(this.dutyPaid);
            this.sectionOptions.ContentPanel.Controls.Add(this.labelDutyPaid);
            this.sectionOptions.ContentPanel.Controls.Add(this.labelNonMachinable);
            this.sectionOptions.ContentPanel.Controls.Add(this.nonMachinable);
            this.sectionOptions.ExtraText = "";
            this.sectionOptions.Location = new System.Drawing.Point(3, 398);
            this.sectionOptions.Name = "sectionOptions";
            this.sectionOptions.SectionName = "Options";
            this.sectionOptions.SettingsKey = "{2740f860-1d14-453e-a511-8f62ad1e7dcc}";
            this.sectionOptions.Size = new System.Drawing.Size(389, 113);
            this.sectionOptions.TabIndex = 7;
            // 
            // saturdayDelivery
            // 
            this.saturdayDelivery.AutoSize = true;
            this.saturdayDelivery.BackColor = System.Drawing.Color.White;
            this.saturdayDelivery.Location = new System.Drawing.Point(108, 14);
            this.saturdayDelivery.Name = "saturdayDelivery";
            this.saturdayDelivery.Size = new System.Drawing.Size(112, 17);
            this.saturdayDelivery.TabIndex = 8;
            this.saturdayDelivery.Text = "Saturday Delivery";
            this.saturdayDelivery.UseVisualStyleBackColor = false;
            // 
            // labelSaturdayDelivery
            // 
            this.labelSaturdayDelivery.AutoSize = true;
            this.labelSaturdayDelivery.BackColor = System.Drawing.Color.Transparent;
            this.labelSaturdayDelivery.Location = new System.Drawing.Point(47, 15);
            this.labelSaturdayDelivery.Name = "labelSaturdayDelivery";
            this.labelSaturdayDelivery.Size = new System.Drawing.Size(55, 13);
            this.labelSaturdayDelivery.TabIndex = 7;
            this.labelSaturdayDelivery.Text = "Saturday:";
            // 
            // dutyPaid
            // 
            this.dutyPaid.AutoSize = true;
            this.dutyPaid.BackColor = System.Drawing.Color.White;
            this.dutyPaid.Location = new System.Drawing.Point(108, 37);
            this.dutyPaid.Name = "dutyPaid";
            this.dutyPaid.Size = new System.Drawing.Size(114, 17);
            this.dutyPaid.TabIndex = 5;
            this.dutyPaid.Text = "Delivery Duty Paid";
            this.dutyPaid.UseVisualStyleBackColor = false;
            // 
            // labelDutyPaid
            // 
            this.labelDutyPaid.AutoSize = true;
            this.labelDutyPaid.BackColor = System.Drawing.Color.Transparent;
            this.labelDutyPaid.Location = new System.Drawing.Point(68, 38);
            this.labelDutyPaid.Name = "labelDutyPaid";
            this.labelDutyPaid.Size = new System.Drawing.Size(34, 13);
            this.labelDutyPaid.TabIndex = 1;
            this.labelDutyPaid.Text = "Duty:";
            // 
            // labelNonMachinable
            // 
            this.labelNonMachinable.AutoSize = true;
            this.labelNonMachinable.BackColor = System.Drawing.Color.Transparent;
            this.labelNonMachinable.Location = new System.Drawing.Point(15, 61);
            this.labelNonMachinable.Name = "labelNonMachinable";
            this.labelNonMachinable.Size = new System.Drawing.Size(87, 13);
            this.labelNonMachinable.TabIndex = 7;
            this.labelNonMachinable.Text = "Non-Machinable:";
            // 
            // nonMachinable
            // 
            this.nonMachinable.AutoSize = true;
            this.nonMachinable.BackColor = System.Drawing.Color.White;
            this.nonMachinable.Location = new System.Drawing.Point(108, 60);
            this.nonMachinable.Name = "nonMachinable";
            this.nonMachinable.Size = new System.Drawing.Size(102, 17);
            this.nonMachinable.TabIndex = 8;
            this.nonMachinable.Text = "Non-Machinable";
            this.nonMachinable.UseVisualStyleBackColor = false;
            // 
            // packageControl
            // 
            this.packageControl.BackColor = System.Drawing.Color.White;
            this.packageControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.packageControl.Location = new System.Drawing.Point(2, 67);
            this.packageControl.Name = "packageControl";
            this.packageControl.Size = new System.Drawing.Size(412, 206);
            this.packageControl.TabIndex = 4;
            this.packageControl.Resize += new System.EventHandler(this.OnPackageControlSizeChanged);
            // 
            // DhlExpressServiceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.sectionFrom);
            this.Controls.Add(this.sectionOptions);
            this.Name = "DhlExpressServiceControl";
            this.Size = new System.Drawing.Size(395, 795);
            this.Controls.SetChildIndex(this.sectionLabelOptions, 0);
            this.Controls.SetChildIndex(this.sectionOptions, 0);
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
            ((System.ComponentModel.ISupportInitialize)(this.sectionLabelOptions.ContentPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionLabelOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom.ContentPanel)).EndInit();
            this.sectionFrom.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionOptions.ContentPanel)).EndInit();
            this.sectionOptions.ContentPanel.ResumeLayout(false);
            this.sectionOptions.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionOptions)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionFrom;
        private ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl originControl;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label accountLabel;
        private ShipWorks.UI.Controls.MultiValueComboBox DhlExpressAccount;
        private ShipWorks.UI.Controls.MultiValueComboBox service;
        private System.Windows.Forms.Label labelService;
        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionOptions;
        private System.Windows.Forms.Label labelDutyPaid;
        private System.Windows.Forms.CheckBox dutyPaid;
        private DhlExpressPackageControl packageControl;
        private System.Windows.Forms.CheckBox saturdayDelivery;
        private System.Windows.Forms.Label labelSaturdayDelivery;
        private System.Windows.Forms.CheckBox nonMachinable;
        private System.Windows.Forms.Label labelNonMachinable;
        private System.Windows.Forms.Label labelShipDate;
        private ShipWorks.UI.Controls.MultiValueDateTimePicker shipDate;
        private Editing.ShippingDateCutoffDisplayControl cutoffDateDisplay;
    }
}
