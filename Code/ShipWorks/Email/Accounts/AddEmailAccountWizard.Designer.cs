namespace ShipWorks.Email.Accounts
{
    partial class AddEmailAccountWizard
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddEmailAccountWizard));
            this.wizardPageBasicInfo = new ShipWorks.UI.Wizard.WizardPage();
            this.panelAutoConfig = new System.Windows.Forms.Panel();
            this.displayName = new System.Windows.Forms.TextBox();
            this.labelDisplayName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.emailAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.password = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.labelManualConfig = new System.Windows.Forms.Label();
            this.pictureBoxManualConfig = new System.Windows.Forms.PictureBox();
            this.labelAutomaticConfig = new System.Windows.Forms.Label();
            this.pictureBoxAutomaticConfig = new System.Windows.Forms.PictureBox();
            this.radioManualConfig = new System.Windows.Forms.RadioButton();
            this.radioAutomaticConfig = new System.Windows.Forms.RadioButton();
            this.wizardPageEnableAccess = new ShipWorks.UI.Wizard.WizardPage();
            this.labelGMailEnablePop = new System.Windows.Forms.Label();
            this.linkGMailPopHelp = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelGmailInfo = new System.Windows.Forms.Label();
            this.wizardPageSearchFailed = new ShipWorks.UI.Wizard.WizardPage();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.wizardPageManualConfigure = new ShipWorks.UI.Wizard.WizardPage();
            this.emailAccountSettings = new ShipWorks.Email.Accounts.EmailAccountSettingsControl();
            this.wizardPageSuccess = new ShipWorks.UI.Wizard.WizardPage();
            this.label6 = new System.Windows.Forms.Label();
            this.iconSetupComplete = new System.Windows.Forms.PictureBox();
            this.wizardPageAccountAlias = new ShipWorks.UI.Wizard.WizardPage();
            this.infotipAccountAlias = new ShipWorks.UI.Controls.InfoTip();
            this.alias = new System.Windows.Forms.TextBox();
            this.labelAlias = new System.Windows.Forms.Label();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageBasicInfo.SuspendLayout();
            this.panelAutoConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxManualConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxAutomaticConfig)).BeginInit();
            this.wizardPageEnableAccess.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).BeginInit();
            this.wizardPageSearchFailed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox2)).BeginInit();
            this.wizardPageManualConfigure.SuspendLayout();
            this.wizardPageSuccess.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.iconSetupComplete)).BeginInit();
            this.wizardPageAccountAlias.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            //
            // next
            //
            this.next.Location = new System.Drawing.Point(325, 435);
            this.next.Text = "Finish";
            //
            // cancel
            //
            this.cancel.Location = new System.Drawing.Point(406, 435);
            //
            // back
            //
            this.back.Location = new System.Drawing.Point(244, 435);
            //
            // mainPanel
            //
            this.mainPanel.Controls.Add(this.wizardPageSuccess);
            this.mainPanel.Size = new System.Drawing.Size(493, 363);
            //
            // etchBottom
            //
            this.etchBottom.Location = new System.Drawing.Point(0, 425);
            this.etchBottom.Size = new System.Drawing.Size(497, 2);
            //
            // pictureBox
            //
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.mailbox_empty_add;
            this.pictureBox.Location = new System.Drawing.Point(440, 3);
            this.pictureBox.Size = new System.Drawing.Size(48, 48);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            //
            // topPanel
            //
            this.topPanel.Size = new System.Drawing.Size(493, 56);
            //
            // wizardPageBasicInfo
            //
            this.wizardPageBasicInfo.Controls.Add(this.panelAutoConfig);
            this.wizardPageBasicInfo.Controls.Add(this.labelManualConfig);
            this.wizardPageBasicInfo.Controls.Add(this.pictureBoxManualConfig);
            this.wizardPageBasicInfo.Controls.Add(this.labelAutomaticConfig);
            this.wizardPageBasicInfo.Controls.Add(this.pictureBoxAutomaticConfig);
            this.wizardPageBasicInfo.Controls.Add(this.radioManualConfig);
            this.wizardPageBasicInfo.Controls.Add(this.radioAutomaticConfig);
            this.wizardPageBasicInfo.Description = "ShipWorks will try to automatically configure your email account settings.";
            this.wizardPageBasicInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageBasicInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageBasicInfo.Location = new System.Drawing.Point(0, 0);
            this.wizardPageBasicInfo.Name = "wizardPageBasicInfo";
            this.wizardPageBasicInfo.Size = new System.Drawing.Size(493, 363);
            this.wizardPageBasicInfo.TabIndex = 0;
            this.wizardPageBasicInfo.Title = "Account Setup";
            this.wizardPageBasicInfo.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnBasicInfoStepNext);
            this.wizardPageBasicInfo.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnBasicInfoSteppingInto);
            //
            // panelAutoConfig
            //
            this.panelAutoConfig.Controls.Add(this.displayName);
            this.panelAutoConfig.Controls.Add(this.labelDisplayName);
            this.panelAutoConfig.Controls.Add(this.label2);
            this.panelAutoConfig.Controls.Add(this.emailAddress);
            this.panelAutoConfig.Controls.Add(this.label3);
            this.panelAutoConfig.Controls.Add(this.password);
            this.panelAutoConfig.Controls.Add(this.label4);
            this.panelAutoConfig.Location = new System.Drawing.Point(43, 67);
            this.panelAutoConfig.Name = "panelAutoConfig";
            this.panelAutoConfig.Size = new System.Drawing.Size(354, 101);
            this.panelAutoConfig.TabIndex = 14;
            //
            // displayName
            //
            this.displayName.Location = new System.Drawing.Point(90, 3);
            this.fieldLengthProvider.SetMaxLengthSource(this.displayName, ShipWorks.Data.Utility.EntityFieldLengthSource.EmailAccountName);
            this.displayName.Name = "displayName";
            this.displayName.Size = new System.Drawing.Size(253, 21);
            this.displayName.TabIndex = 1;
            //
            // labelDisplayName
            //
            this.labelDisplayName.AutoSize = true;
            this.labelDisplayName.Location = new System.Drawing.Point(22, 6);
            this.labelDisplayName.Name = "labelDisplayName";
            this.labelDisplayName.Size = new System.Drawing.Size(62, 13);
            this.labelDisplayName.TabIndex = 0;
            this.labelDisplayName.Text = "Your name:";
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Email Address:";
            //
            // emailAddress
            //
            this.emailAddress.Location = new System.Drawing.Point(90, 30);
            this.fieldLengthProvider.SetMaxLengthSource(this.emailAddress, ShipWorks.Data.Utility.EntityFieldLengthSource.EmailAddress);
            this.emailAddress.Name = "emailAddress";
            this.emailAddress.Size = new System.Drawing.Size(253, 21);
            this.emailAddress.TabIndex = 3;
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Password:";
            //
            // password
            //
            this.password.Location = new System.Drawing.Point(90, 57);
            this.fieldLengthProvider.SetMaxLengthSource(this.password, ShipWorks.Data.Utility.EntityFieldLengthSource.EmailPassword);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(253, 21);
            this.password.TabIndex = 5;
            this.password.UseSystemPasswordChar = true;
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.DimGray;
            this.label4.Location = new System.Drawing.Point(89, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(254, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Type the password you use for your email account.";
            //
            // labelManualConfig
            //
            this.labelManualConfig.ForeColor = System.Drawing.Color.DimGray;
            this.labelManualConfig.Location = new System.Drawing.Point(80, 206);
            this.labelManualConfig.Name = "labelManualConfig";
            this.labelManualConfig.Size = new System.Drawing.Size(389, 33);
            this.labelManualConfig.TabIndex = 12;
            this.labelManualConfig.Text = "Manually configure your email account settings.  You must choose this option if y" +
                "our outgoing mail server does not require a username or password.";
            //
            // pictureBoxManualConfig
            //
            this.pictureBoxManualConfig.Image = global::ShipWorks.Properties.Resources.mail_server32;
            this.pictureBoxManualConfig.Location = new System.Drawing.Point(42, 204);
            this.pictureBoxManualConfig.Name = "pictureBoxManualConfig";
            this.pictureBoxManualConfig.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxManualConfig.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxManualConfig.TabIndex = 13;
            this.pictureBoxManualConfig.TabStop = false;
            //
            // labelAutomaticConfig
            //
            this.labelAutomaticConfig.AutoSize = true;
            this.labelAutomaticConfig.ForeColor = System.Drawing.Color.DimGray;
            this.labelAutomaticConfig.Location = new System.Drawing.Point(80, 38);
            this.labelAutomaticConfig.Name = "labelAutomaticConfig";
            this.labelAutomaticConfig.Size = new System.Drawing.Size(338, 13);
            this.labelAutomaticConfig.TabIndex = 10;
            this.labelAutomaticConfig.Text = "ShipWorks will automatically determine the settings for your account.";
            //
            // pictureBoxAutomaticConfig
            //
            this.pictureBoxAutomaticConfig.Image = global::ShipWorks.Properties.Resources.magicwand;
            this.pictureBoxAutomaticConfig.Location = new System.Drawing.Point(42, 30);
            this.pictureBoxAutomaticConfig.Name = "pictureBoxAutomaticConfig";
            this.pictureBoxAutomaticConfig.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxAutomaticConfig.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxAutomaticConfig.TabIndex = 11;
            this.pictureBoxAutomaticConfig.TabStop = false;
            //
            // radioManualConfig
            //
            this.radioManualConfig.AutoSize = true;
            this.radioManualConfig.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.radioManualConfig.Location = new System.Drawing.Point(26, 181);
            this.radioManualConfig.Name = "radioManualConfig";
            this.radioManualConfig.Size = new System.Drawing.Size(145, 17);
            this.radioManualConfig.TabIndex = 9;
            this.radioManualConfig.Text = "Manual Configuration";
            this.radioManualConfig.UseVisualStyleBackColor = true;
            this.radioManualConfig.CheckedChanged += new System.EventHandler(this.OnChangeManualConfiguration);
            //
            // radioAutomaticConfig
            //
            this.radioAutomaticConfig.AutoSize = true;
            this.radioAutomaticConfig.Checked = true;
            this.radioAutomaticConfig.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.radioAutomaticConfig.Location = new System.Drawing.Point(25, 8);
            this.radioAutomaticConfig.Name = "radioAutomaticConfig";
            this.radioAutomaticConfig.Size = new System.Drawing.Size(163, 17);
            this.radioAutomaticConfig.TabIndex = 8;
            this.radioAutomaticConfig.TabStop = true;
            this.radioAutomaticConfig.Text = "Automatic Configuration";
            this.radioAutomaticConfig.UseVisualStyleBackColor = true;
            this.radioAutomaticConfig.CheckedChanged += new System.EventHandler(this.OnChangeManualConfiguration);
            //
            // wizardPageEnableAccess
            //
            this.wizardPageEnableAccess.Controls.Add(this.labelGMailEnablePop);
            this.wizardPageEnableAccess.Controls.Add(this.linkGMailPopHelp);
            this.wizardPageEnableAccess.Controls.Add(this.pictureBox1);
            this.wizardPageEnableAccess.Controls.Add(this.labelGmailInfo);
            this.wizardPageEnableAccess.Description = "Enable access to your email account.";
            this.wizardPageEnableAccess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageEnableAccess.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageEnableAccess.Location = new System.Drawing.Point(0, 0);
            this.wizardPageEnableAccess.Name = "wizardPageEnableAccess";
            this.wizardPageEnableAccess.Size = new System.Drawing.Size(493, 333);
            this.wizardPageEnableAccess.TabIndex = 0;
            this.wizardPageEnableAccess.Title = "Enable Account Access";
            this.wizardPageEnableAccess.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnEnableAccessStepNext);
            this.wizardPageEnableAccess.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnEnableAccessSteppingInto);
            //
            // labelGMailEnablePop
            //
            this.labelGMailEnablePop.AutoSize = true;
            this.labelGMailEnablePop.Location = new System.Drawing.Point(95, 49);
            this.labelGMailEnablePop.Name = "labelGMailEnablePop";
            this.labelGMailEnablePop.Size = new System.Drawing.Size(341, 13);
            this.labelGMailEnablePop.TabIndex = 3;
            this.labelGMailEnablePop.Text = "to learn how to enable POP in GMail.  Click Next when POP is enabled.";
            //
            // linkGMailPopHelp
            //
            this.linkGMailPopHelp.AutoSize = true;
            this.linkGMailPopHelp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkGMailPopHelp.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.linkGMailPopHelp.ForeColor = System.Drawing.Color.Blue;
            this.linkGMailPopHelp.Location = new System.Drawing.Point(45, 48);
            this.linkGMailPopHelp.Name = "linkGMailPopHelp";
            this.linkGMailPopHelp.Size = new System.Drawing.Size(53, 13);
            this.linkGMailPopHelp.TabIndex = 2;
            this.linkGMailPopHelp.Text = "Click here";
            this.linkGMailPopHelp.Click += new System.EventHandler(this.OnClickGMailPopHelp);
            //
            // pictureBox1
            //
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.exclamation16;
            this.pictureBox1.Location = new System.Drawing.Point(25, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            //
            // labelGmailInfo
            //
            this.labelGmailInfo.Location = new System.Drawing.Point(45, 12);
            this.labelGmailInfo.Name = "labelGmailInfo";
            this.labelGmailInfo.Size = new System.Drawing.Size(413, 32);
            this.labelGmailInfo.TabIndex = 0;
            this.labelGmailInfo.Text = "For ShipWorks to send email using your GMail account, POP must be enabled in your" +
                " GMail settings.";
            //
            // wizardPageSearchFailed
            //
            this.wizardPageSearchFailed.Controls.Add(this.label5);
            this.wizardPageSearchFailed.Controls.Add(this.label1);
            this.wizardPageSearchFailed.Controls.Add(this.pictureBox2);
            this.wizardPageSearchFailed.Description = "ShipWorks could not determine your email account settings.";
            this.wizardPageSearchFailed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageSearchFailed.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageSearchFailed.Location = new System.Drawing.Point(0, 0);
            this.wizardPageSearchFailed.Name = "wizardPageSearchFailed";
            this.wizardPageSearchFailed.Size = new System.Drawing.Size(493, 333);
            this.wizardPageSearchFailed.TabIndex = 0;
            this.wizardPageSearchFailed.Title = "Auto Configuration Failed";
            this.wizardPageSearchFailed.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSearchFailedSteppingInto);
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(67, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(257, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Click Next to manually configure your email settings.";
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label1.Location = new System.Drawing.Point(66, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(350, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "ShipWorks could not find the settings for your email account.";
            //
            // pictureBox2
            //
            this.pictureBox2.Image = ((System.Drawing.Image) (resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(25, 8);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(32, 32);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            //
            // wizardPageManualConfigure
            //
            this.wizardPageManualConfigure.Controls.Add(this.emailAccountSettings);
            this.wizardPageManualConfigure.Description = "Each of these settings are required to get your email account working.";
            this.wizardPageManualConfigure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageManualConfigure.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageManualConfigure.Location = new System.Drawing.Point(0, 0);
            this.wizardPageManualConfigure.Name = "wizardPageManualConfigure";
            this.wizardPageManualConfigure.Size = new System.Drawing.Size(493, 363);
            this.wizardPageManualConfigure.TabIndex = 0;
            this.wizardPageManualConfigure.Title = "Email Account Settings";
            this.wizardPageManualConfigure.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnManualSettingsStepNext);
            this.wizardPageManualConfigure.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnManualSettingsSteppingInto);
            //
            // emailAccountSettings
            //
            this.emailAccountSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.emailAccountSettings.Location = new System.Drawing.Point(23, 5);
            this.emailAccountSettings.Name = "emailAccountSettings";
            this.emailAccountSettings.Size = new System.Drawing.Size(394, 357);
            this.emailAccountSettings.TabIndex = 0;
            //
            // wizardPageSuccess
            //
            this.wizardPageSuccess.Controls.Add(this.label6);
            this.wizardPageSuccess.Controls.Add(this.iconSetupComplete);
            this.wizardPageSuccess.Description = "Your email account has been added to ShipWorks.";
            this.wizardPageSuccess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageSuccess.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageSuccess.Location = new System.Drawing.Point(0, 0);
            this.wizardPageSuccess.Name = "wizardPageSuccess";
            this.wizardPageSuccess.Size = new System.Drawing.Size(493, 363);
            this.wizardPageSuccess.TabIndex = 0;
            this.wizardPageSuccess.Title = "Setup Complete";
            //
            // label6
            //
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(44, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(251, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "ShipWorks is now ready to use your email account.";
            //
            // iconSetupComplete
            //
            this.iconSetupComplete.Image = global::ShipWorks.Properties.Resources.check16;
            this.iconSetupComplete.Location = new System.Drawing.Point(22, 9);
            this.iconSetupComplete.Name = "iconSetupComplete";
            this.iconSetupComplete.Size = new System.Drawing.Size(16, 16);
            this.iconSetupComplete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.iconSetupComplete.TabIndex = 3;
            this.iconSetupComplete.TabStop = false;
            //
            // wizardPageAccountAlias
            //
            this.wizardPageAccountAlias.Controls.Add(this.infotipAccountAlias);
            this.wizardPageAccountAlias.Controls.Add(this.alias);
            this.wizardPageAccountAlias.Controls.Add(this.labelAlias);
            this.wizardPageAccountAlias.Description = "Enter an alias for this account to be known by.";
            this.wizardPageAccountAlias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageAccountAlias.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageAccountAlias.Location = new System.Drawing.Point(0, 0);
            this.wizardPageAccountAlias.Name = "wizardPageAccountAlias";
            this.wizardPageAccountAlias.Size = new System.Drawing.Size(493, 333);
            this.wizardPageAccountAlias.TabIndex = 0;
            this.wizardPageAccountAlias.Title = "Account Alias";
            this.wizardPageAccountAlias.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnAccountAliasStepNext);
            this.wizardPageAccountAlias.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnAccountAliasSteppingInto);
            //
            // infotipAccountAlias
            //
            this.infotipAccountAlias.Caption = "This is the name ShipWorks will use when displaying your email account.  It can b" +
                "e anything you want, and is not used in the messages you send.";
            this.infotipAccountAlias.Location = new System.Drawing.Point(270, 12);
            this.infotipAccountAlias.Name = "infotipAccountAlias";
            this.infotipAccountAlias.Size = new System.Drawing.Size(12, 12);
            this.infotipAccountAlias.TabIndex = 234;
            this.infotipAccountAlias.Title = "Account Alias";
            //
            // alias
            //
            this.alias.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.alias.Location = new System.Drawing.Point(105, 7);
            this.fieldLengthProvider.SetMaxLengthSource(this.alias, ShipWorks.Data.Utility.EntityFieldLengthSource.EmailAccountName);
            this.alias.Name = "alias";
            this.alias.Size = new System.Drawing.Size(160, 21);
            this.alias.TabIndex = 232;
            //
            // labelAlias
            //
            this.labelAlias.AutoSize = true;
            this.labelAlias.Location = new System.Drawing.Point(24, 10);
            this.labelAlias.Name = "labelAlias";
            this.labelAlias.Size = new System.Drawing.Size(75, 13);
            this.labelAlias.TabIndex = 231;
            this.labelAlias.Text = "Account Alias:";
            //
            // AddEmailAccountWizard
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 470);
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AddEmailAccountWizard";
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageBasicInfo,
            this.wizardPageEnableAccess,
            this.wizardPageSearchFailed,
            this.wizardPageManualConfigure,
            this.wizardPageAccountAlias,
            this.wizardPageSuccess});
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Add Email Account";
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageBasicInfo.ResumeLayout(false);
            this.wizardPageBasicInfo.PerformLayout();
            this.panelAutoConfig.ResumeLayout(false);
            this.panelAutoConfig.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxManualConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxAutomaticConfig)).EndInit();
            this.wizardPageEnableAccess.ResumeLayout(false);
            this.wizardPageEnableAccess.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).EndInit();
            this.wizardPageSearchFailed.ResumeLayout(false);
            this.wizardPageSearchFailed.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox2)).EndInit();
            this.wizardPageManualConfigure.ResumeLayout(false);
            this.wizardPageSuccess.ResumeLayout(false);
            this.wizardPageSuccess.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.iconSetupComplete)).EndInit();
            this.wizardPageAccountAlias.ResumeLayout(false);
            this.wizardPageAccountAlias.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Wizard.WizardPage wizardPageBasicInfo;
        private ShipWorks.UI.Wizard.WizardPage wizardPageEnableAccess;
        private System.Windows.Forms.TextBox displayName;
        private System.Windows.Forms.Label labelDisplayName;
        private System.Windows.Forms.TextBox emailAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelGmailInfo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private ShipWorks.UI.Wizard.WizardPage wizardPageSearchFailed;
        private System.Windows.Forms.Label linkGMailPopHelp;
        private System.Windows.Forms.Label labelGMailEnablePop;
        private ShipWorks.UI.Wizard.WizardPage wizardPageManualConfigure;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private ShipWorks.UI.Wizard.WizardPage wizardPageSuccess;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox iconSetupComplete;
        private EmailAccountSettingsControl emailAccountSettings;
        private ShipWorks.UI.Wizard.WizardPage wizardPageAccountAlias;
        private System.Windows.Forms.TextBox alias;
        private System.Windows.Forms.Label labelAlias;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.RadioButton radioAutomaticConfig;
        private System.Windows.Forms.RadioButton radioManualConfig;
        private System.Windows.Forms.Label labelAutomaticConfig;
        private System.Windows.Forms.PictureBox pictureBoxAutomaticConfig;
        private System.Windows.Forms.Label labelManualConfig;
        private System.Windows.Forms.PictureBox pictureBoxManualConfig;
        private System.Windows.Forms.Panel panelAutoConfig;
        private UI.Controls.InfoTip infotipAccountAlias;
    }
}