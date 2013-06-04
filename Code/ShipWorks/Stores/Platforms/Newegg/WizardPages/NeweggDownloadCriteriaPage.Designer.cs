namespace ShipWorks.Stores.Platforms.Newegg.WizardPages
{
    partial class NeweggDownloadCriteriaPage
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
            this.labelCriteria = new System.Windows.Forms.Label();
            this.pictureCriteria = new System.Windows.Forms.PictureBox();
            this.excludeFulfilledByNewegg = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureCriteria)).BeginInit();
            this.SuspendLayout();
            // 
            // labelCriteria
            // 
            this.labelCriteria.AutoSize = true;
            this.labelCriteria.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCriteria.Location = new System.Drawing.Point(36, 12);
            this.labelCriteria.Name = "labelCriteria";
            this.labelCriteria.Size = new System.Drawing.Size(107, 13);
            this.labelCriteria.TabIndex = 19;
            this.labelCriteria.Text = "Download Criteria";
            // 
            // pictureCriteria
            // 
            this.pictureCriteria.Image = global::ShipWorks.Properties.Resources.notebook_preferences;
            this.pictureCriteria.Location = new System.Drawing.Point(14, 11);
            this.pictureCriteria.Name = "pictureCriteria";
            this.pictureCriteria.Size = new System.Drawing.Size(16, 16);
            this.pictureCriteria.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureCriteria.TabIndex = 20;
            this.pictureCriteria.TabStop = false;
            // 
            // excludeFulfilledByNewegg
            // 
            this.excludeFulfilledByNewegg.AutoSize = true;
            this.excludeFulfilledByNewegg.Location = new System.Drawing.Point(39, 33);
            this.excludeFulfilledByNewegg.Name = "excludeFulfilledByNewegg";
            this.excludeFulfilledByNewegg.Size = new System.Drawing.Size(277, 17);
            this.excludeFulfilledByNewegg.TabIndex = 18;
            this.excludeFulfilledByNewegg.Text = "Do not download orders that are fulfilled by Newegg";
            this.excludeFulfilledByNewegg.UseVisualStyleBackColor = true;
            // 
            // NeweggDownloadCriteriaPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.pictureCriteria);
            this.Controls.Add(this.labelCriteria);
            this.Controls.Add(this.excludeFulfilledByNewegg);
            this.Name = "NeweggDownloadCriteriaPage";
            this.Size = new System.Drawing.Size(432, 156);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            ((System.ComponentModel.ISupportInitialize)(this.pictureCriteria)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureCriteria;
        private System.Windows.Forms.Label labelCriteria;
        private System.Windows.Forms.CheckBox excludeFulfilledByNewegg;
    }
}
