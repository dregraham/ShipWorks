namespace ShipWorks.Data.Import.Spreadsheet.Editing
{
    partial class GenericSpreadsheetSourceColumnGrid
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
            ShipWorks.UI.Controls.SandGrid.SandGridThemedSelectionRenderer sandGridThemedSelectionRenderer1 = new ShipWorks.UI.Controls.SandGrid.SandGridThemedSelectionRenderer();
            this.columnsGrid = new ShipWorks.UI.Controls.SandGrid.SandGridDragDrop();
            this.gridColumnName = new Divelements.SandGrid.GridColumn();
            this.gridColumnExample1 = new Divelements.SandGrid.GridColumn();
            this.gridColumnExample2 = new Divelements.SandGrid.GridColumn();
            this.gridColumnExample3 = new Divelements.SandGrid.GridColumn();
            ((System.ComponentModel.ISupportInitialize) (this.columnsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // columnsGrid
            // 
            this.columnsGrid.AllowGroupCollapse = true;
            this.columnsGrid.AllowMultipleSelection = false;
            this.columnsGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnName,
            this.gridColumnExample1,
            this.gridColumnExample2,
            this.gridColumnExample3});
            this.columnsGrid.CommitOnLoseFocus = true;
            this.columnsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.columnsGrid.GridLines = Divelements.SandGrid.GridLinesDisplayType.Both;
            this.columnsGrid.Location = new System.Drawing.Point(0, 0);
            this.columnsGrid.Name = "columnsGrid";
            this.columnsGrid.Renderer = sandGridThemedSelectionRenderer1;
            this.columnsGrid.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.InitiateDragDrop;
            this.columnsGrid.RowHighlightType = Divelements.SandGrid.RowHighlightType.None;
            this.columnsGrid.ShadeAlternateRows = true;
            this.columnsGrid.Size = new System.Drawing.Size(340, 339);
            this.columnsGrid.TabIndex = 28;
            this.columnsGrid.WhitespaceClickBehavior = Divelements.SandGrid.WhitespaceClickBehavior.None;
            this.columnsGrid.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnGridSelectionChanged);
            // 
            // gridColumnName
            // 
            this.gridColumnName.AllowReorder = false;
            this.gridColumnName.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnName.AutoSizeIncludeHeader = true;
            this.gridColumnName.AutoSortType = Divelements.SandGrid.ColumnAutoSortType.None;
            this.gridColumnName.Clickable = false;
            this.gridColumnName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnName.ForeColorSource = Divelements.SandGrid.CellForeColorSource.RowCell;
            this.gridColumnName.HeaderText = "Column Name";
            this.gridColumnName.MinimumWidth = 100;
            // 
            // gridColumnExample1
            // 
            this.gridColumnExample1.AllowReorder = false;
            this.gridColumnExample1.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnExample1.AutoSortType = Divelements.SandGrid.ColumnAutoSortType.None;
            this.gridColumnExample1.Clickable = false;
            this.gridColumnExample1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnExample1.ForeColorSource = Divelements.SandGrid.CellForeColorSource.RowCell;
            this.gridColumnExample1.HeaderText = "Example 1";
            this.gridColumnExample1.MinimumWidth = 50;
            this.gridColumnExample1.Width = 70;
            // 
            // gridColumnExample2
            // 
            this.gridColumnExample2.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnExample2.HeaderText = "Example 2";
            this.gridColumnExample2.Width = 81;
            // 
            // gridColumnExample3
            // 
            this.gridColumnExample3.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnExample3.HeaderText = "Example 3";
            // 
            // GenericCsvSourceColumnGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.columnsGrid);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "GenericCsvSourceColumnGrid";
            this.Size = new System.Drawing.Size(340, 339);
            ((System.ComponentModel.ISupportInitialize) (this.columnsGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Controls.SandGrid.SandGridDragDrop columnsGrid;
        private Divelements.SandGrid.GridColumn gridColumnName;
        private Divelements.SandGrid.GridColumn gridColumnExample1;
        private Divelements.SandGrid.GridColumn gridColumnExample2;
        private Divelements.SandGrid.GridColumn gridColumnExample3;
    }
}
