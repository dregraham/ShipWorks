namespace ShipWorks.Users
{
    partial class UserManagerDlg
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
            this.close = new System.Windows.Forms.Button();
            this.showDeletedUsers = new System.Windows.Forms.CheckBox();
            this.labelManage = new System.Windows.Forms.Label();
            this.labelAdd = new System.Windows.Forms.Label();
            this.sandGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnUser = new Divelements.SandGrid.GridColumn();
            this.gridColumnEmail = new Divelements.SandGrid.GridColumn();
            this.gridColumnDescription = new Divelements.SandGrid.GridColumn();
            this.menuCopyRightsFrom = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.whateverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewAuditHistory = new System.Windows.Forms.Button();
            this.newUser = new System.Windows.Forms.Button();
            this.edit = new System.Windows.Forms.Button();
            this.copyRightsFrom = new ShipWorks.UI.Controls.DropDownButton();
            this.delete = new System.Windows.Forms.Button();
            this.infotipDeletedUsers = new ShipWorks.UI.Controls.InfoTip();
            this.menuCopyRightsFrom.SuspendLayout();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(478, 212);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 9;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // showDeletedUsers
            // 
            this.showDeletedUsers.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.showDeletedUsers.AutoSize = true;
            this.showDeletedUsers.Location = new System.Drawing.Point(12, 212);
            this.showDeletedUsers.Name = "showDeletedUsers";
            this.showDeletedUsers.Size = new System.Drawing.Size(120, 17);
            this.showDeletedUsers.TabIndex = 8;
            this.showDeletedUsers.Text = "Show deleted users";
            this.showDeletedUsers.UseVisualStyleBackColor = true;
            this.showDeletedUsers.Click += new System.EventHandler(this.OnChangeShowDeleted);
            // 
            // labelManage
            // 
            this.labelManage.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelManage.AutoSize = true;
            this.labelManage.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelManage.Location = new System.Drawing.Point(402, 12);
            this.labelManage.Name = "labelManage";
            this.labelManage.Size = new System.Drawing.Size(52, 13);
            this.labelManage.TabIndex = 1;
            this.labelManage.Text = "Manage";
            // 
            // labelAdd
            // 
            this.labelAdd.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAdd.AutoSize = true;
            this.labelAdd.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelAdd.Location = new System.Drawing.Point(402, 150);
            this.labelAdd.Name = "labelAdd";
            this.labelAdd.Size = new System.Drawing.Size(29, 13);
            this.labelAdd.TabIndex = 6;
            this.labelAdd.Text = "Add";
            // 
            // sandGrid
            // 
            this.sandGrid.AllowMultipleSelection = false;
            this.sandGrid.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sandGrid.ColumnClickBehavior = Divelements.SandGrid.ColumnClickBehavior.None;
            this.sandGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnUser,
            this.gridColumnEmail,
            this.gridColumnDescription});
            this.sandGrid.ImageTextSeparation = 1;
            this.sandGrid.Location = new System.Drawing.Point(12, 12);
            this.sandGrid.Name = "sandGrid";
            this.sandGrid.Renderer = windowsXPRenderer1;
            this.sandGrid.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.InitiateDragDrop;
            gridCell1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            gridCell1.ForeColor = System.Drawing.Color.DimGray;
            gridCell1.Text = "Deleted";
            this.sandGrid.Rows.AddRange(new Divelements.SandGrid.GridRow[] {
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell("Brian Nottingham", global::ShipWorks.Properties.Resources.user_16),
                        new Divelements.SandGrid.GridCell("brian@interapptive.com"),
                        new Divelements.SandGrid.GridCell("Administrator")}),
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell("Johnny", global::ShipWorks.Properties.Resources.user_16),
                        new Divelements.SandGrid.GridCell("johnny@interapptive.com"),
                        new Divelements.SandGrid.GridCell()}),
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell("Munzer Sider", global::ShipWorks.Properties.Resources.user_16),
                        new Divelements.SandGrid.GridCell("munz@interapptive.com"),
                        gridCell1}),
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell("Wes Clayton", global::ShipWorks.Properties.Resources.user_16),
                        new Divelements.SandGrid.GridCell("wes@interapptive.com"),
                        new Divelements.SandGrid.GridCell()})});
            this.sandGrid.Size = new System.Drawing.Size(383, 194);
            this.sandGrid.TabIndex = 0;
            this.sandGrid.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnGridSelectionChanged);
            this.sandGrid.RowActivated += new Divelements.SandGrid.GridRowEventHandler(this.OnRowActivated);
            // 
            // gridColumnUser
            // 
            this.gridColumnUser.AllowReorder = false;
            this.gridColumnUser.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnUser.AutoSortType = Divelements.SandGrid.ColumnAutoSortType.None;
            this.gridColumnUser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnUser.ForeColorSource = Divelements.SandGrid.CellForeColorSource.RowCell;
            this.gridColumnUser.HeaderText = "User";
            this.gridColumnUser.MinimumWidth = 120;
            this.gridColumnUser.Width = 120;
            // 
            // gridColumnEmail
            // 
            this.gridColumnEmail.AllowReorder = false;
            this.gridColumnEmail.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnEmail.AutoSortType = Divelements.SandGrid.ColumnAutoSortType.None;
            this.gridColumnEmail.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnEmail.ForeColorSource = Divelements.SandGrid.CellForeColorSource.RowCell;
            this.gridColumnEmail.HeaderText = "Email";
            this.gridColumnEmail.Width = 159;
            // 
            // gridColumnDescription
            // 
            this.gridColumnDescription.AllowReorder = false;
            this.gridColumnDescription.AutoSortType = Divelements.SandGrid.ColumnAutoSortType.None;
            this.gridColumnDescription.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnDescription.ForeColorSource = Divelements.SandGrid.CellForeColorSource.RowCell;
            this.gridColumnDescription.HeaderText = "Description";
            // 
            // menuCopyRightsFrom
            // 
            this.menuCopyRightsFrom.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.menuCopyRightsFrom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.whateverToolStripMenuItem});
            this.menuCopyRightsFrom.Name = "contextMenuStrip";
            this.menuCopyRightsFrom.Size = new System.Drawing.Size(124, 26);
            this.menuCopyRightsFrom.Opening += new System.ComponentModel.CancelEventHandler(this.OnOpeningMenuCopyRightsFrom);
            // 
            // whateverToolStripMenuItem
            // 
            this.whateverToolStripMenuItem.Name = "whateverToolStripMenuItem";
            this.whateverToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.whateverToolStripMenuItem.Text = "Whatever";
            // 
            // viewAuditHistory
            // 
            this.viewAuditHistory.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.viewAuditHistory.Image = global::ShipWorks.Properties.Resources.surveillance_camera;
            this.viewAuditHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.viewAuditHistory.Location = new System.Drawing.Point(405, 57);
            this.viewAuditHistory.Name = "viewAuditHistory";
            this.viewAuditHistory.Size = new System.Drawing.Size(148, 23);
            this.viewAuditHistory.TabIndex = 3;
            this.viewAuditHistory.Text = "View Audit";
            this.viewAuditHistory.UseVisualStyleBackColor = true;
            this.viewAuditHistory.Click += new System.EventHandler(this.OnViewAudit);
            // 
            // newUser
            // 
            this.newUser.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newUser.Image = global::ShipWorks.Properties.Resources.user_add_16;
            this.newUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.newUser.Location = new System.Drawing.Point(405, 166);
            this.newUser.Name = "newUser";
            this.newUser.Size = new System.Drawing.Size(148, 23);
            this.newUser.TabIndex = 7;
            this.newUser.Text = "New User";
            this.newUser.UseVisualStyleBackColor = true;
            this.newUser.Click += new System.EventHandler(this.OnNewUser);
            // 
            // edit
            // 
            this.edit.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.edit.Image = global::ShipWorks.Properties.Resources.edit16;
            this.edit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.edit.Location = new System.Drawing.Point(405, 28);
            this.edit.Name = "edit";
            this.edit.Size = new System.Drawing.Size(148, 23);
            this.edit.TabIndex = 2;
            this.edit.Text = "Edit User";
            this.edit.UseVisualStyleBackColor = true;
            this.edit.Click += new System.EventHandler(this.OnEditUser);
            // 
            // copyRightsFrom
            // 
            this.copyRightsFrom.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.copyRightsFrom.AutoSize = true;
            this.copyRightsFrom.ContextMenuStrip = this.menuCopyRightsFrom;
            this.copyRightsFrom.Image = global::ShipWorks.Properties.Resources.id_card;
            this.copyRightsFrom.Location = new System.Drawing.Point(405, 86);
            this.copyRightsFrom.Name = "copyRightsFrom";
            this.copyRightsFrom.Size = new System.Drawing.Size(148, 23);
            this.copyRightsFrom.SplitButton = false;
            this.copyRightsFrom.SplitContextMenu = this.menuCopyRightsFrom;
            this.copyRightsFrom.TabIndex = 4;
            this.copyRightsFrom.Text = "Copy Rights From";
            this.copyRightsFrom.UseVisualStyleBackColor = true;
            // 
            // delete
            // 
            this.delete.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.delete.Image = global::ShipWorks.Properties.Resources.delete16;
            this.delete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.delete.Location = new System.Drawing.Point(405, 115);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(148, 23);
            this.delete.TabIndex = 5;
            this.delete.Text = "Delete";
            this.delete.UseVisualStyleBackColor = true;
            this.delete.Click += new System.EventHandler(this.OnDelete);
            // 
            // infotipDeletedUsers
            // 
            this.infotipDeletedUsers.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.infotipDeletedUsers.Caption = "ShipWorks users are never really deleted so that ShipWorks can keep the full history for every user.";
            this.infotipDeletedUsers.Location = new System.Drawing.Point(130, 214);
            this.infotipDeletedUsers.Name = "infotipDeletedUsers";
            this.infotipDeletedUsers.Size = new System.Drawing.Size(12, 12);
            this.infotipDeletedUsers.TabIndex = 38;
            this.infotipDeletedUsers.Title = "Deleted Users";
            // 
            // UserManagerDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 247);
            this.Controls.Add(this.infotipDeletedUsers);
            this.Controls.Add(this.delete);
            this.Controls.Add(this.copyRightsFrom);
            this.Controls.Add(this.viewAuditHistory);
            this.Controls.Add(this.sandGrid);
            this.Controls.Add(this.labelManage);
            this.Controls.Add(this.edit);
            this.Controls.Add(this.labelAdd);
            this.Controls.Add(this.showDeletedUsers);
            this.Controls.Add(this.newUser);
            this.Controls.Add(this.close);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(531, 255);
            this.Name = "UserManagerDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manage Users";
            this.Load += new System.EventHandler(this.OnLoad);
            this.menuCopyRightsFrom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button close;
        private System.Windows.Forms.CheckBox showDeletedUsers;
        private System.Windows.Forms.Button newUser;
        private System.Windows.Forms.Label labelManage;
        private System.Windows.Forms.Button edit;
        private System.Windows.Forms.Label labelAdd;
        private Divelements.SandGrid.SandGrid sandGrid;
        private Divelements.SandGrid.GridColumn gridColumnUser;
        private Divelements.SandGrid.GridColumn gridColumnEmail;
        private Divelements.SandGrid.GridColumn gridColumnDescription;
        private System.Windows.Forms.Button viewAuditHistory;
        private System.Windows.Forms.ContextMenuStrip menuCopyRightsFrom;
        private System.Windows.Forms.ToolStripMenuItem whateverToolStripMenuItem;
        private ShipWorks.UI.Controls.DropDownButton copyRightsFrom;
        private System.Windows.Forms.Button delete;
        private UI.Controls.InfoTip infotipDeletedUsers;
    }
}