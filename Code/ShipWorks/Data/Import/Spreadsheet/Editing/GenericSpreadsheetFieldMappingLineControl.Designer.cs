namespace ShipWorks.Data.Import.Spreadsheet.Editing
{
    partial class GenericSpreadsheetFieldMappingLineControl
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
            this.labelName = new System.Windows.Forms.Label();
            this.borderEdge1 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.comboSourceColumn = new ShipWorks.Data.Import.Spreadsheet.Editing.GenericSpreadsheetSourceColumnComboBox();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(3, 7);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(59, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Field Name";
            // 
            // borderEdge1
            // 
            this.borderEdge1.AutoSize = false;
            this.borderEdge1.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.ControlRibbon;
            this.borderEdge1.Dock = System.Windows.Forms.DockStyle.Top;
            this.borderEdge1.Location = new System.Drawing.Point(0, 0);
            this.borderEdge1.Name = "borderEdge1";
            this.borderEdge1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.borderEdge1.Size = new System.Drawing.Size(385, 1);
            this.borderEdge1.TabIndex = 25;
            this.borderEdge1.Text = "kryptonBorderEdge1";
            // 
            // comboSourceColumn
            // 
            this.comboSourceColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSourceColumn.FormattingEnabled = true;
            this.comboSourceColumn.Location = new System.Drawing.Point(121, 4);
            this.comboSourceColumn.Name = "comboSourceColumn";
            this.comboSourceColumn.Size = new System.Drawing.Size(194, 21);
            this.comboSourceColumn.TabIndex = 1;
            // 
            // GenericCsvFieldMappingLineControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.borderEdge1);
            this.Controls.Add(this.comboSourceColumn);
            this.Controls.Add(this.labelName);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "GenericCsvFieldMappingLineControl";
            this.Size = new System.Drawing.Size(385, 28);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private ShipWorks.Data.Import.Spreadsheet.Editing.GenericSpreadsheetSourceColumnComboBox comboSourceColumn;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge borderEdge1;
    }
}
