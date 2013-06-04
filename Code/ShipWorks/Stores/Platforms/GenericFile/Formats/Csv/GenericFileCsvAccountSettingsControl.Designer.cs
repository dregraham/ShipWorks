namespace ShipWorks.Stores.Platforms.GenericFile.Formats.Csv
{
    partial class GenericFileCsvAccountSettingsControl
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
            this.sectionTitleCsvMapping = new ShipWorks.UI.Controls.SectionTitle();
            this.csvMapChooser = new ShipWorks.Data.Import.Spreadsheet.Types.Csv.OrderSchema.GenericCsvOrderMapChooserControl();
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
            // sectionTitleCsvMapping
            // 
            this.sectionTitleCsvMapping.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitleCsvMapping.Location = new System.Drawing.Point(13, 12);
            this.sectionTitleCsvMapping.Name = "sectionTitleCsvMapping";
            this.sectionTitleCsvMapping.Size = new System.Drawing.Size(514, 22);
            this.sectionTitleCsvMapping.TabIndex = 8;
            this.sectionTitleCsvMapping.Text = "CSV Import Map";
            // 
            // csvMapChooser
            // 
            this.csvMapChooser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.csvMapChooser.Location = new System.Drawing.Point(-3, 7);
            this.csvMapChooser.Map = null;
            this.csvMapChooser.Name = "csvMapChooser";
            this.csvMapChooser.Size = new System.Drawing.Size(514, 112);
            this.csvMapChooser.TabIndex = 9;
            // 
            // GenericFileCsvAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.sectionTitleCsvMapping);
            this.Controls.Add(this.sectionTitleImportFrom);
            this.Controls.Add(this.fileSourceControl);
            this.Controls.Add(this.csvMapChooser);
            this.Name = "GenericFileCsvAccountSettingsControl";
            this.Size = new System.Drawing.Size(544, 554);
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Controls.SectionTitle sectionTitleImportFrom;
        private ShipWorks.Stores.Platforms.GenericFile.Sources.GenericFileSourceMasterControl fileSourceControl;
        private UI.Controls.SectionTitle sectionTitleCsvMapping;
        private ShipWorks.Data.Import.Spreadsheet.Types.Csv.OrderSchema.GenericCsvOrderMapChooserControl csvMapChooser;
    }
}
