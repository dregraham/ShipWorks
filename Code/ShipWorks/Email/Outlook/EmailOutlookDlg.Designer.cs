namespace ShipWorks.Email.Outlook
{
    partial class EmailOutlookDlg
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
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer1 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.close = new System.Windows.Forms.Button();
            this.sandTree = new Divelements.SandGrid.SandGrid();
            this.gridColumnFolder = new Divelements.SandGrid.GridColumn();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(794, 396);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 0;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // sandTree
            // 
            this.sandTree.AllowMultipleSelection = false;
            this.sandTree.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnFolder});
            this.sandTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sandTree.EnableSearching = false;
            this.sandTree.Location = new System.Drawing.Point(0, 0);
            this.sandTree.Name = "sandTree";
            this.sandTree.Renderer = windowsXPRenderer1;
            this.sandTree.RowHighlightType = Divelements.SandGrid.RowHighlightType.Full;
            this.sandTree.Rows.AddRange(new Divelements.SandGrid.GridRow[] {
            new Divelements.SandGrid.Specialized.SingleCellRow("Sent Items", global::ShipWorks.Properties.Resources.mail_ok),
            new Divelements.SandGrid.Specialized.SingleCellRow("Outbox", global::ShipWorks.Properties.Resources.mailbox_full)});
            this.sandTree.ShowColumnHeaders = false;
            this.sandTree.ShowRootLines = false;
            this.sandTree.Size = new System.Drawing.Size(125, 378);
            this.sandTree.TabIndex = 1;
            this.sandTree.WhitespaceClickBehavior = Divelements.SandGrid.WhitespaceClickBehavior.None;
            this.sandTree.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnChangeMailbox);
            // 
            // gridColumnFolder
            // 
            this.gridColumnFolder.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnFolder.HeaderText = "Folder";
            this.gridColumnFolder.Width = 121;
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(12, 12);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.sandTree);
            this.splitContainer.Size = new System.Drawing.Size(857, 378);
            this.splitContainer.SplitterDistance = 125;
            this.splitContainer.TabIndex = 5;
            // 
            // EmailOutlookDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(881, 431);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.close);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EmailOutlookDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Email Messages";
            this.Load += new System.EventHandler(this.OnLoad);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button close;
        private Divelements.SandGrid.SandGrid sandTree;
        private System.Windows.Forms.SplitContainer splitContainer;
        private Divelements.SandGrid.GridColumn gridColumnFolder;
    }
}