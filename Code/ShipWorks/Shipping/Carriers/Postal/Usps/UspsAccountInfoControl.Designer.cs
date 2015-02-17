namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    partial class UspsAccountInfoControl
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
            this.labelPostage = new System.Windows.Forms.Label();
            this.postage = new System.Windows.Forms.Label();
            this.labelAccount = new System.Windows.Forms.Label();
            this.accountName = new System.Windows.Forms.Label();
            this.labelStampsWebsite = new System.Windows.Forms.Label();
            this.accountSettingsLink = new System.Windows.Forms.Label();
            this.onlineReportsLink = new System.Windows.Forms.Label();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.labelContractType = new System.Windows.Forms.Label();
            this.contractType = new System.Windows.Forms.Label();
            this.purchase = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panelInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelPostage
            // 
            this.labelPostage.AutoSize = true;
            this.labelPostage.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPostage.Location = new System.Drawing.Point(26, 28);
            this.labelPostage.Name = "labelPostage";
            this.labelPostage.Size = new System.Drawing.Size(48, 13);
            this.labelPostage.TabIndex = 0;
            this.labelPostage.Text = "Balance:";
            // 
            // postage
            // 
            this.postage.AutoSize = true;
            this.postage.Location = new System.Drawing.Point(78, 28);
            this.postage.Name = "postage";
            this.postage.Size = new System.Drawing.Size(47, 13);
            this.postage.TabIndex = 1;
            this.postage.Text = "$109.90";
            // 
            // labelAccount
            // 
            this.labelAccount.AutoSize = true;
            this.labelAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAccount.Location = new System.Drawing.Point(4, 6);
            this.labelAccount.Name = "labelAccount";
            this.labelAccount.Size = new System.Drawing.Size(53, 13);
            this.labelAccount.TabIndex = 0;
            this.labelAccount.Text = "Account";
            // 
            // accountName
            // 
            this.accountName.AutoSize = true;
            this.accountName.Location = new System.Drawing.Point(81, 24);
            this.accountName.Name = "accountName";
            this.accountName.Size = new System.Drawing.Size(53, 13);
            this.accountName.TabIndex = 2;
            this.accountName.Text = "whatever";
            // 
            // labelStampsWebsite
            // 
            this.labelStampsWebsite.AutoSize = true;
            this.labelStampsWebsite.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStampsWebsite.Location = new System.Drawing.Point(1, 49);
            this.labelStampsWebsite.Name = "labelStampsWebsite";
            this.labelStampsWebsite.Size = new System.Drawing.Size(126, 13);
            this.labelStampsWebsite.TabIndex = 3;
            this.labelStampsWebsite.Text = "Stamps.com Website";
            // 
            // accountSettingsLink
            // 
            this.accountSettingsLink.AutoSize = true;
            this.accountSettingsLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.accountSettingsLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountSettingsLink.ForeColor = System.Drawing.Color.Blue;
            this.accountSettingsLink.Location = new System.Drawing.Point(19, 65);
            this.accountSettingsLink.Name = "accountSettingsLink";
            this.accountSettingsLink.Size = new System.Drawing.Size(88, 13);
            this.accountSettingsLink.TabIndex = 4;
            this.accountSettingsLink.Text = "Account Settings";
            this.accountSettingsLink.Click += new System.EventHandler(this.OnLinkAccountSettings);
            // 
            // onlineReportsLink
            // 
            this.onlineReportsLink.AutoSize = true;
            this.onlineReportsLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.onlineReportsLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.onlineReportsLink.ForeColor = System.Drawing.Color.Blue;
            this.onlineReportsLink.Location = new System.Drawing.Point(19, 82);
            this.onlineReportsLink.Name = "onlineReportsLink";
            this.onlineReportsLink.Size = new System.Drawing.Size(78, 13);
            this.onlineReportsLink.TabIndex = 5;
            this.onlineReportsLink.Text = "Online Reports";
            this.onlineReportsLink.Click += new System.EventHandler(this.OnLinkOnlineReports);
            // 
            // panelInfo
            // 
            this.panelInfo.Controls.Add(this.labelContractType);
            this.panelInfo.Controls.Add(this.contractType);
            this.panelInfo.Controls.Add(this.purchase);
            this.panelInfo.Controls.Add(this.labelPostage);
            this.panelInfo.Controls.Add(this.postage);
            this.panelInfo.Controls.Add(this.onlineReportsLink);
            this.panelInfo.Controls.Add(this.labelStampsWebsite);
            this.panelInfo.Controls.Add(this.accountSettingsLink);
            this.panelInfo.Location = new System.Drawing.Point(3, 37);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(273, 101);
            this.panelInfo.TabIndex = 3;
            // 
            // labelContractType
            // 
            this.labelContractType.AutoSize = true;
            this.labelContractType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelContractType.Location = new System.Drawing.Point(13, 8);
            this.labelContractType.Name = "labelContractType";
            this.labelContractType.Size = new System.Drawing.Size(61, 13);
            this.labelContractType.TabIndex = 6;
            this.labelContractType.Text = "Rate Type:";
            // 
            // contractType
            // 
            this.contractType.AutoSize = true;
            this.contractType.Location = new System.Drawing.Point(78, 8);
            this.contractType.Name = "contractType";
            this.contractType.Size = new System.Drawing.Size(50, 13);
            this.contractType.TabIndex = 7;
            this.contractType.Text = "CPP/NSA";
            // 
            // purchase
            // 
            this.purchase.Location = new System.Drawing.Point(131, 23);
            this.purchase.Name = "purchase";
            this.purchase.Size = new System.Drawing.Size(91, 23);
            this.purchase.TabIndex = 2;
            this.purchase.Text = "Buy Postage...";
            this.purchase.UseVisualStyleBackColor = true;
            this.purchase.Click += new System.EventHandler(this.OnPurchasePostage);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Account:";
            // 
            // UspsAccountInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panelInfo);
            this.Controls.Add(this.accountName);
            this.Controls.Add(this.labelAccount);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UspsAccountInfoControl";
            this.Size = new System.Drawing.Size(284, 144);
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelPostage;
        private System.Windows.Forms.Label postage;
        private System.Windows.Forms.Label labelAccount;
        private System.Windows.Forms.Label accountName;
        private System.Windows.Forms.Label labelStampsWebsite;
        private System.Windows.Forms.Label accountSettingsLink;
        private System.Windows.Forms.Label onlineReportsLink;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Button purchase;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelContractType;
        private System.Windows.Forms.Label contractType;
    }
}
