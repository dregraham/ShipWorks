namespace ShipWorks.Data.Import.Spreadsheet.Editing
{
    partial class GenericSpreadsheetFieldMappingGroupControl
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
            this.labelSource = new System.Windows.Forms.Label();
            this.labelField = new System.Windows.Forms.Label();
            this.panelSettings = new System.Windows.Forms.Panel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelSource
            // 
            this.labelSource.AutoSize = true;
            this.labelSource.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelSource.Location = new System.Drawing.Point(117, 5);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(91, 13);
            this.labelSource.TabIndex = 3;
            this.labelSource.Text = "Source Column";
            // 
            // labelField
            // 
            this.labelField.AutoSize = true;
            this.labelField.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelField.Location = new System.Drawing.Point(1, 5);
            this.labelField.Name = "labelField";
            this.labelField.Size = new System.Drawing.Size(96, 13);
            this.labelField.TabIndex = 2;
            this.labelField.Text = "ShipWorks Field";
            // 
            // panelSettings
            // 
            this.panelSettings.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSettings.Location = new System.Drawing.Point(0, 0);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Size = new System.Drawing.Size(464, 73);
            this.panelSettings.TabIndex = 5;
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.labelSource);
            this.panelHeader.Controls.Add(this.labelField);
            this.panelHeader.Location = new System.Drawing.Point(0, 80);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(464, 22);
            this.panelHeader.TabIndex = 6;
            // 
            // GenericCsvFieldMappingGroupControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelSettings);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "GenericCsvFieldMappingGroupControl";
            this.Size = new System.Drawing.Size(464, 266);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.Label labelField;
        private System.Windows.Forms.Panel panelSettings;
        private System.Windows.Forms.Panel panelHeader;
    }
}
