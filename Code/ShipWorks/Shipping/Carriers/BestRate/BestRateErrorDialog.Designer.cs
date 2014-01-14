namespace ShipWorks.Shipping.Carriers.BestRate
{
    partial class BestRateErrorDialog
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
            this.closeButton = new System.Windows.Forms.Button();
            this.descriptionMessage = new System.Windows.Forms.Label();
            this.errorGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnIcon = new Divelements.SandGrid.Specialized.GridImageColumn();
            this.gridColumnSeverity = new Divelements.SandGrid.GridColumn();
            this.gridColumnErrorMessage = new Divelements.SandGrid.GridColumn();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeButton.Location = new System.Drawing.Point(424, 258);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 4;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.OnClose);
            // 
            // descriptionMessage
            // 
            this.descriptionMessage.Location = new System.Drawing.Point(8, 9);
            this.descriptionMessage.Name = "descriptionMessage";
            this.descriptionMessage.Size = new System.Drawing.Size(487, 36);
            this.descriptionMessage.TabIndex = 5;
            this.descriptionMessage.Text = "High severity errors will prevent a shipment from being automatically processed w" +
    "ith best rate. \r\nThe shipment can still be processed manually by selecting a rat" +
    "e from the rates grid, though.";
            // 
            // errorGrid
            // 
            this.errorGrid.AllowMultipleSelection = false;
            this.errorGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.errorGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnIcon,
            this.gridColumnSeverity,
            this.gridColumnErrorMessage});
            this.errorGrid.EmptyTextForeColor = System.Drawing.Color.DimGray;
            this.errorGrid.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorGrid.ImageTextSeparation = 1;
            this.errorGrid.Location = new System.Drawing.Point(11, 48);
            this.errorGrid.Name = "errorGrid";
            this.errorGrid.NullRepresentation = "";
            this.errorGrid.PrimaryColumn = this.gridColumnErrorMessage;
            this.errorGrid.Renderer = windowsXPRenderer2;
            this.errorGrid.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.None;
            this.errorGrid.RowHighlightType = Divelements.SandGrid.RowHighlightType.None;
            this.errorGrid.Rows.AddRange(new Divelements.SandGrid.GridRow[] {
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell(global::ShipWorks.Properties.Resources.error16),
                        new Divelements.SandGrid.GridCell("High"),
                        new Divelements.SandGrid.GridCell("There was an error getting rates from FedEx. Your account could not be authentica" +
                                "ted.")}),
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell(global::ShipWorks.Properties.Resources.warning16),
                        new Divelements.SandGrid.GridCell("Low"),
                        new Divelements.SandGrid.GridCell("OnTrac does not service the destination address.")})});
            this.errorGrid.ShadeAlternateRows = true;
            this.errorGrid.Size = new System.Drawing.Size(488, 204);
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
            this.gridColumnIcon.Visible = false;
            this.gridColumnIcon.Width = 25;
            // 
            // gridColumnSeverity
            // 
            this.gridColumnSeverity.HeaderText = "Severity";
            this.gridColumnSeverity.Width = 50;
            // 
            // gridColumnErrorMessage
            // 
            this.gridColumnErrorMessage.AllowEditing = false;
            this.gridColumnErrorMessage.AllowReorder = false;
            this.gridColumnErrorMessage.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnErrorMessage.Clickable = false;
            this.gridColumnErrorMessage.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumnErrorMessage.HeaderText = "Error";
            this.gridColumnErrorMessage.Width = 393;
            // 
            // BestRateErrorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 293);
            this.Controls.Add(this.errorGrid);
            this.Controls.Add(this.descriptionMessage);
            this.Controls.Add(this.closeButton);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BestRateErrorDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Best Rate Errors";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label descriptionMessage;
        private Divelements.SandGrid.SandGrid errorGrid;
        private Divelements.SandGrid.Specialized.GridImageColumn gridColumnIcon;
        private Divelements.SandGrid.GridColumn gridColumnSeverity;
        private Divelements.SandGrid.GridColumn gridColumnErrorMessage;

    }
}