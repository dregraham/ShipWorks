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
            this.damageValue = new ShipWorks.UI.Controls.MoneyTextBox();
            this.SuspendLayout();
            // 
            // itemNameLabel
            // 
            this.itemNameLabel.AutoSize = true;
            this.itemNameLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itemNameLabel.Location = new System.Drawing.Point(20, 58);
            this.itemNameLabel.Name = "itemNameLabel";
            this.itemNameLabel.Size = new System.Drawing.Size(63, 13);
            this.itemNameLabel.TabIndex = 5;
            this.itemNameLabel.Text = "Item Name:";
            // 
            // damageValueLabel
            // 
            this.damageValueLabel.AutoSize = true;
            this.damageValueLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.damageValueLabel.Location = new System.Drawing.Point(1, 32);
            this.damageValueLabel.Name = "damageValueLabel";
            this.damageValueLabel.Size = new System.Drawing.Size(79, 13);
            this.damageValueLabel.TabIndex = 4;
            this.damageValueLabel.Text = "Damage Value:";
            // 
            // claimTypeLabel
            // 
            this.claimTypeLabel.AutoSize = true;
            this.claimTypeLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.claimTypeLabel.Location = new System.Drawing.Point(19, 6);
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
            this.itemName.Location = new System.Drawing.Point(87, 55);
            this.itemName.Name = "itemName";
            this.itemName.Size = new System.Drawing.Size(180, 21);
            this.itemName.TabIndex = 2;
            // 
            // submitClaim
            // 
            this.submitClaim.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.submitClaim.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.submitClaim.Location = new System.Drawing.Point(192, 200);
            this.submitClaim.Name = "submitClaim";
            this.submitClaim.Size = new System.Drawing.Size(75, 23);
            this.submitClaim.TabIndex = 4;
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
            this.claimType.Location = new System.Drawing.Point(87, 3);
            this.claimType.Name = "claimType";
            this.claimType.Size = new System.Drawing.Size(180, 21);
            this.claimType.TabIndex = 0;
            // 
            // description
            // 
            this.description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.description.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.description.Location = new System.Drawing.Point(87, 81);
            this.description.MaxLength = 255;
            this.description.Multiline = true;
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(180, 113);
            this.description.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(20, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Description:";
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
            this.damageValue.Location = new System.Drawing.Point(87, 29);
            this.damageValue.Name = "damageValue";
            this.damageValue.Size = new System.Drawing.Size(180, 21);
            this.damageValue.TabIndex = 1;
            this.damageValue.Text = "$0.00";
            // 
            // InsuranceSubmitClaimControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
            this.Size = new System.Drawing.Size(287, 235);
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
    }
}
