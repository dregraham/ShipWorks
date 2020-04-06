namespace ShipWorks.Shipping.Editing
{
    partial class ShipmentGridControl
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
            Divelements.SandGrid.Rendering.Office2007Renderer office2007Renderer1 = new Divelements.SandGrid.Rendering.Office2007Renderer();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.ordersToolbar = new System.Windows.Forms.ToolStrip();
            this.chooseMoreButton = new System.Windows.Forms.ToolStripButton();
            this.removeShipmentButton = new System.Windows.Forms.ToolStripButton();
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelShipments = new System.Windows.Forms.Label();
            this.shipmentsToolbar = new System.Windows.Forms.ToolStrip();
            this.newShipmentButton = new System.Windows.Forms.ToolStripButton();
            this.deleteShipmentButton = new System.Windows.Forms.ToolStripButton();
            this.panelStatus = new System.Windows.Forms.Panel();
            this.kryptonStatusPanel = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.labelGridSettings = new System.Windows.Forms.Label();
            this.labelSelectedShipments = new System.Windows.Forms.Label();
            this.labelStatusEtch = new System.Windows.Forms.Label();
            this.labelTotalShipments = new System.Windows.Forms.Label();
            this.shipmentsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuCreateNewShipment = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDeleteShipment = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuChooseMore = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRemoveFromList = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCopyShipment = new System.Windows.Forms.ToolStripDropDownButton();
            this.menuCopyShipment = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCopyShipmentReturn = new System.Windows.Forms.ToolStripMenuItem();
            this.entityGrid = new ShipWorks.Data.Grid.EntityGrid();
            this.gridColumn1 = new Divelements.SandGrid.GridColumn();
            this.gridColumn2 = new Divelements.SandGrid.GridColumn();
            this.panelBottom.SuspendLayout();
            this.ordersToolbar.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.shipmentsToolbar.SuspendLayout();
            this.panelStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonStatusPanel)).BeginInit();
            this.kryptonStatusPanel.Panel.SuspendLayout();
            this.kryptonStatusPanel.SuspendLayout();
            this.shipmentsContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.ordersToolbar);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 365);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(324, 28);
            this.panelBottom.TabIndex = 6;
            // 
            // ordersToolbar
            // 
            this.ordersToolbar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ordersToolbar.BackColor = System.Drawing.Color.Transparent;
            this.ordersToolbar.Dock = System.Windows.Forms.DockStyle.None;
            this.ordersToolbar.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.ordersToolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ordersToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chooseMoreButton,
            this.removeShipmentButton});
            this.ordersToolbar.Location = new System.Drawing.Point(112, 1);
            this.ordersToolbar.Name = "ordersToolbar";
            this.ordersToolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ordersToolbar.Size = new System.Drawing.Size(215, 25);
            this.ordersToolbar.TabIndex = 2;
            this.ordersToolbar.Text = "toolStrip1";
            // 
            // chooseMoreButton
            // 
            this.chooseMoreButton.AutoToolTip = false;
            this.chooseMoreButton.Image = global::ShipWorks.Properties.Resources.add16;
            this.chooseMoreButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chooseMoreButton.Name = "chooseMoreButton";
            this.chooseMoreButton.Size = new System.Drawing.Size(96, 22);
            this.chooseMoreButton.Text = "Choose More";
            this.chooseMoreButton.Click += new System.EventHandler(this.OnChooseMore);
            // 
            // removeShipmentButton
            // 
            this.removeShipmentButton.AutoToolTip = false;
            this.removeShipmentButton.Image = global::ShipWorks.Properties.Resources.forbidden;
            this.removeShipmentButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeShipmentButton.Name = "removeShipmentButton";
            this.removeShipmentButton.Size = new System.Drawing.Size(116, 22);
            this.removeShipmentButton.Text = "Remove From List";
            this.removeShipmentButton.Click += new System.EventHandler(this.OnRemoveShipments);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.labelShipments);
            this.panelTop.Controls.Add(this.shipmentsToolbar);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(324, 27);
            this.panelTop.TabIndex = 5;
            // 
            // labelShipments
            // 
            this.labelShipments.AutoSize = true;
            this.labelShipments.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelShipments.Location = new System.Drawing.Point(1, 6);
            this.labelShipments.Name = "labelShipments";
            this.labelShipments.Size = new System.Drawing.Size(67, 13);
            this.labelShipments.TabIndex = 2;
            this.labelShipments.Text = "Shipments";
            // 
            // shipmentsToolbar
            // 
            this.shipmentsToolbar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.shipmentsToolbar.BackColor = System.Drawing.Color.Transparent;
            this.shipmentsToolbar.Dock = System.Windows.Forms.DockStyle.None;
            this.shipmentsToolbar.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.shipmentsToolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.shipmentsToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newShipmentButton,
            this.menuItemCopyShipment,
            this.deleteShipmentButton});
            this.shipmentsToolbar.Location = new System.Drawing.Point(70, 0);
            this.shipmentsToolbar.Name = "shipmentsToolbar";
            this.shipmentsToolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.shipmentsToolbar.Size = new System.Drawing.Size(257, 25);
            this.shipmentsToolbar.TabIndex = 0;
            this.shipmentsToolbar.Text = "toolStrip1";
            // 
            // newShipmentButton
            // 
            this.newShipmentButton.AutoToolTip = false;
            this.newShipmentButton.Image = global::ShipWorks.Properties.Resources.box_closed_add16;
            this.newShipmentButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newShipmentButton.Name = "newShipmentButton";
            this.newShipmentButton.Size = new System.Drawing.Size(102, 22);
            this.newShipmentButton.Text = "New Shipment";
            this.newShipmentButton.Click += new System.EventHandler(this.OnAddShipmentToOrder);
            // 
            // menuItemCopyShipment
            // 
            this.menuItemCopyShipment.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCopyShipment,
            this.menuCopyShipmentReturn});
            this.menuItemCopyShipment.Image = global::ShipWorks.Properties.Resources.box_closed_redo_16_16;
            this.menuItemCopyShipment.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuItemCopyShipment.Name = "menuItemCopyShipment";
            this.menuItemCopyShipment.Size = new System.Drawing.Size(92, 22);
            this.menuItemCopyShipment.Text = "Ship Again";
            // 
            // menuCopyShipment
            // 
            this.menuCopyShipment.Image = global::ShipWorks.Properties.Resources.box_closed_redo_16_16;
            this.menuCopyShipment.Name = "menuCopyShipment";
            this.menuCopyShipment.Size = new System.Drawing.Size(182, 22);
            this.menuCopyShipment.Text = "&Ship Again";
            this.menuCopyShipment.Click += new System.EventHandler(this.OnCopy);
            // 
            // menuCopyShipmentReturn
            // 
            this.menuCopyShipmentReturn.Image = global::ShipWorks.Properties.Resources.box_next;
            this.menuCopyShipmentReturn.Name = "menuCopyShipmentReturn";
            this.menuCopyShipmentReturn.Size = new System.Drawing.Size(182, 22);
            this.menuCopyShipmentReturn.Text = "Ship Again as Return";
            this.menuCopyShipmentReturn.Click += new System.EventHandler(this.OnCopyAsReturn);
            // 
            // deleteShipmentButton
            // 
            this.deleteShipmentButton.AutoToolTip = false;
            this.deleteShipmentButton.Image = global::ShipWorks.Properties.Resources.box_closed_delete;
            this.deleteShipmentButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteShipmentButton.Name = "deleteShipmentButton";
            this.deleteShipmentButton.Size = new System.Drawing.Size(60, 22);
            this.deleteShipmentButton.Text = "Delete";
            this.deleteShipmentButton.Click += new System.EventHandler(this.OnDeleteShipments);
            // 
            // panelStatus
            // 
            this.panelStatus.Controls.Add(this.kryptonStatusPanel);
            this.panelStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelStatus.Location = new System.Drawing.Point(0, 345);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(324, 20);
            this.panelStatus.TabIndex = 8;
            // 
            // kryptonStatusPanel
            // 
            this.kryptonStatusPanel.AutoSize = true;
            this.kryptonStatusPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonStatusPanel.GroupBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.ContextMenuItemImage;
            this.kryptonStatusPanel.GroupBorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.ButtonStandalone;
            this.kryptonStatusPanel.Location = new System.Drawing.Point(0, 0);
            this.kryptonStatusPanel.Name = "kryptonStatusPanel";
            // 
            // kryptonStatusPanel.Panel
            // 
            this.kryptonStatusPanel.Panel.Controls.Add(this.labelGridSettings);
            this.kryptonStatusPanel.Panel.Controls.Add(this.labelSelectedShipments);
            this.kryptonStatusPanel.Panel.Controls.Add(this.labelStatusEtch);
            this.kryptonStatusPanel.Panel.Controls.Add(this.labelTotalShipments);
            this.kryptonStatusPanel.Size = new System.Drawing.Size(324, 20);
            this.kryptonStatusPanel.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)(((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonStatusPanel.TabIndex = 1;
            // 
            // labelGridSettings
            // 
            this.labelGridSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelGridSettings.AutoSize = true;
            this.labelGridSettings.BackColor = System.Drawing.Color.Transparent;
            this.labelGridSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelGridSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGridSettings.ForeColor = System.Drawing.Color.Blue;
            this.labelGridSettings.Location = new System.Drawing.Point(250, 2);
            this.labelGridSettings.Name = "labelGridSettings";
            this.labelGridSettings.Size = new System.Drawing.Size(68, 13);
            this.labelGridSettings.TabIndex = 44;
            this.labelGridSettings.Text = "Grid Settings";
            this.labelGridSettings.Click += new System.EventHandler(this.OnGridSettings);
            // 
            // labelSelectedShipments
            // 
            this.labelSelectedShipments.AutoSize = true;
            this.labelSelectedShipments.BackColor = System.Drawing.Color.Transparent;
            this.labelSelectedShipments.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelSelectedShipments.Location = new System.Drawing.Point(88, 2);
            this.labelSelectedShipments.Name = "labelSelectedShipments";
            this.labelSelectedShipments.Size = new System.Drawing.Size(67, 13);
            this.labelSelectedShipments.TabIndex = 43;
            this.labelSelectedShipments.Text = "Selected: 12";
            // 
            // labelStatusEtch
            // 
            this.labelStatusEtch.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelStatusEtch.Location = new System.Drawing.Point(85, 3);
            this.labelStatusEtch.Name = "labelStatusEtch";
            this.labelStatusEtch.Size = new System.Drawing.Size(2, 14);
            this.labelStatusEtch.TabIndex = 42;
            this.labelStatusEtch.Text = "Etch";
            // 
            // labelTotalShipments
            // 
            this.labelTotalShipments.AutoSize = true;
            this.labelTotalShipments.BackColor = System.Drawing.Color.Transparent;
            this.labelTotalShipments.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelTotalShipments.Location = new System.Drawing.Point(3, 2);
            this.labelTotalShipments.Name = "labelTotalShipments";
            this.labelTotalShipments.Size = new System.Drawing.Size(81, 13);
            this.labelTotalShipments.TabIndex = 0;
            this.labelTotalShipments.Text = "Shipments: 345";
            // 
            // shipmentsContextMenu
            // 
            this.shipmentsContextMenu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.shipmentsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCreateNewShipment,
            this.menuDeleteShipment,
            this.menuSep1,
            this.menuChooseMore,
            this.menuRemoveFromList,
            this.menuSep3,
            this.menuCopy});
            this.shipmentsContextMenu.Name = "shipmentsContextMenu";
            this.shipmentsContextMenu.Size = new System.Drawing.Size(272, 126);
            this.shipmentsContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.OnShipmentMenuOpening);
            // 
            // menuCreateNewShipment
            // 
            this.menuCreateNewShipment.Image = global::ShipWorks.Properties.Resources.box_closed_add16;
            this.menuCreateNewShipment.Name = "menuCreateNewShipment";
            this.menuCreateNewShipment.Size = new System.Drawing.Size(271, 22);
            this.menuCreateNewShipment.Text = "Create new Shipment for Order 14543";
            this.menuCreateNewShipment.Click += new System.EventHandler(this.OnAddShipmentToOrder);
            // 
            // menuDeleteShipment
            // 
            this.menuDeleteShipment.Image = global::ShipWorks.Properties.Resources.box_closed_delete;
            this.menuDeleteShipment.Name = "menuDeleteShipment";
            this.menuDeleteShipment.Size = new System.Drawing.Size(271, 22);
            this.menuDeleteShipment.Text = "Delete Shipment";
            this.menuDeleteShipment.Click += new System.EventHandler(this.OnDeleteShipments);
            // 
            // menuSep1
            // 
            this.menuSep1.Name = "menuSep1";
            this.menuSep1.Size = new System.Drawing.Size(268, 6);
            // 
            // menuChooseMore
            // 
            this.menuChooseMore.Image = global::ShipWorks.Properties.Resources.add16;
            this.menuChooseMore.Name = "menuChooseMore";
            this.menuChooseMore.Size = new System.Drawing.Size(271, 22);
            this.menuChooseMore.Text = "Choose More";
            this.menuChooseMore.Click += new System.EventHandler(this.OnChooseMore);
            // 
            // menuRemoveFromList
            // 
            this.menuRemoveFromList.Image = global::ShipWorks.Properties.Resources.forbidden;
            this.menuRemoveFromList.Name = "menuRemoveFromList";
            this.menuRemoveFromList.Size = new System.Drawing.Size(271, 22);
            this.menuRemoveFromList.Text = "Remove from List";
            this.menuRemoveFromList.Click += new System.EventHandler(this.OnRemoveShipments);
            // 
            // menuSep3
            // 
            this.menuSep3.Name = "menuSep3";
            this.menuSep3.Size = new System.Drawing.Size(268, 6);
            // 
            // menuCopy
            // 
            this.menuCopy.Image = global::ShipWorks.Properties.Resources.copy;
            this.menuCopy.Name = "menuCopy";
            this.menuCopy.Size = new System.Drawing.Size(271, 22);
            this.menuCopy.Text = "Copy";
            // 
            // entityGrid
            // 
            this.entityGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2});
            this.entityGrid.ContextMenuStrip = this.shipmentsContextMenu;
            this.entityGrid.DetailViewSettings = null;
            this.entityGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entityGrid.GridLines = Divelements.SandGrid.GridLinesDisplayType.Both;
            this.entityGrid.LiveResize = false;
            this.entityGrid.Location = new System.Drawing.Point(0, 27);
            this.entityGrid.Name = "entityGrid";
            office2007Renderer1.ColorScheme = Divelements.SandGrid.Rendering.Office2007ColorScheme.Black;
            office2007Renderer1.ColumnShade = Divelements.SandGrid.Rendering.ColumnShadeType.None;
            this.entityGrid.Renderer = office2007Renderer1;
            this.entityGrid.ShadeAlternateRows = true;
            this.entityGrid.Size = new System.Drawing.Size(324, 318);
            this.entityGrid.SortColumn = this.gridColumn2;
            this.entityGrid.TabIndex = 7;
            this.entityGrid.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnGridSelectionChanged);
            this.entityGrid.SortChanged += new Divelements.SandGrid.GridEventHandler(this.OnGridSortChanged);
            this.entityGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // gridColumn1
            // 
            this.gridColumn1.HeaderText = "Order #";
            // 
            // gridColumn2
            // 
            this.gridColumn2.HeaderText = "Last Name";
            // 
            // ShipmentGridControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.entityGrid);
            this.Controls.Add(this.panelStatus);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ShipmentGridControl";
            this.Size = new System.Drawing.Size(324, 393);
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ordersToolbar.ResumeLayout(false);
            this.ordersToolbar.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.shipmentsToolbar.ResumeLayout(false);
            this.shipmentsToolbar.PerformLayout();
            this.panelStatus.ResumeLayout(false);
            this.panelStatus.PerformLayout();
            this.kryptonStatusPanel.Panel.ResumeLayout(false);
            this.kryptonStatusPanel.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonStatusPanel)).EndInit();
            this.kryptonStatusPanel.ResumeLayout(false);
            this.shipmentsContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.Data.Grid.EntityGrid entityGrid;
        private Divelements.SandGrid.GridColumn gridColumn1;
        private Divelements.SandGrid.GridColumn gridColumn2;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.ToolStrip ordersToolbar;
        private System.Windows.Forms.ToolStripButton chooseMoreButton;
        private System.Windows.Forms.ToolStripButton removeShipmentButton;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labelShipments;
        private System.Windows.Forms.ToolStrip shipmentsToolbar;
        private System.Windows.Forms.ToolStripButton newShipmentButton;
        private System.Windows.Forms.ToolStripButton deleteShipmentButton;
        private System.Windows.Forms.Panel panelStatus;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup kryptonStatusPanel;
        private System.Windows.Forms.Label labelGridSettings;
        private System.Windows.Forms.Label labelSelectedShipments;
        private System.Windows.Forms.Label labelStatusEtch;
        private System.Windows.Forms.Label labelTotalShipments;
        private System.Windows.Forms.ContextMenuStrip shipmentsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuCreateNewShipment;
        private System.Windows.Forms.ToolStripMenuItem menuDeleteShipment;
        private System.Windows.Forms.ToolStripSeparator menuSep1;
        private System.Windows.Forms.ToolStripMenuItem menuChooseMore;
        private System.Windows.Forms.ToolStripMenuItem menuRemoveFromList;
        private System.Windows.Forms.ToolStripSeparator menuSep3;
        private System.Windows.Forms.ToolStripMenuItem menuCopy;
        private System.Windows.Forms.ToolStripDropDownButton menuItemCopyShipment;
        private System.Windows.Forms.ToolStripMenuItem menuCopyShipment;
        private System.Windows.Forms.ToolStripMenuItem menuCopyShipmentReturn;
    }
}
