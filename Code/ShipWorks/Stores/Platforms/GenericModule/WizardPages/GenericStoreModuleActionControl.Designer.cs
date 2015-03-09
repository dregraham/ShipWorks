namespace ShipWorks.Stores.Platforms.GenericModule.WizardPages
{
    partial class GenericStoreModuleActionControl
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
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory1 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            this.panelShipmentUpdate = new System.Windows.Forms.Panel();
            this.shipmentUpdate = new System.Windows.Forms.CheckBox();
            this.panelOrderStatus = new System.Windows.Forms.Panel();
            this.labelComment = new System.Windows.Forms.Label();
            this.comboStatus = new System.Windows.Forms.ComboBox();
            this.commentToken = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.statusUpdate = new System.Windows.Forms.CheckBox();
            this.panelShipmentUpdate.SuspendLayout();
            this.panelOrderStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelShipmentUpdate
            // 
            this.panelShipmentUpdate.Controls.Add(this.shipmentUpdate);
            this.panelShipmentUpdate.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelShipmentUpdate.Location = new System.Drawing.Point(0, 0);
            this.panelShipmentUpdate.Name = "panelShipmentUpdate";
            this.panelShipmentUpdate.Size = new System.Drawing.Size(459, 21);
            this.panelShipmentUpdate.TabIndex = 0;
            // 
            // shipmentUpdate
            // 
            this.shipmentUpdate.AutoSize = true;
            this.shipmentUpdate.Checked = true;
            this.shipmentUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.shipmentUpdate.Location = new System.Drawing.Point(13, 3);
            this.shipmentUpdate.Name = "shipmentUpdate";
            this.shipmentUpdate.Size = new System.Drawing.Size(204, 17);
            this.shipmentUpdate.TabIndex = 0;
            this.shipmentUpdate.Text = "Upload the shipment tracking number";
            this.shipmentUpdate.UseVisualStyleBackColor = true;
            // 
            // panelOrderStatus
            // 
            this.panelOrderStatus.Controls.Add(this.labelComment);
            this.panelOrderStatus.Controls.Add(this.comboStatus);
            this.panelOrderStatus.Controls.Add(this.commentToken);
            this.panelOrderStatus.Controls.Add(this.statusUpdate);
            this.panelOrderStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelOrderStatus.Location = new System.Drawing.Point(0, 21);
            this.panelOrderStatus.Name = "panelOrderStatus";
            this.panelOrderStatus.Size = new System.Drawing.Size(459, 56);
            this.panelOrderStatus.TabIndex = 1;
            // 
            // labelComment
            // 
            this.labelComment.AutoSize = true;
            this.labelComment.Location = new System.Drawing.Point(124, 35);
            this.labelComment.Name = "labelComment";
            this.labelComment.Size = new System.Drawing.Size(56, 13);
            this.labelComment.TabIndex = 4;
            this.labelComment.Text = "Comment:";
            // 
            // comboStatus
            // 
            this.comboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStatus.FormattingEnabled = true;
            this.comboStatus.Location = new System.Drawing.Point(181, 5);
            this.comboStatus.Name = "comboStatus";
            this.comboStatus.Size = new System.Drawing.Size(237, 21);
            this.comboStatus.TabIndex = 3;
            // 
            // commentToken
            // 
            this.commentToken.Location = new System.Drawing.Point(181, 32);
            this.commentToken.MaxLength = 32767;
            this.commentToken.Name = "commentToken";
            this.commentToken.Size = new System.Drawing.Size(237, 21);
            this.commentToken.TabIndex = 2;
            this.commentToken.Text = "{//ServiceUsed} - {//TrackingNumber}";
            this.commentToken.TokenSuggestionFactory = commonTokenSuggestionsFactory1;
            // 
            // statusUpdate
            // 
            this.statusUpdate.AutoSize = true;
            this.statusUpdate.Checked = true;
            this.statusUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.statusUpdate.Location = new System.Drawing.Point(13, 7);
            this.statusUpdate.Name = "statusUpdate";
            this.statusUpdate.Size = new System.Drawing.Size(171, 17);
            this.statusUpdate.TabIndex = 1;
            this.statusUpdate.Text = "Set the online order status to:";
            this.statusUpdate.UseVisualStyleBackColor = true;
            this.statusUpdate.CheckedChanged += new System.EventHandler(this.OnStatusUpdateCheckChanged);
            // 
            // GenericStoreModuleActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelOrderStatus);
            this.Controls.Add(this.panelShipmentUpdate);
            this.Name = "GenericStoreModuleActionControl";
            this.Size = new System.Drawing.Size(459, 80);
            this.panelShipmentUpdate.ResumeLayout(false);
            this.panelShipmentUpdate.PerformLayout();
            this.panelOrderStatus.ResumeLayout(false);
            this.panelOrderStatus.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelShipmentUpdate;
        private System.Windows.Forms.Panel panelOrderStatus;
        private System.Windows.Forms.CheckBox shipmentUpdate;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox commentToken;
        private System.Windows.Forms.CheckBox statusUpdate;
        private System.Windows.Forms.ComboBox comboStatus;
        private System.Windows.Forms.Label labelComment;
    }
}
