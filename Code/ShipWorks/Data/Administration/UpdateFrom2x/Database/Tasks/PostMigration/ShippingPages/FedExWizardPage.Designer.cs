namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.ShippingPages
{
    partial class FedExWizardPage
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
            this.labelTitle = new System.Windows.Forms.Label();
            this.accountLabel = new System.Windows.Forms.Label();
            this.labelnfo1 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.lineContainer = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelTitle.Location = new System.Drawing.Point(157, 9);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(91, 13);
            this.labelTitle.TabIndex = 9;
            this.labelTitle.Text = "FedEx Shipping";
            // 
            // accountLabel
            // 
            this.accountLabel.AutoSize = true;
            this.accountLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.accountLabel.Location = new System.Drawing.Point(56, 82);
            this.accountLabel.Name = "accountLabel";
            this.accountLabel.Size = new System.Drawing.Size(95, 13);
            this.accountLabel.TabIndex = 10;
            this.accountLabel.Text = "FedEx Accounts";
            // 
            // labelnfo1
            // 
            this.labelnfo1.Location = new System.Drawing.Point(158, 30);
            this.labelnfo1.Name = "labelnfo1";
            this.labelnfo1.Size = new System.Drawing.Size(326, 46);
            this.labelnfo1.TabIndex = 12;
            this.labelnfo1.Text = "ShipWorks 3 found some FedEx shipments in your ShipWorks 2 database.  ShipWorks n" +
                "eeds to migrate any FedEx accounts you have for use with ShipWorks 3.";
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.White;
            this.pictureBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox4.Image = global::ShipWorks.Properties.Resources.fedex_Logo;
            this.pictureBox4.Location = new System.Drawing.Point(13, 9);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(136, 59);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox4.TabIndex = 7;
            this.pictureBox4.TabStop = false;
            // 
            // lineContainer
            // 
            this.lineContainer.AutoScroll = true;
            this.lineContainer.Location = new System.Drawing.Point(59, 98);
            this.lineContainer.Name = "lineContainer";
            this.lineContainer.Size = new System.Drawing.Size(425, 124);
            this.lineContainer.TabIndex = 13;
            // 
            // FedExWizardPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lineContainer);
            this.Controls.Add(this.labelnfo1);
            this.Controls.Add(this.accountLabel);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.pictureBox4);
            this.Description = "Setup ShipWorks 3 for FedEx shipping.";
            this.Name = "FedExWizardPage";
            this.Size = new System.Drawing.Size(495, 253);
            this.Title = "FedEx Setup";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label accountLabel;
        private System.Windows.Forms.Label labelnfo1;
        private System.Windows.Forms.Panel lineContainer;
    }
}
