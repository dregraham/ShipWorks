namespace ShipWorks.Stores.Platforms.Magento.WizardPages
{
    partial class MagentoOnlineUpdateActionControl
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
            this.shipmentUpdate = new System.Windows.Forms.CheckBox();
            this.labelComments = new System.Windows.Forms.Label();
            this.commentToken = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelEmail = new System.Windows.Forms.Label();
            this.sendEmail = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // shipmentUpdate
            // 
            this.shipmentUpdate.AutoSize = true;
            this.shipmentUpdate.Checked = true;
            this.shipmentUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.shipmentUpdate.Location = new System.Drawing.Point(12, 8);
            this.shipmentUpdate.Name = "shipmentUpdate";
            this.shipmentUpdate.Size = new System.Drawing.Size(397, 17);
            this.shipmentUpdate.TabIndex = 2;
            this.shipmentUpdate.Text = "Mark and the order as Completed in Magento and upload the shipment details";
            this.shipmentUpdate.UseVisualStyleBackColor = true;
            this.shipmentUpdate.CheckedChanged += new System.EventHandler(this.OnChangeTaskEnabled);
            // 
            // labelComments
            // 
            this.labelComments.AutoSize = true;
            this.labelComments.Location = new System.Drawing.Point(31, 34);
            this.labelComments.Name = "labelComments";
            this.labelComments.Size = new System.Drawing.Size(61, 13);
            this.labelComments.TabIndex = 3;
            this.labelComments.Text = "Comments:";
            // 
            // commentToken
            // 
            this.commentToken.Location = new System.Drawing.Point(94, 30);
            this.commentToken.MaxLength = 32767;
            this.commentToken.Name = "commentToken";
            this.commentToken.Size = new System.Drawing.Size(279, 21);
            this.commentToken.TabIndex = 4;
            this.commentToken.TokenSuggestionFactory = commonTokenSuggestionsFactory1;
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(57, 59);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(35, 13);
            this.labelEmail.TabIndex = 5;
            this.labelEmail.Text = "Email:";
            // 
            // sendEmail
            // 
            this.sendEmail.AutoSize = true;
            this.sendEmail.Location = new System.Drawing.Point(94, 58);
            this.sendEmail.Name = "sendEmail";
            this.sendEmail.Size = new System.Drawing.Size(344, 17);
            this.sendEmail.TabIndex = 6;
            this.sendEmail.Text = "Magento should email the customer that the order was completed.";
            this.sendEmail.UseVisualStyleBackColor = true;
            // 
            // MagentoOnlineUpdateActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sendEmail);
            this.Controls.Add(this.labelEmail);
            this.Controls.Add(this.commentToken);
            this.Controls.Add(this.labelComments);
            this.Controls.Add(this.shipmentUpdate);
            this.Name = "MagentoOnlineUpdateActionControl";
            this.Size = new System.Drawing.Size(459, 80);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox shipmentUpdate;
        private System.Windows.Forms.Label labelComments;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox commentToken;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.CheckBox sendEmail;
    }
}
