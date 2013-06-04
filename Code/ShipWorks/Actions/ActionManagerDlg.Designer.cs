namespace ShipWorks.Actions
{
    partial class ActionManagerDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer1 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.close = new System.Windows.Forms.Button();
            this.sandGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnName = new Divelements.SandGrid.GridColumn();
            this.gridColumnRunWhen = new Divelements.SandGrid.GridColumn();
            this.gridColumnTasks = new Divelements.SandGrid.GridColumn();
            this.labelAdd = new System.Windows.Forms.Label();
            this.newAction = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.rename = new System.Windows.Forms.Button();
            this.delete = new System.Windows.Forms.Button();
            this.edit = new System.Windows.Forms.Button();
            this.editionGuiHelper = new ShipWorks.Editions.EditionGuiHelper(this.components);
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(610, 239);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 7;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // sandGrid
            // 
            this.sandGrid.AllowGroupCollapse = true;
            this.sandGrid.AllowMultipleSelection = false;
            this.sandGrid.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sandGrid.CheckBoxes = true;
            this.sandGrid.ColumnClickBehavior = Divelements.SandGrid.ColumnClickBehavior.None;
            this.sandGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnName,
            this.gridColumnRunWhen,
            this.gridColumnTasks});
            this.sandGrid.CommitOnLoseFocus = true;
            this.sandGrid.EnableSearching = false;
            this.sandGrid.ImageTextSeparation = 1;
            this.sandGrid.Location = new System.Drawing.Point(12, 12);
            this.sandGrid.Name = "sandGrid";
            this.sandGrid.NullRepresentation = "";
            this.sandGrid.Renderer = windowsXPRenderer1;
            this.sandGrid.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.InitiateDragDrop;
            this.sandGrid.Size = new System.Drawing.Size(517, 218);
            this.sandGrid.TabIndex = 0;
            this.sandGrid.AfterCheck += new Divelements.SandGrid.GridRowCheckEventHandler(this.OnCheckChanged);
            this.sandGrid.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnGridSelectionChanged);
            this.sandGrid.RowActivated += new Divelements.SandGrid.GridRowEventHandler(this.OnGridRowActivated);
            this.sandGrid.AfterEdit += new Divelements.SandGrid.GridAfterEditEventHandler(this.OnAfterRename);
            // 
            // gridColumnName
            // 
            this.gridColumnName.AllowReorder = false;
            this.gridColumnName.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnName.Clickable = false;
            this.gridColumnName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnName.ForeColorSource = Divelements.SandGrid.CellForeColorSource.RowCell;
            this.gridColumnName.HeaderText = "Action Name";
            this.gridColumnName.MinimumWidth = 100;
            // 
            // gridColumnRunWhen
            // 
            this.gridColumnRunWhen.AllowReorder = false;
            this.gridColumnRunWhen.Clickable = false;
            this.gridColumnRunWhen.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnRunWhen.ForeColorSource = Divelements.SandGrid.CellForeColorSource.RowCell;
            this.gridColumnRunWhen.HeaderText = "Run When";
            this.gridColumnRunWhen.MinimumWidth = 50;
            this.gridColumnRunWhen.Width = 165;
            // 
            // gridColumnTasks
            // 
            this.gridColumnTasks.AllowReorder = false;
            this.gridColumnTasks.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnTasks.Clickable = false;
            this.gridColumnTasks.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnTasks.ForeColorSource = Divelements.SandGrid.CellForeColorSource.RowCell;
            this.gridColumnTasks.HeaderText = "Tasks";
            this.gridColumnTasks.Width = 248;
            // 
            // labelAdd
            // 
            this.labelAdd.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAdd.AutoSize = true;
            this.labelAdd.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelAdd.Location = new System.Drawing.Point(532, 122);
            this.labelAdd.Name = "labelAdd";
            this.labelAdd.Size = new System.Drawing.Size(29, 13);
            this.labelAdd.TabIndex = 5;
            this.labelAdd.Text = "Add";
            // 
            // newAction
            // 
            this.newAction.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newAction.Image = global::ShipWorks.Properties.Resources.add16;
            this.newAction.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.newAction.Location = new System.Drawing.Point(535, 138);
            this.newAction.Name = "newAction";
            this.newAction.Size = new System.Drawing.Size(150, 23);
            this.newAction.TabIndex = 6;
            this.newAction.Text = "New Action";
            this.newAction.UseVisualStyleBackColor = true;
            this.newAction.Click += new System.EventHandler(this.OnNewAction);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label4.Location = new System.Drawing.Point(532, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Edit";
            // 
            // rename
            // 
            this.rename.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rename.Image = global::ShipWorks.Properties.Resources.rename;
            this.rename.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rename.Location = new System.Drawing.Point(535, 58);
            this.rename.Name = "rename";
            this.rename.Size = new System.Drawing.Size(150, 23);
            this.rename.TabIndex = 3;
            this.rename.Text = "Rename";
            this.rename.UseVisualStyleBackColor = true;
            this.rename.Click += new System.EventHandler(this.OnRename);
            // 
            // delete
            // 
            this.delete.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.delete.Image = global::ShipWorks.Properties.Resources.delete16;
            this.delete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.delete.Location = new System.Drawing.Point(535, 87);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(150, 23);
            this.delete.TabIndex = 4;
            this.delete.Text = "Delete";
            this.delete.UseVisualStyleBackColor = true;
            this.delete.Click += new System.EventHandler(this.OnDelete);
            // 
            // edit
            // 
            this.edit.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.edit.Image = global::ShipWorks.Properties.Resources.edit16;
            this.edit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.edit.Location = new System.Drawing.Point(535, 28);
            this.edit.Name = "edit";
            this.edit.Size = new System.Drawing.Size(150, 23);
            this.edit.TabIndex = 2;
            this.edit.Text = "Edit";
            this.edit.UseVisualStyleBackColor = true;
            this.edit.Click += new System.EventHandler(this.OnEdit);
            // 
            // ActionManagerDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(697, 274);
            this.Controls.Add(this.sandGrid);
            this.Controls.Add(this.labelAdd);
            this.Controls.Add(this.newAction);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.rename);
            this.Controls.Add(this.delete);
            this.Controls.Add(this.edit);
            this.Controls.Add(this.close);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(591, 242);
            this.Name = "ActionManagerDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Action Manager";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button close;
        private Divelements.SandGrid.SandGrid sandGrid;
        private Divelements.SandGrid.GridColumn gridColumnName;
        private Divelements.SandGrid.GridColumn gridColumnRunWhen;
        private Divelements.SandGrid.GridColumn gridColumnTasks;
        private System.Windows.Forms.Label labelAdd;
        private System.Windows.Forms.Button newAction;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button rename;
        private System.Windows.Forms.Button delete;
        private System.Windows.Forms.Button edit;
        private Editions.EditionGuiHelper editionGuiHelper;
    }
}