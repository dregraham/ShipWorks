namespace ShipWorks.ApplicationCore.Appearance
{
    partial class GridMenuEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridMenuEditor));
            ShipWorks.UI.Controls.SandGrid.SandGridThemedSelectionRenderer sandGridThemedSelectionRenderer1 = new ShipWorks.UI.Controls.SandGrid.SandGridThemedSelectionRenderer();
            ShipWorks.UI.Controls.SandGrid.SandGridThemedSelectionRenderer sandGridThemedSelectionRenderer2 = new ShipWorks.UI.Controls.SandGrid.SandGridThemedSelectionRenderer();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.moveDown = new System.Windows.Forms.Button();
            this.moveUp = new System.Windows.Forms.Button();
            this.imageListTabs = new System.Windows.Forms.ImageList(this.components);
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageOrders = new System.Windows.Forms.TabPage();
            this.gridOrders = new ShipWorks.UI.Controls.SandGrid.SandGridDragDrop();
            this.gridColumn2 = new Divelements.SandGrid.GridColumn();
            this.sandGridDragDropColumn1 = new ShipWorks.UI.Controls.SandGrid.SandGridDragDropColumn();
            this.tabPageCustomers = new System.Windows.Forms.TabPage();
            this.gridCustomers = new ShipWorks.UI.Controls.SandGrid.SandGridDragDrop();
            this.gridColumn = new Divelements.SandGrid.GridColumn();
            this.gridColumn1 = new ShipWorks.UI.Controls.SandGrid.SandGridDragDropColumn();
            this.labelMove = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tabPageOrders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.gridOrders)).BeginInit();
            this.tabPageCustomers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.gridCustomers)).BeginInit();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(225, 499);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 4;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(306, 499);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 5;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // moveDown
            // 
            this.moveDown.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moveDown.Image = global::ShipWorks.Properties.Resources.arrow_down_blue;
            this.moveDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.moveDown.Location = new System.Drawing.Point(231, 79);
            this.moveDown.Name = "moveDown";
            this.moveDown.Size = new System.Drawing.Size(150, 23);
            this.moveDown.TabIndex = 3;
            this.moveDown.Text = "Move Down";
            this.moveDown.UseVisualStyleBackColor = true;
            this.moveDown.Click += new System.EventHandler(this.OnMoveDown);
            // 
            // moveUp
            // 
            this.moveUp.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moveUp.Image = global::ShipWorks.Properties.Resources.arrow_up_blue;
            this.moveUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.moveUp.Location = new System.Drawing.Point(231, 51);
            this.moveUp.Name = "moveUp";
            this.moveUp.Size = new System.Drawing.Size(150, 23);
            this.moveUp.TabIndex = 2;
            this.moveUp.Text = "Move Up";
            this.moveUp.UseVisualStyleBackColor = true;
            this.moveUp.Click += new System.EventHandler(this.OnMoveUp);
            // 
            // imageListTabs
            // 
            this.imageListTabs.ImageStream = ((System.Windows.Forms.ImageListStreamer) (resources.GetObject("imageListTabs.ImageStream")));
            this.imageListTabs.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTabs.Images.SetKeyName(0, "user11.png");
            this.imageListTabs.Images.SetKeyName(1, "form_blue.png");
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageOrders);
            this.tabControl.Controls.Add(this.tabPageCustomers);
            this.tabControl.ImageList = this.imageListTabs;
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(213, 481);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.OnSelectedTabChanged);
            // 
            // tabPageOrders
            // 
            this.tabPageOrders.Controls.Add(this.gridOrders);
            this.tabPageOrders.ImageIndex = 1;
            this.tabPageOrders.Location = new System.Drawing.Point(4, 23);
            this.tabPageOrders.Name = "tabPageOrders";
            this.tabPageOrders.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOrders.Size = new System.Drawing.Size(205, 454);
            this.tabPageOrders.TabIndex = 0;
            this.tabPageOrders.Text = "Order Grid";
            this.tabPageOrders.UseVisualStyleBackColor = true;
            // 
            // gridOrders
            // 
            this.gridOrders.AllowDrag = true;
            this.gridOrders.AllowDrop = true;
            this.gridOrders.AllowMultipleSelection = false;
            this.gridOrders.CheckBoxes = true;
            this.gridOrders.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumn2,
            this.sandGridDragDropColumn1});
            this.gridOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridOrders.EnableSearching = false;
            this.gridOrders.ImageTextSeparation = 1;
            this.gridOrders.Location = new System.Drawing.Point(3, 3);
            this.gridOrders.Name = "gridOrders";
            this.gridOrders.Renderer = sandGridThemedSelectionRenderer1;
            this.gridOrders.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.InitiateDragDrop;
            this.gridOrders.Rows.AddRange(new Divelements.SandGrid.GridRow[] {
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell(),
                        new Divelements.SandGrid.GridCell("Nice", global::ShipWorks.Properties.Resources.arrow_up_blue)})});
            this.gridOrders.ShowColumnHeaders = false;
            this.gridOrders.Size = new System.Drawing.Size(199, 448);
            this.gridOrders.TabIndex = 0;
            this.gridOrders.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnGridSelectionChanged);
            this.gridOrders.GridRowDropped += new ShipWorks.UI.Controls.SandGrid.GridRowDroppedEventHandler(this.OnGridRowDropped);
            // 
            // gridColumn2
            // 
            this.gridColumn2.HeaderText = "gridColumn";
            this.gridColumn2.Width = 20;
            // 
            // sandGridDragDropColumn1
            // 
            this.sandGridDragDropColumn1.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.sandGridDragDropColumn1.HeaderText = "gridColumn1";
            this.sandGridDragDropColumn1.Width = 175;
            // 
            // tabPageCustomers
            // 
            this.tabPageCustomers.Controls.Add(this.gridCustomers);
            this.tabPageCustomers.ImageIndex = 0;
            this.tabPageCustomers.Location = new System.Drawing.Point(4, 23);
            this.tabPageCustomers.Name = "tabPageCustomers";
            this.tabPageCustomers.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCustomers.Size = new System.Drawing.Size(205, 454);
            this.tabPageCustomers.TabIndex = 1;
            this.tabPageCustomers.Text = "Customer Grid";
            this.tabPageCustomers.UseVisualStyleBackColor = true;
            // 
            // gridCustomers
            // 
            this.gridCustomers.AllowDrag = true;
            this.gridCustomers.AllowDrop = true;
            this.gridCustomers.AllowMultipleSelection = false;
            this.gridCustomers.CheckBoxes = true;
            this.gridCustomers.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumn,
            this.gridColumn1});
            this.gridCustomers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCustomers.EnableSearching = false;
            this.gridCustomers.ImageTextSeparation = 1;
            this.gridCustomers.Location = new System.Drawing.Point(3, 3);
            this.gridCustomers.Name = "gridCustomers";
            this.gridCustomers.Renderer = sandGridThemedSelectionRenderer2;
            this.gridCustomers.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.InitiateDragDrop;
            this.gridCustomers.Rows.AddRange(new Divelements.SandGrid.GridRow[] {
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell(),
                        new Divelements.SandGrid.GridCell("Nice", global::ShipWorks.Properties.Resources.arrow_up_blue)})});
            this.gridCustomers.ShowColumnHeaders = false;
            this.gridCustomers.Size = new System.Drawing.Size(199, 448);
            this.gridCustomers.TabIndex = 0;
            this.gridCustomers.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnGridSelectionChanged);
            this.gridCustomers.GridRowDropped += new ShipWorks.UI.Controls.SandGrid.GridRowDroppedEventHandler(this.OnGridRowDropped);
            // 
            // gridColumn
            // 
            this.gridColumn.HeaderText = "gridColumn";
            this.gridColumn.Width = 20;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumn1.HeaderText = "gridColumn1";
            this.gridColumn1.Width = 175;
            // 
            // labelMove
            // 
            this.labelMove.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMove.AutoSize = true;
            this.labelMove.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelMove.Location = new System.Drawing.Point(230, 35);
            this.labelMove.Name = "labelMove";
            this.labelMove.Size = new System.Drawing.Size(38, 13);
            this.labelMove.TabIndex = 1;
            this.labelMove.Text = "Move";
            // 
            // GridMenuEditor
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(393, 534);
            this.Controls.Add(this.labelMove);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.moveDown);
            this.Controls.Add(this.moveUp);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GridMenuEditor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Customize Menu";
            this.tabControl.ResumeLayout(false);
            this.tabPageOrders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.gridOrders)).EndInit();
            this.tabPageCustomers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.gridCustomers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private ShipWorks.UI.Controls.SandGrid.SandGridDragDrop gridCustomers;
        private System.Windows.Forms.Button moveDown;
        private System.Windows.Forms.Button moveUp;
        private Divelements.SandGrid.GridColumn gridColumn;
        private ShipWorks.UI.Controls.SandGrid.SandGridDragDropColumn gridColumn1;
        private System.Windows.Forms.ImageList imageListTabs;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageOrders;
        private System.Windows.Forms.TabPage tabPageCustomers;
        private ShipWorks.UI.Controls.SandGrid.SandGridDragDrop gridOrders;
        private Divelements.SandGrid.GridColumn gridColumn2;
        private ShipWorks.UI.Controls.SandGrid.SandGridDragDropColumn sandGridDragDropColumn1;
        private System.Windows.Forms.Label labelMove;
    }
}