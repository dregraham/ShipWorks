namespace ShipWorks.Stores.Platforms.PayPal
{
    partial class PayPalAccountSettingsControl
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
            this.credentials = new ShipWorks.Stores.Platforms.PayPal.PayPalCredentialsControl();
            this.SuspendLayout();
            // 
            // credentials
            // 
            this.credentials.Location = new System.Drawing.Point(3, 3);
            this.credentials.MinimumSize = new System.Drawing.Size(427, 142);
            this.credentials.Name = "credentials";
            this.credentials.Size = new System.Drawing.Size(485, 142);
            this.credentials.TabIndex = 0;
            // 
            // PayPalAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.credentials);
            this.Name = "PayPalAccountSettingsControl";
            this.Size = new System.Drawing.Size(509, 162);
            this.ResumeLayout(false);

        }

        #endregion

        private PayPalCredentialsControl credentials;
    }
}
