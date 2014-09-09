namespace ShipWorks.Stores.Platforms.Amazon
{
    partial class AmazonMwsAccountSettingsControl
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.merchantID = new System.Windows.Forms.TextBox();
            this.marketplaceID = new System.Windows.Forms.TextBox();
            this.buttonChooseMarketplace = new System.Windows.Forms.Button();
            this.buttonFindMerchantID = new System.Windows.Forms.Button();
            this.authToken = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(22, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "1. Enter your Merchant ID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(22, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(169, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "3. Enter your Marketplace ID";
            // 
            // merchantID
            // 
            this.merchantID.Location = new System.Drawing.Point(36, 33);
            this.merchantID.Name = "merchantID";
            this.merchantID.Size = new System.Drawing.Size(184, 21);
            this.merchantID.TabIndex = 3;
            // 
            // marketplaceID
            // 
            this.marketplaceID.Location = new System.Drawing.Point(37, 144);
            this.marketplaceID.Name = "marketplaceID";
            this.marketplaceID.Size = new System.Drawing.Size(184, 21);
            this.marketplaceID.TabIndex = 6;
            // 
            // buttonChooseMarketplace
            // 
            this.buttonChooseMarketplace.Location = new System.Drawing.Point(227, 144);
            this.buttonChooseMarketplace.Name = "buttonChooseMarketplace";
            this.buttonChooseMarketplace.Size = new System.Drawing.Size(146, 23);
            this.buttonChooseMarketplace.TabIndex = 7;
            this.buttonChooseMarketplace.Text = "Choose My Marketplace...";
            this.buttonChooseMarketplace.UseVisualStyleBackColor = true;
            this.buttonChooseMarketplace.Click += new System.EventHandler(this.OnClickFindMarketplaces);
            // 
            // buttonFindMerchantID
            // 
            this.buttonFindMerchantID.Location = new System.Drawing.Point(227, 45);
            this.buttonFindMerchantID.Name = "buttonFindMerchantID";
            this.buttonFindMerchantID.Size = new System.Drawing.Size(146, 51);
            this.buttonFindMerchantID.TabIndex = 8;
            this.buttonFindMerchantID.Text = "Find My Merchant ID && AWS Auth Token...";
            this.buttonFindMerchantID.UseVisualStyleBackColor = true;
            this.buttonFindMerchantID.Click += new System.EventHandler(this.OnGetMerchantID);
            // 
            // authToken
            // 
            this.authToken.Location = new System.Drawing.Point(37, 88);
            this.authToken.Name = "authToken";
            this.authToken.Size = new System.Drawing.Size(184, 21);
            this.authToken.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(22, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(176, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "2. Enter your AWS Auth Token";
            // 
            // AmazonMwsAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.authToken);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonFindMerchantID);
            this.Controls.Add(this.buttonChooseMarketplace);
            this.Controls.Add(this.marketplaceID);
            this.Controls.Add(this.merchantID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "AmazonMwsAccountSettingsControl";
            this.Size = new System.Drawing.Size(416, 180);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox merchantID;
        private System.Windows.Forms.TextBox marketplaceID;
        private System.Windows.Forms.Button buttonChooseMarketplace;
        private System.Windows.Forms.Button buttonFindMerchantID;
        private System.Windows.Forms.TextBox authToken;
        private System.Windows.Forms.Label label1;
    }
}
