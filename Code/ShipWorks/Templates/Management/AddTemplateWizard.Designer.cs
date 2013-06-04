namespace ShipWorks.Templates.Management
{
    partial class AddTemplateWizard
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddTemplateWizard));
            this.wizardPageLocation = new ShipWorks.UI.Wizard.WizardPage();
            this.treeControl = new ShipWorks.Templates.Controls.TemplateTreeControl();
            this.labelLocation = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.wizardPageType = new ShipWorks.UI.Wizard.WizardPage();
            this.infoTemplateTypeReport = new System.Windows.Forms.Label();
            this.imageTemplateTypeReport = new System.Windows.Forms.PictureBox();
            this.infoTemplateTypeLabels = new System.Windows.Forms.Label();
            this.imageTemplateTypeLabels = new System.Windows.Forms.PictureBox();
            this.infoTemplateTypeStandard = new System.Windows.Forms.Label();
            this.imageTemplateTypeStandard = new System.Windows.Forms.PictureBox();
            this.templateTypeReport = new System.Windows.Forms.RadioButton();
            this.templateTypeLabels = new System.Windows.Forms.RadioButton();
            this.templateTypeStandard = new System.Windows.Forms.RadioButton();
            this.labelTemplateTypeDescription = new System.Windows.Forms.Label();
            this.wizardPageFormat = new ShipWorks.UI.Wizard.WizardPage();
            this.infoTemplateFormatXml = new System.Windows.Forms.Label();
            this.imageTemplateFormatXml = new System.Windows.Forms.PictureBox();
            this.infoTemplateFormatPlainText = new System.Windows.Forms.Label();
            this.imageTemplateFormatPlainText = new System.Windows.Forms.PictureBox();
            this.infoTemplateFormatHtml = new System.Windows.Forms.Label();
            this.imageTemplateFormatHtml = new System.Windows.Forms.PictureBox();
            this.templateFormatXml = new System.Windows.Forms.RadioButton();
            this.templateFormatPlainText = new System.Windows.Forms.RadioButton();
            this.templateFormatHtml = new System.Windows.Forms.RadioButton();
            this.labelTemplateFormat = new System.Windows.Forms.Label();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageLocation.SuspendLayout();
            this.wizardPageType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.imageTemplateTypeReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.imageTemplateTypeLabels)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.imageTemplateTypeStandard)).BeginInit();
            this.wizardPageFormat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.imageTemplateFormatXml)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.imageTemplateFormatPlainText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.imageTemplateFormatHtml)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageLocation);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.text_code_add;
            // 
            // wizardPageLocation
            // 
            this.wizardPageLocation.Controls.Add(this.treeControl);
            this.wizardPageLocation.Controls.Add(this.labelLocation);
            this.wizardPageLocation.Controls.Add(this.name);
            this.wizardPageLocation.Controls.Add(this.labelName);
            this.wizardPageLocation.Description = "Choose a name and location for the new template.";
            this.wizardPageLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageLocation.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageLocation.Location = new System.Drawing.Point(0, 0);
            this.wizardPageLocation.Name = "wizardPageLocation";
            this.wizardPageLocation.Size = new System.Drawing.Size(526, 278);
            this.wizardPageLocation.TabIndex = 0;
            this.wizardPageLocation.Title = "Name and Location";
            this.wizardPageLocation.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextNameAndLocation);
            // 
            // templateTree
            // 
            this.treeControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeControl.FoldersOnly = true;
            this.treeControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.treeControl.HotTrackNode = null;
            this.treeControl.Location = new System.Drawing.Point(23, 70);
            this.treeControl.Name = "templateTree";
            this.treeControl.SnippetDisplay = ShipWorks.Templates.Controls.TemplateTreeSnippetDisplayType.Hidden;
            this.treeControl.Size = new System.Drawing.Size(473, 194);
            this.treeControl.TabIndex = 29;
            // 
            // labelLocation
            // 
            this.labelLocation.AutoSize = true;
            this.labelLocation.Location = new System.Drawing.Point(20, 54);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(220, 13);
            this.labelLocation.TabIndex = 31;
            this.labelLocation.Text = "Select the folder to put the new template in:";
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(23, 22);
            this.fieldLengthProvider.SetMaxLengthSource(this.name, ShipWorks.Data.Utility.EntityFieldLengthSource.TemplateName);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(199, 21);
            this.name.TabIndex = 28;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(20, 6);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(106, 13);
            this.labelName.TabIndex = 30;
            this.labelName.Text = "New template name:";
            // 
            // wizardPageType
            // 
            this.wizardPageType.Controls.Add(this.infoTemplateTypeReport);
            this.wizardPageType.Controls.Add(this.imageTemplateTypeReport);
            this.wizardPageType.Controls.Add(this.infoTemplateTypeLabels);
            this.wizardPageType.Controls.Add(this.imageTemplateTypeLabels);
            this.wizardPageType.Controls.Add(this.infoTemplateTypeStandard);
            this.wizardPageType.Controls.Add(this.imageTemplateTypeStandard);
            this.wizardPageType.Controls.Add(this.templateTypeReport);
            this.wizardPageType.Controls.Add(this.templateTypeLabels);
            this.wizardPageType.Controls.Add(this.templateTypeStandard);
            this.wizardPageType.Controls.Add(this.labelTemplateTypeDescription);
            this.wizardPageType.Description = "Select the type of template you want to create.";
            this.wizardPageType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageType.Location = new System.Drawing.Point(0, 0);
            this.wizardPageType.Name = "wizardPageType";
            this.wizardPageType.Size = new System.Drawing.Size(526, 278);
            this.wizardPageType.TabIndex = 0;
            this.wizardPageType.Title = "Template Type";
            this.wizardPageType.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextLastPage);
            // 
            // infoTemplateTypeReport
            // 
            this.infoTemplateTypeReport.ForeColor = System.Drawing.Color.DimGray;
            this.infoTemplateTypeReport.Location = new System.Drawing.Point(91, 208);
            this.infoTemplateTypeReport.Name = "infoTemplateTypeReport";
            this.infoTemplateTypeReport.Size = new System.Drawing.Size(406, 33);
            this.infoTemplateTypeReport.TabIndex = 20;
            this.infoTemplateTypeReport.Text = "Report templates are used to generate summaries and totals.  A report template pr" +
                "ocesses all selected items at once and produces a single result.\r\n ";
            // 
            // imageTemplateTypeReport
            // 
            this.imageTemplateTypeReport.Image = ((System.Drawing.Image) (resources.GetObject("imageTemplateTypeReport.Image")));
            this.imageTemplateTypeReport.Location = new System.Drawing.Point(53, 205);
            this.imageTemplateTypeReport.Name = "imageTemplateTypeReport";
            this.imageTemplateTypeReport.Size = new System.Drawing.Size(32, 32);
            this.imageTemplateTypeReport.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imageTemplateTypeReport.TabIndex = 19;
            this.imageTemplateTypeReport.TabStop = false;
            // 
            // infoTemplateTypeLabels
            // 
            this.infoTemplateTypeLabels.ForeColor = System.Drawing.Color.DimGray;
            this.infoTemplateTypeLabels.Location = new System.Drawing.Point(91, 138);
            this.infoTemplateTypeLabels.Name = "infoTemplateTypeLabels";
            this.infoTemplateTypeLabels.Size = new System.Drawing.Size(406, 36);
            this.infoTemplateTypeLabels.TabIndex = 18;
            this.infoTemplateTypeLabels.Text = "The output of a label template is used to fill in the labels of a label sheet. On" +
                "e label will be produced for each selected item in ShipWorks.";
            // 
            // imageTemplateTypeLabels
            // 
            this.imageTemplateTypeLabels.Image = ((System.Drawing.Image) (resources.GetObject("imageTemplateTypeLabels.Image")));
            this.imageTemplateTypeLabels.Location = new System.Drawing.Point(53, 135);
            this.imageTemplateTypeLabels.Name = "imageTemplateTypeLabels";
            this.imageTemplateTypeLabels.Size = new System.Drawing.Size(32, 32);
            this.imageTemplateTypeLabels.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imageTemplateTypeLabels.TabIndex = 17;
            this.imageTemplateTypeLabels.TabStop = false;
            // 
            // infoTemplateTypeStandard
            // 
            this.infoTemplateTypeStandard.ForeColor = System.Drawing.Color.DimGray;
            this.infoTemplateTypeStandard.Location = new System.Drawing.Point(91, 67);
            this.infoTemplateTypeStandard.Name = "infoTemplateTypeStandard";
            this.infoTemplateTypeStandard.Size = new System.Drawing.Size(423, 39);
            this.infoTemplateTypeStandard.TabIndex = 16;
            this.infoTemplateTypeStandard.Text = "Standard templates are usually used for things like invoices, packing slips, and " +
                "emails.  A standard template is processed once for each item you select.";
            // 
            // imageTemplateTypeStandard
            // 
            this.imageTemplateTypeStandard.Image = ((System.Drawing.Image) (resources.GetObject("imageTemplateTypeStandard.Image")));
            this.imageTemplateTypeStandard.Location = new System.Drawing.Point(53, 66);
            this.imageTemplateTypeStandard.Name = "imageTemplateTypeStandard";
            this.imageTemplateTypeStandard.Size = new System.Drawing.Size(32, 32);
            this.imageTemplateTypeStandard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imageTemplateTypeStandard.TabIndex = 15;
            this.imageTemplateTypeStandard.TabStop = false;
            // 
            // templateTypeReport
            // 
            this.templateTypeReport.AutoSize = true;
            this.templateTypeReport.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.templateTypeReport.Location = new System.Drawing.Point(37, 186);
            this.templateTypeReport.Name = "templateTypeReport";
            this.templateTypeReport.Size = new System.Drawing.Size(64, 17);
            this.templateTypeReport.TabIndex = 14;
            this.templateTypeReport.Text = "Report";
            this.templateTypeReport.UseVisualStyleBackColor = true;
            // 
            // templateTypeLabels
            // 
            this.templateTypeLabels.AutoSize = true;
            this.templateTypeLabels.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.templateTypeLabels.Location = new System.Drawing.Point(37, 116);
            this.templateTypeLabels.Name = "templateTypeLabels";
            this.templateTypeLabels.Size = new System.Drawing.Size(61, 17);
            this.templateTypeLabels.TabIndex = 13;
            this.templateTypeLabels.Text = "Labels";
            this.templateTypeLabels.UseVisualStyleBackColor = true;
            // 
            // templateTypeStandard
            // 
            this.templateTypeStandard.AutoSize = true;
            this.templateTypeStandard.Checked = true;
            this.templateTypeStandard.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.templateTypeStandard.Location = new System.Drawing.Point(37, 47);
            this.templateTypeStandard.Name = "templateTypeStandard";
            this.templateTypeStandard.Size = new System.Drawing.Size(77, 17);
            this.templateTypeStandard.TabIndex = 12;
            this.templateTypeStandard.TabStop = true;
            this.templateTypeStandard.Text = "Standard";
            this.templateTypeStandard.UseVisualStyleBackColor = true;
            // 
            // labelTemplateTypeDescription
            // 
            this.labelTemplateTypeDescription.Location = new System.Drawing.Point(22, 9);
            this.labelTemplateTypeDescription.Name = "labelTemplateTypeDescription";
            this.labelTemplateTypeDescription.Size = new System.Drawing.Size(508, 32);
            this.labelTemplateTypeDescription.TabIndex = 11;
            this.labelTemplateTypeDescription.Text = "The template type controls how ShipWorks processes the template for each item you" +
                " have selected in ShipWorks, such as orders or shipments.";
            // 
            // wizardPageFormat
            // 
            this.wizardPageFormat.Controls.Add(this.infoTemplateFormatXml);
            this.wizardPageFormat.Controls.Add(this.imageTemplateFormatXml);
            this.wizardPageFormat.Controls.Add(this.infoTemplateFormatPlainText);
            this.wizardPageFormat.Controls.Add(this.imageTemplateFormatPlainText);
            this.wizardPageFormat.Controls.Add(this.infoTemplateFormatHtml);
            this.wizardPageFormat.Controls.Add(this.imageTemplateFormatHtml);
            this.wizardPageFormat.Controls.Add(this.templateFormatXml);
            this.wizardPageFormat.Controls.Add(this.templateFormatPlainText);
            this.wizardPageFormat.Controls.Add(this.templateFormatHtml);
            this.wizardPageFormat.Controls.Add(this.labelTemplateFormat);
            this.wizardPageFormat.Description = "Select the output format of the template.";
            this.wizardPageFormat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageFormat.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageFormat.Location = new System.Drawing.Point(0, 0);
            this.wizardPageFormat.Name = "wizardPageFormat";
            this.wizardPageFormat.Size = new System.Drawing.Size(526, 278);
            this.wizardPageFormat.TabIndex = 0;
            this.wizardPageFormat.Title = "Template Format";
            this.wizardPageFormat.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextLastPage);
            // 
            // infoTemplateFormatXml
            // 
            this.infoTemplateFormatXml.ForeColor = System.Drawing.Color.DimGray;
            this.infoTemplateFormatXml.Location = new System.Drawing.Point(91, 196);
            this.infoTemplateFormatXml.Name = "infoTemplateFormatXml";
            this.infoTemplateFormatXml.Size = new System.Drawing.Size(406, 33);
            this.infoTemplateFormatXml.TabIndex = 30;
            this.infoTemplateFormatXml.Text = "The template output will be a well-formed XML document.";
            // 
            // imageTemplateFormatXml
            // 
            this.imageTemplateFormatXml.Image = ((System.Drawing.Image) (resources.GetObject("imageTemplateFormatXml.Image")));
            this.imageTemplateFormatXml.Location = new System.Drawing.Point(53, 193);
            this.imageTemplateFormatXml.Name = "imageTemplateFormatXml";
            this.imageTemplateFormatXml.Size = new System.Drawing.Size(32, 32);
            this.imageTemplateFormatXml.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imageTemplateFormatXml.TabIndex = 29;
            this.imageTemplateFormatXml.TabStop = false;
            // 
            // infoTemplateFormatPlainText
            // 
            this.infoTemplateFormatPlainText.ForeColor = System.Drawing.Color.DimGray;
            this.infoTemplateFormatPlainText.Location = new System.Drawing.Point(91, 126);
            this.infoTemplateFormatPlainText.Name = "infoTemplateFormatPlainText";
            this.infoTemplateFormatPlainText.Size = new System.Drawing.Size(406, 36);
            this.infoTemplateFormatPlainText.TabIndex = 28;
            this.infoTemplateFormatPlainText.Text = "The template output will not have any formatting.  This is useful for things such" +
                " as plain text emails, CSV files, and mailing labels.";
            // 
            // imageTemplateFormatPlainText
            // 
            this.imageTemplateFormatPlainText.Image = ((System.Drawing.Image) (resources.GetObject("imageTemplateFormatPlainText.Image")));
            this.imageTemplateFormatPlainText.Location = new System.Drawing.Point(53, 123);
            this.imageTemplateFormatPlainText.Name = "imageTemplateFormatPlainText";
            this.imageTemplateFormatPlainText.Size = new System.Drawing.Size(32, 32);
            this.imageTemplateFormatPlainText.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imageTemplateFormatPlainText.TabIndex = 27;
            this.imageTemplateFormatPlainText.TabStop = false;
            // 
            // infoTemplateFormatHtml
            // 
            this.infoTemplateFormatHtml.ForeColor = System.Drawing.Color.DimGray;
            this.infoTemplateFormatHtml.Location = new System.Drawing.Point(91, 55);
            this.infoTemplateFormatHtml.Name = "infoTemplateFormatHtml";
            this.infoTemplateFormatHtml.Size = new System.Drawing.Size(406, 39);
            this.infoTemplateFormatHtml.TabIndex = 26;
            this.infoTemplateFormatHtml.Text = "The template output will be HTML and can contain images, links, tables, and any o" +
                "ther HTML formatting.";
            // 
            // imageTemplateFormatHtml
            // 
            this.imageTemplateFormatHtml.Image = ((System.Drawing.Image) (resources.GetObject("imageTemplateFormatHtml.Image")));
            this.imageTemplateFormatHtml.Location = new System.Drawing.Point(53, 54);
            this.imageTemplateFormatHtml.Name = "imageTemplateFormatHtml";
            this.imageTemplateFormatHtml.Size = new System.Drawing.Size(32, 32);
            this.imageTemplateFormatHtml.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imageTemplateFormatHtml.TabIndex = 25;
            this.imageTemplateFormatHtml.TabStop = false;
            // 
            // templateFormatXml
            // 
            this.templateFormatXml.AutoSize = true;
            this.templateFormatXml.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.templateFormatXml.Location = new System.Drawing.Point(37, 174);
            this.templateFormatXml.Name = "templateFormatXml";
            this.templateFormatXml.Size = new System.Drawing.Size(48, 17);
            this.templateFormatXml.TabIndex = 24;
            this.templateFormatXml.Text = "XML";
            this.templateFormatXml.UseVisualStyleBackColor = true;
            // 
            // templateFormatPlainText
            // 
            this.templateFormatPlainText.AutoSize = true;
            this.templateFormatPlainText.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.templateFormatPlainText.Location = new System.Drawing.Point(37, 104);
            this.templateFormatPlainText.Name = "templateFormatPlainText";
            this.templateFormatPlainText.Size = new System.Drawing.Size(81, 17);
            this.templateFormatPlainText.TabIndex = 23;
            this.templateFormatPlainText.Text = "Plain Text";
            this.templateFormatPlainText.UseVisualStyleBackColor = true;
            // 
            // templateFormatHtml
            // 
            this.templateFormatHtml.AutoSize = true;
            this.templateFormatHtml.Checked = true;
            this.templateFormatHtml.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.templateFormatHtml.Location = new System.Drawing.Point(37, 35);
            this.templateFormatHtml.Name = "templateFormatHtml";
            this.templateFormatHtml.Size = new System.Drawing.Size(52, 17);
            this.templateFormatHtml.TabIndex = 22;
            this.templateFormatHtml.TabStop = true;
            this.templateFormatHtml.Text = "Html";
            this.templateFormatHtml.UseVisualStyleBackColor = true;
            // 
            // labelTemplateFormat
            // 
            this.labelTemplateFormat.Location = new System.Drawing.Point(22, 8);
            this.labelTemplateFormat.Name = "labelTemplateFormat";
            this.labelTemplateFormat.Size = new System.Drawing.Size(508, 20);
            this.labelTemplateFormat.TabIndex = 21;
            this.labelTemplateFormat.Text = "The output format determines the type of content and formatting that the template" +
                " will produce.";
            // 
            // AddTemplateWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.LastPageCancelable = true;
            this.Name = "AddTemplateWizard";
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageLocation,
            this.wizardPageType,
            this.wizardPageFormat});
            this.Text = "Add Template Wizard";
            this.Load += new System.EventHandler(this.OnLoad);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageLocation.ResumeLayout(false);
            this.wizardPageLocation.PerformLayout();
            this.wizardPageType.ResumeLayout(false);
            this.wizardPageType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.imageTemplateTypeReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.imageTemplateTypeLabels)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.imageTemplateTypeStandard)).EndInit();
            this.wizardPageFormat.ResumeLayout(false);
            this.wizardPageFormat.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.imageTemplateFormatXml)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.imageTemplateFormatPlainText)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.imageTemplateFormatHtml)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Wizard.WizardPage wizardPageLocation;
        private ShipWorks.Templates.Controls.TemplateTreeControl treeControl;
        private System.Windows.Forms.Label labelLocation;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label labelName;
        private ShipWorks.UI.Wizard.WizardPage wizardPageType;
        private System.Windows.Forms.Label infoTemplateTypeReport;
        private System.Windows.Forms.PictureBox imageTemplateTypeReport;
        private System.Windows.Forms.Label infoTemplateTypeLabels;
        private System.Windows.Forms.PictureBox imageTemplateTypeLabels;
        private System.Windows.Forms.Label infoTemplateTypeStandard;
        private System.Windows.Forms.PictureBox imageTemplateTypeStandard;
        private System.Windows.Forms.RadioButton templateTypeReport;
        private System.Windows.Forms.RadioButton templateTypeLabels;
        private System.Windows.Forms.RadioButton templateTypeStandard;
        private System.Windows.Forms.Label labelTemplateTypeDescription;
        private ShipWorks.UI.Wizard.WizardPage wizardPageFormat;
        private System.Windows.Forms.Label infoTemplateFormatXml;
        private System.Windows.Forms.PictureBox imageTemplateFormatXml;
        private System.Windows.Forms.Label infoTemplateFormatPlainText;
        private System.Windows.Forms.PictureBox imageTemplateFormatPlainText;
        private System.Windows.Forms.Label infoTemplateFormatHtml;
        private System.Windows.Forms.PictureBox imageTemplateFormatHtml;
        private System.Windows.Forms.RadioButton templateFormatXml;
        private System.Windows.Forms.RadioButton templateFormatPlainText;
        private System.Windows.Forms.RadioButton templateFormatHtml;
        private System.Windows.Forms.Label labelTemplateFormat;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
    }
}