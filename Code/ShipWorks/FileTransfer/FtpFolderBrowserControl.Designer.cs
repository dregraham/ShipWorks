namespace ShipWorks.FileTransfer
{
    partial class FtpFolderBrowserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer1 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.sandGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnFilterNode = new Divelements.SandGrid.GridColumn();
            this.SuspendLayout();
            // 
            // sandGrid
            // 
            this.sandGrid.AllowMultipleSelection = false;
            this.sandGrid.ColumnClickBehavior = Divelements.SandGrid.ColumnClickBehavior.None;
            this.sandGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnFilterNode});
            this.sandGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sandGrid.EmptyText = "Loading...";
            this.sandGrid.EmptyTextForeColor = System.Drawing.SystemColors.GrayText;
            this.sandGrid.ImageTextSeparation = 5;
            this.sandGrid.Location = new System.Drawing.Point(0, 0);
            this.sandGrid.Name = "sandGrid";
            this.sandGrid.NullRepresentation = "";
            this.sandGrid.Renderer = windowsXPRenderer1;
            this.sandGrid.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.None;
            this.sandGrid.RowHighlightType = Divelements.SandGrid.RowHighlightType.PrimaryColumnOnly;
            this.sandGrid.ShowColumnHeaders = false;
            this.sandGrid.ShowTreeButtons = true;
            this.sandGrid.Size = new System.Drawing.Size(364, 343);
            this.sandGrid.TabIndex = 3;
            this.sandGrid.WhitespaceClickBehavior = Divelements.SandGrid.WhitespaceClickBehavior.None;
            this.sandGrid.BeforeExpand += new Divelements.SandGrid.GridRowExpandCollapseEventHandler(this.OnBeforeExpand);
            // 
            // gridColumnFilterNode
            // 
            this.gridColumnFilterNode.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnFilterNode.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnFilterNode.Width = 0;
            // 
            // FtpFolderBrowserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sandGrid);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "FtpFolderBrowserControl";
            this.Size = new System.Drawing.Size(364, 343);
            this.ResumeLayout(false);

        }

        #endregion

        private Divelements.SandGrid.SandGrid sandGrid;
        private Divelements.SandGrid.GridColumn gridColumnFilterNode;
    }
}
