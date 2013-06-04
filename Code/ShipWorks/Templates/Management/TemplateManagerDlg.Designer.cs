namespace ShipWorks.Templates.Management
{
    partial class TemplateManagerDlg
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
            this.close = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.labelAdd = new System.Windows.Forms.Label();
            this.panelPreview = new System.Windows.Forms.Panel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.contextMenuTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemNewTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemNewSnippet = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemNewFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemContextMove = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemContextCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemContextRename = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rename = new System.Windows.Forms.Button();
            this.delete = new System.Windows.Forms.Button();
            this.edit = new System.Windows.Forms.Button();
            this.newFolder = new System.Windows.Forms.Button();
            this.copy = new System.Windows.Forms.Button();
            this.newTemplate = new System.Windows.Forms.Button();
            this.moveIntoFolder = new System.Windows.Forms.Button();
            this.labelMove = new System.Windows.Forms.Label();
            this.contextMenuDragDrop = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemMove = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.panelEditControls = new System.Windows.Forms.Panel();
            this.treeControl = new ShipWorks.Templates.Controls.TemplateTreeControl();
            this.templatePreview = new ShipWorks.Templates.Controls.TemplatePreviewControl();
            this.panelPreview.SuspendLayout();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.contextMenuTree.SuspendLayout();
            this.contextMenuDragDrop.SuspendLayout();
            this.panelEditControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(835, 452);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 7;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label4.Location = new System.Drawing.Point(3, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Edit";
            // 
            // labelAdd
            // 
            this.labelAdd.AutoSize = true;
            this.labelAdd.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelAdd.Location = new System.Drawing.Point(3, 115);
            this.labelAdd.Name = "labelAdd";
            this.labelAdd.Size = new System.Drawing.Size(29, 13);
            this.labelAdd.TabIndex = 4;
            this.labelAdd.Text = "Add";
            // 
            // panelPreview
            // 
            this.panelPreview.BackColor = System.Drawing.Color.White;
            this.panelPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelPreview.Controls.Add(this.templatePreview);
            this.panelPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPreview.Location = new System.Drawing.Point(0, 0);
            this.panelPreview.Name = "panelPreview";
            this.panelPreview.Size = new System.Drawing.Size(538, 422);
            this.panelPreview.TabIndex = 26;
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
            this.splitContainer.Panel1.Controls.Add(this.treeControl);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.panelPreview);
            this.splitContainer.Size = new System.Drawing.Size(739, 422);
            this.splitContainer.SplitterDistance = 197;
            this.splitContainer.TabIndex = 0;
            // 
            // contextMenuTree
            // 
            this.contextMenuTree.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemEdit,
            this.toolStripSeparator3,
            this.menuItemNewTemplate,
            this.menuItemNewSnippet,
            this.menuItemNewFolder,
            this.toolStripSeparator1,
            this.menuItemContextMove,
            this.menuItemContextCopy,
            this.menuItemContextRename,
            this.toolStripSeparator4,
            this.deleteToolStripMenuItem});
            this.contextMenuTree.Name = "contextMenuTree";
            this.contextMenuTree.Size = new System.Drawing.Size(153, 220);
            this.contextMenuTree.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextMenuOpening);
            // 
            // menuItemEdit
            // 
            this.menuItemEdit.Image = global::ShipWorks.Properties.Resources.edit16;
            this.menuItemEdit.Name = "menuItemEdit";
            this.menuItemEdit.Size = new System.Drawing.Size(152, 22);
            this.menuItemEdit.Text = "Edit";
            this.menuItemEdit.Click += new System.EventHandler(this.OnEdit);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(149, 6);
            // 
            // menuItemNewTemplate
            // 
            this.menuItemNewTemplate.Image = global::ShipWorks.Properties.Resources.template_add;
            this.menuItemNewTemplate.Name = "menuItemNewTemplate";
            this.menuItemNewTemplate.Size = new System.Drawing.Size(152, 22);
            this.menuItemNewTemplate.Text = "New Template";
            this.menuItemNewTemplate.Click += new System.EventHandler(this.OnNewTemplate);
            // 
            // menuItemNewSnippet
            // 
            this.menuItemNewSnippet.Image = global::ShipWorks.Properties.Resources.template_snippet_add;
            this.menuItemNewSnippet.Name = "menuItemNewSnippet";
            this.menuItemNewSnippet.Size = new System.Drawing.Size(152, 22);
            this.menuItemNewSnippet.Text = "New Snippet";
            this.menuItemNewSnippet.Click += new System.EventHandler(this.OnNewTemplate);
            // 
            // menuItemNewFolder
            // 
            this.menuItemNewFolder.Image = global::ShipWorks.Properties.Resources.folderclosed_add;
            this.menuItemNewFolder.Name = "menuItemNewFolder";
            this.menuItemNewFolder.Size = new System.Drawing.Size(152, 22);
            this.menuItemNewFolder.Text = "New Folder";
            this.menuItemNewFolder.Click += new System.EventHandler(this.OnNewFolder);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // menuItemContextMove
            // 
            this.menuItemContextMove.Image = global::ShipWorks.Properties.Resources.move_into_folder;
            this.menuItemContextMove.Name = "menuItemContextMove";
            this.menuItemContextMove.Size = new System.Drawing.Size(152, 22);
            this.menuItemContextMove.Text = "Move";
            this.menuItemContextMove.Click += new System.EventHandler(this.OnMoveIntoFolder);
            // 
            // menuItemContextCopy
            // 
            this.menuItemContextCopy.Image = global::ShipWorks.Properties.Resources.CopyHS;
            this.menuItemContextCopy.Name = "menuItemContextCopy";
            this.menuItemContextCopy.Size = new System.Drawing.Size(152, 22);
            this.menuItemContextCopy.Text = "Copy";
            this.menuItemContextCopy.Click += new System.EventHandler(this.OnCopy);
            // 
            // menuItemContextRename
            // 
            this.menuItemContextRename.Image = global::ShipWorks.Properties.Resources.rename;
            this.menuItemContextRename.Name = "menuItemContextRename";
            this.menuItemContextRename.Size = new System.Drawing.Size(152, 22);
            this.menuItemContextRename.Text = "Rename";
            this.menuItemContextRename.Click += new System.EventHandler(this.OnRename);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(149, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = global::ShipWorks.Properties.Resources.delete16;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.OnDelete);
            // 
            // rename
            // 
            this.rename.Image = global::ShipWorks.Properties.Resources.rename;
            this.rename.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rename.Location = new System.Drawing.Point(6, 50);
            this.rename.Name = "rename";
            this.rename.Size = new System.Drawing.Size(150, 23);
            this.rename.TabIndex = 2;
            this.rename.Text = "Rename";
            this.rename.UseVisualStyleBackColor = true;
            this.rename.Click += new System.EventHandler(this.OnRename);
            // 
            // delete
            // 
            this.delete.Image = global::ShipWorks.Properties.Resources.delete16;
            this.delete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.delete.Location = new System.Drawing.Point(6, 79);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(150, 23);
            this.delete.TabIndex = 3;
            this.delete.Text = "Delete";
            this.delete.UseVisualStyleBackColor = true;
            this.delete.Click += new System.EventHandler(this.OnDelete);
            // 
            // edit
            // 
            this.edit.Image = global::ShipWorks.Properties.Resources.edit16;
            this.edit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.edit.Location = new System.Drawing.Point(6, 20);
            this.edit.Name = "edit";
            this.edit.Size = new System.Drawing.Size(150, 23);
            this.edit.TabIndex = 1;
            this.edit.Text = "Edit";
            this.edit.UseVisualStyleBackColor = true;
            this.edit.Click += new System.EventHandler(this.OnEdit);
            // 
            // newFolder
            // 
            this.newFolder.Image = global::ShipWorks.Properties.Resources.folderclosed_add;
            this.newFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.newFolder.Location = new System.Drawing.Point(6, 160);
            this.newFolder.Name = "newFolder";
            this.newFolder.Size = new System.Drawing.Size(150, 23);
            this.newFolder.TabIndex = 6;
            this.newFolder.Text = "New Folder";
            this.newFolder.UseVisualStyleBackColor = true;
            this.newFolder.Click += new System.EventHandler(this.OnNewFolder);
            // 
            // copy
            // 
            this.copy.Image = global::ShipWorks.Properties.Resources.CopyHS;
            this.copy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.copy.Location = new System.Drawing.Point(6, 189);
            this.copy.Name = "copy";
            this.copy.Size = new System.Drawing.Size(150, 23);
            this.copy.TabIndex = 7;
            this.copy.Text = "Copy";
            this.copy.UseVisualStyleBackColor = true;
            this.copy.Click += new System.EventHandler(this.OnCopy);
            // 
            // newTemplate
            // 
            this.newTemplate.Image = global::ShipWorks.Properties.Resources.template_add;
            this.newTemplate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.newTemplate.Location = new System.Drawing.Point(6, 131);
            this.newTemplate.Name = "newTemplate";
            this.newTemplate.Size = new System.Drawing.Size(150, 23);
            this.newTemplate.TabIndex = 5;
            this.newTemplate.Text = "New Template";
            this.newTemplate.UseVisualStyleBackColor = true;
            this.newTemplate.Click += new System.EventHandler(this.OnNewTemplate);
            // 
            // moveIntoFolder
            // 
            this.moveIntoFolder.Image = global::ShipWorks.Properties.Resources.move_into_folder;
            this.moveIntoFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.moveIntoFolder.Location = new System.Drawing.Point(6, 242);
            this.moveIntoFolder.Name = "moveIntoFolder";
            this.moveIntoFolder.Size = new System.Drawing.Size(150, 23);
            this.moveIntoFolder.TabIndex = 9;
            this.moveIntoFolder.Text = "Move Into Folder";
            this.moveIntoFolder.UseVisualStyleBackColor = true;
            this.moveIntoFolder.Click += new System.EventHandler(this.OnMoveIntoFolder);
            // 
            // labelMove
            // 
            this.labelMove.AutoSize = true;
            this.labelMove.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelMove.Location = new System.Drawing.Point(3, 226);
            this.labelMove.Name = "labelMove";
            this.labelMove.Size = new System.Drawing.Size(38, 13);
            this.labelMove.TabIndex = 8;
            this.labelMove.Text = "Move";
            // 
            // contextMenuDragDrop
            // 
            this.contextMenuDragDrop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemMove,
            this.menuItemCopy,
            this.toolStripSeparator2,
            this.menuItemCancel});
            this.contextMenuDragDrop.Name = "contextMenuDragDrop";
            this.contextMenuDragDrop.Size = new System.Drawing.Size(133, 76);
            this.contextMenuDragDrop.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.OnDragDropMenuClosed);
            // 
            // menuItemMove
            // 
            this.menuItemMove.Image = global::ShipWorks.Properties.Resources.arrow_right_blue;
            this.menuItemMove.Name = "menuItemMove";
            this.menuItemMove.Size = new System.Drawing.Size(132, 22);
            this.menuItemMove.Text = "Move Here";
            this.menuItemMove.Click += new System.EventHandler(this.OnDragDropMove);
            // 
            // menuItemCopy
            // 
            this.menuItemCopy.Image = global::ShipWorks.Properties.Resources.CopyHS;
            this.menuItemCopy.Name = "menuItemCopy";
            this.menuItemCopy.Size = new System.Drawing.Size(132, 22);
            this.menuItemCopy.Text = "Copy Here";
            this.menuItemCopy.Click += new System.EventHandler(this.OnDragDropCopy);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(129, 6);
            // 
            // menuItemCancel
            // 
            this.menuItemCancel.Name = "menuItemCancel";
            this.menuItemCancel.Size = new System.Drawing.Size(132, 22);
            this.menuItemCancel.Text = "Cancel";
            // 
            // panelEditControls
            // 
            this.panelEditControls.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelEditControls.Controls.Add(this.label4);
            this.panelEditControls.Controls.Add(this.newTemplate);
            this.panelEditControls.Controls.Add(this.copy);
            this.panelEditControls.Controls.Add(this.moveIntoFolder);
            this.panelEditControls.Controls.Add(this.labelAdd);
            this.panelEditControls.Controls.Add(this.labelMove);
            this.panelEditControls.Controls.Add(this.newFolder);
            this.panelEditControls.Controls.Add(this.edit);
            this.panelEditControls.Controls.Add(this.rename);
            this.panelEditControls.Controls.Add(this.delete);
            this.panelEditControls.Location = new System.Drawing.Point(756, 6);
            this.panelEditControls.Name = "panelEditControls";
            this.panelEditControls.Size = new System.Drawing.Size(155, 276);
            this.panelEditControls.TabIndex = 1;
            // 
            // templateTree
            // 
            this.treeControl.ContextMenuStrip = this.contextMenuTree;
            this.treeControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeControl.Editable = true;
            this.treeControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.treeControl.HotTrackNode = null;
            this.treeControl.Location = new System.Drawing.Point(0, 0);
            this.treeControl.Name = "templateTree";
            this.treeControl.SnippetDisplay = ShipWorks.Templates.Controls.TemplateTreeSnippetDisplayType.Visible;
            this.treeControl.Size = new System.Drawing.Size(197, 422);
            this.treeControl.TabIndex = 1;
            this.treeControl.DeleteKeyPressed += new System.EventHandler(this.OnDelete);
            this.treeControl.DragDropComplete += new ShipWorks.Templates.Controls.TemplateDragDropCompleteEventHandler(this.OnDragDropComplete);
            this.treeControl.TemplateNodeRenamed += new ShipWorks.Templates.Controls.TemplateNodeRenameEventHandler(this.OnAfterRename);
            this.treeControl.SelectedTemplateNodeChanged += new ShipWorks.Templates.Controls.TemplateNodeChangedEventHandler(this.OnSelectedTemplateNodeChanged);
            // 
            // templatePreview
            // 
            this.templatePreview.BackColor = System.Drawing.SystemColors.Control;
            this.templatePreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.templatePreview.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.templatePreview.Location = new System.Drawing.Point(0, 0);
            this.templatePreview.Name = "templatePreview";
            this.templatePreview.Size = new System.Drawing.Size(534, 418);
            this.templatePreview.TabIndex = 30;
            // 
            // TemplateManagerDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(922, 487);
            this.Controls.Add(this.panelEditControls);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.close);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(656, 423);
            this.Name = "TemplateManagerDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Template Manager";
            this.Load += new System.EventHandler(this.OnLoad);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.panelPreview.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.contextMenuTree.ResumeLayout(false);
            this.contextMenuDragDrop.ResumeLayout(false);
            this.panelEditControls.ResumeLayout(false);
            this.panelEditControls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button close;
        private ShipWorks.Templates.Controls.TemplateTreeControl treeControl;
        private System.Windows.Forms.Button rename;
        private System.Windows.Forms.Button delete;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button edit;
        private System.Windows.Forms.Button newFolder;
        private System.Windows.Forms.Label labelAdd;
        private System.Windows.Forms.Button copy;
        private System.Windows.Forms.Button newTemplate;
        private System.Windows.Forms.Panel panelPreview;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Button moveIntoFolder;
        private System.Windows.Forms.Label labelMove;
        private System.Windows.Forms.ContextMenuStrip contextMenuTree;
        private System.Windows.Forms.ToolStripMenuItem menuItemEdit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuItemNewTemplate;
        private System.Windows.Forms.ToolStripMenuItem menuItemNewFolder;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemContextMove;
        private System.Windows.Forms.ToolStripMenuItem menuItemContextCopy;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuDragDrop;
        private System.Windows.Forms.ToolStripMenuItem menuItemMove;
        private System.Windows.Forms.ToolStripMenuItem menuItemCopy;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemCancel;
        private ShipWorks.Templates.Controls.TemplatePreviewControl templatePreview;
        private System.Windows.Forms.Panel panelEditControls;
        private System.Windows.Forms.ToolStripMenuItem menuItemContextRename;
        private System.Windows.Forms.ToolStripMenuItem menuItemNewSnippet;
    }
}