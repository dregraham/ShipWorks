namespace ShipWorks.Email.Outlook
{
    partial class EmailOutboundFolderBase
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
            this.components = new System.ComponentModel.Container();
            this.delete = new System.Windows.Forms.Button();
            this.gridMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSep = new System.Windows.Forms.ToolStripSeparator();
            this.menuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.panelMessageControls.SuspendLayout();
            this.gridMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // entityGrid
            // 
            this.entityGrid.ContextMenuStrip = this.gridMenu;
            this.entityGrid.Size = new System.Drawing.Size(696, 469);
            this.entityGrid.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnGridSelectionChanged);
            // 
            // panelMessageControls
            // 
            this.panelMessageControls.Controls.Add(this.delete);
            this.panelMessageControls.Controls.SetChildIndex(this.delete, 0);
            // 
            // delete
            // 
            this.delete.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.delete.Image = global::ShipWorks.Properties.Resources.delete16;
            this.delete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.delete.Location = new System.Drawing.Point(5, 56);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(115, 23);
            this.delete.TabIndex = 21;
            this.delete.Text = "Delete";
            this.delete.UseVisualStyleBackColor = true;
            this.delete.Click += new System.EventHandler(this.OnDelete);
            // 
            // gridMenu
            // 
            this.gridMenu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.gridMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEdit,
            this.menuDelete,
            this.menuSep,
            this.menuCopy});
            this.gridMenu.Name = "contextMenu";
            this.gridMenu.Size = new System.Drawing.Size(153, 98);
            this.gridMenu.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextMenuOpening);
            // 
            // menuEdit
            // 
            this.menuEdit.Image = global::ShipWorks.Properties.Resources.edit16;
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size(152, 22);
            this.menuEdit.Text = "Edit";
            // 
            // menuDelete
            // 
            this.menuDelete.Image = global::ShipWorks.Properties.Resources.delete16;
            this.menuDelete.Name = "menuDelete";
            this.menuDelete.Size = new System.Drawing.Size(152, 22);
            this.menuDelete.Text = "Delete";
            this.menuDelete.Click += new System.EventHandler(this.OnDelete);
            // 
            // menuSep
            // 
            this.menuSep.Name = "menuSep";
            this.menuSep.Size = new System.Drawing.Size(149, 6);
            // 
            // menuCopy
            // 
            this.menuCopy.Image = global::ShipWorks.Properties.Resources.copy;
            this.menuCopy.Name = "menuCopy";
            this.menuCopy.Size = new System.Drawing.Size(152, 22);
            this.menuCopy.Text = "Copy";
            // 
            // EmailOutboundFolderBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "EmailOutboundFolderBase";
            this.panelMessageControls.ResumeLayout(false);
            this.panelMessageControls.PerformLayout();
            this.gridMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Button delete;
        protected System.Windows.Forms.ContextMenuStrip gridMenu;
        private System.Windows.Forms.ToolStripMenuItem menuDelete;
        private System.Windows.Forms.ToolStripSeparator menuSep;
        private System.Windows.Forms.ToolStripMenuItem menuCopy;
        protected System.Windows.Forms.ToolStripMenuItem menuEdit;

    }
}
