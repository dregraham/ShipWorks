namespace ShipWorks.Shipping.Insurance
{
    partial class InsuranceSubmitClaimControl
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
            this.itemNameLabel = new System.Windows.Forms.Label();
            this.damageValueLabel = new System.Windows.Forms.Label();
            this.claimTypeLabel = new System.Windows.Forms.Label();
            this.itemName = new System.Windows.Forms.TextBox();
            this.submitClaim = new System.Windows.Forms.Button();
            this.claimType = new System.Windows.Forms.ComboBox();
            this.description = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.email = new System.Windows.Forms.TextBox();
            this.emailLabel = new System.Windows.Forms.Label();
            this.submitClaimLink = new System.Windows.Forms.LinkLabel();
            this.damageValue = new ShipWorks.UI.Controls.MoneyTextBox();
            this.title = new System.Windows.Forms.Label();
            this.instructions = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // itemNameLabel
            // 
            this.itemNameLabel.AutoSize = true;
            this.itemNameLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itemNameLabel.Location = new System.Drawing.Point(26, 104);
            this.itemNameLabel.Name = "itemNameLabel";
            this.itemNameLabel.Size = new System.Drawing.Size(63, 13);
            this.itemNameLabel.TabIndex = 5;
            this.itemNameLabel.Text = "Item Name:";
            // 
            // damageValueLabel
            // 
            this.damageValueLabel.AutoSize = true;
            this.damageValueLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.damageValueLabel.Location = new System.Drawing.Point(7, 78);
            this.damageValueLabel.Name = "damageValueLabel";
            this.damageValueLabel.Size = new System.Drawing.Size(79, 13);
            this.damageValueLabel.TabIndex = 4;
            this.damageValueLabel.Text = "Damage Value:";
            // 
            // claimTypeLabel
            // 
            this.claimTypeLabel.AutoSize = true;
            this.claimTypeLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.claimTypeLabel.Location = new System.Drawing.Point(25, 52);
            this.claimTypeLabel.Name = "claimTypeLabel";
            this.claimTypeLabel.Size = new System.Drawing.Size(63, 13);
            this.claimTypeLabel.TabIndex = 3;
            this.claimTypeLabel.Text = "Claim Type:";
            // 
            // itemName
            // 
            this.itemName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.itemName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itemName.Location = new System.Drawing.Point(93, 101);
            this.itemName.Name = "itemName";
            this.itemName.Size = new System.Drawing.Size(180, 21);
            this.itemName.TabIndex = 2;
            // 
            // submitClaim
            // 
            this.submitClaim.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.submitClaim.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.submitClaim.Location = new System.Drawing.Point(198, 274);
            this.submitClaim.Name = "submitClaim";
            this.submitClaim.Size = new System.Drawing.Size(75, 23);
            this.submitClaim.TabIndex = 5;
            this.submitClaim.Text = "Submit Claim";
            this.submitClaim.UseVisualStyleBackColor = true;
            this.submitClaim.Click += new System.EventHandler(this.OnSubmitClaimClick);
            // 
            // claimType
            // 
            this.claimType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.claimType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.claimType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.claimType.FormattingEnabled = true;
            this.claimType.Location = new System.Drawing.Point(93, 49);
            this.claimType.Name = "claimType";
            this.claimType.Size = new System.Drawing.Size(180, 21);
            this.claimType.TabIndex = 0;
            // 
            // description
            // 
            this.description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.description.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.description.Location = new System.Drawing.Point(93, 155);
            this.description.MaxLength = 255;
            this.description.Multiline = true;
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(180, 113);
            this.description.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Description:";
            // 
            // email
            // 
            this.email.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.email.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.email.Location = new System.Drawing.Point(93, 128);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(180, 21);
            this.email.TabIndex = 3;
            // 
            // emailLabel
            // 
            this.emailLabel.AutoSize = true;
            this.emailLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emailLabel.Location = new System.Drawing.Point(3, 131);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(83, 13);
            this.emailLabel.TabIndex = 14;
            this.emailLabel.Text = "Company Email:";
            // 
            // submitClaimLink
            // 
            this.submitClaimLink.AutoSize = true;
            this.submitClaimLink.Location = new System.Drawing.Point(-3, 31);
            this.submitClaimLink.Name = "submitClaimLink";
            this.submitClaimLink.Size = new System.Drawing.Size(224, 13);
            this.submitClaimLink.TabIndex = 6;
            this.submitClaimLink.TabStop = true;
            this.submitClaimLink.Text = "Having trouble? Click here to submit via Web.";
            this.submitClaimLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnSubmitClaimLinkClicked);
            // 
            // damageValue
            // 
            this.damageValue.Amount = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.damageValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.damageValue.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.damageValue.IgnoreSet = false;
            this.damageValue.Location = new System.Drawing.Point(93, 75);
            this.damageValue.Name = "damageValue";
            this.damageValue.Size = new System.Drawing.Size(180, 21);
            this.damageValue.TabIndex = 1;
            this.damageValue.Text = "$0.00";
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.Location = new System.Drawing.Point(-3, 0);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(91, 13);
            this.title.TabIndex = 15;
            this.title.Text = "Submit a Claim";
            // 
            // instructions
            // 
            this.instructions.Location = new System.Drawing.Point(-3, 16);
            this.instructions.Name = "instructions";
            this.instructions.Size = new System.Drawing.Size(278, 15);
            this.instructions.TabIndex = 16;
            this.instructions.Text = "Fill out the form below to submit a claim. ";
            // 
            // InsuranceSubmitClaimControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.submitClaimLink);
            this.Controls.Add(this.instructions);
            this.Controls.Add(this.title);
            this.Controls.Add(this.email);
            this.Controls.Add(this.emailLabel);
            this.Controls.Add(this.description);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.damageValue);
            this.Controls.Add(this.claimType);
            this.Controls.Add(this.submitClaim);
            this.Controls.Add(this.itemName);
            this.Controls.Add(this.itemNameLabel);
            this.Controls.Add(this.damageValueLabel);
            this.Controls.Add(this.claimTypeLabel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "InsuranceSubmitClaimControl";
            this.Size = new System.Drawing.Size(287, 304);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label itemNameLabel;
        private System.Windows.Forms.Label damageValueLabel;
        private System.Windows.Forms.Label claimTypeLabel;
        private System.Windows.Forms.TextBox itemName;
        private System.Windows.Forms.Button submitClaim;
        private System.Windows.Forms.ComboBox claimType;
        private UI.Controls.MoneyTextBox damageValue;
        private System.Windows.Forms.TextBox description;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox email;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.LinkLabel submitClaimLink;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Label instructions;
    }
}
