namespace ShipWorks.Stores.Content.Panels
{
    partial class PrintResultsPanel
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
            this.components = new System.ComponentModel.Container();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // addLink
            // 
            this.addLink.Location = new System.Drawing.Point(232, 46);
            this.addLink.Visible = false;
            // 
            // entityGrid
            // 
            this.entityGrid.ContextMenuStrip = this.contextMenu;
            this.entityGrid.EmptyText = "Nothing has been printed.";
            this.entityGrid.EmptyTextForeColor = System.Drawing.SystemColors.GrayText;
            this.entityGrid.Size = new System.Drawing.Size(262, 41);
            this.entityGrid.GridCellLinkClicked += new ShipWorks.Data.Grid.GridHyperlinkClickEventHandler(this.OnGridCellLinkClicked);
            // 
            // contextMenu
            // 
            this.contextMenu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuView,
            this.menuCopy});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(103, 48);
            this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextMenuOpening);
            // 
            // menuView
            // 
            this.menuView.Image = global::ShipWorks.Properties.Resources.view;
            this.menuView.Name = "menuView";
            this.menuView.Size = new System.Drawing.Size(102, 22);
            this.menuView.Text = "View";
            this.menuView.Click += new System.EventHandler(this.OnView);
            // 
            // menuCopy
            // 
            this.menuCopy.Image = global::ShipWorks.Properties.Resources.copy;
            this.menuCopy.Name = "menuCopy";
            this.menuCopy.Size = new System.Drawing.Size(102, 22);
            this.menuCopy.Text = "Copy";
            // 
            // PrintResultsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "PrintResultsPanel";
            this.Controls.SetChildIndex(this.entityGrid, 0);
            this.Controls.SetChildIndex(this.addLink, 0);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuView;
        private System.Windows.Forms.ToolStripMenuItem menuCopy;
    }
}
