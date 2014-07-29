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
            this.claimTypeLabel = new System.Windows.Forms.Label();
            this.damageValueLabel = new System.Windows.Forms.Label();
            this.itemNameLabel = new System.Windows.Forms.Label();
            this.claimType = new System.Windows.Forms.Label();
            this.damageValue = new System.Windows.Forms.Label();
            this.itemName = new System.Windows.Forms.Label();
            this.labelSubmissionDate = new System.Windows.Forms.Label();
            this.submittedOn = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // claimTypeLabel
            // 
            this.claimTypeLabel.AutoSize = true;
            this.claimTypeLabel.Location = new System.Drawing.Point(20, 3);
            this.claimTypeLabel.Name = "claimTypeLabel";
            this.claimTypeLabel.Size = new System.Drawing.Size(62, 13);
            this.claimTypeLabel.TabIndex = 0;
            this.claimTypeLabel.Text = "Claim Type:";
            // 
            // damageValueLabel
            // 
            this.damageValueLabel.AutoSize = true;
            this.damageValueLabel.Location = new System.Drawing.Point(4, 27);
            this.damageValueLabel.Name = "damageValueLabel";
            this.damageValueLabel.Size = new System.Drawing.Size(80, 13);
            this.damageValueLabel.TabIndex = 1;
            this.damageValueLabel.Text = "Damage Value:";
            // 
            // itemNameLabel
            // 
            this.itemNameLabel.AutoSize = true;
            this.itemNameLabel.Location = new System.Drawing.Point(20, 50);
            this.itemNameLabel.Name = "itemNameLabel";
            this.itemNameLabel.Size = new System.Drawing.Size(61, 13);
            this.itemNameLabel.TabIndex = 2;
            this.itemNameLabel.Text = "Item Name:";
            // 
            // claimType
            // 
            this.claimType.AutoSize = true;
            this.claimType.Location = new System.Drawing.Point(88, 3);
            this.claimType.Name = "claimType";
            this.claimType.Size = new System.Drawing.Size(0, 13);
            this.claimType.TabIndex = 3;
            // 
            // damageValue
            // 
            this.damageValue.AutoSize = true;
            this.damageValue.Location = new System.Drawing.Point(88, 27);
            this.damageValue.Name = "damageValue";
            this.damageValue.Size = new System.Drawing.Size(0, 13);
            this.damageValue.TabIndex = 4;
            // 
            // itemName
            // 
            this.itemName.AutoSize = true;
            this.itemName.Location = new System.Drawing.Point(88, 50);
            this.itemName.Name = "itemName";
            this.itemName.Size = new System.Drawing.Size(0, 13);
            this.itemName.TabIndex = 5;
            // 
            // labelSubmissionDate
            // 
            this.labelSubmissionDate.AutoSize = true;
            this.labelSubmissionDate.Location = new System.Drawing.Point(7, 72);
            this.labelSubmissionDate.Name = "labelSubmissionDate";
            this.labelSubmissionDate.Size = new System.Drawing.Size(74, 13);
            this.labelSubmissionDate.TabIndex = 6;
            this.labelSubmissionDate.Text = "Submitted On:";
            // 
            // submittedOn
            // 
            this.submittedOn.AutoSize = true;
            this.submittedOn.Location = new System.Drawing.Point(88, 72);
            this.submittedOn.Name = "submittedOn";
            this.submittedOn.Size = new System.Drawing.Size(0, 13);
            this.submittedOn.TabIndex = 7;
            // 
            // InsuranceViewClaimControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.submittedOn);
            this.Controls.Add(this.labelSubmissionDate);
            this.Controls.Add(this.itemName);
            this.Controls.Add(this.damageValue);
            this.Controls.Add(this.claimType);
            this.Controls.Add(this.itemNameLabel);
            this.Controls.Add(this.damageValueLabel);
            this.Controls.Add(this.claimTypeLabel);
            this.Name = "InsuranceViewClaimControl";
            this.Size = new System.Drawing.Size(332, 93);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label claimTypeLabel;
        private System.Windows.Forms.Label damageValueLabel;
        private System.Windows.Forms.Label itemNameLabel;
        private System.Windows.Forms.Label claimType;
        private System.Windows.Forms.Label damageValue;
        private System.Windows.Forms.Label itemName;
        private System.Windows.Forms.Label labelSubmissionDate;
        private System.Windows.Forms.Label submittedOn;
    }
}
