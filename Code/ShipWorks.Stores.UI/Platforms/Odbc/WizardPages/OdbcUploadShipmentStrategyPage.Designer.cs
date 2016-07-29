namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages
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
            this.uploadToSame = new System.Windows.Forms.RadioButton();
            this.uploadToDifferent = new System.Windows.Forms.RadioButton();
            this.doNotUpload = new System.Windows.Forms.RadioButton();
            this.uploadStrategyPanel = new System.Windows.Forms.Panel();
            this.labelShipmentUpdate = new System.Windows.Forms.Label();
            this.pictureBoxShipmentUpdate = new System.Windows.Forms.PictureBox();
            this.uploadStrategyPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxShipmentUpdate)).BeginInit();
            this.SuspendLayout();
            // 
            // uploadToSame
            // 
            this.uploadToSame.AutoSize = true;
            this.uploadToSame.Location = new System.Drawing.Point(3, 26);
            this.uploadToSame.Name = "uploadToSame";
            this.uploadToSame.Size = new System.Drawing.Size(258, 17);
            this.uploadToSame.TabIndex = 0;
            this.uploadToSame.Text = "Upload shipment details to the same data source";
            this.uploadToSame.UseVisualStyleBackColor = true;
            // 
            // uploadToDifferent
            // 
            this.uploadToDifferent.AutoSize = true;
            this.uploadToDifferent.Location = new System.Drawing.Point(3, 49);
            this.uploadToDifferent.Name = "uploadToDifferent";
            this.uploadToDifferent.Size = new System.Drawing.Size(265, 17);
            this.uploadToDifferent.TabIndex = 1;
            this.uploadToDifferent.Text = "Upload shipment details to a different data source";
            this.uploadToDifferent.UseVisualStyleBackColor = true;
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
            this.uploadStrategyPanel.Controls.Add(this.uploadToDifferent);
            this.uploadStrategyPanel.Controls.Add(this.uploadToSame);
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
            this.uploadStrategyPanel.ResumeLayout(false);
            this.uploadStrategyPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxShipmentUpdate)).EndInit();
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton uploadToSame;
        private System.Windows.Forms.RadioButton uploadToDifferent;
        private System.Windows.Forms.RadioButton doNotUpload;
        private System.Windows.Forms.Panel uploadStrategyPanel;
        private System.Windows.Forms.Label labelShipmentUpdate;
        private System.Windows.Forms.PictureBox pictureBoxShipmentUpdate;
    }
}
