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
            this.title = new System.Windows.Forms.Label();
            this.instructions = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.email = new System.Windows.Forms.TextBox();
            this.emailLabel = new System.Windows.Forms.Label();
            this.description = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.claimType = new System.Windows.Forms.ComboBox();
            this.submitClaim = new System.Windows.Forms.Button();
            this.itemName = new System.Windows.Forms.TextBox();
            this.itemNameLabel = new System.Windows.Forms.Label();
            this.damageValueLabel = new System.Windows.Forms.Label();
            this.claimTypeLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.damageValue = new ShipWorks.UI.Controls.MoneyTextBox();
            this.insurshipInformation = new ShipWorks.Shipping.Insurance.InsureShipQuestionsControl();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            //
            // title
            //
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.Location = new System.Drawing.Point(-3, -1);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(91, 13);
            this.title.TabIndex = 15;
            this.title.Text = "Submit a Claim";
            //
            // instructions
            //
            this.instructions.Location = new System.Drawing.Point(-3, 15);
            this.instructions.Name = "instructions";
            this.instructions.Size = new System.Drawing.Size(206, 15);
            this.instructions.TabIndex = 16;
            this.instructions.Text = "Fill out the form below to submit a claim. ";
            //
            // panel1
            //
            this.panel1.Controls.Add(this.email);
            this.panel1.Controls.Add(this.emailLabel);
            this.panel1.Controls.Add(this.description);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.damageValue);
            this.panel1.Controls.Add(this.claimType);
            this.panel1.Controls.Add(this.submitClaim);
            this.panel1.Controls.Add(this.itemName);
            this.panel1.Controls.Add(this.itemNameLabel);
            this.panel1.Controls.Add(this.damageValueLabel);
            this.panel1.Controls.Add(this.claimTypeLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 80);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(435, 289);
            this.panel1.TabIndex = 29;
            //
            // email
            //
            this.email.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.email.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.email.Location = new System.Drawing.Point(93, 110);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(328, 21);
            this.email.TabIndex = 18;
            //
            // emailLabel
            //
            this.emailLabel.AutoSize = true;
            this.emailLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emailLabel.Location = new System.Drawing.Point(3, 113);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(83, 13);
            this.emailLabel.TabIndex = 26;
            this.emailLabel.Text = "Company Email:";
            //
            // description
            //
            this.description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.description.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.description.Location = new System.Drawing.Point(93, 137);
            this.description.MaxLength = 255;
            this.description.Multiline = true;
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(328, 113);
            this.description.TabIndex = 20;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(22, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Description:";
            //
            // claimType
            //
            this.claimType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.claimType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.claimType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.claimType.FormattingEnabled = true;
            this.claimType.Location = new System.Drawing.Point(93, 31);
            this.claimType.Name = "claimType";
            this.claimType.Size = new System.Drawing.Size(328, 21);
            this.claimType.TabIndex = 15;
            //
            // submitClaim
            //
            this.submitClaim.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.submitClaim.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.submitClaim.Location = new System.Drawing.Point(346, 256);
            this.submitClaim.Name = "submitClaim";
            this.submitClaim.Size = new System.Drawing.Size(75, 23);
            this.submitClaim.TabIndex = 22;
            this.submitClaim.Text = "Submit Claim";
            this.submitClaim.UseVisualStyleBackColor = true;
            this.submitClaim.Click += new System.EventHandler(this.OnSubmitClaimClick);
            //
            // itemName
            //
            this.itemName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.itemName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itemName.Location = new System.Drawing.Point(93, 83);
            this.itemName.Name = "itemName";
            this.itemName.Size = new System.Drawing.Size(328, 21);
            this.itemName.TabIndex = 17;
            //
            // itemNameLabel
            //
            this.itemNameLabel.AutoSize = true;
            this.itemNameLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itemNameLabel.Location = new System.Drawing.Point(23, 86);
            this.itemNameLabel.Name = "itemNameLabel";
            this.itemNameLabel.Size = new System.Drawing.Size(63, 13);
            this.itemNameLabel.TabIndex = 23;
            this.itemNameLabel.Text = "Item Name:";
            //
            // damageValueLabel
            //
            this.damageValueLabel.AutoSize = true;
            this.damageValueLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.damageValueLabel.Location = new System.Drawing.Point(7, 60);
            this.damageValueLabel.Name = "damageValueLabel";
            this.damageValueLabel.Size = new System.Drawing.Size(79, 13);
            this.damageValueLabel.TabIndex = 21;
            this.damageValueLabel.Text = "Damage Value:";
            //
            // claimTypeLabel
            //
            this.claimTypeLabel.AutoSize = true;
            this.claimTypeLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.claimTypeLabel.Location = new System.Drawing.Point(23, 34);
            this.claimTypeLabel.Name = "claimTypeLabel";
            this.claimTypeLabel.Size = new System.Drawing.Size(63, 13);
            this.claimTypeLabel.TabIndex = 19;
            this.claimTypeLabel.Text = "Claim Type:";
            //
            // panel2
            //
            this.panel2.Controls.Add(this.title);
            this.panel2.Controls.Add(this.instructions);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(435, 38);
            this.panel2.TabIndex = 30;
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
            this.damageValue.Location = new System.Drawing.Point(93, 57);
            this.damageValue.Name = "damageValue";
            this.damageValue.Size = new System.Drawing.Size(328, 21);
            this.damageValue.TabIndex = 16;
            this.damageValue.Text = "$0.00";
            //
            // insurshipInformation
            //
            this.insurshipInformation.BackColor = System.Drawing.Color.Transparent;
            this.insurshipInformation.Dock = System.Windows.Forms.DockStyle.Top;
            this.insurshipInformation.Location = new System.Drawing.Point(0, 38);
            this.insurshipInformation.Margin = new System.Windows.Forms.Padding(0);
            this.insurshipInformation.Name = "insurshipInformation";
            this.insurshipInformation.Size = new System.Drawing.Size(435, 42);
            this.insurshipInformation.TabIndex = 28;
            //
            // InsuranceSubmitClaimControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.insurshipInformation);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "InsuranceSubmitClaimControl";
            this.Size = new System.Drawing.Size(435, 372);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Label instructions;
        private InsureShipQuestionsControl insurshipInformation;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox email;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.TextBox description;
        private System.Windows.Forms.Label label1;
        private UI.Controls.MoneyTextBox damageValue;
        private System.Windows.Forms.ComboBox claimType;
        private System.Windows.Forms.Button submitClaim;
        private System.Windows.Forms.TextBox itemName;
        private System.Windows.Forms.Label itemNameLabel;
        private System.Windows.Forms.Label damageValueLabel;
        private System.Windows.Forms.Label claimTypeLabel;
        private System.Windows.Forms.Panel panel2;
    }
}
