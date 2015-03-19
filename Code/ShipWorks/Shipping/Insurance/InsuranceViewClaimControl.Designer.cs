namespace ShipWorks.Shipping.Insurance
{
    partial class InsuranceViewClaimControl
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.email = new System.Windows.Forms.Label();
            this.emailLabel = new System.Windows.Forms.Label();
            this.checkStatus = new System.Windows.Forms.Button();
            this.claimStatus = new System.Windows.Forms.TextBox();
            this.claimStatusLabel = new System.Windows.Forms.Label();
            this.description = new System.Windows.Forms.Label();
            this.claimID = new System.Windows.Forms.Label();
            this.claimIDLabel = new System.Windows.Forms.Label();
            this.submittedOn = new System.Windows.Forms.Label();
            this.labelSubmissionDate = new System.Windows.Forms.Label();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.itemName = new System.Windows.Forms.Label();
            this.damageValue = new System.Windows.Forms.Label();
            this.claimType = new System.Windows.Forms.Label();
            this.itemNameLabel = new System.Windows.Forms.Label();
            this.damageValueLabel = new System.Windows.Forms.Label();
            this.claimTypeLabel = new System.Windows.Forms.Label();
            this.richTextBox1 = new ShipWorks.Shipping.Insurance.InsureShipQuestionsControl();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.email);
            this.panel1.Controls.Add(this.emailLabel);
            this.panel1.Controls.Add(this.checkStatus);
            this.panel1.Controls.Add(this.claimStatus);
            this.panel1.Controls.Add(this.claimStatusLabel);
            this.panel1.Controls.Add(this.description);
            this.panel1.Controls.Add(this.claimID);
            this.panel1.Controls.Add(this.claimIDLabel);
            this.panel1.Controls.Add(this.submittedOn);
            this.panel1.Controls.Add(this.labelSubmissionDate);
            this.panel1.Controls.Add(this.descriptionLabel);
            this.panel1.Controls.Add(this.itemName);
            this.panel1.Controls.Add(this.damageValue);
            this.panel1.Controls.Add(this.claimType);
            this.panel1.Controls.Add(this.itemNameLabel);
            this.panel1.Controls.Add(this.damageValueLabel);
            this.panel1.Controls.Add(this.claimTypeLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 43);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(420, 267);
            this.panel1.TabIndex = 26;
            // 
            // email
            // 
            this.email.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.email.AutoEllipsis = true;
            this.email.AutoSize = true;
            this.email.Location = new System.Drawing.Point(91, 159);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(128, 13);
            this.email.TabIndex = 38;
            this.email.Text = "company email goes here";
            // 
            // emailLabel
            // 
            this.emailLabel.AutoSize = true;
            this.emailLabel.Location = new System.Drawing.Point(3, 159);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(83, 13);
            this.emailLabel.TabIndex = 37;
            this.emailLabel.Text = "Company Email:";
            // 
            // checkStatus
            // 
            this.checkStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkStatus.Location = new System.Drawing.Point(308, 55);
            this.checkStatus.Name = "checkStatus";
            this.checkStatus.Size = new System.Drawing.Size(109, 23);
            this.checkStatus.TabIndex = 36;
            this.checkStatus.Text = "Check Status";
            this.checkStatus.UseVisualStyleBackColor = true;
            this.checkStatus.Click += new System.EventHandler(this.OnStatusButtonClick);
            // 
            // claimStatus
            // 
            this.claimStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.claimStatus.Location = new System.Drawing.Point(91, 57);
            this.claimStatus.Name = "claimStatus";
            this.claimStatus.ReadOnly = true;
            this.claimStatus.Size = new System.Drawing.Size(210, 21);
            this.claimStatus.TabIndex = 35;
            this.claimStatus.Text = "claim status goes here";
            // 
            // claimStatusLabel
            // 
            this.claimStatusLabel.AutoSize = true;
            this.claimStatusLabel.Location = new System.Drawing.Point(44, 60);
            this.claimStatusLabel.Name = "claimStatusLabel";
            this.claimStatusLabel.Size = new System.Drawing.Size(42, 13);
            this.claimStatusLabel.TabIndex = 34;
            this.claimStatusLabel.Text = "Status:";
            // 
            // description
            // 
            this.description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.description.Location = new System.Drawing.Point(91, 181);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(319, 74);
            this.description.TabIndex = 33;
            this.description.Text = "description text goes here";
            // 
            // claimID
            // 
            this.claimID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.claimID.AutoSize = true;
            this.claimID.Location = new System.Drawing.Point(91, 10);
            this.claimID.Name = "claimID";
            this.claimID.Size = new System.Drawing.Size(95, 13);
            this.claimID.TabIndex = 32;
            this.claimID.Text = "claim ID goes here";
            // 
            // claimIDLabel
            // 
            this.claimIDLabel.AutoSize = true;
            this.claimIDLabel.Location = new System.Drawing.Point(36, 10);
            this.claimIDLabel.Name = "claimIDLabel";
            this.claimIDLabel.Size = new System.Drawing.Size(50, 13);
            this.claimIDLabel.TabIndex = 31;
            this.claimIDLabel.Text = "Claim ID:";
            // 
            // submittedOn
            // 
            this.submittedOn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.submittedOn.AutoSize = true;
            this.submittedOn.Location = new System.Drawing.Point(91, 35);
            this.submittedOn.Name = "submittedOn";
            this.submittedOn.Size = new System.Drawing.Size(134, 13);
            this.submittedOn.TabIndex = 30;
            this.submittedOn.Text = "submission date goes here";
            // 
            // labelSubmissionDate
            // 
            this.labelSubmissionDate.AutoSize = true;
            this.labelSubmissionDate.Location = new System.Drawing.Point(10, 35);
            this.labelSubmissionDate.Name = "labelSubmissionDate";
            this.labelSubmissionDate.Size = new System.Drawing.Size(76, 13);
            this.labelSubmissionDate.TabIndex = 29;
            this.labelSubmissionDate.Text = "Submitted On:";
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(22, 181);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(64, 13);
            this.descriptionLabel.TabIndex = 28;
            this.descriptionLabel.Text = "Description:";
            // 
            // itemName
            // 
            this.itemName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.itemName.AutoEllipsis = true;
            this.itemName.AutoSize = true;
            this.itemName.Location = new System.Drawing.Point(91, 135);
            this.itemName.Name = "itemName";
            this.itemName.Size = new System.Drawing.Size(107, 13);
            this.itemName.TabIndex = 27;
            this.itemName.Text = "item name goes here";
            // 
            // damageValue
            // 
            this.damageValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.damageValue.AutoSize = true;
            this.damageValue.Location = new System.Drawing.Point(91, 110);
            this.damageValue.Name = "damageValue";
            this.damageValue.Size = new System.Drawing.Size(125, 13);
            this.damageValue.TabIndex = 26;
            this.damageValue.Text = "damage value goes here";
            // 
            // claimType
            // 
            this.claimType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.claimType.AutoSize = true;
            this.claimType.Location = new System.Drawing.Point(91, 85);
            this.claimType.Name = "claimType";
            this.claimType.Size = new System.Drawing.Size(106, 13);
            this.claimType.TabIndex = 25;
            this.claimType.Text = "claim type goes here";
            // 
            // itemNameLabel
            // 
            this.itemNameLabel.AutoSize = true;
            this.itemNameLabel.Location = new System.Drawing.Point(23, 135);
            this.itemNameLabel.Name = "itemNameLabel";
            this.itemNameLabel.Size = new System.Drawing.Size(63, 13);
            this.itemNameLabel.TabIndex = 24;
            this.itemNameLabel.Text = "Item Name:";
            // 
            // damageValueLabel
            // 
            this.damageValueLabel.AutoSize = true;
            this.damageValueLabel.Location = new System.Drawing.Point(7, 110);
            this.damageValueLabel.Name = "damageValueLabel";
            this.damageValueLabel.Size = new System.Drawing.Size(79, 13);
            this.damageValueLabel.TabIndex = 23;
            this.damageValueLabel.Text = "Damage Value:";
            // 
            // claimTypeLabel
            // 
            this.claimTypeLabel.AutoSize = true;
            this.claimTypeLabel.Location = new System.Drawing.Point(23, 85);
            this.claimTypeLabel.Name = "claimTypeLabel";
            this.claimTypeLabel.Size = new System.Drawing.Size(63, 13);
            this.claimTypeLabel.TabIndex = 22;
            this.claimTypeLabel.Text = "Claim Type:";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.Transparent;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(420, 43);
            this.richTextBox1.TabIndex = 25;
            // 
            // InsuranceViewClaimControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.richTextBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "InsuranceViewClaimControl";
            this.Size = new System.Drawing.Size(420, 310);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private InsureShipQuestionsControl richTextBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label email;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.Button checkStatus;
        private System.Windows.Forms.TextBox claimStatus;
        private System.Windows.Forms.Label claimStatusLabel;
        private System.Windows.Forms.Label description;
        private System.Windows.Forms.Label claimID;
        private System.Windows.Forms.Label claimIDLabel;
        private System.Windows.Forms.Label submittedOn;
        private System.Windows.Forms.Label labelSubmissionDate;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Label itemName;
        private System.Windows.Forms.Label damageValue;
        private System.Windows.Forms.Label claimType;
        private System.Windows.Forms.Label itemNameLabel;
        private System.Windows.Forms.Label damageValueLabel;
        private System.Windows.Forms.Label claimTypeLabel;
    }
}
