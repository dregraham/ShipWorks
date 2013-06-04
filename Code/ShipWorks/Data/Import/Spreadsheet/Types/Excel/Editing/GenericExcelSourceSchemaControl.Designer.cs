namespace ShipWorks.Data.Import.Spreadsheet.Types.Excel.Editing
{
    partial class GenericExcelSourceSchemaControl
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
            this.labelColumnsFound2 = new System.Windows.Forms.Label();
            this.labelSample = new System.Windows.Forms.Label();
            this.labelSampleDescription = new System.Windows.Forms.Label();
            this.labelExcelInfo = new System.Windows.Forms.Label();
            this.sampleFileBrowse = new System.Windows.Forms.Button();
            this.panelOptions = new System.Windows.Forms.Panel();
            this.comboStartingCell = new ShipWorks.Data.Import.Spreadsheet.Types.Excel.Editing.GenericExcelCellChooserComboBox();
            this.labelSheet = new System.Windows.Forms.Label();
            this.columnGrid = new ShipWorks.Data.Import.Spreadsheet.Editing.GenericSpreadsheetSourceColumnGrid();
            this.labelColumnsFound1 = new System.Windows.Forms.Label();
            this.sampleFileName = new ShipWorks.UI.Controls.PathTextBox();
            this.panelOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelColumnsFound2
            // 
            this.labelColumnsFound2.AutoSize = true;
            this.labelColumnsFound2.Location = new System.Drawing.Point(20, 57);
            this.labelColumnsFound2.Name = "labelColumnsFound2";
            this.labelColumnsFound2.Size = new System.Drawing.Size(84, 13);
            this.labelColumnsFound2.TabIndex = 26;
            this.labelColumnsFound2.Text = "Columns Found:";
            // 
            // labelSample
            // 
            this.labelSample.AutoSize = true;
            this.labelSample.Location = new System.Drawing.Point(42, 47);
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
            this.labelSampleDescription.Size = new System.Drawing.Size(327, 13);
            this.labelSampleDescription.TabIndex = 17;
            this.labelSampleDescription.Text = "Select a sample Excel file that you will be importing into ShipWorks:";
            // 
            // labelExcelInfo
            // 
            this.labelExcelInfo.AutoSize = true;
            this.labelExcelInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelExcelInfo.Location = new System.Drawing.Point(1, 2);
            this.labelExcelInfo.Name = "labelExcelInfo";
            this.labelExcelInfo.Size = new System.Drawing.Size(109, 13);
            this.labelExcelInfo.TabIndex = 16;
            this.labelExcelInfo.Text = "Your Excel Format";
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
            this.panelOptions.Controls.Add(this.comboStartingCell);
            this.panelOptions.Controls.Add(this.labelSheet);
            this.panelOptions.Controls.Add(this.columnGrid);
            this.panelOptions.Controls.Add(this.labelColumnsFound2);
            this.panelOptions.Controls.Add(this.labelColumnsFound1);
            this.panelOptions.Location = new System.Drawing.Point(5, 70);
            this.panelOptions.Name = "panelOptions";
            this.panelOptions.Size = new System.Drawing.Size(474, 534);
            this.panelOptions.TabIndex = 28;
            this.panelOptions.Visible = false;
            // 
            // comboStartingCell
            // 
            this.comboStartingCell.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStartingCell.FormattingEnabled = true;
            this.comboStartingCell.Location = new System.Drawing.Point(105, 5);
            this.comboStartingCell.Name = "comboStartingCell";
            this.comboStartingCell.Size = new System.Drawing.Size(281, 21);
            this.comboStartingCell.TabIndex = 29;
            this.comboStartingCell.SelectionChanged += new System.EventHandler(this.OnChangeStartingCell);
            // 
            // labelSheet
            // 
            this.labelSheet.AutoSize = true;
            this.labelSheet.Location = new System.Drawing.Point(8, 8);
            this.labelSheet.Name = "labelSheet";
            this.labelSheet.Size = new System.Drawing.Size(91, 13);
            this.labelSheet.TabIndex = 28;
            this.labelSheet.Text = "Headers location:";
            // 
            // columnGrid
            // 
            this.columnGrid.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.columnGrid.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.columnGrid.HotTrackColumn = null;
            this.columnGrid.Location = new System.Drawing.Point(23, 73);
            this.columnGrid.Name = "columnGrid";
            this.columnGrid.SelectedColumn = null;
            this.columnGrid.Size = new System.Drawing.Size(445, 455);
            this.columnGrid.TabIndex = 27;
            // 
            // labelColumnsFound1
            // 
            this.labelColumnsFound1.AutoSize = true;
            this.labelColumnsFound1.Location = new System.Drawing.Point(3, 37);
            this.labelColumnsFound1.Name = "labelColumnsFound1";
            this.labelColumnsFound1.Size = new System.Drawing.Size(311, 13);
            this.labelColumnsFound1.TabIndex = 25;
            this.labelColumnsFound1.Text = "ShipWorks determined the following from your file and settings.";
            // 
            // sampleFileName
            // 
            this.sampleFileName.Location = new System.Drawing.Point(110, 44);
            this.sampleFileName.Name = "sampleFileName";
            this.sampleFileName.ReadOnly = true;
            this.sampleFileName.Size = new System.Drawing.Size(281, 21);
            this.sampleFileName.TabIndex = 23;
            // 
            // GenericExcelSourceSchemaControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelSample);
            this.Controls.Add(this.sampleFileName);
            this.Controls.Add(this.sampleFileBrowse);
            this.Controls.Add(this.labelSampleDescription);
            this.Controls.Add(this.labelExcelInfo);
            this.Controls.Add(this.panelOptions);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "GenericExcelSourceSchemaControl";
            this.Size = new System.Drawing.Size(493, 604);
            this.panelOptions.ResumeLayout(false);
            this.panelOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelColumnsFound2;
        private System.Windows.Forms.Label labelSample;
        private UI.Controls.PathTextBox sampleFileName;
        private System.Windows.Forms.Button sampleFileBrowse;
        private System.Windows.Forms.Label labelSampleDescription;
        private System.Windows.Forms.Label labelExcelInfo;
        private ShipWorks.Data.Import.Spreadsheet.Editing.GenericSpreadsheetSourceColumnGrid columnGrid;
        private System.Windows.Forms.Panel panelOptions;
        private System.Windows.Forms.Label labelSheet;
        private GenericExcelCellChooserComboBox comboStartingCell;
        private System.Windows.Forms.Label labelColumnsFound1;
    }
}
