namespace ShipWorks.Stores.Platforms.Amazon.WizardPages
{
    partial class AmazonCredentialsPage
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
            this.accountSettings = new ShipWorks.Stores.Platforms.Amazon.AmazonAccountSettingsControl();
            this.SuspendLayout();
            //
            // accountSettings
            //
            this.accountSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.accountSettings.Location = new System.Drawing.Point(4, 4);
            this.accountSettings.Name = "accountSettings";
            this.accountSettings.ShowExtendedPanel = false;
            this.accountSettings.Size = new System.Drawing.Size(469, 210);
            this.accountSettings.TabIndex = 0;
            //
            // CredentialsPage
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.accountSettings);
            this.Description = "Enter the information about your seller account.";
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "CredentialsPage";
            this.Size = new System.Drawing.Size(496, 300);
            this.Title = "Amazon Login";
            this.ResumeLayout(false);

        }

        #endregion

        private AmazonAccountSettingsControl accountSettings;

    }
}
