namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.StorePages
{
    partial class NetworkSolutionsLicenseWizardPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NetworkSolutionsLicenseWizardPage));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panelUpdating = new System.Windows.Forms.Panel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label13 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).BeginInit();
            this.panelUpdating.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.school;
            this.pictureBox1.Location = new System.Drawing.Point(24, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(77, 10);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(341, 13);
            this.label12.TabIndex = 8;
            this.label12.Text = "ShipWorks needs to configure you Network Solutions licensing details.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(77, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(278, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Click Next to update your ShipWorks license information.";
            // 
            // panelUpdating
            // 
            this.panelUpdating.Controls.Add(this.pictureBox3);
            this.panelUpdating.Controls.Add(this.label13);
            this.panelUpdating.Location = new System.Drawing.Point(76, 5);
            this.panelUpdating.Name = "panelUpdating";
            this.panelUpdating.Size = new System.Drawing.Size(390, 71);
            this.panelUpdating.TabIndex = 9;
            this.panelUpdating.Visible = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image) (resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(11, 9);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(16, 16);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 1;
            this.pictureBox3.TabStop = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(31, 11);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(345, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Please wait while ShipWorks configures your Network Solutions stores.";
            // 
            // NetworkSolutionsLicenseWizardPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.panelUpdating);
            this.Description = "ShipWorks needs to update license details.";
            this.Name = "NetworkSolutionsLicenseWizardPage";
            this.Size = new System.Drawing.Size(500, 236);
            this.Title = "Network Solutions Store";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).EndInit();
            this.panelUpdating.ResumeLayout(false);
            this.panelUpdating.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panelUpdating;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label13;
    }
}
