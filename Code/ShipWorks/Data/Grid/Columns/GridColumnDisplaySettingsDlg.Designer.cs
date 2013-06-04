namespace ShipWorks.Data.Grid.Columns
{
    partial class GridColumnDisplaySettingsDlg
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
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer2 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.groupBoxSettings = new System.Windows.Forms.GroupBox();
            this.sandGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnName = new ShipWorks.Data.Grid.Columns.GridColumnDefinitionNameColumn();
            this.gridColumnPreview = new ShipWorks.Data.Grid.Columns.GridColumnDisplaySettingsDlg.GridPreviewColumn();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(494, 395);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 0;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(575, 395);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // groupBoxSettings
            // 
            this.groupBoxSettings.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSettings.Location = new System.Drawing.Point(402, 6);
            this.groupBoxSettings.Name = "groupBoxSettings";
            this.groupBoxSettings.Size = new System.Drawing.Size(244, 125);
            this.groupBoxSettings.TabIndex = 9;
            this.groupBoxSettings.TabStop = false;
            this.groupBoxSettings.Text = "Column Format";
            // 
            // sandGrid
            // 
            this.sandGrid.AllowMultipleSelection = false;
            this.sandGrid.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sandGrid.ColumnClickBehavior = Divelements.SandGrid.ColumnClickBehavior.None;
            this.sandGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnName,
            this.gridColumnPreview});
            this.sandGrid.GridLines = Divelements.SandGrid.GridLinesDisplayType.Both;
            this.sandGrid.Location = new System.Drawing.Point(12, 12);
            this.sandGrid.Name = "sandGrid";
            this.sandGrid.NullRepresentation = "";
            this.sandGrid.Renderer = windowsXPRenderer2;
            this.sandGrid.Size = new System.Drawing.Size(383, 370);
            this.sandGrid.TabIndex = 4;
            this.sandGrid.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnGridSelectionChanged);
            // 
            // gridColumnName
            // 
            this.gridColumnName.AllowReorder = false;
            this.gridColumnName.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnName.Clickable = false;
            this.gridColumnName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnName.HeaderText = "Column";
            this.gridColumnName.ResizeBehavior = Divelements.SandGrid.ElementResizeBehavior.None;
            this.gridColumnName.Width = 199;
            // 
            // gridColumnPreview
            // 
            this.gridColumnPreview.Clickable = false;
            this.gridColumnPreview.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnPreview.HeaderText = "Sample";
            this.gridColumnPreview.ResizeBehavior = Divelements.SandGrid.ElementResizeBehavior.None;
            this.gridColumnPreview.Width = 180;
            // 
            // GridColumnDisplaySettingsDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(654, 430);
            this.Controls.Add(this.groupBoxSettings);
            this.Controls.Add(this.sandGrid);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 5000);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(593, 400);
            this.Name = "GridColumnDisplaySettingsDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Grid Column Formatting";
            this.Load += new System.EventHandler(this.OnLoad);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private Divelements.SandGrid.SandGrid sandGrid;
        private GridColumnDefinitionNameColumn gridColumnName;
        private GridPreviewColumn gridColumnPreview;
        private System.Windows.Forms.GroupBox groupBoxSettings;
    }
}