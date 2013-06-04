namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Actions
{
    partial class EbayMessageBuyerTaskEditor
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
            this.subjectLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.messageTextBox = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.copyMeCheckBox = new System.Windows.Forms.CheckBox();
            this.subjectTextBox = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.messageTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // subjectLabel
            // 
            this.subjectLabel.AutoSize = true;
            this.subjectLabel.Location = new System.Drawing.Point(9, 36);
            this.subjectLabel.Name = "subjectLabel";
            this.subjectLabel.Size = new System.Drawing.Size(47, 13);
            this.subjectLabel.TabIndex = 0;
            this.subjectLabel.Text = "&Subject:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "&Message:";
            // 
            // messageTextBox
            // 
            this.messageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.messageTextBox.Location = new System.Drawing.Point(3, 92);
            this.messageTextBox.MaxLength = 32767;
            this.messageTextBox.Multiline = true;
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.Size = new System.Drawing.Size(342, 69);
            this.messageTextBox.TabIndex = 3;
            // 
            // copyMeCheckBox
            // 
            this.copyMeCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.copyMeCheckBox.AutoSize = true;
            this.copyMeCheckBox.Location = new System.Drawing.Point(231, 69);
            this.copyMeCheckBox.Name = "copyMeCheckBox";
            this.copyMeCheckBox.Size = new System.Drawing.Size(117, 17);
            this.copyMeCheckBox.TabIndex = 4;
            this.copyMeCheckBox.Text = "&Send a Copy to Me";
            this.copyMeCheckBox.UseVisualStyleBackColor = true;
            this.copyMeCheckBox.CheckedChanged += new System.EventHandler(this.OnCopyCheckedChanged);
            // 
            // subjectTextBox
            // 
            this.subjectTextBox.Location = new System.Drawing.Point(62, 31);
            this.subjectTextBox.MaxLength = 32767;
            this.subjectTextBox.Name = "subjectTextBox";
            this.subjectTextBox.Size = new System.Drawing.Size(286, 21);
            this.subjectTextBox.TabIndex = 5;
            // 
            // messageTypeComboBox
            // 
            this.messageTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.messageTypeComboBox.FormattingEnabled = true;
            this.messageTypeComboBox.Location = new System.Drawing.Point(62, 4);
            this.messageTypeComboBox.Name = "messageTypeComboBox";
            this.messageTypeComboBox.Size = new System.Drawing.Size(283, 21);
            this.messageTypeComboBox.TabIndex = 6;
            this.messageTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.OnMessageTypeChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "&Type:";
            // 
            // EbayMessageBuyerTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.messageTypeComboBox);
            this.Controls.Add(this.subjectTextBox);
            this.Controls.Add(this.copyMeCheckBox);
            this.Controls.Add(this.messageTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.subjectLabel);
            this.Name = "EbayMessageBuyerTaskEditor";
            this.Size = new System.Drawing.Size(351, 164);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label subjectLabel;
        private System.Windows.Forms.Label label2;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox messageTextBox;
        private System.Windows.Forms.CheckBox copyMeCheckBox;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox subjectTextBox;
        private System.Windows.Forms.ComboBox messageTypeComboBox;
        private System.Windows.Forms.Label label1;
    }
}
