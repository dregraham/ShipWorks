namespace ShipWorks.Templates.Controls
{
    partial class TemplateTreeControl
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
            ShipWorks.UI.Controls.SandGrid.SandGridThemedSelectionRenderer sandGridThemedSelectionRenderer1 = new ShipWorks.UI.Controls.SandGrid.SandGridThemedSelectionRenderer();
            this.panelManageTemplates = new System.Windows.Forms.Panel();
            this.linkManageTemplates = new System.Windows.Forms.Label();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.sandGrid = new ShipWorks.UI.Controls.SandGrid.SandGridTree();
            this.gridColumnTemplate = new ShipWorks.Templates.Controls.TemplateTreeGridColumn();
            this.gridColumnFill = new Divelements.SandGrid.GridColumn();
            this.panelManageTemplates.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sandGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panelManageTemplates
            // 
            this.panelManageTemplates.BackColor = System.Drawing.Color.White;
            this.panelManageTemplates.Controls.Add(this.linkManageTemplates);
            this.panelManageTemplates.Controls.Add(this.kryptonBorderEdge);
            this.panelManageTemplates.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelManageTemplates.Location = new System.Drawing.Point(0, 0);
            this.panelManageTemplates.Name = "panelManageTemplates";
            this.panelManageTemplates.Size = new System.Drawing.Size(233, 26);
            this.panelManageTemplates.TabIndex = 2;
            this.panelManageTemplates.Visible = false;
            // 
            // linkManageTemplates
            // 
            this.linkManageTemplates.AutoSize = true;
            this.linkManageTemplates.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkManageTemplates.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.linkManageTemplates.ForeColor = System.Drawing.Color.Blue;
            this.linkManageTemplates.Location = new System.Drawing.Point(4, 5);
            this.linkManageTemplates.Name = "linkManageTemplates";
            this.linkManageTemplates.Size = new System.Drawing.Size(97, 13);
            this.linkManageTemplates.TabIndex = 4;
            this.linkManageTemplates.Text = "Manage Templates";
            this.linkManageTemplates.Click += new System.EventHandler(this.OnManageTemplates);
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonBorderEdge.AutoSize = false;
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(-6, 23);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(320, 1);
            this.kryptonBorderEdge.TabIndex = 3;
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            // 
            // sandGrid
            // 
            this.sandGrid.AllowDrop = true;
            this.sandGrid.AllowMultipleSelection = false;
            this.sandGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sandGrid.ColumnClickBehavior = Divelements.SandGrid.ColumnClickBehavior.None;
            this.sandGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnTemplate,
            this.gridColumnFill});
            this.sandGrid.CommitOnLoseFocus = true;
            this.sandGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sandGrid.HighlightImages = false;
            this.sandGrid.ImageTextSeparation = 5;
            this.sandGrid.Location = new System.Drawing.Point(0, 26);
            this.sandGrid.Name = "sandGrid";
            this.sandGrid.NullRepresentation = "";
            this.sandGrid.Renderer = sandGridThemedSelectionRenderer1;
            this.sandGrid.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.InitiateDragDrop;
            this.sandGrid.RowHighlightType = Divelements.SandGrid.RowHighlightType.Full;
            this.sandGrid.ShowColumnHeaders = false;
            this.sandGrid.ShowTreeButtons = true;
            this.sandGrid.Size = new System.Drawing.Size(233, 286);
            this.sandGrid.TabIndex = 1;
            this.sandGrid.WhitespaceClickBehavior = Divelements.SandGrid.WhitespaceClickBehavior.None;
            this.sandGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.sandGrid.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnGridSelectionChanged);
            this.sandGrid.AfterEdit += new Divelements.SandGrid.GridAfterEditEventHandler(this.OnAfterEdit);
            this.sandGrid.BeforeEdit += new Divelements.SandGrid.GridBeforeEditEventHandler(this.OnBeforeEdit);
            this.sandGrid.GridRowDropped += new ShipWorks.UI.Controls.SandGrid.GridRowDroppedEventHandler(this.OnGridRowDropped);
            // 
            // gridColumnTemplate
            // 
            this.gridColumnTemplate.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnTemplate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnTemplate.Width = 0;
            // 
            // gridColumnFill
            // 
            this.gridColumnFill.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnFill.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnFill.Width = 233;
            // 
            // TemplateTreeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.sandGrid);
            this.Controls.Add(this.panelManageTemplates);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "TemplateTreeControl";
            this.Size = new System.Drawing.Size(233, 312);
            this.panelManageTemplates.ResumeLayout(false);
            this.panelManageTemplates.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sandGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Controls.SandGrid.SandGridTree sandGrid;
        private ShipWorks.Templates.Controls.TemplateTreeGridColumn gridColumnTemplate;
        private Divelements.SandGrid.GridColumn gridColumnFill;
        private System.Windows.Forms.Panel panelManageTemplates;
        private System.Windows.Forms.Label linkManageTemplates;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
    }
}
