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
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer1 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            Divelements.SandGrid.GridRow gridRow1 = new Divelements.SandGrid.GridRow();
            Divelements.SandGrid.GridRow gridRow2 = new Divelements.SandGrid.GridRow();
            this.closeButton = new System.Windows.Forms.Button();
            this.descriptionMessage = new System.Windows.Forms.Label();
            this.errorGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnIcon = new Divelements.SandGrid.Specialized.GridImageColumn();
            this.gridColumnProvider = new Divelements.SandGrid.Specialized.GridImageColumn();
            this.gridColumnErrorMessage = new Divelements.SandGrid.GridColumn();
            this.warningLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.errorLabel = new System.Windows.Forms.Label();
            this.errorImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorImage)).BeginInit();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeButton.Location = new System.Drawing.Point(425, 287);
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
            this.descriptionMessage.Text = "Errors will prevent a shipment from being automatically processed with best rate." +
    " \r\nThe shipment can still be processed manually by selecting a rate from the rat" +
    "es grid, though.";
            // 
            // errorGrid
            // 
            this.errorGrid.AllowMultipleSelection = false;
            this.errorGrid.AllowRowResize = true;
            this.errorGrid.ColumnClickBehavior = Divelements.SandGrid.ColumnClickBehavior.None;
            this.errorGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnIcon,
            this.gridColumnProvider,
            this.gridColumnErrorMessage});
            this.errorGrid.EmptyTextForeColor = System.Drawing.Color.DimGray;
            this.errorGrid.EnableSearching = false;
            this.errorGrid.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorGrid.ImageTextSeparation = 1;
            this.errorGrid.Location = new System.Drawing.Point(11, 48);
            this.errorGrid.Name = "errorGrid";
            this.errorGrid.NullRepresentation = "";
            this.errorGrid.PrimaryColumn = this.gridColumnErrorMessage;
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
            this.errorGrid.Size = new System.Drawing.Size(489, 204);
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
            this.gridColumnIcon.HeaderText = "Severity";
            this.gridColumnIcon.MinimumWidth = 25;
            this.gridColumnIcon.ResizeBehavior = Divelements.SandGrid.ElementResizeBehavior.None;
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
            this.gridColumnProvider.ResizeBehavior = Divelements.SandGrid.ElementResizeBehavior.None;
            this.gridColumnProvider.Width = 50;
            // 
            // gridColumnErrorMessage
            // 
            this.gridColumnErrorMessage.AllowEditing = false;
            this.gridColumnErrorMessage.AllowReorder = false;
            this.gridColumnErrorMessage.AllowWrap = true;
            this.gridColumnErrorMessage.Clickable = false;
            this.gridColumnErrorMessage.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumnErrorMessage.HeaderText = "Error";
            this.gridColumnErrorMessage.ResizeBehavior = Divelements.SandGrid.ElementResizeBehavior.None;
            this.gridColumnErrorMessage.Width = 385;
            // 
            // warningLabel
            // 
            this.warningLabel.Location = new System.Drawing.Point(100, 259);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(48, 14);
            this.warningLabel.TabIndex = 11;
            this.warningLabel.Text = "Warning";
            this.warningLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.warning16;
            this.pictureBox1.Location = new System.Drawing.Point(82, 258);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // errorLabel
            // 
            this.errorLabel.Location = new System.Drawing.Point(29, 259);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(31, 14);
            this.errorLabel.TabIndex = 9;
            this.errorLabel.Text = "Error";
            this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // errorImage
            // 
            this.errorImage.Image = global::ShipWorks.Properties.Resources.error16;
            this.errorImage.Location = new System.Drawing.Point(12, 258);
            this.errorImage.Name = "errorImage";
            this.errorImage.Size = new System.Drawing.Size(16, 16);
            this.errorImage.TabIndex = 8;
            this.errorImage.TabStop = false;
            // 
            // BestRateErrorDialog
            // 
            this.AcceptButton = this.closeButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(512, 322);
            this.Controls.Add(this.warningLabel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.errorImage);
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label descriptionMessage;
        private Divelements.SandGrid.SandGrid errorGrid;
        private Divelements.SandGrid.Specialized.GridImageColumn gridColumnIcon;
        private Divelements.SandGrid.GridColumn gridColumnErrorMessage;
        private System.Windows.Forms.Label warningLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.PictureBox errorImage;
        private Divelements.SandGrid.Specialized.GridImageColumn gridColumnProvider;

    }
}