namespace ShipWorks.Stores.Platforms.GenericFile.Formats.Excel
{
    partial class GenericFileExcelAccountSettingsControl
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
            this.sectionTitleImportFrom = new ShipWorks.UI.Controls.SectionTitle();
            this.fileSourceControl = new ShipWorks.Stores.Platforms.GenericFile.Sources.GenericFileSourceMasterControl();
            this.sectionTitleExcelMapping = new ShipWorks.UI.Controls.SectionTitle();
            this.excelMapChooser = new ShipWorks.Data.Import.Spreadsheet.Types.Excel.Editing.GenericExcelMapChooserControl();
            this.SuspendLayout();
            // 
            // sectionTitleImportFrom
            // 
            this.sectionTitleImportFrom.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitleImportFrom.Location = new System.Drawing.Point(13, 131);
            this.sectionTitleImportFrom.Name = "sectionTitleImportFrom";
            this.sectionTitleImportFrom.Size = new System.Drawing.Size(514, 22);
            this.sectionTitleImportFrom.TabIndex = 7;
            this.sectionTitleImportFrom.Text = "Import From";
            // 
            // fileSourceControl
            // 
            this.fileSourceControl.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fileSourceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.fileSourceControl.Location = new System.Drawing.Point(14, 127);
            this.fileSourceControl.Name = "fileSourceControl";
            this.fileSourceControl.Size = new System.Drawing.Size(510, 143);
            this.fileSourceControl.TabIndex = 6;
            // 
            // sectionTitleExcelMapping
            // 
            this.sectionTitleExcelMapping.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitleExcelMapping.Location = new System.Drawing.Point(13, 12);
            this.sectionTitleExcelMapping.Name = "sectionTitleExcelMapping";
            this.sectionTitleExcelMapping.Size = new System.Drawing.Size(514, 22);
            this.sectionTitleExcelMapping.TabIndex = 8;
            this.sectionTitleExcelMapping.Text = "Excel Import Map";
            // 
            // excelMapChooser
            // 
            this.excelMapChooser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.excelMapChooser.Location = new System.Drawing.Point(-3, 7);
            this.excelMapChooser.Map = null;
            this.excelMapChooser.Name = "excelMapChooser";
            this.excelMapChooser.Size = new System.Drawing.Size(514, 112);
            this.excelMapChooser.TabIndex = 9;
            // 
            // GenericFileExcelAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.sectionTitleExcelMapping);
            this.Controls.Add(this.sectionTitleImportFrom);
            this.Controls.Add(this.fileSourceControl);
            this.Controls.Add(this.excelMapChooser);
            this.Name = "GenericFileExcelAccountSettingsControl";
            this.Size = new System.Drawing.Size(544, 554);
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Controls.SectionTitle sectionTitleImportFrom;
        private ShipWorks.Stores.Platforms.GenericFile.Sources.GenericFileSourceMasterControl fileSourceControl;
        private UI.Controls.SectionTitle sectionTitleExcelMapping;
        private ShipWorks.Data.Import.Spreadsheet.Types.Excel.Editing.GenericExcelMapChooserControl excelMapChooser;
    }
}
