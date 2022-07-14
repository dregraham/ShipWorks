namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    partial class AmazonSFPShippingSettingsControl
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
            this.AmazonShippingTitle = new ShipWorks.UI.Controls.SectionTitle();
            this.CredentialsButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // AmazonShippingTitle
            // 
            this.AmazonShippingTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.AmazonShippingTitle.Location = new System.Drawing.Point(0, 0);
            this.AmazonShippingTitle.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.AmazonShippingTitle.Name = "AmazonShippingTitle";
            this.AmazonShippingTitle.Size = new System.Drawing.Size(496, 22);
            this.AmazonShippingTitle.TabIndex = 17;
            this.AmazonShippingTitle.Text = "Amazon Shipping";
            // 
            // CredentialsButton
            // 
            this.CredentialsButton.Location = new System.Drawing.Point(3, 58);
            this.CredentialsButton.Name = "CredentialsButton";
            this.CredentialsButton.Size = new System.Drawing.Size(128, 23);
            this.CredentialsButton.TabIndex = 18;
            this.CredentialsButton.Text = "Create Amazon Token";
            this.CredentialsButton.UseVisualStyleBackColor = true;
            this.CredentialsButton.Click += new System.EventHandler(this.CredentialsButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 28);
            this.label1.MaximumSize = new System.Drawing.Size(420, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(388, 26);
            this.label1.TabIndex = 19;
            this.label1.Text = "If the store does not have a carrier setup, create the credentials. " +
                               "If there is already a carrier configured, update the credentials.";
            // 
            // AmazonSFPShippingSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CredentialsButton);
            this.Controls.Add(this.AmazonShippingTitle);
            this.Name = "AmazonSFPShippingSettingsControl";
            this.Size = new System.Drawing.Size(496, 110);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Controls.SectionTitle AmazonShippingTitle;
        private System.Windows.Forms.Button CredentialsButton;
        private System.Windows.Forms.Label label1;
    }
}
