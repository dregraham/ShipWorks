namespace ShipWorks.Data.Administration
{
    partial class DetailedDatabaseSetupWizard
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
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer1 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DetailedDatabaseSetupWizard));
            this.wizardPageChooseWisely2012 = new ShipWorks.UI.Wizard.WizardPage();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabelAdvancedOptions = new System.Windows.Forms.LinkLabel();
            this.linkEnableRemoteConnections = new ShipWorks.UI.Controls.LinkControl();
            this.labelAnotherPC = new System.Windows.Forms.Label();
            this.pictureBoxAnotherPC = new System.Windows.Forms.PictureBox();
            this.radioChooseRestore2012 = new System.Windows.Forms.RadioButton();
            this.radioChooseConnect2012 = new System.Windows.Forms.RadioButton();
            this.radioChooseCreate2012 = new System.Windows.Forms.RadioButton();
            this.wizardPageChooseSqlServer = new ShipWorks.UI.Wizard.WizardPage();
            this.panelSqlInstanceInstall = new System.Windows.Forms.Panel();
            this.radioInstallSqlServer = new System.Windows.Forms.RadioButton();
            this.labelInstanceName = new System.Windows.Forms.Label();
            this.instanceName = new System.Windows.Forms.TextBox();
            this.infoTip1 = new ShipWorks.UI.Controls.InfoTip();
            this.panelSqlInstanceRunning = new System.Windows.Forms.Panel();
            this.labelSqlServerRunning3 = new System.Windows.Forms.Label();
            this.radioSqlServerRunning = new System.Windows.Forms.RadioButton();
            this.panelSqlInstanceCurrent = new System.Windows.Forms.Panel();
            this.labelSqlServerCurrentName = new System.Windows.Forms.Label();
            this.radioSqlServerCurrent = new System.Windows.Forms.RadioButton();
            this.labelChooseNewOrExistingSqlServer = new System.Windows.Forms.Label();
            this.wizardPageSelectSqlServerInstance = new ShipWorks.UI.Wizard.WizardPage();
            this.panelSearchSqlServers = new System.Windows.Forms.Panel();
            this.searchSqlServersLabel = new System.Windows.Forms.Label();
            this.searchSqlServersPicture = new System.Windows.Forms.PictureBox();
            this.panelSelectedInstance = new System.Windows.Forms.Panel();
            this.labelDatabaseSelect = new System.Windows.Forms.Label();
            this.pictureSqlConnection = new System.Windows.Forms.PictureBox();
            this.gridDatabses = new Divelements.SandGrid.SandGrid();
            this.gridColumnName = new Divelements.SandGrid.GridColumn();
            this.gridColumnStatus = new Divelements.SandGrid.GridColumn();
            this.gridColumnStoreType = new Divelements.SandGrid.GridColumn();
            this.gridColumnLatestOrder = new Divelements.SandGrid.GridColumn();
            this.labelSqlConnection = new System.Windows.Forms.Label();
            this.linkSqlInstanceAccount = new ShipWorks.UI.Controls.LinkControl();
            this.panelSqlInstanceHelp = new System.Windows.Forms.Panel();
            this.linkHelpSelectSqlInstance = new ShipWorks.UI.Controls.LinkControl();
            this.labelSelectSqlInstance2 = new System.Windows.Forms.Label();
            this.labelSelectSqlInstance = new System.Windows.Forms.Label();
            this.comboSqlServers = new ShipWorks.UI.Controls.ImageComboBox();
            this.wizardPageDatabaseName = new ShipWorks.UI.Wizard.WizardPage();
            this.panelDatabaseGivenName = new System.Windows.Forms.Panel();
            this.linkEditGivenDatabaseName = new System.Windows.Forms.LinkLabel();
            this.givenDatabaseName = new System.Windows.Forms.Label();
            this.labelGivenDatabaseName = new System.Windows.Forms.Label();
            this.panelDatabaseChooseName = new System.Windows.Forms.Panel();
            this.labelEnterDatabaseName = new System.Windows.Forms.Label();
            this.linkChooseDataLocation = new ShipWorks.UI.Controls.LinkControl();
            this.panelDataFiles = new System.Windows.Forms.Panel();
            this.pathDataFiles = new ShipWorks.UI.Controls.PathTextBox();
            this.browseDataFiles = new System.Windows.Forms.Button();
            this.labelDataFiles = new System.Windows.Forms.Label();
            this.databaseName = new System.Windows.Forms.TextBox();
            this.labelDatabaseName = new System.Windows.Forms.Label();
            this.wizardPageInstallSqlServer = new ShipWorks.UI.Wizard.WizardPage();
            this.panelSqlServerInstallProgress = new System.Windows.Forms.Panel();
            this.progressPreparing = new System.Windows.Forms.ProgressBar();
            this.picturePreparing = new System.Windows.Forms.PictureBox();
            this.labelPeparingToRun = new System.Windows.Forms.Label();
            this.pictureBoxPreparing = new System.Windows.Forms.PictureBox();
            this.labelPreparing = new System.Windows.Forms.Label();
            this.panelSqlServerInstallReady = new System.Windows.Forms.Panel();
            this.label21 = new System.Windows.Forms.Label();
            this.wizardPageComplete = new ShipWorks.UI.Wizard.WizardPage();
            this.labelSetupComplete = new System.Windows.Forms.Label();
            this.iconSetupComplete = new System.Windows.Forms.PictureBox();
            this.wizardPageShipWorksAdmin = new ShipWorks.UI.Wizard.WizardPage();
            this.helpUserEmail = new ShipWorks.UI.Controls.InfoTip();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
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
            this.labelRestoreIntoNewDatabase = new System.Windows.Forms.Label();
            this.radioRestoreIntoNewDatabase = new System.Windows.Forms.RadioButton();
            this.radioRestoreIntoCurrent = new System.Windows.Forms.RadioButton();
            this.openBackupFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.wizardPageRestoreLogin = new ShipWorks.UI.Wizard.WizardPage();
            this.forgotPassword = new System.Windows.Forms.Label();
            this.forgotUsername = new System.Windows.Forms.Label();
            this.headerImage = new System.Windows.Forms.PictureBox();
            this.restorePassword = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.restoreUsername = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.wizardPagePrerequisitePlaceholder = new ShipWorks.UI.Wizard.WizardPage();
            this.label24 = new System.Windows.Forms.Label();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.databaseLocationBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.progressInstallTimer = new System.Windows.Forms.Timer(this.components);
            this.wizardPageChooseWisely2008 = new ShipWorks.UI.Wizard.WizardPage();
            this.radioChooseCreate2008 = new System.Windows.Forms.RadioButton();
            this.labelRestoreDatabase = new System.Windows.Forms.Label();
            this.radioChooseConnect2008 = new System.Windows.Forms.RadioButton();
            this.pictureBoxRestoreDatabase = new System.Windows.Forms.PictureBox();
            this.radioChooseRestore2008 = new System.Windows.Forms.RadioButton();
            this.labelConnectRunningDatabase = new System.Windows.Forms.Label();
            this.pictureBoxSetupNewDatabase = new System.Windows.Forms.PictureBox();
            this.pictureBoxConnectRunningDatabase = new System.Windows.Forms.PictureBox();
            this.labelSetupNewDatabase = new System.Windows.Forms.Label();
            this.wizardPageManageLocalDb = new ShipWorks.UI.Wizard.WizardPage();
            this.linkAdvancedOptionsLocalDb = new System.Windows.Forms.LinkLabel();
            this.radioRestoreBackupLocalDb = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.linkHelpEnableRemote = new ShipWorks.UI.Controls.LinkControl();
            this.labelLocalDbEnableRemote2 = new System.Windows.Forms.Label();
            this.pictureLocalDbEnableRemote = new System.Windows.Forms.PictureBox();
            this.radioLocalDbEnableRemote = new System.Windows.Forms.RadioButton();
            this.labelLocalDbConnect2 = new System.Windows.Forms.Label();
            this.pictureLocalDbConnect = new System.Windows.Forms.PictureBox();
            this.radioLocalDbConnect = new System.Windows.Forms.RadioButton();
            this.wizardPageUpgradeLocalDb = new ShipWorks.UI.Wizard.WizardPage();
            this.panelUpgradeLocalDb = new System.Windows.Forms.Panel();
            this.progressUpdgradeLocalDb = new System.Windows.Forms.ProgressBar();
            this.pictureUpgradeLocalDbStatus = new System.Windows.Forms.PictureBox();
            this.labelUpgradeLocalDb2 = new System.Windows.Forms.Label();
            this.pictureUpgrdaeLocalDb = new System.Windows.Forms.PictureBox();
            this.labelUpgradeLocalDb = new System.Windows.Forms.Label();
            this.panelUpgradeLocalDbReady = new System.Windows.Forms.Panel();
            this.labelUpgradeLocalDbReady = new System.Windows.Forms.Label();
            this.progressLocalDbTimer = new System.Windows.Forms.Timer(this.components);
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageChooseWisely2012.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAnotherPC)).BeginInit();
            this.wizardPageChooseSqlServer.SuspendLayout();
            this.panelSqlInstanceInstall.SuspendLayout();
            this.panelSqlInstanceRunning.SuspendLayout();
            this.panelSqlInstanceCurrent.SuspendLayout();
            this.wizardPageSelectSqlServerInstance.SuspendLayout();
            this.panelSearchSqlServers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchSqlServersPicture)).BeginInit();
            this.panelSelectedInstance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSqlConnection)).BeginInit();
            this.panelSqlInstanceHelp.SuspendLayout();
            this.wizardPageDatabaseName.SuspendLayout();
            this.panelDatabaseGivenName.SuspendLayout();
            this.panelDatabaseChooseName.SuspendLayout();
            this.panelDataFiles.SuspendLayout();
            this.wizardPageInstallSqlServer.SuspendLayout();
            this.panelSqlServerInstallProgress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePreparing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreparing)).BeginInit();
            this.panelSqlServerInstallReady.SuspendLayout();
            this.wizardPageComplete.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconSetupComplete)).BeginInit();
            this.wizardPageShipWorksAdmin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            this.wizardPageRestoreDatabase.SuspendLayout();
            this.groupInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.warningIcon)).BeginInit();
            this.wizardPageRestoreOption.SuspendLayout();
            this.wizardPageRestoreLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headerImage)).BeginInit();
            this.wizardPagePrerequisitePlaceholder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.wizardPageChooseWisely2008.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRestoreDatabase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSetupNewDatabase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxConnectRunningDatabase)).BeginInit();
            this.wizardPageManageLocalDb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLocalDbEnableRemote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLocalDbConnect)).BeginInit();
            this.wizardPageUpgradeLocalDb.SuspendLayout();
            this.panelUpgradeLocalDb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureUpgradeLocalDbStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureUpgrdaeLocalDb)).BeginInit();
            this.panelUpgradeLocalDbReady.SuspendLayout();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(380, 379);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(461, 379);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(299, 379);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageRestoreLogin);
            this.mainPanel.Size = new System.Drawing.Size(548, 307);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 369);
            this.etchBottom.Size = new System.Drawing.Size(552, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.sw_cubes_big;
            this.pictureBox.Location = new System.Drawing.Point(491, 3);
            this.pictureBox.Size = new System.Drawing.Size(54, 50);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(548, 56);
            // 
            // wizardPageChooseWisely2012
            // 
            this.wizardPageChooseWisely2012.Controls.Add(this.label1);
            this.wizardPageChooseWisely2012.Controls.Add(this.linkLabelAdvancedOptions);
            this.wizardPageChooseWisely2012.Controls.Add(this.linkEnableRemoteConnections);
            this.wizardPageChooseWisely2012.Controls.Add(this.labelAnotherPC);
            this.wizardPageChooseWisely2012.Controls.Add(this.pictureBoxAnotherPC);
            this.wizardPageChooseWisely2012.Controls.Add(this.radioChooseRestore2012);
            this.wizardPageChooseWisely2012.Controls.Add(this.radioChooseConnect2012);
            this.wizardPageChooseWisely2012.Controls.Add(this.radioChooseCreate2012);
            this.wizardPageChooseWisely2012.Description = "Setup or connect to a ShipWorks database.";
            this.wizardPageChooseWisely2012.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageChooseWisely2012.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageChooseWisely2012.Location = new System.Drawing.Point(0, 0);
            this.wizardPageChooseWisely2012.Name = "wizardPageChooseWisely2012";
            this.wizardPageChooseWisely2012.Size = new System.Drawing.Size(548, 307);
            this.wizardPageChooseWisely2012.TabIndex = 0;
            this.wizardPageChooseWisely2012.Title = "Database Configuration";
            this.wizardPageChooseWisely2012.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextSetupOrConnect);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label1.Location = new System.Drawing.Point(80, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(266, 13);
            this.label1.TabIndex = 54;
            this.label1.Text = "But first, make sure it\'s ready for remote connections.";
            // 
            // linkLabelAdvancedOptions
            // 
            this.linkLabelAdvancedOptions.AutoSize = true;
            this.linkLabelAdvancedOptions.Location = new System.Drawing.Point(22, 76);
            this.linkLabelAdvancedOptions.Name = "linkLabelAdvancedOptions";
            this.linkLabelAdvancedOptions.Size = new System.Drawing.Size(121, 13);
            this.linkLabelAdvancedOptions.TabIndex = 53;
            this.linkLabelAdvancedOptions.TabStop = true;
            this.linkLabelAdvancedOptions.Text = "Show advanced options";
            this.linkLabelAdvancedOptions.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnChooseWiselyAdvancedOptions);
            // 
            // linkEnableRemoteConnections
            // 
            this.linkEnableRemoteConnections.AutoSize = true;
            this.linkEnableRemoteConnections.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkEnableRemoteConnections.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkEnableRemoteConnections.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.linkEnableRemoteConnections.Location = new System.Drawing.Point(347, 49);
            this.linkEnableRemoteConnections.Name = "linkEnableRemoteConnections";
            this.linkEnableRemoteConnections.Size = new System.Drawing.Size(57, 13);
            this.linkEnableRemoteConnections.TabIndex = 52;
            this.linkEnableRemoteConnections.Text = "Learn how";
            this.linkEnableRemoteConnections.Click += new System.EventHandler(this.OnLinkLearnEnableRemoteConnections);
            // 
            // labelAnotherPC
            // 
            this.labelAnotherPC.Location = new System.Drawing.Point(80, 32);
            this.labelAnotherPC.Name = "labelAnotherPC";
            this.labelAnotherPC.Size = new System.Drawing.Size(406, 17);
            this.labelAnotherPC.TabIndex = 49;
            this.labelAnotherPC.Text = "Is ShipWorks already on another PC? Let\'s get you connected.";
            // 
            // pictureBoxAnotherPC
            // 
            this.pictureBoxAnotherPC.Image = global::ShipWorks.Properties.Resources.server_to_client;
            this.pictureBoxAnotherPC.Location = new System.Drawing.Point(43, 32);
            this.pictureBoxAnotherPC.Name = "pictureBoxAnotherPC";
            this.pictureBoxAnotherPC.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxAnotherPC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxAnotherPC.TabIndex = 51;
            this.pictureBoxAnotherPC.TabStop = false;
            // 
            // radioChooseRestore2012
            // 
            this.radioChooseRestore2012.AutoSize = true;
            this.radioChooseRestore2012.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioChooseRestore2012.Location = new System.Drawing.Point(23, 125);
            this.radioChooseRestore2012.Name = "radioChooseRestore2012";
            this.radioChooseRestore2012.Size = new System.Drawing.Size(210, 17);
            this.radioChooseRestore2012.TabIndex = 50;
            this.radioChooseRestore2012.Text = "Restore a ShipWorks database backup";
            this.radioChooseRestore2012.UseVisualStyleBackColor = true;
            this.radioChooseRestore2012.Visible = false;
            // 
            // radioChooseConnect2012
            // 
            this.radioChooseConnect2012.AutoSize = true;
            this.radioChooseConnect2012.Checked = true;
            this.radioChooseConnect2012.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioChooseConnect2012.Location = new System.Drawing.Point(23, 9);
            this.radioChooseConnect2012.Name = "radioChooseConnect2012";
            this.radioChooseConnect2012.Size = new System.Drawing.Size(277, 17);
            this.radioChooseConnect2012.TabIndex = 48;
            this.radioChooseConnect2012.TabStop = true;
            this.radioChooseConnect2012.Text = "Connect to ShipWorks running on another PC";
            this.radioChooseConnect2012.UseVisualStyleBackColor = true;
            // 
            // radioChooseCreate2012
            // 
            this.radioChooseCreate2012.AutoSize = true;
            this.radioChooseCreate2012.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioChooseCreate2012.Location = new System.Drawing.Point(23, 99);
            this.radioChooseCreate2012.Name = "radioChooseCreate2012";
            this.radioChooseCreate2012.Size = new System.Drawing.Size(186, 17);
            this.radioChooseCreate2012.TabIndex = 47;
            this.radioChooseCreate2012.Text = "Setup a new ShipWorks database";
            this.radioChooseCreate2012.UseVisualStyleBackColor = true;
            this.radioChooseCreate2012.Visible = false;
            // 
            // wizardPageChooseSqlServer
            // 
            this.wizardPageChooseSqlServer.Controls.Add(this.panelSqlInstanceInstall);
            this.wizardPageChooseSqlServer.Controls.Add(this.panelSqlInstanceRunning);
            this.wizardPageChooseSqlServer.Controls.Add(this.panelSqlInstanceCurrent);
            this.wizardPageChooseSqlServer.Controls.Add(this.labelChooseNewOrExistingSqlServer);
            this.wizardPageChooseSqlServer.Description = "Select where your database will be created.";
            this.wizardPageChooseSqlServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageChooseSqlServer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageChooseSqlServer.Location = new System.Drawing.Point(0, 0);
            this.wizardPageChooseSqlServer.Name = "wizardPageChooseSqlServer";
            this.wizardPageChooseSqlServer.Size = new System.Drawing.Size(548, 307);
            this.wizardPageChooseSqlServer.TabIndex = 0;
            this.wizardPageChooseSqlServer.Title = "Database Configuration";
            this.wizardPageChooseSqlServer.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextNewOrExistingSqlServer);
            this.wizardPageChooseSqlServer.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoNewOrExistingSqlServer);
            // 
            // panelSqlInstanceInstall
            // 
            this.panelSqlInstanceInstall.Controls.Add(this.radioInstallSqlServer);
            this.panelSqlInstanceInstall.Controls.Add(this.labelInstanceName);
            this.panelSqlInstanceInstall.Controls.Add(this.instanceName);
            this.panelSqlInstanceInstall.Controls.Add(this.infoTip1);
            this.panelSqlInstanceInstall.Location = new System.Drawing.Point(40, 77);
            this.panelSqlInstanceInstall.Name = "panelSqlInstanceInstall";
            this.panelSqlInstanceInstall.Size = new System.Drawing.Size(481, 53);
            this.panelSqlInstanceInstall.TabIndex = 45;
            // 
            // radioInstallSqlServer
            // 
            this.radioInstallSqlServer.AutoSize = true;
            this.radioInstallSqlServer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioInstallSqlServer.Location = new System.Drawing.Point(3, 3);
            this.radioInstallSqlServer.Name = "radioInstallSqlServer";
            this.radioInstallSqlServer.Size = new System.Drawing.Size(246, 17);
            this.radioInstallSqlServer.TabIndex = 1;
            this.radioInstallSqlServer.Text = "Install a new instance of Microsoft SQL Server";
            this.radioInstallSqlServer.UseVisualStyleBackColor = true;
            this.radioInstallSqlServer.CheckedChanged += new System.EventHandler(this.OnChangeInstallSqlServerOption);
            // 
            // labelInstanceName
            // 
            this.labelInstanceName.AutoSize = true;
            this.labelInstanceName.Location = new System.Drawing.Point(23, 29);
            this.labelInstanceName.Name = "labelInstanceName";
            this.labelInstanceName.Size = new System.Drawing.Size(104, 13);
            this.labelInstanceName.TabIndex = 2;
            this.labelInstanceName.Text = "New instance name:";
            // 
            // instanceName
            // 
            this.instanceName.Location = new System.Drawing.Point(129, 26);
            this.instanceName.Name = "instanceName";
            this.instanceName.Size = new System.Drawing.Size(100, 21);
            this.instanceName.TabIndex = 3;
            this.instanceName.Text = "SHIPWORKS";
            // 
            // infoTip1
            // 
            this.infoTip1.Caption = "Multiple instances of Microsoft SQL Server can be installed on a computer.  Each " +
    "one must have a unique instance name that is used when connecting to it.";
            this.infoTip1.Location = new System.Drawing.Point(235, 30);
            this.infoTip1.Name = "infoTip1";
            this.infoTip1.Size = new System.Drawing.Size(12, 12);
            this.infoTip1.TabIndex = 7;
            this.infoTip1.Title = "Instance Name";
            // 
            // panelSqlInstanceRunning
            // 
            this.panelSqlInstanceRunning.Controls.Add(this.labelSqlServerRunning3);
            this.panelSqlInstanceRunning.Controls.Add(this.radioSqlServerRunning);
            this.panelSqlInstanceRunning.Location = new System.Drawing.Point(40, 52);
            this.panelSqlInstanceRunning.Name = "panelSqlInstanceRunning";
            this.panelSqlInstanceRunning.Size = new System.Drawing.Size(481, 25);
            this.panelSqlInstanceRunning.TabIndex = 44;
            // 
            // labelSqlServerRunning3
            // 
            this.labelSqlServerRunning3.AutoSize = true;
            this.labelSqlServerRunning3.ForeColor = System.Drawing.Color.DimGray;
            this.labelSqlServerRunning3.Location = new System.Drawing.Point(276, 5);
            this.labelSqlServerRunning3.Name = "labelSqlServerRunning3";
            this.labelSqlServerRunning3.Size = new System.Drawing.Size(155, 13);
            this.labelSqlServerRunning3.TabIndex = 46;
            this.labelSqlServerRunning3.Text = "(You\'ll choose in the next step)";
            // 
            // radioSqlServerRunning
            // 
            this.radioSqlServerRunning.AutoSize = true;
            this.radioSqlServerRunning.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioSqlServerRunning.Location = new System.Drawing.Point(3, 3);
            this.radioSqlServerRunning.Name = "radioSqlServerRunning";
            this.radioSqlServerRunning.Size = new System.Drawing.Size(275, 17);
            this.radioSqlServerRunning.TabIndex = 4;
            this.radioSqlServerRunning.Text = "Choose a different instance of Microsoft SQL Server";
            this.radioSqlServerRunning.UseVisualStyleBackColor = true;
            this.radioSqlServerRunning.CheckedChanged += new System.EventHandler(this.OnChangeInstallSqlServerOption);
            // 
            // panelSqlInstanceCurrent
            // 
            this.panelSqlInstanceCurrent.Controls.Add(this.labelSqlServerCurrentName);
            this.panelSqlInstanceCurrent.Controls.Add(this.radioSqlServerCurrent);
            this.panelSqlInstanceCurrent.Location = new System.Drawing.Point(40, 27);
            this.panelSqlInstanceCurrent.Name = "panelSqlInstanceCurrent";
            this.panelSqlInstanceCurrent.Size = new System.Drawing.Size(481, 25);
            this.panelSqlInstanceCurrent.TabIndex = 43;
            // 
            // labelSqlServerCurrentName
            // 
            this.labelSqlServerCurrentName.AutoSize = true;
            this.labelSqlServerCurrentName.ForeColor = System.Drawing.Color.DimGray;
            this.labelSqlServerCurrentName.Location = new System.Drawing.Point(261, 5);
            this.labelSqlServerCurrentName.Name = "labelSqlServerCurrentName";
            this.labelSqlServerCurrentName.Size = new System.Drawing.Size(116, 13);
            this.labelSqlServerCurrentName.TabIndex = 46;
            this.labelSqlServerCurrentName.Text = "(COMPUTER\\Instance)";
            // 
            // radioSqlServerCurrent
            // 
            this.radioSqlServerCurrent.AutoSize = true;
            this.radioSqlServerCurrent.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioSqlServerCurrent.Location = new System.Drawing.Point(3, 3);
            this.radioSqlServerCurrent.Name = "radioSqlServerCurrent";
            this.radioSqlServerCurrent.Size = new System.Drawing.Size(260, 17);
            this.radioSqlServerCurrent.TabIndex = 4;
            this.radioSqlServerCurrent.Text = "Use the current instance of Microsoft SQL Server";
            this.radioSqlServerCurrent.UseVisualStyleBackColor = true;
            this.radioSqlServerCurrent.CheckedChanged += new System.EventHandler(this.OnChangeInstallSqlServerOption);
            // 
            // labelChooseNewOrExistingSqlServer
            // 
            this.labelChooseNewOrExistingSqlServer.AutoSize = true;
            this.labelChooseNewOrExistingSqlServer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChooseNewOrExistingSqlServer.Location = new System.Drawing.Point(24, 6);
            this.labelChooseNewOrExistingSqlServer.Name = "labelChooseNewOrExistingSqlServer";
            this.labelChooseNewOrExistingSqlServer.Size = new System.Drawing.Size(202, 13);
            this.labelChooseNewOrExistingSqlServer.TabIndex = 0;
            this.labelChooseNewOrExistingSqlServer.Text = "For your new ShipWorks database:";
            // 
            // wizardPageSelectSqlServerInstance
            // 
            this.wizardPageSelectSqlServerInstance.Controls.Add(this.panelSearchSqlServers);
            this.wizardPageSelectSqlServerInstance.Controls.Add(this.panelSelectedInstance);
            this.wizardPageSelectSqlServerInstance.Controls.Add(this.panelSqlInstanceHelp);
            this.wizardPageSelectSqlServerInstance.Controls.Add(this.labelSelectSqlInstance);
            this.wizardPageSelectSqlServerInstance.Controls.Add(this.comboSqlServers);
            this.wizardPageSelectSqlServerInstance.Description = "Choose a running database to connect to.";
            this.wizardPageSelectSqlServerInstance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageSelectSqlServerInstance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageSelectSqlServerInstance.Location = new System.Drawing.Point(0, 0);
            this.wizardPageSelectSqlServerInstance.Name = "wizardPageSelectSqlServerInstance";
            this.wizardPageSelectSqlServerInstance.Size = new System.Drawing.Size(548, 307);
            this.wizardPageSelectSqlServerInstance.TabIndex = 0;
            this.wizardPageSelectSqlServerInstance.Title = "Connect to ShipWorks";
            this.wizardPageSelectSqlServerInstance.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextSelectSqlInstance);
            this.wizardPageSelectSqlServerInstance.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoSelectSqlInstance);
            // 
            // panelSearchSqlServers
            // 
            this.panelSearchSqlServers.Controls.Add(this.searchSqlServersLabel);
            this.panelSearchSqlServers.Controls.Add(this.searchSqlServersPicture);
            this.panelSearchSqlServers.Location = new System.Drawing.Point(267, 31);
            this.panelSearchSqlServers.Name = "panelSearchSqlServers";
            this.panelSearchSqlServers.Size = new System.Drawing.Size(256, 20);
            this.panelSearchSqlServers.TabIndex = 59;
            // 
            // searchSqlServersLabel
            // 
            this.searchSqlServersLabel.AutoSize = true;
            this.searchSqlServersLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.searchSqlServersLabel.Location = new System.Drawing.Point(19, 2);
            this.searchSqlServersLabel.Name = "searchSqlServersLabel";
            this.searchSqlServersLabel.Size = new System.Drawing.Size(66, 13);
            this.searchSqlServersLabel.TabIndex = 57;
            this.searchSqlServersLabel.Text = "Searching...";
            // 
            // searchSqlServersPicture
            // 
            this.searchSqlServersPicture.Image = global::ShipWorks.Properties.Resources.indiciator_green;
            this.searchSqlServersPicture.Location = new System.Drawing.Point(2, 0);
            this.searchSqlServersPicture.Name = "searchSqlServersPicture";
            this.searchSqlServersPicture.Size = new System.Drawing.Size(16, 16);
            this.searchSqlServersPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.searchSqlServersPicture.TabIndex = 58;
            this.searchSqlServersPicture.TabStop = false;
            // 
            // panelSelectedInstance
            // 
            this.panelSelectedInstance.Controls.Add(this.labelDatabaseSelect);
            this.panelSelectedInstance.Controls.Add(this.pictureSqlConnection);
            this.panelSelectedInstance.Controls.Add(this.gridDatabses);
            this.panelSelectedInstance.Controls.Add(this.labelSqlConnection);
            this.panelSelectedInstance.Controls.Add(this.linkSqlInstanceAccount);
            this.panelSelectedInstance.Location = new System.Drawing.Point(18, 58);
            this.panelSelectedInstance.Name = "panelSelectedInstance";
            this.panelSelectedInstance.Size = new System.Drawing.Size(500, 210);
            this.panelSelectedInstance.TabIndex = 56;
            // 
            // labelDatabaseSelect
            // 
            this.labelDatabaseSelect.AutoSize = true;
            this.labelDatabaseSelect.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDatabaseSelect.Location = new System.Drawing.Point(3, 4);
            this.labelDatabaseSelect.Name = "labelDatabaseSelect";
            this.labelDatabaseSelect.Size = new System.Drawing.Size(190, 13);
            this.labelDatabaseSelect.TabIndex = 54;
            this.labelDatabaseSelect.Text = "Select your ShipWorks database";
            // 
            // pictureSqlConnection
            // 
            this.pictureSqlConnection.Image = global::ShipWorks.Properties.Resources.arrows_greengray;
            this.pictureSqlConnection.Location = new System.Drawing.Point(22, 22);
            this.pictureSqlConnection.Name = "pictureSqlConnection";
            this.pictureSqlConnection.Size = new System.Drawing.Size(16, 16);
            this.pictureSqlConnection.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureSqlConnection.TabIndex = 53;
            this.pictureSqlConnection.TabStop = false;
            this.pictureSqlConnection.Visible = false;
            // 
            // gridDatabses
            // 
            this.gridDatabses.AllowGroupCollapse = true;
            this.gridDatabses.AllowMultipleSelection = false;
            this.gridDatabses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridDatabses.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnName,
            this.gridColumnStatus,
            this.gridColumnStoreType,
            this.gridColumnLatestOrder});
            this.gridDatabses.CommitOnLoseFocus = true;
            this.gridDatabses.EmptyTextForeColor = System.Drawing.SystemColors.GrayText;
            this.gridDatabses.EnableSearching = false;
            this.gridDatabses.GridLines = Divelements.SandGrid.GridLinesDisplayType.Both;
            this.gridDatabses.ImageTextSeparation = 1;
            this.gridDatabses.Location = new System.Drawing.Point(22, 46);
            this.gridDatabses.Name = "gridDatabses";
            this.gridDatabses.Renderer = windowsXPRenderer1;
            this.gridDatabses.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.InitiateDragDrop;
            this.gridDatabses.Rows.AddRange(new Divelements.SandGrid.GridRow[] {
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell("ShipWorks"),
                        new Divelements.SandGrid.GridCell("Ready"),
                        new Divelements.SandGrid.GridCell("Brian, on 07/13/11"),
                        new Divelements.SandGrid.GridCell("Order #1292, from 07/11/11")})});
            this.gridDatabses.Size = new System.Drawing.Size(467, 153);
            this.gridDatabses.TabIndex = 55;
            this.gridDatabses.WhitespaceClickBehavior = Divelements.SandGrid.WhitespaceClickBehavior.None;
            // 
            // gridColumnName
            // 
            this.gridColumnName.AllowReorder = false;
            this.gridColumnName.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnName.AutoSizeIncludeHeader = true;
            this.gridColumnName.AutoSortType = Divelements.SandGrid.ColumnAutoSortType.None;
            this.gridColumnName.Clickable = false;
            this.gridColumnName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumnName.ForeColorSource = Divelements.SandGrid.CellForeColorSource.RowCell;
            this.gridColumnName.HeaderText = "Name";
            this.gridColumnName.MinimumWidth = 75;
            // 
            // gridColumnStatus
            // 
            this.gridColumnStatus.AllowReorder = false;
            this.gridColumnStatus.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnStatus.Clickable = false;
            this.gridColumnStatus.HeaderText = "Status";
            this.gridColumnStatus.MinimumWidth = 75;
            // 
            // gridColumnStoreType
            // 
            this.gridColumnStoreType.AllowReorder = false;
            this.gridColumnStoreType.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnStoreType.Clickable = false;
            this.gridColumnStoreType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumnStoreType.ForeColorSource = Divelements.SandGrid.CellForeColorSource.RowCell;
            this.gridColumnStoreType.HeaderText = "Last Activity";
            this.gridColumnStoreType.MinimumWidth = 75;
            this.gridColumnStoreType.Width = 113;
            // 
            // gridColumnLatestOrder
            // 
            this.gridColumnLatestOrder.AllowReorder = false;
            this.gridColumnLatestOrder.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnLatestOrder.Clickable = false;
            this.gridColumnLatestOrder.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumnLatestOrder.ForeColorSource = Divelements.SandGrid.CellForeColorSource.RowCell;
            this.gridColumnLatestOrder.HeaderText = "Latest Order";
            this.gridColumnLatestOrder.MinimumWidth = 75;
            this.gridColumnLatestOrder.Width = 221;
            // 
            // labelSqlConnection
            // 
            this.labelSqlConnection.AutoSize = true;
            this.labelSqlConnection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelSqlConnection.Location = new System.Drawing.Point(39, 24);
            this.labelSqlConnection.Name = "labelSqlConnection";
            this.labelSqlConnection.Size = new System.Drawing.Size(262, 13);
            this.labelSqlConnection.TabIndex = 51;
            this.labelSqlConnection.Text = "Successfully connected using your Windows account.";
            this.labelSqlConnection.Visible = false;
            // 
            // linkSqlInstanceAccount
            // 
            this.linkSqlInstanceAccount.AutoSize = true;
            this.linkSqlInstanceAccount.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkSqlInstanceAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkSqlInstanceAccount.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.linkSqlInstanceAccount.Location = new System.Drawing.Point(300, 24);
            this.linkSqlInstanceAccount.Name = "linkSqlInstanceAccount";
            this.linkSqlInstanceAccount.Size = new System.Drawing.Size(137, 13);
            this.linkSqlInstanceAccount.TabIndex = 50;
            this.linkSqlInstanceAccount.Text = "Change the account to use";
            this.linkSqlInstanceAccount.Visible = false;
            this.linkSqlInstanceAccount.Click += new System.EventHandler(this.OnChangeSqlInstanceAccount);
            // 
            // panelSqlInstanceHelp
            // 
            this.panelSqlInstanceHelp.Controls.Add(this.linkHelpSelectSqlInstance);
            this.panelSqlInstanceHelp.Controls.Add(this.labelSelectSqlInstance2);
            this.panelSqlInstanceHelp.Location = new System.Drawing.Point(267, 6);
            this.panelSqlInstanceHelp.Name = "panelSqlInstanceHelp";
            this.panelSqlInstanceHelp.Size = new System.Drawing.Size(256, 20);
            this.panelSqlInstanceHelp.TabIndex = 52;
            this.panelSqlInstanceHelp.Visible = false;
            // 
            // linkHelpSelectSqlInstance
            // 
            this.linkHelpSelectSqlInstance.AutoSize = true;
            this.linkHelpSelectSqlInstance.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkHelpSelectSqlInstance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkHelpSelectSqlInstance.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.linkHelpSelectSqlInstance.Location = new System.Drawing.Point(65, 2);
            this.linkHelpSelectSqlInstance.Name = "linkHelpSelectSqlInstance";
            this.linkHelpSelectSqlInstance.Size = new System.Drawing.Size(127, 13);
            this.linkHelpSelectSqlInstance.TabIndex = 49;
            this.linkHelpSelectSqlInstance.Text = "Don\'t worry, we can help";
            this.linkHelpSelectSqlInstance.Click += new System.EventHandler(this.OnLinkSqlTroubleshooting);
            // 
            // labelSelectSqlInstance2
            // 
            this.labelSelectSqlInstance2.AutoSize = true;
            this.labelSelectSqlInstance2.ForeColor = System.Drawing.Color.DimGray;
            this.labelSelectSqlInstance2.Location = new System.Drawing.Point(0, 2);
            this.labelSelectSqlInstance2.Name = "labelSelectSqlInstance2";
            this.labelSelectSqlInstance2.Size = new System.Drawing.Size(66, 13);
            this.labelSelectSqlInstance2.TabIndex = 47;
            this.labelSelectSqlInstance2.Text = "Don\'t see it?";
            // 
            // labelSelectSqlInstance
            // 
            this.labelSelectSqlInstance.AutoSize = true;
            this.labelSelectSqlInstance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSelectSqlInstance.Location = new System.Drawing.Point(22, 6);
            this.labelSelectSqlInstance.Name = "labelSelectSqlInstance";
            this.labelSelectSqlInstance.Size = new System.Drawing.Size(193, 13);
            this.labelSelectSqlInstance.TabIndex = 46;
            this.labelSelectSqlInstance.Text = "Where is your database running?";
            // 
            // comboSqlServers
            // 
            this.comboSqlServers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.comboSqlServers.Location = new System.Drawing.Point(41, 29);
            this.comboSqlServers.Name = "comboSqlServers";
            this.comboSqlServers.Size = new System.Drawing.Size(220, 21);
            this.comboSqlServers.TabIndex = 6;
            this.comboSqlServers.SelectedIndexChanged += new System.EventHandler(this.OnChangeSelectedInstance);
            this.comboSqlServers.Leave += new System.EventHandler(this.OnLeaveSqlInstance);
            // 
            // wizardPageDatabaseName
            // 
            this.wizardPageDatabaseName.Controls.Add(this.panelDatabaseGivenName);
            this.wizardPageDatabaseName.Controls.Add(this.panelDatabaseChooseName);
            this.wizardPageDatabaseName.Description = "Confirm the name of the ShipWorks database.";
            this.wizardPageDatabaseName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageDatabaseName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageDatabaseName.Location = new System.Drawing.Point(0, 0);
            this.wizardPageDatabaseName.Name = "wizardPageDatabaseName";
            this.wizardPageDatabaseName.Size = new System.Drawing.Size(548, 307);
            this.wizardPageDatabaseName.TabIndex = 0;
            this.wizardPageDatabaseName.Title = "Database Name";
            this.wizardPageDatabaseName.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextCreateDatabase);
            this.wizardPageDatabaseName.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoCreateDatabase);
            // 
            // panelDatabaseGivenName
            // 
            this.panelDatabaseGivenName.Controls.Add(this.linkEditGivenDatabaseName);
            this.panelDatabaseGivenName.Controls.Add(this.givenDatabaseName);
            this.panelDatabaseGivenName.Controls.Add(this.labelGivenDatabaseName);
            this.panelDatabaseGivenName.Location = new System.Drawing.Point(3, 8);
            this.panelDatabaseGivenName.Name = "panelDatabaseGivenName";
            this.panelDatabaseGivenName.Size = new System.Drawing.Size(533, 20);
            this.panelDatabaseGivenName.TabIndex = 46;
            // 
            // linkEditGivenDatabaseName
            // 
            this.linkEditGivenDatabaseName.AutoSize = true;
            this.linkEditGivenDatabaseName.LinkColor = System.Drawing.Color.CornflowerBlue;
            this.linkEditGivenDatabaseName.Location = new System.Drawing.Point(276, 0);
            this.linkEditGivenDatabaseName.Name = "linkEditGivenDatabaseName";
            this.linkEditGivenDatabaseName.Size = new System.Drawing.Size(33, 13);
            this.linkEditGivenDatabaseName.TabIndex = 2;
            this.linkEditGivenDatabaseName.TabStop = true;
            this.linkEditGivenDatabaseName.Text = "(Edit)";
            this.linkEditGivenDatabaseName.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkEditDatabaseName);
            // 
            // givenDatabaseName
            // 
            this.givenDatabaseName.AutoSize = true;
            this.givenDatabaseName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.givenDatabaseName.Location = new System.Drawing.Point(188, 0);
            this.givenDatabaseName.Name = "givenDatabaseName";
            this.givenDatabaseName.Size = new System.Drawing.Size(88, 13);
            this.givenDatabaseName.TabIndex = 1;
            this.givenDatabaseName.Text = "ShipWorks123";
            // 
            // labelGivenDatabaseName
            // 
            this.labelGivenDatabaseName.AutoSize = true;
            this.labelGivenDatabaseName.Location = new System.Drawing.Point(23, 0);
            this.labelGivenDatabaseName.Name = "labelGivenDatabaseName";
            this.labelGivenDatabaseName.Size = new System.Drawing.Size(165, 13);
            this.labelGivenDatabaseName.TabIndex = 0;
            this.labelGivenDatabaseName.Text = "Your new database name will be:";
            // 
            // panelDatabaseChooseName
            // 
            this.panelDatabaseChooseName.Controls.Add(this.labelEnterDatabaseName);
            this.panelDatabaseChooseName.Controls.Add(this.linkChooseDataLocation);
            this.panelDatabaseChooseName.Controls.Add(this.panelDataFiles);
            this.panelDatabaseChooseName.Controls.Add(this.databaseName);
            this.panelDatabaseChooseName.Controls.Add(this.labelDatabaseName);
            this.panelDatabaseChooseName.Location = new System.Drawing.Point(3, 44);
            this.panelDatabaseChooseName.Name = "panelDatabaseChooseName";
            this.panelDatabaseChooseName.Size = new System.Drawing.Size(533, 107);
            this.panelDatabaseChooseName.TabIndex = 45;
            this.panelDatabaseChooseName.Visible = false;
            // 
            // labelEnterDatabaseName
            // 
            this.labelEnterDatabaseName.Location = new System.Drawing.Point(23, 0);
            this.labelEnterDatabaseName.Name = "labelEnterDatabaseName";
            this.labelEnterDatabaseName.Size = new System.Drawing.Size(348, 18);
            this.labelEnterDatabaseName.TabIndex = 5;
            this.labelEnterDatabaseName.Text = "Enter the name of the ShipWorks database to be created:";
            // 
            // linkChooseDataLocation
            // 
            this.linkChooseDataLocation.AutoSize = true;
            this.linkChooseDataLocation.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkChooseDataLocation.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkChooseDataLocation.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.linkChooseDataLocation.Location = new System.Drawing.Point(122, 47);
            this.linkChooseDataLocation.Name = "linkChooseDataLocation";
            this.linkChooseDataLocation.Size = new System.Drawing.Size(214, 13);
            this.linkChooseDataLocation.TabIndex = 43;
            this.linkChooseDataLocation.Text = "Let me choose the location of the data files";
            this.linkChooseDataLocation.Click += new System.EventHandler(this.OnChooseDataFileLocation);
            // 
            // panelDataFiles
            // 
            this.panelDataFiles.Controls.Add(this.pathDataFiles);
            this.panelDataFiles.Controls.Add(this.browseDataFiles);
            this.panelDataFiles.Controls.Add(this.labelDataFiles);
            this.panelDataFiles.Location = new System.Drawing.Point(2, 45);
            this.panelDataFiles.Name = "panelDataFiles";
            this.panelDataFiles.Size = new System.Drawing.Size(517, 59);
            this.panelDataFiles.TabIndex = 44;
            this.panelDataFiles.Visible = false;
            // 
            // pathDataFiles
            // 
            this.pathDataFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathDataFiles.Location = new System.Drawing.Point(121, 6);
            this.pathDataFiles.Name = "pathDataFiles";
            this.pathDataFiles.ReadOnly = true;
            this.pathDataFiles.Size = new System.Drawing.Size(365, 21);
            this.pathDataFiles.TabIndex = 4;
            // 
            // browseDataFiles
            // 
            this.browseDataFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseDataFiles.Location = new System.Drawing.Point(411, 31);
            this.browseDataFiles.Name = "browseDataFiles";
            this.browseDataFiles.Size = new System.Drawing.Size(75, 23);
            this.browseDataFiles.TabIndex = 5;
            this.browseDataFiles.Text = "Browse...";
            this.browseDataFiles.Click += new System.EventHandler(this.OnBrowseDatabaseLocation);
            // 
            // labelDataFiles
            // 
            this.labelDataFiles.AutoSize = true;
            this.labelDataFiles.Location = new System.Drawing.Point(45, 9);
            this.labelDataFiles.Name = "labelDataFiles";
            this.labelDataFiles.Size = new System.Drawing.Size(74, 13);
            this.labelDataFiles.TabIndex = 7;
            this.labelDataFiles.Text = "Data location:";
            // 
            // databaseName
            // 
            this.databaseName.Location = new System.Drawing.Point(123, 22);
            this.databaseName.Name = "databaseName";
            this.databaseName.Size = new System.Drawing.Size(132, 21);
            this.databaseName.TabIndex = 7;
            this.databaseName.Text = "ShipWorks";
            // 
            // labelDatabaseName
            // 
            this.labelDatabaseName.Location = new System.Drawing.Point(35, 24);
            this.labelDatabaseName.Name = "labelDatabaseName";
            this.labelDatabaseName.Size = new System.Drawing.Size(100, 23);
            this.labelDatabaseName.TabIndex = 6;
            this.labelDatabaseName.Text = "Database name:";
            // 
            // wizardPageInstallSqlServer
            // 
            this.wizardPageInstallSqlServer.Controls.Add(this.panelSqlServerInstallProgress);
            this.wizardPageInstallSqlServer.Controls.Add(this.panelSqlServerInstallReady);
            this.wizardPageInstallSqlServer.Description = "ShipWorks is ready to install Microsoft SQL Server.";
            this.wizardPageInstallSqlServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageInstallSqlServer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageInstallSqlServer.Location = new System.Drawing.Point(0, 0);
            this.wizardPageInstallSqlServer.Name = "wizardPageInstallSqlServer";
            this.wizardPageInstallSqlServer.NextRequiresElevation = true;
            this.wizardPageInstallSqlServer.Size = new System.Drawing.Size(548, 307);
            this.wizardPageInstallSqlServer.TabIndex = 0;
            this.wizardPageInstallSqlServer.Title = "Install Microsoft SQL Server";
            this.wizardPageInstallSqlServer.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextInstallSqlServer);
            this.wizardPageInstallSqlServer.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoInstallSqlServer);
            this.wizardPageInstallSqlServer.Cancelling += new System.ComponentModel.CancelEventHandler(this.OnCancellInstallSqlServer);
            // 
            // panelSqlServerInstallProgress
            // 
            this.panelSqlServerInstallProgress.Controls.Add(this.progressPreparing);
            this.panelSqlServerInstallProgress.Controls.Add(this.picturePreparing);
            this.panelSqlServerInstallProgress.Controls.Add(this.labelPeparingToRun);
            this.panelSqlServerInstallProgress.Controls.Add(this.pictureBoxPreparing);
            this.panelSqlServerInstallProgress.Controls.Add(this.labelPreparing);
            this.panelSqlServerInstallProgress.Location = new System.Drawing.Point(16, 90);
            this.panelSqlServerInstallProgress.Name = "panelSqlServerInstallProgress";
            this.panelSqlServerInstallProgress.Size = new System.Drawing.Size(478, 80);
            this.panelSqlServerInstallProgress.TabIndex = 156;
            this.panelSqlServerInstallProgress.Visible = false;
            // 
            // progressPreparing
            // 
            this.progressPreparing.Location = new System.Drawing.Point(70, 46);
            this.progressPreparing.Name = "progressPreparing";
            this.progressPreparing.Size = new System.Drawing.Size(379, 23);
            this.progressPreparing.TabIndex = 52;
            // 
            // picturePreparing
            // 
            this.picturePreparing.Image = ((System.Drawing.Image)(resources.GetObject("picturePreparing.Image")));
            this.picturePreparing.Location = new System.Drawing.Point(48, 49);
            this.picturePreparing.Name = "picturePreparing";
            this.picturePreparing.Size = new System.Drawing.Size(16, 16);
            this.picturePreparing.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picturePreparing.TabIndex = 51;
            this.picturePreparing.TabStop = false;
            // 
            // labelPeparingToRun
            // 
            this.labelPeparingToRun.AutoSize = true;
            this.labelPeparingToRun.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelPeparingToRun.Location = new System.Drawing.Point(46, 28);
            this.labelPeparingToRun.Name = "labelPeparingToRun";
            this.labelPeparingToRun.Size = new System.Drawing.Size(373, 13);
            this.labelPeparingToRun.TabIndex = 50;
            this.labelPeparingToRun.Text = "ShipWorks is installing Microsoft SQL Server.  This may take a few minutes...";
            // 
            // pictureBoxPreparing
            // 
            this.pictureBoxPreparing.Image = global::ShipWorks.Properties.Resources.gears_preferences;
            this.pictureBoxPreparing.Location = new System.Drawing.Point(9, 10);
            this.pictureBoxPreparing.Name = "pictureBoxPreparing";
            this.pictureBoxPreparing.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxPreparing.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxPreparing.TabIndex = 49;
            this.pictureBoxPreparing.TabStop = false;
            // 
            // labelPreparing
            // 
            this.labelPreparing.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPreparing.Location = new System.Drawing.Point(45, 12);
            this.labelPreparing.Name = "labelPreparing";
            this.labelPreparing.Size = new System.Drawing.Size(404, 16);
            this.labelPreparing.TabIndex = 48;
            this.labelPreparing.Text = "Installing Microsoft SQL Server";
            // 
            // panelSqlServerInstallReady
            // 
            this.panelSqlServerInstallReady.Controls.Add(this.label21);
            this.panelSqlServerInstallReady.Location = new System.Drawing.Point(16, 2);
            this.panelSqlServerInstallReady.Name = "panelSqlServerInstallReady";
            this.panelSqlServerInstallReady.Size = new System.Drawing.Size(478, 82);
            this.panelSqlServerInstallReady.TabIndex = 155;
            // 
            // label21
            // 
            this.label21.Location = new System.Drawing.Point(8, 12);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(470, 19);
            this.label21.TabIndex = 2;
            this.label21.Text = "ShipWorks is ready to install Microsoft SQL Server. Click Next to begin.\r\n";
            // 
            // wizardPageComplete
            // 
            this.wizardPageComplete.Controls.Add(this.labelSetupComplete);
            this.wizardPageComplete.Controls.Add(this.iconSetupComplete);
            this.wizardPageComplete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageComplete.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageComplete.Location = new System.Drawing.Point(0, 0);
            this.wizardPageComplete.Name = "wizardPageComplete";
            this.wizardPageComplete.Size = new System.Drawing.Size(548, 271);
            this.wizardPageComplete.TabIndex = 0;
            this.wizardPageComplete.Title = "Database Configuration Complete";
            this.wizardPageComplete.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoComplete);
            // 
            // labelSetupComplete
            // 
            this.labelSetupComplete.Location = new System.Drawing.Point(44, 11);
            this.labelSetupComplete.Name = "labelSetupComplete";
            this.labelSetupComplete.Size = new System.Drawing.Size(476, 82);
            this.labelSetupComplete.TabIndex = 1;
            this.labelSetupComplete.Text = "The database configuration is complete.";
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
            this.wizardPageShipWorksAdmin.Controls.Add(this.helpUserEmail);
            this.wizardPageShipWorksAdmin.Controls.Add(this.pictureBox6);
            this.wizardPageShipWorksAdmin.Controls.Add(this.swEmail);
            this.wizardPageShipWorksAdmin.Controls.Add(this.label7);
            this.wizardPageShipWorksAdmin.Controls.Add(this.swPasswordAgain);
            this.wizardPageShipWorksAdmin.Controls.Add(this.label6);
            this.wizardPageShipWorksAdmin.Controls.Add(this.swPassword);
            this.wizardPageShipWorksAdmin.Controls.Add(this.label8);
            this.wizardPageShipWorksAdmin.Controls.Add(this.swUsername);
            this.wizardPageShipWorksAdmin.Controls.Add(this.label9);
            this.wizardPageShipWorksAdmin.Controls.Add(this.label10);
            this.wizardPageShipWorksAdmin.Description = "Create a user account to log on to ShipWorks.";
            this.wizardPageShipWorksAdmin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageShipWorksAdmin.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageShipWorksAdmin.Location = new System.Drawing.Point(0, 0);
            this.wizardPageShipWorksAdmin.Name = "wizardPageShipWorksAdmin";
            this.wizardPageShipWorksAdmin.Size = new System.Drawing.Size(548, 271);
            this.wizardPageShipWorksAdmin.TabIndex = 0;
            this.wizardPageShipWorksAdmin.Title = "ShipWorks Account";
            this.wizardPageShipWorksAdmin.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextShipWorksAdmin);
            this.wizardPageShipWorksAdmin.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoShipWorksAdmin);
            // 
            // helpUserEmail
            // 
            this.helpUserEmail.Caption = "Your email address will be used to send you a new password if its forgotten.";
            this.helpUserEmail.Location = new System.Drawing.Point(384, 68);
            this.helpUserEmail.Name = "helpUserEmail";
            this.helpUserEmail.Size = new System.Drawing.Size(12, 12);
            this.helpUserEmail.TabIndex = 184;
            this.helpUserEmail.Title = "Email Address";
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = global::ShipWorks.Properties.Resources.dude31;
            this.pictureBox6.Location = new System.Drawing.Point(23, 8);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(32, 32);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox6.TabIndex = 183;
            this.pictureBox6.TabStop = false;
            // 
            // swEmail
            // 
            this.swEmail.Location = new System.Drawing.Point(135, 64);
            this.fieldLengthProvider.SetMaxLengthSource(this.swEmail, ShipWorks.Data.Utility.EntityFieldLengthSource.UserEmail);
            this.swEmail.Name = "swEmail";
            this.swEmail.Size = new System.Drawing.Size(243, 21);
            this.swEmail.TabIndex = 171;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(53, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 178;
            this.label7.Text = "Email address:";
            // 
            // swPasswordAgain
            // 
            this.swPasswordAgain.Location = new System.Drawing.Point(135, 118);
            this.fieldLengthProvider.SetMaxLengthSource(this.swPasswordAgain, ShipWorks.Data.Utility.EntityFieldLengthSource.UserPassword);
            this.swPasswordAgain.Name = "swPasswordAgain";
            this.swPasswordAgain.Size = new System.Drawing.Size(243, 21);
            this.swPasswordAgain.TabIndex = 173;
            this.swPasswordAgain.UseSystemPasswordChar = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(34, 121);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 177;
            this.label6.Text = "Retype password:";
            // 
            // swPassword
            // 
            this.swPassword.Location = new System.Drawing.Point(135, 91);
            this.fieldLengthProvider.SetMaxLengthSource(this.swPassword, ShipWorks.Data.Utility.EntityFieldLengthSource.UserPassword);
            this.swPassword.Name = "swPassword";
            this.swPassword.Size = new System.Drawing.Size(243, 21);
            this.swPassword.TabIndex = 172;
            this.swPassword.UseSystemPasswordChar = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(72, 94);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 176;
            this.label8.Text = "Password:";
            // 
            // swUsername
            // 
            this.swUsername.Location = new System.Drawing.Point(135, 37);
            this.fieldLengthProvider.SetMaxLengthSource(this.swUsername, ShipWorks.Data.Utility.EntityFieldLengthSource.UserName);
            this.swUsername.Name = "swUsername";
            this.swUsername.Size = new System.Drawing.Size(243, 21);
            this.swUsername.TabIndex = 170;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(70, 40);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 13);
            this.label9.TabIndex = 175;
            this.label9.Text = "Username:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(72, 8);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(172, 13);
            this.label10.TabIndex = 174;
            this.label10.Text = "Create your ShipWorks username:";
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
            this.wizardPageRestoreDatabase.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.label13.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.warningIcon.Image = ((System.Drawing.Image)(resources.GetObject("warningIcon.Image")));
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
            this.backupFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.backupFile.Location = new System.Drawing.Point(21, 26);
            this.backupFile.Name = "backupFile";
            this.backupFile.ReadOnly = true;
            this.backupFile.Size = new System.Drawing.Size(501, 21);
            this.backupFile.TabIndex = 167;
            // 
            // browseForBackupFile
            // 
            this.browseForBackupFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
            this.wizardPageRestoreOption.Controls.Add(this.labelRestoreIntoNewDatabase);
            this.wizardPageRestoreOption.Controls.Add(this.radioRestoreIntoNewDatabase);
            this.wizardPageRestoreOption.Controls.Add(this.radioRestoreIntoCurrent);
            this.wizardPageRestoreOption.Description = "Choose where the database will be restored to.";
            this.wizardPageRestoreOption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageRestoreOption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageRestoreOption.Location = new System.Drawing.Point(0, 0);
            this.wizardPageRestoreOption.Name = "wizardPageRestoreOption";
            this.wizardPageRestoreOption.Size = new System.Drawing.Size(548, 307);
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
            // labelRestoreIntoNewDatabase
            // 
            this.labelRestoreIntoNewDatabase.ForeColor = System.Drawing.Color.DimGray;
            this.labelRestoreIntoNewDatabase.Location = new System.Drawing.Point(43, 77);
            this.labelRestoreIntoNewDatabase.Name = "labelRestoreIntoNewDatabase";
            this.labelRestoreIntoNewDatabase.Size = new System.Drawing.Size(406, 15);
            this.labelRestoreIntoNewDatabase.TabIndex = 18;
            this.labelRestoreIntoNewDatabase.Text = "Select this option to create a new ShipWorks database to load the backup into.";
            // 
            // radioRestoreIntoNewDatabase
            // 
            this.radioRestoreIntoNewDatabase.AutoSize = true;
            this.radioRestoreIntoNewDatabase.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioRestoreIntoNewDatabase.Location = new System.Drawing.Point(23, 57);
            this.radioRestoreIntoNewDatabase.Name = "radioRestoreIntoNewDatabase";
            this.radioRestoreIntoNewDatabase.Size = new System.Drawing.Size(187, 17);
            this.radioRestoreIntoNewDatabase.TabIndex = 13;
            this.radioRestoreIntoNewDatabase.TabStop = true;
            this.radioRestoreIntoNewDatabase.Text = "Restore into a new database";
            this.radioRestoreIntoNewDatabase.UseVisualStyleBackColor = true;
            // 
            // radioRestoreIntoCurrent
            // 
            this.radioRestoreIntoCurrent.AutoSize = true;
            this.radioRestoreIntoCurrent.Checked = true;
            this.radioRestoreIntoCurrent.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioRestoreIntoCurrent.Location = new System.Drawing.Point(23, 8);
            this.radioRestoreIntoCurrent.Name = "radioRestoreIntoCurrent";
            this.radioRestoreIntoCurrent.Size = new System.Drawing.Size(222, 17);
            this.radioRestoreIntoCurrent.TabIndex = 0;
            this.radioRestoreIntoCurrent.TabStop = true;
            this.radioRestoreIntoCurrent.Text = "Restore over the current database";
            this.radioRestoreIntoCurrent.UseVisualStyleBackColor = true;
            // 
            // openBackupFileDialog
            // 
            this.openBackupFileDialog.DefaultExt = "swb";
            this.openBackupFileDialog.Filter = "ShipWorks Backup Files (*.swb)|*.swb";
            // 
            // wizardPageRestoreLogin
            // 
            this.wizardPageRestoreLogin.Controls.Add(this.forgotPassword);
            this.wizardPageRestoreLogin.Controls.Add(this.forgotUsername);
            this.wizardPageRestoreLogin.Controls.Add(this.headerImage);
            this.wizardPageRestoreLogin.Controls.Add(this.restorePassword);
            this.wizardPageRestoreLogin.Controls.Add(this.label16);
            this.wizardPageRestoreLogin.Controls.Add(this.restoreUsername);
            this.wizardPageRestoreLogin.Controls.Add(this.label17);
            this.wizardPageRestoreLogin.Controls.Add(this.label18);
            this.wizardPageRestoreLogin.Description = "Log on as a user with permission to restore a backup.";
            this.wizardPageRestoreLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageRestoreLogin.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageRestoreLogin.Location = new System.Drawing.Point(0, 0);
            this.wizardPageRestoreLogin.Name = "wizardPageRestoreLogin";
            this.wizardPageRestoreLogin.Size = new System.Drawing.Size(548, 307);
            this.wizardPageRestoreLogin.TabIndex = 0;
            this.wizardPageRestoreLogin.Title = "Log On Required";
            this.wizardPageRestoreLogin.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextRestoreLogin);
            this.wizardPageRestoreLogin.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoRestoreLogin);
            // 
            // forgotPassword
            // 
            this.forgotPassword.AutoSize = true;
            this.forgotPassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.forgotPassword.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.forgotPassword.ForeColor = System.Drawing.Color.Blue;
            this.forgotPassword.Location = new System.Drawing.Point(233, 87);
            this.forgotPassword.Name = "forgotPassword";
            this.forgotPassword.Size = new System.Drawing.Size(88, 13);
            this.forgotPassword.TabIndex = 176;
            this.forgotPassword.Text = "Forgot Password";
            this.forgotPassword.Click += new System.EventHandler(this.OnForgotPassword);
            // 
            // forgotUsername
            // 
            this.forgotUsername.AutoSize = true;
            this.forgotUsername.Cursor = System.Windows.Forms.Cursors.Hand;
            this.forgotUsername.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.forgotUsername.ForeColor = System.Drawing.Color.Blue;
            this.forgotUsername.Location = new System.Drawing.Point(145, 87);
            this.forgotUsername.Name = "forgotUsername";
            this.forgotUsername.Size = new System.Drawing.Size(90, 13);
            this.forgotUsername.TabIndex = 175;
            this.forgotUsername.Text = "Forgot Username";
            this.forgotUsername.Click += new System.EventHandler(this.OnForgotUsername);
            // 
            // headerImage
            // 
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
            // wizardPagePrerequisitePlaceholder
            // 
            this.wizardPagePrerequisitePlaceholder.Controls.Add(this.label24);
            this.wizardPagePrerequisitePlaceholder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPagePrerequisitePlaceholder.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPagePrerequisitePlaceholder.Location = new System.Drawing.Point(0, 0);
            this.wizardPagePrerequisitePlaceholder.Name = "wizardPagePrerequisitePlaceholder";
            this.wizardPagePrerequisitePlaceholder.Size = new System.Drawing.Size(548, 307);
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
            // databaseLocationBrowserDialog
            // 
            this.databaseLocationBrowserDialog.Description = "Database File Location";
            this.databaseLocationBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // progressInstallTimer
            // 
            this.progressInstallTimer.Tick += new System.EventHandler(this.OnInstallSqlServerProgressTimer);
            // 
            // wizardPageChooseWisely2008
            // 
            this.wizardPageChooseWisely2008.Controls.Add(this.radioChooseCreate2008);
            this.wizardPageChooseWisely2008.Controls.Add(this.labelRestoreDatabase);
            this.wizardPageChooseWisely2008.Controls.Add(this.radioChooseConnect2008);
            this.wizardPageChooseWisely2008.Controls.Add(this.pictureBoxRestoreDatabase);
            this.wizardPageChooseWisely2008.Controls.Add(this.radioChooseRestore2008);
            this.wizardPageChooseWisely2008.Controls.Add(this.labelConnectRunningDatabase);
            this.wizardPageChooseWisely2008.Controls.Add(this.pictureBoxSetupNewDatabase);
            this.wizardPageChooseWisely2008.Controls.Add(this.pictureBoxConnectRunningDatabase);
            this.wizardPageChooseWisely2008.Controls.Add(this.labelSetupNewDatabase);
            this.wizardPageChooseWisely2008.Description = "Setup or connect to a ShipWorks database.";
            this.wizardPageChooseWisely2008.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageChooseWisely2008.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageChooseWisely2008.Location = new System.Drawing.Point(0, 0);
            this.wizardPageChooseWisely2008.Name = "wizardPageChooseWisely2008";
            this.wizardPageChooseWisely2008.Size = new System.Drawing.Size(548, 307);
            this.wizardPageChooseWisely2008.TabIndex = 0;
            this.wizardPageChooseWisely2008.Title = "Database Configuration";
            this.wizardPageChooseWisely2008.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextSetupOrConnect);
            // 
            // radioChooseCreate2008
            // 
            this.radioChooseCreate2008.AutoSize = true;
            this.radioChooseCreate2008.Checked = true;
            this.radioChooseCreate2008.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioChooseCreate2008.Location = new System.Drawing.Point(23, 8);
            this.radioChooseCreate2008.Name = "radioChooseCreate2008";
            this.radioChooseCreate2008.Size = new System.Drawing.Size(196, 17);
            this.radioChooseCreate2008.TabIndex = 10;
            this.radioChooseCreate2008.TabStop = true;
            this.radioChooseCreate2008.Text = "Setup ShipWorks from scratch";
            this.radioChooseCreate2008.UseVisualStyleBackColor = true;
            // 
            // labelRestoreDatabase
            // 
            this.labelRestoreDatabase.ForeColor = System.Drawing.Color.DimGray;
            this.labelRestoreDatabase.Location = new System.Drawing.Point(77, 162);
            this.labelRestoreDatabase.Name = "labelRestoreDatabase";
            this.labelRestoreDatabase.Size = new System.Drawing.Size(406, 11);
            this.labelRestoreDatabase.TabIndex = 16;
            this.labelRestoreDatabase.Text = "Select this option to load a database from a ShipWorks backup.";
            // 
            // radioChooseConnect2008
            // 
            this.radioChooseConnect2008.AutoSize = true;
            this.radioChooseConnect2008.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioChooseConnect2008.Location = new System.Drawing.Point(23, 69);
            this.radioChooseConnect2008.Name = "radioChooseConnect2008";
            this.radioChooseConnect2008.Size = new System.Drawing.Size(231, 17);
            this.radioChooseConnect2008.TabIndex = 12;
            this.radioChooseConnect2008.Text = "Connect to ShipWorks on another PC";
            this.radioChooseConnect2008.UseVisualStyleBackColor = true;
            // 
            // pictureBoxRestoreDatabase
            // 
            this.pictureBoxRestoreDatabase.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxRestoreDatabase.Image")));
            this.pictureBoxRestoreDatabase.Location = new System.Drawing.Point(39, 151);
            this.pictureBoxRestoreDatabase.Name = "pictureBoxRestoreDatabase";
            this.pictureBoxRestoreDatabase.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxRestoreDatabase.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxRestoreDatabase.TabIndex = 18;
            this.pictureBoxRestoreDatabase.TabStop = false;
            // 
            // radioChooseRestore2008
            // 
            this.radioChooseRestore2008.AutoSize = true;
            this.radioChooseRestore2008.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioChooseRestore2008.Location = new System.Drawing.Point(23, 132);
            this.radioChooseRestore2008.Name = "radioChooseRestore2008";
            this.radioChooseRestore2008.Size = new System.Drawing.Size(187, 17);
            this.radioChooseRestore2008.TabIndex = 14;
            this.radioChooseRestore2008.Text = "Restore a ShipWorks backup";
            this.radioChooseRestore2008.UseVisualStyleBackColor = true;
            // 
            // labelConnectRunningDatabase
            // 
            this.labelConnectRunningDatabase.ForeColor = System.Drawing.Color.DimGray;
            this.labelConnectRunningDatabase.Location = new System.Drawing.Point(77, 95);
            this.labelConnectRunningDatabase.Name = "labelConnectRunningDatabase";
            this.labelConnectRunningDatabase.Size = new System.Drawing.Size(406, 32);
            this.labelConnectRunningDatabase.TabIndex = 13;
            this.labelConnectRunningDatabase.Text = "Select this option to connect to a ShipWorks database that is already running on " +
    "this computer or another computer.";
            // 
            // pictureBoxSetupNewDatabase
            // 
            this.pictureBoxSetupNewDatabase.Image = global::ShipWorks.Properties.Resources.box_software1;
            this.pictureBoxSetupNewDatabase.Location = new System.Drawing.Point(39, 27);
            this.pictureBoxSetupNewDatabase.Name = "pictureBoxSetupNewDatabase";
            this.pictureBoxSetupNewDatabase.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxSetupNewDatabase.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxSetupNewDatabase.TabIndex = 15;
            this.pictureBoxSetupNewDatabase.TabStop = false;
            // 
            // pictureBoxConnectRunningDatabase
            // 
            this.pictureBoxConnectRunningDatabase.Image = global::ShipWorks.Properties.Resources.server_to_client;
            this.pictureBoxConnectRunningDatabase.Location = new System.Drawing.Point(39, 88);
            this.pictureBoxConnectRunningDatabase.Name = "pictureBoxConnectRunningDatabase";
            this.pictureBoxConnectRunningDatabase.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxConnectRunningDatabase.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxConnectRunningDatabase.TabIndex = 17;
            this.pictureBoxConnectRunningDatabase.TabStop = false;
            // 
            // labelSetupNewDatabase
            // 
            this.labelSetupNewDatabase.AutoSize = true;
            this.labelSetupNewDatabase.ForeColor = System.Drawing.Color.DimGray;
            this.labelSetupNewDatabase.Location = new System.Drawing.Point(77, 35);
            this.labelSetupNewDatabase.Name = "labelSetupNewDatabase";
            this.labelSetupNewDatabase.Size = new System.Drawing.Size(299, 13);
            this.labelSetupNewDatabase.TabIndex = 11;
            this.labelSetupNewDatabase.Text = "Select this option if this is your first time installing ShipWorks.";
            // 
            // wizardPageManageLocalDb
            // 
            this.wizardPageManageLocalDb.Controls.Add(this.linkAdvancedOptionsLocalDb);
            this.wizardPageManageLocalDb.Controls.Add(this.radioRestoreBackupLocalDb);
            this.wizardPageManageLocalDb.Controls.Add(this.label2);
            this.wizardPageManageLocalDb.Controls.Add(this.linkHelpEnableRemote);
            this.wizardPageManageLocalDb.Controls.Add(this.labelLocalDbEnableRemote2);
            this.wizardPageManageLocalDb.Controls.Add(this.pictureLocalDbEnableRemote);
            this.wizardPageManageLocalDb.Controls.Add(this.radioLocalDbEnableRemote);
            this.wizardPageManageLocalDb.Controls.Add(this.labelLocalDbConnect2);
            this.wizardPageManageLocalDb.Controls.Add(this.pictureLocalDbConnect);
            this.wizardPageManageLocalDb.Controls.Add(this.radioLocalDbConnect);
            this.wizardPageManageLocalDb.Description = "Configure your ShipWorks database.";
            this.wizardPageManageLocalDb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageManageLocalDb.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageManageLocalDb.Location = new System.Drawing.Point(0, 0);
            this.wizardPageManageLocalDb.Name = "wizardPageManageLocalDb";
            this.wizardPageManageLocalDb.Size = new System.Drawing.Size(548, 307);
            this.wizardPageManageLocalDb.TabIndex = 0;
            this.wizardPageManageLocalDb.Title = "Database Configuration";
            this.wizardPageManageLocalDb.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextManageLocalDb);
            // 
            // linkAdvancedOptionsLocalDb
            // 
            this.linkAdvancedOptionsLocalDb.AutoSize = true;
            this.linkAdvancedOptionsLocalDb.Location = new System.Drawing.Point(22, 151);
            this.linkAdvancedOptionsLocalDb.Name = "linkAdvancedOptionsLocalDb";
            this.linkAdvancedOptionsLocalDb.Size = new System.Drawing.Size(121, 13);
            this.linkAdvancedOptionsLocalDb.TabIndex = 56;
            this.linkAdvancedOptionsLocalDb.TabStop = true;
            this.linkAdvancedOptionsLocalDb.Text = "Show advanced options";
            this.linkAdvancedOptionsLocalDb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnManageLocalDbAdvancedOptions);
            // 
            // radioRestoreBackupLocalDb
            // 
            this.radioRestoreBackupLocalDb.AutoSize = true;
            this.radioRestoreBackupLocalDb.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioRestoreBackupLocalDb.Location = new System.Drawing.Point(23, 172);
            this.radioRestoreBackupLocalDb.Name = "radioRestoreBackupLocalDb";
            this.radioRestoreBackupLocalDb.Size = new System.Drawing.Size(210, 17);
            this.radioRestoreBackupLocalDb.TabIndex = 55;
            this.radioRestoreBackupLocalDb.Text = "Restore a ShipWorks database backup";
            this.radioRestoreBackupLocalDb.UseVisualStyleBackColor = true;
            this.radioRestoreBackupLocalDb.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label2.Location = new System.Drawing.Point(81, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(266, 13);
            this.label2.TabIndex = 54;
            this.label2.Text = "But first, make sure it\'s ready for remote connections.";
            // 
            // linkHelpEnableRemote
            // 
            this.linkHelpEnableRemote.AutoSize = true;
            this.linkHelpEnableRemote.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkHelpEnableRemote.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkHelpEnableRemote.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.linkHelpEnableRemote.Location = new System.Drawing.Point(347, 51);
            this.linkHelpEnableRemote.Name = "linkHelpEnableRemote";
            this.linkHelpEnableRemote.Size = new System.Drawing.Size(57, 13);
            this.linkHelpEnableRemote.TabIndex = 53;
            this.linkHelpEnableRemote.Text = "Learn how";
            this.linkHelpEnableRemote.Click += new System.EventHandler(this.OnLinkLearnEnableRemoteConnections);
            // 
            // labelLocalDbEnableRemote2
            // 
            this.labelLocalDbEnableRemote2.Location = new System.Drawing.Point(81, 112);
            this.labelLocalDbEnableRemote2.Name = "labelLocalDbEnableRemote2";
            this.labelLocalDbEnableRemote2.Size = new System.Drawing.Size(406, 17);
            this.labelLocalDbEnableRemote2.TabIndex = 51;
            this.labelLocalDbEnableRemote2.Text = "Need to connect other ShipWorks PCs to this one? This is for you.";
            // 
            // pictureLocalDbEnableRemote
            // 
            this.pictureLocalDbEnableRemote.Image = global::ShipWorks.Properties.Resources.clients;
            this.pictureLocalDbEnableRemote.Location = new System.Drawing.Point(43, 105);
            this.pictureLocalDbEnableRemote.Name = "pictureLocalDbEnableRemote";
            this.pictureLocalDbEnableRemote.Size = new System.Drawing.Size(32, 32);
            this.pictureLocalDbEnableRemote.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureLocalDbEnableRemote.TabIndex = 52;
            this.pictureLocalDbEnableRemote.TabStop = false;
            // 
            // radioLocalDbEnableRemote
            // 
            this.radioLocalDbEnableRemote.AutoSize = true;
            this.radioLocalDbEnableRemote.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioLocalDbEnableRemote.Location = new System.Drawing.Point(25, 83);
            this.radioLocalDbEnableRemote.Name = "radioLocalDbEnableRemote";
            this.radioLocalDbEnableRemote.Size = new System.Drawing.Size(248, 17);
            this.radioLocalDbEnableRemote.TabIndex = 50;
            this.radioLocalDbEnableRemote.Text = "Enable other PC\'s to connect to this one";
            this.radioLocalDbEnableRemote.UseVisualStyleBackColor = true;
            // 
            // labelLocalDbConnect2
            // 
            this.labelLocalDbConnect2.Location = new System.Drawing.Point(81, 33);
            this.labelLocalDbConnect2.Name = "labelLocalDbConnect2";
            this.labelLocalDbConnect2.Size = new System.Drawing.Size(406, 17);
            this.labelLocalDbConnect2.TabIndex = 46;
            this.labelLocalDbConnect2.Text = "Is ShipWorks installed on another PC? Let\'s get you connected.";
            // 
            // pictureLocalDbConnect
            // 
            this.pictureLocalDbConnect.Image = global::ShipWorks.Properties.Resources.server_to_client;
            this.pictureLocalDbConnect.Location = new System.Drawing.Point(45, 32);
            this.pictureLocalDbConnect.Name = "pictureLocalDbConnect";
            this.pictureLocalDbConnect.Size = new System.Drawing.Size(32, 32);
            this.pictureLocalDbConnect.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureLocalDbConnect.TabIndex = 47;
            this.pictureLocalDbConnect.TabStop = false;
            // 
            // radioLocalDbConnect
            // 
            this.radioLocalDbConnect.AutoSize = true;
            this.radioLocalDbConnect.Checked = true;
            this.radioLocalDbConnect.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioLocalDbConnect.Location = new System.Drawing.Point(25, 9);
            this.radioLocalDbConnect.Name = "radioLocalDbConnect";
            this.radioLocalDbConnect.Size = new System.Drawing.Size(277, 17);
            this.radioLocalDbConnect.TabIndex = 45;
            this.radioLocalDbConnect.TabStop = true;
            this.radioLocalDbConnect.Text = "Connect to ShipWorks running on another PC";
            this.radioLocalDbConnect.UseVisualStyleBackColor = true;
            // 
            // wizardPageUpgradeLocalDb
            // 
            this.wizardPageUpgradeLocalDb.Controls.Add(this.panelUpgradeLocalDb);
            this.wizardPageUpgradeLocalDb.Controls.Add(this.panelUpgradeLocalDbReady);
            this.wizardPageUpgradeLocalDb.Description = "Enable ShipWorks for remote connections.";
            this.wizardPageUpgradeLocalDb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageUpgradeLocalDb.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageUpgradeLocalDb.Location = new System.Drawing.Point(0, 0);
            this.wizardPageUpgradeLocalDb.Name = "wizardPageUpgradeLocalDb";
            this.wizardPageUpgradeLocalDb.NextRequiresElevation = true;
            this.wizardPageUpgradeLocalDb.Size = new System.Drawing.Size(548, 307);
            this.wizardPageUpgradeLocalDb.TabIndex = 0;
            this.wizardPageUpgradeLocalDb.Title = "Database Configuration";
            this.wizardPageUpgradeLocalDb.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextUpgradeLocalDb);
            this.wizardPageUpgradeLocalDb.Cancelling += new System.ComponentModel.CancelEventHandler(this.OnCancellUpgradeLocalDb);
            // 
            // panelUpgradeLocalDb
            // 
            this.panelUpgradeLocalDb.Controls.Add(this.progressUpdgradeLocalDb);
            this.panelUpgradeLocalDb.Controls.Add(this.pictureUpgradeLocalDbStatus);
            this.panelUpgradeLocalDb.Controls.Add(this.labelUpgradeLocalDb2);
            this.panelUpgradeLocalDb.Controls.Add(this.pictureUpgrdaeLocalDb);
            this.panelUpgradeLocalDb.Controls.Add(this.labelUpgradeLocalDb);
            this.panelUpgradeLocalDb.Location = new System.Drawing.Point(23, 93);
            this.panelUpgradeLocalDb.Name = "panelUpgradeLocalDb";
            this.panelUpgradeLocalDb.Size = new System.Drawing.Size(478, 80);
            this.panelUpgradeLocalDb.TabIndex = 158;
            this.panelUpgradeLocalDb.Visible = false;
            // 
            // progressUpdgradeLocalDb
            // 
            this.progressUpdgradeLocalDb.Location = new System.Drawing.Point(70, 46);
            this.progressUpdgradeLocalDb.Name = "progressUpdgradeLocalDb";
            this.progressUpdgradeLocalDb.Size = new System.Drawing.Size(379, 23);
            this.progressUpdgradeLocalDb.TabIndex = 52;
            // 
            // pictureUpgradeLocalDbStatus
            // 
            this.pictureUpgradeLocalDbStatus.Image = ((System.Drawing.Image)(resources.GetObject("pictureUpgradeLocalDbStatus.Image")));
            this.pictureUpgradeLocalDbStatus.Location = new System.Drawing.Point(48, 49);
            this.pictureUpgradeLocalDbStatus.Name = "pictureUpgradeLocalDbStatus";
            this.pictureUpgradeLocalDbStatus.Size = new System.Drawing.Size(16, 16);
            this.pictureUpgradeLocalDbStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureUpgradeLocalDbStatus.TabIndex = 51;
            this.pictureUpgradeLocalDbStatus.TabStop = false;
            // 
            // labelUpgradeLocalDb2
            // 
            this.labelUpgradeLocalDb2.AutoSize = true;
            this.labelUpgradeLocalDb2.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelUpgradeLocalDb2.Location = new System.Drawing.Point(46, 28);
            this.labelUpgradeLocalDb2.Name = "labelUpgradeLocalDb2";
            this.labelUpgradeLocalDb2.Size = new System.Drawing.Size(422, 13);
            this.labelUpgradeLocalDb2.TabIndex = 50;
            this.labelUpgradeLocalDb2.Text = "ShipWorks is enabling support for remote connections.  This may take a few minute" +
    "s...";
            // 
            // pictureUpgrdaeLocalDb
            // 
            this.pictureUpgrdaeLocalDb.Image = global::ShipWorks.Properties.Resources.gears_preferences;
            this.pictureUpgrdaeLocalDb.Location = new System.Drawing.Point(9, 10);
            this.pictureUpgrdaeLocalDb.Name = "pictureUpgrdaeLocalDb";
            this.pictureUpgrdaeLocalDb.Size = new System.Drawing.Size(32, 32);
            this.pictureUpgrdaeLocalDb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureUpgrdaeLocalDb.TabIndex = 49;
            this.pictureUpgrdaeLocalDb.TabStop = false;
            // 
            // labelUpgradeLocalDb
            // 
            this.labelUpgradeLocalDb.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUpgradeLocalDb.Location = new System.Drawing.Point(45, 12);
            this.labelUpgradeLocalDb.Name = "labelUpgradeLocalDb";
            this.labelUpgradeLocalDb.Size = new System.Drawing.Size(404, 16);
            this.labelUpgradeLocalDb.TabIndex = 48;
            this.labelUpgradeLocalDb.Text = "Enabling Remote Connections";
            // 
            // panelUpgradeLocalDbReady
            // 
            this.panelUpgradeLocalDbReady.Controls.Add(this.labelUpgradeLocalDbReady);
            this.panelUpgradeLocalDbReady.Location = new System.Drawing.Point(23, 5);
            this.panelUpgradeLocalDbReady.Name = "panelUpgradeLocalDbReady";
            this.panelUpgradeLocalDbReady.Size = new System.Drawing.Size(478, 82);
            this.panelUpgradeLocalDbReady.TabIndex = 157;
            // 
            // labelUpgradeLocalDbReady
            // 
            this.labelUpgradeLocalDbReady.Location = new System.Drawing.Point(8, 12);
            this.labelUpgradeLocalDbReady.Name = "labelUpgradeLocalDbReady";
            this.labelUpgradeLocalDbReady.Size = new System.Drawing.Size(470, 19);
            this.labelUpgradeLocalDbReady.TabIndex = 2;
            this.labelUpgradeLocalDbReady.Text = "ShipWorks is ready to be enabled for remote connections. Click Next to begin.\r\n";
            // 
            // progressLocalDbTimer
            // 
            this.progressLocalDbTimer.Tick += new System.EventHandler(this.OnUpgradeLocalDbProgressTimer);
            // 
            // DetailedDatabaseSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(548, 414);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximumSize = new System.Drawing.Size(564, 452);
            this.MinimumSize = new System.Drawing.Size(564, 452);
            this.Name = "DetailedDatabaseSetupWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageChooseWisely2012,
            this.wizardPageChooseWisely2008,
            this.wizardPageManageLocalDb,
            this.wizardPageUpgradeLocalDb,
            this.wizardPageRestoreOption,
            this.wizardPageChooseSqlServer,
            this.wizardPagePrerequisitePlaceholder,
            this.wizardPageSelectSqlServerInstance,
            this.wizardPageInstallSqlServer,
            this.wizardPageDatabaseName,
            this.wizardPageRestoreLogin,
            this.wizardPageRestoreDatabase,
            this.wizardPageShipWorksAdmin,
            this.wizardPageComplete});
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ShipWorks Setup";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnClosed);
            this.Load += new System.EventHandler(this.OnLoad);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageChooseWisely2012.ResumeLayout(false);
            this.wizardPageChooseWisely2012.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAnotherPC)).EndInit();
            this.wizardPageChooseSqlServer.ResumeLayout(false);
            this.wizardPageChooseSqlServer.PerformLayout();
            this.panelSqlInstanceInstall.ResumeLayout(false);
            this.panelSqlInstanceInstall.PerformLayout();
            this.panelSqlInstanceRunning.ResumeLayout(false);
            this.panelSqlInstanceRunning.PerformLayout();
            this.panelSqlInstanceCurrent.ResumeLayout(false);
            this.panelSqlInstanceCurrent.PerformLayout();
            this.wizardPageSelectSqlServerInstance.ResumeLayout(false);
            this.wizardPageSelectSqlServerInstance.PerformLayout();
            this.panelSearchSqlServers.ResumeLayout(false);
            this.panelSearchSqlServers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchSqlServersPicture)).EndInit();
            this.panelSelectedInstance.ResumeLayout(false);
            this.panelSelectedInstance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSqlConnection)).EndInit();
            this.panelSqlInstanceHelp.ResumeLayout(false);
            this.panelSqlInstanceHelp.PerformLayout();
            this.wizardPageDatabaseName.ResumeLayout(false);
            this.panelDatabaseGivenName.ResumeLayout(false);
            this.panelDatabaseGivenName.PerformLayout();
            this.panelDatabaseChooseName.ResumeLayout(false);
            this.panelDatabaseChooseName.PerformLayout();
            this.panelDataFiles.ResumeLayout(false);
            this.panelDataFiles.PerformLayout();
            this.wizardPageInstallSqlServer.ResumeLayout(false);
            this.panelSqlServerInstallProgress.ResumeLayout(false);
            this.panelSqlServerInstallProgress.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePreparing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreparing)).EndInit();
            this.panelSqlServerInstallReady.ResumeLayout(false);
            this.wizardPageComplete.ResumeLayout(false);
            this.wizardPageComplete.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconSetupComplete)).EndInit();
            this.wizardPageShipWorksAdmin.ResumeLayout(false);
            this.wizardPageShipWorksAdmin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            this.wizardPageRestoreDatabase.ResumeLayout(false);
            this.wizardPageRestoreDatabase.PerformLayout();
            this.groupInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.warningIcon)).EndInit();
            this.wizardPageRestoreOption.ResumeLayout(false);
            this.wizardPageRestoreOption.PerformLayout();
            this.wizardPageRestoreLogin.ResumeLayout(false);
            this.wizardPageRestoreLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headerImage)).EndInit();
            this.wizardPagePrerequisitePlaceholder.ResumeLayout(false);
            this.wizardPagePrerequisitePlaceholder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.wizardPageChooseWisely2008.ResumeLayout(false);
            this.wizardPageChooseWisely2008.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRestoreDatabase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSetupNewDatabase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxConnectRunningDatabase)).EndInit();
            this.wizardPageManageLocalDb.ResumeLayout(false);
            this.wizardPageManageLocalDb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLocalDbEnableRemote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLocalDbConnect)).EndInit();
            this.wizardPageUpgradeLocalDb.ResumeLayout(false);
            this.panelUpgradeLocalDb.ResumeLayout(false);
            this.panelUpgradeLocalDb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureUpgradeLocalDbStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureUpgrdaeLocalDb)).EndInit();
            this.panelUpgradeLocalDbReady.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Wizard.WizardPage wizardPageChooseWisely2012;
        private ShipWorks.UI.Wizard.WizardPage wizardPageChooseSqlServer;
        private System.Windows.Forms.Label labelChooseNewOrExistingSqlServer;
        private System.Windows.Forms.RadioButton radioInstallSqlServer;
        private System.Windows.Forms.TextBox instanceName;
        private System.Windows.Forms.Label labelInstanceName;
        private System.Windows.Forms.RadioButton radioSqlServerCurrent;
        private ShipWorks.UI.Wizard.WizardPage wizardPageSelectSqlServerInstance;
        private ShipWorks.UI.Controls.ImageComboBox comboSqlServers;
        private ShipWorks.UI.Wizard.WizardPage wizardPageDatabaseName;
        private System.Windows.Forms.TextBox databaseName;
        private System.Windows.Forms.Label labelDatabaseName;
        private System.Windows.Forms.Label labelEnterDatabaseName;
        private ShipWorks.UI.Wizard.WizardPage wizardPageInstallSqlServer;
        private System.Windows.Forms.Panel panelSqlServerInstallReady;
        private System.Windows.Forms.Label label21;
        private ShipWorks.UI.Wizard.WizardPage wizardPageComplete;
        private System.Windows.Forms.PictureBox iconSetupComplete;
        private ShipWorks.UI.Wizard.WizardPage wizardPageShipWorksAdmin;
        private System.Windows.Forms.TextBox swEmail;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox swPasswordAgain;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox swPassword;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox swUsername;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private ShipWorks.UI.Wizard.WizardPage wizardPageRestoreDatabase;
        private ShipWorks.UI.Wizard.WizardPage wizardPageRestoreOption;
        private System.Windows.Forms.RadioButton radioRestoreIntoCurrent;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RadioButton radioRestoreIntoNewDatabase;
        private System.Windows.Forms.Label labelRestoreIntoNewDatabase;
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
        private System.Windows.Forms.PictureBox pictureBox6;
        private ShipWorks.UI.Wizard.WizardPage wizardPagePrerequisitePlaceholder;
        private System.Windows.Forms.Label label24;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private UI.Controls.InfoTip infoTip1;
        private UI.Controls.InfoTip helpUserEmail;
        private System.Windows.Forms.Panel panelSqlInstanceCurrent;
        private System.Windows.Forms.Panel panelSqlInstanceRunning;
        private System.Windows.Forms.Label labelSqlServerRunning3;
        private System.Windows.Forms.RadioButton radioSqlServerRunning;
        private System.Windows.Forms.Label labelSqlServerCurrentName;
        private System.Windows.Forms.Panel panelSqlInstanceInstall;
        private UI.Controls.LinkControl linkChooseDataLocation;
        private System.Windows.Forms.Panel panelDataFiles;
        private System.Windows.Forms.Label labelDataFiles;
        private UI.Controls.PathTextBox pathDataFiles;
        private System.Windows.Forms.Button browseDataFiles;
        private System.Windows.Forms.FolderBrowserDialog databaseLocationBrowserDialog;
        private System.Windows.Forms.Panel panelSqlServerInstallProgress;
        private System.Windows.Forms.ProgressBar progressPreparing;
        private System.Windows.Forms.PictureBox picturePreparing;
        private System.Windows.Forms.Label labelPeparingToRun;
        private System.Windows.Forms.PictureBox pictureBoxPreparing;
        private System.Windows.Forms.Label labelPreparing;
        private System.Windows.Forms.Timer progressInstallTimer;
        private UI.Controls.LinkControl linkHelpSelectSqlInstance;
        private System.Windows.Forms.Label labelSelectSqlInstance2;
        private System.Windows.Forms.Label labelSelectSqlInstance;
        private System.Windows.Forms.Label labelSqlConnection;
        private UI.Controls.LinkControl linkSqlInstanceAccount;
        private System.Windows.Forms.Panel panelSqlInstanceHelp;
        private System.Windows.Forms.PictureBox pictureSqlConnection;
        private Divelements.SandGrid.SandGrid gridDatabses;
        private Divelements.SandGrid.GridColumn gridColumnName;
        private Divelements.SandGrid.GridColumn gridColumnStoreType;
        private Divelements.SandGrid.GridColumn gridColumnLatestOrder;
        private System.Windows.Forms.Label labelDatabaseSelect;
        private Divelements.SandGrid.GridColumn gridColumnStatus;
        private UI.Wizard.WizardPage wizardPageChooseWisely2008;
        private UI.Wizard.WizardPage wizardPageManageLocalDb;
        private System.Windows.Forms.Label labelLocalDbEnableRemote2;
        private System.Windows.Forms.PictureBox pictureLocalDbEnableRemote;
        private System.Windows.Forms.RadioButton radioLocalDbEnableRemote;
        private System.Windows.Forms.Label labelLocalDbConnect2;
        private System.Windows.Forms.PictureBox pictureLocalDbConnect;
        private System.Windows.Forms.RadioButton radioLocalDbConnect;
        private UI.Wizard.WizardPage wizardPageUpgradeLocalDb;
        private System.Windows.Forms.Panel panelUpgradeLocalDb;
        private System.Windows.Forms.ProgressBar progressUpdgradeLocalDb;
        private System.Windows.Forms.PictureBox pictureUpgradeLocalDbStatus;
        private System.Windows.Forms.Label labelUpgradeLocalDb2;
        private System.Windows.Forms.PictureBox pictureUpgrdaeLocalDb;
        private System.Windows.Forms.Label labelUpgradeLocalDb;
        private System.Windows.Forms.Panel panelUpgradeLocalDbReady;
        private System.Windows.Forms.Label labelUpgradeLocalDbReady;
        private System.Windows.Forms.Timer progressLocalDbTimer;
        private System.Windows.Forms.Label labelSetupComplete;
        private System.Windows.Forms.Label label2;
        private UI.Controls.LinkControl linkHelpEnableRemote;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabelAdvancedOptions;
        private UI.Controls.LinkControl linkEnableRemoteConnections;
        private System.Windows.Forms.Label labelAnotherPC;
        private System.Windows.Forms.PictureBox pictureBoxAnotherPC;
        private System.Windows.Forms.RadioButton radioChooseRestore2012;
        private System.Windows.Forms.RadioButton radioChooseConnect2012;
        private System.Windows.Forms.RadioButton radioChooseCreate2012;
        private System.Windows.Forms.RadioButton radioChooseCreate2008;
        private System.Windows.Forms.Label labelRestoreDatabase;
        private System.Windows.Forms.RadioButton radioChooseConnect2008;
        private System.Windows.Forms.PictureBox pictureBoxRestoreDatabase;
        private System.Windows.Forms.RadioButton radioChooseRestore2008;
        private System.Windows.Forms.Label labelConnectRunningDatabase;
        private System.Windows.Forms.PictureBox pictureBoxSetupNewDatabase;
        private System.Windows.Forms.PictureBox pictureBoxConnectRunningDatabase;
        private System.Windows.Forms.Label labelSetupNewDatabase;
        private System.Windows.Forms.Panel panelDatabaseChooseName;
        private System.Windows.Forms.Panel panelDatabaseGivenName;
        private System.Windows.Forms.LinkLabel linkEditGivenDatabaseName;
        private System.Windows.Forms.Label givenDatabaseName;
        private System.Windows.Forms.Label labelGivenDatabaseName;
        private System.Windows.Forms.Panel panelSelectedInstance;
        private System.Windows.Forms.PictureBox searchSqlServersPicture;
        private System.Windows.Forms.Label searchSqlServersLabel;
        private System.Windows.Forms.Panel panelSearchSqlServers;
        private System.Windows.Forms.LinkLabel linkAdvancedOptionsLocalDb;
        private System.Windows.Forms.RadioButton radioRestoreBackupLocalDb;
        private System.Windows.Forms.Label forgotPassword;
        private System.Windows.Forms.Label forgotUsername;
    }
}
