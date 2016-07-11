namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages
{
    partial class OdbcUploadShipmentDataSourceWizardPage
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
            this.uploadToSame = new System.Windows.Forms.RadioButton();
            this.uploadToDifferent = new System.Windows.Forms.RadioButton();
            this.doNotUpload = new System.Windows.Forms.RadioButton();
            this.uploadStrategyPanel = new System.Windows.Forms.Panel();
            this.uploadStrategyPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // uploadToSame
            // 
            this.uploadToSame.AutoSize = true;
            this.uploadToSame.Checked = true;
            this.uploadToSame.Location = new System.Drawing.Point(3, 3);
            this.uploadToSame.Name = "uploadToSame";
            this.uploadToSame.Size = new System.Drawing.Size(178, 17);
            this.uploadToSame.TabIndex = 0;
            this.uploadToSame.TabStop = true;
            this.uploadToSame.Text = "Upload to the same data source";
            this.uploadToSame.UseVisualStyleBackColor = true;
            // 
            // uploadToDifferent
            // 
            this.uploadToDifferent.AutoSize = true;
            this.uploadToDifferent.Location = new System.Drawing.Point(3, 26);
            this.uploadToDifferent.Name = "uploadToDifferent";
            this.uploadToDifferent.Size = new System.Drawing.Size(185, 17);
            this.uploadToDifferent.TabIndex = 1;
            this.uploadToDifferent.Text = "Upload to a different data source";
            this.uploadToDifferent.UseVisualStyleBackColor = true;
            // 
            // doNotUpload
            // 
            this.doNotUpload.AutoSize = true;
            this.doNotUpload.Location = new System.Drawing.Point(3, 49);
            this.doNotUpload.Name = "doNotUpload";
            this.doNotUpload.Size = new System.Drawing.Size(163, 17);
            this.doNotUpload.TabIndex = 2;
            this.doNotUpload.Text = "Do not upload shipment data";
            this.doNotUpload.UseVisualStyleBackColor = true;
            // 
            // uploadStrategyPanel
            // 
            this.uploadStrategyPanel.Controls.Add(this.uploadToDifferent);
            this.uploadStrategyPanel.Controls.Add(this.uploadToSame);
            this.uploadStrategyPanel.Controls.Add(this.doNotUpload);
            this.uploadStrategyPanel.Location = new System.Drawing.Point(20, 10);
            this.uploadStrategyPanel.Name = "uploadStrategyPanel";
            this.uploadStrategyPanel.Size = new System.Drawing.Size(198, 78);
            this.uploadStrategyPanel.TabIndex = 3;
            // 
            // OdbcUploadShipmentDataSourceWizardPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.uploadStrategyPanel);
            this.Description = "Select how you would like to upload shipment data for your store";
            this.Name = "OdbcUploadShipmentDataSourceWizardPage";
            this.Size = new System.Drawing.Size(500, 500);
            this.Title = "Upload Options";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.uploadStrategyPanel.ResumeLayout(false);
            this.uploadStrategyPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton uploadToSame;
        private System.Windows.Forms.RadioButton uploadToDifferent;
        private System.Windows.Forms.RadioButton doNotUpload;
        private System.Windows.Forms.Panel uploadStrategyPanel;
    }
}
