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
            this.form = new System.Windows.Forms.Panel();
            this.step2 = new System.Windows.Forms.Label();
            this.step1 = new System.Windows.Forms.Label();
            this.copyAccountNumber = new System.Windows.Forms.Button();
            this.merchantID = new System.Windows.Forms.TextBox();
            this.marketplaceID = new System.Windows.Forms.TextBox();
            this.buttonChooseMarketplace = new System.Windows.Forms.Button();
            this.step8 = new System.Windows.Forms.Label();
            this.authToken = new System.Windows.Forms.TextBox();
            this.authTokenLabel = new System.Windows.Forms.Label();
            this.accountNumber = new System.Windows.Forms.TextBox();
            this.step5Next = new System.Windows.Forms.Label();
            this.merchantIDLabel = new System.Windows.Forms.Label();
            this.mwsLink = new ShipWorks.UI.Controls.LinkControl();
            this.step7 = new System.Windows.Forms.Label();
            this.step6Next = new System.Windows.Forms.Label();
            this.step6 = new System.Windows.Forms.Label();
            this.step5Part2 = new System.Windows.Forms.Label();
            this.step5 = new System.Windows.Forms.Label();
            this.step4Part2 = new System.Windows.Forms.Label();
            this.step4DeveloperID = new System.Windows.Forms.Label();
            this.step4 = new System.Windows.Forms.Label();
            this.step3ShipWorks = new System.Windows.Forms.Label();
            this.step3Part2 = new System.Windows.Forms.Label();
            this.step3 = new System.Windows.Forms.Label();
            this.step3DevelopersName = new System.Windows.Forms.Label();
            this.title = new System.Windows.Forms.Label();
            this.form.SuspendLayout();
            this.SuspendLayout();
            // 
            // form
            // 
            this.form.Controls.Add(this.step2);
            this.form.Controls.Add(this.step1);
            this.form.Controls.Add(this.copyAccountNumber);
            this.form.Controls.Add(this.merchantID);
            this.form.Controls.Add(this.marketplaceID);
            this.form.Controls.Add(this.buttonChooseMarketplace);
            this.form.Controls.Add(this.step8);
            this.form.Controls.Add(this.authToken);
            this.form.Controls.Add(this.authTokenLabel);
            this.form.Controls.Add(this.accountNumber);
            this.form.Controls.Add(this.step5Next);
            this.form.Controls.Add(this.merchantIDLabel);
            this.form.Controls.Add(this.mwsLink);
            this.form.Controls.Add(this.step7);
            this.form.Controls.Add(this.step6Next);
            this.form.Controls.Add(this.step6);
            this.form.Controls.Add(this.step5Part2);
            this.form.Controls.Add(this.step5);
            this.form.Controls.Add(this.step4Part2);
            this.form.Controls.Add(this.step4DeveloperID);
            this.form.Controls.Add(this.step4);
            this.form.Controls.Add(this.step3ShipWorks);
            this.form.Controls.Add(this.step3Part2);
            this.form.Controls.Add(this.step3);
            this.form.Controls.Add(this.step3DevelopersName);
            this.form.Location = new System.Drawing.Point(25, 27);
            this.form.Name = "form";
            this.form.Size = new System.Drawing.Size(508, 308);
            this.form.TabIndex = 73;
            // 
            // step1
            // 
            this.step1.AutoSize = true;
            this.step1.Location = new System.Drawing.Point(-3, 0);
            this.step1.Name = "step1";
            this.step1.Size = new System.Drawing.Size(49, 13);
            this.step1.TabIndex = 39;
            this.step1.Text = "1.  Go to";
            // 
            // mwsLink
            // 
            this.mwsLink.AutoSize = true;
            this.mwsLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mwsLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.mwsLink.ForeColor = System.Drawing.Color.Blue;
            this.mwsLink.Location = new System.Drawing.Point(45, 0);
            this.mwsLink.Name = "mwsLink";
            this.mwsLink.Size = new System.Drawing.Size(194, 13);
            this.mwsLink.TabIndex = 63;
            this.mwsLink.Text = "https://sellercentral.amazon.com/gp/mws/registration/register.html?signInPageDisplayed=1&devAuth=1.";
            this.mwsLink.Click += new System.EventHandler(this.OnMWSLinkClick);
            // 
            // step2
            // 
            this.step2.AutoSize = true;
            this.step2.Location = new System.Drawing.Point(-3, 20);
            this.step2.Name = "step2";
            this.step2.Size = new System.Drawing.Size(63, 13);
            this.step2.TabIndex = 40;
            this.step2.Text = "2.  If prompted to sign in, do so using your Amazon seller account credentials.";
            // 
            // step3
            // 
            this.step3.AutoSize = true;
            this.step3.Location = new System.Drawing.Point(-3, 42);
            this.step3.Name = "step3";
            this.step3.Size = new System.Drawing.Size(52, 13);
            this.step3.TabIndex = 47;
            this.step3.Text = "3.  In the";
            // 
            // step3DevelopersName
            // 
            this.step3DevelopersName.AutoSize = true;
            this.step3DevelopersName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.step3DevelopersName.Location = new System.Drawing.Point(47, 42);
            this.step3DevelopersName.Name = "step3DevelopersName";
            this.step3DevelopersName.Size = new System.Drawing.Size(105, 13);
            this.step3DevelopersName.TabIndex = 48;
            this.step3DevelopersName.Text = "Developer\'s Name";
            // 
            // step3Part2
            // 
            this.step3Part2.AutoSize = true;
            this.step3Part2.Location = new System.Drawing.Point(153, 42);
            this.step3Part2.Name = "step3Part2";
            this.step3Part2.Size = new System.Drawing.Size(77, 13);
            this.step3Part2.TabIndex = 49;
            this.step3Part2.Text = "text box, type";
            // 
            // step3ShipWorks
            // 
            this.step3ShipWorks.AutoSize = true;
            this.step3ShipWorks.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.step3ShipWorks.Location = new System.Drawing.Point(227, 42);
            this.step3ShipWorks.Name = "step3ShipWorks";
            this.step3ShipWorks.Size = new System.Drawing.Size(70, 13);
            this.step3ShipWorks.TabIndex = 50;
            this.step3ShipWorks.Text = "ShipWorks.";
            // 
            // step4
            // 
            this.step4.AutoSize = true;
            this.step4.Location = new System.Drawing.Point(-3, 65);
            this.step4.Name = "step4";
            this.step4.Size = new System.Drawing.Size(52, 13);
            this.step4.TabIndex = 51;
            this.step4.Text = "4.  In the";
            // 
            // step4DeveloperID
            // 
            this.step4DeveloperID.AutoSize = true;
            this.step4DeveloperID.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.step4DeveloperID.Location = new System.Drawing.Point(47, 65);
            this.step4DeveloperID.Name = "step4DeveloperID";
            this.step4DeveloperID.Size = new System.Drawing.Size(236, 13);
            this.step4DeveloperID.TabIndex = 52;
            this.step4DeveloperID.Text = "Developer ID";
            // 
            // step4Part2
            // 
            this.step4Part2.AutoSize = true;
            this.step4Part2.Location = new System.Drawing.Point(127, 65);
            this.step4Part2.Name = "step4Part2";
            this.step4Part2.Size = new System.Drawing.Size(85, 13);
            this.step4Part2.TabIndex = 53;
            this.step4Part2.Text = "text box, enter:";
            // 
            // accountNumber
            // 
            this.accountNumber.BackColor = System.Drawing.Color.White;
            this.accountNumber.Location = new System.Drawing.Point(74, 87);
            this.accountNumber.Name = "accountNumber";
            this.accountNumber.ReadOnly = true;
            this.accountNumber.Size = new System.Drawing.Size(102, 21);
            this.accountNumber.TabIndex = 54;
            this.accountNumber.Text = "1025-5115-6476";
            // 
            // copyAccountNumber
            // 
            this.copyAccountNumber.Location = new System.Drawing.Point(182, 86);
            this.copyAccountNumber.Name = "copyAccountNumber";
            this.copyAccountNumber.Size = new System.Drawing.Size(128, 23);
            this.copyAccountNumber.TabIndex = 72;
            this.copyAccountNumber.Text = "Copy To Clipboard";
            this.copyAccountNumber.UseVisualStyleBackColor = true;
            this.copyAccountNumber.Click += new System.EventHandler(this.OnCopyAccountNumberClick);
            // 
            // step5
            // 
            this.step5.AutoSize = true;
            this.step5.Location = new System.Drawing.Point(-3, 115);
            this.step5.Name = "step5";
            this.step5.Size = new System.Drawing.Size(66, 13);
            this.step5.TabIndex = 55;
            this.step5.Text = "5.  Click the";
            // 
            // step5Next
            // 
            this.step5Next.AutoSize = true;
            this.step5Next.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.step5Next.Location = new System.Drawing.Point(55, 115);
            this.step5Next.Name = "step5Next";
            this.step5Next.Size = new System.Drawing.Size(33, 13);
            this.step5Next.TabIndex = 56;
            this.step5Next.Text = "Next";
            // 
            // step5Part2
            // 
            this.step5Part2.AutoSize = true;
            this.step5Part2.Location = new System.Drawing.Point(85, 115);
            this.step5Part2.Name = "step5Part2";
            this.step5Part2.Size = new System.Drawing.Size(43, 13);
            this.step5Part2.TabIndex = 57;
            this.step5Part2.Text = "button.";
            // 
            // step6
            // 
            this.step6.AutoSize = true;
            this.step6.Location = new System.Drawing.Point(-3, 139);
            this.step6.Name = "step6";
            this.step6.Size = new System.Drawing.Size(280, 13);
            this.step6.TabIndex = 58;
            this.step6.Text = "6.  Accept the Amazon MWS License Agreement and click";
            // 
            // step6Next
            // 
            this.step6Next.AutoSize = true;
            this.step6Next.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.step6Next.Location = new System.Drawing.Point(275, 139);
            this.step6Next.Name = "step6Next";
            this.step6Next.Size = new System.Drawing.Size(36, 13);
            this.step6Next.TabIndex = 59;
            this.step6Next.Text = "Next.";
            // 
            // step7
            // 
            this.step7.AutoSize = true;
            this.step7.Location = new System.Drawing.Point(-3, 163);
            this.step7.Name = "step7";
            this.step7.Size = new System.Drawing.Size(410, 13);
            this.step7.TabIndex = 60;
            this.step7.Text = "7.  Copy the Seller ID and Auth Token from the confirmation page and enter it here:";
            // 
            // merchantIDLabel
            // 
            this.merchantIDLabel.AutoSize = true;
            this.merchantIDLabel.Location = new System.Drawing.Point(98, 186);
            this.merchantIDLabel.Name = "merchantIDLabel";
            this.merchantIDLabel.Size = new System.Drawing.Size(51, 13);
            this.merchantIDLabel.TabIndex = 61;
            this.merchantIDLabel.Text = "Seller ID:";
            // 
            // merchantID
            // 
            this.merchantID.Location = new System.Drawing.Point(155, 183);
            this.merchantID.Name = "merchantID";
            this.merchantID.Size = new System.Drawing.Size(118, 21);
            this.merchantID.TabIndex = 3;
            // 
            // authTokenLabel
            // 
            this.authTokenLabel.AutoSize = true;
            this.authTokenLabel.Location = new System.Drawing.Point(83, 213);
            this.authTokenLabel.Name = "authTokenLabel";
            this.authTokenLabel.Size = new System.Drawing.Size(66, 13);
            this.authTokenLabel.TabIndex = 66;
            this.authTokenLabel.Text = "Auth Token:";
            // 
            // authToken
            // 
            this.authToken.Location = new System.Drawing.Point(155, 210);
            this.authToken.Name = "authToken";
            this.authToken.Size = new System.Drawing.Size(268, 21);
            this.authToken.TabIndex = 10;
            // 
            // step8
            // 
            this.step8.AutoSize = true;
            this.step8.Location = new System.Drawing.Point(-1, 242);
            this.step8.Name = "step8";
            this.step8.Size = new System.Drawing.Size(150, 13);
            this.step8.TabIndex = 68;
            this.step8.Text = "8. Enter your Marketplace ID:";
            // 
            // marketplaceID
            // 
            this.marketplaceID.Location = new System.Drawing.Point(155, 239);
            this.marketplaceID.Name = "marketplaceID";
            this.marketplaceID.Size = new System.Drawing.Size(118, 21);
            this.marketplaceID.TabIndex = 69;
            // 
            // buttonChooseMarketplace
            // 
            this.buttonChooseMarketplace.Location = new System.Drawing.Point(279, 238);
            this.buttonChooseMarketplace.Name = "buttonChooseMarketplace";
            this.buttonChooseMarketplace.Size = new System.Drawing.Size(146, 23);
            this.buttonChooseMarketplace.TabIndex = 7;
            this.buttonChooseMarketplace.Text = "Choose My Marketplace...";
            this.buttonChooseMarketplace.UseVisualStyleBackColor = true;
            this.buttonChooseMarketplace.Click += new System.EventHandler(this.OnClickFindMarketplaces);
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.title.Location = new System.Drawing.Point(22, 11);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(235, 13);
            this.title.TabIndex = 38;
            this.title.Text = "Authorize ShipWorks for Amazon Access";
            // 
            // AmazonMwsAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.form);
            this.Controls.Add(this.title);
            this.Name = "AmazonMwsAccountSettingsControl";
            this.Size = new System.Drawing.Size(537, 385);
            this.form.ResumeLayout(false);
            this.form.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox merchantID;
        private System.Windows.Forms.Button buttonChooseMarketplace;
        private System.Windows.Forms.TextBox authToken;
        private System.Windows.Forms.Label authTokenLabel;
        private System.Windows.Forms.Label step5Next;
        private UI.Controls.LinkControl mwsLink;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Label step1;
        private System.Windows.Forms.Label step2;
        private System.Windows.Forms.Label step3;
        private System.Windows.Forms.Label step3DevelopersName;
        private System.Windows.Forms.Label step3Part2;
        private System.Windows.Forms.Label step3ShipWorks;
        private System.Windows.Forms.Label step4;
        private System.Windows.Forms.Label step4DeveloperID;
        private System.Windows.Forms.Label step4Part2;
        private System.Windows.Forms.Label step5;
        private System.Windows.Forms.Label step5Part2;
        private System.Windows.Forms.Label step6;
        private System.Windows.Forms.Label step6Next;
        private System.Windows.Forms.Label step7;
        private System.Windows.Forms.Label merchantIDLabel;
        private System.Windows.Forms.TextBox accountNumber;
        private System.Windows.Forms.Label step8;
        private System.Windows.Forms.TextBox marketplaceID;
        private System.Windows.Forms.Button copyAccountNumber;
        private System.Windows.Forms.Panel form;
    }
}
