using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.EquaShip
{
    partial class EquaShipServiceControl
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
            this.sectionFrom = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.originControl = new ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl();
            this.panelTop = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.equashipAccount = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.sectionRates = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.rateControl = new RateControl();
            this.service = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.saturdayDelivery = new System.Windows.Forms.CheckBox();
            this.shipDate = new ShipWorks.UI.Controls.MultiValueDateTimePicker();
            this.labelShipDate = new System.Windows.Forms.Label();
            this.packagingType = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelService = new System.Windows.Forms.Label();
            this.labelPackaging = new System.Windows.Forms.Label();
            this.insuranceControl = new ShipWorks.Shipping.Insurance.InsuranceSelectionControl();
            this.weight = new ShipWorks.UI.Controls.WeightControl();
            this.labelWeight = new System.Windows.Forms.Label();
            this.labelDimensions = new System.Windows.Forms.Label();
            this.dimensionsControl = new ShipWorks.Shipping.Editing.DimensionsControl();
            this.labelConfirmation = new System.Windows.Forms.Label();
            this.confirmation = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.emailNotification = new System.Windows.Forms.CheckBox();
            this.referenceGroup = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.customerShippingNotes = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.customerDescription = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.customerReference = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize) (this.sectionRecipient)).BeginInit();
            this.sectionRecipient.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReturns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionShipment)).BeginInit();
            this.sectionShipment.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionFrom)).BeginInit();
            this.sectionFrom.ContentPanel.SuspendLayout();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionRates)).BeginInit();
            this.sectionRates.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.referenceGroup)).BeginInit();
            this.referenceGroup.ContentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // sectionRecipient
            // 
            this.sectionRecipient.Location = new System.Drawing.Point(3, 34);
            this.sectionRecipient.Size = new System.Drawing.Size(496, 24);
            // 
            // sectionReturns
            // 
            this.sectionReturns.Location = new System.Drawing.Point(3, 439);
            this.sectionReturns.Size = new System.Drawing.Size(496, 24);

            this.sectionLabelOptions.Size = new System.Drawing.Size(496, 24);
            // 
            // sectionShipment
            // 
            // 
            // sectionShipment.ContentPanel
            // 
            this.sectionShipment.ContentPanel.Controls.Add(this.emailNotification);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelConfirmation);
            this.sectionShipment.ContentPanel.Controls.Add(this.confirmation);
            this.sectionShipment.ContentPanel.Controls.Add(this.weight);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelWeight);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelDimensions);
            this.sectionShipment.ContentPanel.Controls.Add(this.dimensionsControl);
            this.sectionShipment.ContentPanel.Controls.Add(this.insuranceControl);
            this.sectionShipment.ContentPanel.Controls.Add(this.service);
            this.sectionShipment.ContentPanel.Controls.Add(this.saturdayDelivery);
            this.sectionShipment.ContentPanel.Controls.Add(this.shipDate);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelShipDate);
            this.sectionShipment.ContentPanel.Controls.Add(this.packagingType);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelService);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelPackaging);
            this.sectionShipment.Location = new System.Drawing.Point(3, 149);
            this.sectionShipment.Size = new System.Drawing.Size(496, 285);
            // 
            // sectionFrom
            // 
            this.sectionFrom.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionFrom.Collapsed = true;
            // 
            // sectionFrom.ContentPanel
            // 
            this.sectionFrom.ContentPanel.Controls.Add(this.originControl);
            this.sectionFrom.ContentPanel.Controls.Add(this.panelTop);
            this.sectionFrom.ExpandedHeight = 538;
            this.sectionFrom.ExtraText = "";
            this.sectionFrom.Location = new System.Drawing.Point(3, 5);
            this.sectionFrom.Name = "sectionFrom";
            this.sectionFrom.SectionName = "From";
            this.sectionFrom.SettingsKey = "6306b47c-8029-44bc-8b97-9b9eb001a61a";
            this.sectionFrom.Size = new System.Drawing.Size(496, 24);
            this.sectionFrom.TabIndex = 4;
            // 
            // originControl
            // 
            this.originControl.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.originControl.BackColor = System.Drawing.Color.Transparent;
            this.originControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.originControl.Location = new System.Drawing.Point(11, 49);
            this.originControl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.originControl.Name = "originControl";
            this.originControl.Size = new System.Drawing.Size(469, 465);
            this.originControl.TabIndex = 3;
            this.originControl.OriginChanged += new System.EventHandler(this.OnOriginChanged);
            // 
            // panelTop
            // 
            this.panelTop.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTop.BackColor = System.Drawing.Color.Transparent;
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Controls.Add(this.equashipAccount);
            this.panelTop.Location = new System.Drawing.Point(11, -2);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(469, 50);
            this.panelTop.TabIndex = 2;
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
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "EquaShip Account";
            // 
            // equashipAccount
            // 
            this.equashipAccount.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.equashipAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.equashipAccount.FormattingEnabled = true;
            this.equashipAccount.Location = new System.Drawing.Point(79, 25);
            this.equashipAccount.Name = "equashipAccount";
            this.equashipAccount.PromptText = "(Multiple Values)";
            this.equashipAccount.Size = new System.Drawing.Size(376, 21);
            this.equashipAccount.TabIndex = 0;
            this.equashipAccount.SelectedIndexChanged += new System.EventHandler(this.OnChangeEquashipAccount);
            // 
            // sectionRates
            // 
            this.sectionRates.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // sectionRates.ContentPanel
            // 
            this.sectionRates.ContentPanel.Controls.Add(this.rateControl);
            this.sectionRates.ExtraText = "";
            this.sectionRates.Location = new System.Drawing.Point(3, 63);
            this.sectionRates.Name = "sectionRates";
            this.sectionRates.SectionName = "Rates";
            this.sectionRates.SettingsKey = "{4b96a784-c2c9-4e5e-9f58-28adec07349f}";
            this.sectionRates.Size = new System.Drawing.Size(496, 81);
            this.sectionRates.TabIndex = 5;
            // 
            // rateControl
            // 
            this.rateControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rateControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.rateControl.Location = new System.Drawing.Point(0, 0);
            this.rateControl.Name = "rateControl";
            this.rateControl.Size = new System.Drawing.Size(492, 55);
            this.rateControl.TabIndex = 3;
            this.rateControl.RateSelected += new RateSelectedEventHandler(this.OnRateSelected);
            // 
            // service
            // 
            this.service.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.service.FormattingEnabled = true;
            this.service.Location = new System.Drawing.Point(94, 16);
            this.service.Name = "service";
            this.service.PromptText = "(Multiple Values)";
            this.service.Size = new System.Drawing.Size(175, 21);
            this.service.TabIndex = 9;
            // 
            // saturdayDelivery
            // 
            this.saturdayDelivery.AutoSize = true;
            this.saturdayDelivery.BackColor = System.Drawing.Color.White;
            this.saturdayDelivery.Location = new System.Drawing.Point(244, 47);
            this.saturdayDelivery.Name = "saturdayDelivery";
            this.saturdayDelivery.Size = new System.Drawing.Size(112, 17);
            this.saturdayDelivery.TabIndex = 12;
            this.saturdayDelivery.Text = "Saturday Delivery";
            this.saturdayDelivery.UseVisualStyleBackColor = false;
            // 
            // shipDate
            // 
            this.shipDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.shipDate.Location = new System.Drawing.Point(94, 44);
            this.shipDate.Name = "shipDate";
            this.shipDate.Size = new System.Drawing.Size(144, 21);
            this.shipDate.TabIndex = 11;
            this.shipDate.ValueChanged += new System.EventHandler(this.OnChangeShipDate);
            // 
            // labelShipDate
            // 
            this.labelShipDate.AutoSize = true;
            this.labelShipDate.BackColor = System.Drawing.Color.Transparent;
            this.labelShipDate.Location = new System.Drawing.Point(32, 48);
            this.labelShipDate.Name = "labelShipDate";
            this.labelShipDate.Size = new System.Drawing.Size(56, 13);
            this.labelShipDate.TabIndex = 10;
            this.labelShipDate.Text = "Ship date:";
            // 
            // packagingType
            // 
            this.packagingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.packagingType.FormattingEnabled = true;
            this.packagingType.Location = new System.Drawing.Point(94, 70);
            this.packagingType.Name = "packagingType";
            this.packagingType.PromptText = "(Multiple Values)";
            this.packagingType.Size = new System.Drawing.Size(144, 21);
            this.packagingType.TabIndex = 14;
            this.packagingType.SelectedIndexChanged += new System.EventHandler(this.OnChangePackaging);
            // 
            // labelService
            // 
            this.labelService.AutoSize = true;
            this.labelService.BackColor = System.Drawing.Color.Transparent;
            this.labelService.Location = new System.Drawing.Point(42, 19);
            this.labelService.Name = "labelService";
            this.labelService.Size = new System.Drawing.Size(46, 13);
            this.labelService.TabIndex = 8;
            this.labelService.Text = "Service:";
            // 
            // labelPackaging
            // 
            this.labelPackaging.AutoSize = true;
            this.labelPackaging.BackColor = System.Drawing.Color.Transparent;
            this.labelPackaging.Location = new System.Drawing.Point(29, 73);
            this.labelPackaging.Name = "labelPackaging";
            this.labelPackaging.Size = new System.Drawing.Size(59, 13);
            this.labelPackaging.TabIndex = 13;
            this.labelPackaging.Text = "Packaging:";
            // 
            // insuranceControl
            // 
            this.insuranceControl.BackColor = System.Drawing.Color.White;
            this.insuranceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.insuranceControl.Location = new System.Drawing.Point(27, 203);
            this.insuranceControl.Name = "insuranceControl";
            this.insuranceControl.Size = new System.Drawing.Size(416, 46);
            this.insuranceControl.TabIndex = 16;
            // 
            // weight
            // 
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.weight.Location = new System.Drawing.Point(94, 97);
            this.weight.Name = "weight";
            this.weight.RangeMax = 2000D;
            this.weight.RangeMin = 0D;
            this.weight.Size = new System.Drawing.Size(218, 21);
            this.weight.TabIndex = 18;
            this.weight.Weight = 0D;
            // 
            // labelWeight
            // 
            this.labelWeight.AutoSize = true;
            this.labelWeight.BackColor = System.Drawing.Color.Transparent;
            this.labelWeight.Location = new System.Drawing.Point(43, 100);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(45, 13);
            this.labelWeight.TabIndex = 17;
            this.labelWeight.Text = "Weight:";
            // 
            // labelDimensions
            // 
            this.labelDimensions.AutoSize = true;
            this.labelDimensions.BackColor = System.Drawing.Color.Transparent;
            this.labelDimensions.Location = new System.Drawing.Point(24, 129);
            this.labelDimensions.Name = "labelDimensions";
            this.labelDimensions.Size = new System.Drawing.Size(64, 13);
            this.labelDimensions.TabIndex = 19;
            this.labelDimensions.Text = "Dimensions:";
            // 
            // dimensionsControl
            // 
            this.dimensionsControl.BackColor = System.Drawing.Color.White;
            this.dimensionsControl.Cleared = false;
            this.dimensionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.dimensionsControl.Location = new System.Drawing.Point(91, 123);
            this.dimensionsControl.Name = "dimensionsControl";
            this.dimensionsControl.Size = new System.Drawing.Size(210, 74);
            this.dimensionsControl.TabIndex = 20;
            // 
            // labelConfirmation
            // 
            this.labelConfirmation.AutoSize = true;
            this.labelConfirmation.BackColor = System.Drawing.Color.Transparent;
            this.labelConfirmation.Location = new System.Drawing.Point(394, 103);
            this.labelConfirmation.Name = "labelConfirmation";
            this.labelConfirmation.Size = new System.Drawing.Size(72, 13);
            this.labelConfirmation.TabIndex = 21;
            this.labelConfirmation.Text = "Confirmation:";
            this.labelConfirmation.Visible = false;
            // 
            // confirmation
            // 
            this.confirmation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.confirmation.FormattingEnabled = true;
            this.confirmation.Location = new System.Drawing.Point(345, 122);
            this.confirmation.Name = "confirmation";
            this.confirmation.PromptText = "(Multiple Values)";
            this.confirmation.Size = new System.Drawing.Size(144, 21);
            this.confirmation.TabIndex = 22;
            this.confirmation.Visible = false;
            // 
            // emailNotification
            // 
            this.emailNotification.AutoSize = true;
            this.emailNotification.BackColor = System.Drawing.Color.White;
            this.emailNotification.Location = new System.Drawing.Point(374, 150);
            this.emailNotification.Name = "emailNotification";
            this.emailNotification.Size = new System.Drawing.Size(107, 17);
            this.emailNotification.TabIndex = 23;
            this.emailNotification.Text = "Email Notification";
            this.emailNotification.UseVisualStyleBackColor = false;
            this.emailNotification.Visible = false;
            // 
            // referenceGroup
            // 
            this.referenceGroup.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.referenceGroup.Collapsed = true;
            // 
            // referenceGroup.ContentPanel
            // 
            this.referenceGroup.ContentPanel.Controls.Add(this.customerShippingNotes);
            this.referenceGroup.ContentPanel.Controls.Add(this.customerDescription);
            this.referenceGroup.ContentPanel.Controls.Add(this.label5);
            this.referenceGroup.ContentPanel.Controls.Add(this.label4);
            this.referenceGroup.ContentPanel.Controls.Add(this.customerReference);
            this.referenceGroup.ContentPanel.Controls.Add(this.label3);
            this.referenceGroup.ExpandedHeight = 129;
            this.referenceGroup.ExtraText = "";
            this.referenceGroup.Location = new System.Drawing.Point(4, 468);
            this.referenceGroup.Name = "referenceGroup";
            this.referenceGroup.SectionName = "Reference";
            this.referenceGroup.SettingsKey = "{541cbc1f-46dc-4c19-9d19-83374add0a6b}";
            this.referenceGroup.Size = new System.Drawing.Size(495, 24);
            this.referenceGroup.TabIndex = 6;
            // 
            // customerShippingNotes
            // 
            this.customerShippingNotes.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.customerShippingNotes.Location = new System.Drawing.Point(106, 64);
            this.customerShippingNotes.MaxLength = 32767;
            this.customerShippingNotes.Name = "customerShippingNotes";
            this.customerShippingNotes.Size = new System.Drawing.Size(341, 21);
            this.customerShippingNotes.TabIndex = 8;
            // 
            // customerDescription
            // 
            this.customerDescription.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.customerDescription.Location = new System.Drawing.Point(106, 37);
            this.customerDescription.MaxLength = 32767;
            this.customerDescription.Name = "customerDescription";
            this.customerDescription.Size = new System.Drawing.Size(341, 21);
            this.customerDescription.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(18, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Shipping Notes:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(36, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Description:";
            // 
            // customerReference
            // 
            this.customerReference.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.customerReference.Location = new System.Drawing.Point(106, 10);
            this.customerReference.MaxLength = 32767;
            this.customerReference.Name = "customerReference";
            this.customerReference.Size = new System.Drawing.Size(341, 21);
            this.customerReference.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(28, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Reference #:";
            // 
            // EquaShipServiceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.sectionFrom);
            this.Controls.Add(this.sectionRates);
            this.Controls.Add(this.referenceGroup);
            this.Name = "EquaShipServiceControl";
            this.Size = new System.Drawing.Size(502, 564);
            this.Controls.SetChildIndex(this.referenceGroup, 0);
            this.Controls.SetChildIndex(this.sectionReturns, 0);
            this.Controls.SetChildIndex(this.sectionShipment, 0);
            this.Controls.SetChildIndex(this.sectionRates, 0);
            this.Controls.SetChildIndex(this.sectionRecipient, 0);
            this.Controls.SetChildIndex(this.sectionFrom, 0);
            this.sectionRecipient.ContentPanel.ResumeLayout(false);
            this.sectionRecipient.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionRecipient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionLabelOptions.ContentPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionLabelOptions)).EndInit();
            this.sectionShipment.ContentPanel.ResumeLayout(false);
            this.sectionShipment.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionShipment)).EndInit();
            this.sectionFrom.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.sectionFrom)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.sectionRates.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.sectionRates)).EndInit();
            this.referenceGroup.ContentPanel.ResumeLayout(false);
            this.referenceGroup.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.referenceGroup)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Controls.CollapsibleGroupControl sectionFrom;
        private UI.Controls.CollapsibleGroupControl sectionRates;
        private UI.Controls.MultiValueComboBox service;
        private System.Windows.Forms.CheckBox saturdayDelivery;
        private UI.Controls.MultiValueDateTimePicker shipDate;
        private System.Windows.Forms.Label labelShipDate;
        private UI.Controls.MultiValueComboBox packagingType;
        private System.Windows.Forms.Label labelService;
        private System.Windows.Forms.Label labelPackaging;
        private Insurance.InsuranceSelectionControl insuranceControl;
        private Settings.Origin.ShipmentOriginControl originControl;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private UI.Controls.MultiValueComboBox equashipAccount;
        private UI.Controls.WeightControl weight;
        private System.Windows.Forms.Label labelWeight;
        private System.Windows.Forms.Label labelDimensions;
        private Editing.DimensionsControl dimensionsControl;
        private RateControl rateControl;
        private System.Windows.Forms.Label labelConfirmation;
        private UI.Controls.MultiValueComboBox confirmation;
        private System.Windows.Forms.CheckBox emailNotification;
        private UI.Controls.CollapsibleGroupControl referenceGroup;
        private System.Windows.Forms.Label label3;
        private Templates.Tokens.TemplateTokenTextBox customerReference;
        private Templates.Tokens.TemplateTokenTextBox customerShippingNotes;
        private Templates.Tokens.TemplateTokenTextBox customerDescription;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;

    }
}
