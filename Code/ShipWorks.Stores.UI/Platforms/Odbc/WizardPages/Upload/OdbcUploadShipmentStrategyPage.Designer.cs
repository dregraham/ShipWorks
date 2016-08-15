namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages.Upload
{
    partial class OdbcUploadShipmentStrategyPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OdbcUploadShipmentStrategyPage));
            this.useImportDataSource = new System.Windows.Forms.RadioButton();
            this.useShipmentDataSource = new System.Windows.Forms.RadioButton();
            this.doNotUpload = new System.Windows.Forms.RadioButton();
            this.uploadStrategyPanel = new System.Windows.Forms.Panel();
            this.labelShipmentUpdate = new System.Windows.Forms.Label();
            this.pictureBoxShipmentUpdate = new System.Windows.Forms.PictureBox();
            this.uploadStrategyPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxShipmentUpdate)).BeginInit();
            this.SuspendLayout();
            // 
            // useImportDataSource
            // 
            this.useImportDataSource.AutoSize = true;
            this.useImportDataSource.Location = new System.Drawing.Point(3, 26);
            this.useImportDataSource.Name = "useImportDataSource";
            this.useImportDataSource.Size = new System.Drawing.Size(258, 17);
            this.useImportDataSource.TabIndex = 0;
            this.useImportDataSource.Text = "Upload shipment details to the same data source";
            this.useImportDataSource.UseVisualStyleBackColor = true;
            // 
            // useShipmentDataSource
            // 
            this.useShipmentDataSource.AutoSize = true;
            this.useShipmentDataSource.Location = new System.Drawing.Point(3, 49);
            this.useShipmentDataSource.Name = "useShipmentDataSource";
            this.useShipmentDataSource.Size = new System.Drawing.Size(265, 17);
            this.useShipmentDataSource.TabIndex = 1;
            this.useShipmentDataSource.Text = "Upload shipment details to a different data source";
            this.useShipmentDataSource.UseVisualStyleBackColor = true;
            // 
            // doNotUpload
            // 
            this.doNotUpload.AutoSize = true;
            this.doNotUpload.Checked = true;
            this.doNotUpload.Location = new System.Drawing.Point(3, 3);
            this.doNotUpload.Name = "doNotUpload";
            this.doNotUpload.Size = new System.Drawing.Size(172, 17);
            this.doNotUpload.TabIndex = 2;
            this.doNotUpload.TabStop = true;
            this.doNotUpload.Text = "Do not upload shipment details";
            this.doNotUpload.UseVisualStyleBackColor = true;
            // 
            // uploadStrategyPanel
            // 
            this.uploadStrategyPanel.Controls.Add(this.useShipmentDataSource);
            this.uploadStrategyPanel.Controls.Add(this.useImportDataSource);
            this.uploadStrategyPanel.Controls.Add(this.doNotUpload);
            this.uploadStrategyPanel.Location = new System.Drawing.Point(60, 31);
            this.uploadStrategyPanel.Name = "uploadStrategyPanel";
            this.uploadStrategyPanel.Size = new System.Drawing.Size(437, 78);
            this.uploadStrategyPanel.TabIndex = 3;
            // 
            // labelShipmentUpdate
            // 
            this.labelShipmentUpdate.AutoSize = true;
            this.labelShipmentUpdate.Location = new System.Drawing.Point(50, 15);
            this.labelShipmentUpdate.Name = "labelShipmentUpdate";
            this.labelShipmentUpdate.Size = new System.Drawing.Size(171, 13);
            this.labelShipmentUpdate.TabIndex = 27;
            this.labelShipmentUpdate.Text = "Where to upload shipment details:";
            // 
            // pictureBoxShipmentUpdate
            // 
            this.pictureBoxShipmentUpdate.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxShipmentUpdate.Image")));
            this.pictureBoxShipmentUpdate.Location = new System.Drawing.Point(20, 10);
            this.pictureBoxShipmentUpdate.Name = "pictureBoxShipmentUpdate";
            this.pictureBoxShipmentUpdate.Size = new System.Drawing.Size(24, 24);
            this.pictureBoxShipmentUpdate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxShipmentUpdate.TabIndex = 26;
            this.pictureBoxShipmentUpdate.TabStop = false;
            // 
            // OdbcUploadShipmentStrategyPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelShipmentUpdate);
            this.Controls.Add(this.pictureBoxShipmentUpdate);
            this.Controls.Add(this.uploadStrategyPanel);
            this.Description = "Select how you would like to upload shipment details for your store.";
            this.Name = "OdbcUploadShipmentStrategyPage";
            this.Size = new System.Drawing.Size(500, 500);
            this.Title = "Upload Options";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.StepBack += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepBack);
            this.uploadStrategyPanel.ResumeLayout(false);
            this.uploadStrategyPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxShipmentUpdate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton useImportDataSource;
        private System.Windows.Forms.RadioButton useShipmentDataSource;
        private System.Windows.Forms.RadioButton doNotUpload;
        private System.Windows.Forms.Panel uploadStrategyPanel;
        private System.Windows.Forms.Label labelShipmentUpdate;
        private System.Windows.Forms.PictureBox pictureBoxShipmentUpdate;
    }
}
