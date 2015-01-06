namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    partial class EndiciaServiceControl
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
            this.components = new System.ComponentModel.Container();
            this.sectionFrom = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.originControl = new ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl();
            this.panelTop = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.endiciaAccount = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.sectionRubberStamps = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.labelReferenceIdInfo = new System.Windows.Forms.Label();
            this.referenceID = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelReferenceID = new System.Windows.Forms.Label();
            this.labelRubberStampWarning = new System.Windows.Forms.Label();
            this.pictureBoxRubberStampWarning = new System.Windows.Forms.PictureBox();
            this.rubberStamp3 = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelRubberStamp3 = new System.Windows.Forms.Label();
            this.rubberStamp2 = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelRubberStamp2 = new System.Windows.Forms.Label();
            this.rubberStamp1 = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelRubberStamp1 = new System.Windows.Forms.Label();
            this.labelStealth = new System.Windows.Forms.Label();
            this.hidePostage = new System.Windows.Forms.CheckBox();
            this.noPostage = new System.Windows.Forms.CheckBox();
            this.labelNoPostage = new System.Windows.Forms.Label();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.sectionEntryFacility = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.labelEntryFacility = new System.Windows.Forms.Label();
            this.labelSortType = new System.Windows.Forms.Label();
            this.entryFacility = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.sortType = new ShipWorks.UI.Controls.MultiValueComboBox();
            ((System.ComponentModel.ISupportInitialize) (this.sectionExpress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionRecipient)).BeginInit();
            this.sectionRecipient.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReturns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionShipment)).BeginInit();
            this.sectionShipment.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionFrom)).BeginInit();
            this.sectionFrom.ContentPanel.SuspendLayout();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionRubberStamps)).BeginInit();
            this.sectionRubberStamps.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxRubberStampWarning)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionEntryFacility)).BeginInit();
            this.sectionEntryFacility.ContentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // insuranceControl
            // 
            this.insuranceControl.Location = new System.Drawing.Point(14, 282);
            this.insuranceControl.Size = new System.Drawing.Size(349, 50);
            // 
            // sectionExpress
            // 
            this.sectionExpress.Location = new System.Drawing.Point(3, 553);
            this.sectionExpress.Size = new System.Drawing.Size(375, 24);
            // 
            // sectionRecipient
            // 
            this.sectionRecipient.Location = new System.Drawing.Point(3, 34);
            this.sectionRecipient.Size = new System.Drawing.Size(375, 24);
            this.sectionRecipient.TabIndex = 1;
            // 
            // personControl
            // 
            this.personControl.Size = new System.Drawing.Size(361, 330);
            // 
            // sectionReturns
            // 
            this.sectionReturns.Location = new System.Drawing.Point(3, 682);
            this.sectionReturns.Size = new System.Drawing.Size(375, 24);

            this.sectionLabelOptions.Size = new System.Drawing.Size(375, 24);
            // 
            // sectionShipment
            // 
            // 
            // sectionShipment.ContentPanel
            // 
            this.sectionShipment.ContentPanel.Controls.Add(this.noPostage);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelNoPostage);
            this.sectionShipment.ContentPanel.Controls.Add(this.hidePostage);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelStealth);
            this.sectionShipment.Location = new System.Drawing.Point(3, 161);
            this.sectionShipment.Size = new System.Drawing.Size(375, 358);
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
            this.sectionFrom.ExpandedHeight = 453;
            this.sectionFrom.ExtraText = "";
            this.sectionFrom.Location = new System.Drawing.Point(3, 5);
            this.sectionFrom.Name = "sectionFrom";
            this.sectionFrom.SectionName = "From";
            this.sectionFrom.SettingsKey = "{fc17183f-f4c4-4a0e-b3f8-d756d5149594}";
            this.sectionFrom.Size = new System.Drawing.Size(375, 24);
            this.sectionFrom.TabIndex = 0;
            // 
            // originControl
            // 
            this.originControl.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.originControl.AvailableFields = ((ShipWorks.Data.Controls.PersonFields) (((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company)
                        | ShipWorks.Data.Controls.PersonFields.Street)
                        | ShipWorks.Data.Controls.PersonFields.City)
                        | ShipWorks.Data.Controls.PersonFields.State)
                        | ShipWorks.Data.Controls.PersonFields.Postal)
                        | ShipWorks.Data.Controls.PersonFields.Email)
                        | ShipWorks.Data.Controls.PersonFields.Phone)
                        | ShipWorks.Data.Controls.PersonFields.Fax)));
            this.originControl.BackColor = System.Drawing.Color.Transparent;
            this.originControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.originControl.Location = new System.Drawing.Point(4, 51);
            this.originControl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.originControl.Name = "originControl";
            this.originControl.Size = new System.Drawing.Size(361, 403);
            this.originControl.TabIndex = 10;
            this.originControl.OriginChanged += new System.EventHandler(this.OnOriginChanged);
            // 
            // panelTop
            // 
            this.panelTop.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTop.BackColor = System.Drawing.Color.Transparent;
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Controls.Add(this.endiciaAccount);
            this.panelTop.Location = new System.Drawing.Point(4, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(360, 50);
            this.panelTop.TabIndex = 11;
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
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Endicia";
            // 
            // endiciaAccount
            // 
            this.endiciaAccount.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.endiciaAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.endiciaAccount.FormattingEnabled = true;
            this.endiciaAccount.Location = new System.Drawing.Point(79, 25);
            this.endiciaAccount.Name = "endiciaAccount";
            this.endiciaAccount.PromptText = "(Multiple Values)";
            this.endiciaAccount.Size = new System.Drawing.Size(267, 21);
            this.endiciaAccount.TabIndex = 3;
            this.endiciaAccount.SelectedIndexChanged += new System.EventHandler(this.OnOriginChanged);
            // 
            // sectionRubberStamps
            // 
            this.sectionRubberStamps.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionRubberStamps.Collapsed = true;
            // 
            // sectionRubberStamps.ContentPanel
            // 
            this.sectionRubberStamps.ContentPanel.Controls.Add(this.labelReferenceIdInfo);
            this.sectionRubberStamps.ContentPanel.Controls.Add(this.referenceID);
            this.sectionRubberStamps.ContentPanel.Controls.Add(this.labelReferenceID);
            this.sectionRubberStamps.ContentPanel.Controls.Add(this.labelRubberStampWarning);
            this.sectionRubberStamps.ContentPanel.Controls.Add(this.pictureBoxRubberStampWarning);
            this.sectionRubberStamps.ContentPanel.Controls.Add(this.rubberStamp3);
            this.sectionRubberStamps.ContentPanel.Controls.Add(this.labelRubberStamp3);
            this.sectionRubberStamps.ContentPanel.Controls.Add(this.rubberStamp2);
            this.sectionRubberStamps.ContentPanel.Controls.Add(this.labelRubberStamp2);
            this.sectionRubberStamps.ContentPanel.Controls.Add(this.rubberStamp1);
            this.sectionRubberStamps.ContentPanel.Controls.Add(this.labelRubberStamp1);
            this.sectionRubberStamps.ExpandedHeight = 198;
            this.sectionRubberStamps.ExtraText = "";
            this.sectionRubberStamps.Location = new System.Drawing.Point(3, 524);
            this.sectionRubberStamps.Name = "sectionRubberStamps";
            this.sectionRubberStamps.SectionName = "Rubber Stamps";
            this.sectionRubberStamps.SettingsKey = "{364567aa-cc02-475f-8261-d980a4b0ccf9}";
            this.sectionRubberStamps.Size = new System.Drawing.Size(375, 24);
            this.sectionRubberStamps.TabIndex = 5;
            // 
            // labelReferenceIdInfo
            // 
            this.labelReferenceIdInfo.AutoSize = true;
            this.labelReferenceIdInfo.BackColor = System.Drawing.Color.White;
            this.labelReferenceIdInfo.ForeColor = System.Drawing.Color.DimGray;
            this.labelReferenceIdInfo.Location = new System.Drawing.Point(98, 146);
            this.labelReferenceIdInfo.Name = "labelReferenceIdInfo";
            this.labelReferenceIdInfo.Size = new System.Drawing.Size(247, 13);
            this.labelReferenceIdInfo.TabIndex = 85;
            this.labelReferenceIdInfo.Text = "This is used to lookup the shipment in Endicia logs.";
            // 
            // referenceID
            // 
            this.referenceID.Location = new System.Drawing.Point(101, 122);
            this.referenceID.MaxLength = 32767;
            this.fieldLengthProvider.SetMaxLengthSource(this.referenceID, ShipWorks.Data.Utility.EntityFieldLengthSource.EndiciaReference);
            this.referenceID.Name = "referenceID";
            this.referenceID.Size = new System.Drawing.Size(236, 21);
            this.referenceID.TabIndex = 3;
            // 
            // labelReferenceID
            // 
            this.labelReferenceID.AutoSize = true;
            this.labelReferenceID.BackColor = System.Drawing.Color.White;
            this.labelReferenceID.Location = new System.Drawing.Point(20, 125);
            this.labelReferenceID.Name = "labelReferenceID";
            this.labelReferenceID.Size = new System.Drawing.Size(75, 13);
            this.labelReferenceID.TabIndex = 83;
            this.labelReferenceID.Text = "Reference ID:";
            // 
            // labelRubberStampWarning
            // 
            this.labelRubberStampWarning.AutoSize = true;
            this.labelRubberStampWarning.BackColor = System.Drawing.Color.White;
            this.labelRubberStampWarning.ForeColor = System.Drawing.Color.DimGray;
            this.labelRubberStampWarning.Location = new System.Drawing.Point(31, 91);
            this.labelRubberStampWarning.Name = "labelRubberStampWarning";
            this.labelRubberStampWarning.Size = new System.Drawing.Size(328, 13);
            this.labelRubberStampWarning.TabIndex = 82;
            this.labelRubberStampWarning.Text = "Express Mail and International labels do not display rubber stamps.";
            // 
            // pictureBoxRubberStampWarning
            // 
            this.pictureBoxRubberStampWarning.BackColor = System.Drawing.Color.White;
            this.pictureBoxRubberStampWarning.Image = global::ShipWorks.Properties.Resources.exclamation16;
            this.pictureBoxRubberStampWarning.Location = new System.Drawing.Point(13, 89);
            this.pictureBoxRubberStampWarning.Name = "pictureBoxRubberStampWarning";
            this.pictureBoxRubberStampWarning.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxRubberStampWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxRubberStampWarning.TabIndex = 81;
            this.pictureBoxRubberStampWarning.TabStop = false;
            // 
            // rubberStamp3
            // 
            this.rubberStamp3.Location = new System.Drawing.Point(101, 63);
            this.rubberStamp3.MaxLength = 32767;
            this.fieldLengthProvider.SetMaxLengthSource(this.rubberStamp3, ShipWorks.Data.Utility.EntityFieldLengthSource.EndiciaRubberStamp);
            this.rubberStamp3.Name = "rubberStamp3";
            this.rubberStamp3.Size = new System.Drawing.Size(236, 21);
            this.rubberStamp3.TabIndex = 2;
            // 
            // labelRubberStamp3
            // 
            this.labelRubberStamp3.AutoSize = true;
            this.labelRubberStamp3.BackColor = System.Drawing.Color.White;
            this.labelRubberStamp3.Location = new System.Drawing.Point(10, 65);
            this.labelRubberStamp3.Name = "labelRubberStamp3";
            this.labelRubberStamp3.Size = new System.Drawing.Size(88, 13);
            this.labelRubberStamp3.TabIndex = 79;
            this.labelRubberStamp3.Text = "Rubber Stamp 3:";
            // 
            // rubberStamp2
            // 
            this.rubberStamp2.Location = new System.Drawing.Point(101, 36);
            this.rubberStamp2.MaxLength = 32767;
            this.fieldLengthProvider.SetMaxLengthSource(this.rubberStamp2, ShipWorks.Data.Utility.EntityFieldLengthSource.EndiciaRubberStamp);
            this.rubberStamp2.Name = "rubberStamp2";
            this.rubberStamp2.Size = new System.Drawing.Size(236, 21);
            this.rubberStamp2.TabIndex = 1;
            // 
            // labelRubberStamp2
            // 
            this.labelRubberStamp2.AutoSize = true;
            this.labelRubberStamp2.BackColor = System.Drawing.Color.White;
            this.labelRubberStamp2.Location = new System.Drawing.Point(10, 38);
            this.labelRubberStamp2.Name = "labelRubberStamp2";
            this.labelRubberStamp2.Size = new System.Drawing.Size(88, 13);
            this.labelRubberStamp2.TabIndex = 77;
            this.labelRubberStamp2.Text = "Rubber Stamp 2:";
            // 
            // rubberStamp1
            // 
            this.rubberStamp1.Location = new System.Drawing.Point(101, 9);
            this.rubberStamp1.MaxLength = 32767;
            this.fieldLengthProvider.SetMaxLengthSource(this.rubberStamp1, ShipWorks.Data.Utility.EntityFieldLengthSource.EndiciaRubberStamp);
            this.rubberStamp1.Name = "rubberStamp1";
            this.rubberStamp1.Size = new System.Drawing.Size(236, 21);
            this.rubberStamp1.TabIndex = 0;
            // 
            // labelRubberStamp1
            // 
            this.labelRubberStamp1.AutoSize = true;
            this.labelRubberStamp1.BackColor = System.Drawing.Color.White;
            this.labelRubberStamp1.Location = new System.Drawing.Point(10, 11);
            this.labelRubberStamp1.Name = "labelRubberStamp1";
            this.labelRubberStamp1.Size = new System.Drawing.Size(88, 13);
            this.labelRubberStamp1.TabIndex = 0;
            this.labelRubberStamp1.Text = "Rubber Stamp 1:";
            // 
            // labelStealth
            // 
            this.labelStealth.AutoSize = true;
            this.labelStealth.BackColor = System.Drawing.Color.White;
            this.labelStealth.Location = new System.Drawing.Point(32, 236);
            this.labelStealth.Name = "labelStealth";
            this.labelStealth.Size = new System.Drawing.Size(45, 13);
            this.labelStealth.TabIndex = 55;
            this.labelStealth.Text = "Stealth:";
            // 
            // hidePostage
            // 
            this.hidePostage.AutoSize = true;
            this.hidePostage.BackColor = System.Drawing.Color.White;
            this.hidePostage.Location = new System.Drawing.Point(83, 235);
            this.hidePostage.Name = "hidePostage";
            this.hidePostage.Size = new System.Drawing.Size(89, 17);
            this.hidePostage.TabIndex = 56;
            this.hidePostage.Text = "Hide Postage";
            this.hidePostage.UseVisualStyleBackColor = false;
            // 
            // noPostage
            // 
            this.noPostage.AutoSize = true;
            this.noPostage.BackColor = System.Drawing.Color.White;
            this.noPostage.Location = new System.Drawing.Point(83, 258);
            this.noPostage.Name = "noPostage";
            this.noPostage.Size = new System.Drawing.Size(214, 17);
            this.noPostage.TabIndex = 58;
            this.noPostage.Text = "Generate label that is not postage-paid";
            this.noPostage.UseVisualStyleBackColor = false;
            // 
            // labelNoPostage
            // 
            this.labelNoPostage.AutoSize = true;
            this.labelNoPostage.BackColor = System.Drawing.Color.White;
            this.labelNoPostage.Location = new System.Drawing.Point(10, 259);
            this.labelNoPostage.Name = "labelNoPostage";
            this.labelNoPostage.Size = new System.Drawing.Size(66, 13);
            this.labelNoPostage.TabIndex = 57;
            this.labelNoPostage.Text = "No Postage:";
            // 
            // sectionParcelSelect
            // 
            this.sectionEntryFacility.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // sectionParcelSelect.ContentPanel
            // 
            this.sectionEntryFacility.ContentPanel.Controls.Add(this.entryFacility);
            this.sectionEntryFacility.ContentPanel.Controls.Add(this.sortType);
            this.sectionEntryFacility.ContentPanel.Controls.Add(this.labelEntryFacility);
            this.sectionEntryFacility.ContentPanel.Controls.Add(this.labelSortType);
            this.sectionEntryFacility.ExtraText = "";
            this.sectionEntryFacility.Location = new System.Drawing.Point(3, 582);
            this.sectionEntryFacility.Name = "sectionEntryFacility";
            this.sectionEntryFacility.SectionName = "Entry Facility";
            this.sectionEntryFacility.SettingsKey = "{d3354c1d-46db-4293-b79f-e9e88d1cca7b}";
            this.sectionEntryFacility.Size = new System.Drawing.Size(375, 95);
            this.sectionEntryFacility.TabIndex = 13;
            // 
            // labelEntryFacility
            // 
            this.labelEntryFacility.AutoSize = true;
            this.labelEntryFacility.BackColor = System.Drawing.Color.Transparent;
            this.labelEntryFacility.Location = new System.Drawing.Point(8, 39);
            this.labelEntryFacility.Name = "labelEntryFacility";
            this.labelEntryFacility.Size = new System.Drawing.Size(73, 13);
            this.labelEntryFacility.TabIndex = 25;
            this.labelEntryFacility.Text = "Entry Facility:";
            // 
            // labelSortType
            // 
            this.labelSortType.AutoSize = true;
            this.labelSortType.BackColor = System.Drawing.Color.Transparent;
            this.labelSortType.Location = new System.Drawing.Point(22, 12);
            this.labelSortType.Name = "labelSortType";
            this.labelSortType.Size = new System.Drawing.Size(58, 13);
            this.labelSortType.TabIndex = 23;
            this.labelSortType.Text = "Sort Type:";
            // 
            // entryFacility
            // 
            this.entryFacility.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.entryFacility.FormattingEnabled = true;
            this.entryFacility.Location = new System.Drawing.Point(83, 36);
            this.entryFacility.Name = "entryFacility";
            this.entryFacility.PromptText = "(Multiple Values)";
            this.entryFacility.Size = new System.Drawing.Size(167, 21);
            this.entryFacility.TabIndex = 101;
            this.entryFacility.SelectedValueChanged += OnEntryFacilityDataChanged;
            // 
            // sortType
            // 
            this.sortType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sortType.FormattingEnabled = true;
            this.sortType.Location = new System.Drawing.Point(83, 9);
            this.sortType.Name = "sortType";
            this.sortType.PromptText = "(Multiple Values)";
            this.sortType.Size = new System.Drawing.Size(167, 21);
            this.sortType.TabIndex = 100;
            this.sortType.SelectedValueChanged += OnEntryFacilityDataChanged;
            // 
            // EndiciaServiceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.sectionFrom);
            this.Controls.Add(this.sectionRubberStamps);
            this.Controls.Add(this.sectionEntryFacility);
            this.Name = "EndiciaServiceControl";
            this.Size = new System.Drawing.Size(381, 1019);
            this.Controls.SetChildIndex(this.sectionReturns, 0);
            this.Controls.SetChildIndex(this.sectionEntryFacility, 0);
            this.Controls.SetChildIndex(this.sectionExpress, 0);
            this.Controls.SetChildIndex(this.sectionRubberStamps, 0);
            this.Controls.SetChildIndex(this.sectionShipment, 0);
            this.Controls.SetChildIndex(this.sectionRecipient, 0);
            this.Controls.SetChildIndex(this.sectionFrom, 0);
            ((System.ComponentModel.ISupportInitialize) (this.sectionExpress)).EndInit();
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
            this.sectionRubberStamps.ContentPanel.ResumeLayout(false);
            this.sectionRubberStamps.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionRubberStamps)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxRubberStampWarning)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.sectionEntryFacility.ContentPanel.ResumeLayout(false);
            this.sectionEntryFacility.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionEntryFacility)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionFrom;
        private ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl originControl;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private ShipWorks.UI.Controls.MultiValueComboBox endiciaAccount;
        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionRubberStamps;
        private System.Windows.Forms.CheckBox noPostage;
        private System.Windows.Forms.Label labelNoPostage;
        private System.Windows.Forms.CheckBox hidePostage;
        private System.Windows.Forms.Label labelStealth;
        private System.Windows.Forms.Label labelRubberStamp1;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox rubberStamp2;
        private System.Windows.Forms.Label labelRubberStamp2;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox rubberStamp1;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox rubberStamp3;
        private System.Windows.Forms.Label labelRubberStamp3;
        private System.Windows.Forms.Label labelRubberStampWarning;
        private System.Windows.Forms.PictureBox pictureBoxRubberStampWarning;
        private System.Windows.Forms.Label labelReferenceIdInfo;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox referenceID;
        private System.Windows.Forms.Label labelReferenceID;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private UI.Controls.CollapsibleGroupControl sectionEntryFacility;
        private System.Windows.Forms.Label labelEntryFacility;
        private System.Windows.Forms.Label labelSortType;
        private UI.Controls.MultiValueComboBox entryFacility;
        private UI.Controls.MultiValueComboBox sortType;
    }
}
