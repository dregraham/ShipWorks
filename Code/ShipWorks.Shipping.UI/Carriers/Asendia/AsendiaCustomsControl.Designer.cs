namespace ShipWorks.Shipping.UI.Carriers.Asendia
{
    partial class AsendiaCustomsControl
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
            this.contentType = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.nonDeliveryType = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelContent = new System.Windows.Forms.Label();
            this.labelNonDelivery = new System.Windows.Forms.Label();
            this.labelCustomsRecipientTIN = new System.Windows.Forms.Label();
            this.customsRecipientTIN = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelCustomsRecipientTINType = new System.Windows.Forms.Label();
            this.customsRecipientTINType = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelCustomsRecipientIssuingAuthority = new System.Windows.Forms.Label();
            this.customsRecipientIssuingAuthority = new ShipWorks.UI.Controls.MultiValueComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.sectionContents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionContents.ContentPanel)).BeginInit();
            this.sectionContents.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionGeneral)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionGeneral.ContentPanel)).BeginInit();
            this.sectionGeneral.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // sectionContents
            // 
            // 
            // sectionContents.ContentPanel
            // 
            this.sectionContents.ContentPanel.Controls.Add(this.itemsGrid);
            this.sectionContents.ContentPanel.Controls.Add(this.groupSelectedContent);
            this.sectionContents.ContentPanel.Controls.Add(this.delete);
            this.sectionContents.ContentPanel.Controls.Add(this.add);
            this.sectionContents.Location = new System.Drawing.Point(6, 210);
            this.sectionContents.MinimumSize = new System.Drawing.Size(480, 362);
            this.sectionContents.Size = new System.Drawing.Size(480, 362);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(90, 10);
            this.label1.TabIndex = 0;
            // 
            // customsValue
            // 
            this.customsValue.Location = new System.Drawing.Point(138, 7);
            this.customsValue.Size = new System.Drawing.Size(95, 21);
            this.customsValue.TabIndex = 1;
            // 
            // sectionGeneral
            // 
            // 
            // sectionGeneral.ContentPanel
            // 
            this.sectionGeneral.ContentPanel.Controls.Add(this.label1);
            this.sectionGeneral.ContentPanel.Controls.Add(this.customsValue);
            this.sectionGeneral.ContentPanel.Controls.Add(this.contentType);
            this.sectionGeneral.ContentPanel.Controls.Add(this.nonDeliveryType);
            this.sectionGeneral.ContentPanel.Controls.Add(this.labelContent);
            this.sectionGeneral.ContentPanel.Controls.Add(this.labelNonDelivery);
            this.sectionGeneral.ContentPanel.Controls.Add(this.labelCustomsRecipientTIN);
            this.sectionGeneral.ContentPanel.Controls.Add(this.customsRecipientTIN);
            this.sectionGeneral.ContentPanel.Controls.Add(this.labelCustomsRecipientTINType);
            this.sectionGeneral.ContentPanel.Controls.Add(this.customsRecipientTINType);
            this.sectionGeneral.ContentPanel.Controls.Add(this.labelCustomsRecipientIssuingAuthority);
            this.sectionGeneral.ContentPanel.Controls.Add(this.customsRecipientIssuingAuthority);
            this.sectionGeneral.Location = new System.Drawing.Point(6, 5);
            this.sectionGeneral.Size = new System.Drawing.Size(480, 200);
            // 
            // groupSelectedContent
            // 
            this.groupSelectedContent.Size = new System.Drawing.Size(643, 190);
            // 
            // add
            // 
            this.add.Location = new System.Drawing.Point(407, 10);
            // 
            // delete
            // 
            this.delete.Location = new System.Drawing.Point(407, 37);
            // 
            // itemsGrid
            // 
            this.itemsGrid.Size = new System.Drawing.Size(393, 116);
            // 
            // contentType
            // 
            this.contentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.contentType.FormattingEnabled = true;
            this.contentType.Location = new System.Drawing.Point(138, 34);
            this.contentType.Name = "contentType";
            this.contentType.PromptText = "(Multiple Values)";
            this.contentType.Size = new System.Drawing.Size(160, 21);
            this.contentType.TabIndex = 3;
            // 
            // nonDeliveryType
            // 
            this.nonDeliveryType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.nonDeliveryType.FormattingEnabled = true;
            this.nonDeliveryType.Location = new System.Drawing.Point(138, 61);
            this.nonDeliveryType.Name = "nonDeliveryType";
            this.nonDeliveryType.PromptText = "(Multiple Values)";
            this.nonDeliveryType.Size = new System.Drawing.Size(160, 21);
            this.nonDeliveryType.TabIndex = 3;
            // 
            // labelContent
            // 
            this.labelContent.AutoSize = true;
            this.labelContent.BackColor = System.Drawing.Color.Transparent;
            this.labelContent.Location = new System.Drawing.Point(78, 37);
            this.labelContent.Name = "labelContent";
            this.labelContent.Size = new System.Drawing.Size(50, 13);
            this.labelContent.TabIndex = 2;
            this.labelContent.Text = "Content:";
            // 
            // labelNonDelivery
            // 
            this.labelNonDelivery.AutoSize = true;
            this.labelNonDelivery.BackColor = System.Drawing.Color.Transparent;
            this.labelNonDelivery.Location = new System.Drawing.Point(55, 64);
            this.labelNonDelivery.Name = "labelNonDelivery";
            this.labelNonDelivery.Size = new System.Drawing.Size(72, 13);
            this.labelNonDelivery.TabIndex = 2;
            this.labelNonDelivery.Text = "Non Delivery:";
            // 
            // labelCustomsRecipientTIN
            // 
            this.labelCustomsRecipientTIN.AutoSize = true;
            this.labelCustomsRecipientTIN.BackColor = System.Drawing.Color.Transparent;
            this.labelCustomsRecipientTIN.Location = new System.Drawing.Point(40, 91);
            this.labelCustomsRecipientTIN.Name = "labelCustomsRecipientTIN";
            this.labelCustomsRecipientTIN.Size = new System.Drawing.Size(89, 13);
            this.labelCustomsRecipientTIN.TabIndex = 116;
            this.labelCustomsRecipientTIN.Text = "Recipient Tax Id:";
            // 
            // customsRecipientTIN
            // 
            this.customsRecipientTIN.BackColor = System.Drawing.Color.White;
            this.customsRecipientTIN.Location = new System.Drawing.Point(138, 88);
            this.customsRecipientTIN.Name = "customsRecipientTIN";
            this.customsRecipientTIN.Size = new System.Drawing.Size(160, 21);
            this.customsRecipientTIN.TabIndex = 5;
            // 
            // labelCustomsRecipientTINType
            // 
            this.labelCustomsRecipientTINType.AutoSize = true;
            this.labelCustomsRecipientTINType.BackColor = System.Drawing.Color.Transparent;
            this.labelCustomsRecipientTINType.Location = new System.Drawing.Point(77, 118);
            this.labelCustomsRecipientTINType.Name = "labelCustomsRecipientTINType";
            this.labelCustomsRecipientTINType.Size = new System.Drawing.Size(52, 13);
            this.labelCustomsRecipientTINType.TabIndex = 126;
            this.labelCustomsRecipientTINType.Text = "Tin Type:";
            // 
            // customsRecipientTINType
            // 
            this.customsRecipientTINType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.customsRecipientTINType.FormattingEnabled = true;
            this.customsRecipientTINType.Location = new System.Drawing.Point(138, 115);
            this.customsRecipientTINType.Name = "customsRecipientTINType";
            this.customsRecipientTINType.PromptText = "(Multiple Values)";
            this.customsRecipientTINType.Size = new System.Drawing.Size(160, 21);
            this.customsRecipientTINType.TabIndex = 7;
            // 
            // labelCustomsRecipientIssuingAuthority
            // 
            this.labelCustomsRecipientIssuingAuthority.AutoSize = true;
            this.labelCustomsRecipientIssuingAuthority.BackColor = System.Drawing.Color.Transparent;
            this.labelCustomsRecipientIssuingAuthority.Location = new System.Drawing.Point(35, 145);
            this.labelCustomsRecipientIssuingAuthority.Name = "labelCustomsRecipientIssuingAuthority";
            this.labelCustomsRecipientIssuingAuthority.Size = new System.Drawing.Size(93, 13);
            this.labelCustomsRecipientIssuingAuthority.TabIndex = 136;
            this.labelCustomsRecipientIssuingAuthority.Text = "Issuing Authority:";
            // 
            // customsRecipientIssuingAuthority
            // 
            this.customsRecipientIssuingAuthority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.customsRecipientIssuingAuthority.FormattingEnabled = true;
            this.customsRecipientIssuingAuthority.Location = new System.Drawing.Point(138, 142);
            this.customsRecipientIssuingAuthority.Name = "customsRecipientIssuingAuthority";
            this.customsRecipientIssuingAuthority.PromptText = "(Multiple Values)";
            this.customsRecipientIssuingAuthority.Size = new System.Drawing.Size(160, 21);
            this.customsRecipientIssuingAuthority.TabIndex = 9;
            // 
            // AsendiaCustomsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.MinimumSize = new System.Drawing.Size(489, 525);
            this.Name = "AsendiaCustomsControl";
            this.Size = new System.Drawing.Size(472, 525);
            ((System.ComponentModel.ISupportInitialize)(this.sectionContents.ContentPanel)).EndInit();
            this.sectionContents.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sectionContents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionGeneral.ContentPanel)).EndInit();
            this.sectionGeneral.ContentPanel.ResumeLayout(false);
            this.sectionGeneral.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionGeneral)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private ShipWorks.UI.Controls.MultiValueComboBox contentType;
        private ShipWorks.UI.Controls.MultiValueComboBox nonDeliveryType;
        private System.Windows.Forms.Label labelContent;
        private System.Windows.Forms.Label labelNonDelivery;
        private System.Windows.Forms.Label labelCustomsRecipientTIN;
        private ShipWorks.UI.Controls.MultiValueTextBox customsRecipientTIN;
        private System.Windows.Forms.Label labelCustomsRecipientTINType;
        private ShipWorks.UI.Controls.MultiValueComboBox customsRecipientTINType;
        private System.Windows.Forms.Label labelCustomsRecipientIssuingAuthority;
        private ShipWorks.UI.Controls.MultiValueComboBox customsRecipientIssuingAuthority;
    }
}
