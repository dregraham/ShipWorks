namespace ShipWorks.Shipping.Carriers.BestRate
{
    partial class BestRateMissingRatesDialog
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
            Divelements.SandGrid.GridRow gridRow1 = new Divelements.SandGrid.GridRow();
            Divelements.SandGrid.GridRow gridRow2 = new Divelements.SandGrid.GridRow();
            this.closeButton = new System.Windows.Forms.Button();
            this.errorGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnIcon = new Divelements.SandGrid.Specialized.GridImageColumn();
            this.gridColumnProvider = new Divelements.SandGrid.Specialized.GridImageColumn();
            this.gridColumnMessage = new Divelements.SandGrid.GridColumn();
            this.header = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeButton.Location = new System.Drawing.Point(539, 192);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 4;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.OnClose);
            // 
            // errorGrid
            // 
            this.errorGrid.AllowMultipleSelection = false;
            this.errorGrid.AllowRowResize = true;
            this.errorGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.errorGrid.ColumnClickBehavior = Divelements.SandGrid.ColumnClickBehavior.None;
            this.errorGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnIcon,
            this.gridColumnProvider,
            this.gridColumnMessage});
            this.errorGrid.EmptyTextForeColor = System.Drawing.Color.DimGray;
            this.errorGrid.EnableSearching = false;
            this.errorGrid.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorGrid.ImageTextSeparation = 1;
            this.errorGrid.Location = new System.Drawing.Point(11, 30);
            this.errorGrid.Name = "errorGrid";
            this.errorGrid.NullRepresentation = "";
            this.errorGrid.PrimaryColumn = this.gridColumnMessage;
            this.errorGrid.Renderer = windowsXPRenderer1;
            this.errorGrid.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.None;
            this.errorGrid.RowHighlightType = Divelements.SandGrid.RowHighlightType.None;
            gridRow1.AllowEditing = false;
            gridRow1.Cells.AddRange(new Divelements.SandGrid.GridCell[] {
            new Divelements.SandGrid.GridCell(global::ShipWorks.Properties.Resources.error16),
            new Divelements.SandGrid.GridCell(global::ShipWorks.Properties.Resources.box_closed16),
            new Divelements.SandGrid.GridCell("There was an error getting rates from FedEx. Your account could not be authentica" +
                    "ted.")});
            gridRow1.ContentsUnknown = true;
            gridRow1.Height = 0;
            gridRow2.Cells.AddRange(new Divelements.SandGrid.GridCell[] {
            new Divelements.SandGrid.GridCell(global::ShipWorks.Properties.Resources.warning16),
            new Divelements.SandGrid.GridCell(global::ShipWorks.Properties.Resources.box_closed16),
            new Divelements.SandGrid.GridCell("OnTrac does not service the destination address.")});
            gridRow2.Height = 0;
            this.errorGrid.Rows.AddRange(new Divelements.SandGrid.GridRow[] {
            gridRow1,
            gridRow2});
            this.errorGrid.ShadeAlternateRows = true;
            this.errorGrid.Size = new System.Drawing.Size(603, 154);
            this.errorGrid.StretchPrimaryGrid = false;
            this.errorGrid.TabIndex = 6;
            // 
            // gridColumnIcon
            // 
            this.gridColumnIcon.AllowEditing = false;
            this.gridColumnIcon.AllowReorder = false;
            this.gridColumnIcon.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnIcon.AutoSizeIncludeHeader = true;
            this.gridColumnIcon.CellHorizontalAlignment = System.Drawing.StringAlignment.Center;
            this.gridColumnIcon.Clickable = false;
            this.gridColumnIcon.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumnIcon.MinimumWidth = 25;
            this.gridColumnIcon.Width = 50;
            // 
            // gridColumnProvider
            // 
            this.gridColumnProvider.AllowEditing = false;
            this.gridColumnProvider.AllowReorder = false;
            this.gridColumnProvider.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnProvider.AutoSizeIncludeHeader = true;
            this.gridColumnProvider.CellHorizontalAlignment = System.Drawing.StringAlignment.Center;
            this.gridColumnProvider.Clickable = false;
            this.gridColumnProvider.HeaderText = "Provider";
            this.gridColumnProvider.MinimumWidth = 50;
            this.gridColumnProvider.Width = 50;
            // 
            // gridColumnMessage
            // 
            this.gridColumnMessage.AllowEditing = false;
            this.gridColumnMessage.AllowReorder = false;
            this.gridColumnMessage.AllowWrap = true;
            this.gridColumnMessage.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnMessage.Clickable = false;
            this.gridColumnMessage.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumnMessage.HeaderText = "Message";
            this.gridColumnMessage.Width = 385;
            // 
            // header
            // 
            this.header.AutoSize = true;
            this.header.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.header.Location = new System.Drawing.Point(8, 10);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(245, 13);
            this.header.TabIndex = 15;
            this.header.Text = "ShipWorks could not get all possible rates:";
            // 
            // BestRateMissingRatesDialog
            // 
            this.AcceptButton = this.closeButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(626, 227);
            this.Controls.Add(this.header);
            this.Controls.Add(this.errorGrid);
            this.Controls.Add(this.closeButton);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(390, 235);
            this.Name = "BestRateMissingRatesDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Shipment Rates";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private Divelements.SandGrid.SandGrid errorGrid;
        private Divelements.SandGrid.Specialized.GridImageColumn gridColumnIcon;
        private Divelements.SandGrid.GridColumn gridColumnMessage;
        private Divelements.SandGrid.Specialized.GridImageColumn gridColumnProvider;
        private System.Windows.Forms.Label header;

    }
}