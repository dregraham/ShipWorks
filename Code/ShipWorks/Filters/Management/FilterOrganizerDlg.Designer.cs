using ShipWorks.Data.Model.EntityClasses;
namespace ShipWorks.Filters.Management
{
    partial class FilterOrganizerDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterOrganizerDlg));
            this.labelAdd = new System.Windows.Forms.Label();
            this.labelMove = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.close = new System.Windows.Forms.Button();
            this.contextMenuStripSort = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemSortSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSortAll = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuDragDrop = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemMove = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemLink = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.rename = new System.Windows.Forms.Button();
            this.moveIntoFolder = new System.Windows.Forms.Button();
            this.delete = new System.Windows.Forms.Button();
            this.edit = new System.Windows.Forms.Button();
            this.moveDown = new System.Windows.Forms.Button();
            this.moveUp = new System.Windows.Forms.Button();
            this.newFolder = new System.Windows.Forms.Button();
            this.createLink = new System.Windows.Forms.Button();
            this.copy = new System.Windows.Forms.Button();
            this.newFilter = new System.Windows.Forms.Button();
            this.contextMenuTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemNewFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemNewFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemContextMove = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemContextCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCreateLink = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageOrders = new System.Windows.Forms.TabPage();
            this.tabPageCustomers = new System.Windows.Forms.TabPage();
            this.imageListTabs = new System.Windows.Forms.ImageList(this.components);
            this.showDisabledFilters = new System.Windows.Forms.CheckBox();
            this.filterTreeOrders = new ShipWorks.Filters.Controls.FilterTree();
            this.filterTreeCustomers = new ShipWorks.Filters.Controls.FilterTree();
            this.sortButton = new ShipWorks.UI.Controls.DropDownButton();
            this.editionGuiHelper = new ShipWorks.Editions.EditionGuiHelper(this.components);
            this.contextMenuStripSort.SuspendLayout();
            this.contextMenuDragDrop.SuspendLayout();
            this.contextMenuTree.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageOrders.SuspendLayout();
            this.tabPageCustomers.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelAdd
            // 
            this.labelAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAdd.AutoSize = true;
            this.labelAdd.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAdd.Location = new System.Drawing.Point(323, 145);
            this.labelAdd.Name = "labelAdd";
            this.labelAdd.Size = new System.Drawing.Size(29, 13);
            this.labelAdd.TabIndex = 5;
            this.labelAdd.Text = "Add";
            // 
            // labelMove
            // 
            this.labelMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMove.AutoSize = true;
            this.labelMove.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMove.Location = new System.Drawing.Point(323, 284);
            this.labelMove.Name = "labelMove";
            this.labelMove.Size = new System.Drawing.Size(38, 13);
            this.labelMove.TabIndex = 10;
            this.labelMove.Text = "Move";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(323, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Edit";
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.close.Location = new System.Drawing.Point(400, 433);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 15;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // contextMenuStripSort
            // 
            this.contextMenuStripSort.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSortSelected,
            this.toolStripMenuItemSortAll});
            this.contextMenuStripSort.Name = "contextMenuStripRename";
            this.contextMenuStripSort.Size = new System.Drawing.Size(196, 48);
            this.contextMenuStripSort.Opening += new System.ComponentModel.CancelEventHandler(this.OnSortMenuOpening);
            // 
            // toolStripMenuItemSortSelected
            // 
            this.toolStripMenuItemSortSelected.Name = "toolStripMenuItemSortSelected";
            this.toolStripMenuItemSortSelected.Size = new System.Drawing.Size(195, 22);
            this.toolStripMenuItemSortSelected.Text = "Contents of \'Whatever\'";
            this.toolStripMenuItemSortSelected.Click += new System.EventHandler(this.OnSortSelectedFolder);
            // 
            // toolStripMenuItemSortAll
            // 
            this.toolStripMenuItemSortAll.Name = "toolStripMenuItemSortAll";
            this.toolStripMenuItemSortAll.Size = new System.Drawing.Size(195, 22);
            this.toolStripMenuItemSortAll.Text = "All Filters and Folders";
            this.toolStripMenuItemSortAll.Click += new System.EventHandler(this.OnSortAll);
            // 
            // contextMenuDragDrop
            // 
            this.contextMenuDragDrop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemMove,
            this.menuItemCopy,
            this.menuItemLink,
            this.toolStripSeparator2,
            this.menuItemCancel});
            this.contextMenuDragDrop.Name = "contextMenuDragDrop";
            this.contextMenuDragDrop.Size = new System.Drawing.Size(133, 98);
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
            // menuItemLink
            // 
            this.menuItemLink.Image = global::ShipWorks.Properties.Resources.paperclip16;
            this.menuItemLink.Name = "menuItemLink";
            this.menuItemLink.Size = new System.Drawing.Size(132, 22);
            this.menuItemLink.Text = "Link Here";
            this.menuItemLink.Click += new System.EventHandler(this.OnDragDropLink);
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
            // rename
            // 
            this.rename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rename.Image = global::ShipWorks.Properties.Resources.rename;
            this.rename.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rename.Location = new System.Drawing.Point(326, 80);
            this.rename.Name = "rename";
            this.rename.Size = new System.Drawing.Size(150, 23);
            this.rename.TabIndex = 3;
            this.rename.Text = "Rename";
            this.rename.UseVisualStyleBackColor = true;
            this.rename.Click += new System.EventHandler(this.OnRename);
            // 
            // moveIntoFolder
            // 
            this.moveIntoFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moveIntoFolder.Image = global::ShipWorks.Properties.Resources.move_into_folder;
            this.moveIntoFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.moveIntoFolder.Location = new System.Drawing.Point(326, 358);
            this.moveIntoFolder.Name = "moveIntoFolder";
            this.moveIntoFolder.Size = new System.Drawing.Size(150, 23);
            this.moveIntoFolder.TabIndex = 13;
            this.moveIntoFolder.Text = "Move Into Folder";
            this.moveIntoFolder.UseVisualStyleBackColor = true;
            this.moveIntoFolder.Click += new System.EventHandler(this.OnMoveIntoFolder);
            // 
            // delete
            // 
            this.delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.delete.Image = global::ShipWorks.Properties.Resources.delete16;
            this.delete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.delete.Location = new System.Drawing.Point(326, 109);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(150, 23);
            this.delete.TabIndex = 4;
            this.delete.Text = "Delete";
            this.delete.UseVisualStyleBackColor = true;
            this.delete.Click += new System.EventHandler(this.OnDelete);
            // 
            // edit
            // 
            this.edit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.edit.Image = global::ShipWorks.Properties.Resources.edit16;
            this.edit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.edit.Location = new System.Drawing.Point(326, 50);
            this.edit.Name = "edit";
            this.edit.Size = new System.Drawing.Size(150, 23);
            this.edit.TabIndex = 2;
            this.edit.Text = "Edit";
            this.edit.UseVisualStyleBackColor = true;
            this.edit.Click += new System.EventHandler(this.OnEdit);
            // 
            // moveDown
            // 
            this.moveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moveDown.Image = global::ShipWorks.Properties.Resources.arrow_down_blue;
            this.moveDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.moveDown.Location = new System.Drawing.Point(326, 329);
            this.moveDown.Name = "moveDown";
            this.moveDown.Size = new System.Drawing.Size(150, 23);
            this.moveDown.TabIndex = 12;
            this.moveDown.Text = "Move Down";
            this.moveDown.UseVisualStyleBackColor = true;
            this.moveDown.Click += new System.EventHandler(this.OnMoveDown);
            // 
            // moveUp
            // 
            this.moveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moveUp.Image = global::ShipWorks.Properties.Resources.arrow_up_blue;
            this.moveUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.moveUp.Location = new System.Drawing.Point(326, 300);
            this.moveUp.Name = "moveUp";
            this.moveUp.Size = new System.Drawing.Size(150, 23);
            this.moveUp.TabIndex = 11;
            this.moveUp.Text = "Move Up";
            this.moveUp.UseVisualStyleBackColor = true;
            this.moveUp.Click += new System.EventHandler(this.OnMoveUp);
            // 
            // newFolder
            // 
            this.newFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newFolder.Image = global::ShipWorks.Properties.Resources.folderclosed_add;
            this.newFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.newFolder.Location = new System.Drawing.Point(326, 190);
            this.newFolder.Name = "newFolder";
            this.newFolder.Size = new System.Drawing.Size(150, 23);
            this.newFolder.TabIndex = 7;
            this.newFolder.Text = "New Folder";
            this.newFolder.UseVisualStyleBackColor = true;
            this.newFolder.Click += new System.EventHandler(this.OnNewFolder);
            // 
            // createLink
            // 
            this.createLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.createLink.Image = global::ShipWorks.Properties.Resources.paperclip16;
            this.createLink.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.createLink.Location = new System.Drawing.Point(326, 248);
            this.createLink.Name = "createLink";
            this.createLink.Size = new System.Drawing.Size(150, 23);
            this.createLink.TabIndex = 9;
            this.createLink.Text = "Create Link";
            this.createLink.UseMnemonic = false;
            this.createLink.UseVisualStyleBackColor = true;
            this.createLink.Click += new System.EventHandler(this.OnCreateLink);
            // 
            // copy
            // 
            this.copy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.copy.Image = global::ShipWorks.Properties.Resources.CopyHS;
            this.copy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.copy.Location = new System.Drawing.Point(326, 219);
            this.copy.Name = "copy";
            this.copy.Size = new System.Drawing.Size(150, 23);
            this.copy.TabIndex = 8;
            this.copy.Text = "Copy";
            this.copy.UseVisualStyleBackColor = true;
            this.copy.Click += new System.EventHandler(this.OnCopy);
            // 
            // newFilter
            // 
            this.newFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newFilter.Image = global::ShipWorks.Properties.Resources.filter_add;
            this.newFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.newFilter.Location = new System.Drawing.Point(326, 161);
            this.newFilter.Name = "newFilter";
            this.newFilter.Size = new System.Drawing.Size(150, 23);
            this.newFilter.TabIndex = 6;
            this.newFilter.Text = "New Filter";
            this.newFilter.UseVisualStyleBackColor = true;
            this.newFilter.Click += new System.EventHandler(this.OnNewFilter);
            // 
            // contextMenuTree
            // 
            this.contextMenuTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemEdit,
            this.toolStripSeparator3,
            this.menuItemNewFilter,
            this.menuItemNewFolder,
            this.toolStripSeparator1,
            this.menuItemContextMove,
            this.menuItemContextCopy,
            this.menuItemCreateLink,
            this.toolStripSeparator4,
            this.deleteToolStripMenuItem});
            this.contextMenuTree.Name = "contextMenuTree";
            this.contextMenuTree.Size = new System.Drawing.Size(135, 176);
            this.contextMenuTree.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextMenuOpening);
            // 
            // menuItemEdit
            // 
            this.menuItemEdit.Image = global::ShipWorks.Properties.Resources.edit16;
            this.menuItemEdit.Name = "menuItemEdit";
            this.menuItemEdit.Size = new System.Drawing.Size(134, 22);
            this.menuItemEdit.Text = "Edit";
            this.menuItemEdit.Click += new System.EventHandler(this.OnEdit);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(131, 6);
            // 
            // menuItemNewFilter
            // 
            this.menuItemNewFilter.Image = global::ShipWorks.Properties.Resources.filter_add;
            this.menuItemNewFilter.Name = "menuItemNewFilter";
            this.menuItemNewFilter.Size = new System.Drawing.Size(134, 22);
            this.menuItemNewFilter.Text = "New Filter";
            this.menuItemNewFilter.Click += new System.EventHandler(this.OnNewFilter);
            // 
            // menuItemNewFolder
            // 
            this.menuItemNewFolder.Image = global::ShipWorks.Properties.Resources.folderclosed_add;
            this.menuItemNewFolder.Name = "menuItemNewFolder";
            this.menuItemNewFolder.Size = new System.Drawing.Size(134, 22);
            this.menuItemNewFolder.Text = "New Folder";
            this.menuItemNewFolder.Click += new System.EventHandler(this.OnNewFolder);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(131, 6);
            // 
            // menuItemContextMove
            // 
            this.menuItemContextMove.Image = global::ShipWorks.Properties.Resources.move_into_folder;
            this.menuItemContextMove.Name = "menuItemContextMove";
            this.menuItemContextMove.Size = new System.Drawing.Size(134, 22);
            this.menuItemContextMove.Text = "Move";
            this.menuItemContextMove.Click += new System.EventHandler(this.OnMoveIntoFolder);
            // 
            // menuItemContextCopy
            // 
            this.menuItemContextCopy.Image = global::ShipWorks.Properties.Resources.CopyHS;
            this.menuItemContextCopy.Name = "menuItemContextCopy";
            this.menuItemContextCopy.Size = new System.Drawing.Size(134, 22);
            this.menuItemContextCopy.Text = "Copy";
            this.menuItemContextCopy.Click += new System.EventHandler(this.OnCopy);
            // 
            // menuItemCreateLink
            // 
            this.menuItemCreateLink.Image = global::ShipWorks.Properties.Resources.paperclip16;
            this.menuItemCreateLink.Name = "menuItemCreateLink";
            this.menuItemCreateLink.Size = new System.Drawing.Size(134, 22);
            this.menuItemCreateLink.Text = "Create Link";
            this.menuItemCreateLink.Click += new System.EventHandler(this.OnCreateLink);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(131, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = global::ShipWorks.Properties.Resources.delete16;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.OnDelete);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageOrders);
            this.tabControl.Controls.Add(this.tabPageCustomers);
            this.tabControl.ImageList = this.imageListTabs;
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(305, 398);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.OnTabChanged);
            // 
            // tabPageOrders
            // 
            this.tabPageOrders.Controls.Add(this.filterTreeOrders);
            this.tabPageOrders.ImageIndex = 1;
            this.tabPageOrders.Location = new System.Drawing.Point(4, 23);
            this.tabPageOrders.Name = "tabPageOrders";
            this.tabPageOrders.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOrders.Size = new System.Drawing.Size(297, 371);
            this.tabPageOrders.TabIndex = 0;
            this.tabPageOrders.Text = "Orders";
            this.tabPageOrders.UseVisualStyleBackColor = true;
            // 
            // tabPageCustomers
            // 
            this.tabPageCustomers.Controls.Add(this.filterTreeCustomers);
            this.tabPageCustomers.ImageIndex = 0;
            this.tabPageCustomers.Location = new System.Drawing.Point(4, 23);
            this.tabPageCustomers.Name = "tabPageCustomers";
            this.tabPageCustomers.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCustomers.Size = new System.Drawing.Size(297, 371);
            this.tabPageCustomers.TabIndex = 1;
            this.tabPageCustomers.Text = "Customers";
            this.tabPageCustomers.UseVisualStyleBackColor = true;
            // 
            // imageListTabs
            // 
            this.imageListTabs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTabs.ImageStream")));
            this.imageListTabs.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTabs.Images.SetKeyName(0, "user11.png");
            this.imageListTabs.Images.SetKeyName(1, "form_blue.png");
            // 
            // showDisabledFilters
            // 
            this.showDisabledFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.showDisabledFilters.AutoSize = true;
            this.showDisabledFilters.Location = new System.Drawing.Point(12, 416);
            this.showDisabledFilters.Name = "showDisabledFilters";
            this.showDisabledFilters.Size = new System.Drawing.Size(124, 17);
            this.showDisabledFilters.TabIndex = 16;
            this.showDisabledFilters.Text = "Show disabled filters";
            this.showDisabledFilters.UseVisualStyleBackColor = true;
            this.showDisabledFilters.CheckedChanged += new System.EventHandler(this.OnShowDisabledFiltersCheckedChanged);
            // 
            // filterTreeOrders
            // 
            this.filterTreeOrders.AutoRefreshCalculatingCounts = true;
            this.filterTreeOrders.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.filterTreeOrders.ContextMenuStrip = this.contextMenuTree;
            this.filterTreeOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterTreeOrders.Editable = true;
            this.filterTreeOrders.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterTreeOrders.HideDisabledFilters = true;
            this.filterTreeOrders.HotTrackNode = null;
            this.filterTreeOrders.Location = new System.Drawing.Point(3, 3);
            this.filterTreeOrders.Name = "filterTreeOrders";
            this.filterTreeOrders.Size = new System.Drawing.Size(291, 365);
            this.filterTreeOrders.TabIndex = 0;
            this.filterTreeOrders.SelectedFilterNodeChanged += new System.EventHandler(this.OnChangeSelectedFilterNode);
            this.filterTreeOrders.FilterRenaming += new ShipWorks.Filters.Controls.FilterNodeRenameEventHandler(this.OnBeforeRename);
            this.filterTreeOrders.FilterRenamed += new ShipWorks.Filters.Controls.FilterNodeRenameEventHandler(this.OnAfterRename);
            this.filterTreeOrders.DeleteKeyPressed += new System.EventHandler(this.OnDelete);
            this.filterTreeOrders.DragDropComplete += new ShipWorks.Filters.Controls.FilterDragDropCompleteEventHandler(this.OnDragDropComplete);
            // 
            // filterTreeCustomers
            // 
            this.filterTreeCustomers.AutoRefreshCalculatingCounts = true;
            this.filterTreeCustomers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.filterTreeCustomers.ContextMenuStrip = this.contextMenuTree;
            this.filterTreeCustomers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterTreeCustomers.Editable = true;
            this.filterTreeCustomers.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterTreeCustomers.HideDisabledFilters = true;
            this.filterTreeCustomers.HotTrackNode = null;
            this.filterTreeCustomers.Location = new System.Drawing.Point(3, 3);
            this.filterTreeCustomers.Name = "filterTreeCustomers";
            this.filterTreeCustomers.Size = new System.Drawing.Size(291, 365);
            this.filterTreeCustomers.TabIndex = 1;
            this.filterTreeCustomers.SelectedFilterNodeChanged += new System.EventHandler(this.OnChangeSelectedFilterNode);
            this.filterTreeCustomers.FilterRenaming += new ShipWorks.Filters.Controls.FilterNodeRenameEventHandler(this.OnBeforeRename);
            this.filterTreeCustomers.FilterRenamed += new ShipWorks.Filters.Controls.FilterNodeRenameEventHandler(this.OnAfterRename);
            this.filterTreeCustomers.DeleteKeyPressed += new System.EventHandler(this.OnDelete);
            this.filterTreeCustomers.DragDropComplete += new ShipWorks.Filters.Controls.FilterDragDropCompleteEventHandler(this.OnDragDropComplete);
            // 
            // sortButton
            // 
            this.sortButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sortButton.AutoSize = true;
            this.sortButton.ContextMenuStrip = this.contextMenuStripSort;
            this.sortButton.Image = global::ShipWorks.Properties.Resources.sort_az_descending;
            this.sortButton.Location = new System.Drawing.Point(326, 387);
            this.sortButton.Name = "sortButton";
            this.sortButton.Size = new System.Drawing.Size(149, 23);
            this.sortButton.SplitContextMenu = this.contextMenuStripSort;
            this.sortButton.TabIndex = 14;
            this.sortButton.Text = "Sort";
            this.sortButton.UseVisualStyleBackColor = true;
            this.sortButton.Click += new System.EventHandler(this.OnSortSelectedFolder);
            // 
            // FilterOrganizerDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(487, 468);
            this.Controls.Add(this.showDisabledFilters);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.rename);
            this.Controls.Add(this.delete);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.edit);
            this.Controls.Add(this.sortButton);
            this.Controls.Add(this.moveIntoFolder);
            this.Controls.Add(this.close);
            this.Controls.Add(this.moveDown);
            this.Controls.Add(this.moveUp);
            this.Controls.Add(this.labelMove);
            this.Controls.Add(this.newFolder);
            this.Controls.Add(this.labelAdd);
            this.Controls.Add(this.createLink);
            this.Controls.Add(this.copy);
            this.Controls.Add(this.newFilter);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(486, 497);
            this.Name = "FilterOrganizerDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Filter Organizer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.contextMenuStripSort.ResumeLayout(false);
            this.contextMenuDragDrop.ResumeLayout(false);
            this.contextMenuTree.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageOrders.ResumeLayout(false);
            this.tabPageCustomers.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button newFilter;
        private System.Windows.Forms.Button copy;
        private System.Windows.Forms.Button createLink;
        private System.Windows.Forms.Label labelAdd;
        private System.Windows.Forms.Button newFolder;
        private System.Windows.Forms.Label labelMove;
        private System.Windows.Forms.Button moveDown;
        private System.Windows.Forms.Button moveUp;
        private System.Windows.Forms.Button delete;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button edit;
        private System.Windows.Forms.Button close;
        private System.Windows.Forms.Button moveIntoFolder;
        private ShipWorks.Filters.Controls.FilterTree filterTreeOrders;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripSort;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSortSelected;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSortAll;
        private System.Windows.Forms.Button rename;
        private System.Windows.Forms.ContextMenuStrip contextMenuDragDrop;
        private System.Windows.Forms.ToolStripMenuItem menuItemMove;
        private System.Windows.Forms.ToolStripMenuItem menuItemCopy;
        private System.Windows.Forms.ToolStripMenuItem menuItemLink;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemCancel;
        private System.Windows.Forms.ContextMenuStrip contextMenuTree;
        private System.Windows.Forms.ToolStripMenuItem menuItemNewFilter;
        private System.Windows.Forms.ToolStripMenuItem menuItemNewFolder;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemContextMove;
        private System.Windows.Forms.ToolStripMenuItem menuItemContextCopy;
        private System.Windows.Forms.ToolStripMenuItem menuItemCreateLink;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuItemEdit;
        private ShipWorks.UI.Controls.DropDownButton sortButton;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageOrders;
        private System.Windows.Forms.TabPage tabPageCustomers;
        private System.Windows.Forms.ImageList imageListTabs;
        private ShipWorks.Filters.Controls.FilterTree filterTreeCustomers;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private Editions.EditionGuiHelper editionGuiHelper;
        private System.Windows.Forms.CheckBox showDisabledFilters;
    }
}