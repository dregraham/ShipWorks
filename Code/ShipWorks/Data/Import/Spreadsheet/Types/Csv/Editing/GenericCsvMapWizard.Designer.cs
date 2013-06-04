namespace ShipWorks.Data.Import.Spreadsheet.Types.Csv.Editing
{
    partial class GenericCsvMapWizard
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
            this.wizardPageSampleFile = new ShipWorks.UI.Wizard.WizardPage();
            this.csvSchemaControl = new ShipWorks.Data.Import.Spreadsheet.Types.Csv.Editing.GenericCsvSourceSchemaControl();
            this.wizardPageMapSettings = new ShipWorks.UI.Wizard.WizardPage();
            this.csvMapEditor = new ShipWorks.Data.Import.Spreadsheet.Types.Csv.Editing.GenericCsvMapEditorControl();
            this.wizardPageSuccess = new ShipWorks.UI.Wizard.WizardPage();
            this.iconSetupComplete = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageSampleFile.SuspendLayout();
            this.wizardPageMapSettings.SuspendLayout();
            this.wizardPageSuccess.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.iconSetupComplete)).BeginInit();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(382, 483);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(463, 483);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(301, 483);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageSampleFile);
            this.mainPanel.Size = new System.Drawing.Size(550, 411);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 473);
            this.etchBottom.Size = new System.Drawing.Size(554, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.table_sql_add;
            this.pictureBox.Location = new System.Drawing.Point(497, 3);
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(550, 56);
            // 
            // wizardPageSampleFile
            // 
            this.wizardPageSampleFile.Controls.Add(this.csvSchemaControl);
            this.wizardPageSampleFile.Description = "Select a sample of your CSV \\ Text formatted data files.";
            this.wizardPageSampleFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageSampleFile.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageSampleFile.Location = new System.Drawing.Point(0, 0);
            this.wizardPageSampleFile.Name = "wizardPageSampleFile";
            this.wizardPageSampleFile.Size = new System.Drawing.Size(550, 411);
            this.wizardPageSampleFile.TabIndex = 0;
            this.wizardPageSampleFile.Title = "CSV \\ Text Format";
            this.wizardPageSampleFile.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextSampleMap);
            // 
            // csvSchemaControl
            // 
            this.csvSchemaControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.csvSchemaControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.csvSchemaControl.Location = new System.Drawing.Point(21, 7);
            this.csvSchemaControl.Name = "csvSchemaControl";
            this.csvSchemaControl.Size = new System.Drawing.Size(502, 401);
            this.csvSchemaControl.TabIndex = 15;
            // 
            // wizardPageMapSettings
            // 
            this.wizardPageMapSettings.Controls.Add(this.csvMapEditor);
            this.wizardPageMapSettings.Description = "Setup the mappings of your columns into ShipWorks.";
            this.wizardPageMapSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageMapSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageMapSettings.Location = new System.Drawing.Point(0, 0);
            this.wizardPageMapSettings.Name = "wizardPageMapSettings";
            this.wizardPageMapSettings.Size = new System.Drawing.Size(550, 411);
            this.wizardPageMapSettings.TabIndex = 0;
            this.wizardPageMapSettings.Title = "Column Mappings";
            this.wizardPageMapSettings.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextSettingsPage);
            // 
            // csvMapEditor
            // 
            this.csvMapEditor.AllowChangeSourceColumns = false;
            this.csvMapEditor.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.csvMapEditor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.csvMapEditor.Location = new System.Drawing.Point(23, 5);
            this.csvMapEditor.Name = "csvMapEditor";
            this.csvMapEditor.Size = new System.Drawing.Size(520, 403);
            this.csvMapEditor.TabIndex = 0;
            // 
            // wizardPageSuccess
            // 
            this.wizardPageSuccess.Controls.Add(this.iconSetupComplete);
            this.wizardPageSuccess.Controls.Add(this.label6);
            this.wizardPageSuccess.Description = "Your CSV \\ Text map has been successfully created.";
            this.wizardPageSuccess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageSuccess.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageSuccess.Location = new System.Drawing.Point(0, 0);
            this.wizardPageSuccess.Name = "wizardPageSuccess";
            this.wizardPageSuccess.Size = new System.Drawing.Size(538, 411);
            this.wizardPageSuccess.TabIndex = 0;
            this.wizardPageSuccess.Title = "CSV \\ Text Map Created";
            // 
            // iconSetupComplete
            // 
            this.iconSetupComplete.Image = global::ShipWorks.Properties.Resources.check16;
            this.iconSetupComplete.Location = new System.Drawing.Point(21, 10);
            this.iconSetupComplete.Name = "iconSetupComplete";
            this.iconSetupComplete.Size = new System.Drawing.Size(16, 16);
            this.iconSetupComplete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.iconSetupComplete.TabIndex = 8;
            this.iconSetupComplete.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(40, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(143, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Your map has been created.";
            // 
            // GenericCsvMapWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 518);
            this.MinimumSize = new System.Drawing.Size(550, 526);
            this.Name = "GenericCsvMapWizard";
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageSampleFile,
            this.wizardPageMapSettings,
            this.wizardPageSuccess});
            this.Text = "CSV \\ Text Import Map";
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageSampleFile.ResumeLayout(false);
            this.wizardPageMapSettings.ResumeLayout(false);
            this.wizardPageSuccess.ResumeLayout(false);
            this.wizardPageSuccess.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.iconSetupComplete)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Wizard.WizardPage wizardPageSampleFile;
        private UI.Wizard.WizardPage wizardPageMapSettings;
        private ShipWorks.Data.Import.Spreadsheet.Types.Csv.Editing.GenericCsvSourceSchemaControl csvSchemaControl;
        private UI.Wizard.WizardPage wizardPageSuccess;
        private System.Windows.Forms.PictureBox iconSetupComplete;
        private System.Windows.Forms.Label label6;
        private ShipWorks.Data.Import.Spreadsheet.Types.Csv.Editing.GenericCsvMapEditorControl csvMapEditor;
    }
}