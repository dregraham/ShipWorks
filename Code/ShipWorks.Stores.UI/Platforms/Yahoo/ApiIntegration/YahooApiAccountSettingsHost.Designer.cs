﻿namespace ShipWorks.Stores.UI.Platforms.Yahoo.ApiIntegration
{
    partial class YahooApiAccountSettingsHost
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
            this.ControlHost = new System.Windows.Forms.Integration.ElementHost();
            this.SuspendLayout();
            // 
            // ControlHost
            // 
            this.ControlHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ControlHost.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.ControlHost.Location = new System.Drawing.Point(0, 0);
            this.ControlHost.Name = "ControlHost";
            this.ControlHost.Size = new System.Drawing.Size(150, 150);
            this.ControlHost.TabIndex = 0;
            this.ControlHost.Child = null;
            // 
            // YahooApiAccountSettingsHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ControlHost);
            this.Name = "YahooApiAccountSettingsHost";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost ControlHost;
    }
}
