namespace ShipWorks.Stores.Platforms.GenericFile.Sources
{
    partial class GenericFileSourceMasterControl
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
            this.fileSource = new System.Windows.Forms.ComboBox();
            this.labelImport = new System.Windows.Forms.Label();
            this.panelHolder = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // fileSource
            // 
            this.fileSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fileSource.FormattingEnabled = true;
            this.fileSource.Location = new System.Drawing.Point(34, 36);
            this.fileSource.Name = "fileSource";
            this.fileSource.Size = new System.Drawing.Size(294, 21);
            this.fileSource.TabIndex = 83;
            this.fileSource.SelectedIndexChanged += new System.EventHandler(this.OnChangeFileSource);
            // 
            // labelImport
            // 
            this.labelImport.AutoSize = true;
            this.labelImport.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelImport.Location = new System.Drawing.Point(14, 13);
            this.labelImport.Name = "labelImport";
            this.labelImport.Size = new System.Drawing.Size(131, 13);
            this.labelImport.TabIndex = 82;
            this.labelImport.Text = "Import the data from:";
            // 
            // panelHolder
            // 
            this.panelHolder.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHolder.Location = new System.Drawing.Point(17, 63);
            this.panelHolder.Name = "panelHolder";
            this.panelHolder.Size = new System.Drawing.Size(589, 348);
            this.panelHolder.TabIndex = 84;
            // 
            // GenericFileSourceMasterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelHolder);
            this.Controls.Add(this.fileSource);
            this.Controls.Add(this.labelImport);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "GenericFileSourceMasterControl";
            this.Size = new System.Drawing.Size(606, 411);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox fileSource;
        private System.Windows.Forms.Label labelImport;
        private System.Windows.Forms.Panel panelHolder;
    }
}
