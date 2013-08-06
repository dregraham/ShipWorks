namespace ShipWorks.ApplicationCore
{
    partial class SplashScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashScreen));
            this.status = new System.Windows.Forms.Label();
            this.copyright = new System.Windows.Forms.Label();
            this.website = new System.Windows.Forms.Label();
            this.labelReleaseInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.BackColor = System.Drawing.Color.Transparent;
            this.status.ForeColor = System.Drawing.Color.Gray;
            this.status.Location = new System.Drawing.Point(12, 267);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(178, 13);
            this.status.TabIndex = 0;
            this.status.Text = "Loading Interapptive.Framework.dll";
            // 
            // copyright
            // 
            this.copyright.AutoSize = true;
            this.copyright.BackColor = System.Drawing.Color.Transparent;
            this.copyright.ForeColor = System.Drawing.Color.DarkGray;
            this.copyright.Location = new System.Drawing.Point(12, 349);
            this.copyright.Name = "copyright";
            this.copyright.Size = new System.Drawing.Size(328, 13);
            this.copyright.TabIndex = 4;
            this.copyright.Text = "© Copyright 2002-2007, Interapptive®, Inc.  All Rights Reserved.";
            // 
            // website
            // 
            this.website.AutoSize = true;
            this.website.BackColor = System.Drawing.Color.Transparent;
            this.website.ForeColor = System.Drawing.Color.DarkGray;
            this.website.Location = new System.Drawing.Point(464, 349);
            this.website.Name = "website";
            this.website.Size = new System.Drawing.Size(116, 13);
            this.website.TabIndex = 3;
            this.website.Text = "www.interapptive.com";
            // 
            // labelReleaseInfo
            // 
            this.labelReleaseInfo.AutoSize = true;
            this.labelReleaseInfo.BackColor = System.Drawing.Color.Transparent;
            this.labelReleaseInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelReleaseInfo.ForeColor = System.Drawing.Color.Black;
            this.labelReleaseInfo.Location = new System.Drawing.Point(13, 14);
            this.labelReleaseInfo.Name = "labelReleaseInfo";
            this.labelReleaseInfo.Size = new System.Drawing.Size(128, 13);
            this.labelReleaseInfo.TabIndex = 5;
            this.labelReleaseInfo.Text = "Optional Release Info";
            // 
            // SplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image) (resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(480, 289);
            this.Controls.Add(this.labelReleaseInfo);
            this.Controls.Add(this.copyright);
            this.Controls.Add(this.website);
            this.Controls.Add(this.status);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.Name = "SplashScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShipWorks";
            this.Load += new System.EventHandler(this.OnLoad);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label status;
        private System.Windows.Forms.Label copyright;
        private System.Windows.Forms.Label website;
        private System.Windows.Forms.Label labelReleaseInfo;
    }
}