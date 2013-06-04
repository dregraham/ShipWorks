namespace ShipWorks.Stores.Platforms.ChannelAdvisor.WizardPages
{
    partial class ChannelAdvisorDownloadCriteriaPage
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
            this.storeSettings = new ShipWorks.Stores.Platforms.ChannelAdvisor.ChannelAdvisorStoreSettingsControl();
            this.pictureCriteria = new System.Windows.Forms.PictureBox();
            this.labelCriteria = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize) (this.pictureCriteria)).BeginInit();
            this.SuspendLayout();
            // 
            // storeSettings
            // 
            this.storeSettings.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.storeSettings.Location = new System.Drawing.Point(37, 26);
            this.storeSettings.Name = "storeSettings";
            this.storeSettings.ShowHeader = false;
            this.storeSettings.Size = new System.Drawing.Size(425, 59);
            this.storeSettings.TabIndex = 0;
            // 
            // pictureCriteria
            // 
            this.pictureCriteria.Image = global::ShipWorks.Properties.Resources.notebook_preferences;
            this.pictureCriteria.Location = new System.Drawing.Point(27, 12);
            this.pictureCriteria.Name = "pictureCriteria";
            this.pictureCriteria.Size = new System.Drawing.Size(16, 16);
            this.pictureCriteria.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureCriteria.TabIndex = 1;
            this.pictureCriteria.TabStop = false;
            // 
            // labelCriteria
            // 
            this.labelCriteria.AutoSize = true;
            this.labelCriteria.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelCriteria.Location = new System.Drawing.Point(45, 14);
            this.labelCriteria.Name = "labelCriteria";
            this.labelCriteria.Size = new System.Drawing.Size(107, 13);
            this.labelCriteria.TabIndex = 2;
            this.labelCriteria.Text = "Download Criteria";
            // 
            // ChannelAdvisorDownloadCriteriaPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelCriteria);
            this.Controls.Add(this.pictureCriteria);
            this.Controls.Add(this.storeSettings);
            this.Name = "ChannelAdvisorDownloadCriteriaPage";
            this.Size = new System.Drawing.Size(460, 216);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            ((System.ComponentModel.ISupportInitialize) (this.pictureCriteria)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ChannelAdvisorStoreSettingsControl storeSettings;
        private System.Windows.Forms.PictureBox pictureCriteria;
        private System.Windows.Forms.Label labelCriteria;

    }
}
