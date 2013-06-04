namespace ShipWorks.ApplicationCore.Appearance
{
    partial class EnvironmentSaveDlg
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
            this.save = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.labelSettings = new System.Windows.Forms.Label();
            this.settingContextMenus = new System.Windows.Forms.CheckBox();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.settingRibbonPanels = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(119, 82);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(75, 23);
            this.save.TabIndex = 3;
            this.save.Text = "Save";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.OnSave);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(200, 82);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 4;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // labelSettings
            // 
            this.labelSettings.AutoSize = true;
            this.labelSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelSettings.Location = new System.Drawing.Point(12, 9);
            this.labelSettings.Name = "labelSettings";
            this.labelSettings.Size = new System.Drawing.Size(213, 13);
            this.labelSettings.TabIndex = 0;
            this.labelSettings.Text = "Which settings do you want to save?";
            // 
            // settingContextMenus
            // 
            this.settingContextMenus.AutoSize = true;
            this.settingContextMenus.Checked = true;
            this.settingContextMenus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.settingContextMenus.Location = new System.Drawing.Point(30, 50);
            this.settingContextMenus.Name = "settingContextMenus";
            this.settingContextMenus.Size = new System.Drawing.Size(121, 17);
            this.settingContextMenus.TabIndex = 2;
            this.settingContextMenus.Text = "Grid Context Menus";
            this.settingContextMenus.UseVisualStyleBackColor = true;
            this.settingContextMenus.CheckedChanged += new System.EventHandler(this.OnChangeSelectedSettings);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "swe";
            this.saveFileDialog.Filter = "ShipWorks Environment (*.swe)|*.swe";
            this.saveFileDialog.Title = "ShipWorks Environment Settings";
            // 
            // settingRibbonPanels
            // 
            this.settingRibbonPanels.AutoSize = true;
            this.settingRibbonPanels.Checked = true;
            this.settingRibbonPanels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.settingRibbonPanels.Location = new System.Drawing.Point(30, 29);
            this.settingRibbonPanels.Name = "settingRibbonPanels";
            this.settingRibbonPanels.Size = new System.Drawing.Size(114, 17);
            this.settingRibbonPanels.TabIndex = 1;
            this.settingRibbonPanels.Text = "Ribbon and Panels";
            this.settingRibbonPanels.UseVisualStyleBackColor = true;
            this.settingRibbonPanels.CheckedChanged += new System.EventHandler(this.OnChangeSelectedSettings);
            // 
            // EnvironmentSaveDlg
            // 
            this.AcceptButton = this.save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(287, 116);
            this.Controls.Add(this.settingRibbonPanels);
            this.Controls.Add(this.settingContextMenus);
            this.Controls.Add(this.labelSettings);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.save);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EnvironmentSaveDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Save Environment Settings";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label labelSettings;
        private System.Windows.Forms.CheckBox settingContextMenus;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.CheckBox settingRibbonPanels;
    }
}