namespace ShipWorks.Data.Import.Spreadsheet.Types.Csv.Editing
{
    partial class GenericCsvMapEditorControl
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
            this.labelMappings = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.labelDates = new System.Windows.Forms.Label();
            this.labelMapName = new System.Windows.Forms.Label();
            this.labelFileFormat = new System.Windows.Forms.Label();
            this.fileFormatSummary = new System.Windows.Forms.TextBox();
            this.dateFormatSummary = new System.Windows.Forms.TextBox();
            this.editFileFormat = new System.Windows.Forms.Button();
            this.editDateFormat = new System.Windows.Forms.Button();
            this.updateSourceColumns = new System.Windows.Forms.Button();
            this.sourceColumnSummary = new System.Windows.Forms.TextBox();
            this.labelSourceColumns = new System.Windows.Forms.Label();
            this.optionControl = new ShipWorks.UI.Controls.OptionControl();
            this.optionPageOrder = new ShipWorks.UI.Controls.OptionPage();
            this.orderMappings = new ShipWorks.Data.Import.Spreadsheet.Editing.GenericSpreadsheetFieldMappingGroupControl();
            this.optionPageAddress = new ShipWorks.UI.Controls.OptionPage();
            this.optionPageItems = new ShipWorks.UI.Controls.OptionPage();
            this.optionControl.SuspendLayout();
            this.optionPageOrder.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelMappings
            // 
            this.labelMappings.AutoSize = true;
            this.labelMappings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelMappings.Location = new System.Drawing.Point(4, 133);
            this.labelMappings.Name = "labelMappings";
            this.labelMappings.Size = new System.Drawing.Size(106, 13);
            this.labelMappings.TabIndex = 11;
            this.labelMappings.Text = "Column Mappings";
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(90, 27);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(175, 21);
            this.name.TabIndex = 10;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelName.Location = new System.Drawing.Point(6, 6);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(81, 13);
            this.labelName.TabIndex = 9;
            this.labelName.Text = "Map Settings";
            // 
            // labelDates
            // 
            this.labelDates.AutoSize = true;
            this.labelDates.Location = new System.Drawing.Point(18, 107);
            this.labelDates.Name = "labelDates";
            this.labelDates.Size = new System.Drawing.Size(69, 13);
            this.labelDates.TabIndex = 20;
            this.labelDates.Text = "Date format:";
            // 
            // labelMapName
            // 
            this.labelMapName.AutoSize = true;
            this.labelMapName.Location = new System.Drawing.Point(49, 30);
            this.labelMapName.Name = "labelMapName";
            this.labelMapName.Size = new System.Drawing.Size(38, 13);
            this.labelMapName.TabIndex = 22;
            this.labelMapName.Text = "Name:";
            // 
            // labelFileFormat
            // 
            this.labelFileFormat.AutoSize = true;
            this.labelFileFormat.Location = new System.Drawing.Point(25, 80);
            this.labelFileFormat.Name = "labelFileFormat";
            this.labelFileFormat.Size = new System.Drawing.Size(62, 13);
            this.labelFileFormat.TabIndex = 24;
            this.labelFileFormat.Text = "File format:";
            // 
            // fileFormatSummary
            // 
            this.fileFormatSummary.Location = new System.Drawing.Point(90, 77);
            this.fileFormatSummary.Name = "fileFormatSummary";
            this.fileFormatSummary.ReadOnly = true;
            this.fileFormatSummary.Size = new System.Drawing.Size(297, 21);
            this.fileFormatSummary.TabIndex = 25;
            this.fileFormatSummary.Text = "Delimited by (,), Using (\") as Quotes";
            // 
            // dateFormatSummary
            // 
            this.dateFormatSummary.Location = new System.Drawing.Point(90, 104);
            this.dateFormatSummary.Name = "dateFormatSummary";
            this.dateFormatSummary.ReadOnly = true;
            this.dateFormatSummary.Size = new System.Drawing.Size(297, 21);
            this.dateFormatSummary.TabIndex = 26;
            this.dateFormatSummary.Text = "Automatic, local timezone";
            // 
            // editFileFormat
            // 
            this.editFileFormat.Location = new System.Drawing.Point(393, 75);
            this.editFileFormat.Name = "editFileFormat";
            this.editFileFormat.Size = new System.Drawing.Size(75, 23);
            this.editFileFormat.TabIndex = 28;
            this.editFileFormat.Text = "Edit...";
            this.editFileFormat.UseVisualStyleBackColor = true;
            this.editFileFormat.Click += new System.EventHandler(this.OnEditFileFormat);
            // 
            // editDateFormat
            // 
            this.editDateFormat.Location = new System.Drawing.Point(393, 102);
            this.editDateFormat.Name = "editDateFormat";
            this.editDateFormat.Size = new System.Drawing.Size(75, 23);
            this.editDateFormat.TabIndex = 29;
            this.editDateFormat.Text = "Edit...";
            this.editDateFormat.UseVisualStyleBackColor = true;
            this.editDateFormat.Click += new System.EventHandler(this.OnEditDateFormat);
            // 
            // updateSourceColumns
            // 
            this.updateSourceColumns.Location = new System.Drawing.Point(312, 49);
            this.updateSourceColumns.Name = "updateSourceColumns";
            this.updateSourceColumns.Size = new System.Drawing.Size(75, 23);
            this.updateSourceColumns.TabIndex = 32;
            this.updateSourceColumns.Text = "Update...";
            this.updateSourceColumns.UseVisualStyleBackColor = true;
            this.updateSourceColumns.Click += new System.EventHandler(this.OnChangeSource);
            // 
            // sourceColumnSummary
            // 
            this.sourceColumnSummary.Location = new System.Drawing.Point(90, 51);
            this.sourceColumnSummary.Name = "sourceColumnSummary";
            this.sourceColumnSummary.ReadOnly = true;
            this.sourceColumnSummary.Size = new System.Drawing.Size(216, 21);
            this.sourceColumnSummary.TabIndex = 31;
            this.sourceColumnSummary.Text = "22 columns";
            // 
            // labelSourceColumns
            // 
            this.labelSourceColumns.AutoSize = true;
            this.labelSourceColumns.Location = new System.Drawing.Point(2, 54);
            this.labelSourceColumns.Name = "labelSourceColumns";
            this.labelSourceColumns.Size = new System.Drawing.Size(85, 13);
            this.labelSourceColumns.TabIndex = 30;
            this.labelSourceColumns.Text = "Source columns:";
            // 
            // optionControl
            // 
            this.optionControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.optionControl.Controls.Add(this.optionPageOrder);
            this.optionControl.Controls.Add(this.optionPageAddress);
            this.optionControl.Controls.Add(this.optionPageItems);
            this.optionControl.Location = new System.Drawing.Point(6, 150);
            this.optionControl.Name = "optionControl";
            this.optionControl.SelectedIndex = 0;
            this.optionControl.Size = new System.Drawing.Size(626, 327);
            this.optionControl.TabIndex = 8;
            // 
            // optionPageOrder
            // 
            this.optionPageOrder.BackColor = System.Drawing.Color.White;
            this.optionPageOrder.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.optionPageOrder.Controls.Add(this.orderMappings);
            this.optionPageOrder.Location = new System.Drawing.Point(153, 0);
            this.optionPageOrder.Name = "optionPageOrder";
            this.optionPageOrder.Padding = new System.Windows.Forms.Padding(3);
            this.optionPageOrder.Size = new System.Drawing.Size(473, 327);
            this.optionPageOrder.TabIndex = 1;
            this.optionPageOrder.Text = "Order";
            // 
            // orderMappings
            // 
            this.orderMappings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.orderMappings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.orderMappings.Location = new System.Drawing.Point(3, 3);
            this.orderMappings.Name = "orderMappings";
            this.orderMappings.Size = new System.Drawing.Size(463, 317);
            this.orderMappings.TabIndex = 1;
            // 
            // optionPageAddress
            // 
            this.optionPageAddress.BackColor = System.Drawing.Color.White;
            this.optionPageAddress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.optionPageAddress.Location = new System.Drawing.Point(153, 0);
            this.optionPageAddress.Name = "optionPageAddress";
            this.optionPageAddress.Padding = new System.Windows.Forms.Padding(3);
            this.optionPageAddress.Size = new System.Drawing.Size(473, 327);
            this.optionPageAddress.TabIndex = 2;
            this.optionPageAddress.Text = "Address";
            // 
            // optionPageItems
            // 
            this.optionPageItems.BackColor = System.Drawing.Color.White;
            this.optionPageItems.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.optionPageItems.Location = new System.Drawing.Point(153, 0);
            this.optionPageItems.Name = "optionPageItems";
            this.optionPageItems.Padding = new System.Windows.Forms.Padding(3);
            this.optionPageItems.Size = new System.Drawing.Size(473, 327);
            this.optionPageItems.TabIndex = 5;
            this.optionPageItems.Text = "Items";
            // 
            // GenericCsvMapEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.updateSourceColumns);
            this.Controls.Add(this.sourceColumnSummary);
            this.Controls.Add(this.labelSourceColumns);
            this.Controls.Add(this.editDateFormat);
            this.Controls.Add(this.editFileFormat);
            this.Controls.Add(this.dateFormatSummary);
            this.Controls.Add(this.fileFormatSummary);
            this.Controls.Add(this.labelFileFormat);
            this.Controls.Add(this.labelMapName);
            this.Controls.Add(this.labelDates);
            this.Controls.Add(this.labelMappings);
            this.Controls.Add(this.name);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.optionControl);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "GenericCsvMapEditorControl";
            this.Size = new System.Drawing.Size(641, 483);
            this.optionControl.ResumeLayout(false);
            this.optionPageOrder.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelMappings;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label labelName;
        private UI.Controls.OptionControl optionControl;
        private UI.Controls.OptionPage optionPageOrder;
        private UI.Controls.OptionPage optionPageAddress;
        private UI.Controls.OptionPage optionPageItems;
        private ShipWorks.Data.Import.Spreadsheet.Editing.GenericSpreadsheetFieldMappingGroupControl orderMappings;
        private System.Windows.Forms.Label labelDates;
        private System.Windows.Forms.Label labelMapName;
        private System.Windows.Forms.Label labelFileFormat;
        private System.Windows.Forms.TextBox fileFormatSummary;
        private System.Windows.Forms.TextBox dateFormatSummary;
        private System.Windows.Forms.Button editFileFormat;
        private System.Windows.Forms.Button editDateFormat;
        private System.Windows.Forms.Button updateSourceColumns;
        private System.Windows.Forms.TextBox sourceColumnSummary;
        private System.Windows.Forms.Label labelSourceColumns;
    }
}
