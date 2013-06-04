namespace ShipWorks.Stores.Communication
{
    partial class DownloadLogDlg
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
            ShipWorks.UI.Controls.SandGrid.WindowsXPShipWorksRenderer windowsXPShipWorksRenderer1 = new ShipWorks.UI.Controls.SandGrid.WindowsXPShipWorksRenderer();
            this.close = new System.Windows.Forms.Button();
            this.panelGridArea = new System.Windows.Forms.Panel();
            this.entityGrid = new ShipWorks.Data.Grid.Paging.PagedEntityGrid();
            this.gridColumn1 = new Divelements.SandGrid.GridColumn();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.panelTools = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.labelGridSettings = new System.Windows.Forms.Label();
            this.labelFilterInitiatedBy = new System.Windows.Forms.Label();
            this.filterInitiatedBy = new System.Windows.Forms.Label();
            this.filterResult = new System.Windows.Forms.Label();
            this.labelFilterResult = new System.Windows.Forms.Label();
            this.filterStore = new System.Windows.Forms.Label();
            this.labelFilterStore = new System.Windows.Forms.Label();
            this.menuList = new ShipWorks.UI.Controls.MenuList();
            this.panelGridArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.panelTools)).BeginInit();
            this.panelTools.SuspendLayout();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(620, 418);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 0;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // panelGridArea
            // 
            this.panelGridArea.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelGridArea.BackColor = System.Drawing.Color.White;
            this.panelGridArea.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelGridArea.Controls.Add(this.entityGrid);
            this.panelGridArea.Controls.Add(this.kryptonBorderEdge);
            this.panelGridArea.Controls.Add(this.panelTools);
            this.panelGridArea.Location = new System.Drawing.Point(111, 12);
            this.panelGridArea.Name = "panelGridArea";
            this.panelGridArea.Size = new System.Drawing.Size(584, 400);
            this.panelGridArea.TabIndex = 3;
            // 
            // entityGrid
            // 
            this.entityGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.entityGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumn1});
            this.entityGrid.DetailViewSettings = null;
            this.entityGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entityGrid.EnableSearching = false;
            this.entityGrid.GridLines = Divelements.SandGrid.GridLinesDisplayType.Both;
            this.entityGrid.LiveResize = false;
            this.entityGrid.Location = new System.Drawing.Point(0, 0);
            this.entityGrid.Name = "entityGrid";
            this.entityGrid.NullRepresentation = "";
            this.entityGrid.Renderer = windowsXPShipWorksRenderer1;
            this.entityGrid.RowHighlightType = Divelements.SandGrid.RowHighlightType.Full;
            this.entityGrid.ShadeAlternateRows = true;
            this.entityGrid.Size = new System.Drawing.Size(580, 369);
            this.entityGrid.StretchPrimaryGrid = false;
            this.entityGrid.TabIndex = 3;
            // 
            // gridColumn1
            // 
            this.gridColumn1.DataPropertyName = "Started";
            this.gridColumn1.HeaderText = "Started";
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(0, 369);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(580, 1);
            this.kryptonBorderEdge.TabIndex = 2;
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            // 
            // panelTools
            // 
            this.panelTools.Controls.Add(this.labelGridSettings);
            this.panelTools.Controls.Add(this.labelFilterInitiatedBy);
            this.panelTools.Controls.Add(this.filterInitiatedBy);
            this.panelTools.Controls.Add(this.filterResult);
            this.panelTools.Controls.Add(this.labelFilterResult);
            this.panelTools.Controls.Add(this.filterStore);
            this.panelTools.Controls.Add(this.labelFilterStore);
            this.panelTools.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelTools.Location = new System.Drawing.Point(0, 370);
            this.panelTools.Name = "panelTools";
            this.panelTools.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridHeaderColumnSheet;
            this.panelTools.Size = new System.Drawing.Size(580, 26);
            this.panelTools.TabIndex = 1;
            // 
            // labelGridSettings
            // 
            this.labelGridSettings.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelGridSettings.AutoSize = true;
            this.labelGridSettings.BackColor = System.Drawing.Color.Transparent;
            this.labelGridSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelGridSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelGridSettings.ForeColor = System.Drawing.Color.Blue;
            this.labelGridSettings.Location = new System.Drawing.Point(507, 6);
            this.labelGridSettings.Name = "labelGridSettings";
            this.labelGridSettings.Size = new System.Drawing.Size(68, 13);
            this.labelGridSettings.TabIndex = 12;
            this.labelGridSettings.Text = "Grid Settings";
            this.labelGridSettings.Click += new System.EventHandler(this.OnGridSettings);
            // 
            // labelFilterInitiatedBy
            // 
            this.labelFilterInitiatedBy.AutoSize = true;
            this.labelFilterInitiatedBy.BackColor = System.Drawing.Color.Transparent;
            this.labelFilterInitiatedBy.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (64)))), ((int) (((byte) (64)))), ((int) (((byte) (64)))));
            this.labelFilterInitiatedBy.Location = new System.Drawing.Point(143, 6);
            this.labelFilterInitiatedBy.Name = "labelFilterInitiatedBy";
            this.labelFilterInitiatedBy.Size = new System.Drawing.Size(66, 13);
            this.labelFilterInitiatedBy.TabIndex = 11;
            this.labelFilterInitiatedBy.Text = "Initiated By:";
            // 
            // filterInitiatedBy
            // 
            this.filterInitiatedBy.AutoSize = true;
            this.filterInitiatedBy.BackColor = System.Drawing.Color.Transparent;
            this.filterInitiatedBy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.filterInitiatedBy.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.filterInitiatedBy.ForeColor = System.Drawing.Color.Blue;
            this.filterInitiatedBy.Location = new System.Drawing.Point(207, 6);
            this.filterInitiatedBy.Name = "filterInitiatedBy";
            this.filterInitiatedBy.Size = new System.Drawing.Size(26, 13);
            this.filterInitiatedBy.TabIndex = 10;
            this.filterInitiatedBy.Text = "Any";
            this.filterInitiatedBy.Click += new System.EventHandler(this.OnClickReasonFilter);
            // 
            // filterResult
            // 
            this.filterResult.AutoSize = true;
            this.filterResult.BackColor = System.Drawing.Color.Transparent;
            this.filterResult.Cursor = System.Windows.Forms.Cursors.Hand;
            this.filterResult.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.filterResult.ForeColor = System.Drawing.Color.Blue;
            this.filterResult.Location = new System.Drawing.Point(46, 6);
            this.filterResult.Name = "filterResult";
            this.filterResult.Size = new System.Drawing.Size(26, 13);
            this.filterResult.TabIndex = 9;
            this.filterResult.Text = "Any";
            this.filterResult.Click += new System.EventHandler(this.OnClickResultFilter);
            // 
            // labelFilterResult
            // 
            this.labelFilterResult.AutoSize = true;
            this.labelFilterResult.BackColor = System.Drawing.Color.Transparent;
            this.labelFilterResult.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (64)))), ((int) (((byte) (64)))), ((int) (((byte) (64)))));
            this.labelFilterResult.Location = new System.Drawing.Point(8, 6);
            this.labelFilterResult.Name = "labelFilterResult";
            this.labelFilterResult.Size = new System.Drawing.Size(41, 13);
            this.labelFilterResult.TabIndex = 8;
            this.labelFilterResult.Text = "Result:";
            // 
            // filterStore
            // 
            this.filterStore.AutoSize = true;
            this.filterStore.BackColor = System.Drawing.Color.Transparent;
            this.filterStore.Cursor = System.Windows.Forms.Cursors.Hand;
            this.filterStore.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.filterStore.ForeColor = System.Drawing.Color.Blue;
            this.filterStore.Location = new System.Drawing.Point(111, 6);
            this.filterStore.Name = "filterStore";
            this.filterStore.Size = new System.Drawing.Size(26, 13);
            this.filterStore.TabIndex = 7;
            this.filterStore.Text = "Any";
            this.filterStore.Click += new System.EventHandler(this.OnClickStoreFilter);
            // 
            // labelFilterStore
            // 
            this.labelFilterStore.AutoSize = true;
            this.labelFilterStore.BackColor = System.Drawing.Color.Transparent;
            this.labelFilterStore.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (64)))), ((int) (((byte) (64)))), ((int) (((byte) (64)))));
            this.labelFilterStore.Location = new System.Drawing.Point(77, 6);
            this.labelFilterStore.Name = "labelFilterStore";
            this.labelFilterStore.Size = new System.Drawing.Size(37, 13);
            this.labelFilterStore.TabIndex = 4;
            this.labelFilterStore.Text = "Store:";
            // 
            // menuList
            // 
            this.menuList.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.menuList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.menuList.FormattingEnabled = true;
            this.menuList.IntegralHeight = false;
            this.menuList.ItemHeight = 26;
            this.menuList.Items.AddRange(new object[] {
            "Today",
            "Last 7 Days",
            "Last 30 Days",
            "All"});
            this.menuList.Location = new System.Drawing.Point(12, 12);
            this.menuList.Name = "menuList";
            this.menuList.Size = new System.Drawing.Size(93, 400);
            this.menuList.TabIndex = 2;
            this.menuList.SelectedIndexChanged += new System.EventHandler(this.OnChangeQueryDateFilter);
            // 
            // DownloadLogDlg
            // 
            this.AcceptButton = this.close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(707, 453);
            this.Controls.Add(this.panelGridArea);
            this.Controls.Add(this.menuList);
            this.Controls.Add(this.close);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DownloadLogDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Download Log";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnLoad);
            this.panelGridArea.ResumeLayout(false);
            this.panelGridArea.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.panelTools)).EndInit();
            this.panelTools.ResumeLayout(false);
            this.panelTools.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button close;
        private ShipWorks.UI.Controls.MenuList menuList;
        private System.Windows.Forms.Panel panelGridArea;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel panelTools;
        private System.Windows.Forms.Label filterStore;
        private System.Windows.Forms.Label labelFilterStore;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        private System.Windows.Forms.Label filterResult;
        private System.Windows.Forms.Label labelFilterResult;
        private System.Windows.Forms.Label labelFilterInitiatedBy;
        private System.Windows.Forms.Label filterInitiatedBy;
        private ShipWorks.Data.Grid.Paging.PagedEntityGrid entityGrid;
        private Divelements.SandGrid.GridColumn gridColumn1;
        private System.Windows.Forms.Label labelGridSettings;
    }
}