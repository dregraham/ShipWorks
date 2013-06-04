namespace ShipWorks.Stores.Platforms.GenericFile.Formats.Xml
{
    partial class GenericFileXmlSetupControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenericFileXmlSetupControl));
            this.labelConfiguration = new System.Windows.Forms.Label();
            this.labelConfigurationInfo = new System.Windows.Forms.Label();
            this.labelStylesheetInfo = new System.Windows.Forms.Label();
            this.labelStylesheet = new System.Windows.Forms.Label();
            this.labelStylesheetBrowse = new System.Windows.Forms.Label();
            this.xsltBrowse = new System.Windows.Forms.Button();
            this.xsltClear = new System.Windows.Forms.Button();
            this.labelVerifyInfo = new System.Windows.Forms.Label();
            this.verify = new System.Windows.Forms.Button();
            this.panelVerifySuccess = new System.Windows.Forms.Panel();
            this.labelSetup = new System.Windows.Forms.Label();
            this.pictureBoxSetup = new System.Windows.Forms.PictureBox();
            this.xsltPath = new ShipWorks.UI.Controls.PathTextBox();
            this.linkSchema = new ShipWorks.UI.Controls.LinkControl();
            this.label1 = new System.Windows.Forms.Label();
            this.panelVerifySuccess.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxSetup)).BeginInit();
            this.SuspendLayout();
            // 
            // labelConfiguration
            // 
            this.labelConfiguration.AutoSize = true;
            this.labelConfiguration.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelConfiguration.Location = new System.Drawing.Point(14, 13);
            this.labelConfiguration.Name = "labelConfiguration";
            this.labelConfiguration.Size = new System.Drawing.Size(109, 13);
            this.labelConfiguration.TabIndex = 0;
            this.labelConfiguration.Text = "XML Configuration";
            // 
            // labelConfigurationInfo
            // 
            this.labelConfigurationInfo.Location = new System.Drawing.Point(26, 33);
            this.labelConfigurationInfo.Name = "labelConfigurationInfo";
            this.labelConfigurationInfo.Size = new System.Drawing.Size(463, 31);
            this.labelConfigurationInfo.TabIndex = 1;
            this.labelConfigurationInfo.Text = "XML files must conform to the ShipWorks XML Schema to be imported successfully.  " +
                "The schema and documentation can be";
            // 
            // labelStylesheetInfo
            // 
            this.labelStylesheetInfo.Location = new System.Drawing.Point(26, 176);
            this.labelStylesheetInfo.Name = "labelStylesheetInfo";
            this.labelStylesheetInfo.Size = new System.Drawing.Size(463, 28);
            this.labelStylesheetInfo.TabIndex = 2;
            this.labelStylesheetInfo.Text = "If you have an existing XML format you can use it as-is by providing ShipWorks wi" +
                "th an XSLT Stylesheet to transform each incoming XML document.";
            // 
            // labelStylesheet
            // 
            this.labelStylesheet.AutoSize = true;
            this.labelStylesheet.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelStylesheet.Location = new System.Drawing.Point(14, 155);
            this.labelStylesheet.Name = "labelStylesheet";
            this.labelStylesheet.Size = new System.Drawing.Size(98, 13);
            this.labelStylesheet.TabIndex = 4;
            this.labelStylesheet.Text = "XSLT Stylesheet";
            // 
            // labelStylesheetBrowse
            // 
            this.labelStylesheetBrowse.AutoSize = true;
            this.labelStylesheetBrowse.Location = new System.Drawing.Point(26, 213);
            this.labelStylesheetBrowse.Name = "labelStylesheetBrowse";
            this.labelStylesheetBrowse.Size = new System.Drawing.Size(88, 13);
            this.labelStylesheetBrowse.TabIndex = 5;
            this.labelStylesheetBrowse.Text = "XSLT Stylesheet:";
            // 
            // xsltBrowse
            // 
            this.xsltBrowse.Location = new System.Drawing.Point(414, 236);
            this.xsltBrowse.Name = "xsltBrowse";
            this.xsltBrowse.Size = new System.Drawing.Size(75, 23);
            this.xsltBrowse.TabIndex = 4;
            this.xsltBrowse.Text = "Browse...";
            this.xsltBrowse.UseVisualStyleBackColor = true;
            this.xsltBrowse.Click += new System.EventHandler(this.OnXsltBrowse);
            // 
            // xsltClear
            // 
            this.xsltClear.Location = new System.Drawing.Point(333, 236);
            this.xsltClear.Name = "xsltClear";
            this.xsltClear.Size = new System.Drawing.Size(75, 23);
            this.xsltClear.TabIndex = 3;
            this.xsltClear.Text = "Clear";
            this.xsltClear.UseVisualStyleBackColor = true;
            this.xsltClear.Click += new System.EventHandler(this.OnClearXslt);
            // 
            // labelVerifyInfo
            // 
            this.labelVerifyInfo.Location = new System.Drawing.Point(26, 68);
            this.labelVerifyInfo.Name = "labelVerifyInfo";
            this.labelVerifyInfo.Size = new System.Drawing.Size(477, 45);
            this.labelVerifyInfo.TabIndex = 10;
            this.labelVerifyInfo.Text = resources.GetString("labelVerifyInfo.Text");
            // 
            // verify
            // 
            this.verify.Location = new System.Drawing.Point(27, 115);
            this.verify.Name = "verify";
            this.verify.Size = new System.Drawing.Size(75, 23);
            this.verify.TabIndex = 1;
            this.verify.Text = "Verify...";
            this.verify.UseVisualStyleBackColor = true;
            this.verify.Click += new System.EventHandler(this.OnVerify);
            // 
            // panelVerifySuccess
            // 
            this.panelVerifySuccess.Controls.Add(this.labelSetup);
            this.panelVerifySuccess.Controls.Add(this.pictureBoxSetup);
            this.panelVerifySuccess.Location = new System.Drawing.Point(108, 115);
            this.panelVerifySuccess.Name = "panelVerifySuccess";
            this.panelVerifySuccess.Size = new System.Drawing.Size(72, 23);
            this.panelVerifySuccess.TabIndex = 12;
            this.panelVerifySuccess.Visible = false;
            // 
            // labelSetup
            // 
            this.labelSetup.AutoSize = true;
            this.labelSetup.Location = new System.Drawing.Point(19, 5);
            this.labelSetup.Name = "labelSetup";
            this.labelSetup.Size = new System.Drawing.Size(47, 13);
            this.labelSetup.TabIndex = 14;
            this.labelSetup.Text = "Verified!";
            // 
            // pictureBoxSetup
            // 
            this.pictureBoxSetup.Image = global::ShipWorks.Properties.Resources.check16;
            this.pictureBoxSetup.Location = new System.Drawing.Point(1, 4);
            this.pictureBoxSetup.Name = "pictureBoxSetup";
            this.pictureBoxSetup.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxSetup.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxSetup.TabIndex = 13;
            this.pictureBoxSetup.TabStop = false;
            // 
            // xsltPath
            // 
            this.xsltPath.Location = new System.Drawing.Point(118, 210);
            this.xsltPath.Name = "xsltPath";
            this.xsltPath.ReadOnly = true;
            this.xsltPath.Size = new System.Drawing.Size(371, 21);
            this.xsltPath.TabIndex = 2;
            // 
            // linkSchema
            // 
            this.linkSchema.AutoSize = true;
            this.linkSchema.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkSchema.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkSchema.ForeColor = System.Drawing.Color.Blue;
            this.linkSchema.Location = new System.Drawing.Point(157, 46);
            this.linkSchema.Name = "linkSchema";
            this.linkSchema.Size = new System.Drawing.Size(64, 13);
            this.linkSchema.TabIndex = 0;
            this.linkSchema.Text = "found here.";
            this.linkSchema.Click += new System.EventHandler(this.OnLinkSchemaDocumentation);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label1.Location = new System.Drawing.Point(114, 155);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "(Optional)";
            // 
            // GenericFileXmlSetupControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panelVerifySuccess);
            this.Controls.Add(this.verify);
            this.Controls.Add(this.labelVerifyInfo);
            this.Controls.Add(this.xsltClear);
            this.Controls.Add(this.xsltBrowse);
            this.Controls.Add(this.xsltPath);
            this.Controls.Add(this.labelStylesheetBrowse);
            this.Controls.Add(this.labelStylesheet);
            this.Controls.Add(this.linkSchema);
            this.Controls.Add(this.labelStylesheetInfo);
            this.Controls.Add(this.labelConfigurationInfo);
            this.Controls.Add(this.labelConfiguration);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "GenericFileXmlSetupControl";
            this.Size = new System.Drawing.Size(523, 271);
            this.panelVerifySuccess.ResumeLayout(false);
            this.panelVerifySuccess.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxSetup)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelConfiguration;
        private System.Windows.Forms.Label labelConfigurationInfo;
        private System.Windows.Forms.Label labelStylesheetInfo;
        private UI.Controls.LinkControl linkSchema;
        private System.Windows.Forms.Label labelStylesheet;
        private System.Windows.Forms.Label labelStylesheetBrowse;
        private UI.Controls.PathTextBox xsltPath;
        private System.Windows.Forms.Button xsltBrowse;
        private System.Windows.Forms.Button xsltClear;
        private System.Windows.Forms.Label labelVerifyInfo;
        private System.Windows.Forms.Button verify;
        private System.Windows.Forms.Panel panelVerifySuccess;
        private System.Windows.Forms.Label labelSetup;
        private System.Windows.Forms.PictureBox pictureBoxSetup;
        private System.Windows.Forms.Label label1;
    }
}
