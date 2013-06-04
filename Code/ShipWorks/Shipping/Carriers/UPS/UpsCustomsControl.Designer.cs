namespace ShipWorks.Shipping.Carriers.UPS
{
    partial class UpsCustomsControl
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
            this.documentsOnly = new System.Windows.Forms.CheckBox();
            this.labelDocuments = new System.Windows.Forms.Label();
            this.labelDescriptionOfGoods = new System.Windows.Forms.Label();
            this.descriptionOfGoods = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.sectionCommercialInvoice = new ShipWorks.UI.Controls.CollapsibleGroupControl();
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
            this.paperless = new System.Windows.Forms.CheckBox();
            this.paperlessLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.sectionContents)).BeginInit();
            this.sectionContents.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionGeneral)).BeginInit();
            this.sectionGeneral.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionCommercialInvoice)).BeginInit();
            this.sectionCommercialInvoice.ContentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // sectionContents
            // 
            this.sectionContents.Location = new System.Drawing.Point(6, 140);
            this.sectionContents.Size = new System.Drawing.Size(555, 362);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(33, 10);
            this.label1.TabIndex = 0;
            // 
            // customsValue
            // 
            this.customsValue.Location = new System.Drawing.Point(76, 7);
            this.customsValue.TabIndex = 1;
            // 
            // sectionGeneral
            // 
            // 
            // sectionGeneral.ContentPanel
            // 
            this.sectionGeneral.ContentPanel.Controls.Add(this.paperless);
            this.sectionGeneral.ContentPanel.Controls.Add(this.paperlessLabel);
            this.sectionGeneral.ContentPanel.Controls.Add(this.descriptionOfGoods);
            this.sectionGeneral.ContentPanel.Controls.Add(this.labelDescriptionOfGoods);
            this.sectionGeneral.ContentPanel.Controls.Add(this.documentsOnly);
            this.sectionGeneral.ContentPanel.Controls.Add(this.labelDocuments);
            this.sectionGeneral.Location = new System.Drawing.Point(6, 5);
            this.sectionGeneral.Size = new System.Drawing.Size(555, 130);
            // 
            // groupSelectedContent
            // 
            this.groupSelectedContent.Size = new System.Drawing.Size(534, 190);
            // 
            // add
            // 
            this.add.Location = new System.Drawing.Point(483, 10);
            // 
            // delete
            // 
            this.delete.Location = new System.Drawing.Point(483, 37);
            // 
            // sandGrid
            // 
            this.sandGrid.Size = new System.Drawing.Size(469, 116);
            // 
            // documentsOnly
            // 
            this.documentsOnly.AutoSize = true;
            this.documentsOnly.BackColor = System.Drawing.Color.White;
            this.documentsOnly.Location = new System.Drawing.Point(75, 34);
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
            this.labelDocuments.Location = new System.Drawing.Point(15, 35);
            this.labelDocuments.Name = "labelDocuments";
            this.labelDocuments.Size = new System.Drawing.Size(55, 13);
            this.labelDocuments.TabIndex = 2;
            this.labelDocuments.Text = "Contents:";
            // 
            // labelDescriptionOfGoods
            // 
            this.labelDescriptionOfGoods.AutoSize = true;
            this.labelDescriptionOfGoods.BackColor = System.Drawing.Color.White;
            this.labelDescriptionOfGoods.Location = new System.Drawing.Point(6, 59);
            this.labelDescriptionOfGoods.Name = "labelDescriptionOfGoods";
            this.labelDescriptionOfGoods.Size = new System.Drawing.Size(64, 13);
            this.labelDescriptionOfGoods.TabIndex = 4;
            this.labelDescriptionOfGoods.Text = "Description:";
            // 
            // descriptionOfGoods
            // 
            this.descriptionOfGoods.Location = new System.Drawing.Point(75, 56);
            this.fieldLengthProvider.SetMaxLengthSource(this.descriptionOfGoods, ShipWorks.Data.Utility.EntityFieldLengthSource.UpsCustomsDescription);
            this.descriptionOfGoods.Name = "descriptionOfGoods";
            this.descriptionOfGoods.Size = new System.Drawing.Size(258, 21);
            this.descriptionOfGoods.TabIndex = 5;
            // 
            // sectionCommercialInvoice
            // 
            this.sectionCommercialInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // sectionCommercialInvoice.ContentPanel
            // 
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
            this.sectionCommercialInvoice.ContentPanel.Controls.Add(this.commercialInvoice);
            this.sectionCommercialInvoice.ExtraText = "";
            this.sectionCommercialInvoice.Location = new System.Drawing.Point(6, 507);
            this.sectionCommercialInvoice.Name = "sectionCommercialInvoice";
            this.sectionCommercialInvoice.SectionName = "Commercial Invoice";
            this.sectionCommercialInvoice.SettingsKey = "{ae2272b7-0a1c-43d8-9215-e301bcc1f9b8}";
            this.sectionCommercialInvoice.Size = new System.Drawing.Size(555, 221);
            this.sectionCommercialInvoice.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(27, 139);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 13);
            this.label7.TabIndex = 94;
            this.label7.Text = "Insurance costs:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(28, 166);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 13);
            this.label6.TabIndex = 93;
            this.label6.Text = "Additional costs:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(41, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 92;
            this.label5.Text = "Freight costs:";
            // 
            // ciAdditional
            // 
            this.ciAdditional.Amount = new decimal(new int[] {
            0,
            0,
            0,
            65536});
            this.ciAdditional.Location = new System.Drawing.Point(120, 163);
            this.ciAdditional.Name = "ciAdditional";
            this.ciAdditional.Size = new System.Drawing.Size(95, 21);
            this.ciAdditional.TabIndex = 6;
            this.ciAdditional.Text = "$0.00";
            // 
            // ciInsurance
            // 
            this.ciInsurance.Amount = new decimal(new int[] {
            0,
            0,
            0,
            65536});
            this.ciInsurance.Location = new System.Drawing.Point(120, 136);
            this.ciInsurance.Name = "ciInsurance";
            this.ciInsurance.Size = new System.Drawing.Size(95, 21);
            this.ciInsurance.TabIndex = 5;
            this.ciInsurance.Text = "$0.00";
            // 
            // ciFreight
            // 
            this.ciFreight.Amount = new decimal(new int[] {
            0,
            0,
            0,
            65536});
            this.ciFreight.Location = new System.Drawing.Point(120, 112);
            this.ciFreight.Name = "ciFreight";
            this.ciFreight.Size = new System.Drawing.Size(95, 21);
            this.ciFreight.TabIndex = 4;
            this.ciFreight.Text = "$0.00";
            // 
            // ciPurpose
            // 
            this.ciPurpose.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ciPurpose.FormattingEnabled = true;
            this.ciPurpose.Location = new System.Drawing.Point(119, 58);
            this.ciPurpose.Name = "ciPurpose";
            this.ciPurpose.PromptText = "(Multiple Values)";
            this.ciPurpose.Size = new System.Drawing.Size(161, 21);
            this.ciPurpose.TabIndex = 2;
            // 
            // ciTermsOfSale
            // 
            this.ciTermsOfSale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ciTermsOfSale.FormattingEnabled = true;
            this.ciTermsOfSale.Location = new System.Drawing.Point(119, 31);
            this.ciTermsOfSale.Name = "ciTermsOfSale";
            this.ciTermsOfSale.PromptText = "(Multiple Values)";
            this.ciTermsOfSale.Size = new System.Drawing.Size(161, 21);
            this.ciTermsOfSale.TabIndex = 1;
            // 
            // labelPurpose
            // 
            this.labelPurpose.AutoSize = true;
            this.labelPurpose.BackColor = System.Drawing.Color.White;
            this.labelPurpose.Location = new System.Drawing.Point(64, 61);
            this.labelPurpose.Name = "labelPurpose";
            this.labelPurpose.Size = new System.Drawing.Size(50, 13);
            this.labelPurpose.TabIndex = 86;
            this.labelPurpose.Text = "Purpose:";
            // 
            // ciComments
            // 
            this.ciComments.Location = new System.Drawing.Point(119, 85);
            this.fieldLengthProvider.SetMaxLengthSource(this.ciComments, ShipWorks.Data.Utility.EntityFieldLengthSource.UpsCommercialInvoiceComments);
            this.ciComments.Name = "ciComments";
            this.ciComments.Size = new System.Drawing.Size(161, 21);
            this.ciComments.TabIndex = 3;
            // 
            // labelComments
            // 
            this.labelComments.AutoSize = true;
            this.labelComments.BackColor = System.Drawing.Color.White;
            this.labelComments.Location = new System.Drawing.Point(53, 88);
            this.labelComments.Name = "labelComments";
            this.labelComments.Size = new System.Drawing.Size(61, 13);
            this.labelComments.TabIndex = 84;
            this.labelComments.Text = "Comments:";
            // 
            // labelTermsOfSale
            // 
            this.labelTermsOfSale.AutoSize = true;
            this.labelTermsOfSale.BackColor = System.Drawing.Color.White;
            this.labelTermsOfSale.Location = new System.Drawing.Point(42, 34);
            this.labelTermsOfSale.Name = "labelTermsOfSale";
            this.labelTermsOfSale.Size = new System.Drawing.Size(75, 13);
            this.labelTermsOfSale.TabIndex = 83;
            this.labelTermsOfSale.Text = "Terms of sale:";
            // 
            // commercialInvoice
            // 
            this.commercialInvoice.AutoSize = true;
            this.commercialInvoice.BackColor = System.Drawing.Color.White;
            this.commercialInvoice.Location = new System.Drawing.Point(12, 8);
            this.commercialInvoice.Name = "commercialInvoice";
            this.commercialInvoice.Size = new System.Drawing.Size(154, 17);
            this.commercialInvoice.TabIndex = 0;
            this.commercialInvoice.Text = "Create Commercial Invoice";
            this.commercialInvoice.UseVisualStyleBackColor = false;
            // 
            // paperless
            // 
            this.paperless.AutoSize = true;
            this.paperless.BackColor = System.Drawing.Color.White;
            this.paperless.Location = new System.Drawing.Point(75, 84);
            this.paperless.Name = "paperless";
            this.paperless.Size = new System.Drawing.Size(205, 17);
            this.paperless.TabIndex = 7;
            this.paperless.Text = "Paperless international documentation";
            this.paperless.UseVisualStyleBackColor = false;
            // 
            // paperlessLabel
            // 
            this.paperlessLabel.AutoSize = true;
            this.paperlessLabel.BackColor = System.Drawing.Color.White;
            this.paperlessLabel.Location = new System.Drawing.Point(13, 85);
            this.paperlessLabel.Name = "paperlessLabel";
            this.paperlessLabel.Size = new System.Drawing.Size(57, 13);
            this.paperlessLabel.TabIndex = 6;
            this.paperlessLabel.Text = "Paperless:";
            // 
            // UpsCustomsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sectionCommercialInvoice);
            this.Name = "UpsCustomsControl";
            this.Size = new System.Drawing.Size(564, 727);
            this.Controls.SetChildIndex(this.sectionCommercialInvoice, 0);
            this.Controls.SetChildIndex(this.sectionContents, 0);
            this.Controls.SetChildIndex(this.sectionGeneral, 0);
            this.sectionContents.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sectionContents)).EndInit();
            this.sectionGeneral.ContentPanel.ResumeLayout(false);
            this.sectionGeneral.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionGeneral)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.sectionCommercialInvoice.ContentPanel.ResumeLayout(false);
            this.sectionCommercialInvoice.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionCommercialInvoice)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox documentsOnly;
        private System.Windows.Forms.Label labelDocuments;
        private ShipWorks.UI.Controls.MultiValueTextBox descriptionOfGoods;
        private System.Windows.Forms.Label labelDescriptionOfGoods;
        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionCommercialInvoice;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private ShipWorks.UI.Controls.MoneyTextBox ciAdditional;
        private ShipWorks.UI.Controls.MoneyTextBox ciInsurance;
        private ShipWorks.UI.Controls.MoneyTextBox ciFreight;
        private ShipWorks.UI.Controls.MultiValueComboBox ciPurpose;
        private ShipWorks.UI.Controls.MultiValueComboBox ciTermsOfSale;
        private System.Windows.Forms.Label labelPurpose;
        private ShipWorks.UI.Controls.MultiValueTextBox ciComments;
        private System.Windows.Forms.Label labelComments;
        private System.Windows.Forms.Label labelTermsOfSale;
        private System.Windows.Forms.CheckBox commercialInvoice;
        private System.Windows.Forms.CheckBox paperless;
        private System.Windows.Forms.Label paperlessLabel;
    }
}
