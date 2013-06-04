namespace ShipWorks.Shipping.Profiles
{
    partial class ShippingProfileManagerDlg
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
            this.sandGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnName = new Divelements.SandGrid.GridColumn();
            this.delete = new System.Windows.Forms.Button();
            this.edit = new System.Windows.Forms.Button();
            this.add = new System.Windows.Forms.Button();
            this.menuList = new ShipWorks.UI.Controls.MenuList();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(373, 241);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(86, 23);
            this.close.TabIndex = 4;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // sandGrid
            // 
            this.sandGrid.AllowMultipleSelection = false;
            this.sandGrid.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sandGrid.ColumnClickBehavior = Divelements.SandGrid.ColumnClickBehavior.None;
            this.sandGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnName});
            this.sandGrid.Location = new System.Drawing.Point(138, 12);
            this.sandGrid.Name = "sandGrid";
            this.sandGrid.NullRepresentation = "";
            this.sandGrid.Renderer = windowsXPRenderer1;
            this.sandGrid.ShowColumnHeaders = false;
            this.sandGrid.Size = new System.Drawing.Size(229, 213);
            this.sandGrid.TabIndex = 0;
            this.sandGrid.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnChangeSelectedProfile);
            this.sandGrid.RowActivated += new Divelements.SandGrid.GridRowEventHandler(this.OnActivate);
            // 
            // gridColumnName
            // 
            this.gridColumnName.AllowEditing = false;
            this.gridColumnName.AllowReorder = false;
            this.gridColumnName.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnName.AutoSortType = Divelements.SandGrid.ColumnAutoSortType.None;
            this.gridColumnName.Clickable = false;
            this.gridColumnName.HeaderText = "Name";
            this.gridColumnName.Width = 225;
            // 
            // delete
            // 
            this.delete.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.delete.Image = global::ShipWorks.Properties.Resources.delete16;
            this.delete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.delete.Location = new System.Drawing.Point(373, 71);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(86, 23);
            this.delete.TabIndex = 3;
            this.delete.Text = "Delete";
            this.delete.UseVisualStyleBackColor = true;
            this.delete.Click += new System.EventHandler(this.OnDelete);
            // 
            // edit
            // 
            this.edit.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.edit.Image = global::ShipWorks.Properties.Resources.edit16;
            this.edit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.edit.Location = new System.Drawing.Point(373, 12);
            this.edit.Name = "edit";
            this.edit.Size = new System.Drawing.Size(86, 23);
            this.edit.TabIndex = 1;
            this.edit.Text = "Edit";
            this.edit.UseVisualStyleBackColor = true;
            this.edit.Click += new System.EventHandler(this.OnEdit);
            // 
            // add
            // 
            this.add.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.add.Image = global::ShipWorks.Properties.Resources.add16;
            this.add.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.add.Location = new System.Drawing.Point(373, 41);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(86, 23);
            this.add.TabIndex = 6;
            this.add.Text = "New";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.OnAdd);
            // 
            // menuList
            // 
            this.menuList.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.menuList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.menuList.FormattingEnabled = true;
            this.menuList.IntegralHeight = false;
            this.menuList.ItemHeight = 26;
            this.menuList.Location = new System.Drawing.Point(12, 12);
            this.menuList.Name = "menuList";
            this.menuList.Size = new System.Drawing.Size(120, 213);
            this.menuList.TabIndex = 5;
            this.menuList.SelectedIndexChanged += new System.EventHandler(this.OnChangeShipmentType);
            // 
            // ShippingProfileManagerDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(471, 276);
            this.Controls.Add(this.add);
            this.Controls.Add(this.menuList);
            this.Controls.Add(this.delete);
            this.Controls.Add(this.edit);
            this.Controls.Add(this.sandGrid);
            this.Controls.Add(this.close);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(352, 312);
            this.Name = "ShippingProfileManagerDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Shipping Profiles";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button close;
        private Divelements.SandGrid.SandGrid sandGrid;
        private System.Windows.Forms.Button delete;
        private System.Windows.Forms.Button edit;
        private Divelements.SandGrid.GridColumn gridColumnName;
        private ShipWorks.UI.Controls.MenuList menuList;
        private System.Windows.Forms.Button add;
    }
}