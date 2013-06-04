namespace ShipWorks.Common.Threading
{
    partial class ProgressDlg
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
            Divelements.SandGrid.GridCell gridCell1 = new Divelements.SandGrid.GridCell();
            Divelements.SandGrid.GridCell gridCell2 = new Divelements.SandGrid.GridCell();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressDlg));
            this.sep = new System.Windows.Forms.Label();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.description = new System.Windows.Forms.Label();
            this.headerImage = new System.Windows.Forms.PictureBox();
            this.title = new System.Windows.Forms.Label();
            this.progressGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnImage = new Divelements.SandGrid.GridColumn();
            this.gridColumnAction = new Divelements.SandGrid.GridColumn();
            this.gridColumnDetail = new Divelements.SandGrid.GridColumn();
            this.gridColumnProgress = new Divelements.SandGrid.Specialized.GridProgressBarColumn();
            this.contextMenuCopyError = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancel = new System.Windows.Forms.Button();
            this.ok = new System.Windows.Forms.Button();
            this.uiUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.headerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.headerImage)).BeginInit();
            this.contextMenuCopyError.SuspendLayout();
            this.SuspendLayout();
            // 
            // sep
            // 
            this.sep.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.sep.Dock = System.Windows.Forms.DockStyle.Top;
            this.sep.Location = new System.Drawing.Point(0, 51);
            this.sep.Name = "sep";
            this.sep.Size = new System.Drawing.Size(486, 3);
            this.sep.TabIndex = 3;
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.White;
            this.headerPanel.Controls.Add(this.description);
            this.headerPanel.Controls.Add(this.headerImage);
            this.headerPanel.Controls.Add(this.title);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(486, 51);
            this.headerPanel.TabIndex = 4;
            // 
            // description
            // 
            this.description.AutoSize = true;
            this.description.Location = new System.Drawing.Point(28, 27);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(245, 13);
            this.description.TabIndex = 1;
            this.description.Text = "ShipWorks is creating a backup of your database.";
            // 
            // headerImage
            // 
            this.headerImage.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.headerImage.Cursor = System.Windows.Forms.Cursors.Default;
            this.headerImage.Image = global::ShipWorks.Properties.Resources.squares_circle_green;
            this.headerImage.Location = new System.Drawing.Point(441, 9);
            this.headerImage.Name = "headerImage";
            this.headerImage.Size = new System.Drawing.Size(31, 31);
            this.headerImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.headerImage.TabIndex = 5;
            this.headerImage.TabStop = false;
            // 
            // title
            // 
            this.title.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.title.Location = new System.Drawing.Point(12, 9);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(296, 14);
            this.title.TabIndex = 0;
            this.title.Text = "Backup Progress";
            // 
            // progressGrid
            // 
            this.progressGrid.AllowMultipleSelection = false;
            this.progressGrid.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressGrid.ColumnClickBehavior = Divelements.SandGrid.ColumnClickBehavior.None;
            this.progressGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnImage,
            this.gridColumnAction,
            this.gridColumnDetail,
            this.gridColumnProgress});
            this.progressGrid.ContextMenuStrip = this.contextMenuCopyError;
            this.progressGrid.GridLines = Divelements.SandGrid.GridLinesDisplayType.Both;
            this.progressGrid.ImageTextSeparation = 1;
            this.progressGrid.LiveResize = false;
            this.progressGrid.Location = new System.Drawing.Point(12, 63);
            this.progressGrid.Name = "progressGrid";
            this.progressGrid.NullRepresentation = "";
            this.progressGrid.Renderer = windowsXPRenderer1;
            this.progressGrid.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.None;
            this.progressGrid.RowHighlightType = Divelements.SandGrid.RowHighlightType.None;
            gridCell1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            gridCell1.ForeColor = System.Drawing.Color.Green;
            gridCell1.Text = "Success";
            gridCell2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            gridCell2.Text = "Compress Backup";
            this.progressGrid.Rows.AddRange(new Divelements.SandGrid.GridRow[] {
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell(global::ShipWorks.Properties.Resources.arrow_right_green),
                        new Divelements.SandGrid.GridCell("Create SQL Server Backup"),
                        gridCell1,
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridIntegerCell(100)))}),
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell(global::ShipWorks.Properties.Resources.arrow_right_green),
                        gridCell2,
                        new Divelements.SandGrid.GridCell("12,900 of 788,809 kb\r\nBaggins"),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridIntegerCell(25)))}),
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell(global::ShipWorks.Properties.Resources.cancel16),
                        new Divelements.SandGrid.GridCell("Verify Contents"),
                        new Divelements.SandGrid.GridCell(),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridIntegerCell()))})});
            this.progressGrid.Size = new System.Drawing.Size(462, 174);
            this.progressGrid.TabIndex = 2;
            // 
            // gridColumnImage
            // 
            this.gridColumnImage.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnImage.Clickable = false;
            this.gridColumnImage.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnImage.MinimumWidth = 25;
            this.gridColumnImage.Width = 25;
            // 
            // gridColumnAction
            // 
            this.gridColumnAction.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnAction.Clickable = false;
            this.gridColumnAction.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnAction.HeaderText = "Action";
            this.gridColumnAction.MinimumWidth = 50;
            this.gridColumnAction.Width = 135;
            // 
            // gridColumnDetail
            // 
            this.gridColumnDetail.AllowWrap = true;
            this.gridColumnDetail.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnDetail.Clickable = false;
            this.gridColumnDetail.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnDetail.ForeColorSource = Divelements.SandGrid.CellForeColorSource.RowCell;
            this.gridColumnDetail.HeaderText = "Detail";
            this.gridColumnDetail.MinimumWidth = 50;
            this.gridColumnDetail.Width = 173;
            // 
            // gridColumnProgress
            // 
            this.gridColumnProgress.Clickable = false;
            this.gridColumnProgress.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnProgress.HeaderText = "Progress";
            this.gridColumnProgress.Width = 125;
            // 
            // contextMenuCopyError
            // 
            this.contextMenuCopyError.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.contextMenuCopyError.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.contextMenuCopyError.Name = "contextMenuCopyError";
            this.contextMenuCopyError.Size = new System.Drawing.Size(101, 26);
            this.contextMenuCopyError.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextMenuOpening);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.OnCopyErrorMessage);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(397, 243);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.OnCancel);
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Enabled = false;
            this.ok.Location = new System.Drawing.Point(316, 243);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 0;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnClose);
            // 
            // uiUpdateTimer
            // 
            this.uiUpdateTimer.Interval = 50;
            this.uiUpdateTimer.Tick += new System.EventHandler(this.OnUpdateTimer);
            // 
            // ProgressDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(486, 278);
            this.ControlBox = false;
            this.Controls.Add(this.progressGrid);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.sep);
            this.Controls.Add(this.headerPanel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(492, 281);
            this.Name = "ProgressDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.headerImage)).EndInit();
            this.contextMenuCopyError.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label sep;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.PictureBox headerImage;
        private System.Windows.Forms.Label title;
        private Divelements.SandGrid.SandGrid progressGrid;
        private System.Windows.Forms.Button cancel;
        private Divelements.SandGrid.GridColumn gridColumnAction;
        private Divelements.SandGrid.Specialized.GridProgressBarColumn gridColumnProgress;
        private Divelements.SandGrid.GridColumn gridColumnImage;
        private Divelements.SandGrid.GridColumn gridColumnDetail;
        private System.Windows.Forms.Label description;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.ContextMenuStrip contextMenuCopyError;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.Timer uiUpdateTimer;
    }
}