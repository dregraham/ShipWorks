namespace ShipWorks.Data.Administration
{
    partial class DatabaseSetupWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseSetupWizard));
            this.wizardPageSetupOrConnect = new ShipWorks.UI.Wizard.WizardPage();
            this.labelRestoreDatabase = new System.Windows.Forms.Label();
            this.pictureBoxRestoreDatabase = new System.Windows.Forms.PictureBox();
            this.labelConnectRunningDatabase = new System.Windows.Forms.Label();
            this.pictureBoxConnectRunningDatabase = new System.Windows.Forms.PictureBox();
            this.labelSetupNewDatabase = new System.Windows.Forms.Label();
            this.pictureBoxSetupNewDatabase = new System.Windows.Forms.PictureBox();
            this.radioRestoreDatabase = new System.Windows.Forms.RadioButton();
            this.radioConnectRunningDatabase = new System.Windows.Forms.RadioButton();
            this.radioSetupNewDatabase = new System.Windows.Forms.RadioButton();
            this.labelShipWorksUsesSqlServer = new System.Windows.Forms.Label();
            this.wizardPageChooseSqlServer = new ShipWorks.UI.Wizard.WizardPage();
            this.infoTip1 = new ShipWorks.UI.Controls.InfoTip();
            this.label5 = new System.Windows.Forms.Label();
            this.radioSqlServerAlreadyInstalled = new System.Windows.Forms.RadioButton();
            this.instanceName = new System.Windows.Forms.TextBox();
            this.labelInstanceName = new System.Windows.Forms.Label();
            this.radioInstallSqlServer = new System.Windows.Forms.RadioButton();
            this.labelChooseNewOrExistingSqlServer = new System.Windows.Forms.Label();
            this.wizardPageNewCredentials = new ShipWorks.UI.Wizard.WizardPage();
            this.labelSaWarning = new System.Windows.Forms.Label();
            this.labelImportantSa = new System.Windows.Forms.Label();
            this.iconWarningSa = new System.Windows.Forms.PictureBox();
            this.saPasswordAgain = new System.Windows.Forms.TextBox();
            this.saPassword = new System.Windows.Forms.TextBox();
            this.textBoxSa = new System.Windows.Forms.TextBox();
            this.labelSaPasswordAgain = new System.Windows.Forms.Label();
            this.labelSaPassword = new System.Windows.Forms.Label();
            this.labelSaUsername = new System.Windows.Forms.Label();
            this.labelAboutSa = new System.Windows.Forms.Label();
            this.wizardPageSelectSqlServerInstance = new ShipWorks.UI.Wizard.WizardPage();
            this.labelServerSearching = new System.Windows.Forms.Label();
            this.pictureServerSearching = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelFoundInstanceTroubleshooting = new System.Windows.Forms.Label();
            this.comboSqlServers = new System.Windows.Forms.ComboBox();
            this.labelFoundInstances = new System.Windows.Forms.Label();
            this.wizardPageLoginSqlServer = new ShipWorks.UI.Wizard.WizardPage();
            this.remember = new System.Windows.Forms.CheckBox();
            this.password = new System.Windows.Forms.TextBox();
            this.username = new System.Windows.Forms.TextBox();
            this.labelConnectUsing = new System.Windows.Forms.Label();
            this.labelNeedLoginInfo = new System.Windows.Forms.Label();
            this.labelWindowsAuthDescription = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelUsername = new System.Windows.Forms.Label();
            this.sqlServerAuth = new System.Windows.Forms.RadioButton();
            this.windowsAuth = new System.Windows.Forms.RadioButton();
            this.wizardPageDatabaseName = new ShipWorks.UI.Wizard.WizardPage();
            this.infoTipDatabaseName = new ShipWorks.UI.Controls.InfoTip();
            this.databaseName = new System.Windows.Forms.TextBox();
            this.labelDatabaseName = new System.Windows.Forms.Label();
            this.labelEnterDatabaseName = new System.Windows.Forms.Label();
            this.wizardPageChooseDatabase = new ShipWorks.UI.Wizard.WizardPage();
            this.databaseNames = new System.Windows.Forms.ComboBox();
            this.labelFoundDatabases = new System.Windows.Forms.Label();
            this.wizardPageDownloadSqlServer = new ShipWorks.UI.Wizard.WizardPage();
            this.bytesSqlServer = new System.Windows.Forms.Label();
            this.downloadSqlServer = new System.Windows.Forms.Button();
            this.progressSqlServer = new System.Windows.Forms.ProgressBar();
            this.labelDownloadSqlServer = new System.Windows.Forms.Label();
            this.wizardPageInstallSqlServer = new ShipWorks.UI.Wizard.WizardPage();
            this.labelInstallSqlServer = new System.Windows.Forms.Panel();
            this.label21 = new System.Windows.Forms.Label();
            this.labelNote = new System.Windows.Forms.Label();
            this.labelWarning = new System.Windows.Forms.Label();
            this.labelInstallingSqlServer = new System.Windows.Forms.Label();
            this.wizardPageComplete = new ShipWorks.UI.Wizard.WizardPage();
            this.labelSetupComplete = new System.Windows.Forms.Label();
            this.iconSetupComplete = new System.Windows.Forms.PictureBox();
            this.wizardPageShipWorksAdmin = new ShipWorks.UI.Wizard.WizardPage();
            this.helpAutomaticLogon = new ShipWorks.UI.Controls.InfoTip();
            this.helpUserEmail = new ShipWorks.UI.Controls.InfoTip();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.label11 = new System.Windows.Forms.Label();
            this.swAutomaticLogon = new System.Windows.Forms.CheckBox();
            this.swEmail = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.swPasswordAgain = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.swPassword = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.swUsername = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.wizardPageRestoreDatabase = new ShipWorks.UI.Wizard.WizardPage();
            this.labelCantRestore = new System.Windows.Forms.Label();
            this.groupInfo = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.labelNote1 = new System.Windows.Forms.Label();
            this.warningIcon = new System.Windows.Forms.PictureBox();
            this.labelNote2 = new System.Windows.Forms.Label();
            this.backupFile = new ShipWorks.UI.Controls.PathTextBox();
            this.browseForBackupFile = new System.Windows.Forms.Button();
            this.labelBackupFile = new System.Windows.Forms.Label();
            this.wizardPageRestoreOption = new ShipWorks.UI.Wizard.WizardPage();
            this.label12 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.radioRestoreIntoNewDatabase = new System.Windows.Forms.RadioButton();
            this.radioRestoreIntoCurrent = new System.Windows.Forms.RadioButton();
            this.openBackupFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.wizardPageRestoreLogin = new ShipWorks.UI.Wizard.WizardPage();
            this.headerImage = new System.Windows.Forms.PictureBox();
            this.restorePassword = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.restoreUsername = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.wizardPageWindowsFirewall = new ShipWorks.UI.Wizard.WizardPage();
            this.firewallUpdatedLabel = new System.Windows.Forms.Label();
            this.firewallUpdatedPicture = new System.Windows.Forms.PictureBox();
            this.label19 = new System.Windows.Forms.Label();
            this.updateWindowsFirewall = new ShipWorks.UI.Controls.ShieldButton();
            this.label14 = new System.Windows.Forms.Label();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.wizardPagePrerequisitePlaceholder = new ShipWorks.UI.Wizard.WizardPage();
            this.label24 = new System.Windows.Forms.Label();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageSetupOrConnect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxRestoreDatabase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxConnectRunningDatabase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxSetupNewDatabase)).BeginInit();
            this.wizardPageChooseSqlServer.SuspendLayout();
            this.wizardPageNewCredentials.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.iconWarningSa)).BeginInit();
            this.wizardPageSelectSqlServerInstance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureServerSearching)).BeginInit();
            this.wizardPageLoginSqlServer.SuspendLayout();
            this.wizardPageDatabaseName.SuspendLayout();
            this.wizardPageChooseDatabase.SuspendLayout();
            this.wizardPageDownloadSqlServer.SuspendLayout();
            this.wizardPageInstallSqlServer.SuspendLayout();
            this.labelInstallSqlServer.SuspendLayout();
            this.wizardPageComplete.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.iconSetupComplete)).BeginInit();
            this.wizardPageShipWorksAdmin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox6)).BeginInit();
            this.wizardPageRestoreDatabase.SuspendLayout();
            this.groupInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.warningIcon)).BeginInit();
            this.wizardPageRestoreOption.SuspendLayout();
            this.wizardPageRestoreLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.headerImage)).BeginInit();
            this.wizardPageWindowsFirewall.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.firewallUpdatedPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox5)).BeginInit();
            this.wizardPagePrerequisitePlaceholder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(380, 343);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(461, 343);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(299, 343);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageSetupOrConnect);
            this.mainPanel.Size = new System.Drawing.Size(548, 271);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 333);
            this.etchBottom.Size = new System.Drawing.Size(552, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image) (resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(495, 3);
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(548, 56);
            // 
            // wizardPageSetupOrConnect
            // 
            this.wizardPageSetupOrConnect.Controls.Add(this.labelRestoreDatabase);
            this.wizardPageSetupOrConnect.Controls.Add(this.pictureBoxRestoreDatabase);
            this.wizardPageSetupOrConnect.Controls.Add(this.labelConnectRunningDatabase);
            this.wizardPageSetupOrConnect.Controls.Add(this.pictureBoxConnectRunningDatabase);
            this.wizardPageSetupOrConnect.Controls.Add(this.labelSetupNewDatabase);
            this.wizardPageSetupOrConnect.Controls.Add(this.pictureBoxSetupNewDatabase);
            this.wizardPageSetupOrConnect.Controls.Add(this.radioRestoreDatabase);
            this.wizardPageSetupOrConnect.Controls.Add(this.radioConnectRunningDatabase);
            this.wizardPageSetupOrConnect.Controls.Add(this.radioSetupNewDatabase);
            this.wizardPageSetupOrConnect.Controls.Add(this.labelShipWorksUsesSqlServer);
            this.wizardPageSetupOrConnect.Description = "Setup or connect to a ShipWorks database.";
            this.wizardPageSetupOrConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageSetupOrConnect.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageSetupOrConnect.Location = new System.Drawing.Point(0, 0);
            this.wizardPageSetupOrConnect.Name = "wizardPageSetupOrConnect";
            this.wizardPageSetupOrConnect.Size = new System.Drawing.Size(548, 271);
            this.wizardPageSetupOrConnect.TabIndex = 0;
            this.wizardPageSetupOrConnect.Title = "Database Setup";
            this.wizardPageSetupOrConnect.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextSetupOrConnect);
            // 
            // labelRestoreDatabase
            // 
            this.labelRestoreDatabase.ForeColor = System.Drawing.Color.DimGray;
            this.labelRestoreDatabase.Location = new System.Drawing.Point(93, 213);
            this.labelRestoreDatabase.Name = "labelRestoreDatabase";
            this.labelRestoreDatabase.Size = new System.Drawing.Size(406, 15);
            this.labelRestoreDatabase.TabIndex = 6;
            this.labelRestoreDatabase.Text = "Select this option to load a database from a ShipWorks backup.";
            // 
            // pictureBoxRestoreDatabase
            // 
            this.pictureBoxRestoreDatabase.Image = ((System.Drawing.Image) (resources.GetObject("pictureBoxRestoreDatabase.Image")));
            this.pictureBoxRestoreDatabase.Location = new System.Drawing.Point(55, 206);
            this.pictureBoxRestoreDatabase.Name = "pictureBoxRestoreDatabase";
            this.pictureBoxRestoreDatabase.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxRestoreDatabase.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxRestoreDatabase.TabIndex = 9;
            this.pictureBoxRestoreDatabase.TabStop = false;
            // 
            // labelConnectRunningDatabase
            // 
            this.labelConnectRunningDatabase.ForeColor = System.Drawing.Color.DimGray;
            this.labelConnectRunningDatabase.Location = new System.Drawing.Point(93, 139);
            this.labelConnectRunningDatabase.Name = "labelConnectRunningDatabase";
            this.labelConnectRunningDatabase.Size = new System.Drawing.Size(406, 36);
            this.labelConnectRunningDatabase.TabIndex = 4;
            this.labelConnectRunningDatabase.Text = "Select this option to connect to a ShipWorks database that is already running on " +
                "this computer or another computer.";
            // 
            // pictureBoxConnectRunningDatabase
            // 
            this.pictureBoxConnectRunningDatabase.Image = ((System.Drawing.Image) (resources.GetObject("pictureBoxConnectRunningDatabase.Image")));
            this.pictureBoxConnectRunningDatabase.Location = new System.Drawing.Point(55, 136);
            this.pictureBoxConnectRunningDatabase.Name = "pictureBoxConnectRunningDatabase";
            this.pictureBoxConnectRunningDatabase.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxConnectRunningDatabase.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxConnectRunningDatabase.TabIndex = 7;
            this.pictureBoxConnectRunningDatabase.TabStop = false;
            // 
            // labelSetupNewDatabase
            // 
            this.labelSetupNewDatabase.AutoSize = true;
            this.labelSetupNewDatabase.ForeColor = System.Drawing.Color.DimGray;
            this.labelSetupNewDatabase.Location = new System.Drawing.Point(93, 75);
            this.labelSetupNewDatabase.Name = "labelSetupNewDatabase";
            this.labelSetupNewDatabase.Size = new System.Drawing.Size(299, 13);
            this.labelSetupNewDatabase.TabIndex = 2;
            this.labelSetupNewDatabase.Text = "Select this option if this is your first time installing ShipWorks.";
            // 
            // pictureBoxSetupNewDatabase
            // 
            this.pictureBoxSetupNewDatabase.Image = ((System.Drawing.Image) (resources.GetObject("pictureBoxSetupNewDatabase.Image")));
            this.pictureBoxSetupNewDatabase.Location = new System.Drawing.Point(55, 67);
            this.pictureBoxSetupNewDatabase.Name = "pictureBoxSetupNewDatabase";
            this.pictureBoxSetupNewDatabase.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxSetupNewDatabase.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxSetupNewDatabase.TabIndex = 5;
            this.pictureBoxSetupNewDatabase.TabStop = false;
            // 
            // radioRestoreDatabase
            // 
            this.radioRestoreDatabase.AutoSize = true;
            this.radioRestoreDatabase.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.radioRestoreDatabase.Location = new System.Drawing.Point(39, 187);
            this.radioRestoreDatabase.Name = "radioRestoreDatabase";
            this.radioRestoreDatabase.Size = new System.Drawing.Size(181, 17);
            this.radioRestoreDatabase.TabIndex = 5;
            this.radioRestoreDatabase.Text = "Restore a Database Backup";
            this.radioRestoreDatabase.UseVisualStyleBackColor = true;
            // 
            // radioConnectRunningDatabase
            // 
            this.radioConnectRunningDatabase.AutoSize = true;
            this.radioConnectRunningDatabase.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.radioConnectRunningDatabase.Location = new System.Drawing.Point(39, 117);
            this.radioConnectRunningDatabase.Name = "radioConnectRunningDatabase";
            this.radioConnectRunningDatabase.Size = new System.Drawing.Size(202, 17);
            this.radioConnectRunningDatabase.TabIndex = 3;
            this.radioConnectRunningDatabase.Text = "Connect to a Running Database";
            this.radioConnectRunningDatabase.UseVisualStyleBackColor = true;
            // 
            // radioSetupNewDatabase
            // 
            this.radioSetupNewDatabase.AutoSize = true;
            this.radioSetupNewDatabase.Checked = true;
            this.radioSetupNewDatabase.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.radioSetupNewDatabase.Location = new System.Drawing.Point(39, 48);
            this.radioSetupNewDatabase.Name = "radioSetupNewDatabase";
            this.radioSetupNewDatabase.Size = new System.Drawing.Size(151, 17);
            this.radioSetupNewDatabase.TabIndex = 1;
            this.radioSetupNewDatabase.TabStop = true;
            this.radioSetupNewDatabase.Text = "Setup a New Database";
            this.radioSetupNewDatabase.UseVisualStyleBackColor = true;
            // 
            // labelShipWorksUsesSqlServer
            // 
            this.labelShipWorksUsesSqlServer.Location = new System.Drawing.Point(24, 8);
            this.labelShipWorksUsesSqlServer.Name = "labelShipWorksUsesSqlServer";
            this.labelShipWorksUsesSqlServer.Size = new System.Drawing.Size(508, 34);
            this.labelShipWorksUsesSqlServer.TabIndex = 0;
            this.labelShipWorksUsesSqlServer.Text = "ShipWorks uses Microsoft SQL Server 2008 to store its data.  Please select from o" +
                "ne of the following options to continue.\r\n";
            // 
            // wizardPageChooseSqlServer
            // 
            this.wizardPageChooseSqlServer.Controls.Add(this.infoTip1);
            this.wizardPageChooseSqlServer.Controls.Add(this.label5);
            this.wizardPageChooseSqlServer.Controls.Add(this.radioSqlServerAlreadyInstalled);
            this.wizardPageChooseSqlServer.Controls.Add(this.instanceName);
            this.wizardPageChooseSqlServer.Controls.Add(this.labelInstanceName);
            this.wizardPageChooseSqlServer.Controls.Add(this.radioInstallSqlServer);
            this.wizardPageChooseSqlServer.Controls.Add(this.labelChooseNewOrExistingSqlServer);
            this.wizardPageChooseSqlServer.Description = "Install or use an existing installation of Microsoft SQL Server 2008.";
            this.wizardPageChooseSqlServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageChooseSqlServer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageChooseSqlServer.Location = new System.Drawing.Point(0, 0);
            this.wizardPageChooseSqlServer.Name = "wizardPageChooseSqlServer";
            this.wizardPageChooseSqlServer.Size = new System.Drawing.Size(548, 271);
            this.wizardPageChooseSqlServer.TabIndex = 0;
            this.wizardPageChooseSqlServer.Title = "Setup Microsoft SQL Server 2008";
            this.wizardPageChooseSqlServer.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextNewOrExistingSqlServer);
            // 
            // infoTip1
            // 
            this.infoTip1.Caption = "Multiple instances of Microsoft SQL Server 2008 can be installed on a computer.  " +
                "Each one must have a unique instance name that is used when connecting to it.";
            this.infoTip1.Location = new System.Drawing.Point(252, 82);
            this.infoTip1.Name = "infoTip1";
            this.infoTip1.Size = new System.Drawing.Size(12, 12);
            this.infoTip1.TabIndex = 7;
            this.infoTip1.Title = "Instance Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.DimGray;
            this.label5.Location = new System.Drawing.Point(56, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(437, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Select this option if you already own or have already installed Microsoft SQL Ser" +
                "ver 2008.";
            // 
            // radioSqlServerAlreadyInstalled
            // 
            this.radioSqlServerAlreadyInstalled.AutoSize = true;
            this.radioSqlServerAlreadyInstalled.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.radioSqlServerAlreadyInstalled.Location = new System.Drawing.Point(39, 115);
            this.radioSqlServerAlreadyInstalled.Name = "radioSqlServerAlreadyInstalled";
            this.radioSqlServerAlreadyInstalled.Size = new System.Drawing.Size(346, 17);
            this.radioSqlServerAlreadyInstalled.TabIndex = 4;
            this.radioSqlServerAlreadyInstalled.Text = "Use an existing installation of Microsoft SQL Server 2008.";
            this.radioSqlServerAlreadyInstalled.UseVisualStyleBackColor = true;
            this.radioSqlServerAlreadyInstalled.CheckedChanged += new System.EventHandler(this.OnChangeInstallSqlServer);
            // 
            // instanceName
            // 
            this.instanceName.Location = new System.Drawing.Point(146, 78);
            this.instanceName.Name = "instanceName";
            this.instanceName.Size = new System.Drawing.Size(100, 21);
            this.instanceName.TabIndex = 3;
            this.instanceName.Text = "SHIPWORKS";
            // 
            // labelInstanceName
            // 
            this.labelInstanceName.AutoSize = true;
            this.labelInstanceName.Location = new System.Drawing.Point(59, 81);
            this.labelInstanceName.Name = "labelInstanceName";
            this.labelInstanceName.Size = new System.Drawing.Size(82, 13);
            this.labelInstanceName.TabIndex = 2;
            this.labelInstanceName.Text = "Instance name:";
            // 
            // radioInstallSqlServer
            // 
            this.radioInstallSqlServer.AutoSize = true;
            this.radioInstallSqlServer.Checked = true;
            this.radioInstallSqlServer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.radioInstallSqlServer.Location = new System.Drawing.Point(39, 53);
            this.radioInstallSqlServer.Name = "radioInstallSqlServer";
            this.radioInstallSqlServer.Size = new System.Drawing.Size(216, 17);
            this.radioInstallSqlServer.TabIndex = 1;
            this.radioInstallSqlServer.TabStop = true;
            this.radioInstallSqlServer.Text = "Install Microsoft SQL Server 2008.";
            this.radioInstallSqlServer.UseVisualStyleBackColor = true;
            this.radioInstallSqlServer.CheckedChanged += new System.EventHandler(this.OnChangeInstallSqlServer);
            // 
            // labelChooseNewOrExistingSqlServer
            // 
            this.labelChooseNewOrExistingSqlServer.Location = new System.Drawing.Point(24, 8);
            this.labelChooseNewOrExistingSqlServer.Name = "labelChooseNewOrExistingSqlServer";
            this.labelChooseNewOrExistingSqlServer.Size = new System.Drawing.Size(511, 36);
            this.labelChooseNewOrExistingSqlServer.TabIndex = 0;
            this.labelChooseNewOrExistingSqlServer.Text = resources.GetString("labelChooseNewOrExistingSqlServer.Text");
            // 
            // wizardPageNewCredentials
            // 
            this.wizardPageNewCredentials.Controls.Add(this.labelSaWarning);
            this.wizardPageNewCredentials.Controls.Add(this.labelImportantSa);
            this.wizardPageNewCredentials.Controls.Add(this.iconWarningSa);
            this.wizardPageNewCredentials.Controls.Add(this.saPasswordAgain);
            this.wizardPageNewCredentials.Controls.Add(this.saPassword);
            this.wizardPageNewCredentials.Controls.Add(this.textBoxSa);
            this.wizardPageNewCredentials.Controls.Add(this.labelSaPasswordAgain);
            this.wizardPageNewCredentials.Controls.Add(this.labelSaPassword);
            this.wizardPageNewCredentials.Controls.Add(this.labelSaUsername);
            this.wizardPageNewCredentials.Controls.Add(this.labelAboutSa);
            this.wizardPageNewCredentials.Description = "Enter the username and password to use for SQL Server.";
            this.wizardPageNewCredentials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageNewCredentials.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageNewCredentials.Location = new System.Drawing.Point(0, 0);
            this.wizardPageNewCredentials.Name = "wizardPageNewCredentials";
            this.wizardPageNewCredentials.Size = new System.Drawing.Size(548, 271);
            this.wizardPageNewCredentials.TabIndex = 0;
            this.wizardPageNewCredentials.Title = "SQL Server Account";
            this.wizardPageNewCredentials.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextCreateSa);
            this.wizardPageNewCredentials.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoCreateSa);
            // 
            // labelSaWarning
            // 
            this.labelSaWarning.Location = new System.Drawing.Point(121, 174);
            this.labelSaWarning.Name = "labelSaWarning";
            this.labelSaWarning.Size = new System.Drawing.Size(398, 32);
            this.labelSaWarning.TabIndex = 9;
            this.labelSaWarning.Text = "Store this password in a safe and secure place.  If this password is lost, you ma" +
                "y not be able to recover your database.";
            // 
            // labelImportantSa
            // 
            this.labelImportantSa.AutoSize = true;
            this.labelImportantSa.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelImportantSa.Location = new System.Drawing.Point(44, 174);
            this.labelImportantSa.Name = "labelImportantSa";
            this.labelImportantSa.Size = new System.Drawing.Size(69, 13);
            this.labelImportantSa.TabIndex = 8;
            this.labelImportantSa.Text = "Important:";
            // 
            // iconWarningSa
            // 
            this.iconWarningSa.Image = global::ShipWorks.Properties.Resources.warning16;
            this.iconWarningSa.Location = new System.Drawing.Point(26, 172);
            this.iconWarningSa.Name = "iconWarningSa";
            this.iconWarningSa.Size = new System.Drawing.Size(16, 16);
            this.iconWarningSa.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.iconWarningSa.TabIndex = 7;
            this.iconWarningSa.TabStop = false;
            // 
            // saPasswordAgain
            // 
            this.saPasswordAgain.Location = new System.Drawing.Point(121, 112);
            this.saPasswordAgain.Name = "saPasswordAgain";
            this.saPasswordAgain.Size = new System.Drawing.Size(178, 21);
            this.saPasswordAgain.TabIndex = 6;
            this.saPasswordAgain.UseSystemPasswordChar = true;
            // 
            // saPassword
            // 
            this.saPassword.Location = new System.Drawing.Point(121, 85);
            this.saPassword.Name = "saPassword";
            this.saPassword.Size = new System.Drawing.Size(178, 21);
            this.saPassword.TabIndex = 5;
            this.saPassword.UseSystemPasswordChar = true;
            // 
            // textBoxSa
            // 
            this.textBoxSa.Location = new System.Drawing.Point(121, 58);
            this.textBoxSa.Name = "textBoxSa";
            this.textBoxSa.ReadOnly = true;
            this.textBoxSa.Size = new System.Drawing.Size(178, 21);
            this.textBoxSa.TabIndex = 4;
            this.textBoxSa.Text = "sa";
            // 
            // labelSaPasswordAgain
            // 
            this.labelSaPasswordAgain.AutoSize = true;
            this.labelSaPasswordAgain.Location = new System.Drawing.Point(19, 115);
            this.labelSaPasswordAgain.Name = "labelSaPasswordAgain";
            this.labelSaPasswordAgain.Size = new System.Drawing.Size(94, 13);
            this.labelSaPasswordAgain.TabIndex = 3;
            this.labelSaPasswordAgain.Text = "Password (again):";
            // 
            // labelSaPassword
            // 
            this.labelSaPassword.AutoSize = true;
            this.labelSaPassword.Location = new System.Drawing.Point(56, 88);
            this.labelSaPassword.Name = "labelSaPassword";
            this.labelSaPassword.Size = new System.Drawing.Size(57, 13);
            this.labelSaPassword.TabIndex = 2;
            this.labelSaPassword.Text = "Password:";
            // 
            // labelSaUsername
            // 
            this.labelSaUsername.AutoSize = true;
            this.labelSaUsername.Location = new System.Drawing.Point(56, 61);
            this.labelSaUsername.Name = "labelSaUsername";
            this.labelSaUsername.Size = new System.Drawing.Size(59, 13);
            this.labelSaUsername.TabIndex = 1;
            this.labelSaUsername.Text = "Username:";
            // 
            // labelAboutSa
            // 
            this.labelAboutSa.Location = new System.Drawing.Point(24, 8);
            this.labelAboutSa.Name = "labelAboutSa";
            this.labelAboutSa.Size = new System.Drawing.Size(496, 35);
            this.labelAboutSa.TabIndex = 0;
            this.labelAboutSa.Text = "Microsoft SQL Server 2008 has a builtin system administrator user called \'sa\'.  S" +
                "QL Server requires that you create a password for the \'sa\' account.";
            // 
            // wizardPageSelectSqlServerInstance
            // 
            this.wizardPageSelectSqlServerInstance.Controls.Add(this.labelServerSearching);
            this.wizardPageSelectSqlServerInstance.Controls.Add(this.pictureServerSearching);
            this.wizardPageSelectSqlServerInstance.Controls.Add(this.label1);
            this.wizardPageSelectSqlServerInstance.Controls.Add(this.labelFoundInstanceTroubleshooting);
            this.wizardPageSelectSqlServerInstance.Controls.Add(this.comboSqlServers);
            this.wizardPageSelectSqlServerInstance.Controls.Add(this.labelFoundInstances);
            this.wizardPageSelectSqlServerInstance.Description = "Select a running instance of Microsoft SQL Server 2008.";
            this.wizardPageSelectSqlServerInstance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageSelectSqlServerInstance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageSelectSqlServerInstance.Location = new System.Drawing.Point(0, 0);
            this.wizardPageSelectSqlServerInstance.Name = "wizardPageSelectSqlServerInstance";
            this.wizardPageSelectSqlServerInstance.Size = new System.Drawing.Size(548, 271);
            this.wizardPageSelectSqlServerInstance.TabIndex = 0;
            this.wizardPageSelectSqlServerInstance.Title = "Connect to SQL Server";
            this.wizardPageSelectSqlServerInstance.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextSelectSqlInstance);
            // 
            // labelServerSearching
            // 
            this.labelServerSearching.AutoSize = true;
            this.labelServerSearching.ForeColor = System.Drawing.Color.DimGray;
            this.labelServerSearching.Location = new System.Drawing.Point(286, 49);
            this.labelServerSearching.Name = "labelServerSearching";
            this.labelServerSearching.Size = new System.Drawing.Size(66, 13);
            this.labelServerSearching.TabIndex = 11;
            this.labelServerSearching.Text = "Searching...";
            // 
            // pictureServerSearching
            // 
            this.pictureServerSearching.Image = ((System.Drawing.Image) (resources.GetObject("pictureServerSearching.Image")));
            this.pictureServerSearching.Location = new System.Drawing.Point(268, 47);
            this.pictureServerSearching.Name = "pictureServerSearching";
            this.pictureServerSearching.Size = new System.Drawing.Size(16, 16);
            this.pictureServerSearching.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureServerSearching.TabIndex = 10;
            this.pictureServerSearching.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label1.Location = new System.Drawing.Point(24, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Note";
            // 
            // labelFoundInstanceTroubleshooting
            // 
            this.labelFoundInstanceTroubleshooting.ForeColor = System.Drawing.Color.DimGray;
            this.labelFoundInstanceTroubleshooting.Location = new System.Drawing.Point(37, 159);
            this.labelFoundInstanceTroubleshooting.Name = "labelFoundInstanceTroubleshooting";
            this.labelFoundInstanceTroubleshooting.Size = new System.Drawing.Size(448, 80);
            this.labelFoundInstanceTroubleshooting.TabIndex = 8;
            this.labelFoundInstanceTroubleshooting.Text = resources.GetString("labelFoundInstanceTroubleshooting.Text");
            // 
            // comboSqlServers
            // 
            this.comboSqlServers.Location = new System.Drawing.Point(40, 45);
            this.comboSqlServers.Name = "comboSqlServers";
            this.comboSqlServers.Size = new System.Drawing.Size(220, 21);
            this.comboSqlServers.TabIndex = 6;
            // 
            // labelFoundInstances
            // 
            this.labelFoundInstances.Location = new System.Drawing.Point(24, 8);
            this.labelFoundInstances.Name = "labelFoundInstances";
            this.labelFoundInstances.Size = new System.Drawing.Size(487, 36);
            this.labelFoundInstances.TabIndex = 1;
            this.labelFoundInstances.Text = "The following SQL Server installations were found on your network.  Please select" +
                " the one to use for your ShipWorks database.\r\n";
            // 
            // wizardPageLoginSqlServer
            // 
            this.wizardPageLoginSqlServer.Controls.Add(this.remember);
            this.wizardPageLoginSqlServer.Controls.Add(this.password);
            this.wizardPageLoginSqlServer.Controls.Add(this.username);
            this.wizardPageLoginSqlServer.Controls.Add(this.labelConnectUsing);
            this.wizardPageLoginSqlServer.Controls.Add(this.labelNeedLoginInfo);
            this.wizardPageLoginSqlServer.Controls.Add(this.labelWindowsAuthDescription);
            this.wizardPageLoginSqlServer.Controls.Add(this.labelPassword);
            this.wizardPageLoginSqlServer.Controls.Add(this.labelUsername);
            this.wizardPageLoginSqlServer.Controls.Add(this.sqlServerAuth);
            this.wizardPageLoginSqlServer.Controls.Add(this.windowsAuth);
            this.wizardPageLoginSqlServer.Description = "Enter the login information you use for SQL Server.";
            this.wizardPageLoginSqlServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageLoginSqlServer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageLoginSqlServer.Location = new System.Drawing.Point(0, 0);
            this.wizardPageLoginSqlServer.Name = "wizardPageLoginSqlServer";
            this.wizardPageLoginSqlServer.Size = new System.Drawing.Size(548, 271);
            this.wizardPageLoginSqlServer.TabIndex = 0;
            this.wizardPageLoginSqlServer.Title = "SQL Server Login";
            this.wizardPageLoginSqlServer.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextSqlLogin);
            // 
            // remember
            // 
            this.remember.Location = new System.Drawing.Point(114, 128);
            this.remember.Name = "remember";
            this.remember.Size = new System.Drawing.Size(130, 24);
            this.remember.TabIndex = 32;
            this.remember.Text = "Remember Password";
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(114, 106);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(128, 21);
            this.password.TabIndex = 31;
            this.password.UseSystemPasswordChar = true;
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(114, 80);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(128, 21);
            this.username.TabIndex = 30;
            // 
            // labelConnectUsing
            // 
            this.labelConnectUsing.Location = new System.Drawing.Point(26, 34);
            this.labelConnectUsing.Name = "labelConnectUsing";
            this.labelConnectUsing.Size = new System.Drawing.Size(100, 23);
            this.labelConnectUsing.TabIndex = 29;
            this.labelConnectUsing.Text = "Connect using:";
            // 
            // labelNeedLoginInfo
            // 
            this.labelNeedLoginInfo.Location = new System.Drawing.Point(24, 8);
            this.labelNeedLoginInfo.Name = "labelNeedLoginInfo";
            this.labelNeedLoginInfo.Size = new System.Drawing.Size(470, 22);
            this.labelNeedLoginInfo.TabIndex = 28;
            this.labelNeedLoginInfo.Text = "ShipWorks must now connect to your SQL Server instance and needs your login infor" +
                "mation.";
            // 
            // labelWindowsAuthDescription
            // 
            this.labelWindowsAuthDescription.ForeColor = System.Drawing.Color.DimGray;
            this.labelWindowsAuthDescription.Location = new System.Drawing.Point(60, 176);
            this.labelWindowsAuthDescription.Name = "labelWindowsAuthDescription";
            this.labelWindowsAuthDescription.Size = new System.Drawing.Size(401, 28);
            this.labelWindowsAuthDescription.TabIndex = 27;
            this.labelWindowsAuthDescription.Text = "You must be currently logged in as Windows user that has access to SQL Server.";
            // 
            // labelPassword
            // 
            this.labelPassword.Location = new System.Drawing.Point(56, 108);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(100, 18);
            this.labelPassword.TabIndex = 26;
            this.labelPassword.Text = "Password:";
            // 
            // labelUsername
            // 
            this.labelUsername.Location = new System.Drawing.Point(56, 82);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(100, 23);
            this.labelUsername.TabIndex = 25;
            this.labelUsername.Text = "Username:";
            // 
            // sqlServerAuth
            // 
            this.sqlServerAuth.Checked = true;
            this.sqlServerAuth.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.sqlServerAuth.Location = new System.Drawing.Point(40, 54);
            this.sqlServerAuth.Name = "sqlServerAuth";
            this.sqlServerAuth.Size = new System.Drawing.Size(236, 24);
            this.sqlServerAuth.TabIndex = 24;
            this.sqlServerAuth.TabStop = true;
            this.sqlServerAuth.Text = "SQL Server authentication";
            this.sqlServerAuth.CheckedChanged += new System.EventHandler(this.OnChangeLoginMethod);
            // 
            // windowsAuth
            // 
            this.windowsAuth.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.windowsAuth.Location = new System.Drawing.Point(42, 153);
            this.windowsAuth.Name = "windowsAuth";
            this.windowsAuth.Size = new System.Drawing.Size(228, 24);
            this.windowsAuth.TabIndex = 23;
            this.windowsAuth.Text = "Windows authentication";
            this.windowsAuth.CheckedChanged += new System.EventHandler(this.OnChangeLoginMethod);
            // 
            // wizardPageDatabaseName
            // 
            this.wizardPageDatabaseName.Controls.Add(this.infoTipDatabaseName);
            this.wizardPageDatabaseName.Controls.Add(this.databaseName);
            this.wizardPageDatabaseName.Controls.Add(this.labelDatabaseName);
            this.wizardPageDatabaseName.Controls.Add(this.labelEnterDatabaseName);
            this.wizardPageDatabaseName.Description = "Choose the name of the ShipWorks database.";
            this.wizardPageDatabaseName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageDatabaseName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageDatabaseName.Location = new System.Drawing.Point(0, 0);
            this.wizardPageDatabaseName.Name = "wizardPageDatabaseName";
            this.wizardPageDatabaseName.Size = new System.Drawing.Size(548, 271);
            this.wizardPageDatabaseName.TabIndex = 0;
            this.wizardPageDatabaseName.Title = "Database Name";
            this.wizardPageDatabaseName.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextCreateDatabase);
            this.wizardPageDatabaseName.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoCreateDatabase);
            // 
            // infoTipDatabaseName
            // 
            this.infoTipDatabaseName.Caption = "Multiple databases can be created in a single instance of Microsoft SQL Server 20" +
                "05.  Each database must have a unique name that is used to connect to it.";
            this.infoTipDatabaseName.Location = new System.Drawing.Point(262, 34);
            this.infoTipDatabaseName.Name = "infoTipDatabaseName";
            this.infoTipDatabaseName.Size = new System.Drawing.Size(12, 12);
            this.infoTipDatabaseName.TabIndex = 9;
            this.infoTipDatabaseName.Title = "Database Name";
            // 
            // databaseName
            // 
            this.databaseName.Location = new System.Drawing.Point(124, 30);
            this.databaseName.Name = "databaseName";
            this.databaseName.Size = new System.Drawing.Size(132, 21);
            this.databaseName.TabIndex = 7;
            this.databaseName.Text = "ShipWorks";
            // 
            // labelDatabaseName
            // 
            this.labelDatabaseName.Location = new System.Drawing.Point(36, 32);
            this.labelDatabaseName.Name = "labelDatabaseName";
            this.labelDatabaseName.Size = new System.Drawing.Size(100, 23);
            this.labelDatabaseName.TabIndex = 6;
            this.labelDatabaseName.Text = "Database name:";
            // 
            // labelEnterDatabaseName
            // 
            this.labelEnterDatabaseName.Location = new System.Drawing.Point(24, 8);
            this.labelEnterDatabaseName.Name = "labelEnterDatabaseName";
            this.labelEnterDatabaseName.Size = new System.Drawing.Size(348, 18);
            this.labelEnterDatabaseName.TabIndex = 5;
            this.labelEnterDatabaseName.Text = "Enter the name of the ShipWorks database to be created:";
            // 
            // wizardPageChooseDatabase
            // 
            this.wizardPageChooseDatabase.Controls.Add(this.databaseNames);
            this.wizardPageChooseDatabase.Controls.Add(this.labelFoundDatabases);
            this.wizardPageChooseDatabase.Description = "Select the name of your ShipWorks database.";
            this.wizardPageChooseDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageChooseDatabase.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageChooseDatabase.Location = new System.Drawing.Point(0, 0);
            this.wizardPageChooseDatabase.Name = "wizardPageChooseDatabase";
            this.wizardPageChooseDatabase.Size = new System.Drawing.Size(548, 271);
            this.wizardPageChooseDatabase.TabIndex = 0;
            this.wizardPageChooseDatabase.Title = "Choose Database";
            this.wizardPageChooseDatabase.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextChooseDatabase);
            this.wizardPageChooseDatabase.PageShown += new System.EventHandler<ShipWorks.UI.Wizard.WizardPageShownEventArgs>(this.OnShownChooseDatabase);
            // 
            // databaseNames
            // 
            this.databaseNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.databaseNames.Location = new System.Drawing.Point(41, 37);
            this.databaseNames.MaxDropDownItems = 16;
            this.databaseNames.Name = "databaseNames";
            this.databaseNames.Size = new System.Drawing.Size(224, 21);
            this.databaseNames.TabIndex = 4;
            // 
            // labelFoundDatabases
            // 
            this.labelFoundDatabases.Location = new System.Drawing.Point(24, 8);
            this.labelFoundDatabases.Name = "labelFoundDatabases";
            this.labelFoundDatabases.Size = new System.Drawing.Size(488, 16);
            this.labelFoundDatabases.TabIndex = 3;
            this.labelFoundDatabases.Text = "The following databases were found in SQL Server.  Please select your ShipWorks d" +
                "atabase.";
            // 
            // wizardPageDownloadSqlServer
            // 
            this.wizardPageDownloadSqlServer.Controls.Add(this.bytesSqlServer);
            this.wizardPageDownloadSqlServer.Controls.Add(this.downloadSqlServer);
            this.wizardPageDownloadSqlServer.Controls.Add(this.progressSqlServer);
            this.wizardPageDownloadSqlServer.Controls.Add(this.labelDownloadSqlServer);
            this.wizardPageDownloadSqlServer.Description = "ShipWorks needs to download Microsoft SQL Server 2008.";
            this.wizardPageDownloadSqlServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageDownloadSqlServer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageDownloadSqlServer.Location = new System.Drawing.Point(0, 0);
            this.wizardPageDownloadSqlServer.Name = "wizardPageDownloadSqlServer";
            this.wizardPageDownloadSqlServer.Size = new System.Drawing.Size(548, 271);
            this.wizardPageDownloadSqlServer.TabIndex = 0;
            this.wizardPageDownloadSqlServer.Title = "Install Microsoft SQL Server 2008";
            this.wizardPageDownloadSqlServer.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoDownloadSqlServer);
            this.wizardPageDownloadSqlServer.PageShown += new System.EventHandler<ShipWorks.UI.Wizard.WizardPageShownEventArgs>(this.OnShownDownloadSqlServer);
            this.wizardPageDownloadSqlServer.Cancelling += new System.ComponentModel.CancelEventHandler(this.OnCancelSqlServerDownload);
            // 
            // bytesSqlServer
            // 
            this.bytesSqlServer.Location = new System.Drawing.Point(375, 71);
            this.bytesSqlServer.Name = "bytesSqlServer";
            this.bytesSqlServer.Size = new System.Drawing.Size(145, 23);
            this.bytesSqlServer.TabIndex = 11;
            this.bytesSqlServer.Text = "(0 of 8052 KB)";
            this.bytesSqlServer.Visible = false;
            // 
            // downloadSqlServer
            // 
            this.downloadSqlServer.Location = new System.Drawing.Point(47, 67);
            this.downloadSqlServer.Name = "downloadSqlServer";
            this.downloadSqlServer.Size = new System.Drawing.Size(75, 23);
            this.downloadSqlServer.TabIndex = 10;
            this.downloadSqlServer.Text = "Download";
            this.downloadSqlServer.Click += new System.EventHandler(this.OnDownloadSqlServer);
            // 
            // progressSqlServer
            // 
            this.progressSqlServer.Location = new System.Drawing.Point(127, 67);
            this.progressSqlServer.Name = "progressSqlServer";
            this.progressSqlServer.Size = new System.Drawing.Size(246, 23);
            this.progressSqlServer.TabIndex = 9;
            // 
            // labelDownloadSqlServer
            // 
            this.labelDownloadSqlServer.Location = new System.Drawing.Point(24, 8);
            this.labelDownloadSqlServer.Name = "labelDownloadSqlServer";
            this.labelDownloadSqlServer.Size = new System.Drawing.Size(508, 46);
            this.labelDownloadSqlServer.TabIndex = 8;
            this.labelDownloadSqlServer.Text = "ShipWorks needs to install Microsoft SQL Server 2008, but first it must be downlo" +
                "aded. \r\n\r\nClick Download to download Microsoft SQL Server 2008.";
            // 
            // wizardPageInstallSqlServer
            // 
            this.wizardPageInstallSqlServer.Controls.Add(this.labelInstallSqlServer);
            this.wizardPageInstallSqlServer.Controls.Add(this.labelInstallingSqlServer);
            this.wizardPageInstallSqlServer.Description = "ShipWorks is ready to install Microsoft SQL Server 2008.";
            this.wizardPageInstallSqlServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageInstallSqlServer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageInstallSqlServer.Location = new System.Drawing.Point(0, 0);
            this.wizardPageInstallSqlServer.Name = "wizardPageInstallSqlServer";
            this.wizardPageInstallSqlServer.NextRequiresElevation = true;
            this.wizardPageInstallSqlServer.Size = new System.Drawing.Size(548, 271);
            this.wizardPageInstallSqlServer.TabIndex = 0;
            this.wizardPageInstallSqlServer.Title = "Install Microsoft SQL Server 2008";
            this.wizardPageInstallSqlServer.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextInstallSqlServer);
            this.wizardPageInstallSqlServer.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoInstallSqlServer);
            this.wizardPageInstallSqlServer.Cancelling += new System.ComponentModel.CancelEventHandler(this.OnCancellInstallSqlServer);
            // 
            // labelInstallSqlServer
            // 
            this.labelInstallSqlServer.Controls.Add(this.label21);
            this.labelInstallSqlServer.Controls.Add(this.labelNote);
            this.labelInstallSqlServer.Controls.Add(this.labelWarning);
            this.labelInstallSqlServer.Location = new System.Drawing.Point(16, -2);
            this.labelInstallSqlServer.Name = "labelInstallSqlServer";
            this.labelInstallSqlServer.Size = new System.Drawing.Size(478, 82);
            this.labelInstallSqlServer.TabIndex = 155;
            // 
            // label21
            // 
            this.label21.Location = new System.Drawing.Point(8, 12);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(470, 30);
            this.label21.TabIndex = 2;
            this.label21.Text = "ShipWorks will now install Microsoft SQL Server 2008. Click Next to continue with" +
                " the installation.";
            // 
            // labelNote
            // 
            this.labelNote.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelNote.Location = new System.Drawing.Point(9, 40);
            this.labelNote.Name = "labelNote";
            this.labelNote.Size = new System.Drawing.Size(38, 22);
            this.labelNote.TabIndex = 152;
            this.labelNote.Text = "Note:";
            // 
            // labelWarning
            // 
            this.labelWarning.Location = new System.Drawing.Point(47, 40);
            this.labelWarning.Name = "labelWarning";
            this.labelWarning.Size = new System.Drawing.Size(342, 18);
            this.labelWarning.TabIndex = 153;
            this.labelWarning.Text = "Please close all other running applications before proceeding.";
            // 
            // labelInstallingSqlServer
            // 
            this.labelInstallingSqlServer.Location = new System.Drawing.Point(24, 8);
            this.labelInstallingSqlServer.Name = "labelInstallingSqlServer";
            this.labelInstallingSqlServer.Size = new System.Drawing.Size(491, 78);
            this.labelInstallingSqlServer.TabIndex = 156;
            this.labelInstallingSqlServer.Text = "ShipWorks is now installing Microsoft SQL Server 2008.  This may take a few minut" +
                "es.";
            // 
            // wizardPageComplete
            // 
            this.wizardPageComplete.Controls.Add(this.labelSetupComplete);
            this.wizardPageComplete.Controls.Add(this.iconSetupComplete);
            this.wizardPageComplete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageComplete.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageComplete.Location = new System.Drawing.Point(0, 0);
            this.wizardPageComplete.Name = "wizardPageComplete";
            this.wizardPageComplete.Size = new System.Drawing.Size(548, 271);
            this.wizardPageComplete.TabIndex = 0;
            this.wizardPageComplete.Title = "Database Setup Complete";
            this.wizardPageComplete.PageShown += new System.EventHandler<ShipWorks.UI.Wizard.WizardPageShownEventArgs>(this.OnShownComplete);
            // 
            // labelSetupComplete
            // 
            this.labelSetupComplete.AutoSize = true;
            this.labelSetupComplete.Location = new System.Drawing.Point(44, 11);
            this.labelSetupComplete.Name = "labelSetupComplete";
            this.labelSetupComplete.Size = new System.Drawing.Size(163, 13);
            this.labelSetupComplete.TabIndex = 1;
            this.labelSetupComplete.Text = "The database setup is complete.";
            // 
            // iconSetupComplete
            // 
            this.iconSetupComplete.Image = global::ShipWorks.Properties.Resources.check16;
            this.iconSetupComplete.Location = new System.Drawing.Point(24, 8);
            this.iconSetupComplete.Name = "iconSetupComplete";
            this.iconSetupComplete.Size = new System.Drawing.Size(16, 16);
            this.iconSetupComplete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.iconSetupComplete.TabIndex = 0;
            this.iconSetupComplete.TabStop = false;
            // 
            // wizardPageShipWorksAdmin
            // 
            this.wizardPageShipWorksAdmin.Controls.Add(this.helpAutomaticLogon);
            this.wizardPageShipWorksAdmin.Controls.Add(this.helpUserEmail);
            this.wizardPageShipWorksAdmin.Controls.Add(this.pictureBox6);
            this.wizardPageShipWorksAdmin.Controls.Add(this.label11);
            this.wizardPageShipWorksAdmin.Controls.Add(this.swAutomaticLogon);
            this.wizardPageShipWorksAdmin.Controls.Add(this.swEmail);
            this.wizardPageShipWorksAdmin.Controls.Add(this.label7);
            this.wizardPageShipWorksAdmin.Controls.Add(this.swPasswordAgain);
            this.wizardPageShipWorksAdmin.Controls.Add(this.label6);
            this.wizardPageShipWorksAdmin.Controls.Add(this.swPassword);
            this.wizardPageShipWorksAdmin.Controls.Add(this.label8);
            this.wizardPageShipWorksAdmin.Controls.Add(this.swUsername);
            this.wizardPageShipWorksAdmin.Controls.Add(this.label9);
            this.wizardPageShipWorksAdmin.Controls.Add(this.label10);
            this.wizardPageShipWorksAdmin.Description = "Create an administrator account to log on to ShipWorks.";
            this.wizardPageShipWorksAdmin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageShipWorksAdmin.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageShipWorksAdmin.Location = new System.Drawing.Point(0, 0);
            this.wizardPageShipWorksAdmin.Name = "wizardPageShipWorksAdmin";
            this.wizardPageShipWorksAdmin.Size = new System.Drawing.Size(548, 271);
            this.wizardPageShipWorksAdmin.TabIndex = 0;
            this.wizardPageShipWorksAdmin.Title = "Create a ShipWorks User Account";
            this.wizardPageShipWorksAdmin.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextShipWorksAdmin);
            this.wizardPageShipWorksAdmin.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoShipWorksAdmin);
            // 
            // helpAutomaticLogon
            // 
            this.helpAutomaticLogon.Caption = "To stop automatically logging on, Log Off using the main application menu.";
            this.helpAutomaticLogon.Location = new System.Drawing.Point(406, 190);
            this.helpAutomaticLogon.Name = "helpAutomaticLogon";
            this.helpAutomaticLogon.Size = new System.Drawing.Size(12, 12);
            this.helpAutomaticLogon.TabIndex = 185;
            this.helpAutomaticLogon.Title = "Automatic Log On";
            // 
            // helpUserEmail
            // 
            this.helpUserEmail.Caption = "Your email address will be used to send you a new password if its forgotten.";
            this.helpUserEmail.Location = new System.Drawing.Point(428, 111);
            this.helpUserEmail.Name = "helpUserEmail";
            this.helpUserEmail.Size = new System.Drawing.Size(12, 12);
            this.helpUserEmail.TabIndex = 184;
            this.helpUserEmail.Title = "Email Address";
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = global::ShipWorks.Properties.Resources.dude33;
            this.pictureBox6.Location = new System.Drawing.Point(23, 8);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(48, 48);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox6.TabIndex = 183;
            this.pictureBox6.TabStop = false;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(78, 29);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(446, 31);
            this.label11.TabIndex = 182;
            this.label11.Text = "The user account you create here is a ShipWorks administrator.  A ShipWorks admin" +
                "istrator can do anything in ShipWorks.";
            // 
            // swAutomaticLogon
            // 
            this.swAutomaticLogon.AutoSize = true;
            this.swAutomaticLogon.Location = new System.Drawing.Point(179, 188);
            this.swAutomaticLogon.Name = "swAutomaticLogon";
            this.swAutomaticLogon.Size = new System.Drawing.Size(228, 17);
            this.swAutomaticLogon.TabIndex = 180;
            this.swAutomaticLogon.Text = "Log me on automatically on this computer.";
            this.swAutomaticLogon.UseVisualStyleBackColor = true;
            // 
            // swEmail
            // 
            this.swEmail.Location = new System.Drawing.Point(179, 107);
            this.fieldLengthProvider.SetMaxLengthSource(this.swEmail, ShipWorks.Data.Utility.EntityFieldLengthSource.UserEmail);
            this.swEmail.Name = "swEmail";
            this.swEmail.Size = new System.Drawing.Size(243, 21);
            this.swEmail.TabIndex = 171;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(97, 110);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 178;
            this.label7.Text = "Email address:";
            // 
            // swPasswordAgain
            // 
            this.swPasswordAgain.Location = new System.Drawing.Point(179, 161);
            this.fieldLengthProvider.SetMaxLengthSource(this.swPasswordAgain, ShipWorks.Data.Utility.EntityFieldLengthSource.UserPassword);
            this.swPasswordAgain.Name = "swPasswordAgain";
            this.swPasswordAgain.Size = new System.Drawing.Size(243, 21);
            this.swPasswordAgain.TabIndex = 173;
            this.swPasswordAgain.UseSystemPasswordChar = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(78, 164);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 177;
            this.label6.Text = "Retype password:";
            // 
            // swPassword
            // 
            this.swPassword.Location = new System.Drawing.Point(179, 134);
            this.fieldLengthProvider.SetMaxLengthSource(this.swPassword, ShipWorks.Data.Utility.EntityFieldLengthSource.UserPassword);
            this.swPassword.Name = "swPassword";
            this.swPassword.Size = new System.Drawing.Size(243, 21);
            this.swPassword.TabIndex = 172;
            this.swPassword.UseSystemPasswordChar = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(116, 137);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 176;
            this.label8.Text = "Password:";
            // 
            // swUsername
            // 
            this.swUsername.Location = new System.Drawing.Point(179, 80);
            this.fieldLengthProvider.SetMaxLengthSource(this.swUsername, ShipWorks.Data.Utility.EntityFieldLengthSource.UserName);
            this.swUsername.Name = "swUsername";
            this.swUsername.Size = new System.Drawing.Size(243, 21);
            this.swUsername.TabIndex = 170;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(114, 83);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 13);
            this.label9.TabIndex = 175;
            this.label9.Text = "Username:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(78, 8);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(418, 13);
            this.label10.TabIndex = 174;
            this.label10.Text = "ShipWorks allows you to control who has access to ShipWorks through user accounts" +
                ".";
            // 
            // wizardPageRestoreDatabase
            // 
            this.wizardPageRestoreDatabase.Controls.Add(this.labelCantRestore);
            this.wizardPageRestoreDatabase.Controls.Add(this.groupInfo);
            this.wizardPageRestoreDatabase.Controls.Add(this.backupFile);
            this.wizardPageRestoreDatabase.Controls.Add(this.browseForBackupFile);
            this.wizardPageRestoreDatabase.Controls.Add(this.labelBackupFile);
            this.wizardPageRestoreDatabase.Description = "Select the ShipWorks backup file to restore from.";
            this.wizardPageRestoreDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageRestoreDatabase.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageRestoreDatabase.Location = new System.Drawing.Point(0, 0);
            this.wizardPageRestoreDatabase.Name = "wizardPageRestoreDatabase";
            this.wizardPageRestoreDatabase.Size = new System.Drawing.Size(548, 271);
            this.wizardPageRestoreDatabase.TabIndex = 0;
            this.wizardPageRestoreDatabase.Title = "Restore Backup";
            this.wizardPageRestoreDatabase.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextRestoreBackup);
            this.wizardPageRestoreDatabase.PageShown += new System.EventHandler<ShipWorks.UI.Wizard.WizardPageShownEventArgs>(this.OnPageShownRestoreBackup);
            // 
            // labelCantRestore
            // 
            this.labelCantRestore.AutoSize = true;
            this.labelCantRestore.ForeColor = System.Drawing.Color.Red;
            this.labelCantRestore.Location = new System.Drawing.Point(20, 86);
            this.labelCantRestore.Name = "labelCantRestore";
            this.labelCantRestore.Size = new System.Drawing.Size(382, 13);
            this.labelCantRestore.TabIndex = 174;
            this.labelCantRestore.Text = "A ShipWorks restore can only be done from the computer running SQL Server.";
            this.labelCantRestore.Visible = false;
            // 
            // groupInfo
            // 
            this.groupInfo.Controls.Add(this.label13);
            this.groupInfo.Controls.Add(this.labelNote1);
            this.groupInfo.Controls.Add(this.warningIcon);
            this.groupInfo.Controls.Add(this.labelNote2);
            this.groupInfo.Location = new System.Drawing.Point(21, 102);
            this.groupInfo.Name = "groupInfo";
            this.groupInfo.Size = new System.Drawing.Size(501, 70);
            this.groupInfo.TabIndex = 173;
            this.groupInfo.TabStop = false;
            this.groupInfo.Text = "Important Information";
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label13.Location = new System.Drawing.Point(34, 26);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(72, 22);
            this.label13.TabIndex = 159;
            this.label13.Text = "Important:";
            // 
            // labelNote1
            // 
            this.labelNote1.Location = new System.Drawing.Point(108, 26);
            this.labelNote1.Name = "labelNote1";
            this.labelNote1.Size = new System.Drawing.Size(352, 18);
            this.labelNote1.TabIndex = 161;
            this.labelNote1.Text = "- The data being restored will overwrite any existing data.";
            // 
            // warningIcon
            // 
            this.warningIcon.Image = ((System.Drawing.Image) (resources.GetObject("warningIcon.Image")));
            this.warningIcon.Location = new System.Drawing.Point(16, 24);
            this.warningIcon.Name = "warningIcon";
            this.warningIcon.Size = new System.Drawing.Size(22, 16);
            this.warningIcon.TabIndex = 158;
            this.warningIcon.TabStop = false;
            // 
            // labelNote2
            // 
            this.labelNote2.Location = new System.Drawing.Point(108, 44);
            this.labelNote2.Name = "labelNote2";
            this.labelNote2.Size = new System.Drawing.Size(352, 18);
            this.labelNote2.TabIndex = 160;
            this.labelNote2.Text = "- Any other users currently using ShipWorks will be disconnected.";
            // 
            // backupFile
            // 
            this.backupFile.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.backupFile.Location = new System.Drawing.Point(21, 26);
            this.backupFile.Name = "backupFile";
            this.backupFile.ReadOnly = true;
            this.backupFile.Size = new System.Drawing.Size(501, 21);
            this.backupFile.TabIndex = 167;
            // 
            // browseForBackupFile
            // 
            this.browseForBackupFile.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseForBackupFile.Location = new System.Drawing.Point(447, 53);
            this.browseForBackupFile.Name = "browseForBackupFile";
            this.browseForBackupFile.Size = new System.Drawing.Size(75, 23);
            this.browseForBackupFile.TabIndex = 166;
            this.browseForBackupFile.Text = "Browse...";
            this.browseForBackupFile.Click += new System.EventHandler(this.OnBrowseBackupFile);
            // 
            // labelBackupFile
            // 
            this.labelBackupFile.AutoSize = true;
            this.labelBackupFile.Location = new System.Drawing.Point(20, 10);
            this.labelBackupFile.Name = "labelBackupFile";
            this.labelBackupFile.Size = new System.Drawing.Size(242, 13);
            this.labelBackupFile.TabIndex = 165;
            this.labelBackupFile.Text = "Select the ShipWorks backup file to restore from:";
            // 
            // wizardPageRestoreOption
            // 
            this.wizardPageRestoreOption.Controls.Add(this.label12);
            this.wizardPageRestoreOption.Controls.Add(this.label15);
            this.wizardPageRestoreOption.Controls.Add(this.radioRestoreIntoNewDatabase);
            this.wizardPageRestoreOption.Controls.Add(this.radioRestoreIntoCurrent);
            this.wizardPageRestoreOption.Description = "Choose where the database will be restored to.";
            this.wizardPageRestoreOption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageRestoreOption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageRestoreOption.Location = new System.Drawing.Point(0, 0);
            this.wizardPageRestoreOption.Name = "wizardPageRestoreOption";
            this.wizardPageRestoreOption.Size = new System.Drawing.Size(548, 271);
            this.wizardPageRestoreOption.TabIndex = 0;
            this.wizardPageRestoreOption.Title = "Restore Location";
            this.wizardPageRestoreOption.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextRestoreOption);
            this.wizardPageRestoreOption.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoRestoreOption);
            // 
            // label12
            // 
            this.label12.ForeColor = System.Drawing.Color.DimGray;
            this.label12.Location = new System.Drawing.Point(46, 28);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(445, 15);
            this.label12.TabIndex = 11;
            this.label12.Text = "All of the data in the current database will be overwritten.";
            // 
            // label15
            // 
            this.label15.ForeColor = System.Drawing.Color.DimGray;
            this.label15.Location = new System.Drawing.Point(43, 77);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(406, 15);
            this.label15.TabIndex = 18;
            this.label15.Text = "Select this option to create a new ShipWorks database to load the backup into.";
            // 
            // radioRestoreIntoNewDatabase
            // 
            this.radioRestoreIntoNewDatabase.AutoSize = true;
            this.radioRestoreIntoNewDatabase.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.radioRestoreIntoNewDatabase.Location = new System.Drawing.Point(23, 57);
            this.radioRestoreIntoNewDatabase.Name = "radioRestoreIntoNewDatabase";
            this.radioRestoreIntoNewDatabase.Size = new System.Drawing.Size(188, 17);
            this.radioRestoreIntoNewDatabase.TabIndex = 13;
            this.radioRestoreIntoNewDatabase.TabStop = true;
            this.radioRestoreIntoNewDatabase.Text = "Restore into a New Database";
            this.radioRestoreIntoNewDatabase.UseVisualStyleBackColor = true;
            // 
            // radioRestoreIntoCurrent
            // 
            this.radioRestoreIntoCurrent.AutoSize = true;
            this.radioRestoreIntoCurrent.Checked = true;
            this.radioRestoreIntoCurrent.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.radioRestoreIntoCurrent.Location = new System.Drawing.Point(23, 8);
            this.radioRestoreIntoCurrent.Name = "radioRestoreIntoCurrent";
            this.radioRestoreIntoCurrent.Size = new System.Drawing.Size(224, 17);
            this.radioRestoreIntoCurrent.TabIndex = 0;
            this.radioRestoreIntoCurrent.TabStop = true;
            this.radioRestoreIntoCurrent.Text = "Restore over the Current Database";
            this.radioRestoreIntoCurrent.UseVisualStyleBackColor = true;
            // 
            // openBackupFileDialog
            // 
            this.openBackupFileDialog.DefaultExt = "swb";
            this.openBackupFileDialog.Filter = "ShipWorks Backup Files (*.swb)|*.swb";
            // 
            // wizardPageRestoreLogin
            // 
            this.wizardPageRestoreLogin.Controls.Add(this.headerImage);
            this.wizardPageRestoreLogin.Controls.Add(this.restorePassword);
            this.wizardPageRestoreLogin.Controls.Add(this.label16);
            this.wizardPageRestoreLogin.Controls.Add(this.restoreUsername);
            this.wizardPageRestoreLogin.Controls.Add(this.label17);
            this.wizardPageRestoreLogin.Controls.Add(this.label18);
            this.wizardPageRestoreLogin.Description = "Log on as a user with permission to restore a backup.";
            this.wizardPageRestoreLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageRestoreLogin.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageRestoreLogin.Location = new System.Drawing.Point(0, 0);
            this.wizardPageRestoreLogin.Name = "wizardPageRestoreLogin";
            this.wizardPageRestoreLogin.Size = new System.Drawing.Size(548, 271);
            this.wizardPageRestoreLogin.TabIndex = 0;
            this.wizardPageRestoreLogin.Title = "Log On Required";
            this.wizardPageRestoreLogin.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextRestoreLogin);
            this.wizardPageRestoreLogin.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoRestoreLogin);
            // 
            // headerImage
            // 
            this.headerImage.Cursor = System.Windows.Forms.Cursors.Default;
            this.headerImage.Image = global::ShipWorks.Properties.Resources.user_lock_48;
            this.headerImage.Location = new System.Drawing.Point(28, 34);
            this.headerImage.Name = "headerImage";
            this.headerImage.Size = new System.Drawing.Size(48, 48);
            this.headerImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.headerImage.TabIndex = 174;
            this.headerImage.TabStop = false;
            // 
            // restorePassword
            // 
            this.restorePassword.Location = new System.Drawing.Point(148, 61);
            this.fieldLengthProvider.SetMaxLengthSource(this.restorePassword, ShipWorks.Data.Utility.EntityFieldLengthSource.UserPassword);
            this.restorePassword.Name = "restorePassword";
            this.restorePassword.Size = new System.Drawing.Size(245, 21);
            this.restorePassword.TabIndex = 173;
            this.restorePassword.UseSystemPasswordChar = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(85, 64);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(57, 13);
            this.label16.TabIndex = 172;
            this.label16.Text = "Password:";
            // 
            // restoreUsername
            // 
            this.restoreUsername.Location = new System.Drawing.Point(148, 34);
            this.fieldLengthProvider.SetMaxLengthSource(this.restoreUsername, ShipWorks.Data.Utility.EntityFieldLengthSource.UserName);
            this.restoreUsername.Name = "restoreUsername";
            this.restoreUsername.Size = new System.Drawing.Size(245, 21);
            this.restoreUsername.TabIndex = 171;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(83, 37);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(59, 13);
            this.label17.TabIndex = 170;
            this.label17.Text = "Username:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(25, 9);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(448, 13);
            this.label18.TabIndex = 169;
            this.label18.Text = "Enter the username and password of a ShipWorks user with permission to restore a " +
                "backup:";
            // 
            // wizardPageWindowsFirewall
            // 
            this.wizardPageWindowsFirewall.Controls.Add(this.firewallUpdatedLabel);
            this.wizardPageWindowsFirewall.Controls.Add(this.firewallUpdatedPicture);
            this.wizardPageWindowsFirewall.Controls.Add(this.label19);
            this.wizardPageWindowsFirewall.Controls.Add(this.updateWindowsFirewall);
            this.wizardPageWindowsFirewall.Controls.Add(this.label14);
            this.wizardPageWindowsFirewall.Controls.Add(this.pictureBox5);
            this.wizardPageWindowsFirewall.Description = "Enable ShipWorks to work through Windows Firewall.";
            this.wizardPageWindowsFirewall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageWindowsFirewall.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageWindowsFirewall.Location = new System.Drawing.Point(0, 0);
            this.wizardPageWindowsFirewall.Name = "wizardPageWindowsFirewall";
            this.wizardPageWindowsFirewall.Size = new System.Drawing.Size(548, 271);
            this.wizardPageWindowsFirewall.TabIndex = 0;
            this.wizardPageWindowsFirewall.Title = "Windows Firewall";
            this.wizardPageWindowsFirewall.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoWindowsFirewall);
            // 
            // firewallUpdatedLabel
            // 
            this.firewallUpdatedLabel.AutoSize = true;
            this.firewallUpdatedLabel.ForeColor = System.Drawing.Color.Green;
            this.firewallUpdatedLabel.Location = new System.Drawing.Point(277, 108);
            this.firewallUpdatedLabel.Name = "firewallUpdatedLabel";
            this.firewallUpdatedLabel.Size = new System.Drawing.Size(183, 13);
            this.firewallUpdatedLabel.TabIndex = 5;
            this.firewallUpdatedLabel.Text = "Windows Firewall has been updated.";
            this.firewallUpdatedLabel.Visible = false;
            // 
            // firewallUpdatedPicture
            // 
            this.firewallUpdatedPicture.Image = global::ShipWorks.Properties.Resources.check16;
            this.firewallUpdatedPicture.Location = new System.Drawing.Point(259, 106);
            this.firewallUpdatedPicture.Name = "firewallUpdatedPicture";
            this.firewallUpdatedPicture.Size = new System.Drawing.Size(16, 16);
            this.firewallUpdatedPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.firewallUpdatedPicture.TabIndex = 4;
            this.firewallUpdatedPicture.TabStop = false;
            this.firewallUpdatedPicture.Visible = false;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(81, 75);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(420, 13);
            this.label19.TabIndex = 3;
            this.label19.Text = "Only the minimum changes required for running ShipWorks will be made to the firew" +
                "all.";
            // 
            // updateWindowsFirewall
            // 
            this.updateWindowsFirewall.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.updateWindowsFirewall.Location = new System.Drawing.Point(84, 103);
            this.updateWindowsFirewall.Name = "updateWindowsFirewall";
            this.updateWindowsFirewall.Size = new System.Drawing.Size(168, 23);
            this.updateWindowsFirewall.TabIndex = 2;
            this.updateWindowsFirewall.Text = "Update Windows Firewall";
            this.updateWindowsFirewall.UseVisualStyleBackColor = true;
            this.updateWindowsFirewall.Click += new System.EventHandler(this.OnUpdateWindowsFirewall);
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(81, 8);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(455, 56);
            this.label14.TabIndex = 1;
            this.label14.Text = resources.GetString("label14.Text");
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = global::ShipWorks.Properties.Resources.firewall;
            this.pictureBox5.Location = new System.Drawing.Point(23, 8);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(48, 48);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox5.TabIndex = 0;
            this.pictureBox5.TabStop = false;
            // 
            // wizardPagePrerequisitePlaceholder
            // 
            this.wizardPagePrerequisitePlaceholder.Controls.Add(this.label24);
            this.wizardPagePrerequisitePlaceholder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPagePrerequisitePlaceholder.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPagePrerequisitePlaceholder.Location = new System.Drawing.Point(0, 0);
            this.wizardPagePrerequisitePlaceholder.Name = "wizardPagePrerequisitePlaceholder";
            this.wizardPagePrerequisitePlaceholder.Size = new System.Drawing.Size(548, 271);
            this.wizardPagePrerequisitePlaceholder.TabIndex = 0;
            this.wizardPagePrerequisitePlaceholder.Title = "Prerequisite Placeholder";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(22, 8);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(338, 13);
            this.label24.TabIndex = 0;
            this.label24.Text = "Placeholder page for dynamically created and inserted prereq pages.";
            // 
            // DatabaseSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(548, 378);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DatabaseSetupWizard";
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageSetupOrConnect,
            this.wizardPageRestoreOption,
            this.wizardPageChooseSqlServer,
            this.wizardPagePrerequisitePlaceholder,
            this.wizardPageNewCredentials,
            this.wizardPageSelectSqlServerInstance,
            this.wizardPageLoginSqlServer,
            this.wizardPageChooseDatabase,
            this.wizardPageDownloadSqlServer,
            this.wizardPageInstallSqlServer,
            this.wizardPageWindowsFirewall,
            this.wizardPageDatabaseName,
            this.wizardPageRestoreLogin,
            this.wizardPageRestoreDatabase,
            this.wizardPageShipWorksAdmin,
            this.wizardPageComplete});
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ShipWorks Database Setup";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnClosed);
            this.Load += new System.EventHandler(this.OnLoad);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageSetupOrConnect.ResumeLayout(false);
            this.wizardPageSetupOrConnect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxRestoreDatabase)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxConnectRunningDatabase)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxSetupNewDatabase)).EndInit();
            this.wizardPageChooseSqlServer.ResumeLayout(false);
            this.wizardPageChooseSqlServer.PerformLayout();
            this.wizardPageNewCredentials.ResumeLayout(false);
            this.wizardPageNewCredentials.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.iconWarningSa)).EndInit();
            this.wizardPageSelectSqlServerInstance.ResumeLayout(false);
            this.wizardPageSelectSqlServerInstance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureServerSearching)).EndInit();
            this.wizardPageLoginSqlServer.ResumeLayout(false);
            this.wizardPageLoginSqlServer.PerformLayout();
            this.wizardPageDatabaseName.ResumeLayout(false);
            this.wizardPageDatabaseName.PerformLayout();
            this.wizardPageChooseDatabase.ResumeLayout(false);
            this.wizardPageDownloadSqlServer.ResumeLayout(false);
            this.wizardPageInstallSqlServer.ResumeLayout(false);
            this.labelInstallSqlServer.ResumeLayout(false);
            this.wizardPageComplete.ResumeLayout(false);
            this.wizardPageComplete.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.iconSetupComplete)).EndInit();
            this.wizardPageShipWorksAdmin.ResumeLayout(false);
            this.wizardPageShipWorksAdmin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox6)).EndInit();
            this.wizardPageRestoreDatabase.ResumeLayout(false);
            this.wizardPageRestoreDatabase.PerformLayout();
            this.groupInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.warningIcon)).EndInit();
            this.wizardPageRestoreOption.ResumeLayout(false);
            this.wizardPageRestoreOption.PerformLayout();
            this.wizardPageRestoreLogin.ResumeLayout(false);
            this.wizardPageRestoreLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.headerImage)).EndInit();
            this.wizardPageWindowsFirewall.ResumeLayout(false);
            this.wizardPageWindowsFirewall.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.firewallUpdatedPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox5)).EndInit();
            this.wizardPagePrerequisitePlaceholder.ResumeLayout(false);
            this.wizardPagePrerequisitePlaceholder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Wizard.WizardPage wizardPageSetupOrConnect;
        private ShipWorks.UI.Wizard.WizardPage wizardPageChooseSqlServer;
        private System.Windows.Forms.Label labelShipWorksUsesSqlServer;
        private System.Windows.Forms.RadioButton radioSetupNewDatabase;
        private System.Windows.Forms.RadioButton radioRestoreDatabase;
        private System.Windows.Forms.RadioButton radioConnectRunningDatabase;
        private System.Windows.Forms.Label labelChooseNewOrExistingSqlServer;
        private System.Windows.Forms.RadioButton radioInstallSqlServer;
        private System.Windows.Forms.TextBox instanceName;
        private System.Windows.Forms.Label labelInstanceName;
        private System.Windows.Forms.RadioButton radioSqlServerAlreadyInstalled;
        private ShipWorks.UI.Wizard.WizardPage wizardPageNewCredentials;
        private System.Windows.Forms.Label labelAboutSa;
        private System.Windows.Forms.Label labelSaPasswordAgain;
        private System.Windows.Forms.Label labelSaPassword;
        private System.Windows.Forms.Label labelSaUsername;
        private System.Windows.Forms.TextBox saPassword;
        private System.Windows.Forms.TextBox textBoxSa;
        private System.Windows.Forms.TextBox saPasswordAgain;
        private System.Windows.Forms.PictureBox iconWarningSa;
        private System.Windows.Forms.Label labelSaWarning;
        private System.Windows.Forms.Label labelImportantSa;
        private ShipWorks.UI.Wizard.WizardPage wizardPageSelectSqlServerInstance;
        private System.Windows.Forms.Label labelFoundInstances;
        private System.Windows.Forms.ComboBox comboSqlServers;
        private System.Windows.Forms.Label labelFoundInstanceTroubleshooting;
        private ShipWorks.UI.Wizard.WizardPage wizardPageLoginSqlServer;
        private System.Windows.Forms.CheckBox remember;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Label labelConnectUsing;
        private System.Windows.Forms.Label labelNeedLoginInfo;
        private System.Windows.Forms.Label labelWindowsAuthDescription;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.RadioButton sqlServerAuth;
        private System.Windows.Forms.RadioButton windowsAuth;
        private ShipWorks.UI.Wizard.WizardPage wizardPageDatabaseName;
        private System.Windows.Forms.TextBox databaseName;
        private System.Windows.Forms.Label labelDatabaseName;
        private System.Windows.Forms.Label labelEnterDatabaseName;
        private ShipWorks.UI.Wizard.WizardPage wizardPageChooseDatabase;
        private System.Windows.Forms.ComboBox databaseNames;
        private System.Windows.Forms.Label labelFoundDatabases;
        private ShipWorks.UI.Wizard.WizardPage wizardPageDownloadSqlServer;
        private System.Windows.Forms.Label bytesSqlServer;
        private System.Windows.Forms.Button downloadSqlServer;
        private System.Windows.Forms.ProgressBar progressSqlServer;
        private System.Windows.Forms.Label labelDownloadSqlServer;
        private ShipWorks.UI.Wizard.WizardPage wizardPageInstallSqlServer;
        private System.Windows.Forms.Label labelInstallingSqlServer;
        private System.Windows.Forms.Panel labelInstallSqlServer;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label labelNote;
        private System.Windows.Forms.Label labelWarning;
        private ShipWorks.UI.Wizard.WizardPage wizardPageComplete;
        private System.Windows.Forms.PictureBox iconSetupComplete;
        private System.Windows.Forms.Label labelSetupComplete;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBoxSetupNewDatabase;
        private System.Windows.Forms.Label labelSetupNewDatabase;
        private System.Windows.Forms.Label labelConnectRunningDatabase;
        private System.Windows.Forms.PictureBox pictureBoxConnectRunningDatabase;
        private System.Windows.Forms.Label labelRestoreDatabase;
        private System.Windows.Forms.PictureBox pictureBoxRestoreDatabase;
        private System.Windows.Forms.Label label5;
        private ShipWorks.UI.Wizard.WizardPage wizardPageShipWorksAdmin;
        private System.Windows.Forms.CheckBox swAutomaticLogon;
        private System.Windows.Forms.TextBox swEmail;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox swPasswordAgain;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox swPassword;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox swUsername;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private ShipWorks.UI.Wizard.WizardPage wizardPageRestoreDatabase;
        private ShipWorks.UI.Wizard.WizardPage wizardPageRestoreOption;
        private System.Windows.Forms.RadioButton radioRestoreIntoCurrent;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RadioButton radioRestoreIntoNewDatabase;
        private System.Windows.Forms.Label label15;
        private ShipWorks.UI.Controls.PathTextBox backupFile;
        private System.Windows.Forms.Button browseForBackupFile;
        private System.Windows.Forms.Label labelBackupFile;
        private System.Windows.Forms.OpenFileDialog openBackupFileDialog;
        private ShipWorks.UI.Wizard.WizardPage wizardPageRestoreLogin;
        private System.Windows.Forms.PictureBox headerImage;
        private System.Windows.Forms.TextBox restorePassword;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox restoreUsername;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.GroupBox groupInfo;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label labelNote1;
        private System.Windows.Forms.PictureBox warningIcon;
        private System.Windows.Forms.Label labelNote2;
        private System.Windows.Forms.Label labelCantRestore;
        private System.Windows.Forms.PictureBox pictureServerSearching;
        private System.Windows.Forms.Label labelServerSearching;
        private ShipWorks.UI.Wizard.WizardPage wizardPageWindowsFirewall;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Label label14;
        private ShipWorks.UI.Controls.ShieldButton updateWindowsFirewall;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label firewallUpdatedLabel;
        private System.Windows.Forms.PictureBox firewallUpdatedPicture;
        private System.Windows.Forms.PictureBox pictureBox6;
        private ShipWorks.UI.Wizard.WizardPage wizardPagePrerequisitePlaceholder;
        private System.Windows.Forms.Label label24;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private UI.Controls.InfoTip infoTip1;
        private UI.Controls.InfoTip infoTipDatabaseName;
        private UI.Controls.InfoTip helpAutomaticLogon;
        private UI.Controls.InfoTip helpUserEmail;
    }
}
