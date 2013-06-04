namespace ShipWorks.Email.Outlook
{
    partial class EmailOutboundMultipleRelationsDlg
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
            this.entityGrid = new ShipWorks.Data.Grid.Paging.PagedEntityGrid();
            this.gridColumnTest1 = new Divelements.SandGrid.Specialized.GridImageColumn();
            this.gridColumnTest2 = new Divelements.SandGrid.GridColumn();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(152, 300);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 0;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // entityGrid
            // 
            this.entityGrid.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.entityGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnTest1,
            this.gridColumnTest2});
            this.entityGrid.DetailViewSettings = null;
            this.entityGrid.EnableSearching = false;
            this.entityGrid.GridLines = Divelements.SandGrid.GridLinesDisplayType.Both;
            this.entityGrid.LiveResize = false;
            this.entityGrid.Location = new System.Drawing.Point(12, 12);
            this.entityGrid.Name = "entityGrid";
            this.entityGrid.NullRepresentation = "";
            this.entityGrid.PrimaryColumn = this.gridColumnTest2;
            this.entityGrid.Renderer = windowsXPRenderer1;
            this.entityGrid.RowHighlightType = Divelements.SandGrid.RowHighlightType.Full;
            this.entityGrid.ShadeAlternateRows = true;
            this.entityGrid.Size = new System.Drawing.Size(215, 282);
            this.entityGrid.TabIndex = 3;
            // 
            // gridColumnTest1
            // 
            this.gridColumnTest1.Width = 35;
            // 
            // gridColumnTest2
            // 
            this.gridColumnTest2.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnTest2.HeaderText = "Column";
            this.gridColumnTest2.Width = 176;
            // 
            // EmailOutboundMultipleRelationsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(239, 335);
            this.Controls.Add(this.entityGrid);
            this.Controls.Add(this.close);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EmailOutboundMultipleRelationsDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Email Related To";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button close;
        private Divelements.SandGrid.Specialized.GridImageColumn gridColumnTest1;
        private Divelements.SandGrid.GridColumn gridColumnTest2;
        private ShipWorks.Data.Grid.Paging.PagedEntityGrid entityGrid;
    }
}