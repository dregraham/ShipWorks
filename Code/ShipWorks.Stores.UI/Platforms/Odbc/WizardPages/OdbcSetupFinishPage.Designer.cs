namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages
{
    partial class OdbcSetupFinishPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OdbcSetupFinishPage));
            this.iconSetupComplete = new System.Windows.Forms.PictureBox();
            this.labelMessage = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.iconSetupComplete)).BeginInit();
            this.SuspendLayout();
            // 
            // iconSetupComplete
            // 
            this.iconSetupComplete.Image = ((System.Drawing.Image)(resources.GetObject("iconSetupComplete.Image")));
            this.iconSetupComplete.Location = new System.Drawing.Point(20, 10);
            this.iconSetupComplete.Name = "iconSetupComplete";
            this.iconSetupComplete.Size = new System.Drawing.Size(16, 16);
            this.iconSetupComplete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.iconSetupComplete.TabIndex = 12;
            this.iconSetupComplete.TabStop = false;
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Location = new System.Drawing.Point(42, 12);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(171, 13);
            this.labelMessage.TabIndex = 11;
            this.labelMessage.Text = "Your settings have been updated.";
            // 
            // OdbcSetupFinishPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.iconSetupComplete);
            this.Controls.Add(this.labelMessage);
            this.Description = "Your settings have been updated.";
            this.Name = "OdbcSetupFinishPage";
            this.Size = new System.Drawing.Size(204, 150);
            this.Title = "Store Settings Updated";
            ((System.ComponentModel.ISupportInitialize)(this.iconSetupComplete)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox iconSetupComplete;
        private System.Windows.Forms.Label labelMessage;
    }
}