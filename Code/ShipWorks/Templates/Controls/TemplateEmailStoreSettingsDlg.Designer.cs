namespace ShipWorks.Templates.Controls
{
    partial class TemplateEmailStoreSettingsDlg
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
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.radioUseShared = new System.Windows.Forms.RadioButton();
            this.radioUseUnique = new System.Windows.Forms.RadioButton();
            this.sharedSettings = new ShipWorks.Templates.Emailing.EmailTemplateStoreSettingsControl();
            this.uniqueSettings = new ShipWorks.Templates.Emailing.EmailTemplateStoreSettingsControl();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(248, 343);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 0;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(329, 343);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // radioUseShared
            // 
            this.radioUseShared.AutoSize = true;
            this.radioUseShared.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.radioUseShared.Location = new System.Drawing.Point(11, 12);
            this.radioUseShared.Name = "radioUseShared";
            this.radioUseShared.Size = new System.Drawing.Size(137, 17);
            this.radioUseShared.TabIndex = 12;
            this.radioUseShared.TabStop = true;
            this.radioUseShared.Text = "Use shared settings";
            this.radioUseShared.UseVisualStyleBackColor = true;
            this.radioUseShared.CheckedChanged += new System.EventHandler(this.OnCheckChanged);
            // 
            // radioUseUnique
            // 
            this.radioUseUnique.AutoSize = true;
            this.radioUseUnique.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.radioUseUnique.Location = new System.Drawing.Point(11, 176);
            this.radioUseUnique.Name = "radioUseUnique";
            this.radioUseUnique.Size = new System.Drawing.Size(212, 17);
            this.radioUseUnique.TabIndex = 13;
            this.radioUseUnique.TabStop = true;
            this.radioUseUnique.Text = "Use unique settings for this store";
            this.radioUseUnique.UseVisualStyleBackColor = true;
            this.radioUseUnique.CheckedChanged += new System.EventHandler(this.OnCheckChanged);
            // 
            // sharedSettings
            // 
            this.sharedSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.sharedSettings.Location = new System.Drawing.Point(31, 34);
            this.sharedSettings.Name = "sharedSettings";
            this.sharedSettings.Size = new System.Drawing.Size(371, 135);
            this.sharedSettings.TabIndex = 9;
            // 
            // uniqueSettings
            // 
            this.uniqueSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.uniqueSettings.Location = new System.Drawing.Point(31, 199);
            this.uniqueSettings.Name = "uniqueSettings";
            this.uniqueSettings.Size = new System.Drawing.Size(371, 135);
            this.uniqueSettings.TabIndex = 14;
            // 
            // TemplateEmailStoreSettingsDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(416, 378);
            this.Controls.Add(this.radioUseShared);
            this.Controls.Add(this.sharedSettings);
            this.Controls.Add(this.uniqueSettings);
            this.Controls.Add(this.radioUseUnique);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TemplateEmailStoreSettingsDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Email Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.RadioButton radioUseShared;
        private Emailing.EmailTemplateStoreSettingsControl sharedSettings;
        private Emailing.EmailTemplateStoreSettingsControl uniqueSettings;
        private System.Windows.Forms.RadioButton radioUseUnique;
    }
}