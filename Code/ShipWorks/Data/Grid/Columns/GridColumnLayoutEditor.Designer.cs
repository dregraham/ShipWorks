namespace ShipWorks.Data.Grid.Columns
{
    partial class GridColumnLayoutEditor
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
            this.moveDown = new System.Windows.Forms.Button();
            this.moveUp = new System.Windows.Forms.Button();
            this.labelOrderVisibility = new System.Windows.Forms.Label();
            this.comboSortDirection = new System.Windows.Forms.ComboBox();
            this.comboSortColumn = new System.Windows.Forms.ComboBox();
            this.labelDefaultSort = new System.Windows.Forms.Label();
            this.width = new System.Windows.Forms.TextBox();
            this.labelWidth = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.editColumnFormat = new System.Windows.Forms.Button();
            this.labelReset = new System.Windows.Forms.Label();
            this.sandGrid = new ShipWorks.UI.Controls.SandGrid.SandGridDragDrop();
            this.gridColumnColumn = new GridColumnDefinitionNameColumn();
            this.trackBarWidth = new ShipWorks.UI.Controls.TransparentTrackBar();
            ((System.ComponentModel.ISupportInitialize) (this.sandGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.trackBarWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // moveDown
            // 
            this.moveDown.Image = global::ShipWorks.Properties.Resources.arrow_down_blue;
            this.moveDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.moveDown.Location = new System.Drawing.Point(219, 48);
            this.moveDown.Name = "moveDown";
            this.moveDown.Size = new System.Drawing.Size(114, 23);
            this.moveDown.TabIndex = 3;
            this.moveDown.Text = "Move Down";
            this.moveDown.UseVisualStyleBackColor = true;
            this.moveDown.Click += new System.EventHandler(this.OnMoveDown);
            // 
            // moveUp
            // 
            this.moveUp.Image = global::ShipWorks.Properties.Resources.arrow_up_blue;
            this.moveUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.moveUp.Location = new System.Drawing.Point(219, 19);
            this.moveUp.Name = "moveUp";
            this.moveUp.Size = new System.Drawing.Size(114, 23);
            this.moveUp.TabIndex = 2;
            this.moveUp.Text = "Move Up";
            this.moveUp.UseVisualStyleBackColor = true;
            this.moveUp.Click += new System.EventHandler(this.OnMoveUp);
            // 
            // labelOrderVisibility
            // 
            this.labelOrderVisibility.AutoSize = true;
            this.labelOrderVisibility.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelOrderVisibility.Location = new System.Drawing.Point(4, 3);
            this.labelOrderVisibility.Name = "labelOrderVisibility";
            this.labelOrderVisibility.Size = new System.Drawing.Size(158, 13);
            this.labelOrderVisibility.TabIndex = 0;
            this.labelOrderVisibility.Text = "Column Order and Visibility";
            // 
            // comboSortDirection
            // 
            this.comboSortDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSortDirection.FormattingEnabled = true;
            this.comboSortDirection.Items.AddRange(new object[] {
            "Ascending",
            "Descending"});
            this.comboSortDirection.Location = new System.Drawing.Point(219, 169);
            this.comboSortDirection.Name = "comboSortDirection";
            this.comboSortDirection.Size = new System.Drawing.Size(114, 21);
            this.comboSortDirection.TabIndex = 9;
            // 
            // comboSortColumn
            // 
            this.comboSortColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSortColumn.FormattingEnabled = true;
            this.comboSortColumn.Location = new System.Drawing.Point(219, 144);
            this.comboSortColumn.MaxDropDownItems = 20;
            this.comboSortColumn.Name = "comboSortColumn";
            this.comboSortColumn.Size = new System.Drawing.Size(114, 21);
            this.comboSortColumn.TabIndex = 8;
            // 
            // labelDefaultSort
            // 
            this.labelDefaultSort.AutoSize = true;
            this.labelDefaultSort.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelDefaultSort.Location = new System.Drawing.Point(216, 128);
            this.labelDefaultSort.Name = "labelDefaultSort";
            this.labelDefaultSort.Size = new System.Drawing.Size(75, 13);
            this.labelDefaultSort.TabIndex = 7;
            this.labelDefaultSort.Text = "Default Sort";
            // 
            // width
            // 
            this.width.Location = new System.Drawing.Point(299, 96);
            this.width.Name = "width";
            this.width.Size = new System.Drawing.Size(34, 21);
            this.width.TabIndex = 6;
            this.width.Text = "25";
            // 
            // labelWidth
            // 
            this.labelWidth.AutoSize = true;
            this.labelWidth.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelWidth.Location = new System.Drawing.Point(216, 81);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(40, 13);
            this.labelWidth.TabIndex = 4;
            this.labelWidth.Text = "Width";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label1.Location = new System.Drawing.Point(219, 199);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Formatting";
            // 
            // editColumnFormat
            // 
            this.editColumnFormat.Enabled = false;
            this.editColumnFormat.Location = new System.Drawing.Point(219, 215);
            this.editColumnFormat.Name = "editColumnFormat";
            this.editColumnFormat.Size = new System.Drawing.Size(114, 23);
            this.editColumnFormat.TabIndex = 11;
            this.editColumnFormat.Text = "Edit...";
            this.editColumnFormat.UseVisualStyleBackColor = true;
            this.editColumnFormat.Click += new System.EventHandler(this.OnEditFormatting);
            // 
            // labelReset
            // 
            this.labelReset.AutoSize = true;
            this.labelReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelReset.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelReset.ForeColor = System.Drawing.Color.Blue;
            this.labelReset.Location = new System.Drawing.Point(219, 258);
            this.labelReset.Name = "labelReset";
            this.labelReset.Size = new System.Drawing.Size(35, 13);
            this.labelReset.TabIndex = 12;
            this.labelReset.Text = "Reset";
            this.labelReset.Click += new System.EventHandler(this.OnReset);
            // 
            // sandGrid
            // 
            this.sandGrid.AllowDrag = true;
            this.sandGrid.AllowDrop = true;
            this.sandGrid.AllowMultipleSelection = false;
            this.sandGrid.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.sandGrid.CellDragBehavior = Divelements.SandGrid.CellDragBehavior.None;
            this.sandGrid.CheckBoxes = true;
            this.sandGrid.ColumnClickBehavior = Divelements.SandGrid.ColumnClickBehavior.None;
            this.sandGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnColumn});
            this.sandGrid.CommitOnLoseFocus = true;
            this.sandGrid.GridLines = Divelements.SandGrid.GridLinesDisplayType.Both;
            this.sandGrid.ImageTextSeparation = 7;
            this.sandGrid.KeyboardEditMode = ((Divelements.SandGrid.KeyboardEditMode) ((Divelements.SandGrid.KeyboardEditMode.EditOnKeystroke | Divelements.SandGrid.KeyboardEditMode.EditOnF2)));
            this.sandGrid.Location = new System.Drawing.Point(7, 19);
            this.sandGrid.MouseEditMode = Divelements.SandGrid.MouseEditMode.DoubleClick;
            this.sandGrid.Name = "sandGrid";
            this.sandGrid.Renderer = sandGridThemedSelectionRenderer1;
            this.sandGrid.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.InitiateDragDrop;
            this.sandGrid.RowEditMode = Divelements.SandGrid.RowEditMode.TargetCell;
            this.sandGrid.ShowColumnHeaders = false;
            this.sandGrid.Size = new System.Drawing.Size(206, 278);
            this.sandGrid.TabIndex = 1;
            this.sandGrid.WhitespaceClickBehavior = Divelements.SandGrid.WhitespaceClickBehavior.None;
            this.sandGrid.AfterCheck += new Divelements.SandGrid.GridRowCheckEventHandler(this.OnCheckChanged);
            this.sandGrid.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnGridSelectionChanged);
            this.sandGrid.GridRowDropped += new ShipWorks.UI.Controls.SandGrid.GridRowDroppedEventHandler(this.OnGridRowDropped);
            // 
            // gridColumnColumn
            // 
            this.gridColumnColumn.AllowEditing = false;
            this.gridColumnColumn.AllowReorder = false;
            this.gridColumnColumn.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnColumn.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnColumn.HeaderText = "Column";
            this.gridColumnColumn.Width = 202;
            // 
            // trackBarWidth
            // 
            this.trackBarWidth.LargeChange = 50;
            this.trackBarWidth.Location = new System.Drawing.Point(217, 96);
            this.trackBarWidth.Maximum = 250;
            this.trackBarWidth.Name = "trackBarWidth";
            this.trackBarWidth.Size = new System.Drawing.Size(86, 45);
            this.trackBarWidth.SmallChange = 10;
            this.trackBarWidth.TabIndex = 5;
            this.trackBarWidth.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // GridColumnLayoutEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelReset);
            this.Controls.Add(this.editColumnFormat);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelOrderVisibility);
            this.Controls.Add(this.sandGrid);
            this.Controls.Add(this.width);
            this.Controls.Add(this.labelWidth);
            this.Controls.Add(this.moveUp);
            this.Controls.Add(this.labelDefaultSort);
            this.Controls.Add(this.comboSortDirection);
            this.Controls.Add(this.moveDown);
            this.Controls.Add(this.comboSortColumn);
            this.Controls.Add(this.trackBarWidth);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MinimumSize = new System.Drawing.Size(344, 200);
            this.Name = "GridColumnLayoutEditor";
            this.Size = new System.Drawing.Size(344, 300);
            ((System.ComponentModel.ISupportInitialize) (this.sandGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.trackBarWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.UI.Controls.SandGrid.SandGridDragDrop sandGrid;
        private GridColumnDefinitionNameColumn gridColumnColumn;
        private System.Windows.Forms.Button moveDown;
        private System.Windows.Forms.Button moveUp;
        private System.Windows.Forms.Label labelOrderVisibility;
        private System.Windows.Forms.ComboBox comboSortDirection;
        private System.Windows.Forms.ComboBox comboSortColumn;
        private System.Windows.Forms.Label labelDefaultSort;
        private System.Windows.Forms.Label labelWidth;
        private ShipWorks.UI.Controls.TransparentTrackBar trackBarWidth;
        private System.Windows.Forms.TextBox width;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button editColumnFormat;
        private System.Windows.Forms.Label labelReset;
    }
}
