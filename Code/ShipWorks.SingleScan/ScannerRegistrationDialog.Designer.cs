using System.Windows.Forms;

namespace ShipWorks.SingleScan
{
    partial class ScannerRegistrationDialog
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
            registerScannerHost?.Dispose();
            base.Dispose(disposing);

        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.registerScannerHost = new System.Windows.Forms.Integration.ElementHost();
            this.registrationRegistrationControl = new ShipWorks.SingleScan.ScannerRegistrationControl();
            this.StartPosition = FormStartPosition.CenterParent;
            this.SuspendLayout();
            //
            // registerScannerHost
            //
            this.registerScannerHost.AutoSize = true;
            this.registerScannerHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.registerScannerHost.Location = new System.Drawing.Point(0, 0);
            this.registerScannerHost.Name = "registerScannerHost";
            this.registerScannerHost.Size = new System.Drawing.Size(501, 154);
            this.registerScannerHost.TabIndex = 0;
            this.registerScannerHost.Text = "elementHost1";
            this.registerScannerHost.Child = this.registrationRegistrationControl;
            //
            // RegisterScannerDialog
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(501, 154);
            this.ControlBox = false;
            this.Controls.Add(this.registerScannerHost);
            this.Name = "RegisterScannerDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Register bar code scanner for Single Scan";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost registerScannerHost;
        private ScannerRegistrationControl registrationRegistrationControl;
    }
}