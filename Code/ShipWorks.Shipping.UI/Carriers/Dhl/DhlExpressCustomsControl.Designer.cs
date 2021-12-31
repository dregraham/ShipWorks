using System.Drawing;
using System.Windows.Forms;

namespace ShipWorks.Shipping.UI.Carriers.Dhl
{
    partial class DhlExpressCustomsControl
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
            this.labelCustomsRecipientTin = new System.Windows.Forms.Label();
            this.customsRecipientTin = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.taxIdType = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelTaxIdType = new System.Windows.Forms.Label();
            this.customsTinIssuingAuthority = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelCustomsTinIssuingAuthority = new Label();
            ((System.ComponentModel.ISupportInitialize) (this.sectionContents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionContents.ContentPanel)).BeginInit();
            this.sectionContents.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionGeneral)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionGeneral.ContentPanel)).BeginInit();
            this.sectionGeneral.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
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
            this.sectionContents.Location = new System.Drawing.Point(6, 132);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(70, 10);
            this.label1.TabIndex = 0;
            // 
            // customsValue
            // 
            this.customsValue.Location = new System.Drawing.Point(113, 7);
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
            this.sectionGeneral.ContentPanel.Controls.Add(this.labelCustomsRecipientTin);
            this.sectionGeneral.ContentPanel.Controls.Add(this.customsRecipientTin);
            this.sectionGeneral.ContentPanel.Controls.Add(this.labelTaxIdType);
            this.sectionGeneral.ContentPanel.Controls.Add(this.taxIdType);
            this.sectionGeneral.ContentPanel.Controls.Add(this.customsTinIssuingAuthority);
            this.sectionGeneral.ContentPanel.Controls.Add(this.labelCustomsTinIssuingAuthority);
            this.sectionGeneral.Location = new System.Drawing.Point(6, 5);
            this.sectionGeneral.Size = new System.Drawing.Size(572, 191);
            // 
            // contentType
            // 
            this.contentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.contentType.FormattingEnabled = true;
            this.contentType.Location = new System.Drawing.Point(113, 34);
            this.contentType.Name = "contentType";
            this.contentType.PromptText = "(Multiple Values)";
            this.contentType.Size = new System.Drawing.Size(160, 21);
            this.contentType.TabIndex = 3;
            // 
            // nonDeliveryType
            // 
            this.nonDeliveryType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.nonDeliveryType.FormattingEnabled = true;
            this.nonDeliveryType.Location = new System.Drawing.Point(113, 61);
            this.nonDeliveryType.Name = "nonDeliveryType";
            this.nonDeliveryType.PromptText = "(Multiple Values)";
            this.nonDeliveryType.Size = new System.Drawing.Size(160, 21);
            this.nonDeliveryType.TabIndex = 3;
            // 
            // labelContent
            // 
            this.labelContent.AutoSize = true;
            this.labelContent.BackColor = System.Drawing.Color.Transparent;
            this.labelContent.Location = new System.Drawing.Point(57, 37);
            this.labelContent.Name = "labelContent";
            this.labelContent.Size = new System.Drawing.Size(50, 13);
            this.labelContent.TabIndex = 2;
            this.labelContent.Text = "Content:";
            // 
            // labelNonDelivery
            // 
            this.labelNonDelivery.AutoSize = true;
            this.labelNonDelivery.BackColor = System.Drawing.Color.Transparent;
            this.labelNonDelivery.Location = new System.Drawing.Point(35, 66);
            this.labelNonDelivery.Name = "labelNonDelivery";
            this.labelNonDelivery.Size = new System.Drawing.Size(72, 13);
            this.labelNonDelivery.TabIndex = 2;
            this.labelNonDelivery.Text = "Non Delivery:";
            //
            // labelCustomsRecipientTin
            //
            this.labelCustomsRecipientTin.AutoSize = true;
            this.labelCustomsRecipientTin.BackColor = System.Drawing.Color.Transparent;
            this.labelCustomsRecipientTin.Location = new System.Drawing.Point(20, 90);
            this.labelCustomsRecipientTin.Name = "labelCustomsRecipientTin";
            this.labelCustomsRecipientTin.TabIndex = 3;
            this.labelCustomsRecipientTin.Text = "Recipient Tax ID:";
            //
            // customsRecipientTin
            //
            this.customsRecipientTin.Location = new System.Drawing.Point(113, 85);
            this.customsRecipientTin.Name = "customsRecipientTin";
            this.customsRecipientTin.TabIndex = 4;
            this.customsRecipientTin.Size = new System.Drawing.Size(160, 21);
            //
            // labelTaxIdType
            //
            this.labelTaxIdType.AutoSize = true;
            this.labelTaxIdType.BackColor = System.Drawing.Color.Transparent;
            this.labelTaxIdType.Location = new System.Drawing.Point(40, 114);
            this.labelTaxIdType.Name = "labelTaxIdType";
            this.labelTaxIdType.TabIndex = 5;
            this.labelTaxIdType.Text = "TIN Type:";
            //
            // taxIdType
            //
            this.taxIdType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.taxIdType.FormattingEnabled = true;
            this.taxIdType.Location = new System.Drawing.Point(113, 111);
            this.taxIdType.Name = "taxIdType";
            this.taxIdType.PromptText = "(TIN Types)";
            this.taxIdType.Size = new System.Drawing.Size(160, 21);
            this.taxIdType.TabIndex = 6;
            //
            // labelCustomsTinIssuingAuthority
            //
            this.labelCustomsTinIssuingAuthority.AutoSize = true;
            this.labelCustomsTinIssuingAuthority.BackColor = Color.Transparent;
            this.labelCustomsTinIssuingAuthority.Location = new Point(20, 138);
            this.labelCustomsTinIssuingAuthority.Name = "labelCustomsTinIssuingAuthority";
            this.labelCustomsTinIssuingAuthority.TabIndex = 7;
            this.labelCustomsTinIssuingAuthority.Text = "Issuing Authority:";
            //
            // customsTinIssuingAuthority
            //
            this.customsTinIssuingAuthority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.customsTinIssuingAuthority.FormattingEnabled = false;
            this.customsTinIssuingAuthority.Location = new Point(113, 137);
            this.customsTinIssuingAuthority.Name = "customsTinIssuingAuthority";
            this.customsTinIssuingAuthority.PromptText = "(Issuing Authority)";
            this.customsTinIssuingAuthority.Size = new Size(160, 21);
            this.customsTinIssuingAuthority.TabIndex = 8;
            //
            // DhlExpressCustomsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "DhlExpressCustomsControl";
            this.Size = new System.Drawing.Size(581, 525);
            ((System.ComponentModel.ISupportInitialize) (this.sectionContents.ContentPanel)).EndInit();
            this.sectionContents.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.sectionContents)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionGeneral.ContentPanel)).EndInit();
            this.sectionGeneral.ContentPanel.ResumeLayout(false);
            this.sectionGeneral.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionGeneral)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Controls.MultiValueComboBox contentType;
        private ShipWorks.UI.Controls.MultiValueComboBox nonDeliveryType;
        private System.Windows.Forms.Label labelContent;
        private System.Windows.Forms.Label labelNonDelivery;
        private System.Windows.Forms.Label labelCustomsRecipientTin;
        private ShipWorks.UI.Controls.MultiValueTextBox customsRecipientTin;
        private ShipWorks.UI.Controls.MultiValueComboBox taxIdType;
        private System.Windows.Forms.Label labelTaxIdType;
        private System.Windows.Forms.Label labelCustomsTinIssuingAuthority;
        private ShipWorks.UI.Controls.MultiValueComboBox customsTinIssuingAuthority;
    }
}