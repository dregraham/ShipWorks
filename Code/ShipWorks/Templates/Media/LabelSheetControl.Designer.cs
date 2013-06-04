namespace ShipWorks.Templates.Media
{
    partial class LabelSheetControl
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
            this.customSheets = new System.Windows.Forms.Button();
            this.quantity = new System.Windows.Forms.Label();
            this.pageSize = new System.Windows.Forms.Label();
            this.dimensions = new System.Windows.Forms.Label();
            this.labelQuantity = new System.Windows.Forms.Label();
            this.labelPageSize = new System.Windows.Forms.Label();
            this.labelDimensions = new System.Windows.Forms.Label();
            this.groupSheetType = new System.Windows.Forms.GroupBox();
            this.sheetNameChooser = new ShipWorks.UI.Controls.DropDownButton();
            this.labelSheetName = new System.Windows.Forms.Label();
            this.editCustomSheet = new System.Windows.Forms.Button();
            this.groupSheetType.SuspendLayout();
            this.SuspendLayout();
            // 
            // customSheets
            // 
            this.customSheets.Location = new System.Drawing.Point(3, 134);
            this.customSheets.Name = "customSheets";
            this.customSheets.Size = new System.Drawing.Size(144, 23);
            this.customSheets.TabIndex = 1;
            this.customSheets.Text = "Custom Label Sheets...";
            this.customSheets.UseVisualStyleBackColor = true;
            this.customSheets.Click += new System.EventHandler(this.OnCustomSheets);
            // 
            // quantity
            // 
            this.quantity.AutoSize = true;
            this.quantity.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (50)))), ((int) (((byte) (50)))), ((int) (((byte) (50)))));
            this.quantity.Location = new System.Drawing.Point(72, 98);
            this.quantity.Name = "quantity";
            this.quantity.Size = new System.Drawing.Size(142, 13);
            this.quantity.TabIndex = 8;
            this.quantity.Text = "3 rows  x  12 cols,  36 labels";
            // 
            // pageSize
            // 
            this.pageSize.AutoSize = true;
            this.pageSize.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (50)))), ((int) (((byte) (50)))), ((int) (((byte) (50)))));
            this.pageSize.Location = new System.Drawing.Point(72, 74);
            this.pageSize.Name = "pageSize";
            this.pageSize.Size = new System.Drawing.Size(102, 13);
            this.pageSize.TabIndex = 6;
            this.pageSize.Text = "Letter (8 ½ x 11 in)";
            // 
            // dimensions
            // 
            this.dimensions.AutoSize = true;
            this.dimensions.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (50)))), ((int) (((byte) (50)))), ((int) (((byte) (50)))));
            this.dimensions.Location = new System.Drawing.Point(72, 50);
            this.dimensions.Name = "dimensions";
            this.dimensions.Size = new System.Drawing.Size(77, 13);
            this.dimensions.TabIndex = 4;
            this.dimensions.Text = "1.33\"  x  4.00\"";
            // 
            // labelQuantity
            // 
            this.labelQuantity.AutoSize = true;
            this.labelQuantity.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (50)))), ((int) (((byte) (50)))), ((int) (((byte) (50)))));
            this.labelQuantity.Location = new System.Drawing.Point(8, 98);
            this.labelQuantity.Name = "labelQuantity";
            this.labelQuantity.Size = new System.Drawing.Size(53, 13);
            this.labelQuantity.TabIndex = 7;
            this.labelQuantity.Text = "Quantity:";
            // 
            // labelPageSize
            // 
            this.labelPageSize.AutoSize = true;
            this.labelPageSize.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (50)))), ((int) (((byte) (50)))), ((int) (((byte) (50)))));
            this.labelPageSize.Location = new System.Drawing.Point(8, 74);
            this.labelPageSize.Name = "labelPageSize";
            this.labelPageSize.Size = new System.Drawing.Size(57, 13);
            this.labelPageSize.TabIndex = 5;
            this.labelPageSize.Text = "Page Size:";
            // 
            // labelDimensions
            // 
            this.labelDimensions.AutoSize = true;
            this.labelDimensions.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (50)))), ((int) (((byte) (50)))), ((int) (((byte) (50)))));
            this.labelDimensions.Location = new System.Drawing.Point(8, 50);
            this.labelDimensions.Name = "labelDimensions";
            this.labelDimensions.Size = new System.Drawing.Size(64, 13);
            this.labelDimensions.TabIndex = 3;
            this.labelDimensions.Text = "Dimensions:";
            // 
            // groupSheetType
            // 
            this.groupSheetType.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSheetType.Controls.Add(this.quantity);
            this.groupSheetType.Controls.Add(this.sheetNameChooser);
            this.groupSheetType.Controls.Add(this.pageSize);
            this.groupSheetType.Controls.Add(this.labelSheetName);
            this.groupSheetType.Controls.Add(this.dimensions);
            this.groupSheetType.Controls.Add(this.labelQuantity);
            this.groupSheetType.Controls.Add(this.editCustomSheet);
            this.groupSheetType.Controls.Add(this.labelPageSize);
            this.groupSheetType.Controls.Add(this.labelDimensions);
            this.groupSheetType.Location = new System.Drawing.Point(3, 3);
            this.groupSheetType.Name = "groupSheetType";
            this.groupSheetType.Size = new System.Drawing.Size(312, 121);
            this.groupSheetType.TabIndex = 0;
            this.groupSheetType.TabStop = false;
            this.groupSheetType.Text = "Label Sheet";
            // 
            // sheetNameChooser
            // 
            this.sheetNameChooser.AutoSize = true;
            this.sheetNameChooser.Location = new System.Drawing.Point(141, 21);
            this.sheetNameChooser.Name = "sheetNameChooser";
            this.sheetNameChooser.Size = new System.Drawing.Size(67, 23);
            this.sheetNameChooser.SplitButton = false;
            this.sheetNameChooser.TabIndex = 1;
            this.sheetNameChooser.Text = "Choose";
            this.sheetNameChooser.UseVisualStyleBackColor = true;
            this.sheetNameChooser.DropDownShowing += new System.EventHandler(this.OnSelectSheetDropDownShowing);
            // 
            // labelSheetName
            // 
            this.labelSheetName.AutoSize = true;
            this.labelSheetName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelSheetName.Location = new System.Drawing.Point(8, 26);
            this.labelSheetName.Name = "labelSheetName";
            this.labelSheetName.Size = new System.Drawing.Size(129, 13);
            this.labelSheetName.TabIndex = 0;
            this.labelSheetName.Text = "Avery - 2156 Address";
            // 
            // editCustomSheet
            // 
            this.editCustomSheet.Location = new System.Drawing.Point(212, 21);
            this.editCustomSheet.Name = "editCustomSheet";
            this.editCustomSheet.Size = new System.Drawing.Size(67, 23);
            this.editCustomSheet.TabIndex = 2;
            this.editCustomSheet.Text = "Edit...";
            this.editCustomSheet.UseVisualStyleBackColor = true;
            this.editCustomSheet.Click += new System.EventHandler(this.OnEditCustomSheet);
            // 
            // LabelSheetControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customSheets);
            this.Controls.Add(this.groupSheetType);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "LabelSheetControl";
            this.Size = new System.Drawing.Size(326, 168);
            this.groupSheetType.ResumeLayout(false);
            this.groupSheetType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label quantity;
        private System.Windows.Forms.Label pageSize;
        private System.Windows.Forms.Label dimensions;
        private System.Windows.Forms.Label labelQuantity;
        private System.Windows.Forms.Label labelPageSize;
        private System.Windows.Forms.Label labelDimensions;
        private System.Windows.Forms.Button customSheets;
        private System.Windows.Forms.GroupBox groupSheetType;
        private System.Windows.Forms.Button editCustomSheet;
        private System.Windows.Forms.Label labelSheetName;
        private ShipWorks.UI.Controls.DropDownButton sheetNameChooser;
    }
}
