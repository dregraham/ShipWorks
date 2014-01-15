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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BestRateMissingRatesDialog));
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
            this.warningIcon = new System.Windows.Forms.PictureBox();
            this.errorLabel = new System.Windows.Forms.Label();
            this.errorImage = new System.Windows.Forms.PictureBox();
            this.informationLabel = new System.Windows.Forms.Label();
            this.informationIcon = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.header = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.warningIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.informationIcon)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeButton.Location = new System.Drawing.Point(425, 318);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 4;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.OnClose);
            // 
            // descriptionMessage
            // 
            this.descriptionMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionMessage.Location = new System.Drawing.Point(8, 29);
            this.descriptionMessage.Name = "descriptionMessage";
            this.descriptionMessage.Size = new System.Drawing.Size(492, 49);
            this.descriptionMessage.TabIndex = 5;
            this.descriptionMessage.Text = resources.GetString("descriptionMessage.Text");
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
            this.gridColumnErrorMessage});
            this.errorGrid.EmptyTextForeColor = System.Drawing.Color.DimGray;
            this.errorGrid.EnableSearching = false;
            this.errorGrid.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorGrid.ImageTextSeparation = 1;
            this.errorGrid.Location = new System.Drawing.Point(11, 76);
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
            // gridColumnErrorMessage
            // 
            this.gridColumnErrorMessage.AllowEditing = false;
            this.gridColumnErrorMessage.AllowReorder = false;
            this.gridColumnErrorMessage.AllowWrap = true;
            this.gridColumnErrorMessage.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnErrorMessage.Clickable = false;
            this.gridColumnErrorMessage.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumnErrorMessage.HeaderText = "Error";
            this.gridColumnErrorMessage.Width = 385;
            // 
            // warningLabel
            // 
            this.warningLabel.Location = new System.Drawing.Point(230, 5);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(48, 14);
            this.warningLabel.TabIndex = 11;
            this.warningLabel.Text = "Warning";
            this.warningLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // warningIcon
            // 
            this.warningIcon.Image = global::ShipWorks.Properties.Resources.warning16;
            this.warningIcon.Location = new System.Drawing.Point(212, 4);
            this.warningIcon.Name = "warningIcon";
            this.warningIcon.Size = new System.Drawing.Size(16, 16);
            this.warningIcon.TabIndex = 10;
            this.warningIcon.TabStop = false;
            // 
            // errorLabel
            // 
            this.errorLabel.Location = new System.Drawing.Point(37, 5);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(31, 14);
            this.errorLabel.TabIndex = 9;
            this.errorLabel.Text = "Error";
            this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // errorImage
            // 
            this.errorImage.Image = global::ShipWorks.Properties.Resources.error16;
            this.errorImage.Location = new System.Drawing.Point(19, 4);
            this.errorImage.Name = "errorImage";
            this.errorImage.Size = new System.Drawing.Size(16, 16);
            this.errorImage.TabIndex = 8;
            this.errorImage.TabStop = false;
            // 
            // informationLabel
            // 
            this.informationLabel.Location = new System.Drawing.Point(395, 5);
            this.informationLabel.Name = "informationLabel";
            this.informationLabel.Size = new System.Drawing.Size(72, 15);
            this.informationLabel.TabIndex = 13;
            this.informationLabel.Text = "Information";
            this.informationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // informationIcon
            // 
            this.informationIcon.ErrorImage = null;
            this.informationIcon.Image = global::ShipWorks.Properties.Resources.information16;
            this.informationIcon.Location = new System.Drawing.Point(378, 4);
            this.informationIcon.Name = "informationIcon";
            this.informationIcon.Size = new System.Drawing.Size(16, 16);
            this.informationIcon.TabIndex = 12;
            this.informationIcon.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.errorLabel);
            this.panel1.Controls.Add(this.informationLabel);
            this.panel1.Controls.Add(this.errorImage);
            this.panel1.Controls.Add(this.informationIcon);
            this.panel1.Controls.Add(this.warningIcon);
            this.panel1.Controls.Add(this.warningLabel);
            this.panel1.Location = new System.Drawing.Point(11, 279);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(489, 26);
            this.panel1.TabIndex = 14;
            // 
            // header
            // 
            this.header.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.header.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.header.Location = new System.Drawing.Point(8, 9);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(492, 20);
            this.header.TabIndex = 15;
            this.header.Text = "ShipWorks could not get rates from all providers.";
            // 
            // BestRateMissingRatesDialog
            // 
            this.AcceptButton = this.closeButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(512, 353);
            this.Controls.Add(this.header);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.errorGrid);
            this.Controls.Add(this.descriptionMessage);
            this.Controls.Add(this.closeButton);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BestRateMissingRatesDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Missing Rates";
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.warningIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.informationIcon)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label descriptionMessage;
        private Divelements.SandGrid.SandGrid errorGrid;
        private Divelements.SandGrid.Specialized.GridImageColumn gridColumnIcon;
        private Divelements.SandGrid.GridColumn gridColumnErrorMessage;
        private System.Windows.Forms.Label warningLabel;
        private System.Windows.Forms.PictureBox warningIcon;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.PictureBox errorImage;
        private Divelements.SandGrid.Specialized.GridImageColumn gridColumnProvider;
        private System.Windows.Forms.Label informationLabel;
        private System.Windows.Forms.PictureBox informationIcon;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label header;

    }
}