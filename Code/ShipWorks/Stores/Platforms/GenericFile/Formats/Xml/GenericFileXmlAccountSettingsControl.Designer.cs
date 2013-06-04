namespace ShipWorks.Stores.Platforms.GenericFile.Formats.Xml
{
    partial class GenericFileXmlAccountSettingsControl
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
            this.fileSourceControl = new ShipWorks.Stores.Platforms.GenericFile.Sources.GenericFileSourceMasterControl();
            this.sectionXmlConfiguration = new ShipWorks.UI.Controls.SectionTitle();
            this.sectionTitleImport = new ShipWorks.UI.Controls.SectionTitle();
            this.xmlSetupControl = new ShipWorks.Stores.Platforms.GenericFile.Formats.Xml.GenericFileXmlSetupControl();
            this.SuspendLayout();
            // 
            // fileSourceControl
            // 
            this.fileSourceControl.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fileSourceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.fileSourceControl.Location = new System.Drawing.Point(14, 276);
            this.fileSourceControl.Name = "fileSourceControl";
            this.fileSourceControl.Size = new System.Drawing.Size(585, 170);
            this.fileSourceControl.TabIndex = 0;
            // 
            // sectionXmlConfiguration
            // 
            this.sectionXmlConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionXmlConfiguration.Location = new System.Drawing.Point(13, 10);
            this.sectionXmlConfiguration.Name = "sectionXmlConfiguration";
            this.sectionXmlConfiguration.Size = new System.Drawing.Size(586, 22);
            this.sectionXmlConfiguration.TabIndex = 4;
            this.sectionXmlConfiguration.Text = "XML Configuration";
            // 
            // sectionTitleImport
            // 
            this.sectionTitleImport.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitleImport.Location = new System.Drawing.Point(13, 280);
            this.sectionTitleImport.Name = "sectionTitleImport";
            this.sectionTitleImport.Size = new System.Drawing.Size(586, 22);
            this.sectionTitleImport.TabIndex = 5;
            this.sectionTitleImport.Text = "Import From";
            // 
            // xmlSetupControl
            // 
            this.xmlSetupControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.xmlSetupControl.IsVerified = false;
            this.xmlSetupControl.Location = new System.Drawing.Point(8, 6);
            this.xmlSetupControl.Name = "xmlSetupControl";
            this.xmlSetupControl.Size = new System.Drawing.Size(509, 269);
            this.xmlSetupControl.TabIndex = 6;
            // 
            // GenericFileXmlAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.sectionTitleImport);
            this.Controls.Add(this.sectionXmlConfiguration);
            this.Controls.Add(this.fileSourceControl);
            this.Controls.Add(this.xmlSetupControl);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "GenericFileXmlAccountSettingsControl";
            this.Size = new System.Drawing.Size(609, 449);
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.Stores.Platforms.GenericFile.Sources.GenericFileSourceMasterControl fileSourceControl;
        private UI.Controls.SectionTitle sectionXmlConfiguration;
        private UI.Controls.SectionTitle sectionTitleImport;
        private GenericFileXmlSetupControl xmlSetupControl;
    }
}
