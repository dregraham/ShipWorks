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
            this.sectionContents.Location = new System.Drawing.Point(6, 108);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(17, 10);
            this.label1.TabIndex = 0;
            // 
            // customsValue
            // 
            this.customsValue.Location = new System.Drawing.Point(63, 7);
            this.customsValue.TabIndex = 1;
            // 
            // sectionGeneral
            // 
            // 
            // sectionGeneral.ContentPanel
            // 
            this.sectionGeneral.ContentPanel.Controls.Add(this.otherDetail);
            this.sectionGeneral.ContentPanel.Controls.Add(this.labelOtherDetail);
            this.sectionGeneral.ContentPanel.Controls.Add(this.contentType);
            this.sectionGeneral.ContentPanel.Controls.Add(this.labelContent);
            this.sectionGeneral.ContentPanel.Controls.Add(this.labelCustomsRecipientTin);
            this.sectionGeneral.ContentPanel.Controls.Add(this.customsRecipientTin);
            this.sectionGeneral.Location = new System.Drawing.Point(6, 5);
            this.sectionGeneral.Size = new System.Drawing.Size(572, 115);
            // 
            // otherDetail
            // 
            this.otherDetail.Location = new System.Drawing.Point(268, 34);
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
            this.labelOtherDetail.Location = new System.Drawing.Point(202, 37);
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
            this.contentType.Location = new System.Drawing.Point(63, 34);
            this.contentType.Name = "contentType";
            this.contentType.Size = new System.Drawing.Size(132, 21);
            this.contentType.TabIndex = 3;
            // 
            // labelContent
            // 
            this.labelContent.AutoSize = true;
            this.labelContent.BackColor = System.Drawing.Color.Transparent;
            this.labelContent.Location = new System.Drawing.Point(5, 37);
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
            this.labelCustomsRecipientTin.Size = new System.Drawing.Size(65, 20);
            this.labelCustomsRecipientTin.TabIndex = 4;
            this.labelCustomsRecipientTin.Text = "Recipient Tax ID:";
            //
            // custumsRecipientTin
            //
            this.customsRecipientTin.Location = new System.Drawing.Point(98, 60);
            this.customsRecipientTin.Size = new System.Drawing.Size(132, 21);
            this.customsRecipientTin.Name = "customsRecipientTin";
            this.customsRecipientTin.TabIndex = 5;
            // 
            // PostalCustomsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "PostalCustomsControl";
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

        private ShipWorks.UI.Controls.MultiValueTextBox otherDetail;
        private System.Windows.Forms.Label labelOtherDetail;
        private ShipWorks.UI.Controls.MultiValueComboBox contentType;
        private System.Windows.Forms.Label labelContent;
        private System.Windows.Forms.Label labelCustomsRecipientTin;
        private ShipWorks.UI.Controls.MultiValueTextBox customsRecipientTin;
    }
}
