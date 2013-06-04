namespace ShipWorks.Stores.Content.Panels
{
    partial class PanelControlBase
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
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer2 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.addLink = new ShipWorks.UI.Controls.LinkControl();
            this.entityGrid = new ShipWorks.Stores.Content.Panels.PanelEntityGrid();
            this.gridColumnTest1 = new Divelements.SandGrid.Specialized.GridImageColumn();
            this.gridColumnTest2 = new Divelements.SandGrid.GridColumn();
            this.SuspendLayout();
            // 
            // addLink
            // 
            this.addLink.AutoSize = true;
            this.addLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.addLink.ForeColor = System.Drawing.Color.Blue;
            this.addLink.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addLink.Location = new System.Drawing.Point(209, 46);
            this.addLink.Name = "addLink";
            this.addLink.Size = new System.Drawing.Size(26, 13);
            this.addLink.TabIndex = 3;
            this.addLink.Text = "Add";
            this.addLink.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // entityGrid
            // 
            this.entityGrid.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.entityGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.entityGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnTest1,
            this.gridColumnTest2});
            this.entityGrid.DetailViewSettings = null;
            this.entityGrid.EnableSearching = false;
            this.entityGrid.GridLines = Divelements.SandGrid.GridLinesDisplayType.Both;
            this.entityGrid.LiveResize = false;
            this.entityGrid.Location = new System.Drawing.Point(0, 0);
            this.entityGrid.Name = "entityGrid";
            this.entityGrid.NullRepresentation = "";
            this.entityGrid.PrimaryColumn = this.gridColumnTest2;
            this.entityGrid.Renderer = windowsXPRenderer2;
            this.entityGrid.RowHighlightType = Divelements.SandGrid.RowHighlightType.Full;
            this.entityGrid.ShadeAlternateRows = true;
            this.entityGrid.Size = new System.Drawing.Size(262, 43);
            this.entityGrid.TabIndex = 2;
            this.entityGrid.MinimumNoScrollSizeChanged += new System.EventHandler(this.OnGridMinimumSizeChanged);
            this.entityGrid.RowActivated += new Divelements.SandGrid.GridRowEventHandler(this.OnRowActivated);
            // 
            // gridColumnTest1
            // 
            this.gridColumnTest1.Width = 35;
            // 
            // gridColumnTest2
            // 
            this.gridColumnTest2.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnTest2.HeaderText = "Column";
            this.gridColumnTest2.Width = 227;
            // 
            // PanelControlBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.addLink);
            this.Controls.Add(this.entityGrid);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "PanelControlBase";
            this.Size = new System.Drawing.Size(262, 87);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            this.Resize += new System.EventHandler(this.OnResize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Divelements.SandGrid.Specialized.GridImageColumn gridColumnTest1;
        private Divelements.SandGrid.GridColumn gridColumnTest2;
        protected ShipWorks.UI.Controls.LinkControl addLink;
        protected PanelEntityGrid entityGrid;
    }
}
