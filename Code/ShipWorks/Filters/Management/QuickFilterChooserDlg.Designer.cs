namespace ShipWorks.Filters.Management
{
    partial class QuickFilterChooserDlg
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
            this.components = new System.ComponentModel.Container();
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer1 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.labelFiltersFor = new System.Windows.Forms.Label();
            this.filtersFor = new System.Windows.Forms.ComboBox();
            this.edit = new System.Windows.Forms.Button();
            this.newFilter = new System.Windows.Forms.Button();
            this.sandGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnFilter = new ShipWorks.Filters.Controls.FilterTreeGridColumn();
            this.gridColumnUsedBy = new Divelements.SandGrid.GridColumn();
            this.filterCountTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(440, 376);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 0;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(521, 376);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // labelFiltersFor
            // 
            this.labelFiltersFor.AutoSize = true;
            this.labelFiltersFor.Location = new System.Drawing.Point(12, 13);
            this.labelFiltersFor.Name = "labelFiltersFor";
            this.labelFiltersFor.Size = new System.Drawing.Size(84, 13);
            this.labelFiltersFor.TabIndex = 3;
            this.labelFiltersFor.Text = "Show filters for:";
            // 
            // filtersFor
            // 
            this.filtersFor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filtersFor.FormattingEnabled = true;
            this.filtersFor.Location = new System.Drawing.Point(98, 10);
            this.filtersFor.Name = "filtersFor";
            this.filtersFor.Size = new System.Drawing.Size(142, 21);
            this.filtersFor.TabIndex = 4;
            this.filtersFor.SelectedIndexChanged += new System.EventHandler(this.OnChangeFilterTarget);
            // 
            // edit
            // 
            this.edit.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.edit.Image = global::ShipWorks.Properties.Resources.edit16;
            this.edit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.edit.Location = new System.Drawing.Point(505, 37);
            this.edit.Name = "edit";
            this.edit.Size = new System.Drawing.Size(91, 23);
            this.edit.TabIndex = 7;
            this.edit.Text = "Edit";
            this.edit.UseVisualStyleBackColor = true;
            this.edit.Click += new System.EventHandler(this.OnEdit);
            // 
            // newFilter
            // 
            this.newFilter.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newFilter.Image = global::ShipWorks.Properties.Resources.filter_add;
            this.newFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.newFilter.Location = new System.Drawing.Point(505, 66);
            this.newFilter.Name = "newFilter";
            this.newFilter.Size = new System.Drawing.Size(91, 23);
            this.newFilter.TabIndex = 8;
            this.newFilter.Text = "Create";
            this.newFilter.UseVisualStyleBackColor = true;
            this.newFilter.Click += new System.EventHandler(this.OnCreate);
            // 
            // sandGrid
            // 
            this.sandGrid.AllowMultipleSelection = false;
            this.sandGrid.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sandGrid.ColumnClickBehavior = Divelements.SandGrid.ColumnClickBehavior.None;
            this.sandGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnFilter,
            this.gridColumnUsedBy});
            this.sandGrid.EnableSearching = false;
            this.sandGrid.GridLines = Divelements.SandGrid.GridLinesDisplayType.Both;
            this.sandGrid.HighlightImages = false;
            this.sandGrid.Location = new System.Drawing.Point(12, 37);
            this.sandGrid.Name = "sandGrid";
            this.sandGrid.Renderer = windowsXPRenderer1;
            this.sandGrid.RowHighlightType = Divelements.SandGrid.RowHighlightType.Full;
            this.sandGrid.ShadeAlternateRows = true;
            this.sandGrid.Size = new System.Drawing.Size(484, 322);
            this.sandGrid.TabIndex = 2;
            this.sandGrid.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnSelectionChanged);
            this.sandGrid.RowActivated += new Divelements.SandGrid.GridRowEventHandler(this.OnRowActivated);
            // 
            // gridColumnFilter
            // 
            this.gridColumnFilter.AllowReorder = false;
            this.gridColumnFilter.Clickable = false;
            this.gridColumnFilter.ClipText = true;
            this.gridColumnFilter.HeaderText = "Filter Name";
            this.gridColumnFilter.MinimumWidth = 100;
            this.gridColumnFilter.Width = 174;
            // 
            // gridColumnUsedBy
            // 
            this.gridColumnUsedBy.AllowReorder = false;
            this.gridColumnUsedBy.AllowWrap = true;
            this.gridColumnUsedBy.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnUsedBy.Clickable = false;
            this.gridColumnUsedBy.HeaderText = "Used By";
            this.gridColumnUsedBy.Width = 306;
            // 
            // filterCountTimer
            // 
            this.filterCountTimer.Interval = 1000;
            this.filterCountTimer.Tick += new System.EventHandler(this.OnRefreshFilterCountTimer);
            // 
            // QuickFilterChooserDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(608, 411);
            this.Controls.Add(this.edit);
            this.Controls.Add(this.newFilter);
            this.Controls.Add(this.filtersFor);
            this.Controls.Add(this.labelFiltersFor);
            this.Controls.Add(this.sandGrid);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QuickFilterChooserDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Quick Filter";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private Divelements.SandGrid.SandGrid sandGrid;
        private System.Windows.Forms.Label labelFiltersFor;
        private System.Windows.Forms.ComboBox filtersFor;
        private ShipWorks.Filters.Controls.FilterTreeGridColumn gridColumnFilter;
        private Divelements.SandGrid.GridColumn gridColumnUsedBy;
        private System.Windows.Forms.Button edit;
        private System.Windows.Forms.Button newFilter;
        private System.Windows.Forms.Timer filterCountTimer;
    }
}