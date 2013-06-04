namespace ShipWorks.Email.Accounts
{
    partial class EmailAccountManagerDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmailAccountManagerDlg));
            this.close = new System.Windows.Forms.Button();
            this.accountGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnAlias = new Divelements.SandGrid.GridColumn();
            this.gridColumnName = new Divelements.SandGrid.GridColumn();
            this.gridColumnAddress = new Divelements.SandGrid.GridColumn();
            this.add = new System.Windows.Forms.Button();
            this.delete = new System.Windows.Forms.Button();
            this.edit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.configureDefaultAccounts = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(493, 240);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 6;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // accountGrid
            // 
            this.accountGrid.AllowMultipleSelection = false;
            this.accountGrid.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.accountGrid.ColumnClickBehavior = Divelements.SandGrid.ColumnClickBehavior.None;
            this.accountGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnAlias,
            this.gridColumnName,
            this.gridColumnAddress});
            this.accountGrid.CommitOnLoseFocus = true;
            this.accountGrid.ImageTextSeparation = 1;
            this.accountGrid.Location = new System.Drawing.Point(12, 12);
            this.accountGrid.Name = "accountGrid";
            windowsXPRenderer1.ColumnShade = Divelements.SandGrid.Rendering.ColumnShadeType.None;
            this.accountGrid.Renderer = windowsXPRenderer1;
            this.accountGrid.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.None;
            this.accountGrid.Size = new System.Drawing.Size(456, 177);
            this.accountGrid.TabIndex = 0;
            this.accountGrid.WhitespaceClickBehavior = Divelements.SandGrid.WhitespaceClickBehavior.None;
            this.accountGrid.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnChangeSelectedAccount);
            this.accountGrid.RowActivated += new Divelements.SandGrid.GridRowEventHandler(this.OnActivate);
            // 
            // gridColumnAlias
            // 
            this.gridColumnAlias.AllowReorder = false;
            this.gridColumnAlias.AutoSortType = Divelements.SandGrid.ColumnAutoSortType.None;
            this.gridColumnAlias.Clickable = false;
            this.gridColumnAlias.HeaderText = "Alias";
            this.gridColumnAlias.Width = 125;
            // 
            // gridColumnName
            // 
            this.gridColumnName.AllowReorder = false;
            this.gridColumnName.AutoSortType = Divelements.SandGrid.ColumnAutoSortType.None;
            this.gridColumnName.Clickable = false;
            this.gridColumnName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnName.HeaderText = "Your Name";
            this.gridColumnName.MinimumWidth = 50;
            this.gridColumnName.Width = 150;
            // 
            // gridColumnAddress
            // 
            this.gridColumnAddress.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnAddress.AutoSortType = Divelements.SandGrid.ColumnAutoSortType.None;
            this.gridColumnAddress.Clickable = false;
            this.gridColumnAddress.HeaderText = "Email Address";
            this.gridColumnAddress.Width = 177;
            // 
            // add
            // 
            this.add.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.add.Image = ((System.Drawing.Image) (resources.GetObject("add.Image")));
            this.add.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.add.Location = new System.Drawing.Point(474, 42);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(94, 23);
            this.add.TabIndex = 2;
            this.add.Text = "New";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.OnNewEmailAccount);
            // 
            // delete
            // 
            this.delete.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.delete.Image = ((System.Drawing.Image) (resources.GetObject("delete.Image")));
            this.delete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.delete.Location = new System.Drawing.Point(474, 71);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(94, 23);
            this.delete.TabIndex = 3;
            this.delete.Text = "Delete";
            this.delete.UseVisualStyleBackColor = true;
            this.delete.Click += new System.EventHandler(this.OnDelete);
            // 
            // edit
            // 
            this.edit.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.edit.Image = ((System.Drawing.Image) (resources.GetObject("edit.Image")));
            this.edit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.edit.Location = new System.Drawing.Point(474, 12);
            this.edit.Name = "edit";
            this.edit.Size = new System.Drawing.Size(94, 23);
            this.edit.TabIndex = 1;
            this.edit.Text = "Edit";
            this.edit.UseVisualStyleBackColor = true;
            this.edit.Click += new System.EventHandler(this.OnEdit);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 199);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(191, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Default account to use for each store:";
            // 
            // configureDefaultAccounts
            // 
            this.configureDefaultAccounts.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.configureDefaultAccounts.Location = new System.Drawing.Point(28, 217);
            this.configureDefaultAccounts.Name = "configureDefaultAccounts";
            this.configureDefaultAccounts.Size = new System.Drawing.Size(88, 23);
            this.configureDefaultAccounts.TabIndex = 5;
            this.configureDefaultAccounts.Text = "Configure...";
            this.configureDefaultAccounts.UseVisualStyleBackColor = true;
            this.configureDefaultAccounts.Click += new System.EventHandler(this.OnConfigureDefaultAccounts);
            // 
            // EmailAccountManagerDlg
            // 
            this.AcceptButton = this.close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(580, 275);
            this.Controls.Add(this.configureDefaultAccounts);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.accountGrid);
            this.Controls.Add(this.add);
            this.Controls.Add(this.delete);
            this.Controls.Add(this.edit);
            this.Controls.Add(this.close);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(596, 290);
            this.Name = "EmailAccountManagerDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Email Accounts";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button close;
        private Divelements.SandGrid.SandGrid accountGrid;
        private Divelements.SandGrid.GridColumn gridColumnName;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.Button delete;
        private System.Windows.Forms.Button edit;
        private Divelements.SandGrid.GridColumn gridColumnAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button configureDefaultAccounts;
        private Divelements.SandGrid.GridColumn gridColumnAlias;
    }
}