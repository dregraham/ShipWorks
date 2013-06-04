namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration
{
    partial class ImportCertificatesWizardPage
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
            this.label25 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.skipCheckBox = new System.Windows.Forms.CheckBox();
            this.storeNameLabel = new System.Windows.Forms.Label();
            this.payPalCredentials = new ShipWorks.Stores.Platforms.PayPal.PayPalCredentialsControl();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // label25
            // 
            this.label25.Location = new System.Drawing.Point(72, 10);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(420, 31);
            this.label25.TabIndex = 5;
            this.label25.Text = "The SSL Certificate used by the following store was not able to be transferred du" +
                "ring the upgrade and needs to be imported\r\n";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::ShipWorks.Properties.Resources.document_certificate;
            this.pictureBox4.Location = new System.Drawing.Point(18, 9);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(48, 48);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 4;
            this.pictureBox4.TabStop = false;
            // 
            // skipCheckBox
            // 
            this.skipCheckBox.AutoSize = true;
            this.skipCheckBox.Location = new System.Drawing.Point(75, 43);
            this.skipCheckBox.Name = "skipCheckBox";
            this.skipCheckBox.Size = new System.Drawing.Size(175, 17);
            this.skipCheckBox.TabIndex = 9;
            this.skipCheckBox.Text = "I will import the certificate later";
            this.skipCheckBox.UseVisualStyleBackColor = true;
            this.skipCheckBox.CheckedChanged += new System.EventHandler(this.OnSkipChanged);
            // 
            // storeNameLabel
            // 
            this.storeNameLabel.AutoSize = true;
            this.storeNameLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.storeNameLabel.Location = new System.Drawing.Point(15, 84);
            this.storeNameLabel.Name = "storeNameLabel";
            this.storeNameLabel.Size = new System.Drawing.Size(135, 13);
            this.storeNameLabel.TabIndex = 9;
            this.storeNameLabel.Text = "Store Name Certificate";
            // 
            // payPalCredentials
            // 
            this.payPalCredentials.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.payPalCredentials.Location = new System.Drawing.Point(33, 101);
            this.payPalCredentials.MinimumSize = new System.Drawing.Size(427, 142);
            this.payPalCredentials.Name = "payPalCredentials";
            this.payPalCredentials.Size = new System.Drawing.Size(479, 142);
            this.payPalCredentials.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label2.Location = new System.Drawing.Point(91, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(407, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Important: ShipWorks cannot connect to your store until the certificate is import" +
                "ed.\r\n";
            // 
            // ImportCertificatesWizardPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.payPalCredentials);
            this.Controls.Add(this.storeNameLabel);
            this.Controls.Add(this.skipCheckBox);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.pictureBox4);
            this.Description = "ShipWorks could not transfer the SSL certificate for your store.";
            this.Name = "ImportCertificatesWizardPage";
            this.Size = new System.Drawing.Size(529, 255);
            this.Title = "Import SSL Certificate";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.CheckBox skipCheckBox;
        private System.Windows.Forms.Label storeNameLabel;
        private Stores.Platforms.PayPal.PayPalCredentialsControl payPalCredentials;
        private System.Windows.Forms.Label label2;
    }
}
