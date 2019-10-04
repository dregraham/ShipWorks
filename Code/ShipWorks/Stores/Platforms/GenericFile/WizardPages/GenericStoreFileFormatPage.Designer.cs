namespace ShipWorks.Stores.Platforms.GenericFile.WizardPages
{
    partial class GenericStoreFileFormatPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenericStoreFileFormatPage));
            this.label1 = new System.Windows.Forms.Label();
            this.labelXml = new System.Windows.Forms.Label();
            this.pictureXml = new System.Windows.Forms.PictureBox();
            this.labelCsv = new System.Windows.Forms.Label();
            this.pictureCsv = new System.Windows.Forms.PictureBox();
            this.radioXml = new System.Windows.Forms.RadioButton();
            this.radioCsv = new System.Windows.Forms.RadioButton();
            this.labelExcel = new System.Windows.Forms.Label();
            this.imageExcel = new System.Windows.Forms.PictureBox();
            this.radioExcel = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize) (this.pictureXml)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureCsv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.imageExcel)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label1.Location = new System.Drawing.Point(18, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(262, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "What type of file will ShipWorks be importing:";
            // 
            // labelXml
            // 
            this.labelXml.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelXml.Location = new System.Drawing.Point(93, 133);
            this.labelXml.Name = "labelXml";
            this.labelXml.Size = new System.Drawing.Size(406, 15);
            this.labelXml.TabIndex = 16;
            this.labelXml.Text = "Select this option to import XML files of hierarchical data.";
            // 
            // pictureXml
            // 
            this.pictureXml.Image = global::ShipWorks.Properties.Resources.text_code_colored;
            this.pictureXml.Location = new System.Drawing.Point(55, 126);
            this.pictureXml.Name = "pictureXml";
            this.pictureXml.Size = new System.Drawing.Size(32, 32);
            this.pictureXml.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureXml.TabIndex = 18;
            this.pictureXml.TabStop = false;
            // 
            // labelCsv
            // 
            this.labelCsv.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelCsv.Location = new System.Drawing.Point(93, 66);
            this.labelCsv.Name = "labelCsv";
            this.labelCsv.Size = new System.Drawing.Size(392, 26);
            this.labelCsv.TabIndex = 13;
            this.labelCsv.Text = "Select this option to import flat files of delimited data.";
            // 
            // pictureCsv
            // 
            this.pictureCsv.Image = global::ShipWorks.Properties.Resources.table_selection_row;
            this.pictureCsv.Location = new System.Drawing.Point(55, 56);
            this.pictureCsv.Name = "pictureCsv";
            this.pictureCsv.Size = new System.Drawing.Size(32, 32);
            this.pictureCsv.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureCsv.TabIndex = 17;
            this.pictureCsv.TabStop = false;
            // 
            // radioXml
            // 
            this.radioXml.AutoSize = true;
            this.radioXml.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.radioXml.Location = new System.Drawing.Point(39, 107);
            this.radioXml.Name = "radioXml";
            this.radioXml.Size = new System.Drawing.Size(44, 17);
            this.radioXml.TabIndex = 14;
            this.radioXml.Text = "XML";
            this.radioXml.UseVisualStyleBackColor = true;
            // 
            // radioCsv
            // 
            this.radioCsv.AutoSize = true;
            this.radioCsv.Checked = true;
            this.radioCsv.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.radioCsv.Location = new System.Drawing.Point(39, 37);
            this.radioCsv.Name = "radioCsv";
            this.radioCsv.Size = new System.Drawing.Size(76, 17);
            this.radioCsv.TabIndex = 12;
            this.radioCsv.TabStop = true;
            this.radioCsv.Text = "CSV / Text";
            this.radioCsv.UseVisualStyleBackColor = true;
            // 
            // labelExcel
            // 
            this.labelExcel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelExcel.Location = new System.Drawing.Point(93, 201);
            this.labelExcel.Name = "labelExcel";
            this.labelExcel.Size = new System.Drawing.Size(406, 67);
            this.labelExcel.TabIndex = 21;
            this.labelExcel.Text = "Select this option to import Microsoft Excel formatted files.";
            // 
            // imageExcel
            // 
            this.imageExcel.Image = ((System.Drawing.Image) (resources.GetObject("imageExcel.Image")));
            this.imageExcel.Location = new System.Drawing.Point(55, 194);
            this.imageExcel.Name = "imageExcel";
            this.imageExcel.Size = new System.Drawing.Size(32, 32);
            this.imageExcel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imageExcel.TabIndex = 22;
            this.imageExcel.TabStop = false;
            // 
            // radioExcel
            // 
            this.radioExcel.AutoSize = true;
            this.radioExcel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.radioExcel.Location = new System.Drawing.Point(39, 175);
            this.radioExcel.Name = "radioExcel";
            this.radioExcel.Size = new System.Drawing.Size(50, 17);
            this.radioExcel.TabIndex = 20;
            this.radioExcel.Text = "Excel";
            this.radioExcel.UseVisualStyleBackColor = true;
            // 
            // GenericStoreFileFormatPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelExcel);
            this.Controls.Add(this.imageExcel);
            this.Controls.Add(this.radioExcel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelXml);
            this.Controls.Add(this.pictureXml);
            this.Controls.Add(this.labelCsv);
            this.Controls.Add(this.pictureCsv);
            this.Controls.Add(this.radioXml);
            this.Controls.Add(this.radioCsv);
            this.Name = "GenericStoreFileFormatPage";
            this.Size = new System.Drawing.Size(522, 272);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            ((System.ComponentModel.ISupportInitialize) (this.pictureXml)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureCsv)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.imageExcel)).EndInit();
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelXml;
        private System.Windows.Forms.PictureBox pictureXml;
        private System.Windows.Forms.Label labelCsv;
        private System.Windows.Forms.PictureBox pictureCsv;
        private System.Windows.Forms.RadioButton radioXml;
        private System.Windows.Forms.RadioButton radioCsv;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelExcel;
        private System.Windows.Forms.PictureBox imageExcel;
        private System.Windows.Forms.RadioButton radioExcel;
    }
}
