namespace ShipWorks.Shipping.Carriers.Postal
{
    partial class PostalCustomsControl
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
			this.otherDetail = new ShipWorks.UI.Controls.MultiValueTextBox();
			this.labelOtherDetail = new System.Windows.Forms.Label();
			this.contentType = new ShipWorks.UI.Controls.MultiValueComboBox();
			this.labelContent = new System.Windows.Forms.Label();
			this.labelCustomsRecipientTin = new System.Windows.Forms.Label();
			this.customsRecipientTin = new ShipWorks.UI.Controls.MultiValueTextBox();
			this.labelInternalTransaction = new System.Windows.Forms.Label();
			this.internalTransactionNumber = new ShipWorks.UI.Controls.MultiValueTextBox();
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
			this.sectionContents.Location = new System.Drawing.Point(6, 154);
			this.sectionContents.Size = new System.Drawing.Size(538, 372);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(58, 10);
			this.label1.TabIndex = 0;
			// 
			// customsValue
			// 
			this.customsValue.Location = new System.Drawing.Point(98, 7);
			this.customsValue.Size = new System.Drawing.Size(105, 21);
			this.customsValue.TabIndex = 1;
			// 
			// sectionGeneral
			// 
			// 
			// sectionGeneral.ContentPanel
			// 
			this.sectionGeneral.ContentPanel.Controls.Add(this.label1);
			this.sectionGeneral.ContentPanel.Controls.Add(this.customsValue);
			this.sectionGeneral.ContentPanel.Controls.Add(this.otherDetail);
			this.sectionGeneral.ContentPanel.Controls.Add(this.labelOtherDetail);
			this.sectionGeneral.ContentPanel.Controls.Add(this.contentType);
			this.sectionGeneral.ContentPanel.Controls.Add(this.labelContent);
			this.sectionGeneral.ContentPanel.Controls.Add(this.labelCustomsRecipientTin);
			this.sectionGeneral.ContentPanel.Controls.Add(this.customsRecipientTin);
			this.sectionGeneral.ContentPanel.Controls.Add(this.internalTransactionNumber);
			this.sectionGeneral.ContentPanel.Controls.Add(this.labelInternalTransaction);
			this.sectionGeneral.Location = new System.Drawing.Point(6, 5);
			this.sectionGeneral.Size = new System.Drawing.Size(538, 144);
			// 
			// groupSelectedContent
			// 
			this.groupSelectedContent.Size = new System.Drawing.Size(585, 190);
			// 
			// add
			// 
			this.add.Location = new System.Drawing.Point(457, 10);
			this.add.Size = new System.Drawing.Size(70, 23);
			// 
			// delete
			// 
			this.delete.Location = new System.Drawing.Point(457, 37);
			this.delete.Size = new System.Drawing.Size(70, 23);
			// 
			// itemsGrid
			// 
			this.itemsGrid.Size = new System.Drawing.Size(444, 116);
			// 
			// otherDetail
			// 
			this.otherDetail.Location = new System.Drawing.Point(305, 34);
			this.otherDetail.MaxLength = 15;
			this.fieldLengthProvider.SetMaxLengthSource(this.otherDetail, ShipWorks.Data.Utility.EntityFieldLengthSource.PostalCustomsDescription);
			this.otherDetail.Name = "otherDetail";
			this.otherDetail.Size = new System.Drawing.Size(136, 21);
			this.otherDetail.TabIndex = 5;
			this.otherDetail.Visible = false;
			// 
			// labelOtherDetail
			// 
			this.labelOtherDetail.AutoSize = true;
			this.labelOtherDetail.BackColor = System.Drawing.Color.Transparent;
			this.labelOtherDetail.Location = new System.Drawing.Point(238, 37);
			this.labelOtherDetail.Name = "labelOtherDetail";
			this.labelOtherDetail.Size = new System.Drawing.Size(64, 13);
			this.labelOtherDetail.TabIndex = 4;
			this.labelOtherDetail.Text = "Description:";
			this.labelOtherDetail.Visible = false;
			// 
			// contentType
			// 
			this.contentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.contentType.FormattingEnabled = true;
			this.contentType.Location = new System.Drawing.Point(98, 34);
			this.contentType.Name = "contentType";
			this.contentType.PromptText = "(Multiple Values)";
			this.contentType.Size = new System.Drawing.Size(132, 21);
			this.contentType.TabIndex = 3;
			// 
			// labelContent
			// 
			this.labelContent.AutoSize = true;
			this.labelContent.BackColor = System.Drawing.Color.Transparent;
			this.labelContent.Location = new System.Drawing.Point(45, 37);
			this.labelContent.Name = "labelContent";
			this.labelContent.Size = new System.Drawing.Size(50, 13);
			this.labelContent.TabIndex = 2;
			this.labelContent.Text = "Content:";
			// 
			// labelCustomsRecipientTin
			// 
			this.labelCustomsRecipientTin.AutoSize = true;
			this.labelCustomsRecipientTin.BackColor = System.Drawing.Color.Transparent;
			this.labelCustomsRecipientTin.Location = new System.Drawing.Point(5, 63);
			this.labelCustomsRecipientTin.Name = "labelCustomsRecipientTin";
			this.labelCustomsRecipientTin.Size = new System.Drawing.Size(90, 13);
			this.labelCustomsRecipientTin.TabIndex = 4;
			this.labelCustomsRecipientTin.Text = "Recipient Tax ID:";
			// 
			// customsRecipientTin
			// 
			this.customsRecipientTin.Location = new System.Drawing.Point(98, 60);
			this.customsRecipientTin.Name = "customsRecipientTin";
			this.customsRecipientTin.Size = new System.Drawing.Size(132, 21);
			this.customsRecipientTin.TabIndex = 5;
			// 
			// labelInternalTransaction
			// 
			this.labelInternalTransaction.AutoSize = true;
			this.labelInternalTransaction.BackColor = System.Drawing.Color.Transparent;
			this.labelInternalTransaction.Location = new System.Drawing.Point(56, 92);
			this.labelInternalTransaction.Name = "labelInternalTransaction";
			this.labelInternalTransaction.Size = new System.Drawing.Size(39, 13);
			this.labelInternalTransaction.TabIndex = 6;
			this.labelInternalTransaction.Text = "ITN #:";
			// 
			// internalTransactionNumber
			// 
			this.internalTransactionNumber.Location = new System.Drawing.Point(98, 88);
			this.internalTransactionNumber.Name = "internalTransactionNumber";
			this.internalTransactionNumber.Size = new System.Drawing.Size(132, 21);
			this.internalTransactionNumber.TabIndex = 7;
			// 
			// PostalCustomsControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Name = "PostalCustomsControl";
			this.Size = new System.Drawing.Size(547, 525);
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

        private ShipWorks.UI.Controls.MultiValueTextBox otherDetail;
        private System.Windows.Forms.Label labelOtherDetail;
        private ShipWorks.UI.Controls.MultiValueComboBox contentType;
        private System.Windows.Forms.Label labelContent;
        private System.Windows.Forms.Label labelCustomsRecipientTin;
        private ShipWorks.UI.Controls.MultiValueTextBox customsRecipientTin;
		private UI.Controls.MultiValueTextBox internalTransactionNumber;
		private System.Windows.Forms.Label labelInternalTransaction;
	}
}
