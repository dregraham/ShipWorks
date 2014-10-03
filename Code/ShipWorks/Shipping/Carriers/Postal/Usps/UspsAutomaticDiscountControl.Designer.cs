﻿namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    partial class UspsAutomaticDiscountControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UspsAutomaticDiscountControl));
            this.panelExpress1Account = new System.Windows.Forms.Panel();
            this.labelExpeditedAccount = new System.Windows.Forms.Label();
            this.expeditedAccounts = new System.Windows.Forms.ComboBox();
            this.expeditedLearnMore = new ShipWorks.UI.Controls.LinkControl();
            this.expeditedSignup = new System.Windows.Forms.Button();
            this.labelDiscountedPostage = new System.Windows.Forms.Label();
            this.labelDiscountInfo1 = new System.Windows.Forms.Label();
            this.checkBoxUseExpedited = new System.Windows.Forms.CheckBox();
            this.panelExpress1Account.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelExpress1Account
            // 
            this.panelExpress1Account.Controls.Add(this.labelExpeditedAccount);
            this.panelExpress1Account.Controls.Add(this.expeditedAccounts);
            this.panelExpress1Account.Controls.Add(this.expeditedLearnMore);
            this.panelExpress1Account.Controls.Add(this.expeditedSignup);
            this.panelExpress1Account.Enabled = false;
            this.panelExpress1Account.Location = new System.Drawing.Point(6, 132);
            this.panelExpress1Account.Name = "panelExpress1Account";
            this.panelExpress1Account.Size = new System.Drawing.Size(442, 34);
            this.panelExpress1Account.TabIndex = 11;
            // 
            // labelExpeditedAccount
            // 
            this.labelExpeditedAccount.AutoSize = true;
            this.labelExpeditedAccount.Location = new System.Drawing.Point(14, 10);
            this.labelExpeditedAccount.Name = "labelExpeditedAccount";
            this.labelExpeditedAccount.Size = new System.Drawing.Size(110, 13);
            this.labelExpeditedAccount.TabIndex = 1;
            this.labelExpeditedAccount.Text = "Stamps.com account:";
            // 
            // expeditedAccounts
            // 
            this.expeditedAccounts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.expeditedAccounts.FormattingEnabled = true;
            this.expeditedAccounts.Location = new System.Drawing.Point(305, 7);
            this.expeditedAccounts.Name = "expeditedAccounts";
            this.expeditedAccounts.Size = new System.Drawing.Size(122, 21);
            this.expeditedAccounts.TabIndex = 2;
            this.expeditedAccounts.Visible = false;
            this.expeditedAccounts.SelectedIndexChanged += new System.EventHandler(this.OnExpeditedAccountsSelectedIndexChanged);
            // 
            // expeditedLearnMore
            // 
            this.expeditedLearnMore.AutoSize = true;
            this.expeditedLearnMore.Cursor = System.Windows.Forms.Cursors.Hand;
            this.expeditedLearnMore.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.expeditedLearnMore.ForeColor = System.Drawing.Color.Blue;
            this.expeditedLearnMore.Location = new System.Drawing.Point(229, 10);
            this.expeditedLearnMore.Name = "expeditedLearnMore";
            this.expeditedLearnMore.Size = new System.Drawing.Size(69, 13);
            this.expeditedLearnMore.TabIndex = 7;
            this.expeditedLearnMore.Text = "(Learn more)";
            this.expeditedLearnMore.Click += new System.EventHandler(this.OnExpedited1LearnMore);
            // 
            // expeditedSignup
            // 
            this.expeditedSignup.Location = new System.Drawing.Point(130, 5);
            this.expeditedSignup.Name = "expeditedSignup";
            this.expeditedSignup.Size = new System.Drawing.Size(94, 23);
            this.expeditedSignup.TabIndex = 3;
            this.expeditedSignup.Text = "Signup for Free";
            this.expeditedSignup.UseVisualStyleBackColor = true;
            this.expeditedSignup.Click += new System.EventHandler(this.OnSignup);
            // 
            // labelDiscountedPostage
            // 
            this.labelDiscountedPostage.AutoSize = true;
            this.labelDiscountedPostage.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDiscountedPostage.Location = new System.Drawing.Point(6, 6);
            this.labelDiscountedPostage.Name = "labelDiscountedPostage";
            this.labelDiscountedPostage.Size = new System.Drawing.Size(105, 13);
            this.labelDiscountedPostage.TabIndex = 9;
            this.labelDiscountedPostage.Text = "Postage Discount";
            // 
            // labelDiscountInfo1
            // 
            this.labelDiscountInfo1.Location = new System.Drawing.Point(18, 29);
            this.labelDiscountInfo1.Name = "labelDiscountInfo1";
            this.labelDiscountInfo1.Size = new System.Drawing.Size(417, 69);
            this.labelDiscountInfo1.TabIndex = 12;
            this.labelDiscountInfo1.Text = resources.GetString("labelDiscountInfo1.Text");
            // 
            // checkBoxUseExpedited
            // 
            this.checkBoxUseExpedited.Location = new System.Drawing.Point(21, 104);
            this.checkBoxUseExpedited.Name = "checkBoxUseExpedited";
            this.checkBoxUseExpedited.Size = new System.Drawing.Size(424, 33);
            this.checkBoxUseExpedited.TabIndex = 10;
            this.checkBoxUseExpedited.Text = "Automatically save postage costs on domestic and international Priority and Expre" +
    "ss shipments:";
            this.checkBoxUseExpedited.UseVisualStyleBackColor = true;
            this.checkBoxUseExpedited.Click += new System.EventHandler(this.OnChangeUseExpedited);
            // 
            // UspsAutomaticDiscountControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelExpress1Account);
            this.Controls.Add(this.labelDiscountedPostage);
            this.Controls.Add(this.labelDiscountInfo1);
            this.Controls.Add(this.checkBoxUseExpedited);
            this.Name = "UspsAutomaticDiscountControl";
            this.Size = new System.Drawing.Size(454, 173);
            this.panelExpress1Account.ResumeLayout(false);
            this.panelExpress1Account.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelExpress1Account;
        private System.Windows.Forms.Label labelExpeditedAccount;
        private System.Windows.Forms.ComboBox expeditedAccounts;
        private UI.Controls.LinkControl expeditedLearnMore;
        private System.Windows.Forms.Button expeditedSignup;
        private System.Windows.Forms.Label labelDiscountedPostage;
        private System.Windows.Forms.Label labelDiscountInfo1;
        private System.Windows.Forms.CheckBox checkBoxUseExpedited;
    }
}
