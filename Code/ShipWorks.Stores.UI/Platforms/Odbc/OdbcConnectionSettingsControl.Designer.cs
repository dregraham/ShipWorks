namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    partial class OdbcConnectionSettingsControl
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
            this.importSettingsLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.openImportSettingsWizard = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // importSettingsLabel
            // 
            this.importSettingsLabel.AutoSize = true;
            this.importSettingsLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.importSettingsLabel.Location = new System.Drawing.Point(25, 25);
            this.importSettingsLabel.Name = "importSettingsLabel";
            this.importSettingsLabel.Size = new System.Drawing.Size(97, 13);
            this.importSettingsLabel.TabIndex = 0;
            this.importSettingsLabel.Text = "Import Settings";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(378, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Edit the data source used for importing, import settings and import mappings.";
            // 
            // openImportSettingsWizard
            // 
            this.openImportSettingsWizard.Location = new System.Drawing.Point(422, 40);
            this.openImportSettingsWizard.Name = "openImportSettingsWizard";
            this.openImportSettingsWizard.Size = new System.Drawing.Size(125, 23);
            this.openImportSettingsWizard.TabIndex = 2;
            this.openImportSettingsWizard.Text = "Edit Import Settings";
            this.openImportSettingsWizard.UseVisualStyleBackColor = true;
            this.openImportSettingsWizard.Click += new System.EventHandler(this.OnClickEditImportSettings);
            // 
            // OdbcConnectionSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.openImportSettingsWizard);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.importSettingsLabel);
            this.Name = "OdbcConnectionSettingsControl";
            this.Size = new System.Drawing.Size(550, 345);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label importSettingsLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button openImportSettingsWizard;
    }
}