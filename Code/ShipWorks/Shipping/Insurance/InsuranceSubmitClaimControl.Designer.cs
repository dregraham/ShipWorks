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
            this.damageValue = new ShipWorks.UI.Controls.MoneyTextBox();
            this.SuspendLayout();
            // 
            // itemNameLabel
            // 
            this.itemNameLabel.AutoSize = true;
            this.itemNameLabel.Location = new System.Drawing.Point(20, 58);
            this.itemNameLabel.Name = "itemNameLabel";
            this.itemNameLabel.Size = new System.Drawing.Size(61, 13);
            this.itemNameLabel.TabIndex = 5;
            this.itemNameLabel.Text = "Item Name:";
            // 
            // damageValueLabel
            // 
            this.damageValueLabel.AutoSize = true;
            this.damageValueLabel.Location = new System.Drawing.Point(1, 32);
            this.damageValueLabel.Name = "damageValueLabel";
            this.damageValueLabel.Size = new System.Drawing.Size(80, 13);
            this.damageValueLabel.TabIndex = 4;
            this.damageValueLabel.Text = "Damage Value:";
            // 
            // claimTypeLabel
            // 
            this.claimTypeLabel.AutoSize = true;
            this.claimTypeLabel.Location = new System.Drawing.Point(19, 6);
            this.claimTypeLabel.Name = "claimTypeLabel";
            this.claimTypeLabel.Size = new System.Drawing.Size(62, 13);
            this.claimTypeLabel.TabIndex = 3;
            this.claimTypeLabel.Text = "Claim Type:";
            // 
            // itemName
            // 
            this.itemName.Location = new System.Drawing.Point(87, 55);
            this.itemName.Name = "itemName";
            this.itemName.Size = new System.Drawing.Size(121, 20);
            this.itemName.TabIndex = 7;
            // 
            // submitClaim
            // 
            this.submitClaim.Location = new System.Drawing.Point(133, 81);
            this.submitClaim.Name = "submitClaim";
            this.submitClaim.Size = new System.Drawing.Size(75, 23);
            this.submitClaim.TabIndex = 9;
            this.submitClaim.Text = "Submit Claim";
            this.submitClaim.UseVisualStyleBackColor = true;
            this.submitClaim.Click += new System.EventHandler(this.OnSubmitClaimClick);
            // 
            // claimType
            // 
            this.claimType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.claimType.FormattingEnabled = true;
            this.claimType.Location = new System.Drawing.Point(87, 3);
            this.claimType.Name = "claimType";
            this.claimType.Size = new System.Drawing.Size(121, 21);
            this.claimType.TabIndex = 10;
            // 
            // damageValue
            // 
            this.damageValue.Amount = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.damageValue.IgnoreSet = false;
            this.damageValue.Location = new System.Drawing.Point(87, 29);
            this.damageValue.Name = "damageValue";
            this.damageValue.Size = new System.Drawing.Size(121, 20);
            this.damageValue.TabIndex = 11;
            this.damageValue.Text = "$0.00";
            // 
            // InsuranceSubmitClaimControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.damageValue);
            this.Controls.Add(this.claimType);
            this.Controls.Add(this.submitClaim);
            this.Controls.Add(this.itemName);
            this.Controls.Add(this.itemNameLabel);
            this.Controls.Add(this.damageValueLabel);
            this.Controls.Add(this.claimTypeLabel);
            this.Name = "InsuranceSubmitClaimControl";
            this.Size = new System.Drawing.Size(287, 131);
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
    }
}
