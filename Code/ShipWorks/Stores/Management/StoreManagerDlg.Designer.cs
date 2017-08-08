namespace ShipWorks.Stores.Management
{
    partial class StoreManagerDlg
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

                if (fontStrikeout != null)
                {
                    fontStrikeout.Dispose();
                }
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
            this.components = new System.ComponentModel.Container();
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer1 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.label4 = new System.Windows.Forms.Label();
            this.labelAdd = new System.Windows.Forms.Label();
            this.sandGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnName = new Divelements.SandGrid.GridColumn();
            this.gridColumnStoreType = new Divelements.SandGrid.GridColumn();
            this.gridColumnLastDownload = new Divelements.SandGrid.GridColumn();
            this.cancel = new System.Windows.Forms.Button();
            this.addStore = new System.Windows.Forms.Button();
            this.rename = new System.Windows.Forms.Button();
            this.delete = new System.Windows.Forms.Button();
            this.edit = new System.Windows.Forms.Button();
            this.editionGuiHelper = new ShipWorks.Editions.EditionGuiHelper(this.components);
            this.showDisabledStores = new System.Windows.Forms.CheckBox();
            this.labelDisabledCount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(451, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Edit";
            // 
            // labelAdd
            // 
            this.labelAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAdd.AutoSize = true;
            this.labelAdd.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAdd.Location = new System.Drawing.Point(451, 120);
            this.labelAdd.Name = "labelAdd";
            this.labelAdd.Size = new System.Drawing.Size(29, 13);
            this.labelAdd.TabIndex = 20;
            this.labelAdd.Text = "Add";
            // 
            // sandGrid
            // 
            this.sandGrid.AllowGroupCollapse = true;
            this.sandGrid.AllowMultipleSelection = false;
            this.sandGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sandGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnName,
            this.gridColumnStoreType,
            this.gridColumnLastDownload});
            this.sandGrid.CommitOnLoseFocus = true;
            this.sandGrid.GridLines = Divelements.SandGrid.GridLinesDisplayType.Both;
            this.sandGrid.ImageTextSeparation = 1;
            this.sandGrid.Location = new System.Drawing.Point(7, 12);
            this.sandGrid.Name = "sandGrid";
            this.sandGrid.Renderer = windowsXPRenderer1;
            this.sandGrid.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.InitiateDragDrop;
            this.sandGrid.Size = new System.Drawing.Size(438, 184);
            this.sandGrid.TabIndex = 0;
            this.sandGrid.WhitespaceClickBehavior = Divelements.SandGrid.WhitespaceClickBehavior.None;
            this.sandGrid.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnChangeGridSelection);
            this.sandGrid.RowActivated += new Divelements.SandGrid.GridRowEventHandler(this.OnRowActivated);
            this.sandGrid.AfterEdit += new Divelements.SandGrid.GridAfterEditEventHandler(this.OnAfterRename);
            // 
            // gridColumnName
            // 
            this.gridColumnName.AllowReorder = false;
            this.gridColumnName.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnName.AutoSizeIncludeHeader = true;
            this.gridColumnName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumnName.ForeColorSource = Divelements.SandGrid.CellForeColorSource.RowCell;
            this.gridColumnName.HeaderText = "Store Name";
            this.gridColumnName.MinimumWidth = 100;
            // 
            // gridColumnStoreType
            // 
            this.gridColumnStoreType.AllowReorder = false;
            this.gridColumnStoreType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumnStoreType.ForeColorSource = Divelements.SandGrid.CellForeColorSource.RowCell;
            this.gridColumnStoreType.HeaderText = "Store Type";
            this.gridColumnStoreType.MinimumWidth = 50;
            this.gridColumnStoreType.Width = 113;
            // 
            // gridColumnLastDownload
            // 
            this.gridColumnLastDownload.AllowReorder = false;
            this.gridColumnLastDownload.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnLastDownload.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumnLastDownload.ForeColorSource = Divelements.SandGrid.CellForeColorSource.RowCell;
            this.gridColumnLastDownload.HeaderText = "Last Download";
            this.gridColumnLastDownload.Width = 221;
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(529, 198);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 5;
            this.cancel.Text = "Close";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // addStore
            // 
            this.addStore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addStore.Image = global::ShipWorks.Properties.Resources.store_add_16;
            this.addStore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addStore.Location = new System.Drawing.Point(454, 136);
            this.addStore.Name = "addStore";
            this.addStore.Size = new System.Drawing.Size(150, 23);
            this.addStore.TabIndex = 4;
            this.addStore.Text = "Add Store";
            this.addStore.UseVisualStyleBackColor = true;
            this.addStore.Click += new System.EventHandler(this.OnAddStore);
            // 
            // rename
            // 
            this.rename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rename.Image = global::ShipWorks.Properties.Resources.rename;
            this.rename.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rename.Location = new System.Drawing.Point(454, 56);
            this.rename.Name = "rename";
            this.rename.Size = new System.Drawing.Size(150, 23);
            this.rename.TabIndex = 2;
            this.rename.Text = "Rename";
            this.rename.UseVisualStyleBackColor = true;
            this.rename.Click += new System.EventHandler(this.OnRename);
            // 
            // delete
            // 
            this.delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.delete.Image = global::ShipWorks.Properties.Resources.delete16;
            this.delete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.delete.Location = new System.Drawing.Point(454, 85);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(150, 23);
            this.delete.TabIndex = 3;
            this.delete.Text = "Delete";
            this.delete.UseVisualStyleBackColor = true;
            this.delete.Click += new System.EventHandler(this.OnDelete);
            // 
            // edit
            // 
            this.edit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.edit.Image = global::ShipWorks.Properties.Resources.edit16;
            this.edit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.edit.Location = new System.Drawing.Point(454, 26);
            this.edit.Name = "edit";
            this.edit.Size = new System.Drawing.Size(150, 23);
            this.edit.TabIndex = 1;
            this.edit.Text = "Edit";
            this.edit.UseVisualStyleBackColor = true;
            this.edit.Click += new System.EventHandler(this.OnEditStore);
            // 
            // showDisabledStores
            // 
            this.showDisabledStores.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.showDisabledStores.AutoSize = true;
            this.showDisabledStores.Location = new System.Drawing.Point(7, 204);
            this.showDisabledStores.Name = "showDisabledStores";
            this.showDisabledStores.Size = new System.Drawing.Size(127, 17);
            this.showDisabledStores.TabIndex = 21;
            this.showDisabledStores.Text = "Show disabled stores";
            this.showDisabledStores.UseVisualStyleBackColor = true;
            this.showDisabledStores.CheckedChanged += new System.EventHandler(this.OnChangeShowDisabledStores);
            // 
            // labelDisabledCount
            // 
            this.labelDisabledCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelDisabledCount.AutoSize = true;
            this.labelDisabledCount.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelDisabledCount.Location = new System.Drawing.Point(130, 205);
            this.labelDisabledCount.Name = "labelDisabledCount";
            this.labelDisabledCount.Size = new System.Drawing.Size(102, 13);
            this.labelDisabledCount.TabIndex = 22;
            this.labelDisabledCount.Text = "(12 disabled stores)";
            // 
            // StoreManagerDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(616, 233);
            this.Controls.Add(this.labelDisabledCount);
            this.Controls.Add(this.showDisabledStores);
            this.Controls.Add(this.sandGrid);
            this.Controls.Add(this.labelAdd);
            this.Controls.Add(this.addStore);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.rename);
            this.Controls.Add(this.delete);
            this.Controls.Add(this.edit);
            this.Controls.Add(this.cancel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(350, 262);
            this.Name = "StoreManagerDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manage Stores";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button rename;
        private System.Windows.Forms.Button delete;
        private System.Windows.Forms.Button edit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelAdd;
        private System.Windows.Forms.Button addStore;
        private Divelements.SandGrid.SandGrid sandGrid;
        private Divelements.SandGrid.GridColumn gridColumnName;
        private Divelements.SandGrid.GridColumn gridColumnStoreType;
        private Divelements.SandGrid.GridColumn gridColumnLastDownload;
        private Editions.EditionGuiHelper editionGuiHelper;
        private System.Windows.Forms.CheckBox showDisabledStores;
        private System.Windows.Forms.Label labelDisabledCount;
    }
}