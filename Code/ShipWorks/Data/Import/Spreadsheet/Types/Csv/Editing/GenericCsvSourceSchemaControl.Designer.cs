namespace ShipWorks.Data.Import.Spreadsheet.Types.Csv.Editing
{
    partial class GenericCsvSourceSchemaControl
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
            this.labelColumnsFound1 = new System.Windows.Forms.Label();
            this.labelSample = new System.Windows.Forms.Label();
            this.labelSampleDescription = new System.Windows.Forms.Label();
            this.labelCsvInfo = new System.Windows.Forms.Label();
            this.sampleFileBrowse = new System.Windows.Forms.Button();
            this.panelOptions = new System.Windows.Forms.Panel();
            this.fileFormatControl = new ShipWorks.Data.Import.Spreadsheet.Types.Csv.Editing.GenericCsvFileFormatControl();
            this.columnGrid = new ShipWorks.Data.Import.Spreadsheet.Editing.GenericSpreadsheetSourceColumnGrid();
            this.sampleFileName = new ShipWorks.UI.Controls.PathTextBox();
            this.panelOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelColumnsFound1
            // 
            this.labelColumnsFound1.AutoSize = true;
            this.labelColumnsFound1.Location = new System.Drawing.Point(3, 92);
            this.labelColumnsFound1.Name = "labelColumnsFound1";
            this.labelColumnsFound1.Size = new System.Drawing.Size(311, 13);
            this.labelColumnsFound1.TabIndex = 25;
            this.labelColumnsFound1.Text = "ShipWorks determined the following from your file and settings.";
            // 
            // labelSample
            // 
            this.labelSample.AutoSize = true;
            this.labelSample.Location = new System.Drawing.Point(29, 47);
            this.labelSample.Name = "labelSample";
            this.labelSample.Size = new System.Drawing.Size(62, 13);
            this.labelSample.TabIndex = 24;
            this.labelSample.Text = "Sample file:";
            // 
            // labelSampleDescription
            // 
            this.labelSampleDescription.AutoSize = true;
            this.labelSampleDescription.Location = new System.Drawing.Point(2, 21);
            this.labelSampleDescription.Name = "labelSampleDescription";
            this.labelSampleDescription.Size = new System.Drawing.Size(386, 13);
            this.labelSampleDescription.TabIndex = 17;
            this.labelSampleDescription.Text = "Select a sample file in the CSV format that you will be importing into ShipWorks:" +
                "";
            // 
            // labelCsvInfo
            // 
            this.labelCsvInfo.AutoSize = true;
            this.labelCsvInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelCsvInfo.Location = new System.Drawing.Point(1, 2);
            this.labelCsvInfo.Name = "labelCsvInfo";
            this.labelCsvInfo.Size = new System.Drawing.Size(139, 13);
            this.labelCsvInfo.TabIndex = 16;
            this.labelCsvInfo.Text = "Your CSV \\ Text Format";
            // 
            // sampleFileBrowse
            // 
            this.sampleFileBrowse.Location = new System.Drawing.Point(397, 42);
            this.sampleFileBrowse.Name = "sampleFileBrowse";
            this.sampleFileBrowse.Size = new System.Drawing.Size(75, 23);
            this.sampleFileBrowse.TabIndex = 18;
            this.sampleFileBrowse.Text = "Browse...";
            this.sampleFileBrowse.UseVisualStyleBackColor = true;
            this.sampleFileBrowse.Click += new System.EventHandler(this.OnBrowse);
            // 
            // panelOptions
            // 
            this.panelOptions.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelOptions.Controls.Add(this.fileFormatControl);
            this.panelOptions.Controls.Add(this.columnGrid);
            this.panelOptions.Controls.Add(this.labelColumnsFound1);
            this.panelOptions.Location = new System.Drawing.Point(12, 70);
            this.panelOptions.Name = "panelOptions";
            this.panelOptions.Size = new System.Drawing.Size(481, 336);
            this.panelOptions.TabIndex = 28;
            this.panelOptions.Visible = false;
            // 
            // fileFormatControl
            // 
            this.fileFormatControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.fileFormatControl.Location = new System.Drawing.Point(6, 1);
            this.fileFormatControl.Name = "fileFormatControl";
            this.fileFormatControl.Size = new System.Drawing.Size(458, 84);
            this.fileFormatControl.TabIndex = 28;
            this.fileFormatControl.FileFormatChanged += new System.EventHandler(this.OnChangeFileFormatOption);
            // 
            // columnGrid
            // 
            this.columnGrid.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.columnGrid.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.columnGrid.HotTrackColumn = null;
            this.columnGrid.Location = new System.Drawing.Point(23, 112);
            this.columnGrid.Name = "columnGrid";
            this.columnGrid.SelectedColumn = null;
            this.columnGrid.Size = new System.Drawing.Size(437, 218);
            this.columnGrid.TabIndex = 27;
            // 
            // sampleFileName
            // 
            this.sampleFileName.Location = new System.Drawing.Point(96, 44);
            this.sampleFileName.Name = "sampleFileName";
            this.sampleFileName.ReadOnly = true;
            this.sampleFileName.Size = new System.Drawing.Size(295, 21);
            this.sampleFileName.TabIndex = 23;
            // 
            // GenericCsvSourceSchemaControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelSample);
            this.Controls.Add(this.sampleFileName);
            this.Controls.Add(this.sampleFileBrowse);
            this.Controls.Add(this.labelSampleDescription);
            this.Controls.Add(this.labelCsvInfo);
            this.Controls.Add(this.panelOptions);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "GenericCsvSourceSchemaControl";
            this.Size = new System.Drawing.Size(493, 409);
            this.panelOptions.ResumeLayout(false);
            this.panelOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelColumnsFound1;
        private System.Windows.Forms.Label labelSample;
        private UI.Controls.PathTextBox sampleFileName;
        private System.Windows.Forms.Button sampleFileBrowse;
        private System.Windows.Forms.Label labelSampleDescription;
        private System.Windows.Forms.Label labelCsvInfo;
        private ShipWorks.Data.Import.Spreadsheet.Editing.GenericSpreadsheetSourceColumnGrid columnGrid;
        private System.Windows.Forms.Panel panelOptions;
        private GenericCsvFileFormatControl fileFormatControl;
    }
}
