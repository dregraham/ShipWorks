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
            this.sectionGeneral.Location = new System.Drawing.Point(6, 5);
            this.sectionGeneral.Size = new System.Drawing.Size(572, 122);
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
            this.labelNonDelivery.Location = new System.Drawing.Point(35, 64);
            this.labelNonDelivery.Name = "labelNonDelivery";
            this.labelNonDelivery.Size = new System.Drawing.Size(72, 13);
            this.labelNonDelivery.TabIndex = 2;
            this.labelNonDelivery.Text = "Non Delivery:";
            // 
            // DhlExpressCustomsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "DhlExpressCustomsControl";
            this.Size = new System.Drawing.Size(581, 525);
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
    }
}
