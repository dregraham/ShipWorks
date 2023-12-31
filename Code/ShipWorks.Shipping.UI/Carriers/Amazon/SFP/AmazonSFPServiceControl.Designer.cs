﻿namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    partial class AmazonSFPServiceControl
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            this.components = new System.ComponentModel.Container();
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory1 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            this.weight = new ShipWorks.UI.Controls.WeightControl();
            this.labelWeight = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.sectionFrom = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.originControl = new ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.insuranceControl = new ShipWorks.Shipping.Insurance.InsuranceSelectionControl();
            this.dimensionsControl = new ShipWorks.Shipping.Editing.DimensionsControl();
            this.label2 = new System.Windows.Forms.Label();
            this.labelDimensions = new System.Windows.Forms.Label();
            this.deliveryConfirmation = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelShipDate = new System.Windows.Forms.Label();
            this.shipDate = new ShipWorks.UI.Controls.MultiValueDateTimePicker();
            this.service = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelDeliveryConf = new System.Windows.Forms.Label();
            this.labelReference1 = new System.Windows.Forms.Label();
            this.labelService = new System.Windows.Forms.Label();
            this.referenceTemplateToken = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.referenceInfoTip = new ShipWorks.UI.Controls.InfoTip();
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
            this.sectionLabelOptions.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom.ContentPanel)).BeginInit();
            this.sectionFrom.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
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
            this.sectionRecipient.Location = new System.Drawing.Point(3, 304);
            this.sectionRecipient.Size = new System.Drawing.Size(216, 24);
            this.sectionRecipient.TabIndex = 1;
            this.sectionRecipient.Visible = false;
            // 
            // personControl
            // 
            this.personControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.personControl.Size = new System.Drawing.Size(70, 330);
            // 
            // sectionReturns
            // 
            this.sectionReturns.Location = new System.Drawing.Point(3, 333);
            this.sectionReturns.Size = new System.Drawing.Size(216, 24);
            this.sectionReturns.Visible = false;
            // 
            // sectionShipment
            // 
            // 
            // sectionShipment.ContentPanel
            // 
            this.sectionShipment.ContentPanel.Controls.Add(this.insuranceControl);
            this.sectionShipment.ContentPanel.Controls.Add(this.weight);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelWeight);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelDimensions);
            this.sectionShipment.ContentPanel.Controls.Add(this.dimensionsControl);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelDeliveryConf);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelReference1);
            this.sectionShipment.ContentPanel.Controls.Add(this.deliveryConfirmation);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelShipDate);
            this.sectionShipment.ContentPanel.Controls.Add(this.shipDate);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelService);
            this.sectionShipment.ContentPanel.Controls.Add(this.service);
            this.sectionShipment.ContentPanel.Controls.Add(this.referenceTemplateToken);
            this.sectionShipment.ContentPanel.Controls.Add(this.referenceInfoTip);
            this.sectionShipment.Location = new System.Drawing.Point(3, 5);
            this.sectionShipment.Size = new System.Drawing.Size(403, 294);
            // 
            // sectionLabelOptions
            // 
            this.sectionLabelOptions.Collapsed = false;
            // 
            // sectionLabelOptions.ContentPanel
            // 
            this.sectionLabelOptions.ContentPanel.Controls.Add(this.labelFormat);
            this.sectionLabelOptions.Location = new System.Drawing.Point(3, 391);
            this.sectionLabelOptions.Size = new System.Drawing.Size(403, 70);
            // 
            // weight
            // 
            this.weight.AutoSize = true;
            this.weight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.ConfigureTelemetryEntityCounts = null;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weight.Location = new System.Drawing.Point(91, 61);
            this.weight.Name = "weight";
            this.weight.RangeMax = 300D;
            this.weight.RangeMin = 0D;
            this.weight.ShowShortcutInfo = true;
            this.weight.Size = new System.Drawing.Size(269, 24);
            this.weight.TabIndex = 7;
            this.weight.Weight = 0D;
            // 
            // labelWeight
            // 
            this.labelWeight.AutoSize = true;
            this.labelWeight.BackColor = System.Drawing.Color.Transparent;
            this.labelWeight.Location = new System.Drawing.Point(40, 65);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(45, 13);
            this.labelWeight.TabIndex = 6;
            this.labelWeight.Text = "Weight:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(159, 125);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
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
            this.sectionFrom.ContentPanel.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.sectionFrom.ContentPanel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.sectionFrom.ExpandedHeight = 487;
            this.sectionFrom.ExtraText = "";
            this.sectionFrom.Location = new System.Drawing.Point(3, 362);
            this.sectionFrom.Name = "sectionFrom";
            this.sectionFrom.SectionName = "From";
            this.sectionFrom.SettingsKey = "6306b47c-8029-44bc-8b97-9b9eb001a61a";
            this.sectionFrom.Size = new System.Drawing.Size(403, 24);
            this.sectionFrom.TabIndex = 0;
            // 
            // originControl
            // 
            this.originControl.BackColor = System.Drawing.Color.Transparent;
            this.originControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.originControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.originControl.Location = new System.Drawing.Point(0, 5);
            this.originControl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.originControl.Name = "originControl";
            this.originControl.Size = new System.Drawing.Size(399, 0);
            this.originControl.TabIndex = 1;
            this.originControl.OriginChanged += new System.EventHandler(this.OnOriginChanged);
            // 
            // insuranceControl
            // 
            this.insuranceControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.insuranceControl.BackColor = System.Drawing.Color.Transparent;
            this.insuranceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceControl.Location = new System.Drawing.Point(22, 187);
            this.insuranceControl.Name = "insuranceControl";
            this.insuranceControl.Size = new System.Drawing.Size(691, 48);
            this.insuranceControl.TabIndex = 12;
            // 
            // dimensionsControl
            // 
            this.dimensionsControl.BackColor = System.Drawing.Color.White;
            this.dimensionsControl.Cleared = false;
            this.dimensionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dimensionsControl.Location = new System.Drawing.Point(88, 85);
            this.dimensionsControl.Name = "dimensionsControl";
            this.dimensionsControl.Size = new System.Drawing.Size(210, 77);
            this.dimensionsControl.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(34, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Weight:";
            // 
            // labelDimensions
            // 
            this.labelDimensions.AutoSize = true;
            this.labelDimensions.BackColor = System.Drawing.Color.White;
            this.labelDimensions.Location = new System.Drawing.Point(21, 92);
            this.labelDimensions.Name = "labelDimensions";
            this.labelDimensions.Size = new System.Drawing.Size(64, 13);
            this.labelDimensions.TabIndex = 7;
            this.labelDimensions.Text = "Dimensions:";
            // 
            // deliveryConfirmation
            // 
            this.deliveryConfirmation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.deliveryConfirmation.FormattingEnabled = true;
            this.deliveryConfirmation.Location = new System.Drawing.Point(91, 162);
            this.deliveryConfirmation.Name = "deliveryConfirmation";
            this.deliveryConfirmation.PromptText = "(Multiple Values)";
            this.deliveryConfirmation.Size = new System.Drawing.Size(220, 21);
            this.deliveryConfirmation.TabIndex = 9;
            // 
            // labelShipDate
            // 
            this.labelShipDate.AutoSize = true;
            this.labelShipDate.BackColor = System.Drawing.Color.Transparent;
            this.labelShipDate.Location = new System.Drawing.Point(29, 38);
            this.labelShipDate.Name = "labelShipDate";
            this.labelShipDate.Size = new System.Drawing.Size(56, 13);
            this.labelShipDate.TabIndex = 4;
            this.labelShipDate.Text = "Ship date:";
            // 
            // shipDate
            // 
            this.shipDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.shipDate.Location = new System.Drawing.Point(91, 34);
            this.shipDate.Name = "shipDate";
            this.shipDate.Size = new System.Drawing.Size(124, 21);
            this.shipDate.TabIndex = 5;
            // 
            // service
            // 
            this.service.DisplayMember = "Description";
            this.service.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.service.FormattingEnabled = true;
            this.service.Location = new System.Drawing.Point(91, 7);
            this.service.Name = "service";
            this.service.PromptText = "(Multiple Values)";
            this.service.Size = new System.Drawing.Size(220, 21);
            this.service.TabIndex = 6;
            this.service.ValueMember = "ApiValue";
            // 
            // labelDeliveryConf
            // 
            this.labelDeliveryConf.AutoSize = true;
            this.labelDeliveryConf.BackColor = System.Drawing.Color.White;
            this.labelDeliveryConf.Location = new System.Drawing.Point(13, 166);
            this.labelDeliveryConf.Name = "labelDeliveryConf";
            this.labelDeliveryConf.Size = new System.Drawing.Size(72, 13);
            this.labelDeliveryConf.TabIndex = 7;
            this.labelDeliveryConf.Text = "Confirmation:";
            // 
            // labelReference1
            // 
            this.labelReference1.AutoSize = true;
            this.labelReference1.BackColor = System.Drawing.Color.White;
            this.labelReference1.Location = new System.Drawing.Point(13, 240);
            this.labelReference1.Name = "labelReference1";
            this.labelReference1.Size = new System.Drawing.Size(72, 13);
            this.labelReference1.TabIndex = 7;
            this.labelReference1.Text = "Reference #:";
            // 
            // labelService
            // 
            this.labelService.AutoSize = true;
            this.labelService.BackColor = System.Drawing.Color.White;
            this.labelService.Location = new System.Drawing.Point(39, 11);
            this.labelService.Name = "labelService";
            this.labelService.Size = new System.Drawing.Size(46, 13);
            this.labelService.TabIndex = 7;
            this.labelService.Text = "Service:";
            // 
            // referenceTemplateToken
            // 
            this.referenceTemplateToken.Location = new System.Drawing.Point(91, 236);
            this.referenceTemplateToken.MaxLength = 32767;
            this.referenceTemplateToken.Name = "referenceTemplateToken";
            this.referenceTemplateToken.Size = new System.Drawing.Size(210, 21);
            this.referenceTemplateToken.TabIndex = 6;
            this.referenceTemplateToken.TokenSuggestionFactory = commonTokenSuggestionsFactory1;
            // 
            // referenceInfoTip
            // 
            this.referenceInfoTip.Caption = "Reference number is only available when using thermal (ZPL) label format.";
            this.referenceInfoTip.Location = new System.Drawing.Point(310, 239);
            this.referenceInfoTip.Name = "referenceInfoTip";
            this.referenceInfoTip.Size = new System.Drawing.Size(12, 12);
            this.referenceInfoTip.TabIndex = 13;
            this.referenceInfoTip.Title = "Reference";
            // 
            // AmazonServiceControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.Controls.Add(this.sectionFrom);
            this.Name = "AmazonServiceControl";
            this.Size = new System.Drawing.Size(409, 427);
            this.Controls.SetChildIndex(this.sectionLabelOptions, 0);
            this.Controls.SetChildIndex(this.sectionFrom, 0);
            this.Controls.SetChildIndex(this.sectionReturns, 0);
            this.Controls.SetChildIndex(this.sectionRecipient, 0);
            this.Controls.SetChildIndex(this.sectionShipment, 0);
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
            this.sectionLabelOptions.ContentPanel.ResumeLayout(false);
            this.sectionLabelOptions.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionLabelOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom.ContentPanel)).EndInit();
            this.sectionFrom.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelWeight;
        private ShipWorks.UI.Controls.WeightControl weight;
        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionFrom;
        private ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl originControl;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private Insurance.InsuranceSelectionControl insuranceControl;
        private System.Windows.Forms.Label labelDimensions;
        private Editing.DimensionsControl dimensionsControl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelDeliveryConf;
        private System.Windows.Forms.Label labelReference1;
        private ShipWorks.UI.Controls.MultiValueComboBox deliveryConfirmation;
        private System.Windows.Forms.Label labelService;
        private ShipWorks.UI.Controls.MultiValueComboBox service;
        private System.Windows.Forms.Label labelShipDate;
        private ShipWorks.UI.Controls.MultiValueDateTimePicker shipDate;
        private Templates.Tokens.TemplateTokenTextBox referenceTemplateToken;
        private ShipWorks.UI.Controls.InfoTip referenceInfoTip;
    }
}
