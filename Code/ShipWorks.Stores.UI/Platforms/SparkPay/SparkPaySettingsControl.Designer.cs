namespace ShipWorks.Stores.UI.Platforms.SparkPay
{
    partial class SparkPaySettingsControl
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
            this.accountHost = new System.Windows.Forms.Integration.ElementHost();
            this.sparkPayAccountControl = new ShipWorks.Stores.UI.Platforms.SparkPay.WizardPages.SparkPayAccountControl();
            this.SuspendLayout();
            // 
            // accountHost
            // 
            this.accountHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accountHost.Location = new System.Drawing.Point(0, 0);
            this.accountHost.Name = "accountHost";
            this.accountHost.Size = new System.Drawing.Size(487, 100);
            this.accountHost.TabIndex = 0;
            this.accountHost.Text = "elementHost1";
            this.accountHost.Child = this.sparkPayAccountControl;
            // 
            // SparkPaySettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.accountHost);
            this.Name = "SparkPaySettingsControl";
            this.Size = new System.Drawing.Size(487, 100);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost accountHost;
        private WizardPages.SparkPayAccountControl sparkPayAccountControl;
    }
}
