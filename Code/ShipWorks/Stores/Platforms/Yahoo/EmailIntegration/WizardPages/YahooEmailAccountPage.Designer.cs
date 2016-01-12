namespace ShipWorks.Stores.Platforms.Yahoo.EmailIntegration.WizardPages
{
    partial class YahooEmailAccountPage
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
            this.labelYahoo1 = new System.Windows.Forms.Label();
            this.labelImportant = new System.Windows.Forms.Label();
            this.labelImportantInfo = new System.Windows.Forms.Label();
            this.pictureImportant = new System.Windows.Forms.PictureBox();
            this.labelHelp = new System.Windows.Forms.Label();
            this.linkHelp = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            this.label5 = new System.Windows.Forms.Label();
            this.addAccount = new System.Windows.Forms.Button();
            this.pictureBoxSetup = new System.Windows.Forms.PictureBox();
            this.labelSetup = new System.Windows.Forms.Label();
            this.panelNotSetup = new System.Windows.Forms.Panel();
            this.emailAccountControl = new YahooEmailAccountControl();
            ((System.ComponentModel.ISupportInitialize) (this.pictureImportant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxSetup)).BeginInit();
            this.panelNotSetup.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelYahoo1
            // 
            this.labelYahoo1.Location = new System.Drawing.Point(25, 11);
            this.labelYahoo1.Name = "labelYahoo1";
            this.labelYahoo1.Size = new System.Drawing.Size(523, 34);
            this.labelYahoo1.TabIndex = 9;
            this.labelYahoo1.Text = "ShipWorks downloads Yahoo! orders by checking an email account that you configure" +
                " your Yahoo! store to send new order notifications.";
            // 
            // labelImportant
            // 
            this.labelImportant.AutoSize = true;
            this.labelImportant.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelImportant.Location = new System.Drawing.Point(46, 51);
            this.labelImportant.Name = "labelImportant";
            this.labelImportant.Size = new System.Drawing.Size(66, 13);
            this.labelImportant.TabIndex = 1;
            this.labelImportant.Text = "Important";
            // 
            // labelImportantInfo
            // 
            this.labelImportantInfo.Location = new System.Drawing.Point(118, 50);
            this.labelImportantInfo.Name = "labelImportantInfo";
            this.labelImportantInfo.Size = new System.Drawing.Size(430, 30);
            this.labelImportantInfo.TabIndex = 2;
            this.labelImportantInfo.Text = "ShipWorks deletes all messages after each download.  You must use an email accoun" +
                "t dedicated only to ShipWorks.";
            // 
            // pictureImportant
            // 
            this.pictureImportant.Image = global::ShipWorks.Properties.Resources.warning16;
            this.pictureImportant.Location = new System.Drawing.Point(26, 50);
            this.pictureImportant.Name = "pictureImportant";
            this.pictureImportant.Size = new System.Drawing.Size(16, 16);
            this.pictureImportant.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureImportant.TabIndex = 3;
            this.pictureImportant.TabStop = false;
            // 
            // labelHelp
            // 
            this.labelHelp.Location = new System.Drawing.Point(28, 90);
            this.labelHelp.Name = "labelHelp";
            this.labelHelp.Size = new System.Drawing.Size(520, 31);
            this.labelHelp.TabIndex = 3;
            this.labelHelp.Text = "For help setting up an email account, and to configure Yahoo! to automatically se" +
                "nd new order emails to the account, ";
            // 
            // linkHelp
            // 
            this.linkHelp.AutoSize = true;
            this.linkHelp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkHelp.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkHelp.ForeColor = System.Drawing.Color.Blue;
            this.linkHelp.Location = new System.Drawing.Point(94, 103);
            this.linkHelp.Name = "linkHelp";
            this.linkHelp.Size = new System.Drawing.Size(55, 13);
            this.linkHelp.TabIndex = 4;
            this.linkHelp.Text = "click here.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 13);
            this.label5.TabIndex = 6;
            // 
            // addAccount
            // 
            this.addAccount.Location = new System.Drawing.Point(17, 3);
            this.addAccount.Name = "addAccount";
            this.addAccount.Size = new System.Drawing.Size(139, 23);
            this.addAccount.TabIndex = 6;
            this.addAccount.Text = "Setup Email Account...";
            this.addAccount.UseVisualStyleBackColor = true;
            this.addAccount.Click += new System.EventHandler(this.OnAddAccount);
            // 
            // pictureBoxSetup
            // 
            this.pictureBoxSetup.Image = global::ShipWorks.Properties.Resources.check16;
            this.pictureBoxSetup.Location = new System.Drawing.Point(162, 6);
            this.pictureBoxSetup.Name = "pictureBoxSetup";
            this.pictureBoxSetup.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxSetup.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxSetup.TabIndex = 11;
            this.pictureBoxSetup.TabStop = false;
            this.pictureBoxSetup.Visible = false;
            // 
            // labelSetup
            // 
            this.labelSetup.AutoSize = true;
            this.labelSetup.Location = new System.Drawing.Point(180, 7);
            this.labelSetup.Name = "labelSetup";
            this.labelSetup.Size = new System.Drawing.Size(195, 13);
            this.labelSetup.TabIndex = 12;
            this.labelSetup.Text = "Account setup! Click \'Next\' to continue.";
            this.labelSetup.Visible = false;
            // 
            // panelNotSetup
            // 
            this.panelNotSetup.Controls.Add(this.addAccount);
            this.panelNotSetup.Controls.Add(this.labelSetup);
            this.panelNotSetup.Controls.Add(this.pictureBoxSetup);
            this.panelNotSetup.Location = new System.Drawing.Point(13, 126);
            this.panelNotSetup.Name = "panelNotSetup";
            this.panelNotSetup.Size = new System.Drawing.Size(435, 30);
            this.panelNotSetup.TabIndex = 13;
            // 
            // emailAccountControl
            // 
            this.emailAccountControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.emailAccountControl.Location = new System.Drawing.Point(31, 161);
            this.emailAccountControl.Name = "emailAccountControl";
            this.emailAccountControl.Size = new System.Drawing.Size(321, 76);
            this.emailAccountControl.TabIndex = 14;
            // 
            // YahooEmailAccountPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.emailAccountControl);
            this.Controls.Add(this.panelNotSetup);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.linkHelp);
            this.Controls.Add(this.labelHelp);
            this.Controls.Add(this.pictureImportant);
            this.Controls.Add(this.labelImportantInfo);
            this.Controls.Add(this.labelImportant);
            this.Controls.Add(this.labelYahoo1);
            this.Description = "Enter the following information about your online store.";
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "YahooEmailAccountPage";
            this.Size = new System.Drawing.Size(565, 278);
            this.Title = "Store Setup";
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            ((System.ComponentModel.ISupportInitialize) (this.pictureImportant)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxSetup)).EndInit();
            this.panelNotSetup.ResumeLayout(false);
            this.panelNotSetup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelYahoo1;
        private System.Windows.Forms.Label labelImportant;
        private System.Windows.Forms.Label labelImportantInfo;
        private System.Windows.Forms.PictureBox pictureImportant;
        private System.Windows.Forms.Label labelHelp;
        private ShipWorks.ApplicationCore.Interaction.HelpLink linkHelp;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button addAccount;
        private System.Windows.Forms.PictureBox pictureBoxSetup;
        private System.Windows.Forms.Label labelSetup;
        private System.Windows.Forms.Panel panelNotSetup;
        private YahooEmailAccountControl emailAccountControl;
    }
}
