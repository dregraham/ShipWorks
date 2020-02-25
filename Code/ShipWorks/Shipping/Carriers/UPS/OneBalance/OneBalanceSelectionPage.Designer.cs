namespace ShipWorks.Shipping.Carriers.UPS.OneBalance
{
    partial class OneBalanceSelectionPage
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
            this.oneBalanceDescription = new System.Windows.Forms.Label();
            this.oneBalanceSelectionText = new System.Windows.Forms.Label();
            this.createOneBalanceOption = new System.Windows.Forms.RadioButton();
            this.createStandardOption = new System.Windows.Forms.RadioButton();
            this.oneBalanceLink = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // oneBalanceDescription
            // 
            this.oneBalanceDescription.Location = new System.Drawing.Point(14, 14);
            this.oneBalanceDescription.Name = "oneBalanceDescription";
            this.oneBalanceDescription.Size = new System.Drawing.Size(542, 33);
            this.oneBalanceDescription.TabIndex = 0;
            this.oneBalanceDescription.Text = "ShipWorks offers savings of up to 55% off regular rates on select UPS services, a" +
    "s well as waived fuel and residential surcharges, through ShipWorks One Balance™" +
    ".";
            // 
            // oneBalanceSelectionText
            // 
            this.oneBalanceSelectionText.Location = new System.Drawing.Point(14, 60);
            this.oneBalanceSelectionText.Name = "oneBalanceSelectionText";
            this.oneBalanceSelectionText.Size = new System.Drawing.Size(542, 32);
            this.oneBalanceSelectionText.TabIndex = 1;
            this.oneBalanceSelectionText.Text = "Would you like to create your ShipWorks One Balance account now, to prepay for UP" +
    "S labels and unlock these rates?";
            // 
            // createOneBalanceOption
            // 
            this.createOneBalanceOption.AutoSize = true;
            this.createOneBalanceOption.Location = new System.Drawing.Point(44, 95);
            this.createOneBalanceOption.Name = "createOneBalanceOption";
            this.createOneBalanceOption.Size = new System.Drawing.Size(137, 17);
            this.createOneBalanceOption.TabIndex = 2;
            this.createOneBalanceOption.TabStop = true;
            this.createOneBalanceOption.Text = "Yes! I\'m ready to save.";
            this.createOneBalanceOption.UseVisualStyleBackColor = true;
            // 
            // createStandardOption
            // 
            this.createStandardOption.AutoSize = true;
            this.createStandardOption.Location = new System.Drawing.Point(44, 119);
            this.createStandardOption.Name = "createStandardOption";
            this.createStandardOption.Size = new System.Drawing.Size(238, 17);
            this.createStandardOption.TabIndex = 3;
            this.createStandardOption.TabStop = true;
            this.createStandardOption.Text = "Not now. I\'ll create a standard UPS account.";
            this.createStandardOption.UseVisualStyleBackColor = true;
            // 
            // oneBalanceLink
            // 
            this.oneBalanceLink.AutoSize = true;
            this.oneBalanceLink.Location = new System.Drawing.Point(299, 27);
            this.oneBalanceLink.Name = "oneBalanceLink";
            this.oneBalanceLink.Size = new System.Drawing.Size(61, 13);
            this.oneBalanceLink.TabIndex = 4;
            this.oneBalanceLink.TabStop = true;
            this.oneBalanceLink.Text = "Learn More";
            // 
            // OneBalanceSelectionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.oneBalanceLink);
            this.Controls.Add(this.createStandardOption);
            this.Controls.Add(this.createOneBalanceOption);
            this.Controls.Add(this.oneBalanceSelectionText);
            this.Controls.Add(this.oneBalanceDescription);
            this.Description = "Setup ShipWorks to work with your UPS account.";
            this.Name = "OneBalanceSelectionPage";
            this.Size = new System.Drawing.Size(579, 474);
            this.Title = "Account Registration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label oneBalanceDescription;
        private System.Windows.Forms.Label oneBalanceSelectionText;
        private System.Windows.Forms.RadioButton createOneBalanceOption;
        private System.Windows.Forms.RadioButton createStandardOption;
        private System.Windows.Forms.LinkLabel oneBalanceLink;
    }
}
