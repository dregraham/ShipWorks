namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    partial class ChannelAdvisorAmazonSettingsControl
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
            this.merchantID = new System.Windows.Forms.TextBox();
            this.authToken = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.countries = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.helpLink1 = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            this.AmazonShippingTitle = new ShipWorks.UI.Controls.SectionTitle();
            this.SuspendLayout();
            // 
            // merchantID
            // 
            this.merchantID.Location = new System.Drawing.Point(75, 55);
            this.merchantID.Name = "merchantID";
            this.merchantID.Size = new System.Drawing.Size(208, 20);
            this.merchantID.TabIndex = 67;
            // 
            // authToken
            // 
            this.authToken.Location = new System.Drawing.Point(75, 81);
            this.authToken.Name = "authToken";
            this.authToken.Size = new System.Drawing.Size(208, 20);
            this.authToken.TabIndex = 68;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 70;
            this.label4.Text = "Auth Token:";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(19, 58);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(50, 13);
            this.label27.TabIndex = 69;
            this.label27.Text = "Seller ID:";
            // 
            // countries
            // 
            this.countries.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.countries.FormattingEnabled = true;
            this.countries.Location = new System.Drawing.Point(75, 28);
            this.countries.Name = "countries";
            this.countries.Size = new System.Drawing.Size(208, 21);
            this.countries.TabIndex = 71;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 72;
            this.label1.Text = "Api Region:";
            // 
            // helpLink1
            // 
            this.helpLink1.AutoSize = true;
            this.helpLink1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.helpLink1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.helpLink1.ForeColor = System.Drawing.Color.Blue;
            this.helpLink1.Location = new System.Drawing.Point(7, 104);
            this.helpLink1.Name = "helpLink1";
            this.helpLink1.Size = new System.Drawing.Size(337, 13);
            this.helpLink1.TabIndex = 73;
            this.helpLink1.Text = "Click here for instructions on obtaining your Seller ID and Auth Token";
            this.helpLink1.Url = "http://www.interapptive.com/shipworks/help";
            // 
            // AmazonShippingTitle
            // 
            this.AmazonShippingTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.AmazonShippingTitle.Location = new System.Drawing.Point(0, 0);
            this.AmazonShippingTitle.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.AmazonShippingTitle.Name = "AmazonShippingTitle";
            this.AmazonShippingTitle.Size = new System.Drawing.Size(558, 22);
            this.AmazonShippingTitle.TabIndex = 17;
            this.AmazonShippingTitle.Text = "Amazon Shipping";
            // 
            // ChannelAdvisorAmazonSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.helpLink1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.countries);
            this.Controls.Add(this.merchantID);
            this.Controls.Add(this.authToken);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.AmazonShippingTitle);
            this.Name = "ChannelAdvisorAmazonSettingsControl";
            this.Size = new System.Drawing.Size(558, 129);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Controls.SectionTitle AmazonShippingTitle;
        private System.Windows.Forms.TextBox merchantID;
        private System.Windows.Forms.TextBox authToken;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.ComboBox countries;
        private System.Windows.Forms.Label label1;
        private ApplicationCore.Interaction.HelpLink helpLink1;
    }
}
