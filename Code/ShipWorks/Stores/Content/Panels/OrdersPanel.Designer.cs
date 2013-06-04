namespace ShipWorks.Stores.Content.Panels
{
    partial class OrdersPanel
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
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSep = new System.Windows.Forms.ToolStripSeparator();
            this.menuLocalStatus = new System.Windows.Forms.ToolStripMenuItem();
            this.contentGoesHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSepStatus = new System.Windows.Forms.ToolStripSeparator();
            this.menuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.editionGuiHelper = new ShipWorks.Editions.EditionGuiHelper(this.components);
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // addLink
            // 
            this.addLink.Location = new System.Drawing.Point(201, 46);
            this.addLink.Size = new System.Drawing.Size(57, 13);
            this.addLink.Text = "Add Order";
            this.addLink.Click += new System.EventHandler(this.OnAddOrder);
            // 
            // entityGrid
            // 
            this.entityGrid.ContextMenuStrip = this.contextMenu;
            this.entityGrid.EmptyText = "The customer has no orders.";
            this.entityGrid.EmptyTextForeColor = System.Drawing.SystemColors.GrayText;
            this.entityGrid.Size = new System.Drawing.Size(262, 41);
            this.entityGrid.GridCellLinkClicked += new ShipWorks.Data.Grid.GridHyperlinkClickEventHandler(this.OnGridCellLinkClicked);
            // 
            // contextMenu
            // 
            this.contextMenu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEdit,
            this.menuDelete,
            this.menuSep,
            this.menuLocalStatus,
            this.menuSepStatus,
            this.menuCopy});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(138, 104);
            this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextMenuOpening);
            // 
            // menuEdit
            // 
            this.menuEdit.Image = global::ShipWorks.Properties.Resources.edit16;
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size(137, 22);
            this.menuEdit.Text = "Edit";
            this.menuEdit.Click += new System.EventHandler(this.OnEdit);
            // 
            // menuDelete
            // 
            this.menuDelete.Image = global::ShipWorks.Properties.Resources.delete16;
            this.menuDelete.Name = "menuDelete";
            this.menuDelete.Size = new System.Drawing.Size(137, 22);
            this.menuDelete.Text = "Delete";
            this.menuDelete.Click += new System.EventHandler(this.OnDelete);
            // 
            // menuSep
            // 
            this.menuSep.Name = "menuSep";
            this.menuSep.Size = new System.Drawing.Size(134, 6);
            // 
            // menuLocalStatus
            // 
            this.menuLocalStatus.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentGoesHereToolStripMenuItem});
            this.menuLocalStatus.Image = global::ShipWorks.Properties.Resources.document_pulse;
            this.menuLocalStatus.Name = "menuLocalStatus";
            this.menuLocalStatus.Size = new System.Drawing.Size(137, 22);
            this.menuLocalStatus.Text = "Local Status";
            // 
            // contentGoesHereToolStripMenuItem
            // 
            this.contentGoesHereToolStripMenuItem.Name = "contentGoesHereToolStripMenuItem";
            this.contentGoesHereToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.contentGoesHereToolStripMenuItem.Text = "Content goes here";
            // 
            // menuSepStatus
            // 
            this.menuSepStatus.Name = "menuSepStatus";
            this.menuSepStatus.Size = new System.Drawing.Size(134, 6);
            // 
            // menuCopy
            // 
            this.menuCopy.Image = global::ShipWorks.Properties.Resources.copy;
            this.menuCopy.Name = "menuCopy";
            this.menuCopy.Size = new System.Drawing.Size(137, 22);
            this.menuCopy.Text = "Copy";
            // 
            // OrdersPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "OrdersPanel";
            this.Controls.SetChildIndex(this.addLink, 0);
            this.Controls.SetChildIndex(this.entityGrid, 0);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuEdit;
        private System.Windows.Forms.ToolStripMenuItem menuDelete;
        private System.Windows.Forms.ToolStripSeparator menuSep;
        private System.Windows.Forms.ToolStripMenuItem menuCopy;
        private System.Windows.Forms.ToolStripMenuItem menuLocalStatus;
        private System.Windows.Forms.ToolStripSeparator menuSepStatus;
        private System.Windows.Forms.ToolStripMenuItem contentGoesHereToolStripMenuItem;
        private Editions.EditionGuiHelper editionGuiHelper;
    }
}
