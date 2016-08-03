namespace ShipWorks.Stores.UI.Platforms.Odbc.Controls
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
            this.uploadSettingsLabel = new System.Windows.Forms.Label();
            this.uploadStrategy = new System.Windows.Forms.ComboBox();
            this.labelShipmentUpdate = new System.Windows.Forms.Label();
            this.editUploadSettings = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
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
            this.label1.Location = new System.Drawing.Point(63, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Edit settings to import order details:";
            // 
            // openImportSettingsWizard
            // 
            this.openImportSettingsWizard.Location = new System.Drawing.Point(248, 40);
            this.openImportSettingsWizard.Name = "openImportSettingsWizard";
            this.openImportSettingsWizard.Size = new System.Drawing.Size(125, 23);
            this.openImportSettingsWizard.TabIndex = 2;
            this.openImportSettingsWizard.Text = "Edit Import Settings";
            this.openImportSettingsWizard.UseVisualStyleBackColor = true;
            this.openImportSettingsWizard.Click += new System.EventHandler(this.OnEditImportSettingsClick);
            // 
            // uploadSettingsLabel
            // 
            this.uploadSettingsLabel.AutoSize = true;
            this.uploadSettingsLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uploadSettingsLabel.Location = new System.Drawing.Point(25, 69);
            this.uploadSettingsLabel.Name = "uploadSettingsLabel";
            this.uploadSettingsLabel.Size = new System.Drawing.Size(96, 13);
            this.uploadSettingsLabel.TabIndex = 3;
            this.uploadSettingsLabel.Text = "Upload Settings";
            // 
            // uploadStrategy
            // 
            this.uploadStrategy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uploadStrategy.FormattingEnabled = true;
            this.uploadStrategy.Location = new System.Drawing.Point(249, 88);
            this.uploadStrategy.Name = "uploadStrategy";
            this.uploadStrategy.Size = new System.Drawing.Size(266, 21);
            this.uploadStrategy.TabIndex = 5;
            this.uploadStrategy.SelectedIndexChanged += new System.EventHandler(this.OnUploadStrategySelectedIndexChanged);
            // 
            // labelShipmentUpdate
            // 
            this.labelShipmentUpdate.AutoSize = true;
            this.labelShipmentUpdate.Location = new System.Drawing.Point(71, 91);
            this.labelShipmentUpdate.Name = "labelShipmentUpdate";
            this.labelShipmentUpdate.Size = new System.Drawing.Size(171, 13);
            this.labelShipmentUpdate.TabIndex = 28;
            this.labelShipmentUpdate.Text = "Where to upload shipment details:";
            // 
            // editUploadSettings
            // 
            this.editUploadSettings.Location = new System.Drawing.Point(248, 115);
            this.editUploadSettings.Name = "editUploadSettings";
            this.editUploadSettings.Size = new System.Drawing.Size(125, 23);
            this.editUploadSettings.TabIndex = 30;
            this.editUploadSettings.Text = "Edit Upload Settings";
            this.editUploadSettings.UseVisualStyleBackColor = true;
            this.editUploadSettings.Click += new System.EventHandler(this.OnEditUploadSettingsClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(198, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Edit settings to upload shipment details:";
            // 
            // OdbcConnectionSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.editUploadSettings);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelShipmentUpdate);
            this.Controls.Add(this.uploadStrategy);
            this.Controls.Add(this.uploadSettingsLabel);
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
        private System.Windows.Forms.Label uploadSettingsLabel;
        private System.Windows.Forms.ComboBox uploadStrategy;
        private System.Windows.Forms.Label labelShipmentUpdate;
        private System.Windows.Forms.Button editUploadSettings;
        private System.Windows.Forms.Label label2;
    }
}