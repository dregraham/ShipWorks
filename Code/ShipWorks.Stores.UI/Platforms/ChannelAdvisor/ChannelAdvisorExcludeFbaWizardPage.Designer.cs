namespace ShipWorks.Stores.UI.Platforms.ChannelAdvisor
{
    partial class ChannelAdvisorExcludeFbaWizardPage
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
            this.pictureCriteria = new System.Windows.Forms.PictureBox();
            this.labelCriteria = new System.Windows.Forms.Label();
            this.excludeFba = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize) (this.pictureCriteria)).BeginInit();
            this.SuspendLayout();
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
            // excludeFba
            // 
            this.excludeFba.AutoSize = true;
            this.excludeFba.Location = new System.Drawing.Point(48, 35);
            this.excludeFba.Name = "excludeFba";
            this.excludeFba.Size = new System.Drawing.Size(308, 17);
            this.excludeFba.TabIndex = 3;
            this.excludeFba.Text = "Do not download orders that are Fulfilled By Amazon (FBA)";
            this.excludeFba.UseVisualStyleBackColor = true;
            // 
            // AmazonMwsDownloadCriteriaPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.excludeFba);
            this.Controls.Add(this.labelCriteria);
            this.Controls.Add(this.pictureCriteria);
            this.Name = "AmazonMwsDownloadCriteriaPage";
            this.Size = new System.Drawing.Size(460, 216);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            ((System.ComponentModel.ISupportInitialize) (this.pictureCriteria)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureCriteria;
        private System.Windows.Forms.Label labelCriteria;
        private System.Windows.Forms.CheckBox excludeFba;

    }
}
