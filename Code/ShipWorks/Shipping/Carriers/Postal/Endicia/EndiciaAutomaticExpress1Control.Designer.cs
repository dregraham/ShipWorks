namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    partial class EndiciaAutomaticExpress1Control
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EndiciaAutomaticExpress1Control));
            this.panelExpress1Account = new System.Windows.Forms.Panel();
            this.labelExpress1Account = new System.Windows.Forms.Label();
            this.express1Accounts = new System.Windows.Forms.ComboBox();
            this.express1LearnMore = new ShipWorks.UI.Controls.LinkControl();
            this.express1Signup = new System.Windows.Forms.Button();
            this.labelDiscountedPostage = new System.Windows.Forms.Label();
            this.labelDiscountInfo1 = new System.Windows.Forms.Label();
            this.checkBoxUseExpress1 = new System.Windows.Forms.CheckBox();
            this.panelExpress1Account.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelExpress1Account
            // 
            this.panelExpress1Account.Controls.Add(this.labelExpress1Account);
            this.panelExpress1Account.Controls.Add(this.express1Accounts);
            this.panelExpress1Account.Controls.Add(this.express1LearnMore);
            this.panelExpress1Account.Controls.Add(this.express1Signup);
            this.panelExpress1Account.Enabled = false;
            this.panelExpress1Account.Location = new System.Drawing.Point(3, 129);
            this.panelExpress1Account.Name = "panelExpress1Account";
            this.panelExpress1Account.Size = new System.Drawing.Size(442, 34);
            this.panelExpress1Account.TabIndex = 5;
            // 
            // labelExpress1Account
            // 
            this.labelExpress1Account.AutoSize = true;
            this.labelExpress1Account.Location = new System.Drawing.Point(31, 10);
            this.labelExpress1Account.Name = "labelExpress1Account";
            this.labelExpress1Account.Size = new System.Drawing.Size(96, 13);
            this.labelExpress1Account.TabIndex = 1;
            this.labelExpress1Account.Text = "Express1 account:";
            // 
            // express1Accounts
            // 
            this.express1Accounts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.express1Accounts.FormattingEnabled = true;
            this.express1Accounts.Location = new System.Drawing.Point(305, 7);
            this.express1Accounts.Name = "express1Accounts";
            this.express1Accounts.Size = new System.Drawing.Size(122, 21);
            this.express1Accounts.TabIndex = 2;
            this.express1Accounts.Visible = false;
            // 
            // express1LearnMore
            // 
            this.express1LearnMore.AutoSize = true;
            this.express1LearnMore.Cursor = System.Windows.Forms.Cursors.Hand;
            this.express1LearnMore.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.express1LearnMore.ForeColor = System.Drawing.Color.Blue;
            this.express1LearnMore.Location = new System.Drawing.Point(229, 10);
            this.express1LearnMore.Name = "express1LearnMore";
            this.express1LearnMore.Size = new System.Drawing.Size(69, 13);
            this.express1LearnMore.TabIndex = 7;
            this.express1LearnMore.Text = "(Learn more)";
            this.express1LearnMore.Click += new System.EventHandler(this.OnEndiciaExpress1LearnMore);
            // 
            // express1Signup
            // 
            this.express1Signup.Location = new System.Drawing.Point(130, 5);
            this.express1Signup.Name = "express1Signup";
            this.express1Signup.Size = new System.Drawing.Size(94, 23);
            this.express1Signup.TabIndex = 3;
            this.express1Signup.Text = "Signup for Free";
            this.express1Signup.UseVisualStyleBackColor = true;
            this.express1Signup.Click += new System.EventHandler(this.OnEndiciaExpress1Signup);
            // 
            // labelDiscountedPostage
            // 
            this.labelDiscountedPostage.AutoSize = true;
            this.labelDiscountedPostage.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelDiscountedPostage.Location = new System.Drawing.Point(3, 3);
            this.labelDiscountedPostage.Name = "labelDiscountedPostage";
            this.labelDiscountedPostage.Size = new System.Drawing.Size(105, 13);
            this.labelDiscountedPostage.TabIndex = 0;
            this.labelDiscountedPostage.Text = "Postage Discount";
            // 
            // labelDiscountInfo1
            // 
            this.labelDiscountInfo1.Location = new System.Drawing.Point(15, 26);
            this.labelDiscountInfo1.Name = "labelDiscountInfo1";
            this.labelDiscountInfo1.Size = new System.Drawing.Size(417, 69);
            this.labelDiscountInfo1.TabIndex = 8;
            this.labelDiscountInfo1.Text = resources.GetString("labelDiscountInfo1.Text");
            // 
            // checkBoxUseExpress1
            // 
            this.checkBoxUseExpress1.Location = new System.Drawing.Point(18, 101);
            this.checkBoxUseExpress1.Name = "checkBoxUseExpress1";
            this.checkBoxUseExpress1.Size = new System.Drawing.Size(424, 33);
            this.checkBoxUseExpress1.TabIndex = 0;
            this.checkBoxUseExpress1.Text = "Automatically save postage costs on domestic and international Priority and Expre" +
                "ss shipments:";
            this.checkBoxUseExpress1.UseVisualStyleBackColor = true;
            this.checkBoxUseExpress1.Click += new System.EventHandler(this.OnEndiciaChangeUseExpress1);
            // 
            // EndiciaAutomaticExpress1Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelExpress1Account);
            this.Controls.Add(this.labelDiscountedPostage);
            this.Controls.Add(this.labelDiscountInfo1);
            this.Controls.Add(this.checkBoxUseExpress1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "EndiciaAutomaticExpress1Control";
            this.Size = new System.Drawing.Size(454, 173);
            this.panelExpress1Account.ResumeLayout(false);
            this.panelExpress1Account.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelExpress1Account;
        private System.Windows.Forms.Label labelExpress1Account;
        private System.Windows.Forms.ComboBox express1Accounts;
        private UI.Controls.LinkControl express1LearnMore;
        private System.Windows.Forms.Button express1Signup;
        private System.Windows.Forms.Label labelDiscountedPostage;
        private System.Windows.Forms.Label labelDiscountInfo1;
        private System.Windows.Forms.CheckBox checkBoxUseExpress1;
    }
}
