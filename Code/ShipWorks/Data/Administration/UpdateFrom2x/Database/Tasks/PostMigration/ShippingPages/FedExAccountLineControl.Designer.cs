namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.ShippingPages
{
    partial class FedExAccountLineControl
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
            this.labelAccount = new System.Windows.Forms.Label();
            this.linkConfigure = new ShipWorks.UI.Controls.LinkControl();
            this.panelConfigured = new System.Windows.Forms.Panel();
            this.labelConfigured = new System.Windows.Forms.Label();
            this.pictureBoxSuccess = new System.Windows.Forms.PictureBox();
            this.panelConfigured.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxSuccess)).BeginInit();
            this.SuspendLayout();
            // 
            // labelAccount
            // 
            this.labelAccount.AutoSize = true;
            this.labelAccount.Location = new System.Drawing.Point(3, 4);
            this.labelAccount.Name = "labelAccount";
            this.labelAccount.Size = new System.Drawing.Size(103, 13);
            this.labelAccount.TabIndex = 15;
            this.labelAccount.Text = "Account#: 6546893";
            // 
            // linkConfigure
            // 
            this.linkConfigure.AutoSize = true;
            this.linkConfigure.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkConfigure.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkConfigure.ForeColor = System.Drawing.Color.Blue;
            this.linkConfigure.Location = new System.Drawing.Point(114, 3);
            this.linkConfigure.Name = "linkConfigure";
            this.linkConfigure.Size = new System.Drawing.Size(54, 13);
            this.linkConfigure.TabIndex = 16;
            this.linkConfigure.Text = "Configure";
            this.linkConfigure.Click += new System.EventHandler(this.OnConfigure);
            // 
            // panelConfigured
            // 
            this.panelConfigured.Controls.Add(this.labelConfigured);
            this.panelConfigured.Controls.Add(this.pictureBoxSuccess);
            this.panelConfigured.Location = new System.Drawing.Point(173, 0);
            this.panelConfigured.Name = "panelConfigured";
            this.panelConfigured.Size = new System.Drawing.Size(170, 20);
            this.panelConfigured.TabIndex = 17;
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
            // FedExAccountLineControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelConfigured);
            this.Controls.Add(this.linkConfigure);
            this.Controls.Add(this.labelAccount);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "FedExAccountLineControl";
            this.Size = new System.Drawing.Size(351, 20);
            this.Load += new System.EventHandler(this.OnLoad);
            this.panelConfigured.ResumeLayout(false);
            this.panelConfigured.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxSuccess)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Controls.LinkControl linkConfigure;
        private System.Windows.Forms.Label labelAccount;
        private System.Windows.Forms.Panel panelConfigured;
        private System.Windows.Forms.Label labelConfigured;
        private System.Windows.Forms.PictureBox pictureBoxSuccess;
    }
}
