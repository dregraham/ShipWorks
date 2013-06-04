namespace ShipWorks.Templates.Controls
{
    partial class TemplateSettingsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TemplateSettingsControl));
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer1 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            Divelements.SandGrid.GridRow gridRow1 = new Divelements.SandGrid.GridRow();
            Divelements.SandGrid.GridRow gridRow2 = new Divelements.SandGrid.GridRow();
            Divelements.SandGrid.GridRow gridRow3 = new Divelements.SandGrid.GridRow();
            this.customSheetMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.manageCustomSheetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createNewSheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editSelectedSheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionControl = new ShipWorks.UI.Controls.OptionControl();
            this.optionPageGeneral = new ShipWorks.UI.Controls.OptionPage();
            this.infotipOutputType = new ShipWorks.UI.Controls.InfoTip();
            this.fileEncoding = new System.Windows.Forms.ComboBox();
            this.labelFileEncoding = new System.Windows.Forms.Label();
            this.xslOutput = new ShipWorks.UI.Controls.ImageComboBox();
            this.templateType = new ShipWorks.Templates.Controls.TemplateTypeComboBox();
            this.labelTemplateType = new System.Windows.Forms.Label();
            this.labelContextInformation = new System.Windows.Forms.Label();
            this.labelTemplateContext = new System.Windows.Forms.Label();
            this.templateContext = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.optionPagePageSetup = new ShipWorks.UI.Controls.OptionPage();
            this.pageSettings = new ShipWorks.Templates.Media.PageSetupControl();
            this.optionPageLabelSetup = new ShipWorks.UI.Controls.OptionPage();
            this.labelSheetControl = new ShipWorks.Templates.Media.LabelSheetControl();
            this.optionPagePrinting = new ShipWorks.UI.Controls.OptionPage();
            this.groupBoxPrinter = new System.Windows.Forms.GroupBox();
            this.printerControl = new ShipWorks.Templates.Media.PrinterSelectionControl();
            this.groupBoxCopies = new System.Windows.Forms.GroupBox();
            this.copiesControl = new ShipWorks.Templates.Media.PageCopiesControl();
            this.optionPageSaving = new ShipWorks.UI.Controls.OptionPage();
            this.groupFileName = new System.Windows.Forms.GroupBox();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.promptAlways = new System.Windows.Forms.RadioButton();
            this.labelPromptWhen = new System.Windows.Forms.Label();
            this.labelPromptAlways = new System.Windows.Forms.Label();
            this.fileFolder = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.promptOnce = new System.Windows.Forms.RadioButton();
            this.fileName = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelPromptOnce = new System.Windows.Forms.Label();
            this.browse = new System.Windows.Forms.Button();
            this.promptNever = new System.Windows.Forms.RadioButton();
            this.labelPromptNever = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelFilename = new System.Windows.Forms.Label();
            this.groupFileNamePrompting = new System.Windows.Forms.GroupBox();
            this.fileWriteBOM = new System.Windows.Forms.CheckBox();
            this.labelFileWriteBOM = new System.Windows.Forms.Label();
            this.fileSaveOnlineImages = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.optionPageEmailing = new ShipWorks.UI.Controls.OptionPage();
            this.emailSingleStoreSettings = new ShipWorks.Templates.Emailing.EmailTemplateStoreSettingsControl();
            this.emailMultiEditSettings = new System.Windows.Forms.Button();
            this.emailMultiStoreLabel = new System.Windows.Forms.Label();
            this.labelEmailAccounts = new System.Windows.Forms.Label();
            this.emailMultiSettingsGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnStore = new Divelements.SandGrid.GridColumn();
            this.gridColumnSource = new Divelements.SandGrid.GridColumn();
            this.gridColumnSettings = new Divelements.SandGrid.GridColumn();
            this.gridColumnValues = new Divelements.SandGrid.GridColumn();
            this.manageEmailAccounts = new System.Windows.Forms.Button();
            this.optionControlFake = new ShipWorks.UI.Controls.OptionControl();
            this.optionPageFake = new ShipWorks.UI.Controls.OptionPage();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.customSheetMenu.SuspendLayout();
            this.optionControl.SuspendLayout();
            this.optionPageGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.templateType)).BeginInit();
            this.optionPagePageSetup.SuspendLayout();
            this.optionPageLabelSetup.SuspendLayout();
            this.optionPagePrinting.SuspendLayout();
            this.groupBoxPrinter.SuspendLayout();
            this.groupBoxCopies.SuspendLayout();
            this.optionPageSaving.SuspendLayout();
            this.groupFileName.SuspendLayout();
            this.groupFileNamePrompting.SuspendLayout();
            this.optionPageEmailing.SuspendLayout();
            this.optionControlFake.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // customSheetMenu
            // 
            this.customSheetMenu.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.customSheetMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manageCustomSheetsToolStripMenuItem,
            this.createNewSheetToolStripMenuItem,
            this.editSelectedSheetToolStripMenuItem});
            this.customSheetMenu.Name = "customSheetMenu";
            this.customSheetMenu.Size = new System.Drawing.Size(196, 70);
            // 
            // manageCustomSheetsToolStripMenuItem
            // 
            this.manageCustomSheetsToolStripMenuItem.Name = "manageCustomSheetsToolStripMenuItem";
            this.manageCustomSheetsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.manageCustomSheetsToolStripMenuItem.Text = "Manage Custom Sheets";
            // 
            // createNewSheetToolStripMenuItem
            // 
            this.createNewSheetToolStripMenuItem.Name = "createNewSheetToolStripMenuItem";
            this.createNewSheetToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.createNewSheetToolStripMenuItem.Text = "Create New Sheet";
            // 
            // editSelectedSheetToolStripMenuItem
            // 
            this.editSelectedSheetToolStripMenuItem.Name = "editSelectedSheetToolStripMenuItem";
            this.editSelectedSheetToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.editSelectedSheetToolStripMenuItem.Text = "Edit Selected Sheet";
            // 
            // optionControl
            // 
            this.optionControl.Controls.Add(this.optionPageGeneral);
            this.optionControl.Controls.Add(this.optionPagePageSetup);
            this.optionControl.Controls.Add(this.optionPageLabelSetup);
            this.optionControl.Controls.Add(this.optionPagePrinting);
            this.optionControl.Controls.Add(this.optionPageSaving);
            this.optionControl.Controls.Add(this.optionPageEmailing);
            this.optionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionControl.Location = new System.Drawing.Point(0, 0);
            this.optionControl.Name = "optionControl";
            this.optionControl.SelectedIndex = 0;
            this.optionControl.Size = new System.Drawing.Size(631, 654);
            this.optionControl.TabIndex = 0;
            this.optionControl.Deselecting += new ShipWorks.UI.Controls.OptionControlCancelEventHandler(this.OnPageDeselecting);
            // 
            // optionPageGeneral
            // 
            this.optionPageGeneral.Controls.Add(this.infotipOutputType);
            this.optionPageGeneral.Controls.Add(this.fileEncoding);
            this.optionPageGeneral.Controls.Add(this.labelFileEncoding);
            this.optionPageGeneral.Controls.Add(this.xslOutput);
            this.optionPageGeneral.Controls.Add(this.templateType);
            this.optionPageGeneral.Controls.Add(this.labelTemplateType);
            this.optionPageGeneral.Controls.Add(this.labelContextInformation);
            this.optionPageGeneral.Controls.Add(this.labelTemplateContext);
            this.optionPageGeneral.Controls.Add(this.templateContext);
            this.optionPageGeneral.Controls.Add(this.label14);
            this.optionPageGeneral.Location = new System.Drawing.Point(153, 0);
            this.optionPageGeneral.Name = "optionPageGeneral";
            this.optionPageGeneral.Size = new System.Drawing.Size(478, 654);
            this.optionPageGeneral.TabIndex = 1;
            this.optionPageGeneral.Text = "General";
            // 
            // infotipOutputType
            // 
            this.infotipOutputType.Caption = "The output type cannot be changed when the template XSL code is invalid.";
            this.infotipOutputType.Location = new System.Drawing.Point(259, 43);
            this.infotipOutputType.Name = "infotipOutputType";
            this.infotipOutputType.Size = new System.Drawing.Size(12, 12);
            this.infotipOutputType.TabIndex = 215;
            this.infotipOutputType.Title = "Output Type";
            // 
            // fileEncoding
            // 
            this.fileEncoding.Location = new System.Drawing.Point(105, 66);
            this.fieldLengthProvider.SetMaxLengthSource(this.fileEncoding, ShipWorks.Data.Utility.EntityFieldLengthSource.TemplateEncoding);
            this.fileEncoding.Name = "fileEncoding";
            this.fileEncoding.Size = new System.Drawing.Size(148, 21);
            this.fileEncoding.TabIndex = 5;
            // 
            // labelFileEncoding
            // 
            this.labelFileEncoding.AutoSize = true;
            this.labelFileEncoding.Location = new System.Drawing.Point(8, 69);
            this.labelFileEncoding.Name = "labelFileEncoding";
            this.labelFileEncoding.Size = new System.Drawing.Size(91, 13);
            this.labelFileEncoding.TabIndex = 4;
            this.labelFileEncoding.Text = "Output encoding:";
            // 
            // xslOutput
            // 
            this.xslOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.xslOutput.FormattingEnabled = true;
            this.xslOutput.Location = new System.Drawing.Point(105, 38);
            this.xslOutput.Name = "xslOutput";
            this.xslOutput.Size = new System.Drawing.Size(148, 21);
            this.xslOutput.TabIndex = 3;
            // 
            // templateType
            // 
            this.templateType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.templateType.FormattingEnabled = true;
            this.templateType.Location = new System.Drawing.Point(105, 10);
            this.templateType.Name = "templateType";
            this.templateType.Size = new System.Drawing.Size(148, 21);
            this.templateType.TabIndex = 1;
            this.templateType.SelectedIndexChanged += new System.EventHandler(this.OnChangeTemplateType);
            // 
            // labelTemplateType
            // 
            this.labelTemplateType.AutoSize = true;
            this.labelTemplateType.Location = new System.Drawing.Point(19, 14);
            this.labelTemplateType.Name = "labelTemplateType";
            this.labelTemplateType.Size = new System.Drawing.Size(80, 13);
            this.labelTemplateType.TabIndex = 0;
            this.labelTemplateType.Text = "Template type:";
            this.labelTemplateType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelContextInformation
            // 
            this.labelContextInformation.ForeColor = System.Drawing.Color.DimGray;
            this.labelContextInformation.Location = new System.Drawing.Point(102, 117);
            this.labelContextInformation.Name = "labelContextInformation";
            this.labelContextInformation.Size = new System.Drawing.Size(294, 69);
            this.labelContextInformation.TabIndex = 8;
            this.labelContextInformation.Text = "An example of what this does: If this was set to\"Shipment\" and you had an order s" +
                "elected in ShipWorks, ShipWorks will process the template as if you had selected" +
                " each individual shipment in the order.";
            // 
            // labelTemplateContext
            // 
            this.labelTemplateContext.AutoSize = true;
            this.labelTemplateContext.Location = new System.Drawing.Point(8, 96);
            this.labelTemplateContext.Name = "labelTemplateContext";
            this.labelTemplateContext.Size = new System.Drawing.Size(91, 13);
            this.labelTemplateContext.TabIndex = 6;
            this.labelTemplateContext.Text = "Process for each:";
            // 
            // templateContext
            // 
            this.templateContext.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.templateContext.Location = new System.Drawing.Point(105, 93);
            this.templateContext.Name = "templateContext";
            this.templateContext.Size = new System.Drawing.Size(148, 21);
            this.templateContext.TabIndex = 7;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(19, 42);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(80, 13);
            this.label14.TabIndex = 2;
            this.label14.Text = "Output format:";
            // 
            // optionPagePageSetup
            // 
            this.optionPagePageSetup.BackColor = System.Drawing.Color.Transparent;
            this.optionPagePageSetup.Controls.Add(this.pageSettings);
            this.optionPagePageSetup.Location = new System.Drawing.Point(153, 0);
            this.optionPagePageSetup.Name = "optionPagePageSetup";
            this.optionPagePageSetup.Padding = new System.Windows.Forms.Padding(3);
            this.optionPagePageSetup.Size = new System.Drawing.Size(478, 654);
            this.optionPagePageSetup.TabIndex = 2;
            this.optionPagePageSetup.Text = "Page Setup";
            // 
            // pageSettings
            // 
            this.pageSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.pageSettings.Location = new System.Drawing.Point(6, 5);
            this.pageSettings.MarginBottom = 0.75D;
            this.pageSettings.MarginLeft = 0.65D;
            this.pageSettings.MarginRight = 0.75D;
            this.pageSettings.MarginTop = 0.65D;
            this.pageSettings.Name = "pageSettings";
            this.pageSettings.PaperHeight = 11D;
            this.pageSettings.PaperWidth = 8.5D;
            this.pageSettings.Size = new System.Drawing.Size(326, 192);
            this.pageSettings.TabIndex = 0;
            // 
            // optionPageLabelSetup
            // 
            this.optionPageLabelSetup.Controls.Add(this.labelSheetControl);
            this.optionPageLabelSetup.Location = new System.Drawing.Point(153, 0);
            this.optionPageLabelSetup.Name = "optionPageLabelSetup";
            this.optionPageLabelSetup.Padding = new System.Windows.Forms.Padding(3);
            this.optionPageLabelSetup.Size = new System.Drawing.Size(478, 654);
            this.optionPageLabelSetup.TabIndex = 3;
            this.optionPageLabelSetup.Text = "Labels";
            // 
            // labelSheetControl
            // 
            this.labelSheetControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelSheetControl.Location = new System.Drawing.Point(6, 5);
            this.labelSheetControl.Name = "labelSheetControl";
            this.labelSheetControl.Size = new System.Drawing.Size(326, 228);
            this.labelSheetControl.TabIndex = 0;
            // 
            // optionPagePrinting
            // 
            this.optionPagePrinting.Controls.Add(this.groupBoxPrinter);
            this.optionPagePrinting.Controls.Add(this.groupBoxCopies);
            this.optionPagePrinting.Location = new System.Drawing.Point(153, 0);
            this.optionPagePrinting.Name = "optionPagePrinting";
            this.optionPagePrinting.Padding = new System.Windows.Forms.Padding(3);
            this.optionPagePrinting.Size = new System.Drawing.Size(478, 654);
            this.optionPagePrinting.TabIndex = 4;
            this.optionPagePrinting.Text = "Printing";
            // 
            // groupBoxPrinter
            // 
            this.groupBoxPrinter.Controls.Add(this.printerControl);
            this.groupBoxPrinter.Location = new System.Drawing.Point(9, 8);
            this.groupBoxPrinter.Name = "groupBoxPrinter";
            this.groupBoxPrinter.Size = new System.Drawing.Size(347, 85);
            this.groupBoxPrinter.TabIndex = 3;
            this.groupBoxPrinter.TabStop = false;
            this.groupBoxPrinter.Text = "Printer";
            // 
            // printerControl
            // 
            this.printerControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.printerControl.Location = new System.Drawing.Point(6, 20);
            this.printerControl.Name = "printerControl";
            this.printerControl.Size = new System.Drawing.Size(328, 59);
            this.printerControl.TabIndex = 0;
            this.printerControl.SizeChanged += new System.EventHandler(this.OnPrinterControlSizeChanged);
            // 
            // groupBoxCopies
            // 
            this.groupBoxCopies.Controls.Add(this.copiesControl);
            this.groupBoxCopies.Location = new System.Drawing.Point(9, 99);
            this.groupBoxCopies.Name = "groupBoxCopies";
            this.groupBoxCopies.Size = new System.Drawing.Size(347, 100);
            this.groupBoxCopies.TabIndex = 2;
            this.groupBoxCopies.TabStop = false;
            this.groupBoxCopies.Text = "Copies";
            // 
            // copiesControl
            // 
            this.copiesControl.Collate = false;
            this.copiesControl.Copies = 1;
            this.copiesControl.Location = new System.Drawing.Point(6, 20);
            this.copiesControl.Name = "copiesControl";
            this.copiesControl.Size = new System.Drawing.Size(177, 78);
            this.copiesControl.TabIndex = 1;
            // 
            // optionPageSaving
            // 
            this.optionPageSaving.Controls.Add(this.groupFileName);
            this.optionPageSaving.Controls.Add(this.groupFileNamePrompting);
            this.optionPageSaving.Location = new System.Drawing.Point(153, 0);
            this.optionPageSaving.Name = "optionPageSaving";
            this.optionPageSaving.Size = new System.Drawing.Size(478, 654);
            this.optionPageSaving.TabIndex = 5;
            this.optionPageSaving.Text = "Saving";
            // 
            // groupFileName
            // 
            this.groupFileName.Controls.Add(this.kryptonBorderEdge);
            this.groupFileName.Controls.Add(this.promptAlways);
            this.groupFileName.Controls.Add(this.labelPromptWhen);
            this.groupFileName.Controls.Add(this.labelPromptAlways);
            this.groupFileName.Controls.Add(this.fileFolder);
            this.groupFileName.Controls.Add(this.promptOnce);
            this.groupFileName.Controls.Add(this.fileName);
            this.groupFileName.Controls.Add(this.labelPromptOnce);
            this.groupFileName.Controls.Add(this.browse);
            this.groupFileName.Controls.Add(this.promptNever);
            this.groupFileName.Controls.Add(this.labelPromptNever);
            this.groupFileName.Controls.Add(this.label2);
            this.groupFileName.Controls.Add(this.labelFilename);
            this.groupFileName.Location = new System.Drawing.Point(9, 8);
            this.groupFileName.Name = "groupFileName";
            this.groupFileName.Size = new System.Drawing.Size(386, 301);
            this.groupFileName.TabIndex = 0;
            this.groupFileName.TabStop = false;
            this.groupFileName.Text = "File Name";
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonBorderEdge.AutoSize = false;
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.ControlRibbon;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(9, 106);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.kryptonBorderEdge.Size = new System.Drawing.Size(368, 1);
            this.kryptonBorderEdge.TabIndex = 221;
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            // 
            // promptAlways
            // 
            this.promptAlways.AutoSize = true;
            this.promptAlways.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.promptAlways.Location = new System.Drawing.Point(24, 246);
            this.promptAlways.Name = "promptAlways";
            this.promptAlways.Size = new System.Drawing.Size(59, 17);
            this.promptAlways.TabIndex = 232;
            this.promptAlways.TabStop = true;
            this.promptAlways.Text = "Always";
            this.promptAlways.UseVisualStyleBackColor = true;
            // 
            // labelPromptWhen
            // 
            this.labelPromptWhen.AutoSize = true;
            this.labelPromptWhen.Location = new System.Drawing.Point(12, 117);
            this.labelPromptWhen.Name = "labelPromptWhen";
            this.labelPromptWhen.Size = new System.Drawing.Size(171, 13);
            this.labelPromptWhen.TabIndex = 222;
            this.labelPromptWhen.Text = "Prompt me for the name or folder:";
            // 
            // labelPromptAlways
            // 
            this.labelPromptAlways.ForeColor = System.Drawing.Color.Gray;
            this.labelPromptAlways.Location = new System.Drawing.Point(41, 264);
            this.labelPromptAlways.Name = "labelPromptAlways";
            this.labelPromptAlways.Size = new System.Drawing.Size(336, 31);
            this.labelPromptAlways.TabIndex = 231;
            this.labelPromptAlways.Text = "You will be prompted to save each file.  The configured file name and folder will" +
                " be the initial selection.";
            // 
            // fileFolder
            // 
            this.fileFolder.Location = new System.Drawing.Point(60, 47);
            this.fileFolder.MaxLength = 32767;
            this.fieldLengthProvider.SetMaxLengthSource(this.fileFolder, ShipWorks.Data.Utility.EntityFieldLengthSource.TemplateSaveFolder);
            this.fileFolder.Name = "fileFolder";
            this.fileFolder.Size = new System.Drawing.Size(317, 21);
            this.fileFolder.TabIndex = 221;
            this.fileFolder.TokenUsage = ShipWorks.Templates.Tokens.TokenUsage.FileName;
            // 
            // promptOnce
            // 
            this.promptOnce.AutoSize = true;
            this.promptOnce.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.promptOnce.Location = new System.Drawing.Point(24, 171);
            this.promptOnce.Name = "promptOnce";
            this.promptOnce.Size = new System.Drawing.Size(50, 17);
            this.promptOnce.TabIndex = 230;
            this.promptOnce.TabStop = true;
            this.promptOnce.Text = "Once";
            this.promptOnce.UseVisualStyleBackColor = true;
            // 
            // fileName
            // 
            this.fileName.Location = new System.Drawing.Point(60, 20);
            this.fileName.MaxLength = 32767;
            this.fieldLengthProvider.SetMaxLengthSource(this.fileName, ShipWorks.Data.Utility.EntityFieldLengthSource.TemplateSaveFile);
            this.fileName.Name = "fileName";
            this.fileName.Size = new System.Drawing.Size(317, 21);
            this.fileName.TabIndex = 220;
            this.fileName.TokenUsage = ShipWorks.Templates.Tokens.TokenUsage.FileName;
            // 
            // labelPromptOnce
            // 
            this.labelPromptOnce.ForeColor = System.Drawing.Color.Gray;
            this.labelPromptOnce.Location = new System.Drawing.Point(41, 189);
            this.labelPromptOnce.Name = "labelPromptOnce";
            this.labelPromptOnce.Size = new System.Drawing.Size(336, 58);
            this.labelPromptOnce.TabIndex = 229;
            this.labelPromptOnce.Text = resources.GetString("labelPromptOnce.Text");
            // 
            // browse
            // 
            this.browse.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browse.Location = new System.Drawing.Point(302, 74);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(75, 23);
            this.browse.TabIndex = 220;
            this.browse.Text = "Browse...";
            this.browse.Click += new System.EventHandler(this.OnBrowseFileSaveFolder);
            // 
            // promptNever
            // 
            this.promptNever.AutoSize = true;
            this.promptNever.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.promptNever.Location = new System.Drawing.Point(24, 135);
            this.promptNever.Name = "promptNever";
            this.promptNever.Size = new System.Drawing.Size(54, 17);
            this.promptNever.TabIndex = 228;
            this.promptNever.TabStop = true;
            this.promptNever.Text = "Never";
            this.promptNever.UseVisualStyleBackColor = true;
            // 
            // labelPromptNever
            // 
            this.labelPromptNever.ForeColor = System.Drawing.Color.Gray;
            this.labelPromptNever.Location = new System.Drawing.Point(41, 153);
            this.labelPromptNever.Name = "labelPromptNever";
            this.labelPromptNever.Size = new System.Drawing.Size(336, 19);
            this.labelPromptNever.TabIndex = 227;
            this.labelPromptNever.Text = "Each file will be saved using the configured file name and folder.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 205;
            this.label2.Text = "Folder:";
            // 
            // labelFilename
            // 
            this.labelFilename.AutoSize = true;
            this.labelFilename.Location = new System.Drawing.Point(16, 23);
            this.labelFilename.Name = "labelFilename";
            this.labelFilename.Size = new System.Drawing.Size(38, 13);
            this.labelFilename.TabIndex = 203;
            this.labelFilename.Text = "Name:";
            // 
            // groupFileNamePrompting
            // 
            this.groupFileNamePrompting.Controls.Add(this.fileWriteBOM);
            this.groupFileNamePrompting.Controls.Add(this.labelFileWriteBOM);
            this.groupFileNamePrompting.Controls.Add(this.fileSaveOnlineImages);
            this.groupFileNamePrompting.Controls.Add(this.label1);
            this.groupFileNamePrompting.Location = new System.Drawing.Point(9, 318);
            this.groupFileNamePrompting.Name = "groupFileNamePrompting";
            this.groupFileNamePrompting.Size = new System.Drawing.Size(386, 72);
            this.groupFileNamePrompting.TabIndex = 1;
            this.groupFileNamePrompting.TabStop = false;
            this.groupFileNamePrompting.Text = "Options";
            // 
            // fileWriteBOM
            // 
            this.fileWriteBOM.AutoSize = true;
            this.fileWriteBOM.Location = new System.Drawing.Point(76, 43);
            this.fileWriteBOM.Name = "fileWriteBOM";
            this.fileWriteBOM.Size = new System.Drawing.Size(273, 17);
            this.fileWriteBOM.TabIndex = 1;
            this.fileWriteBOM.Text = "Output the byte-order mark for Unicode encodings.";
            // 
            // labelFileWriteBOM
            // 
            this.labelFileWriteBOM.AutoSize = true;
            this.labelFileWriteBOM.Location = new System.Drawing.Point(16, 44);
            this.labelFileWriteBOM.Name = "labelFileWriteBOM";
            this.labelFileWriteBOM.Size = new System.Drawing.Size(54, 13);
            this.labelFileWriteBOM.TabIndex = 213;
            this.labelFileWriteBOM.Text = "Encoding:";
            // 
            // fileSaveOnlineImages
            // 
            this.fileSaveOnlineImages.AutoSize = true;
            this.fileSaveOnlineImages.Location = new System.Drawing.Point(76, 22);
            this.fileSaveOnlineImages.Name = "fileSaveOnlineImages";
            this.fileSaveOnlineImages.Size = new System.Drawing.Size(191, 17);
            this.fileSaveOnlineImages.TabIndex = 0;
            this.fileSaveOnlineImages.Text = "Download and save online images.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 212;
            this.label1.Text = "HTML files:";
            // 
            // optionPageEmailing
            // 
            this.optionPageEmailing.AutoScrollMargin = new System.Drawing.Size(0, 8);
            this.optionPageEmailing.Controls.Add(this.emailSingleStoreSettings);
            this.optionPageEmailing.Controls.Add(this.emailMultiEditSettings);
            this.optionPageEmailing.Controls.Add(this.emailMultiStoreLabel);
            this.optionPageEmailing.Controls.Add(this.labelEmailAccounts);
            this.optionPageEmailing.Controls.Add(this.emailMultiSettingsGrid);
            this.optionPageEmailing.Controls.Add(this.manageEmailAccounts);
            this.optionPageEmailing.Location = new System.Drawing.Point(153, 0);
            this.optionPageEmailing.Name = "optionPageEmailing";
            this.optionPageEmailing.Size = new System.Drawing.Size(478, 654);
            this.optionPageEmailing.TabIndex = 6;
            this.optionPageEmailing.Text = "Email";
            // 
            // emailSingleStoreSettings
            // 
            this.emailSingleStoreSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.emailSingleStoreSettings.Location = new System.Drawing.Point(15, 310);
            this.emailSingleStoreSettings.Name = "emailSingleStoreSettings";
            this.emailSingleStoreSettings.Size = new System.Drawing.Size(361, 135);
            this.emailSingleStoreSettings.TabIndex = 10;
            // 
            // emailMultiEditSettings
            // 
            this.emailMultiEditSettings.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.emailMultiEditSettings.Location = new System.Drawing.Point(347, 210);
            this.emailMultiEditSettings.Name = "emailMultiEditSettings";
            this.emailMultiEditSettings.Size = new System.Drawing.Size(117, 23);
            this.emailMultiEditSettings.TabIndex = 12;
            this.emailMultiEditSettings.Text = "Edit Settings...";
            this.emailMultiEditSettings.UseVisualStyleBackColor = true;
            this.emailMultiEditSettings.Click += new System.EventHandler(this.OnEmailEditSettings);
            // 
            // emailMultiStoreLabel
            // 
            this.emailMultiStoreLabel.AutoSize = true;
            this.emailMultiStoreLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.emailMultiStoreLabel.Location = new System.Drawing.Point(12, 12);
            this.emailMultiStoreLabel.Name = "emailMultiStoreLabel";
            this.emailMultiStoreLabel.Size = new System.Drawing.Size(118, 13);
            this.emailMultiStoreLabel.TabIndex = 10;
            this.emailMultiStoreLabel.Text = "Stores and Settings";
            // 
            // labelEmailAccounts
            // 
            this.labelEmailAccounts.AutoSize = true;
            this.labelEmailAccounts.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelEmailAccounts.Location = new System.Drawing.Point(12, 244);
            this.labelEmailAccounts.Name = "labelEmailAccounts";
            this.labelEmailAccounts.Size = new System.Drawing.Size(92, 13);
            this.labelEmailAccounts.TabIndex = 9;
            this.labelEmailAccounts.Text = "Email Accounts";
            this.labelEmailAccounts.Visible = false;
            // 
            // emailMultiSettingsGrid
            // 
            this.emailMultiSettingsGrid.AllowMultipleSelection = false;
            this.emailMultiSettingsGrid.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.emailMultiSettingsGrid.ColumnClickBehavior = Divelements.SandGrid.ColumnClickBehavior.None;
            this.emailMultiSettingsGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnStore,
            this.gridColumnSource,
            this.gridColumnSettings,
            this.gridColumnValues});
            this.emailMultiSettingsGrid.EnableSearching = false;
            this.emailMultiSettingsGrid.GridLines = Divelements.SandGrid.GridLinesDisplayType.HorizontalOnly;
            this.emailMultiSettingsGrid.ImageTextSeparation = 1;
            this.emailMultiSettingsGrid.Location = new System.Drawing.Point(15, 30);
            this.emailMultiSettingsGrid.Name = "emailMultiSettingsGrid";
            this.emailMultiSettingsGrid.Renderer = windowsXPRenderer1;
            this.emailMultiSettingsGrid.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.InitiateDragDrop;
            gridRow1.Cells.AddRange(new Divelements.SandGrid.GridCell[] {
            new Divelements.SandGrid.GridCell("Baggins Store Bakala"),
            new Divelements.SandGrid.GridCell("Shared"),
            new Divelements.SandGrid.GridCell("Account:\r\nTo:\r\nSubject:"),
            new Divelements.SandGrid.GridCell("Default\r\n{Order/Email/Derka}\r\nStonker Ponker")});
            gridRow1.Height = 0;
            gridRow2.Cells.AddRange(new Divelements.SandGrid.GridCell[] {
            new Divelements.SandGrid.GridCell("Stanky Cockerton"),
            new Divelements.SandGrid.GridCell("Unique"),
            new Divelements.SandGrid.GridCell("Account:\r\nTo:\r\nBcc:\r\nSubject:"),
            new Divelements.SandGrid.GridCell("Whatever\r\nSteve\r\nMe\r\nEat me")});
            gridRow2.Height = 0;
            gridRow3.Cells.AddRange(new Divelements.SandGrid.GridCell[] {
            new Divelements.SandGrid.GridCell("Derka Libberton"),
            new Divelements.SandGrid.GridCell("Shared"),
            new Divelements.SandGrid.GridCell("Account:\r\nTo:\r\nSubject:"),
            new Divelements.SandGrid.GridCell("Default\r\n{Order/Email/Derka}\r\nStonker Ponker")});
            gridRow3.Height = 0;
            this.emailMultiSettingsGrid.Rows.AddRange(new Divelements.SandGrid.GridRow[] {
            gridRow1,
            gridRow2,
            gridRow3});
            this.emailMultiSettingsGrid.ShadeAlternateRows = true;
            this.emailMultiSettingsGrid.Size = new System.Drawing.Size(449, 174);
            this.emailMultiSettingsGrid.StretchPrimaryGrid = false;
            this.emailMultiSettingsGrid.TabIndex = 3;
            this.emailMultiSettingsGrid.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnEmailSettingsGridSelectionChanged);
            this.emailMultiSettingsGrid.RowActivated += new Divelements.SandGrid.GridRowEventHandler(this.OnEmailSettingsGridRowActivated);
            // 
            // gridColumnStore
            // 
            this.gridColumnStore.AllowReorder = false;
            this.gridColumnStore.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnStore.CellVerticalAlignment = System.Drawing.StringAlignment.Near;
            this.gridColumnStore.Clickable = false;
            this.gridColumnStore.HeaderText = "Store";
            this.gridColumnStore.MinimumWidth = 50;
            this.gridColumnStore.Width = 108;
            // 
            // gridColumnSource
            // 
            this.gridColumnSource.AllowReorder = false;
            this.gridColumnSource.CellVerticalAlignment = System.Drawing.StringAlignment.Near;
            this.gridColumnSource.Clickable = false;
            this.gridColumnSource.HeaderText = "Source";
            this.gridColumnSource.Width = 43;
            // 
            // gridColumnSettings
            // 
            this.gridColumnSettings.AllowReorder = false;
            this.gridColumnSettings.AllowWrap = true;
            this.gridColumnSettings.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnSettings.CellHorizontalAlignment = System.Drawing.StringAlignment.Far;
            this.gridColumnSettings.CellVerticalAlignment = System.Drawing.StringAlignment.Near;
            this.gridColumnSettings.Clickable = false;
            this.gridColumnSettings.HeaderText = "Settings";
            this.gridColumnSettings.MinimumWidth = 20;
            this.gridColumnSettings.Width = 51;
            // 
            // gridColumnValues
            // 
            this.gridColumnValues.AllowReorder = false;
            this.gridColumnValues.AllowWrap = true;
            this.gridColumnValues.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnValues.CellVerticalAlignment = System.Drawing.StringAlignment.Near;
            this.gridColumnValues.Clickable = false;
            this.gridColumnValues.Width = 243;
            // 
            // manageEmailAccounts
            // 
            this.manageEmailAccounts.Location = new System.Drawing.Point(15, 262);
            this.manageEmailAccounts.Name = "manageEmailAccounts";
            this.manageEmailAccounts.Size = new System.Drawing.Size(143, 23);
            this.manageEmailAccounts.TabIndex = 2;
            this.manageEmailAccounts.Text = "Manage Email Accounts...";
            this.manageEmailAccounts.UseVisualStyleBackColor = true;
            this.manageEmailAccounts.Visible = false;
            this.manageEmailAccounts.Click += new System.EventHandler(this.OnManageEmailAccounts);
            // 
            // optionControlFake
            // 
            this.optionControlFake.Controls.Add(this.optionPageFake);
            this.optionControlFake.Location = new System.Drawing.Point(67, 400);
            this.optionControlFake.Name = "optionControlFake";
            this.optionControlFake.SelectedIndex = 0;
            this.optionControlFake.Size = new System.Drawing.Size(298, 69);
            this.optionControlFake.TabIndex = 1;
            this.optionControlFake.Text = "optionControl1";
            this.optionControlFake.Visible = false;
            // 
            // optionPageFake
            // 
            this.optionPageFake.Location = new System.Drawing.Point(153, 0);
            this.optionPageFake.Name = "optionPageFake";
            this.optionPageFake.Padding = new System.Windows.Forms.Padding(3);
            this.optionPageFake.Size = new System.Drawing.Size(145, 69);
            this.optionPageFake.TabIndex = 1;
            this.optionPageFake.Text = "optionPage1";
            // 
            // TemplateSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.optionControl);
            this.Controls.Add(this.optionControlFake);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "TemplateSettingsControl";
            this.Size = new System.Drawing.Size(631, 654);
            this.customSheetMenu.ResumeLayout(false);
            this.optionControl.ResumeLayout(false);
            this.optionPageGeneral.ResumeLayout(false);
            this.optionPageGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.templateType)).EndInit();
            this.optionPagePageSetup.ResumeLayout(false);
            this.optionPageLabelSetup.ResumeLayout(false);
            this.optionPagePrinting.ResumeLayout(false);
            this.groupBoxPrinter.ResumeLayout(false);
            this.groupBoxCopies.ResumeLayout(false);
            this.optionPageSaving.ResumeLayout(false);
            this.groupFileName.ResumeLayout(false);
            this.groupFileName.PerformLayout();
            this.groupFileNamePrompting.ResumeLayout(false);
            this.groupFileNamePrompting.PerformLayout();
            this.optionPageEmailing.ResumeLayout(false);
            this.optionPageEmailing.PerformLayout();
            this.optionControlFake.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Controls.OptionControl optionControl;
        private ShipWorks.UI.Controls.OptionPage optionPageGeneral;
        private ShipWorks.UI.Controls.OptionPage optionPagePageSetup;
        private ShipWorks.UI.Controls.OptionPage optionPageLabelSetup;
        private ShipWorks.UI.Controls.OptionPage optionPagePrinting;
        private ShipWorks.UI.Controls.OptionPage optionPageSaving;
        private ShipWorks.UI.Controls.OptionPage optionPageEmailing;
        private System.Windows.Forms.Label labelTemplateContext;
        private System.Windows.Forms.ComboBox templateContext;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label labelFilename;
        private System.Windows.Forms.CheckBox fileSaveOnlineImages;
        private System.Windows.Forms.Label labelContextInformation;
        private TemplateTypeComboBox templateType;
        private System.Windows.Forms.Label labelTemplateType;
        private System.Windows.Forms.ContextMenuStrip customSheetMenu;
        private System.Windows.Forms.ToolStripMenuItem manageCustomSheetsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createNewSheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editSelectedSheetToolStripMenuItem;
        private ShipWorks.UI.Controls.ImageComboBox xslOutput;
        private ShipWorks.Templates.Media.PrinterSelectionControl printerControl;
        private ShipWorks.Templates.Media.PageSetupControl pageSettings;
        private ShipWorks.Templates.Media.LabelSheetControl labelSheetControl;
        private System.Windows.Forms.GroupBox groupBoxPrinter;
        private System.Windows.Forms.GroupBox groupBoxCopies;
        private ShipWorks.Templates.Media.PageCopiesControl copiesControl;
        private System.Windows.Forms.GroupBox groupFileNamePrompting;
        private System.Windows.Forms.GroupBox groupFileName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button browse;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox fileName;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox fileFolder;
        private System.Windows.Forms.RadioButton promptAlways;
        private System.Windows.Forms.Label labelPromptAlways;
        private System.Windows.Forms.RadioButton promptOnce;
        private System.Windows.Forms.Label labelPromptOnce;
        private System.Windows.Forms.RadioButton promptNever;
        private System.Windows.Forms.Label labelPromptNever;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        private System.Windows.Forms.Label labelPromptWhen;
        private System.Windows.Forms.ComboBox fileEncoding;
        private System.Windows.Forms.Label labelFileEncoding;
        private System.Windows.Forms.Label labelFileWriteBOM;
        private System.Windows.Forms.CheckBox fileWriteBOM;
        private System.Windows.Forms.Button manageEmailAccounts;
        private ShipWorks.UI.Controls.OptionControl optionControlFake;
        private ShipWorks.UI.Controls.OptionPage optionPageFake;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private UI.Controls.InfoTip infotipOutputType;
        private Divelements.SandGrid.SandGrid emailMultiSettingsGrid;
        private Divelements.SandGrid.GridColumn gridColumnStore;
        private Divelements.SandGrid.GridColumn gridColumnSource;
        private Divelements.SandGrid.GridColumn gridColumnValues;
        private System.Windows.Forms.Label labelEmailAccounts;
        private Divelements.SandGrid.GridColumn gridColumnSettings;
        private System.Windows.Forms.Label emailMultiStoreLabel;
        private System.Windows.Forms.Button emailMultiEditSettings;
        private Emailing.EmailTemplateStoreSettingsControl emailSingleStoreSettings;


    }
}
