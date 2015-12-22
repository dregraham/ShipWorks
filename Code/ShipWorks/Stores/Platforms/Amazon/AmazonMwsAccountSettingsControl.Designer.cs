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
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.copyAccountNumber = new System.Windows.Forms.Button();
            this.merchantID = new System.Windows.Forms.TextBox();
            this.marketplaceID = new System.Windows.Forms.TextBox();
            this.buttonChooseMarketplace = new System.Windows.Forms.Button();
            this.label28 = new System.Windows.Forms.Label();
            this.authToken = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.accountNumber = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.mwsLink = new ShipWorks.UI.Controls.LinkControl();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.title = new System.Windows.Forms.Label();
            this.form.SuspendLayout();
            this.SuspendLayout();
            // 
            // form
            // 
            this.form.Controls.Add(this.label7);
            this.form.Controls.Add(this.label6);
            this.form.Controls.Add(this.copyAccountNumber);
            this.form.Controls.Add(this.merchantID);
            this.form.Controls.Add(this.marketplaceID);
            this.form.Controls.Add(this.buttonChooseMarketplace);
            this.form.Controls.Add(this.label28);
            this.form.Controls.Add(this.authToken);
            this.form.Controls.Add(this.label4);
            this.form.Controls.Add(this.accountNumber);
            this.form.Controls.Add(this.label19);
            this.form.Controls.Add(this.label27);
            this.form.Controls.Add(this.mwsLink);
            this.form.Controls.Add(this.label26);
            this.form.Controls.Add(this.label25);
            this.form.Controls.Add(this.label24);
            this.form.Controls.Add(this.label23);
            this.form.Controls.Add(this.label8);
            this.form.Controls.Add(this.label22);
            this.form.Controls.Add(this.label9);
            this.form.Controls.Add(this.label21);
            this.form.Controls.Add(this.label20);
            this.form.Controls.Add(this.label11);
            this.form.Controls.Add(this.label18);
            this.form.Controls.Add(this.label12);
            this.form.Controls.Add(this.label17);
            this.form.Controls.Add(this.label13);
            this.form.Controls.Add(this.label16);
            this.form.Controls.Add(this.label14);
            this.form.Controls.Add(this.label15);
            this.form.Location = new System.Drawing.Point(25, 27);
            this.form.Name = "form";
            this.form.Size = new System.Drawing.Size(508, 308);
            this.form.TabIndex = 73;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(-3, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 40;
            this.label7.Text = "2.  Click the";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(-3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 39;
            this.label6.Text = "1.  Go to";
            // 
            // copyAccountNumber
            // 
            this.copyAccountNumber.Location = new System.Drawing.Point(182, 127);
            this.copyAccountNumber.Name = "copyAccountNumber";
            this.copyAccountNumber.Size = new System.Drawing.Size(128, 23);
            this.copyAccountNumber.TabIndex = 72;
            this.copyAccountNumber.Text = "Copy To Clipboard";
            this.copyAccountNumber.UseVisualStyleBackColor = true;
            this.copyAccountNumber.Click += new System.EventHandler(this.OnCopyAccountNumberClick);
            // 
            // merchantID
            // 
            this.merchantID.Location = new System.Drawing.Point(155, 224);
            this.merchantID.Name = "merchantID";
            this.merchantID.Size = new System.Drawing.Size(118, 21);
            this.merchantID.TabIndex = 3;
            // 
            // marketplaceID
            // 
            this.marketplaceID.Location = new System.Drawing.Point(155, 280);
            this.marketplaceID.Name = "marketplaceID";
            this.marketplaceID.Size = new System.Drawing.Size(118, 21);
            this.marketplaceID.TabIndex = 69;
            // 
            // buttonChooseMarketplace
            // 
            this.buttonChooseMarketplace.Location = new System.Drawing.Point(279, 279);
            this.buttonChooseMarketplace.Name = "buttonChooseMarketplace";
            this.buttonChooseMarketplace.Size = new System.Drawing.Size(146, 23);
            this.buttonChooseMarketplace.TabIndex = 7;
            this.buttonChooseMarketplace.Text = "Choose My Marketplace...";
            this.buttonChooseMarketplace.UseVisualStyleBackColor = true;
            this.buttonChooseMarketplace.Click += new System.EventHandler(this.OnClickFindMarketplaces);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(-1, 283);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(150, 13);
            this.label28.TabIndex = 68;
            this.label28.Text = "9. Enter your Marketplace ID:";
            // 
            // authToken
            // 
            this.authToken.Location = new System.Drawing.Point(155, 251);
            this.authToken.Name = "authToken";
            this.authToken.Size = new System.Drawing.Size(268, 21);
            this.authToken.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(83, 254);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 66;
            this.label4.Text = "Auth Token:";
            // 
            // accountNumber
            // 
            this.accountNumber.BackColor = System.Drawing.Color.White;
            this.accountNumber.Location = new System.Drawing.Point(74, 128);
            this.accountNumber.Name = "accountNumber";
            this.accountNumber.ReadOnly = true;
            this.accountNumber.Size = new System.Drawing.Size(102, 21);
            this.accountNumber.TabIndex = 54;
            this.accountNumber.Text = "1025-5115-6476";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(55, 156);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(33, 13);
            this.label19.TabIndex = 56;
            this.label19.Text = "Next";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(98, 227);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(51, 13);
            this.label27.TabIndex = 61;
            this.label27.Text = "Seller ID:";
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
            this.mwsLink.Text = "http://developer.amazonservices.com.";
            this.mwsLink.Click += new System.EventHandler(this.OnMWSLinkClick);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(-3, 204);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(410, 13);
            this.label26.TabIndex = 60;
            this.label26.Text = "8.  Copy the Seller ID and Auth Token from the confirmation page and enter it her" +
    "e:";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(275, 180);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(36, 13);
            this.label25.TabIndex = 59;
            this.label25.Text = "Next.";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(-3, 180);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(280, 13);
            this.label24.TabIndex = 58;
            this.label24.Text = "7.  Accept the Amazon MWS License Agreement and click";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(85, 156);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(43, 13);
            this.label23.TabIndex = 57;
            this.label23.Text = "button.";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(56, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 13);
            this.label8.TabIndex = 41;
            this.label8.Text = "Sign up for MWS";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(-3, 156);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(66, 13);
            this.label22.TabIndex = 55;
            this.label22.Text = "6.  Click the ";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(154, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(237, 13);
            this.label9.TabIndex = 42;
            this.label9.Text = "button and log into your Amazon seller account.";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(282, 106);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(85, 13);
            this.label21.TabIndex = 53;
            this.label21.Text = "text box, enter:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(48, 106);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(236, 13);
            this.label20.TabIndex = 52;
            this.label20.Text = "Application\'s Developer Account Number";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(-3, 42);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(268, 13);
            this.label11.TabIndex = 44;
            this.label11.Text = "3.  On the MWS registration page, click the button for ";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(-3, 106);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(52, 13);
            this.label18.TabIndex = 51;
            this.label18.Text = "5.  In the";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(262, 42);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(178, 13);
            this.label12.TabIndex = 45;
            this.label12.Text = "I want to use an application to";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(227, 83);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(70, 13);
            this.label17.TabIndex = 50;
            this.label17.Text = "ShipWorks.";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(12, 61);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(258, 13);
            this.label13.TabIndex = 46;
            this.label13.Text = "access my Amazon seller account with MWS.";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(151, 83);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(77, 13);
            this.label16.TabIndex = 49;
            this.label16.Text = "text box, type";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(-3, 83);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(52, 13);
            this.label14.TabIndex = 47;
            this.label14.Text = "4.  In the";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(47, 83);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(105, 13);
            this.label15.TabIndex = 48;
            this.label15.Text = "Application Name";
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label19;
        private UI.Controls.LinkControl mwsLink;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox accountNumber;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox marketplaceID;
        private System.Windows.Forms.Button copyAccountNumber;
        private System.Windows.Forms.Panel form;
    }
}
