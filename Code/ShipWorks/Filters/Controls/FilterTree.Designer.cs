using Interapptive.Shared.Messaging;

namespace ShipWorks.Filters.Controls
{
    partial class FilterTree
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

                filterEditedToken?.Dispose();

                quickFilterDisplayManager.Dispose();
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
            this.components = new System.ComponentModel.Container();
            ShipWorks.UI.Controls.SandGrid.SandGridThemedSelectionRenderer sandGridThemedSelectionRenderer1 = new ShipWorks.UI.Controls.SandGrid.SandGridThemedSelectionRenderer();
            this.filterCountTimer = new System.Windows.Forms.Timer(this.components);
            this.panelQuickFilter = new System.Windows.Forms.Panel();
            this.quickFilterDivider = new System.Windows.Forms.Panel();
            this.labelAllFilters = new System.Windows.Forms.Label();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.quickFilterBackground = new System.Windows.Forms.Panel();
            this.quickFilterChoose = new System.Windows.Forms.Label();
            this.quickFilterCalculating = new System.Windows.Forms.Panel();
            this.quickFilterSpinner = new System.Windows.Forms.PictureBox();
            this.quickFilterCountLeftParen = new System.Windows.Forms.Label();
            this.quickFilterCountRightParen = new System.Windows.Forms.Label();
            this.quickFilterCount = new System.Windows.Forms.Label();
            this.quickFilterName = new System.Windows.Forms.Label();
            this.quickFilterEdit = new System.Windows.Forms.Label();
            this.quickFilterPicture = new System.Windows.Forms.PictureBox();
            this.labelQuick = new System.Windows.Forms.Label();
            this.sandGrid = new ShipWorks.UI.Controls.SandGrid.SandGridTree();
            this.gridColumnFilterNode = new ShipWorks.Filters.Controls.FilterTreeGridColumn();
            this.gridColumnFill = new Divelements.SandGrid.GridColumn();
            this.panelQuickFilter.SuspendLayout();
            this.quickFilterDivider.SuspendLayout();
            this.quickFilterBackground.SuspendLayout();
            this.quickFilterCalculating.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.quickFilterSpinner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.quickFilterPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sandGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // filterCountTimer
            // 
            this.filterCountTimer.Interval = 1000;
            this.filterCountTimer.Tick += new System.EventHandler(this.OnRefreshFilterCountTimer);
            // 
            // panelQuickFilter
            // 
            this.panelQuickFilter.BackColor = System.Drawing.Color.White;
            this.panelQuickFilter.Controls.Add(this.quickFilterDivider);
            this.panelQuickFilter.Controls.Add(this.quickFilterBackground);
            this.panelQuickFilter.Controls.Add(this.labelQuick);
            this.panelQuickFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelQuickFilter.Location = new System.Drawing.Point(0, 0);
            this.panelQuickFilter.Name = "panelQuickFilter";
            this.panelQuickFilter.Size = new System.Drawing.Size(239, 62);
            this.panelQuickFilter.TabIndex = 0;
            this.panelQuickFilter.Visible = false;
            // 
            // quickFilterDivider
            // 
            this.quickFilterDivider.Controls.Add(this.labelAllFilters);
            this.quickFilterDivider.Controls.Add(this.kryptonBorderEdge);
            this.quickFilterDivider.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.quickFilterDivider.Location = new System.Drawing.Point(0, 38);
            this.quickFilterDivider.Name = "quickFilterDivider";
            this.quickFilterDivider.Size = new System.Drawing.Size(239, 24);
            this.quickFilterDivider.TabIndex = 13;
            // 
            // labelAllFilters
            // 
            this.labelAllFilters.AutoSize = true;
            this.labelAllFilters.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAllFilters.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.labelAllFilters.Location = new System.Drawing.Point(3, 5);
            this.labelAllFilters.Name = "labelAllFilters";
            this.labelAllFilters.Size = new System.Drawing.Size(59, 13);
            this.labelAllFilters.TabIndex = 10;
            this.labelAllFilters.Text = "All Filters";
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonBorderEdge.AutoSize = false;
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(0, 1);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(320, 1);
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            // 
            // quickFilterBackground
            // 
            this.quickFilterBackground.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.quickFilterBackground.BackColor = System.Drawing.Color.Transparent;
            this.quickFilterBackground.Controls.Add(this.quickFilterChoose);
            this.quickFilterBackground.Controls.Add(this.quickFilterCalculating);
            this.quickFilterBackground.Controls.Add(this.quickFilterCount);
            this.quickFilterBackground.Controls.Add(this.quickFilterName);
            this.quickFilterBackground.Controls.Add(this.quickFilterEdit);
            this.quickFilterBackground.Controls.Add(this.quickFilterPicture);
            this.quickFilterBackground.Location = new System.Drawing.Point(0, 18);
            this.quickFilterBackground.Name = "quickFilterBackground";
            this.quickFilterBackground.Size = new System.Drawing.Size(239, 18);
            this.quickFilterBackground.TabIndex = 11;
            this.quickFilterBackground.Paint += new System.Windows.Forms.PaintEventHandler(this.OnQuickFilterPaintBackground);
            this.quickFilterBackground.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnQuickFilterMouseDown);
            this.quickFilterBackground.MouseEnter += new System.EventHandler(this.OnQuickFilterMouseEnter);
            this.quickFilterBackground.MouseLeave += new System.EventHandler(this.OnQuickFilterMouseLeave);
            this.quickFilterBackground.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnQuickFilterMouseUp);
            // 
            // quickFilterChoose
            // 
            this.quickFilterChoose.AutoSize = true;
            this.quickFilterChoose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.quickFilterChoose.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quickFilterChoose.ForeColor = System.Drawing.Color.Blue;
            this.quickFilterChoose.Location = new System.Drawing.Point(178, 2);
            this.quickFilterChoose.Name = "quickFilterChoose";
            this.quickFilterChoose.Size = new System.Drawing.Size(55, 13);
            this.quickFilterChoose.TabIndex = 11;
            this.quickFilterChoose.Text = "Choose...";
            this.quickFilterChoose.Click += new System.EventHandler(this.OnQuickFilterChoose);
            // 
            // quickFilterCalculating
            // 
            this.quickFilterCalculating.Controls.Add(this.quickFilterSpinner);
            this.quickFilterCalculating.Controls.Add(this.quickFilterCountLeftParen);
            this.quickFilterCalculating.Controls.Add(this.quickFilterCountRightParen);
            this.quickFilterCalculating.Location = new System.Drawing.Point(208, 0);
            this.quickFilterCalculating.Name = "quickFilterCalculating";
            this.quickFilterCalculating.Size = new System.Drawing.Size(26, 18);
            this.quickFilterCalculating.TabIndex = 10;
            this.quickFilterCalculating.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnQuickFilterMouseDown);
            this.quickFilterCalculating.MouseEnter += new System.EventHandler(this.OnQuickFilterMouseEnter);
            this.quickFilterCalculating.MouseLeave += new System.EventHandler(this.OnQuickFilterMouseLeave);
            this.quickFilterCalculating.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnQuickFilterMouseUp);
            // 
            // quickFilterSpinner
            // 
            this.quickFilterSpinner.Image = global::ShipWorks.Properties.Resources.arrows_blue;
            this.quickFilterSpinner.Location = new System.Drawing.Point(7, 4);
            this.quickFilterSpinner.Name = "quickFilterSpinner";
            this.quickFilterSpinner.Size = new System.Drawing.Size(10, 10);
            this.quickFilterSpinner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.quickFilterSpinner.TabIndex = 9;
            this.quickFilterSpinner.TabStop = false;
            this.quickFilterSpinner.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnQuickFilterMouseDown);
            this.quickFilterSpinner.MouseEnter += new System.EventHandler(this.OnQuickFilterMouseEnter);
            this.quickFilterSpinner.MouseLeave += new System.EventHandler(this.OnQuickFilterMouseLeave);
            this.quickFilterSpinner.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnQuickFilterMouseUp);
            // 
            // quickFilterCountLeftParen
            // 
            this.quickFilterCountLeftParen.AutoSize = true;
            this.quickFilterCountLeftParen.ForeColor = System.Drawing.Color.Blue;
            this.quickFilterCountLeftParen.Location = new System.Drawing.Point(0, 2);
            this.quickFilterCountLeftParen.Name = "quickFilterCountLeftParen";
            this.quickFilterCountLeftParen.Size = new System.Drawing.Size(11, 13);
            this.quickFilterCountLeftParen.TabIndex = 0;
            this.quickFilterCountLeftParen.Text = "(";
            this.quickFilterCountLeftParen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnQuickFilterMouseDown);
            this.quickFilterCountLeftParen.MouseEnter += new System.EventHandler(this.OnQuickFilterMouseEnter);
            this.quickFilterCountLeftParen.MouseLeave += new System.EventHandler(this.OnQuickFilterMouseLeave);
            this.quickFilterCountLeftParen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnQuickFilterMouseUp);
            // 
            // quickFilterCountRightParen
            // 
            this.quickFilterCountRightParen.AutoSize = true;
            this.quickFilterCountRightParen.ForeColor = System.Drawing.Color.Blue;
            this.quickFilterCountRightParen.Location = new System.Drawing.Point(15, 2);
            this.quickFilterCountRightParen.Name = "quickFilterCountRightParen";
            this.quickFilterCountRightParen.Size = new System.Drawing.Size(11, 13);
            this.quickFilterCountRightParen.TabIndex = 1;
            this.quickFilterCountRightParen.Text = ")";
            this.quickFilterCountRightParen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnQuickFilterMouseDown);
            this.quickFilterCountRightParen.MouseEnter += new System.EventHandler(this.OnQuickFilterMouseEnter);
            this.quickFilterCountRightParen.MouseLeave += new System.EventHandler(this.OnQuickFilterMouseLeave);
            this.quickFilterCountRightParen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnQuickFilterMouseUp);
            // 
            // quickFilterCount
            // 
            this.quickFilterCount.AutoSize = true;
            this.quickFilterCount.ForeColor = System.Drawing.Color.Blue;
            this.quickFilterCount.Location = new System.Drawing.Point(121, 2);
            this.quickFilterCount.Name = "quickFilterCount";
            this.quickFilterCount.Size = new System.Drawing.Size(33, 13);
            this.quickFilterCount.TabIndex = 8;
            this.quickFilterCount.Text = "(167)";
            this.quickFilterCount.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnQuickFilterMouseDown);
            this.quickFilterCount.MouseEnter += new System.EventHandler(this.OnQuickFilterMouseEnter);
            this.quickFilterCount.MouseLeave += new System.EventHandler(this.OnQuickFilterMouseLeave);
            this.quickFilterCount.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnQuickFilterMouseUp);
            // 
            // quickFilterName
            // 
            this.quickFilterName.AutoSize = true;
            this.quickFilterName.Location = new System.Drawing.Point(30, 2);
            this.quickFilterName.Name = "quickFilterName";
            this.quickFilterName.Size = new System.Drawing.Size(94, 13);
            this.quickFilterName.TabIndex = 4;
            this.quickFilterName.Text = "My awesome filter";
            this.quickFilterName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnQuickFilterMouseDown);
            this.quickFilterName.MouseEnter += new System.EventHandler(this.OnQuickFilterMouseEnter);
            this.quickFilterName.MouseLeave += new System.EventHandler(this.OnQuickFilterMouseLeave);
            this.quickFilterName.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnQuickFilterMouseUp);
            // 
            // quickFilterEdit
            // 
            this.quickFilterEdit.AutoSize = true;
            this.quickFilterEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.quickFilterEdit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quickFilterEdit.ForeColor = System.Drawing.Color.Blue;
            this.quickFilterEdit.Location = new System.Drawing.Point(155, 2);
            this.quickFilterEdit.Name = "quickFilterEdit";
            this.quickFilterEdit.Size = new System.Drawing.Size(25, 13);
            this.quickFilterEdit.TabIndex = 5;
            this.quickFilterEdit.Text = "Edit";
            this.quickFilterEdit.Click += new System.EventHandler(this.OnQuickFilterEditCreate);
            this.quickFilterEdit.MouseEnter += new System.EventHandler(this.OnQuickFilterMouseEnter);
            this.quickFilterEdit.MouseLeave += new System.EventHandler(this.OnQuickFilterMouseLeave);
            // 
            // quickFilterPicture
            // 
            this.quickFilterPicture.Image = global::ShipWorks.Properties.Resources.filter;
            this.quickFilterPicture.Location = new System.Drawing.Point(13, 1);
            this.quickFilterPicture.Name = "quickFilterPicture";
            this.quickFilterPicture.Size = new System.Drawing.Size(16, 16);
            this.quickFilterPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.quickFilterPicture.TabIndex = 7;
            this.quickFilterPicture.TabStop = false;
            this.quickFilterPicture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnQuickFilterMouseDown);
            this.quickFilterPicture.MouseEnter += new System.EventHandler(this.OnQuickFilterMouseEnter);
            this.quickFilterPicture.MouseLeave += new System.EventHandler(this.OnQuickFilterMouseLeave);
            this.quickFilterPicture.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnQuickFilterMouseUp);
            // 
            // labelQuick
            // 
            this.labelQuick.AutoSize = true;
            this.labelQuick.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelQuick.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.labelQuick.Location = new System.Drawing.Point(3, 3);
            this.labelQuick.Name = "labelQuick";
            this.labelQuick.Size = new System.Drawing.Size(70, 13);
            this.labelQuick.TabIndex = 9;
            this.labelQuick.Text = "Quick Filter";
            // 
            // sandGrid
            // 
            this.sandGrid.AllowDrop = true;
            this.sandGrid.AllowMultipleSelection = false;
            this.sandGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sandGrid.ColumnClickBehavior = Divelements.SandGrid.ColumnClickBehavior.None;
            this.sandGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnFilterNode,
            this.gridColumnFill});
            this.sandGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sandGrid.HighlightImages = false;
            this.sandGrid.ImageTextSeparation = 5;
            this.sandGrid.Location = new System.Drawing.Point(0, 62);
            this.sandGrid.Name = "sandGrid";
            this.sandGrid.NullRepresentation = "";
            this.sandGrid.Renderer = sandGridThemedSelectionRenderer1;
            this.sandGrid.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.InitiateDragDrop;
            this.sandGrid.RowHighlightType = Divelements.SandGrid.RowHighlightType.Full;
            this.sandGrid.ShowColumnHeaders = false;
            this.sandGrid.ShowTreeButtons = true;
            this.sandGrid.Size = new System.Drawing.Size(239, 253);
            this.sandGrid.TabIndex = 1;
            this.sandGrid.WhitespaceClickBehavior = Divelements.SandGrid.WhitespaceClickBehavior.None;
            this.sandGrid.GridRowDropped += new ShipWorks.UI.Controls.SandGrid.GridRowDroppedEventHandler(this.OnGridRowDropped);
            this.sandGrid.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnGridSelectionChanged);
            this.sandGrid.BeforeEdit += new Divelements.SandGrid.GridBeforeEditEventHandler(this.OnBeforeEdit);
            this.sandGrid.AfterEdit += new Divelements.SandGrid.GridAfterEditEventHandler(this.OnAfterEdit);
            this.sandGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.sandGrid.Leave += new System.EventHandler(this.OnLeave);
            // 
            // gridColumnFilterNode
            // 
            this.gridColumnFilterNode.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnFilterNode.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumnFilterNode.ForeColorSource = Divelements.SandGrid.CellForeColorSource.RowCell;
            this.gridColumnFilterNode.Width = 0;
            // 
            // gridColumnFill
            // 
            this.gridColumnFill.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnFill.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumnFill.Width = 239;
            // 
            // FilterTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.sandGrid);
            this.Controls.Add(this.panelQuickFilter);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FilterTree";
            this.Size = new System.Drawing.Size(239, 315);
            this.panelQuickFilter.ResumeLayout(false);
            this.panelQuickFilter.PerformLayout();
            this.quickFilterDivider.ResumeLayout(false);
            this.quickFilterDivider.PerformLayout();
            this.quickFilterBackground.ResumeLayout(false);
            this.quickFilterBackground.PerformLayout();
            this.quickFilterCalculating.ResumeLayout(false);
            this.quickFilterCalculating.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.quickFilterSpinner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.quickFilterPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sandGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Controls.SandGrid.SandGridTree sandGrid;
        private FilterTreeGridColumn gridColumnFilterNode;
        private Divelements.SandGrid.GridColumn gridColumnFill;
        private System.Windows.Forms.Timer filterCountTimer;
        private System.Windows.Forms.Panel panelQuickFilter;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        private System.Windows.Forms.Label quickFilterEdit;
        private System.Windows.Forms.Label quickFilterName;
        private System.Windows.Forms.PictureBox quickFilterPicture;
        private System.Windows.Forms.Label quickFilterCount;
        private System.Windows.Forms.Label labelQuick;
        private System.Windows.Forms.Label labelAllFilters;
        private System.Windows.Forms.Panel quickFilterBackground;
        private System.Windows.Forms.PictureBox quickFilterSpinner;
        private System.Windows.Forms.Panel quickFilterCalculating;
        private System.Windows.Forms.Label quickFilterCountLeftParen;
        private System.Windows.Forms.Label quickFilterCountRightParen;
        private System.Windows.Forms.Panel quickFilterDivider;
        private System.Windows.Forms.Label quickFilterChoose;
    }
}
