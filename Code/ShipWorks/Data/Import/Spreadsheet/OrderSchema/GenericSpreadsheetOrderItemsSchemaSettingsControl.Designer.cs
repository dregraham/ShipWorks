namespace ShipWorks.Data.Import.Spreadsheet.OrderSchema
{
    partial class GenericSpreadsheetOrderItemsSchemaSettingsControl
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
            this.labelSingleLine2 = new System.Windows.Forms.Label();
            this.comboSingleLineCount = new System.Windows.Forms.ComboBox();
            this.labelSingleLine1 = new System.Windows.Forms.Label();
            this.multiItemStrategy = new System.Windows.Forms.ComboBox();
            this.labelMultipleItems = new System.Windows.Forms.Label();
            this.panelSingleLine = new System.Windows.Forms.Panel();
            this.panelMultiLine = new System.Windows.Forms.Panel();
            this.infotipMinimizeRibbon = new ShipWorks.UI.Controls.InfoTip();
            this.comboUniqueColumn = new ShipWorks.Data.Import.Spreadsheet.Editing.GenericSpreadsheetSourceColumnComboBox();
            this.labelUniqueColumn = new System.Windows.Forms.Label();
            this.panelAttributes = new System.Windows.Forms.Panel();
            this.labelAttributes2 = new System.Windows.Forms.Label();
            this.comboAttributeCount = new System.Windows.Forms.ComboBox();
            this.labelAttributes1 = new System.Windows.Forms.Label();
            this.panelSingleLine.SuspendLayout();
            this.panelMultiLine.SuspendLayout();
            this.panelAttributes.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelSingleLine2
            // 
            this.labelSingleLine2.AutoSize = true;
            this.labelSingleLine2.Location = new System.Drawing.Point(235, 6);
            this.labelSingleLine2.Name = "labelSingleLine2";
            this.labelSingleLine2.Size = new System.Drawing.Size(80, 13);
            this.labelSingleLine2.TabIndex = 11;
            this.labelSingleLine2.Text = "items per order";
            // 
            // comboSingleLineCount
            // 
            this.comboSingleLineCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSingleLineCount.FormattingEnabled = true;
            this.comboSingleLineCount.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25"});
            this.comboSingleLineCount.Location = new System.Drawing.Point(178, 2);
            this.comboSingleLineCount.Name = "comboSingleLineCount";
            this.comboSingleLineCount.Size = new System.Drawing.Size(53, 21);
            this.comboSingleLineCount.TabIndex = 10;
            // 
            // labelSingleLine1
            // 
            this.labelSingleLine1.AutoSize = true;
            this.labelSingleLine1.Location = new System.Drawing.Point(3, 5);
            this.labelSingleLine1.Name = "labelSingleLine1";
            this.labelSingleLine1.Size = new System.Drawing.Size(175, 13);
            this.labelSingleLine1.TabIndex = 9;
            this.labelSingleLine1.Text = "There are source columns for up to";
            // 
            // multiItemStrategy
            // 
            this.multiItemStrategy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.multiItemStrategy.FormattingEnabled = true;
            this.multiItemStrategy.Items.AddRange(new object[] {
            "All items are on the same line"});
            this.multiItemStrategy.Location = new System.Drawing.Point(18, 23);
            this.multiItemStrategy.Name = "multiItemStrategy";
            this.multiItemStrategy.Size = new System.Drawing.Size(232, 21);
            this.multiItemStrategy.TabIndex = 8;
            // 
            // labelMultipleItems
            // 
            this.labelMultipleItems.AutoSize = true;
            this.labelMultipleItems.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMultipleItems.Location = new System.Drawing.Point(1, 5);
            this.labelMultipleItems.Name = "labelMultipleItems";
            this.labelMultipleItems.Size = new System.Drawing.Size(89, 13);
            this.labelMultipleItems.TabIndex = 7;
            this.labelMultipleItems.Text = "Multiple Items";
            // 
            // panelSingleLine
            // 
            this.panelSingleLine.Controls.Add(this.labelSingleLine1);
            this.panelSingleLine.Controls.Add(this.labelSingleLine2);
            this.panelSingleLine.Controls.Add(this.comboSingleLineCount);
            this.panelSingleLine.Location = new System.Drawing.Point(15, 50);
            this.panelSingleLine.Name = "panelSingleLine";
            this.panelSingleLine.Size = new System.Drawing.Size(469, 29);
            this.panelSingleLine.TabIndex = 12;
            // 
            // panelMultiLine
            // 
            this.panelMultiLine.Controls.Add(this.infotipMinimizeRibbon);
            this.panelMultiLine.Controls.Add(this.comboUniqueColumn);
            this.panelMultiLine.Controls.Add(this.labelUniqueColumn);
            this.panelMultiLine.Location = new System.Drawing.Point(15, 85);
            this.panelMultiLine.Name = "panelMultiLine";
            this.panelMultiLine.Size = new System.Drawing.Size(469, 52);
            this.panelMultiLine.TabIndex = 13;
            this.panelMultiLine.Visible = false;
            // 
            // infotipMinimizeRibbon
            // 
            this.infotipMinimizeRibbon.Caption = "Each repeating line that represents an item of the same order must have the same " +
    "value in this column.\r\n\r\nThis is the \'primary key\' of the order, and mostly comm" +
    "only the Order Number.";
            this.infotipMinimizeRibbon.Location = new System.Drawing.Point(241, 25);
            this.infotipMinimizeRibbon.Name = "infotipMinimizeRibbon";
            this.infotipMinimizeRibbon.Size = new System.Drawing.Size(12, 12);
            this.infotipMinimizeRibbon.TabIndex = 22;
            this.infotipMinimizeRibbon.Title = "Unique Column";
            // 
            // comboUniqueColumn
            // 
            this.comboUniqueColumn.FormattingEnabled = true;
            this.comboUniqueColumn.Location = new System.Drawing.Point(23, 21);
            this.comboUniqueColumn.Name = "comboUniqueColumn";
            this.comboUniqueColumn.Size = new System.Drawing.Size(212, 21);
            this.comboUniqueColumn.TabIndex = 1;
            // 
            // labelUniqueColumn
            // 
            this.labelUniqueColumn.AutoSize = true;
            this.labelUniqueColumn.Location = new System.Drawing.Point(4, 5);
            this.labelUniqueColumn.Name = "labelUniqueColumn";
            this.labelUniqueColumn.Size = new System.Drawing.Size(213, 13);
            this.labelUniqueColumn.TabIndex = 0;
            this.labelUniqueColumn.Text = "Column that uniquely identifies each order:";
            // 
            // panelAttributes
            // 
            this.panelAttributes.Controls.Add(this.labelAttributes2);
            this.panelAttributes.Controls.Add(this.comboAttributeCount);
            this.panelAttributes.Controls.Add(this.labelAttributes1);
            this.panelAttributes.Location = new System.Drawing.Point(15, 143);
            this.panelAttributes.Name = "panelAttributes";
            this.panelAttributes.Size = new System.Drawing.Size(469, 34);
            this.panelAttributes.TabIndex = 14;
            // 
            // labelAttributes2
            // 
            this.labelAttributes2.AutoSize = true;
            this.labelAttributes2.Location = new System.Drawing.Point(235, 5);
            this.labelAttributes2.Name = "labelAttributes2";
            this.labelAttributes2.Size = new System.Drawing.Size(96, 13);
            this.labelAttributes2.TabIndex = 12;
            this.labelAttributes2.Text = "attributes per item";
            // 
            // comboAttributeCount
            // 
            this.comboAttributeCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAttributeCount.FormattingEnabled = true;
            this.comboAttributeCount.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25"});
            this.comboAttributeCount.Location = new System.Drawing.Point(178, 2);
            this.comboAttributeCount.Name = "comboAttributeCount";
            this.comboAttributeCount.Size = new System.Drawing.Size(53, 21);
            this.comboAttributeCount.TabIndex = 11;
            // 
            // labelAttributes1
            // 
            this.labelAttributes1.AutoSize = true;
            this.labelAttributes1.Location = new System.Drawing.Point(3, 5);
            this.labelAttributes1.Name = "labelAttributes1";
            this.labelAttributes1.Size = new System.Drawing.Size(175, 13);
            this.labelAttributes1.TabIndex = 10;
            this.labelAttributes1.Text = "There are source columns for up to";
            // 
            // GenericSpreadsheetOrderItemsSchemaSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelAttributes);
            this.Controls.Add(this.panelMultiLine);
            this.Controls.Add(this.panelSingleLine);
            this.Controls.Add(this.multiItemStrategy);
            this.Controls.Add(this.labelMultipleItems);
            this.Name = "GenericSpreadsheetOrderItemsSchemaSettingsControl";
            this.Size = new System.Drawing.Size(539, 210);
            this.panelSingleLine.ResumeLayout(false);
            this.panelSingleLine.PerformLayout();
            this.panelMultiLine.ResumeLayout(false);
            this.panelMultiLine.PerformLayout();
            this.panelAttributes.ResumeLayout(false);
            this.panelAttributes.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSingleLine2;
        private System.Windows.Forms.ComboBox comboSingleLineCount;
        private System.Windows.Forms.Label labelSingleLine1;
        private System.Windows.Forms.ComboBox multiItemStrategy;
        private System.Windows.Forms.Label labelMultipleItems;
        private System.Windows.Forms.Panel panelSingleLine;
        private System.Windows.Forms.Panel panelMultiLine;
        private System.Windows.Forms.Label labelUniqueColumn;
        private Data.Import.Spreadsheet.Editing.GenericSpreadsheetSourceColumnComboBox comboUniqueColumn;
        private UI.Controls.InfoTip infotipMinimizeRibbon;
        private System.Windows.Forms.Panel panelAttributes;
        private System.Windows.Forms.Label labelAttributes2;
        private System.Windows.Forms.ComboBox comboAttributeCount;
        private System.Windows.Forms.Label labelAttributes1;
    }
}
