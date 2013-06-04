namespace ShipWorks.Data.Administration.UpdateFrom2x.Configuration
{
    partial class ConfigurationMigrationWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationMigrationWizard));
            this.wizardPageInPlace = new ShipWorks.UI.Wizard.WizardPage();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.wizardPageCommonCleanup = new ShipWorks.UI.Wizard.WizardPage();
            this.label9 = new System.Windows.Forms.Label();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cleanupBox = new System.Windows.Forms.CheckBox();
            this.wizardPageSelectFolder = new ShipWorks.UI.Wizard.WizardPage();
            this.label10 = new System.Windows.Forms.Label();
            this.labelRecommended = new System.Windows.Forms.Label();
            this.labelSeperateInfo = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.radioDontUpgrade = new System.Windows.Forms.RadioButton();
            this.panelSelectFolderLocation = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.installPath = new ShipWorks.UI.Controls.PathTextBox();
            this.browse = new System.Windows.Forms.Button();
            this.radioSelectFolder = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.wizardPageFinish = new ShipWorks.UI.Wizard.WizardPage();
            this.labelSetupComplete = new System.Windows.Forms.Label();
            this.iconSetupComplete = new System.Windows.Forms.PictureBox();
            this.openShipWorksPathDlg = new System.Windows.Forms.OpenFileDialog();
            this.wizardPageUpgradeType = new ShipWorks.UI.Wizard.WizardPage();
            this.labelUpgradeLeaveInfo2 = new System.Windows.Forms.Label();
            this.labelUpgradeLeaveInfo1 = new System.Windows.Forms.Label();
            this.labelUpgradeRemoveInfo = new System.Windows.Forms.Label();
            this.labelUpgradeTypeQuestion = new System.Windows.Forms.Label();
            this.radioUpgradeLeave = new System.Windows.Forms.RadioButton();
            this.radioUpgradeRemove = new System.Windows.Forms.RadioButton();
            this.upgrade2xPath = new ShipWorks.UI.Controls.PathTextBox();
            this.labelUpgraeTypeInfo = new System.Windows.Forms.Label();
            this.pictureBoxUpradeType = new System.Windows.Forms.PictureBox();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageInPlace.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).BeginInit();
            this.wizardPageCommonCleanup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox2)).BeginInit();
            this.wizardPageSelectFolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox4)).BeginInit();
            this.panelSelectFolderLocation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox3)).BeginInit();
            this.wizardPageFinish.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.iconSetupComplete)).BeginInit();
            this.wizardPageUpgradeType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxUpradeType)).BeginInit();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(358, 345);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(439, 345);
            this.cancel.Text = "Exit";
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(277, 345);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageSelectFolder);
            this.mainPanel.Size = new System.Drawing.Size(526, 273);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 335);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.sw_cubes_big;
            // 
            // wizardPageInPlace
            // 
            this.wizardPageInPlace.Controls.Add(this.label5);
            this.wizardPageInPlace.Controls.Add(this.label2);
            this.wizardPageInPlace.Controls.Add(this.label1);
            this.wizardPageInPlace.Controls.Add(this.pictureBox1);
            this.wizardPageInPlace.Description = "Thank you for choosing Interapptive® ShipWorks®!";
            this.wizardPageInPlace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageInPlace.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageInPlace.Location = new System.Drawing.Point(0, 0);
            this.wizardPageInPlace.Name = "wizardPageInPlace";
            this.wizardPageInPlace.Size = new System.Drawing.Size(526, 273);
            this.wizardPageInPlace.TabIndex = 0;
            this.wizardPageInPlace.Title = "Welcome to ShipWorks";
            this.wizardPageInPlace.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextInPlace);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(67, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(387, 31);
            this.label5.TabIndex = 9;
            this.label5.Text = "If you are not ready to upgrade to ShipWorks 3 you can exit now and reinstall Shi" +
                "pWorks 2.";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(65, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(415, 34);
            this.label2.TabIndex = 8;
            this.label2.Text = "Some of your ShipWorks 2 settings will now be migrated to ShipWorks 3.  Your data" +
                " will be migrated later when we update your database.\r\n";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label1.Location = new System.Drawing.Point(63, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "ShipWorks 3 Upgrade";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.box_software1;
            this.pictureBox1.Location = new System.Drawing.Point(26, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // wizardPageCommonCleanup
            // 
            this.wizardPageCommonCleanup.Controls.Add(this.label9);
            this.wizardPageCommonCleanup.Controls.Add(this.pictureBox5);
            this.wizardPageCommonCleanup.Controls.Add(this.pictureBox2);
            this.wizardPageCommonCleanup.Controls.Add(this.label4);
            this.wizardPageCommonCleanup.Controls.Add(this.label3);
            this.wizardPageCommonCleanup.Controls.Add(this.cleanupBox);
            this.wizardPageCommonCleanup.Description = "Cleanup data leftover by ShipWorks 2.";
            this.wizardPageCommonCleanup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageCommonCleanup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageCommonCleanup.Location = new System.Drawing.Point(0, 0);
            this.wizardPageCommonCleanup.Name = "wizardPageCommonCleanup";
            this.wizardPageCommonCleanup.NextRequiresElevation = true;
            this.wizardPageCommonCleanup.Size = new System.Drawing.Size(501, 249);
            this.wizardPageCommonCleanup.TabIndex = 0;
            this.wizardPageCommonCleanup.Title = "ShipWorks 2 Cleanup";
            this.wizardPageCommonCleanup.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextCleanup);
            this.wizardPageCommonCleanup.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoCleanup);
            // 
            // label9
            // 
            this.label9.ForeColor = System.Drawing.Color.DimGray;
            this.label9.Location = new System.Drawing.Point(102, 116);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(330, 45);
            this.label9.TabIndex = 27;
            this.label9.Text = "Note: Unchecking this option does not keep ShipWorks 3 from removing your selecte" +
                "d ShipWorks 2 installation.  This only affects data that would be common to mult" +
                "iple installations of ShipWorks 2.";
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = global::ShipWorks.Properties.Resources.information16;
            this.pictureBox5.Location = new System.Drawing.Point(84, 116);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(16, 16);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox5.TabIndex = 26;
            this.pictureBox5.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ShipWorks.Properties.Resources.garbage_empty;
            this.pictureBox2.Location = new System.Drawing.Point(26, 10);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(32, 32);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 15;
            this.pictureBox2.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label4.Location = new System.Drawing.Point(63, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "ShipWorks 2 Cleanup";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label3.Location = new System.Drawing.Point(65, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(387, 46);
            this.label3.TabIndex = 13;
            this.label3.Text = "If you do not have any other copies of ShipWorks 2 installed on this computer we " +
                "recommend removing the extra configuration and preferences that would be left be" +
                "hind.";
            // 
            // cleanupBox
            // 
            this.cleanupBox.Checked = true;
            this.cleanupBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cleanupBox.Location = new System.Drawing.Point(66, 80);
            this.cleanupBox.Name = "cleanupBox";
            this.cleanupBox.Size = new System.Drawing.Size(362, 34);
            this.cleanupBox.TabIndex = 12;
            this.cleanupBox.Text = "I don\'t have any other copies of ShipWorks 2 installed, so cleanup any unnecessar" +
                "y ShipWorks 2 data leftover on my computer.";
            this.cleanupBox.UseVisualStyleBackColor = true;
            // 
            // wizardPageSelectFolder
            // 
            this.wizardPageSelectFolder.Controls.Add(this.label10);
            this.wizardPageSelectFolder.Controls.Add(this.labelRecommended);
            this.wizardPageSelectFolder.Controls.Add(this.labelSeperateInfo);
            this.wizardPageSelectFolder.Controls.Add(this.pictureBox4);
            this.wizardPageSelectFolder.Controls.Add(this.radioDontUpgrade);
            this.wizardPageSelectFolder.Controls.Add(this.panelSelectFolderLocation);
            this.wizardPageSelectFolder.Controls.Add(this.radioSelectFolder);
            this.wizardPageSelectFolder.Controls.Add(this.label7);
            this.wizardPageSelectFolder.Controls.Add(this.label6);
            this.wizardPageSelectFolder.Controls.Add(this.pictureBox3);
            this.wizardPageSelectFolder.Description = "Thank you for choosing Interapptive® ShipWorks®!";
            this.wizardPageSelectFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageSelectFolder.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageSelectFolder.Location = new System.Drawing.Point(0, 0);
            this.wizardPageSelectFolder.Name = "wizardPageSelectFolder";
            this.wizardPageSelectFolder.Size = new System.Drawing.Size(526, 273);
            this.wizardPageSelectFolder.TabIndex = 0;
            this.wizardPageSelectFolder.Title = "Welcome to ShipWorks";
            this.wizardPageSelectFolder.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextSelectFolder);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(83, 154);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(338, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "When using this option you will no longer be able to run ShipWorks 2.";
            // 
            // labelRecommended
            // 
            this.labelRecommended.AutoSize = true;
            this.labelRecommended.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelRecommended.ForeColor = System.Drawing.Color.Blue;
            this.labelRecommended.Location = new System.Drawing.Point(353, 54);
            this.labelRecommended.Name = "labelRecommended";
            this.labelRecommended.Size = new System.Drawing.Size(102, 13);
            this.labelRecommended.TabIndex = 26;
            this.labelRecommended.Text = "(Recommended)";
            // 
            // labelSeperateInfo
            // 
            this.labelSeperateInfo.ForeColor = System.Drawing.Color.DimGray;
            this.labelSeperateInfo.Location = new System.Drawing.Point(105, 73);
            this.labelSeperateInfo.Name = "labelSeperateInfo";
            this.labelSeperateInfo.Size = new System.Drawing.Size(386, 50);
            this.labelSeperateInfo.TabIndex = 25;
            this.labelSeperateInfo.Text = "This option allows you to import a ShipWorks 2 backup into ShipWorks 3 while not " +
                "affecting ShipWorks 2 at all.  You can get familiar with ShipWorks 3 while still" +
                " using ShipWorks 2.\r\n";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::ShipWorks.Properties.Resources.information16;
            this.pictureBox4.Location = new System.Drawing.Point(83, 73);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(16, 16);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 24;
            this.pictureBox4.TabStop = false;
            // 
            // radioDontUpgrade
            // 
            this.radioDontUpgrade.AutoSize = true;
            this.radioDontUpgrade.Location = new System.Drawing.Point(66, 52);
            this.radioDontUpgrade.Name = "radioDontUpgrade";
            this.radioDontUpgrade.Size = new System.Drawing.Size(292, 17);
            this.radioDontUpgrade.TabIndex = 0;
            this.radioDontUpgrade.TabStop = true;
            this.radioDontUpgrade.Text = "I want to use ShipWorks 3 separately from ShipWorks 2";
            this.radioDontUpgrade.UseVisualStyleBackColor = true;
            this.radioDontUpgrade.CheckedChanged += new System.EventHandler(this.OnChangeSelectFolderChoice);
            // 
            // panelSelectFolderLocation
            // 
            this.panelSelectFolderLocation.Controls.Add(this.label8);
            this.panelSelectFolderLocation.Controls.Add(this.installPath);
            this.panelSelectFolderLocation.Controls.Add(this.browse);
            this.panelSelectFolderLocation.Enabled = false;
            this.panelSelectFolderLocation.Location = new System.Drawing.Point(80, 172);
            this.panelSelectFolderLocation.Name = "panelSelectFolderLocation";
            this.panelSelectFolderLocation.Size = new System.Drawing.Size(434, 80);
            this.panelSelectFolderLocation.TabIndex = 22;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(110, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "ShipWorks 2 location:";
            // 
            // installPath
            // 
            this.installPath.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.installPath.Location = new System.Drawing.Point(6, 24);
            this.installPath.Name = "installPath";
            this.installPath.ReadOnly = true;
            this.installPath.Size = new System.Drawing.Size(422, 21);
            this.installPath.TabIndex = 20;
            // 
            // browse
            // 
            this.browse.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browse.Location = new System.Drawing.Point(353, 51);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(75, 23);
            this.browse.TabIndex = 21;
            this.browse.Text = "Browse...";
            this.browse.Click += new System.EventHandler(this.OnBrowseShipWorks);
            // 
            // radioSelectFolder
            // 
            this.radioSelectFolder.AutoSize = true;
            this.radioSelectFolder.Location = new System.Drawing.Point(66, 134);
            this.radioSelectFolder.Name = "radioSelectFolder";
            this.radioSelectFolder.Size = new System.Drawing.Size(384, 17);
            this.radioSelectFolder.TabIndex = 1;
            this.radioSelectFolder.TabStop = true;
            this.radioSelectFolder.Text = "Upgrade and remove an instance of ShipWorks 2 installed on this computer";
            this.radioSelectFolder.UseVisualStyleBackColor = true;
            this.radioSelectFolder.CheckedChanged += new System.EventHandler(this.OnChangeSelectFolderChoice);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label7.Location = new System.Drawing.Point(63, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(128, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "ShipWorks 3 Upgrade";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(63, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(401, 21);
            this.label6.TabIndex = 17;
            this.label6.Text = "ShipWorks 2 was found installed on your computer.";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::ShipWorks.Properties.Resources.box_software1;
            this.pictureBox3.Location = new System.Drawing.Point(26, 10);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(32, 32);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 16;
            this.pictureBox3.TabStop = false;
            // 
            // wizardPageFinish
            // 
            this.wizardPageFinish.Controls.Add(this.labelSetupComplete);
            this.wizardPageFinish.Controls.Add(this.iconSetupComplete);
            this.wizardPageFinish.Description = "Thank you for choosing Interapptive® ShipWorks®!";
            this.wizardPageFinish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageFinish.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageFinish.Location = new System.Drawing.Point(0, 0);
            this.wizardPageFinish.Name = "wizardPageFinish";
            this.wizardPageFinish.Size = new System.Drawing.Size(526, 273);
            this.wizardPageFinish.TabIndex = 0;
            this.wizardPageFinish.Title = "ShipWorks 3";
            // 
            // labelSetupComplete
            // 
            this.labelSetupComplete.AutoSize = true;
            this.labelSetupComplete.Location = new System.Drawing.Point(43, 12);
            this.labelSetupComplete.Name = "labelSetupComplete";
            this.labelSetupComplete.Size = new System.Drawing.Size(176, 13);
            this.labelSetupComplete.TabIndex = 3;
            this.labelSetupComplete.Text = "ShipWorks 3 is now ready to begin.";
            // 
            // iconSetupComplete
            // 
            this.iconSetupComplete.Image = global::ShipWorks.Properties.Resources.check16;
            this.iconSetupComplete.Location = new System.Drawing.Point(23, 9);
            this.iconSetupComplete.Name = "iconSetupComplete";
            this.iconSetupComplete.Size = new System.Drawing.Size(16, 16);
            this.iconSetupComplete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.iconSetupComplete.TabIndex = 2;
            this.iconSetupComplete.TabStop = false;
            // 
            // openShipWorksPathDlg
            // 
            this.openShipWorksPathDlg.Filter = "ShipWorks 2|shipworks.exe";
            this.openShipWorksPathDlg.Title = "Browse for ShipWorks 2";
            this.openShipWorksPathDlg.FileOk += new System.ComponentModel.CancelEventHandler(this.OnBrowseValidateShipWorksPath);
            // 
            // wizardPageUpgradeType
            // 
            this.wizardPageUpgradeType.Controls.Add(this.labelUpgradeLeaveInfo2);
            this.wizardPageUpgradeType.Controls.Add(this.labelUpgradeLeaveInfo1);
            this.wizardPageUpgradeType.Controls.Add(this.labelUpgradeRemoveInfo);
            this.wizardPageUpgradeType.Controls.Add(this.labelUpgradeTypeQuestion);
            this.wizardPageUpgradeType.Controls.Add(this.radioUpgradeLeave);
            this.wizardPageUpgradeType.Controls.Add(this.radioUpgradeRemove);
            this.wizardPageUpgradeType.Controls.Add(this.upgrade2xPath);
            this.wizardPageUpgradeType.Controls.Add(this.labelUpgraeTypeInfo);
            this.wizardPageUpgradeType.Controls.Add(this.pictureBoxUpradeType);
            this.wizardPageUpgradeType.Description = "Specify how ShipWorks 3 should upgrade from ShipWorks 2.";
            this.wizardPageUpgradeType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageUpgradeType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageUpgradeType.Location = new System.Drawing.Point(0, 0);
            this.wizardPageUpgradeType.Name = "wizardPageUpgradeType";
            this.wizardPageUpgradeType.Size = new System.Drawing.Size(501, 249);
            this.wizardPageUpgradeType.TabIndex = 0;
            this.wizardPageUpgradeType.Title = "ShipWorks 2 Upgrade";
            this.wizardPageUpgradeType.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextUpgradeType);
            this.wizardPageUpgradeType.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoUpgradeType);
            // 
            // labelUpgradeLeaveInfo2
            // 
            this.labelUpgradeLeaveInfo2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelUpgradeLeaveInfo2.ForeColor = System.Drawing.Color.Red;
            this.labelUpgradeLeaveInfo2.Location = new System.Drawing.Point(102, 171);
            this.labelUpgradeLeaveInfo2.Name = "labelUpgradeLeaveInfo2";
            this.labelUpgradeLeaveInfo2.Size = new System.Drawing.Size(330, 57);
            this.labelUpgradeLeaveInfo2.TabIndex = 28;
            this.labelUpgradeLeaveInfo2.Text = resources.GetString("labelUpgradeLeaveInfo2.Text");
            // 
            // labelUpgradeLeaveInfo1
            // 
            this.labelUpgradeLeaveInfo1.ForeColor = System.Drawing.Color.DimGray;
            this.labelUpgradeLeaveInfo1.Location = new System.Drawing.Point(102, 140);
            this.labelUpgradeLeaveInfo1.Name = "labelUpgradeLeaveInfo1";
            this.labelUpgradeLeaveInfo1.Size = new System.Drawing.Size(371, 30);
            this.labelUpgradeLeaveInfo1.TabIndex = 26;
            this.labelUpgradeLeaveInfo1.Text = "Copy the database and user settings from ShipWorks 2, but otherwise leave the Shi" +
                "pWorks 2 installation alone.";
            // 
            // labelUpgradeRemoveInfo
            // 
            this.labelUpgradeRemoveInfo.AutoSize = true;
            this.labelUpgradeRemoveInfo.ForeColor = System.Drawing.Color.DimGray;
            this.labelUpgradeRemoveInfo.Location = new System.Drawing.Point(102, 103);
            this.labelUpgradeRemoveInfo.Name = "labelUpgradeRemoveInfo";
            this.labelUpgradeRemoveInfo.Size = new System.Drawing.Size(337, 13);
            this.labelUpgradeRemoveInfo.TabIndex = 25;
            this.labelUpgradeRemoveInfo.Text = "Permanently upgrade from and remove this instance of ShipWorks 2.";
            // 
            // labelUpgradeTypeQuestion
            // 
            this.labelUpgradeTypeQuestion.AutoSize = true;
            this.labelUpgradeTypeQuestion.Location = new System.Drawing.Point(62, 65);
            this.labelUpgradeTypeQuestion.Name = "labelUpgradeTypeQuestion";
            this.labelUpgradeTypeQuestion.Size = new System.Drawing.Size(275, 13);
            this.labelUpgradeTypeQuestion.TabIndex = 24;
            this.labelUpgradeTypeQuestion.Text = "What do you want ShipWorks 3 to do with ShipWorks 2:";
            // 
            // radioUpgradeLeave
            // 
            this.radioUpgradeLeave.AutoSize = true;
            this.radioUpgradeLeave.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.radioUpgradeLeave.Location = new System.Drawing.Point(84, 121);
            this.radioUpgradeLeave.Name = "radioUpgradeLeave";
            this.radioUpgradeLeave.Size = new System.Drawing.Size(72, 17);
            this.radioUpgradeLeave.TabIndex = 23;
            this.radioUpgradeLeave.TabStop = true;
            this.radioUpgradeLeave.Text = "Leave It";
            this.radioUpgradeLeave.UseVisualStyleBackColor = true;
            // 
            // radioUpgradeRemove
            // 
            this.radioUpgradeRemove.AutoSize = true;
            this.radioUpgradeRemove.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.radioUpgradeRemove.Location = new System.Drawing.Point(84, 84);
            this.radioUpgradeRemove.Name = "radioUpgradeRemove";
            this.radioUpgradeRemove.Size = new System.Drawing.Size(85, 17);
            this.radioUpgradeRemove.TabIndex = 22;
            this.radioUpgradeRemove.TabStop = true;
            this.radioUpgradeRemove.Text = "Remove It";
            this.radioUpgradeRemove.UseVisualStyleBackColor = true;
            // 
            // upgrade2xPath
            // 
            this.upgrade2xPath.Location = new System.Drawing.Point(84, 29);
            this.upgrade2xPath.Name = "upgrade2xPath";
            this.upgrade2xPath.ReadOnly = true;
            this.upgrade2xPath.Size = new System.Drawing.Size(324, 21);
            this.upgrade2xPath.TabIndex = 21;
            // 
            // labelUpgraeTypeInfo
            // 
            this.labelUpgraeTypeInfo.Location = new System.Drawing.Point(62, 11);
            this.labelUpgraeTypeInfo.Name = "labelUpgraeTypeInfo";
            this.labelUpgraeTypeInfo.Size = new System.Drawing.Size(401, 21);
            this.labelUpgraeTypeInfo.TabIndex = 20;
            this.labelUpgraeTypeInfo.Text = "You have chosen to upgrade from the following installation of ShipWorks 2:";
            // 
            // pictureBoxUpradeType
            // 
            this.pictureBoxUpradeType.Image = global::ShipWorks.Properties.Resources.signpost;
            this.pictureBoxUpradeType.Location = new System.Drawing.Point(25, 9);
            this.pictureBoxUpradeType.Name = "pictureBoxUpradeType";
            this.pictureBoxUpradeType.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxUpradeType.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxUpradeType.TabIndex = 19;
            this.pictureBoxUpradeType.TabStop = false;
            // 
            // ConfigurationMigrationWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 380);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ConfigurationMigrationWizard";
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageInPlace,
            this.wizardPageSelectFolder,
            this.wizardPageUpgradeType,
            this.wizardPageCommonCleanup,
            this.wizardPageFinish});
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ShipWorks Upgrade";
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageInPlace.ResumeLayout(false);
            this.wizardPageInPlace.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).EndInit();
            this.wizardPageCommonCleanup.ResumeLayout(false);
            this.wizardPageCommonCleanup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox2)).EndInit();
            this.wizardPageSelectFolder.ResumeLayout(false);
            this.wizardPageSelectFolder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox4)).EndInit();
            this.panelSelectFolderLocation.ResumeLayout(false);
            this.panelSelectFolderLocation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox3)).EndInit();
            this.wizardPageFinish.ResumeLayout(false);
            this.wizardPageFinish.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.iconSetupComplete)).EndInit();
            this.wizardPageUpgradeType.ResumeLayout(false);
            this.wizardPageUpgradeType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxUpradeType)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Wizard.WizardPage wizardPageInPlace;
        private ShipWorks.UI.Wizard.WizardPage wizardPageCommonCleanup;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cleanupBox;
        private ShipWorks.UI.Wizard.WizardPage wizardPageSelectFolder;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox2;
        private ShipWorks.UI.Wizard.WizardPage wizardPageFinish;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.RadioButton radioSelectFolder;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panelSelectFolderLocation;
        private ShipWorks.UI.Controls.PathTextBox installPath;
        private System.Windows.Forms.Button browse;
        private System.Windows.Forms.RadioButton radioDontUpgrade;
        private System.Windows.Forms.Label labelSetupComplete;
        private System.Windows.Forms.PictureBox iconSetupComplete;
        private System.Windows.Forms.OpenFileDialog openShipWorksPathDlg;
        private System.Windows.Forms.Label labelSeperateInfo;
        private System.Windows.Forms.PictureBox pictureBox4;
        private UI.Wizard.WizardPage wizardPageUpgradeType;
        private System.Windows.Forms.Label labelUpgraeTypeInfo;
        private System.Windows.Forms.PictureBox pictureBoxUpradeType;
        private UI.Controls.PathTextBox upgrade2xPath;
        private System.Windows.Forms.RadioButton radioUpgradeRemove;
        private System.Windows.Forms.RadioButton radioUpgradeLeave;
        private System.Windows.Forms.Label labelUpgradeRemoveInfo;
        private System.Windows.Forms.Label labelUpgradeTypeQuestion;
        private System.Windows.Forms.Label labelUpgradeLeaveInfo1;
        private System.Windows.Forms.Label labelUpgradeLeaveInfo2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Label labelRecommended;
        private System.Windows.Forms.Label label10;
    }
}
