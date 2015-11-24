namespace ShipWorks.Stores.Platforms.PayPal
{
    partial class PayPalCredentialsControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.credentialType = new System.Windows.Forms.ComboBox();
            this.panelCertificate = new System.Windows.Forms.Panel();
            this.importButton = new System.Windows.Forms.Button();
            this.certificatePasswordTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.certificateTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panelSignature = new System.Windows.Forms.Panel();
            this.signatureTextBox = new System.Windows.Forms.TextBox();
            this.signaturePasswordTextBox = new System.Windows.Forms.TextBox();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.linkHelp = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            this.panelCertificate.SuspendLayout();
            this.panelSignature.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(238, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "to learn how to get your PayPal API credentials.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Credential Type:";
            // 
            // credentialType
            // 
            this.credentialType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.credentialType.FormattingEnabled = true;
            this.credentialType.Items.AddRange(new object[] {
            "Signature (Recommended)",
            "SSL client-side certificate"});
            this.credentialType.Location = new System.Drawing.Point(97, 5);
            this.credentialType.Name = "credentialType";
            this.credentialType.Size = new System.Drawing.Size(203, 21);
            this.credentialType.TabIndex = 2;
            this.credentialType.SelectedIndexChanged += new System.EventHandler(this.OnCredentialTypeChanged);
            // 
            // panelCertificate
            // 
            this.panelCertificate.Controls.Add(this.importButton);
            this.panelCertificate.Controls.Add(this.certificatePasswordTextBox);
            this.panelCertificate.Controls.Add(this.label4);
            this.panelCertificate.Controls.Add(this.certificateTextBox);
            this.panelCertificate.Controls.Add(this.label3);
            this.panelCertificate.Location = new System.Drawing.Point(2, 26);
            this.panelCertificate.Name = "panelCertificate";
            this.panelCertificate.Size = new System.Drawing.Size(405, 62);
            this.panelCertificate.TabIndex = 3;
            // 
            // importButton
            // 
            this.importButton.Location = new System.Drawing.Point(305, 4);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(75, 23);
            this.importButton.TabIndex = 1;
            this.importButton.Text = "&Import...";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.OnImportClick);
            // 
            // certificatePasswordTextBox
            // 
            this.certificatePasswordTextBox.Location = new System.Drawing.Point(96, 32);
            this.certificatePasswordTextBox.Name = "certificatePasswordTextBox";
            this.certificatePasswordTextBox.Size = new System.Drawing.Size(203, 21);
            this.certificatePasswordTextBox.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "API Password:";
            // 
            // certificateTextBox
            // 
            this.certificateTextBox.Location = new System.Drawing.Point(96, 6);
            this.certificateTextBox.Name = "certificateTextBox";
            this.certificateTextBox.ReadOnly = true;
            this.certificateTextBox.Size = new System.Drawing.Size(203, 21);
            this.certificateTextBox.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "API Certificate:";
            // 
            // panelSignature
            // 
            this.panelSignature.Controls.Add(this.signatureTextBox);
            this.panelSignature.Controls.Add(this.signaturePasswordTextBox);
            this.panelSignature.Controls.Add(this.usernameTextBox);
            this.panelSignature.Controls.Add(this.label7);
            this.panelSignature.Controls.Add(this.label6);
            this.panelSignature.Controls.Add(this.label5);
            this.panelSignature.Location = new System.Drawing.Point(1, 27);
            this.panelSignature.Name = "panelSignature";
            this.panelSignature.Size = new System.Drawing.Size(467, 83);
            this.panelSignature.TabIndex = 4;
            this.panelSignature.Visible = false;
            // 
            // signatureTextBox
            // 
            this.signatureTextBox.Location = new System.Drawing.Point(97, 54);
            this.signatureTextBox.Name = "signatureTextBox";
            this.signatureTextBox.Size = new System.Drawing.Size(296, 21);
            this.signatureTextBox.TabIndex = 5;
            // 
            // signaturePasswordTextBox
            // 
            this.signaturePasswordTextBox.Location = new System.Drawing.Point(97, 29);
            this.signaturePasswordTextBox.Name = "signaturePasswordTextBox";
            this.signaturePasswordTextBox.Size = new System.Drawing.Size(296, 21);
            this.signaturePasswordTextBox.TabIndex = 4;
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Location = new System.Drawing.Point(97, 5);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(296, 21);
            this.usernameTextBox.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(36, 57);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Signature:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 34);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "API Password:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "API Username:";
            // 
            // linkHelp
            // 
            this.linkHelp.AutoSize = true;
            this.linkHelp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkHelp.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkHelp.ForeColor = System.Drawing.Color.Blue;
            this.linkHelp.Location = new System.Drawing.Point(6, 114);
            this.linkHelp.Name = "linkHelp";
            this.linkHelp.Size = new System.Drawing.Size(53, 13);
            this.linkHelp.TabIndex = 5;
            this.linkHelp.Text = "Click here";
            this.linkHelp.Url = "http://www.interapptive.com/shipworks/help";
            // 
            // PayPalCredentialsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkHelp);
            this.Controls.Add(this.credentialType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panelSignature);
            this.Controls.Add(this.panelCertificate);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(427, 142);
            this.Name = "PayPalCredentialsControl";
            this.Size = new System.Drawing.Size(479, 142);
            this.panelCertificate.ResumeLayout(false);
            this.panelCertificate.PerformLayout();
            this.panelSignature.ResumeLayout(false);
            this.panelSignature.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox credentialType;
        private System.Windows.Forms.Panel panelCertificate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.TextBox certificatePasswordTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox certificateTextBox;
        private System.Windows.Forms.Panel panelSignature;
        private System.Windows.Forms.TextBox signaturePasswordTextBox;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox signatureTextBox;
        private ShipWorks.ApplicationCore.Interaction.HelpLink linkHelp;
    }
}
