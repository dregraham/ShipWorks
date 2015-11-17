using System.Drawing;
using System.Windows.Forms;
using ShipWorks.Core.Messaging;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    partial class FedExCustomsControl
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
                fedExServiceChangedToken?.Dispose();

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
            this.labelBrokerAccountHeading = new System.Windows.Forms.Label();
            this.brokerAccount = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelBrokerAccount = new System.Windows.Forms.Label();
            this.brokerEnabled = new System.Windows.Forms.CheckBox();
            this.brokerControl = new ShipWorks.Data.Controls.PersonControl();
            this.sectionBroker = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.admissibilityPackaging = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelAdmissibilityPackaging = new System.Windows.Forms.Label();
            this.documentsOnly = new System.Windows.Forms.CheckBox();
            this.labelDocuments = new System.Windows.Forms.Label();
            this.labelRecipientTaxID = new System.Windows.Forms.Label();
            this.recipientTaxID = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.sectionCommercialInvoice = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.commercialInvoiceReference = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.commercialInvoiceReferenceLabel = new System.Windows.Forms.Label();
            this.iorFedExAccount = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelIorTaxID = new System.Windows.Forms.Label();
            this.iorTaxID = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelIorFedexAccount = new System.Windows.Forms.Label();
            this.labelIorAccount = new System.Windows.Forms.Label();
            this.iorPersonControl = new ShipWorks.Data.Controls.PersonControl();
            this.importerOfRecord = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ciAdditional = new ShipWorks.UI.Controls.MoneyTextBox();
            this.ciInsurance = new ShipWorks.UI.Controls.MoneyTextBox();
            this.ciFreight = new ShipWorks.UI.Controls.MoneyTextBox();
            this.ciPurpose = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.ciTermsOfSale = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelPurpose = new System.Windows.Forms.Label();
            this.ciComments = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelComments = new System.Windows.Forms.Label();
            this.labelTermsOfSale = new System.Windows.Forms.Label();
            this.commercialInvoice = new System.Windows.Forms.CheckBox();
            this.electronicTradeDocuments = new CheckBox();
            this.filingOptionLabel = new System.Windows.Forms.Label();
            this.filingOption = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.electronicExportInfoLabel = new System.Windows.Forms.Label();
            this.sectionNafta = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.naftaProducerId = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelNaftaProducerId = new System.Windows.Forms.Label();
            this.netCostMethodLabel = new System.Windows.Forms.Label();
            this.producerDeterminationLabel = new System.Windows.Forms.Label();
            this.preferenceLabel = new System.Windows.Forms.Label();
            this.naftaEnabled = new System.Windows.Forms.CheckBox();
            this.naftaPreference = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.naftaProducerDetermination = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.naftaNetCostMethod = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.electronicExportInfo = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelNumberOfPieces = new System.Windows.Forms.Label();
            this.numberOfPieces = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.unitPrice = new ShipWorks.UI.Controls.MoneyTextBox();
            this.labelUnitPrice = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.sectionContents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionContents.ContentPanel)).BeginInit();
            this.sectionContents.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionGeneral)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionGeneral.ContentPanel)).BeginInit();
            this.sectionGeneral.ContentPanel.SuspendLayout();
            this.groupSelectedContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionBroker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionBroker.ContentPanel)).BeginInit();
            this.sectionBroker.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionCommercialInvoice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionCommercialInvoice.ContentPanel)).BeginInit();
            this.sectionCommercialInvoice.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionNafta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionNafta.ContentPanel)).BeginInit();
            this.sectionNafta.ContentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // sectionContents
            // 
            this.sectionContents.Location = new System.Drawing.Point(6, 222);
            this.sectionContents.Size = new System.Drawing.Size(438, 420);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(84, 11);
            this.label1.TabIndex = 0;
            // 
            // customsValue
            // 
            this.customsValue.Location = new System.Drawing.Point(128, 8);
            this.customsValue.TabIndex = 1;
            // 
            // sectionGeneral
            // 
            // 
            // sectionGeneral.ContentPanel
            // 
            this.sectionGeneral.ContentPanel.Controls.Add(this.electronicExportInfo);
            this.sectionGeneral.ContentPanel.Controls.Add(this.recipientTaxID);
            this.sectionGeneral.ContentPanel.Controls.Add(this.filingOption);
            this.sectionGeneral.ContentPanel.Controls.Add(this.electronicExportInfoLabel);
            this.sectionGeneral.ContentPanel.Controls.Add(this.labelRecipientTaxID);
            this.sectionGeneral.ContentPanel.Controls.Add(this.filingOptionLabel);
            this.sectionGeneral.ContentPanel.Controls.Add(this.labelDocuments);
            this.sectionGeneral.ContentPanel.Controls.Add(this.documentsOnly);
            this.sectionGeneral.ContentPanel.Controls.Add(this.admissibilityPackaging);
            this.sectionGeneral.ContentPanel.Controls.Add(this.labelAdmissibilityPackaging);
            this.sectionGeneral.Location = new System.Drawing.Point(6, 5);
            this.sectionGeneral.Size = new System.Drawing.Size(441, 212);
            // 
            // groupSelectedContent
            // 
            this.groupSelectedContent.Controls.Add(this.unitPrice);
            this.groupSelectedContent.Controls.Add(this.labelUnitPrice);
            this.groupSelectedContent.Controls.Add(this.numberOfPieces);
            this.groupSelectedContent.Controls.Add(this.labelNumberOfPieces);
            this.groupSelectedContent.Size = new System.Drawing.Size(418, 244);
            this.groupSelectedContent.Controls.SetChildIndex(this.labelNumberOfPieces, 0);
            this.groupSelectedContent.Controls.SetChildIndex(this.numberOfPieces, 0);
            this.groupSelectedContent.Controls.SetChildIndex(this.labelUnitPrice, 0);
            this.groupSelectedContent.Controls.SetChildIndex(this.unitPrice, 0);
            // 
            // add
            // 
            this.add.Location = new System.Drawing.Point(362, 10);
            // 
            // delete
            // 
            this.delete.Location = new System.Drawing.Point(362, 37);
            // 
            // sandGrid
            // 
            this.itemsGrid.Size = new System.Drawing.Size(344, 116);
            // 
            // labelBrokerAccountHeading
            // 
            this.labelBrokerAccountHeading.AutoSize = true;
            this.labelBrokerAccountHeading.BackColor = System.Drawing.Color.Transparent;
            this.labelBrokerAccountHeading.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBrokerAccountHeading.Location = new System.Drawing.Point(27, 31);
            this.labelBrokerAccountHeading.Name = "labelBrokerAccountHeading";
            this.labelBrokerAccountHeading.Size = new System.Drawing.Size(53, 13);
            this.labelBrokerAccountHeading.TabIndex = 0;
            this.labelBrokerAccountHeading.Text = "Account";
            // 
            // brokerAccount
            // 
            this.brokerAccount.Location = new System.Drawing.Point(102, 49);
            this.fieldLengthProvider.SetMaxLengthSource(this.brokerAccount, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExAccountNumber);
            this.brokerAccount.Name = "brokerAccount";
            this.brokerAccount.Size = new System.Drawing.Size(250, 21);
            this.brokerAccount.TabIndex = 2;
            // 
            // labelBrokerAccount
            // 
            this.labelBrokerAccount.AutoSize = true;
            this.labelBrokerAccount.BackColor = System.Drawing.Color.Transparent;
            this.labelBrokerAccount.Location = new System.Drawing.Point(35, 52);
            this.labelBrokerAccount.Name = "labelBrokerAccount";
            this.labelBrokerAccount.Size = new System.Drawing.Size(61, 13);
            this.labelBrokerAccount.TabIndex = 1;
            this.labelBrokerAccount.Text = "Account #:";
            // 
            // brokerEnabled
            // 
            this.brokerEnabled.AutoSize = true;
            this.brokerEnabled.BackColor = System.Drawing.Color.White;
            this.brokerEnabled.Location = new System.Drawing.Point(11, 8);
            this.brokerEnabled.Name = "brokerEnabled";
            this.brokerEnabled.Size = new System.Drawing.Size(142, 17);
            this.brokerEnabled.TabIndex = 45;
            this.brokerEnabled.Text = "Use the following broker";
            this.brokerEnabled.UseVisualStyleBackColor = false;
            // 
            // brokerControl
            // 
            this.brokerControl.AvailableFields = ((ShipWorks.Data.Controls.PersonFields)(((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Country) 
            | ShipWorks.Data.Controls.PersonFields.Email) 
            | ShipWorks.Data.Controls.PersonFields.Phone)));
            this.brokerControl.BackColor = System.Drawing.Color.White;
            this.brokerControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.brokerControl.Location = new System.Drawing.Point(26, 69);
            this.brokerControl.Name = "brokerControl";
            this.brokerControl.Size = new System.Drawing.Size(337, 340);
            this.brokerControl.TabIndex = 3;
            // 
            // sectionBroker
            // 
            this.sectionBroker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionBroker.Collapsed = true;
            // 
            // sectionBroker.ContentPanel
            // 
            this.sectionBroker.ContentPanel.Controls.Add(this.brokerAccount);
            this.sectionBroker.ContentPanel.Controls.Add(this.brokerEnabled);
            this.sectionBroker.ContentPanel.Controls.Add(this.labelBrokerAccountHeading);
            this.sectionBroker.ContentPanel.Controls.Add(this.brokerControl);
            this.sectionBroker.ContentPanel.Controls.Add(this.labelBrokerAccount);
            this.sectionBroker.ExpandedHeight = 438;
            this.sectionBroker.ExtraText = "";
            this.sectionBroker.Location = new System.Drawing.Point(6, 851);
            this.sectionBroker.Name = "sectionBroker";
            this.sectionBroker.SectionName = "International Broker";
            this.sectionBroker.SettingsKey = "{4da9daec-f847-4177-b9fb-0f9bb5f1cdcc}";
            this.sectionBroker.Size = new System.Drawing.Size(438, 24);
            this.sectionBroker.TabIndex = 3;
            // 
            // admissibilityPackaging
            // 
            this.admissibilityPackaging.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.admissibilityPackaging.FormattingEnabled = true;
            this.admissibilityPackaging.Location = new System.Drawing.Point(128, 141);
            this.admissibilityPackaging.Name = "admissibilityPackaging";
            this.admissibilityPackaging.PromptText = "(Multiple Values)";
            this.admissibilityPackaging.Size = new System.Drawing.Size(145, 21);
            this.admissibilityPackaging.TabIndex = 9;
            // 
            // labelAdmissibilityPackaging
            // 
            this.labelAdmissibilityPackaging.AutoSize = true;
            this.labelAdmissibilityPackaging.BackColor = System.Drawing.Color.Transparent;
            this.labelAdmissibilityPackaging.Location = new System.Drawing.Point(3, 144);
            this.labelAdmissibilityPackaging.Name = "labelAdmissibilityPackaging";
            this.labelAdmissibilityPackaging.Size = new System.Drawing.Size(119, 13);
            this.labelAdmissibilityPackaging.TabIndex = 8;
            this.labelAdmissibilityPackaging.Text = "Admissibility Packaging:";
            // 
            // documentsOnly
            // 
            this.documentsOnly.AutoSize = true;
            this.documentsOnly.BackColor = System.Drawing.Color.White;
            this.documentsOnly.Location = new System.Drawing.Point(128, 35);
            this.documentsOnly.Name = "documentsOnly";
            this.documentsOnly.Size = new System.Drawing.Size(102, 17);
            this.documentsOnly.TabIndex = 3;
            this.documentsOnly.Text = "Documents only";
            this.documentsOnly.UseVisualStyleBackColor = false;
            // 
            // labelDocuments
            // 
            this.labelDocuments.AutoSize = true;
            this.labelDocuments.BackColor = System.Drawing.Color.White;
            this.labelDocuments.Location = new System.Drawing.Point(67, 36);
            this.labelDocuments.Name = "labelDocuments";
            this.labelDocuments.Size = new System.Drawing.Size(55, 13);
            this.labelDocuments.TabIndex = 2;
            this.labelDocuments.Text = "Contents:";
            // 
            // labelRecipientTaxID
            // 
            this.labelRecipientTaxID.AutoSize = true;
            this.labelRecipientTaxID.BackColor = System.Drawing.Color.White;
            this.labelRecipientTaxID.Location = new System.Drawing.Point(33, 60);
            this.labelRecipientTaxID.Name = "labelRecipientTaxID";
            this.labelRecipientTaxID.Size = new System.Drawing.Size(88, 13);
            this.labelRecipientTaxID.TabIndex = 6;
            this.labelRecipientTaxID.Text = "Recipient tax ID:";
            // 
            // recipientTaxID
            // 
            this.recipientTaxID.Location = new System.Drawing.Point(128, 57);
            this.fieldLengthProvider.SetMaxLengthSource(this.recipientTaxID, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExCustomsTin);
            this.recipientTaxID.Name = "recipientTaxID";
            this.recipientTaxID.Size = new System.Drawing.Size(145, 21);
            this.recipientTaxID.TabIndex = 7;
            // 
            // sectionCommercialInvoice
            // 
            this.sectionCommercialInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionCommercialInvoice.Collapsed = true;
            // 
            // sectionCommercialInvoice.ContentPanel
            // 
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.commercialInvoiceReference);
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.commercialInvoiceReferenceLabel);
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.iorFedExAccount);
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.labelIorTaxID);
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.iorTaxID);
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.labelIorFedexAccount);
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.labelIorAccount);
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.iorPersonControl);
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.importerOfRecord);
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.label7);
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.label6);
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.label5);
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.ciAdditional);
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.ciInsurance);
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.ciFreight);
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.ciPurpose);
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.ciTermsOfSale);
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.labelPurpose);
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.ciComments);
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.labelComments);
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.labelTermsOfSale);
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.electronicTradeDocuments);
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.commercialInvoice);
            this.sectionCommercialInvoice.ExpandedHeight = 683;
            this.sectionCommercialInvoice.ExtraText = "";
            this.sectionCommercialInvoice.Location = new System.Drawing.Point(6, 822);
            this.sectionCommercialInvoice.Name = "sectionCommercialInvoice";
            this.sectionCommercialInvoice.SectionName = "Commercial Invoice";
            this.sectionCommercialInvoice.SettingsKey = "{de95083a-38da-4848-8967-9078ca062e50}";
            this.sectionCommercialInvoice.Size = new System.Drawing.Size(438, 24);
            this.sectionCommercialInvoice.TabIndex = 3;
            // 
            // commercialInvoiceReference
            // 
            this.commercialInvoiceReference.Location = new System.Drawing.Point(115, 220);
            this.fieldLengthProvider.SetMaxLengthSource(this.commercialInvoiceReference, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExCommercialInvoiceReference);
            this.commercialInvoiceReference.Name = "commercialInvoiceReference";
            this.commercialInvoiceReference.Size = new System.Drawing.Size(161, 21);
            this.commercialInvoiceReference.TabIndex = 13;
            // 
            // commercialInvoiceReferenceLabel
            // 
            this.commercialInvoiceReferenceLabel.AutoSize = true;
            this.commercialInvoiceReferenceLabel.BackColor = System.Drawing.Color.White;
            this.commercialInvoiceReferenceLabel.Location = new System.Drawing.Point(35, 223);
            this.commercialInvoiceReferenceLabel.Name = "commercialInvoiceReferenceLabel";
            this.commercialInvoiceReferenceLabel.Size = new System.Drawing.Size(72, 13);
            this.commercialInvoiceReferenceLabel.TabIndex = 20;
            this.commercialInvoiceReferenceLabel.Text = "Reference #:";
            this.commercialInvoiceReferenceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // iorFedExAccount
            // 
            this.iorFedExAccount.Location = new System.Drawing.Point(129, 326);
            this.fieldLengthProvider.SetMaxLengthSource(this.iorFedExAccount, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExAccountNumber);
            this.iorFedExAccount.Name = "iorFedExAccount";
            this.iorFedExAccount.Size = new System.Drawing.Size(161, 21);
            this.iorFedExAccount.TabIndex = 19;
            // 
            // labelIorTaxID
            // 
            this.labelIorTaxID.AutoSize = true;
            this.labelIorTaxID.BackColor = System.Drawing.Color.White;
            this.labelIorTaxID.Location = new System.Drawing.Point(80, 301);
            this.labelIorTaxID.Name = "labelIorTaxID";
            this.labelIorTaxID.Size = new System.Drawing.Size(43, 13);
            this.labelIorTaxID.TabIndex = 16;
            this.labelIorTaxID.Text = "Tax ID:";
            // 
            // iorTaxID
            // 
            this.iorTaxID.Location = new System.Drawing.Point(129, 298);
            this.fieldLengthProvider.SetMaxLengthSource(this.iorTaxID, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExCustomsTin);
            this.iorTaxID.Name = "iorTaxID";
            this.iorTaxID.Size = new System.Drawing.Size(161, 21);
            this.iorTaxID.TabIndex = 17;
            // 
            // labelIorFedexAccount
            // 
            this.labelIorFedexAccount.AutoSize = true;
            this.labelIorFedexAccount.BackColor = System.Drawing.Color.White;
            this.labelIorFedexAccount.Location = new System.Drawing.Point(41, 328);
            this.labelIorFedexAccount.Name = "labelIorFedexAccount";
            this.labelIorFedexAccount.Size = new System.Drawing.Size(83, 13);
            this.labelIorFedexAccount.TabIndex = 18;
            this.labelIorFedexAccount.Text = "FedEx Account:";
            // 
            // labelIorAccount
            // 
            this.labelIorAccount.AutoSize = true;
            this.labelIorAccount.BackColor = System.Drawing.Color.White;
            this.labelIorAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIorAccount.Location = new System.Drawing.Point(57, 281);
            this.labelIorAccount.Name = "labelIorAccount";
            this.labelIorAccount.Size = new System.Drawing.Size(53, 13);
            this.labelIorAccount.TabIndex = 15;
            this.labelIorAccount.Text = "Account";
            // 
            // iorPersonControl
            // 
            this.iorPersonControl.AvailableFields = ((ShipWorks.Data.Controls.PersonFields)((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Country) 
            | ShipWorks.Data.Controls.PersonFields.Phone)));
            this.iorPersonControl.BackColor = System.Drawing.Color.White;
            this.iorPersonControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iorPersonControl.Location = new System.Drawing.Point(54, 346);
            this.iorPersonControl.Name = "iorPersonControl";
            this.iorPersonControl.Size = new System.Drawing.Size(358, 308);
            this.iorPersonControl.TabIndex = 20;
            // 
            // importerOfRecord
            // 
            this.importerOfRecord.AutoSize = true;
            this.importerOfRecord.BackColor = System.Drawing.Color.White;
            this.importerOfRecord.Location = new System.Drawing.Point(36, 257);
            this.importerOfRecord.Name = "importerOfRecord";
            this.importerOfRecord.Size = new System.Drawing.Size(245, 17);
            this.importerOfRecord.TabIndex = 14;
            this.importerOfRecord.Text = "Recipient is not the Importer of Record\\Buyer";
            this.importerOfRecord.UseVisualStyleBackColor = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(23, 169);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Insurance costs:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(24, 170);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Additional costs:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(37, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Freight costs:";
            // 
            // ciAdditional
            // 
            this.ciAdditional.Amount = new decimal(new int[] {
            0,
            0,
            0,
            65536});
            this.ciAdditional.Location = new System.Drawing.Point(116, 193);
            this.ciAdditional.Name = "ciAdditional";
            this.ciAdditional.Size = new System.Drawing.Size(95, 21);
            this.ciAdditional.TabIndex = 12;
            this.ciAdditional.Text = "$0.00";
            // 
            // ciInsurance
            // 
            this.ciInsurance.Amount = new decimal(new int[] {
            0,
            0,
            0,
            65536});
            this.ciInsurance.Location = new System.Drawing.Point(116, 166);
            this.ciInsurance.Name = "ciInsurance";
            this.ciInsurance.Size = new System.Drawing.Size(95, 21);
            this.ciInsurance.TabIndex = 10;
            this.ciInsurance.Text = "$0.00";
            // 
            // ciFreight
            // 
            this.ciFreight.Amount = new decimal(new int[] {
            0,
            0,
            0,
            65536});
            this.ciFreight.Location = new System.Drawing.Point(116, 142);
            this.ciFreight.Name = "ciFreight";
            this.ciFreight.Size = new System.Drawing.Size(95, 21);
            this.ciFreight.TabIndex = 8;
            this.ciFreight.Text = "$0.00";
            // 
            // ciPurpose
            // 
            this.ciPurpose.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ciPurpose.FormattingEnabled = true;
            this.ciPurpose.Location = new System.Drawing.Point(115, 86);
            this.ciPurpose.Name = "ciPurpose";
            this.ciPurpose.PromptText = "(Multiple Values)";
            this.ciPurpose.Size = new System.Drawing.Size(161, 21);
            this.ciPurpose.TabIndex = 4;
            // 
            // ciTermsOfSale
            // 
            this.ciTermsOfSale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ciTermsOfSale.FormattingEnabled = true;
            this.ciTermsOfSale.Location = new System.Drawing.Point(115, 61);
            this.ciTermsOfSale.Name = "ciTermsOfSale";
            this.ciTermsOfSale.PromptText = "(Multiple Values)";
            this.ciTermsOfSale.Size = new System.Drawing.Size(161, 21);
            this.ciTermsOfSale.TabIndex = 1;
            // 
            // labelPurpose
            // 
            this.labelPurpose.AutoSize = true;
            this.labelPurpose.BackColor = System.Drawing.Color.White;
            this.labelPurpose.Location = new System.Drawing.Point(60, 91);
            this.labelPurpose.Name = "labelPurpose";
            this.labelPurpose.Size = new System.Drawing.Size(50, 13);
            this.labelPurpose.TabIndex = 3;
            this.labelPurpose.Text = "Purpose:";
            // 
            // ciComments
            // 
            this.ciComments.Location = new System.Drawing.Point(115, 115);
            this.fieldLengthProvider.SetMaxLengthSource(this.ciComments, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExCommercialInvoiceComments);
            this.ciComments.Name = "ciComments";
            this.ciComments.Size = new System.Drawing.Size(161, 21);
            this.ciComments.TabIndex = 6;
            // 
            // labelComments
            // 
            this.labelComments.AutoSize = true;
            this.labelComments.BackColor = System.Drawing.Color.White;
            this.labelComments.Location = new System.Drawing.Point(49, 118);
            this.labelComments.Name = "labelComments";
            this.labelComments.Size = new System.Drawing.Size(61, 13);
            this.labelComments.TabIndex = 5;
            this.labelComments.Text = "Comments:";
            // 
            // labelTermsOfSale
            // 
            this.labelTermsOfSale.AutoSize = true;
            this.labelTermsOfSale.BackColor = System.Drawing.Color.White;
            this.labelTermsOfSale.Location = new System.Drawing.Point(38, 64);
            this.labelTermsOfSale.Name = "labelTermsOfSale";
            this.labelTermsOfSale.Size = new System.Drawing.Size(75, 13);
            this.labelTermsOfSale.TabIndex = 0;
            this.labelTermsOfSale.Text = "Terms of sale:";
            // 
            // electronicTradeDocuments
            // 
            this.electronicTradeDocuments.AutoSize = true;
            this.electronicTradeDocuments.BackColor = System.Drawing.Color.White;
            this.electronicTradeDocuments.Location = new System.Drawing.Point(36, 38);
            this.electronicTradeDocuments.Name = "electronicTradeDocuments";
            this.electronicTradeDocuments.Size = new System.Drawing.Size(154, 17);
            this.electronicTradeDocuments.TabIndex = 0;
            this.electronicTradeDocuments.Text = "File electronically";
            this.electronicTradeDocuments.UseVisualStyleBackColor = false;
            // 
            // commercialInvoice
            // 
            this.commercialInvoice.AutoSize = true;
            this.commercialInvoice.BackColor = System.Drawing.Color.White;
            this.commercialInvoice.Location = new System.Drawing.Point(8, 12);
            this.commercialInvoice.Name = "commercialInvoice";
            this.commercialInvoice.Size = new System.Drawing.Size(154, 17);
            this.commercialInvoice.TabIndex = 0;
            this.commercialInvoice.Text = "Create Commercial Invoice";
            this.commercialInvoice.UseVisualStyleBackColor = false;
            // 
            // filingOptionLabel
            // 
            this.filingOptionLabel.AutoSize = true;
            this.filingOptionLabel.BackColor = System.Drawing.Color.White;
            this.filingOptionLabel.Location = new System.Drawing.Point(24, 87);
            this.filingOptionLabel.Name = "filingOptionLabel";
            this.filingOptionLabel.Size = new System.Drawing.Size(98, 13);
            this.filingOptionLabel.TabIndex = 10;
            this.filingOptionLabel.Text = "B13A Filing Option:";
            this.filingOptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // filingOption
            // 
            this.filingOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filingOption.FormattingEnabled = true;
            this.filingOption.Location = new System.Drawing.Point(129, 84);
            this.filingOption.Name = "filingOption";
            this.filingOption.PromptText = "(Multiple Values)";
            this.filingOption.Size = new System.Drawing.Size(145, 21);
            this.filingOption.TabIndex = 11;
            // 
            // electronicExportInfoLabel
            // 
            this.electronicExportInfoLabel.AutoSize = true;
            this.electronicExportInfoLabel.BackColor = System.Drawing.Color.White;
            this.electronicExportInfoLabel.Location = new System.Drawing.Point(19, 116);
            this.electronicExportInfoLabel.Name = "electronicExportInfoLabel";
            this.electronicExportInfoLabel.Size = new System.Drawing.Size(103, 13);
            this.electronicExportInfoLabel.TabIndex = 12;
            this.electronicExportInfoLabel.Text = "AES/EEI Exemption:";
            this.electronicExportInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sectionNafta
            // 
            this.sectionNafta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // sectionNafta.ContentPanel
            // 
            this.sectionNafta.ContentPanel.Controls.Add(this.naftaProducerId);
            this.sectionNafta.ContentPanel.Controls.Add(this.labelNaftaProducerId);
            this.sectionNafta.ContentPanel.Controls.Add(this.netCostMethodLabel);
            this.sectionNafta.ContentPanel.Controls.Add(this.producerDeterminationLabel);
            this.sectionNafta.ContentPanel.Controls.Add(this.preferenceLabel);
            this.sectionNafta.ContentPanel.Controls.Add(this.naftaEnabled);
            this.sectionNafta.ContentPanel.Controls.Add(this.naftaPreference);
            this.sectionNafta.ContentPanel.Controls.Add(this.naftaProducerDetermination);
            this.sectionNafta.ContentPanel.Controls.Add(this.naftaNetCostMethod);
            this.sectionNafta.ExtraText = "";
            this.sectionNafta.Location = new System.Drawing.Point(6, 647);
            this.sectionNafta.Name = "sectionNafta";
            this.sectionNafta.SectionName = "NAFTA";
            this.sectionNafta.SettingsKey = "{214e751c-4c94-4be8-bb4a-5421289a47a2}";
            this.sectionNafta.Size = new System.Drawing.Size(438, 170);
            this.sectionNafta.TabIndex = 4;
            // 
            // naftaProducerId
            // 
            this.naftaProducerId.Location = new System.Drawing.Point(156, 85);
            this.fieldLengthProvider.SetMaxLengthSource(this.naftaProducerId, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExCustomsTin);
            this.naftaProducerId.Name = "naftaProducerId";
            this.naftaProducerId.Size = new System.Drawing.Size(145, 21);
            this.naftaProducerId.TabIndex = 15;
            // 
            // labelNaftaProducerId
            // 
            this.labelNaftaProducerId.AutoSize = true;
            this.labelNaftaProducerId.BackColor = System.Drawing.Color.White;
            this.labelNaftaProducerId.Location = new System.Drawing.Point(50, 88);
            this.labelNaftaProducerId.Name = "labelNaftaProducerId";
            this.labelNaftaProducerId.Size = new System.Drawing.Size(98, 13);
            this.labelNaftaProducerId.TabIndex = 14;
            this.labelNaftaProducerId.Text = "Nafta Producer ID:";
            // 
            // netCostMethodLabel
            // 
            this.netCostMethodLabel.AutoSize = true;
            this.netCostMethodLabel.BackColor = System.Drawing.Color.White;
            this.netCostMethodLabel.Location = new System.Drawing.Point(56, 115);
            this.netCostMethodLabel.Name = "netCostMethodLabel";
            this.netCostMethodLabel.Size = new System.Drawing.Size(92, 13);
            this.netCostMethodLabel.TabIndex = 5;
            this.netCostMethodLabel.Text = "Net Cost Method:";
            this.netCostMethodLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // producerDeterminationLabel
            // 
            this.producerDeterminationLabel.AutoSize = true;
            this.producerDeterminationLabel.BackColor = System.Drawing.Color.White;
            this.producerDeterminationLabel.Location = new System.Drawing.Point(24, 61);
            this.producerDeterminationLabel.Name = "producerDeterminationLabel";
            this.producerDeterminationLabel.Size = new System.Drawing.Size(124, 13);
            this.producerDeterminationLabel.TabIndex = 3;
            this.producerDeterminationLabel.Text = "Producer Determination:";
            this.producerDeterminationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // preferenceLabel
            // 
            this.preferenceLabel.AutoSize = true;
            this.preferenceLabel.BackColor = System.Drawing.Color.White;
            this.preferenceLabel.Location = new System.Drawing.Point(84, 34);
            this.preferenceLabel.Name = "preferenceLabel";
            this.preferenceLabel.Size = new System.Drawing.Size(64, 13);
            this.preferenceLabel.TabIndex = 1;
            this.preferenceLabel.Text = "Preference:";
            this.preferenceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // naftaEnabled
            // 
            this.naftaEnabled.AutoSize = true;
            this.naftaEnabled.BackColor = System.Drawing.Color.White;
            this.naftaEnabled.Location = new System.Drawing.Point(14, 8);
            this.naftaEnabled.Name = "naftaEnabled";
            this.naftaEnabled.Size = new System.Drawing.Size(237, 17);
            this.naftaEnabled.TabIndex = 0;
            this.naftaEnabled.Text = "Include NAFTA information for this shipment";
            this.naftaEnabled.UseVisualStyleBackColor = false;
            // 
            // naftaPreference
            // 
            this.naftaPreference.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.naftaPreference.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.naftaPreference.FormattingEnabled = true;
            this.naftaPreference.Location = new System.Drawing.Point(156, 31);
            this.naftaPreference.Name = "naftaPreference";
            this.naftaPreference.PromptText = "(Multiple Values)";
            this.naftaPreference.Size = new System.Drawing.Size(243, 21);
            this.naftaPreference.TabIndex = 2;
            // 
            // naftaProducerDetermination
            // 
            this.naftaProducerDetermination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.naftaProducerDetermination.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.naftaProducerDetermination.FormattingEnabled = true;
            this.naftaProducerDetermination.Location = new System.Drawing.Point(156, 58);
            this.naftaProducerDetermination.Name = "naftaProducerDetermination";
            this.naftaProducerDetermination.PromptText = "(Multiple Values)";
            this.naftaProducerDetermination.Size = new System.Drawing.Size(243, 21);
            this.naftaProducerDetermination.TabIndex = 4;
            // 
            // naftaNetCostMethod
            // 
            this.naftaNetCostMethod.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.naftaNetCostMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.naftaNetCostMethod.FormattingEnabled = true;
            this.naftaNetCostMethod.Location = new System.Drawing.Point(156, 112);
            this.naftaNetCostMethod.Name = "naftaNetCostMethod";
            this.naftaNetCostMethod.PromptText = "(Multiple Values)";
            this.naftaNetCostMethod.Size = new System.Drawing.Size(243, 21);
            this.naftaNetCostMethod.TabIndex = 6;
            // 
            // electronicExportInfo
            // 
            this.electronicExportInfo.Location = new System.Drawing.Point(128, 113);
            this.fieldLengthProvider.SetMaxLengthSource(this.electronicExportInfo, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExCustomsAESEEI);
            this.electronicExportInfo.Name = "electronicExportInfo";
            this.electronicExportInfo.Size = new System.Drawing.Size(145, 21);
            this.electronicExportInfo.TabIndex = 13;
            // 
            // labelNumberOfPieces
            // 
            this.labelNumberOfPieces.AutoSize = true;
            this.labelNumberOfPieces.Location = new System.Drawing.Point(5, 190);
            this.labelNumberOfPieces.Name = "labelNumberOfPieces";
            this.labelNumberOfPieces.Size = new System.Drawing.Size(94, 13);
            this.labelNumberOfPieces.TabIndex = 12;
            this.labelNumberOfPieces.Text = "Number of Pieces:";
            // 
            // numberOfPieces
            // 
            this.numberOfPieces.Location = new System.Drawing.Point(105, 187);
            this.numberOfPieces.Name = "numberOfPieces";
            this.numberOfPieces.Size = new System.Drawing.Size(100, 21);
            this.numberOfPieces.TabIndex = 13;
            // 
            // unitPrice
            // 
            this.unitPrice.Location = new System.Drawing.Point(105, 214);
            this.unitPrice.Name = "unitPrice";
            this.unitPrice.Size = new System.Drawing.Size(100, 21);
            this.unitPrice.TabIndex = 15;
            // 
            // labelUnitPrice
            // 
            this.labelUnitPrice.AutoSize = true;
            this.labelUnitPrice.Location = new System.Drawing.Point(43, 217);
            this.labelUnitPrice.Name = "labelUnitPrice";
            this.labelUnitPrice.Size = new System.Drawing.Size(56, 13);
            this.labelUnitPrice.TabIndex = 14;
            this.labelUnitPrice.Text = "Unit Price:";
            // 
            // FedExCustomsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sectionNafta);
            this.Controls.Add(this.sectionCommercialInvoice);
            this.Controls.Add(this.sectionBroker);
            this.Name = "FedExCustomsControl";
            this.Size = new System.Drawing.Size(447, 925);
            this.Controls.SetChildIndex(this.sectionBroker, 0);
            this.Controls.SetChildIndex(this.sectionCommercialInvoice, 0);
            this.Controls.SetChildIndex(this.sectionNafta, 0);
            this.Controls.SetChildIndex(this.sectionContents, 0);
            this.Controls.SetChildIndex(this.sectionGeneral, 0);
            ((System.ComponentModel.ISupportInitialize)(this.sectionContents.ContentPanel)).EndInit();
            this.sectionContents.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sectionContents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionGeneral.ContentPanel)).EndInit();
            this.sectionGeneral.ContentPanel.ResumeLayout(false);
            this.sectionGeneral.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionGeneral)).EndInit();
            this.groupSelectedContent.ResumeLayout(false);
            this.groupSelectedContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionBroker.ContentPanel)).EndInit();
            this.sectionBroker.ContentPanel.ResumeLayout(false);
            this.sectionBroker.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionBroker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionCommercialInvoice.ContentPanel)).EndInit();
            this.sectionCommercialInvoice.ContentPanel.ResumeLayout(false);
            this.sectionCommercialInvoice.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionCommercialInvoice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionNafta.ContentPanel)).EndInit();
            this.sectionNafta.ContentPanel.ResumeLayout(false);
            this.sectionNafta.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionNafta)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelBrokerAccountHeading;
        private ShipWorks.UI.Controls.MultiValueTextBox brokerAccount;
        private System.Windows.Forms.Label labelBrokerAccount;
        private System.Windows.Forms.CheckBox brokerEnabled;
        private ShipWorks.Data.Controls.PersonControl brokerControl;
        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionBroker;
        private ShipWorks.UI.Controls.MultiValueComboBox admissibilityPackaging;
        private System.Windows.Forms.Label labelAdmissibilityPackaging;
        private System.Windows.Forms.Label labelDocuments;
        private System.Windows.Forms.CheckBox documentsOnly;
        private System.Windows.Forms.Label labelRecipientTaxID;
        private ShipWorks.UI.Controls.MultiValueTextBox recipientTaxID;
        //private ShipWorks.UI.Controls.MultiValueTextBox documentDescription;
        //private System.Windows.Forms.Label labelDocumentDescription;
        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionCommercialInvoice;
        private ShipWorks.UI.Controls.MultiValueComboBox ciPurpose;
        private ShipWorks.UI.Controls.MultiValueComboBox ciTermsOfSale;
        private System.Windows.Forms.Label labelPurpose;
        private ShipWorks.UI.Controls.MultiValueTextBox ciComments;
        private System.Windows.Forms.Label labelComments;
        private System.Windows.Forms.Label labelTermsOfSale;
        private System.Windows.Forms.CheckBox commercialInvoice;
        private System.Windows.Forms.CheckBox electronicTradeDocuments;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private ShipWorks.UI.Controls.MoneyTextBox ciAdditional;
        private ShipWorks.UI.Controls.MoneyTextBox ciInsurance;
        private ShipWorks.UI.Controls.MoneyTextBox ciFreight;
        private ShipWorks.Data.Controls.PersonControl iorPersonControl;
        private System.Windows.Forms.CheckBox importerOfRecord;
        private ShipWorks.UI.Controls.MultiValueTextBox iorFedExAccount;
        private System.Windows.Forms.Label labelIorTaxID;
        private ShipWorks.UI.Controls.MultiValueTextBox iorTaxID;
        private System.Windows.Forms.Label labelIorFedexAccount;
        private System.Windows.Forms.Label labelIorAccount;
        private System.Windows.Forms.Label commercialInvoiceReferenceLabel;
        private ShipWorks.UI.Controls.MultiValueTextBox commercialInvoiceReference;
        private System.Windows.Forms.Label electronicExportInfoLabel;
        private MultiValueComboBox filingOption;
        private System.Windows.Forms.Label filingOptionLabel;

        private CollapsibleGroupControl sectionNafta;
        private System.Windows.Forms.CheckBox naftaEnabled;
        private System.Windows.Forms.Label preferenceLabel;
        private System.Windows.Forms.Label producerDeterminationLabel;
        private System.Windows.Forms.Label netCostMethodLabel;
        private MultiValueComboBox naftaPreference;
        private MultiValueComboBox naftaProducerDetermination;
        private MultiValueComboBox naftaNetCostMethod;
        private MultiValueTextBox electronicExportInfo;
        private MultiValueTextBox naftaProducerId;
        private System.Windows.Forms.Label labelNaftaProducerId;
        private MultiValueTextBox numberOfPieces;
        private System.Windows.Forms.Label labelNumberOfPieces;
        private MoneyTextBox unitPrice;
        private System.Windows.Forms.Label labelUnitPrice;
    }
}
