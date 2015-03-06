namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.ShippingPages
{
    partial class UspsWizardPage
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
            this.panelConfigured = new System.Windows.Forms.Panel();
            this.labelConfigured = new System.Windows.Forms.Label();
            this.labelnfo1 = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.linkConfigure = new ShipWorks.UI.Controls.LinkControl();
            this.pictureBoxSuccess = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.labelNote2 = new System.Windows.Forms.Label();
            this.labelNote1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelConfigured.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxSuccess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelConfigured
            // 
            this.panelConfigured.Controls.Add(this.labelConfigured);
            this.panelConfigured.Controls.Add(this.pictureBoxSuccess);
            this.panelConfigured.Location = new System.Drawing.Point(167, 166);
            this.panelConfigured.Name = "panelConfigured";
            this.panelConfigured.Size = new System.Drawing.Size(170, 20);
            this.panelConfigured.TabIndex = 28;
            this.panelConfigured.Visible = false;
            // 
            // labelConfigured
            // 
            this.labelConfigured.AutoSize = true;
            this.labelConfigured.ForeColor = System.Drawing.Color.Green;
            this.labelConfigured.Location = new System.Drawing.Point(22, 3);
            this.labelConfigured.Name = "labelConfigured";
            this.labelConfigured.Size = new System.Drawing.Size(139, 13);
            this.labelConfigured.TabIndex = 1;
            this.labelConfigured.Text = "Configured for ShipWorks 3";
            // 
            // labelnfo1
            // 
            this.labelnfo1.Location = new System.Drawing.Point(166, 35);
            this.labelnfo1.Name = "labelnfo1";
            this.labelnfo1.Size = new System.Drawing.Size(319, 46);
            this.labelnfo1.TabIndex = 27;
            this.labelnfo1.Text = "ShipWorks 3 found some USPS shipments in your ShipWorks 2 database.  If you" +
                " are going to continue shipping using USPS, we recommend you configure tho" +
                "se settings now.";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelTitle.Location = new System.Drawing.Point(165, 14);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(128, 13);
            this.labelTitle.TabIndex = 26;
            this.labelTitle.Text = "USPS Shipping";
            // 
            // linkConfigure
            // 
            this.linkConfigure.AutoSize = true;
            this.linkConfigure.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkConfigure.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkConfigure.ForeColor = System.Drawing.Color.Blue;
            this.linkConfigure.Location = new System.Drawing.Point(167, 150);
            this.linkConfigure.Name = "linkConfigure";
            this.linkConfigure.Size = new System.Drawing.Size(161, 13);
            this.linkConfigure.TabIndex = 25;
            this.linkConfigure.Text = "Configure \'USPS\' shipping";
            this.linkConfigure.Click += new System.EventHandler(this.OnConfigureClick);
            // 
            // pictureBoxSuccess
            // 
            this.pictureBoxSuccess.Image = global::ShipWorks.Properties.Resources.check16;
            this.pictureBoxSuccess.Location = new System.Drawing.Point(3, 1);
            this.pictureBoxSuccess.Name = "pictureBoxSuccess";
            this.pictureBoxSuccess.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxSuccess.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxSuccess.TabIndex = 0;
            this.pictureBoxSuccess.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::ShipWorks.Properties.Resources.logo_sdc;
            this.pictureBox4.Location = new System.Drawing.Point(12, 14);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(147, 44);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 24;
            this.pictureBox4.TabStop = false;
            // 
            // labelNote2
            // 
            this.labelNote2.Location = new System.Drawing.Point(226, 86);
            this.labelNote2.Name = "labelNote2";
            this.labelNote2.Size = new System.Drawing.Size(255, 55);
            this.labelNote2.TabIndex = 39;
            this.labelNote2.Text = "ShipWorks 3 does not require the USPS software to be installed on your comp" +
                "uter.  ShipWorks will connect directly to your online USPS account for pro" +
                "cessing shipping labels.";
            // 
            // labelNote1
            // 
            this.labelNote1.AutoSize = true;
            this.labelNote1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelNote1.Location = new System.Drawing.Point(186, 86);
            this.labelNote1.Name = "labelNote1";
            this.labelNote1.Size = new System.Drawing.Size(33, 13);
            this.labelNote1.TabIndex = 38;
            this.labelNote1.Text = "Note";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.exclamation16;
            this.pictureBox1.Location = new System.Drawing.Point(167, 84);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 37;
            this.pictureBox1.TabStop = false;
            // 
            // UspsWizardPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelNote2);
            this.Controls.Add(this.labelNote1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panelConfigured);
            this.Controls.Add(this.labelnfo1);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.linkConfigure);
            this.Controls.Add(this.pictureBox4);
            this.Description = "ShipWorks 3 needs some information about your Stamps.com account.";
            this.Name = "UspsWizardPage";
            this.Size = new System.Drawing.Size(502, 230);
            this.Title = "Stamps.com Account Configuration";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.panelConfigured.ResumeLayout(false);
            this.panelConfigured.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxSuccess)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelConfigured;
        private System.Windows.Forms.Label labelConfigured;
        private System.Windows.Forms.PictureBox pictureBoxSuccess;
        private System.Windows.Forms.Label labelnfo1;
        private System.Windows.Forms.Label labelTitle;
        private UI.Controls.LinkControl linkConfigure;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label labelNote2;
        private System.Windows.Forms.Label labelNote1;
        private System.Windows.Forms.PictureBox pictureBox1;

    }
}
