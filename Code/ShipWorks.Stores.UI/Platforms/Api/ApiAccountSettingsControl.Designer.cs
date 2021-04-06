﻿
namespace ShipWorks.Stores.UI.Platforms.Api
{
    partial class ApiAccountSettingsControl
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
            this.SettingsLabel = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // SettingsLabel
            // 
            this.SettingsLabel.BackColor = System.Drawing.SystemColors.Window;
            this.SettingsLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SettingsLabel.Location = new System.Drawing.Point(0, 0);
            this.SettingsLabel.Name = "SettingsLabel";
            this.SettingsLabel.ReadOnly = true;
            this.SettingsLabel.Size = new System.Drawing.Size(395, 31);
            this.SettingsLabel.TabIndex = 1;
            this.SettingsLabel.Text = "To update API store settings, go to https://hub.shipworks.com.";
            this.SettingsLabel.SelectionChanged += new System.EventHandler(this.OnSettingsLabelSelectionChanged);
            this.SettingsLabel.Click += new System.EventHandler(this.OnSettingsLabelClick);
            // 
            // ApiAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SettingsLabel);
            this.Name = "ApiAccountSettingsControl";
            this.Size = new System.Drawing.Size(476, 150);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox SettingsLabel;
    }
}
