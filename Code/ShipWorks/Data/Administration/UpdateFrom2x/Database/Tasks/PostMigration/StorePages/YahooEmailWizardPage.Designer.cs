using ShipWorks.Stores.Platforms.Yahoo.EmailIntegration;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.StorePages
{
    partial class YahooEmailWizardPage
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
            this.skipCheckBox = new System.Windows.Forms.CheckBox();
            this.emailAccountControl = new YahooEmailAccountControl();
            this.storeNameLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // skipCheckBox
            // 
            this.skipCheckBox.AutoSize = true;
            this.skipCheckBox.Location = new System.Drawing.Point(25, 96);
            this.skipCheckBox.Name = "skipCheckBox";
            this.skipCheckBox.Size = new System.Drawing.Size(208, 17);
            this.skipCheckBox.TabIndex = 16;
            this.skipCheckBox.Text = "I will configure this after the upgrade.";
            this.skipCheckBox.UseVisualStyleBackColor = true;
            // 
            // emailAccountControl
            // 
            this.emailAccountControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emailAccountControl.Location = new System.Drawing.Point(81, 128);
            this.emailAccountControl.Name = "emailAccountControl";
            this.emailAccountControl.Size = new System.Drawing.Size(316, 76);
            this.emailAccountControl.TabIndex = 15;
            // 
            // storeNameLabel
            // 
            this.storeNameLabel.AutoSize = true;
            this.storeNameLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storeNameLabel.Location = new System.Drawing.Point(21, 76);
            this.storeNameLabel.Name = "storeNameLabel";
            this.storeNameLabel.Size = new System.Drawing.Size(127, 13);
            this.storeNameLabel.TabIndex = 13;
            this.storeNameLabel.Text = "Yahoo! Store";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(78, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(365, 32);
            this.label1.TabIndex = 12;
            this.label1.Text = "Your Yahoo! Store is missing SMTP configuration for sending order updates back to" +
                " Yahoo!.";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::ShipWorks.Properties.Resources.mail_forward3;
            this.pictureBox4.Location = new System.Drawing.Point(24, 9);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(48, 48);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 11;
            this.pictureBox4.TabStop = false;
            // 
            // YahooEmailWizardPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.skipCheckBox);
            this.Controls.Add(this.emailAccountControl);
            this.Controls.Add(this.storeNameLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox4);
            this.Description = "ShipWorks needs more information about your Yahoo! store.";
            this.Name = "YahooEmailWizardPage";
            this.Size = new System.Drawing.Size(500, 300);
            this.Title = "Yahoo! Email Account";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label storeNameLabel;
        private YahooEmailAccountControl emailAccountControl;
        private System.Windows.Forms.CheckBox skipCheckBox;
    }
}
