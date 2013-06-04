namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.StorePages
{
    partial class AmazonWizardPage
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
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.skipCheckBox = new System.Windows.Forms.CheckBox();
            this.storeNameLabel = new System.Windows.Forms.Label();
            this.mwsAccountSettings = new ShipWorks.Stores.Platforms.Amazon.AmazonMwsAccountSettingsControl();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::ShipWorks.Properties.Resources.school;
            this.pictureBox4.Location = new System.Drawing.Point(24, 9);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(48, 48);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 12;
            this.pictureBox4.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(78, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(365, 32);
            this.label1.TabIndex = 13;
            this.label1.Text = "Amazon now requires sellers use Amazon Marketplace Web Service (MWS) to access th" +
                "eir order information.";
            // 
            // skipCheckBox
            // 
            this.skipCheckBox.AutoSize = true;
            this.skipCheckBox.Location = new System.Drawing.Point(25, 97);
            this.skipCheckBox.Name = "skipCheckBox";
            this.skipCheckBox.Size = new System.Drawing.Size(208, 17);
            this.skipCheckBox.TabIndex = 18;
            this.skipCheckBox.Text = "I will configure this after the upgrade.";
            this.skipCheckBox.UseVisualStyleBackColor = true;
            // 
            // storeNameLabel
            // 
            this.storeNameLabel.AutoSize = true;
            this.storeNameLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.storeNameLabel.Location = new System.Drawing.Point(21, 77);
            this.storeNameLabel.Name = "storeNameLabel";
            this.storeNameLabel.Size = new System.Drawing.Size(87, 13);
            this.storeNameLabel.TabIndex = 17;
            this.storeNameLabel.Text = "Amazon Store";
            // 
            // mwsAccountSettings
            // 
            this.mwsAccountSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.mwsAccountSettings.Location = new System.Drawing.Point(47, 110);
            this.mwsAccountSettings.Name = "mwsAccountSettings";
            this.mwsAccountSettings.Size = new System.Drawing.Size(425, 176);
            this.mwsAccountSettings.TabIndex = 19;
            // 
            // AmazonWizardPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.skipCheckBox);
            this.Controls.Add(this.storeNameLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.mwsAccountSettings);
            this.Description = "ShipWorks needs more information about your Amazon MWS account.";
            this.Name = "AmazonWizardPage";
            this.Size = new System.Drawing.Size(500, 300);
            this.Title = "Amazon Marketplace Web Services";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox skipCheckBox;
        private System.Windows.Forms.Label storeNameLabel;
        private Stores.Platforms.Amazon.AmazonMwsAccountSettingsControl mwsAccountSettings;
    }
}
