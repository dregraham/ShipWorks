namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration
{
    partial class ImportTemplatesWizardPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportTemplatesWizardPage));
            this.importTemplatesBackupFile = new ShipWorks.UI.Controls.PathTextBox();
            this.browseBackupFile = new System.Windows.Forms.Button();
            this.importTemplatesAppDataFolder = new ShipWorks.UI.Controls.PathTextBox();
            this.browseAppData = new System.Windows.Forms.Button();
            this.radioImportTemplatesBackupFile = new System.Windows.Forms.RadioButton();
            this.radioImportTemplatesAppData = new System.Windows.Forms.RadioButton();
            this.label20 = new System.Windows.Forms.Label();
            this.openAppDataFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.openBackupFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.radioDontImport = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // importTemplatesBackupFile
            // 
            this.importTemplatesBackupFile.Location = new System.Drawing.Point(45, 169);
            this.importTemplatesBackupFile.Name = "importTemplatesBackupFile";
            this.importTemplatesBackupFile.Size = new System.Drawing.Size(362, 21);
            this.importTemplatesBackupFile.TabIndex = 13;
            // 
            // browseBackupFile
            // 
            this.browseBackupFile.Location = new System.Drawing.Point(411, 167);
            this.browseBackupFile.Name = "browseBackupFile";
            this.browseBackupFile.Size = new System.Drawing.Size(75, 23);
            this.browseBackupFile.TabIndex = 14;
            this.browseBackupFile.Text = "Browse...";
            this.browseBackupFile.Click += new System.EventHandler(this.OnBrowseBackupFile);
            // 
            // importTemplatesAppDataFolder
            // 
            this.importTemplatesAppDataFolder.Location = new System.Drawing.Point(45, 106);
            this.importTemplatesAppDataFolder.Name = "importTemplatesAppDataFolder";
            this.importTemplatesAppDataFolder.Size = new System.Drawing.Size(362, 21);
            this.importTemplatesAppDataFolder.TabIndex = 10;
            // 
            // browseAppData
            // 
            this.browseAppData.Location = new System.Drawing.Point(411, 104);
            this.browseAppData.Name = "browseAppData";
            this.browseAppData.Size = new System.Drawing.Size(75, 23);
            this.browseAppData.TabIndex = 11;
            this.browseAppData.Text = "Browse...";
            this.browseAppData.Click += new System.EventHandler(this.OnBrowseApplicationData);
            // 
            // radioImportTemplatesBackupFile
            // 
            this.radioImportTemplatesBackupFile.AutoSize = true;
            this.radioImportTemplatesBackupFile.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.radioImportTemplatesBackupFile.Location = new System.Drawing.Point(27, 146);
            this.radioImportTemplatesBackupFile.Name = "radioImportTemplatesBackupFile";
            this.radioImportTemplatesBackupFile.Size = new System.Drawing.Size(212, 17);
            this.radioImportTemplatesBackupFile.TabIndex = 12;
            this.radioImportTemplatesBackupFile.TabStop = true;
            this.radioImportTemplatesBackupFile.Text = "Import from a ShipWorks Backup";
            this.radioImportTemplatesBackupFile.UseVisualStyleBackColor = true;
            this.radioImportTemplatesBackupFile.CheckedChanged += new System.EventHandler(this.OnChangeImportTemplateMethod);
            // 
            // radioImportTemplatesAppData
            // 
            this.radioImportTemplatesAppData.AutoSize = true;
            this.radioImportTemplatesAppData.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.radioImportTemplatesAppData.Location = new System.Drawing.Point(27, 86);
            this.radioImportTemplatesAppData.Name = "radioImportTemplatesAppData";
            this.radioImportTemplatesAppData.Size = new System.Drawing.Size(229, 17);
            this.radioImportTemplatesAppData.TabIndex = 9;
            this.radioImportTemplatesAppData.TabStop = true;
            this.radioImportTemplatesAppData.Text = "Import from Application Data Folder";
            this.radioImportTemplatesAppData.UseVisualStyleBackColor = true;
            this.radioImportTemplatesAppData.CheckedChanged += new System.EventHandler(this.OnChangeImportTemplateMethod);
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(77, 10);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(371, 72);
            this.label20.TabIndex = 8;
            this.label20.Text = resources.GetString("label20.Text");
            // 
            // openAppDataFolder
            // 
            this.openAppDataFolder.Description = "Select Application Data Folder";
            // 
            // openBackupFileDialog
            // 
            this.openBackupFileDialog.DefaultExt = "swb";
            this.openBackupFileDialog.Filter = "ShipWorks Backup Files (*.swb)|*.swb";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.text_code_add;
            this.pictureBox1.Location = new System.Drawing.Point(24, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // radioDontImport
            // 
            this.radioDontImport.AutoSize = true;
            this.radioDontImport.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.radioDontImport.Location = new System.Drawing.Point(27, 202);
            this.radioDontImport.Name = "radioDontImport";
            this.radioDontImport.Size = new System.Drawing.Size(267, 17);
            this.radioDontImport.TabIndex = 16;
            this.radioDontImport.TabStop = true;
            this.radioDontImport.Text = "Do not import templates from ShipWorks 2";
            this.radioDontImport.UseVisualStyleBackColor = true;
            // 
            // ImportTemplatesWizardPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radioDontImport);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.importTemplatesBackupFile);
            this.Controls.Add(this.browseBackupFile);
            this.Controls.Add(this.importTemplatesAppDataFolder);
            this.Controls.Add(this.browseAppData);
            this.Controls.Add(this.radioImportTemplatesBackupFile);
            this.Controls.Add(this.radioImportTemplatesAppData);
            this.Controls.Add(this.label20);
            this.Description = "Import templates from ShipWorks 2.";
            this.Name = "ImportTemplatesWizardPage";
            this.Size = new System.Drawing.Size(529, 240);
            this.Title = "Import Templates";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Controls.PathTextBox importTemplatesBackupFile;
        private System.Windows.Forms.Button browseBackupFile;
        private UI.Controls.PathTextBox importTemplatesAppDataFolder;
        private System.Windows.Forms.Button browseAppData;
        private System.Windows.Forms.RadioButton radioImportTemplatesBackupFile;
        private System.Windows.Forms.RadioButton radioImportTemplatesAppData;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.FolderBrowserDialog openAppDataFolder;
        private System.Windows.Forms.OpenFileDialog openBackupFileDialog;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RadioButton radioDontImport;
    }
}
