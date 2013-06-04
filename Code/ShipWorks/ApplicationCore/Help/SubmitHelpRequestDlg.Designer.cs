namespace ShipWorks.ApplicationCore.Help
{
    partial class SubmitHelpRequestDlg
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.submit = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.labelStores = new System.Windows.Forms.Label();
            this.comboStores = new System.Windows.Forms.ComboBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelPhone = new System.Windows.Forms.Label();
            this.labelEmail = new System.Windows.Forms.Label();
            this.labelCompany = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.description = new System.Windows.Forms.TextBox();
            this.labelUrgency = new System.Windows.Forms.Label();
            this.comboUrgency = new System.Windows.Forms.ComboBox();
            this.labelConctact = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.TextBox();
            this.company = new System.Windows.Forms.TextBox();
            this.email = new System.Windows.Forms.TextBox();
            this.phone = new System.Windows.Forms.TextBox();
            this.panelStores = new System.Windows.Forms.Panel();
            this.panelStores.SuspendLayout();
            this.SuspendLayout();
            // 
            // submit
            // 
            this.submit.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.submit.Location = new System.Drawing.Point(290, 516);
            this.submit.Name = "submit";
            this.submit.Size = new System.Drawing.Size(75, 23);
            this.submit.TabIndex = 14;
            this.submit.Text = "Submit";
            this.submit.UseVisualStyleBackColor = true;
            this.submit.Click += new System.EventHandler(this.OnSubmit);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(371, 516);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 15;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // labelStores
            // 
            this.labelStores.AutoSize = true;
            this.labelStores.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelStores.Location = new System.Drawing.Point(0, 11);
            this.labelStores.Name = "labelStores";
            this.labelStores.Size = new System.Drawing.Size(178, 13);
            this.labelStores.TabIndex = 1;
            this.labelStores.Text = "Which store do you need help with?";
            // 
            // comboStores
            // 
            this.comboStores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStores.FormattingEnabled = true;
            this.comboStores.Location = new System.Drawing.Point(19, 30);
            this.comboStores.Name = "comboStores";
            this.comboStores.Size = new System.Drawing.Size(219, 21);
            this.comboStores.TabIndex = 0;
            this.comboStores.SelectedIndexChanged += new System.EventHandler(this.OnChangeSelectedStore);
            // 
            // labelDescription
            // 
            this.labelDescription.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelDescription.AutoSize = true;
            this.labelDescription.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelDescription.Location = new System.Drawing.Point(12, 68);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(147, 13);
            this.labelDescription.TabIndex = 1;
            this.labelDescription.Text = "What do you need help with?";
            // 
            // labelPhone
            // 
            this.labelPhone.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelPhone.AutoSize = true;
            this.labelPhone.Location = new System.Drawing.Point(52, 466);
            this.labelPhone.Name = "labelPhone";
            this.labelPhone.Size = new System.Drawing.Size(41, 13);
            this.labelPhone.TabIndex = 12;
            this.labelPhone.Text = "Phone:";
            // 
            // labelEmail
            // 
            this.labelEmail.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(58, 439);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(35, 13);
            this.labelEmail.TabIndex = 10;
            this.labelEmail.Text = "Email:";
            // 
            // labelCompany
            // 
            this.labelCompany.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCompany.AutoSize = true;
            this.labelCompany.Location = new System.Drawing.Point(37, 412);
            this.labelCompany.Name = "labelCompany";
            this.labelCompany.Size = new System.Drawing.Size(56, 13);
            this.labelCompany.TabIndex = 8;
            this.labelCompany.Text = "Company:";
            // 
            // labelName
            // 
            this.labelName.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(31, 385);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(62, 13);
            this.labelName.TabIndex = 6;
            this.labelName.Text = "Your name:";
            // 
            // description
            // 
            this.description.AcceptsReturn = true;
            this.description.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.description.Location = new System.Drawing.Point(34, 86);
            this.description.Multiline = true;
            this.description.Name = "description";
            this.description.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.description.Size = new System.Drawing.Size(416, 191);
            this.description.TabIndex = 2;
            // 
            // labelUrgency
            // 
            this.labelUrgency.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelUrgency.AutoSize = true;
            this.labelUrgency.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelUrgency.Location = new System.Drawing.Point(12, 292);
            this.labelUrgency.Name = "labelUrgency";
            this.labelUrgency.Size = new System.Drawing.Size(98, 13);
            this.labelUrgency.TabIndex = 3;
            this.labelUrgency.Text = "How urgent is this?";
            // 
            // comboUrgency
            // 
            this.comboUrgency.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboUrgency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboUrgency.FormattingEnabled = true;
            this.comboUrgency.Location = new System.Drawing.Point(34, 313);
            this.comboUrgency.Name = "comboUrgency";
            this.comboUrgency.Size = new System.Drawing.Size(219, 21);
            this.comboUrgency.TabIndex = 4;
            // 
            // labelConctact
            // 
            this.labelConctact.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelConctact.AutoSize = true;
            this.labelConctact.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelConctact.Location = new System.Drawing.Point(12, 355);
            this.labelConctact.Name = "labelConctact";
            this.labelConctact.Size = new System.Drawing.Size(174, 13);
            this.labelConctact.TabIndex = 5;
            this.labelConctact.Text = "How can we get in touch with you?";
            // 
            // name
            // 
            this.name.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.name.Location = new System.Drawing.Point(99, 382);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(207, 21);
            this.name.TabIndex = 7;
            // 
            // company
            // 
            this.company.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.company.Location = new System.Drawing.Point(99, 409);
            this.company.Name = "company";
            this.company.Size = new System.Drawing.Size(207, 21);
            this.company.TabIndex = 9;
            // 
            // email
            // 
            this.email.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.email.Location = new System.Drawing.Point(99, 436);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(207, 21);
            this.email.TabIndex = 11;
            // 
            // phone
            // 
            this.phone.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.phone.Location = new System.Drawing.Point(99, 463);
            this.phone.Name = "phone";
            this.phone.Size = new System.Drawing.Size(207, 21);
            this.phone.TabIndex = 13;
            // 
            // panelStores
            // 
            this.panelStores.Controls.Add(this.labelStores);
            this.panelStores.Controls.Add(this.comboStores);
            this.panelStores.Location = new System.Drawing.Point(15, 2);
            this.panelStores.Name = "panelStores";
            this.panelStores.Size = new System.Drawing.Size(438, 58);
            this.panelStores.TabIndex = 0;
            // 
            // SubmitHelpRequestDlg
            // 
            this.AcceptButton = this.submit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(458, 551);
            this.Controls.Add(this.panelStores);
            this.Controls.Add(this.phone);
            this.Controls.Add(this.email);
            this.Controls.Add(this.company);
            this.Controls.Add(this.name);
            this.Controls.Add(this.labelConctact);
            this.Controls.Add(this.comboUrgency);
            this.Controls.Add(this.labelUrgency);
            this.Controls.Add(this.description);
            this.Controls.Add(this.labelPhone);
            this.Controls.Add(this.labelEmail);
            this.Controls.Add(this.labelCompany);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.submit);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SubmitHelpRequestDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Request Help";
            this.Load += new System.EventHandler(this.OnLoad);
            this.panelStores.ResumeLayout(false);
            this.panelStores.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button submit;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label labelStores;
        private System.Windows.Forms.ComboBox comboStores;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelPhone;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.Label labelCompany;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox description;
        private System.Windows.Forms.Label labelUrgency;
        private System.Windows.Forms.ComboBox comboUrgency;
        private System.Windows.Forms.Label labelConctact;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.TextBox company;
        private System.Windows.Forms.TextBox email;
        private System.Windows.Forms.TextBox phone;
        private System.Windows.Forms.Panel panelStores;
    }
}