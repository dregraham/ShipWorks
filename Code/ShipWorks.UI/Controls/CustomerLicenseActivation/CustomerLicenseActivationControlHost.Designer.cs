namespace ShipWorks.UI.Controls.CustomerLicenseActivation
{
    partial class CustomerLicenseActivationControlHost
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
            this.elementHost = new System.Windows.Forms.Integration.ElementHost();
            this.tangoUserControl = new CustomerLicenseActivationControl();
            this.SuspendLayout();
            // 
            // elementHost
            // 
            this.elementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost.Location = new System.Drawing.Point(0, 0);
            this.elementHost.Name = "elementHost";
            this.elementHost.Size = new System.Drawing.Size(1856, 1034);
            this.elementHost.TabIndex = 1;
            this.elementHost.Text = "elementHost2";
            this.elementHost.Child = this.tangoUserControl;
            // 
            // TangoUserControlHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.elementHost);
            this.Description = "Create a user account to log on to ShipWorks.";
            this.Name = "TangoUserControlHost";
            this.Size = new System.Drawing.Size(1856, 1034);
            this.Title = "ShipWorks Account";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost elementHost;
        private CustomerLicenseActivationControl tangoUserControl;
    }
}
