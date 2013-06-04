namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.ShippingPages
{
    partial class WorldShipWizardPage
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
            this.pictureBoxSuccess = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.linkConfigure = new ShipWorks.UI.Controls.LinkControl();
            this.panelConfigured.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxSuccess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // panelConfigured
            // 
            this.panelConfigured.Controls.Add(this.labelConfigured);
            this.panelConfigured.Controls.Add(this.pictureBoxSuccess);
            this.panelConfigured.Location = new System.Drawing.Point(80, 99);
            this.panelConfigured.Name = "panelConfigured";
            this.panelConfigured.Size = new System.Drawing.Size(170, 20);
            this.panelConfigured.TabIndex = 23;
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
            this.labelnfo1.Location = new System.Drawing.Point(79, 30);
            this.labelnfo1.Name = "labelnfo1";
            this.labelnfo1.Size = new System.Drawing.Size(396, 46);
            this.labelnfo1.TabIndex = 22;
            this.labelnfo1.Text = "ShipWorks 3 found some UPS WorldShip shipments in your ShipWorks 2 database.  If " +
                "you are going to continue shipping using WorldShip, we recommend you configure t" +
                "hose settings now.";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelTitle.Location = new System.Drawing.Point(78, 9);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(115, 13);
            this.labelTitle.TabIndex = 21;
            this.labelTitle.Text = "WorldShip Shipping";
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
            this.pictureBox4.Image = global::ShipWorks.Properties.Resources.glo_ups_brandmark_pfv1;
            this.pictureBox4.Location = new System.Drawing.Point(24, 9);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(42, 55);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 13;
            this.pictureBox4.TabStop = false;
            // 
            // linkConfigure
            // 
            this.linkConfigure.AutoSize = true;
            this.linkConfigure.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkConfigure.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkConfigure.ForeColor = System.Drawing.Color.Blue;
            this.linkConfigure.Location = new System.Drawing.Point(80, 83);
            this.linkConfigure.Name = "linkConfigure";
            this.linkConfigure.Size = new System.Drawing.Size(151, 13);
            this.linkConfigure.TabIndex = 20;
            this.linkConfigure.Text = "Configure \'WorldShip\' shipping";
            this.linkConfigure.Click += new System.EventHandler(this.OnConfigureClick);
            // 
            // WorldShipWizardPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelConfigured);
            this.Controls.Add(this.labelnfo1);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.linkConfigure);
            this.Controls.Add(this.pictureBox4);
            this.Description = "Setup ShipWorks for shipping with UPS WorldShip.";
            this.Name = "WorldShipWizardPage";
            this.Size = new System.Drawing.Size(501, 245);
            this.Title = "Configure WorldShip Shipping";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.panelConfigured.ResumeLayout(false);
            this.panelConfigured.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxSuccess)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox4;
        private UI.Controls.LinkControl linkConfigure;
        private System.Windows.Forms.Label labelnfo1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelConfigured;
        private System.Windows.Forms.Label labelConfigured;
        private System.Windows.Forms.PictureBox pictureBoxSuccess;
    }
}
