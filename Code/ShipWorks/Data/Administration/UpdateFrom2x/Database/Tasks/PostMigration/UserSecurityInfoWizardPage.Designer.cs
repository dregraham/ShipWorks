namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration
{
    partial class UserSecurityInfoWizardPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserSecurityInfoWizardPage));
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // label26
            // 
            this.label26.Location = new System.Drawing.Point(65, 37);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(403, 80);
            this.label26.TabIndex = 5;
            this.label26.Text = resources.GetString("label26.Text");
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(65, 14);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(372, 13);
            this.label25.TabIndex = 4;
            this.label25.Text = "ShipWorks now supports more control over what users can do in ShipWorks.";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::ShipWorks.Properties.Resources.users4;
            this.pictureBox4.Location = new System.Drawing.Point(10, 14);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(48, 48);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 3;
            this.pictureBox4.TabStop = false;
            // 
            // UserSecurityInfoWizardPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label26);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.pictureBox4);
            this.Description = "Changes to ShipWorks user accounts.";
            this.Name = "UserSecurityInfoWizardPage";
            this.Size = new System.Drawing.Size(508, 151);
            this.Title = "User Accounts";
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.PictureBox pictureBox4;
    }
}
