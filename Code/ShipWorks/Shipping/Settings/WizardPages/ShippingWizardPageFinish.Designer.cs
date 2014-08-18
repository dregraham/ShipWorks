namespace ShipWorks.Shipping.Settings.WizardPages
{
    partial class ShippingWizardPageFinish
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
            this.labelSuccess = new System.Windows.Forms.Label();
            this.pictureSuccess = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize) (this.pictureSuccess)).BeginInit();
            this.SuspendLayout();
            // 
            // labelSuccess
            // 
            this.labelSuccess.AutoSize = true;
            this.labelSuccess.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelSuccess.Location = new System.Drawing.Point(45, 12);
            this.labelSuccess.Name = "labelSuccess";
            this.labelSuccess.Size = new System.Drawing.Size(235, 13);
            this.labelSuccess.TabIndex = 3;
            this.labelSuccess.Text = "ShipWorks is now set up to ship with {0}!";
            // 
            // pictureSuccess
            // 
            this.pictureSuccess.Image = global::ShipWorks.Properties.Resources.check16;
            this.pictureSuccess.Location = new System.Drawing.Point(26, 10);
            this.pictureSuccess.Name = "pictureSuccess";
            this.pictureSuccess.Size = new System.Drawing.Size(16, 16);
            this.pictureSuccess.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureSuccess.TabIndex = 2;
            this.pictureSuccess.TabStop = false;
            // 
            // ShippingWizardPageFinish
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelSuccess);
            this.Controls.Add(this.pictureSuccess);
            this.Description = "ShipWorks is now set up to ship with {0}.";
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "ShippingWizardPageFinish";
            this.Size = new System.Drawing.Size(486, 186);
            this.Title = "Setup Complete";
            ((System.ComponentModel.ISupportInitialize) (this.pictureSuccess)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSuccess;
        private System.Windows.Forms.PictureBox pictureSuccess;
    }
}
