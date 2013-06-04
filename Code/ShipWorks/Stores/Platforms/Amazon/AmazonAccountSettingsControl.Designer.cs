namespace ShipWorks.Stores.Platforms.Amazon
{
    partial class AmazonAccountSettingsControl
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.merchant = new System.Windows.Forms.TextBox();
            this.merchantToken = new System.Windows.Forms.TextBox();
            this.linkSellerCentralAccountInfo = new ShipWorks.UI.Controls.LinkControl();
            this.label7 = new System.Windows.Forms.Label();
            this.extendedPanel = new System.Windows.Forms.Panel();
            this.importButton = new System.Windows.Forms.Button();
            this.certificateTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.accessKeyTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.label11 = new System.Windows.Forms.Label();
            this.enableMWSLink = new ShipWorks.UI.Controls.LinkControl();
            this.extendedPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(403, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter the username and password you use to login to your Amazon seller account.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(66, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Username:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(68, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Password:";
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(146, 36);
            this.fieldLengthProvider.SetMaxLengthSource(this.username, ShipWorks.Data.Utility.EntityFieldLengthSource.AmazonSellerUsername);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(188, 21);
            this.username.TabIndex = 3;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(146, 62);
            this.fieldLengthProvider.SetMaxLengthSource(this.password, ShipWorks.Data.Utility.EntityFieldLengthSource.AmazonSellerPassword);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(188, 21);
            this.password.TabIndex = 4;
            this.password.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(238, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Enter your merchant name and merchant token.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(69, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Merchant:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(37, 152);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Merchant Token:";
            // 
            // merchant
            // 
            this.merchant.Location = new System.Drawing.Point(146, 123);
            this.fieldLengthProvider.SetMaxLengthSource(this.merchant, ShipWorks.Data.Utility.EntityFieldLengthSource.AmazonMerchantName);
            this.merchant.Name = "merchant";
            this.merchant.Size = new System.Drawing.Size(188, 21);
            this.merchant.TabIndex = 8;
            // 
            // merchantToken
            // 
            this.merchantToken.Location = new System.Drawing.Point(146, 149);
            this.fieldLengthProvider.SetMaxLengthSource(this.merchantToken, ShipWorks.Data.Utility.EntityFieldLengthSource.AmazonMerchantToken);
            this.merchantToken.Name = "merchantToken";
            this.merchantToken.Size = new System.Drawing.Size(188, 21);
            this.merchantToken.TabIndex = 9;
            // 
            // linkSellerCentralAccountInfo
            // 
            this.linkSellerCentralAccountInfo.AutoSize = true;
            this.linkSellerCentralAccountInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkSellerCentralAccountInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkSellerCentralAccountInfo.ForeColor = System.Drawing.Color.Blue;
            this.linkSellerCentralAccountInfo.Location = new System.Drawing.Point(394, 190);
            this.linkSellerCentralAccountInfo.Name = "linkSellerCentralAccountInfo";
            this.linkSellerCentralAccountInfo.Size = new System.Drawing.Size(55, 13);
            this.linkSellerCentralAccountInfo.TabIndex = 10;
            this.linkSellerCentralAccountInfo.Text = "click here.";
            this.linkSellerCentralAccountInfo.Click += new System.EventHandler(this.OnLinkSellerSentral);
            // 
            // label7
            // 
            this.label7.ForeColor = System.Drawing.Color.DimGray;
            this.label7.Location = new System.Drawing.Point(145, 177);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(308, 31);
            this.label7.TabIndex = 11;
            this.label7.Text = "This information can be found in the Settings -> Account Info section of Seller C" +
                "entral.  Top open this page now, ";
            // 
            // extendedPanel
            // 
            this.extendedPanel.Controls.Add(this.importButton);
            this.extendedPanel.Controls.Add(this.certificateTextBox);
            this.extendedPanel.Controls.Add(this.label10);
            this.extendedPanel.Controls.Add(this.accessKeyTextBox);
            this.extendedPanel.Controls.Add(this.label9);
            this.extendedPanel.Controls.Add(this.label8);
            this.extendedPanel.Location = new System.Drawing.Point(10, 210);
            this.extendedPanel.Name = "extendedPanel";
            this.extendedPanel.Size = new System.Drawing.Size(443, 86);
            this.extendedPanel.TabIndex = 14;
            // 
            // importButton
            // 
            this.importButton.Location = new System.Drawing.Point(354, 52);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(75, 23);
            this.importButton.TabIndex = 25;
            this.importButton.Text = "&Import...";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.OnImportClick);
            // 
            // certificateTextBox
            // 
            this.certificateTextBox.Location = new System.Drawing.Point(136, 54);
            this.certificateTextBox.Name = "certificateTextBox";
            this.certificateTextBox.ReadOnly = true;
            this.certificateTextBox.Size = new System.Drawing.Size(209, 21);
            this.certificateTextBox.TabIndex = 24;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(54, 57);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 13);
            this.label10.TabIndex = 23;
            this.label10.Text = "Certificate:";
            // 
            // accessKeyTextBox
            // 
            this.accessKeyTextBox.Location = new System.Drawing.Point(136, 28);
            this.accessKeyTextBox.Name = "accessKeyTextBox";
            this.accessKeyTextBox.ReadOnly = true;
            this.accessKeyTextBox.Size = new System.Drawing.Size(209, 21);
            this.accessKeyTextBox.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(36, 31);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(79, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Access Key ID:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(2, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(170, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Amazon Web Services Credentials";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(12, 309);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(446, 32);
            this.label11.TabIndex = 15;
            this.label11.Text = "Amazon will be disabling the method ShipWorks is currently using to download from" +
                " your account.\r\n";
            // 
            // enableMWSLink
            // 
            this.enableMWSLink.AutoSize = true;
            this.enableMWSLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.enableMWSLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.enableMWSLink.ForeColor = System.Drawing.Color.Blue;
            this.enableMWSLink.Location = new System.Drawing.Point(61, 322);
            this.enableMWSLink.Name = "enableMWSLink";
            this.enableMWSLink.Size = new System.Drawing.Size(275, 13);
            this.enableMWSLink.TabIndex = 16;
            this.enableMWSLink.Text = "Click here to enable Amazon Marketplace Web Services.";
            this.enableMWSLink.Click += new System.EventHandler(this.OnEnableMWSClicked);
            // 
            // AmazonAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.enableMWSLink);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.linkSellerCentralAccountInfo);
            this.Controls.Add(this.merchantToken);
            this.Controls.Add(this.merchant);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.password);
            this.Controls.Add(this.username);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.extendedPanel);
            this.Name = "AmazonAccountSettingsControl";
            this.Size = new System.Drawing.Size(469, 354);
            this.Load += new System.EventHandler(this.OnLoad);
            this.extendedPanel.ResumeLayout(false);
            this.extendedPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox merchant;
        private System.Windows.Forms.TextBox merchantToken;
        private ShipWorks.UI.Controls.LinkControl linkSellerCentralAccountInfo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel extendedPanel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.TextBox certificateTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox accessKeyTextBox;
        private System.Windows.Forms.Label label9;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.Label label11;
        private UI.Controls.LinkControl enableMWSLink;
    }
}
