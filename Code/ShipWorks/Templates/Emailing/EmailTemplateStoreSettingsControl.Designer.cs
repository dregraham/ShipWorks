namespace ShipWorks.Templates.Emailing
{
    partial class EmailTemplateStoreSettingsControl
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
            this.components = new System.ComponentModel.Container();
            this.labelAccount = new System.Windows.Forms.Label();
            this.account = new System.Windows.Forms.ComboBox();
            this.labelTo = new System.Windows.Forms.Label();
            this.labelCc = new System.Windows.Forms.Label();
            this.labelBcc = new System.Windows.Forms.Label();
            this.bcc = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.cc = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.to = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.subject = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelSubject = new System.Windows.Forms.Label();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // labelAccount
            // 
            this.labelAccount.AutoSize = true;
            this.labelAccount.Location = new System.Drawing.Point(-1, 5);
            this.labelAccount.Name = "labelAccount";
            this.labelAccount.Size = new System.Drawing.Size(50, 13);
            this.labelAccount.TabIndex = 0;
            this.labelAccount.Text = "Account:";
            // 
            // account
            // 
            this.account.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.account.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.account.FormattingEnabled = true;
            this.account.Location = new System.Drawing.Point(55, 2);
            this.account.Name = "account";
            this.account.Size = new System.Drawing.Size(239, 21);
            this.account.TabIndex = 1;
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Location = new System.Drawing.Point(26, 32);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(23, 13);
            this.labelTo.TabIndex = 3;
            this.labelTo.Text = "To:";
            // 
            // labelCc
            // 
            this.labelCc.AutoSize = true;
            this.labelCc.Location = new System.Drawing.Point(24, 56);
            this.labelCc.Name = "labelCc";
            this.labelCc.Size = new System.Drawing.Size(25, 13);
            this.labelCc.TabIndex = 4;
            this.labelCc.Text = "CC:";
            // 
            // labelBcc
            // 
            this.labelBcc.AutoSize = true;
            this.labelBcc.Location = new System.Drawing.Point(18, 84);
            this.labelBcc.Name = "labelBcc";
            this.labelBcc.Size = new System.Drawing.Size(31, 13);
            this.labelBcc.TabIndex = 5;
            this.labelBcc.Text = "BCC:";
            // 
            // bcc
            // 
            this.bcc.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.bcc.Location = new System.Drawing.Point(55, 82);
            this.bcc.MaxLength = 32767;
            this.bcc.Name = "bcc";
            this.bcc.Size = new System.Drawing.Size(239, 21);
            this.bcc.TabIndex = 9;
            this.bcc.TokenUsage = ShipWorks.Templates.Tokens.TokenUsage.EmailAddress;
            // 
            // cc
            // 
            this.cc.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cc.Location = new System.Drawing.Point(55, 56);
            this.cc.MaxLength = 32767;
            this.cc.Name = "cc";
            this.cc.Size = new System.Drawing.Size(239, 21);
            this.cc.TabIndex = 8;
            this.cc.TokenUsage = ShipWorks.Templates.Tokens.TokenUsage.EmailAddress;
            // 
            // to
            // 
            this.to.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.to.Location = new System.Drawing.Point(55, 29);
            this.to.MaxLength = 32767;
            this.to.Name = "to";
            this.to.Size = new System.Drawing.Size(239, 21);
            this.to.TabIndex = 7;
            this.to.TokenUsage = ShipWorks.Templates.Tokens.TokenUsage.EmailAddress;
            // 
            // subject
            // 
            this.subject.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.subject.Location = new System.Drawing.Point(55, 109);
            this.subject.MaxLength = 32767;
            this.fieldLengthProvider.SetMaxLengthSource(this.subject, ShipWorks.Data.Utility.EntityFieldLengthSource.EmailSubject);
            this.subject.Name = "subject";
            this.subject.Size = new System.Drawing.Size(239, 21);
            this.subject.TabIndex = 11;
            this.subject.TokenUsage = ShipWorks.Templates.Tokens.TokenUsage.EmailSubject;
            // 
            // labelSubject
            // 
            this.labelSubject.AutoSize = true;
            this.labelSubject.Location = new System.Drawing.Point(3, 111);
            this.labelSubject.Name = "labelSubject";
            this.labelSubject.Size = new System.Drawing.Size(47, 13);
            this.labelSubject.TabIndex = 10;
            this.labelSubject.Text = "Subject:";
            // 
            // EmailTemplateStoreSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.subject);
            this.Controls.Add(this.labelSubject);
            this.Controls.Add(this.bcc);
            this.Controls.Add(this.cc);
            this.Controls.Add(this.to);
            this.Controls.Add(this.labelBcc);
            this.Controls.Add(this.labelCc);
            this.Controls.Add(this.labelTo);
            this.Controls.Add(this.account);
            this.Controls.Add(this.labelAccount);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "EmailTemplateStoreSettingsControl";
            this.Size = new System.Drawing.Size(297, 133);
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAccount;
        private System.Windows.Forms.ComboBox account;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.Label labelCc;
        private System.Windows.Forms.Label labelBcc;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox to;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox cc;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox bcc;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox subject;
        private System.Windows.Forms.Label labelSubject;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
    }
}
