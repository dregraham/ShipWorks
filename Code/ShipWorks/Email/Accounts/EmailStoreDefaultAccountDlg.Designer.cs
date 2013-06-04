namespace ShipWorks.Email.Accounts
{
    partial class EmailStoreDefaultAccountDlg
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
            this.accountDropDown = new ShipWorks.UI.Controls.DropDownButton();
            this.accountsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.storeGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnStore = new Divelements.SandGrid.GridColumn();
            this.gridColumnDefaultAccount = new Divelements.SandGrid.GridColumn();
            this.labelDefaultAccount = new System.Windows.Forms.Label();
            this.close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // accountDropDown
            // 
            this.accountDropDown.AutoSize = true;
            this.accountDropDown.ContextMenuStrip = this.accountsContextMenu;
            this.accountDropDown.Location = new System.Drawing.Point(358, 25);
            this.accountDropDown.Name = "accountDropDown";
            this.accountDropDown.Size = new System.Drawing.Size(94, 23);
            this.accountDropDown.SplitButton = false;
            this.accountDropDown.SplitContextMenu = this.accountsContextMenu;
            this.accountDropDown.TabIndex = 2;
            this.accountDropDown.Text = "Set Account";
            this.accountDropDown.UseVisualStyleBackColor = true;
            // 
            // accountsContextMenu
            // 
            this.accountsContextMenu.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.accountsContextMenu.Name = "accountsContextMenu";
            this.accountsContextMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // storeGrid
            // 
            this.storeGrid.AllowMultipleSelection = false;
            this.storeGrid.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.storeGrid.ColumnClickBehavior = Divelements.SandGrid.ColumnClickBehavior.None;
            this.storeGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnStore,
            this.gridColumnDefaultAccount});
            this.storeGrid.CommitOnLoseFocus = true;
            this.storeGrid.ImageTextSeparation = 1;
            this.storeGrid.Location = new System.Drawing.Point(13, 25);
            this.storeGrid.Name = "storeGrid";
            windowsXPRenderer1.ColumnShade = Divelements.SandGrid.Rendering.ColumnShadeType.None;
            this.storeGrid.Renderer = windowsXPRenderer1;
            this.storeGrid.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.None;
            this.storeGrid.Size = new System.Drawing.Size(339, 113);
            this.storeGrid.TabIndex = 1;
            this.storeGrid.WhitespaceClickBehavior = Divelements.SandGrid.WhitespaceClickBehavior.None;
            this.storeGrid.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnGridSelectionChanged);
            // 
            // gridColumnStore
            // 
            this.gridColumnStore.AllowReorder = false;
            this.gridColumnStore.AutoSortType = Divelements.SandGrid.ColumnAutoSortType.None;
            this.gridColumnStore.Clickable = false;
            this.gridColumnStore.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnStore.HeaderText = "Store";
            this.gridColumnStore.MinimumWidth = 50;
            this.gridColumnStore.Width = 150;
            // 
            // gridColumnDefaultAccount
            // 
            this.gridColumnDefaultAccount.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnDefaultAccount.AutoSortType = Divelements.SandGrid.ColumnAutoSortType.None;
            this.gridColumnDefaultAccount.Clickable = false;
            this.gridColumnDefaultAccount.HeaderText = "Default Account";
            this.gridColumnDefaultAccount.Width = 185;
            // 
            // labelDefaultAccount
            // 
            this.labelDefaultAccount.AutoSize = true;
            this.labelDefaultAccount.Location = new System.Drawing.Point(12, 9);
            this.labelDefaultAccount.Name = "labelDefaultAccount";
            this.labelDefaultAccount.Size = new System.Drawing.Size(173, 13);
            this.labelDefaultAccount.TabIndex = 0;
            this.labelDefaultAccount.Text = "Send from this account by default:";
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(377, 152);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 3;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // EmailStoreDefaultAccountDlg
            // 
            this.AcceptButton = this.close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(464, 187);
            this.Controls.Add(this.close);
            this.Controls.Add(this.labelDefaultAccount);
            this.Controls.Add(this.accountDropDown);
            this.Controls.Add(this.storeGrid);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EmailStoreDefaultAccountDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Default Email Account";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.UI.Controls.DropDownButton accountDropDown;
        private Divelements.SandGrid.SandGrid storeGrid;
        private Divelements.SandGrid.GridColumn gridColumnStore;
        private Divelements.SandGrid.GridColumn gridColumnDefaultAccount;
        private System.Windows.Forms.Label labelDefaultAccount;
        private System.Windows.Forms.ContextMenuStrip accountsContextMenu;
        private System.Windows.Forms.Button close;
    }
}