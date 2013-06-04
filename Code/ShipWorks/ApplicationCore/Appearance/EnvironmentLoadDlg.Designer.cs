namespace ShipWorks.ApplicationCore.Appearance
{
    partial class EnvironmentLoadDlg
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
            this.load = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.labelSettings = new System.Windows.Forms.Label();
            this.settingContextMenus = new System.Windows.Forms.CheckBox();
            this.settingRibbonPanels = new System.Windows.Forms.CheckBox();
            this.settingsFile = new ShipWorks.UI.Controls.PathTextBox();
            this.browse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.panelSettings = new System.Windows.Forms.Panel();
            this.panelSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // load
            // 
            this.load.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.load.Location = new System.Drawing.Point(312, 150);
            this.load.Name = "load";
            this.load.Size = new System.Drawing.Size(75, 23);
            this.load.TabIndex = 4;
            this.load.Text = "Load";
            this.load.UseVisualStyleBackColor = true;
            this.load.Click += new System.EventHandler(this.OnSave);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(393, 150);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 5;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // labelSettings
            // 
            this.labelSettings.AutoSize = true;
            this.labelSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelSettings.Location = new System.Drawing.Point(0, 4);
            this.labelSettings.Name = "labelSettings";
            this.labelSettings.Size = new System.Drawing.Size(210, 13);
            this.labelSettings.TabIndex = 0;
            this.labelSettings.Text = "Which settings do you want to load?";
            // 
            // settingContextMenus
            // 
            this.settingContextMenus.AutoSize = true;
            this.settingContextMenus.Checked = true;
            this.settingContextMenus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.settingContextMenus.Location = new System.Drawing.Point(24, 43);
            this.settingContextMenus.Name = "settingContextMenus";
            this.settingContextMenus.Size = new System.Drawing.Size(121, 17);
            this.settingContextMenus.TabIndex = 2;
            this.settingContextMenus.Text = "Grid Context Menus";
            this.settingContextMenus.UseVisualStyleBackColor = true;
            this.settingContextMenus.CheckedChanged += new System.EventHandler(this.OnChangeSelectedSettings);
            // 
            // settingRibbonPanels
            // 
            this.settingRibbonPanels.AutoSize = true;
            this.settingRibbonPanels.Checked = true;
            this.settingRibbonPanels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.settingRibbonPanels.Location = new System.Drawing.Point(24, 22);
            this.settingRibbonPanels.Name = "settingRibbonPanels";
            this.settingRibbonPanels.Size = new System.Drawing.Size(114, 17);
            this.settingRibbonPanels.TabIndex = 1;
            this.settingRibbonPanels.Text = "Ribbon and Panels";
            this.settingRibbonPanels.UseVisualStyleBackColor = true;
            this.settingRibbonPanels.CheckedChanged += new System.EventHandler(this.OnChangeSelectedSettings);
            // 
            // settingsFile
            // 
            this.settingsFile.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsFile.Location = new System.Drawing.Point(12, 25);
            this.settingsFile.Name = "settingsFile";
            this.settingsFile.ReadOnly = true;
            this.settingsFile.Size = new System.Drawing.Size(456, 21);
            this.settingsFile.TabIndex = 1;
            // 
            // browse
            // 
            this.browse.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browse.Location = new System.Drawing.Point(393, 50);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(75, 23);
            this.browse.TabIndex = 2;
            this.browse.Text = "Browse...";
            this.browse.Click += new System.EventHandler(this.OnBrowse);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Environment settings file:";
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "swe";
            this.openFileDialog.Filter = "ShipWorks Environment (*.swe)|*.swe";
            this.openFileDialog.Title = "ShipWorks Environment Settings";
            // 
            // panelSettings
            // 
            this.panelSettings.Controls.Add(this.labelSettings);
            this.panelSettings.Controls.Add(this.settingContextMenus);
            this.panelSettings.Controls.Add(this.settingRibbonPanels);
            this.panelSettings.Location = new System.Drawing.Point(12, 74);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Size = new System.Drawing.Size(462, 65);
            this.panelSettings.TabIndex = 3;
            // 
            // EnvironmentLoadDlg
            // 
            this.AcceptButton = this.load;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(480, 185);
            this.Controls.Add(this.panelSettings);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.settingsFile);
            this.Controls.Add(this.browse);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.load);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EnvironmentLoadDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Load Environment Settings";
            this.Load += new System.EventHandler(this.OnLoad);
            this.panelSettings.ResumeLayout(false);
            this.panelSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button load;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label labelSettings;
        private System.Windows.Forms.CheckBox settingContextMenus;
        private System.Windows.Forms.CheckBox settingRibbonPanels;
        private ShipWorks.UI.Controls.PathTextBox settingsFile;
        private System.Windows.Forms.Button browse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Panel panelSettings;
    }
}