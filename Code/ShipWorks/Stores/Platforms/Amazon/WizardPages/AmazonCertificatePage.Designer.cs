namespace ShipWorks.Stores.Platforms.Amazon.WizardPages
{
    partial class AmazonCertificatePage
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
            this.importControl = new ShipWorks.Stores.Platforms.Amazon.AmazonCertificateImportControl();
            this.SuspendLayout();
            // 
            // importControl
            // 
            this.importControl.AccessKeyID = "";
            this.importControl.Location = new System.Drawing.Point(0, 0);
            this.importControl.Name = "importControl";
            this.importControl.Size = new System.Drawing.Size(516, 290);
            this.importControl.TabIndex = 0;
            // 
            // CertificatePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.importControl);
            this.Description = "Enter your Access Key and Certificate for AWS.";
            this.Name = "CertificatePage";
            this.Size = new System.Drawing.Size(519, 299);
            this.Title = "Amazon Certificate";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);

        }

        #endregion

        private AmazonCertificateImportControl importControl;
    }
}
