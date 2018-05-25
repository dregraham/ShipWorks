namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    partial class UpsOltReturnsControl
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
            this.returnContents = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.returnService = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.returnEmailPanel = new System.Windows.Forms.Panel();
            this.returnUndeliverableEmail = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.returnDescription = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.returnEmailPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // returnContents
            // 
            this.returnContents.Location = new System.Drawing.Point(152, 48);
            this.returnContents.MaxLength = 32767;
            this.returnContents.Name = "returnContents";
            this.returnContents.Size = new System.Drawing.Size(190, 21);
            this.returnContents.TabIndex = 4;
            this.returnContents.TokenSuggestionFactory = commonTokenSuggestionsFactory1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(94, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Contents:";
            // 
            // returnService
            // 
            this.returnService.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.returnService.FormattingEnabled = true;
            this.returnService.Location = new System.Drawing.Point(96, 3);
            this.returnService.Name = "returnService";
            this.returnService.PromptText = "(Multiple Values)";
            this.returnService.Size = new System.Drawing.Size(207, 21);
            this.returnService.TabIndex = 1;
            this.returnService.SelectedIndexChanged += new System.EventHandler(this.OnReturnServiceChanged);
            // 
            // returnEmailPanel
            // 
            this.returnEmailPanel.BackColor = System.Drawing.Color.White;
            this.returnEmailPanel.Controls.Add(this.returnUndeliverableEmail);
            this.returnEmailPanel.Controls.Add(this.label4);
            this.returnEmailPanel.Location = new System.Drawing.Point(11, 73);
            this.returnEmailPanel.Name = "returnEmailPanel";
            this.returnEmailPanel.Size = new System.Drawing.Size(340, 28);
            this.returnEmailPanel.TabIndex = 5;
            // 
            // returnUndeliverableEmail
            // 
            this.returnUndeliverableEmail.Location = new System.Drawing.Point(141, 5);
            this.returnUndeliverableEmail.Name = "returnUndeliverableEmail";
            this.returnUndeliverableEmail.Size = new System.Drawing.Size(191, 21);
            this.returnUndeliverableEmail.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Undeliverable email:";
            // 
            // returnDescription
            // 
            this.returnDescription.AutoSize = true;
            this.returnDescription.BackColor = System.Drawing.Color.White;
            this.returnDescription.ForeColor = System.Drawing.Color.DimGray;
            this.returnDescription.Location = new System.Drawing.Point(94, 28);
            this.returnDescription.Name = "returnDescription";
            this.returnDescription.Size = new System.Drawing.Size(138, 13);
            this.returnDescription.TabIndex = 2;
            this.returnDescription.Text = "All about the return service";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(3, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = " UPS Returns® :";
            // 
            // UpsOltReturnsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.returnContents);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.returnService);
            this.Controls.Add(this.returnEmailPanel);
            this.Controls.Add(this.returnDescription);
            this.Controls.Add(this.label3);
            this.Name = "UpsOltReturnsControl";
            this.Size = new System.Drawing.Size(353, 110);
            this.returnEmailPanel.ResumeLayout(false);
            this.returnEmailPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Templates.Tokens.TemplateTokenTextBox returnContents;
        private System.Windows.Forms.Label label5;
        private UI.Controls.MultiValueComboBox returnService;
        private System.Windows.Forms.Panel returnEmailPanel;
        private UI.Controls.MultiValueTextBox returnUndeliverableEmail;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label returnDescription;
        private System.Windows.Forms.Label label3;
    }
}
