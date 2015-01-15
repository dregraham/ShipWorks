namespace ShipWorks.Filters.Management
{
    partial class FilterEditorDlg
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterEditorDlg));
            this.labelFilterName = new System.Windows.Forms.Label();
            this.filterName = new System.Windows.Forms.TextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageCondition = new System.Windows.Forms.TabPage();
            this.conditionControl = new ShipWorks.Filters.Controls.FilterConditionControl();
            this.tabPageGridColumns = new System.Windows.Forms.TabPage();
            this.columnSettingList = new ShipWorks.UI.Controls.MenuList();
            this.panelDefaultColumns = new System.Windows.Forms.Panel();
            this.infotipDefaultColumns = new ShipWorks.UI.Controls.InfoTip();
            this.label2 = new System.Windows.Forms.Label();
            this.copyFromMySettings = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.gridColumnsDefault = new ShipWorks.Filters.Controls.FilterNodeColumnEditor();
            this.panelPersonalColumns = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.resetToDefualt = new System.Windows.Forms.Button();
            this.gridColumnsPersonal = new ShipWorks.Filters.Controls.FilterNodeColumnEditor();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.appliesTo = new System.Windows.Forms.Label();
            this.labelAppliesTo = new System.Windows.Forms.Label();
            this.appliesToImage = new System.Windows.Forms.PictureBox();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.enabled = new System.Windows.Forms.CheckBox();
            this.labelEnabled = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tabPageCondition.SuspendLayout();
            this.tabPageGridColumns.SuspendLayout();
            this.panelDefaultColumns.SuspendLayout();
            this.panelPersonalColumns.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.appliesToImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // labelFilterName
            // 
            this.labelFilterName.Location = new System.Drawing.Point(1, 15);
            this.labelFilterName.Name = "labelFilterName";
            this.labelFilterName.Size = new System.Drawing.Size(75, 13);
            this.labelFilterName.TabIndex = 0;
            this.labelFilterName.Text = "Filter Name:";
            this.labelFilterName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // filterName
            // 
            this.filterName.Location = new System.Drawing.Point(86, 12);
            this.fieldLengthProvider.SetMaxLengthSource(this.filterName, ShipWorks.Data.Utility.EntityFieldLengthSource.FilterName);
            this.filterName.Name = "filterName";
            this.filterName.Size = new System.Drawing.Size(252, 21);
            this.filterName.TabIndex = 1;
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageCondition);
            this.tabControl.Controls.Add(this.tabPageGridColumns);
            this.tabControl.Location = new System.Drawing.Point(15, 85);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(501, 440);
            this.tabControl.TabIndex = 4;
            // 
            // tabPageCondition
            // 
            this.tabPageCondition.Controls.Add(this.conditionControl);
            this.tabPageCondition.Location = new System.Drawing.Point(4, 22);
            this.tabPageCondition.Name = "tabPageCondition";
            this.tabPageCondition.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCondition.Size = new System.Drawing.Size(493, 414);
            this.tabPageCondition.TabIndex = 0;
            this.tabPageCondition.Text = "Condition";
            this.tabPageCondition.UseVisualStyleBackColor = true;
            // 
            // conditionControl
            // 
            this.conditionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conditionControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.conditionControl.Location = new System.Drawing.Point(3, 3);
            this.conditionControl.Name = "conditionControl";
            this.conditionControl.Size = new System.Drawing.Size(487, 408);
            this.conditionControl.TabIndex = 0;
            // 
            // tabPageGridColumns
            // 
            this.tabPageGridColumns.Controls.Add(this.columnSettingList);
            this.tabPageGridColumns.Controls.Add(this.panelDefaultColumns);
            this.tabPageGridColumns.Controls.Add(this.panelPersonalColumns);
            this.tabPageGridColumns.Location = new System.Drawing.Point(4, 22);
            this.tabPageGridColumns.Name = "tabPageGridColumns";
            this.tabPageGridColumns.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGridColumns.Size = new System.Drawing.Size(493, 414);
            this.tabPageGridColumns.TabIndex = 1;
            this.tabPageGridColumns.Text = "Grid Columns";
            this.tabPageGridColumns.UseVisualStyleBackColor = true;
            // 
            // columnSettingList
            // 
            this.columnSettingList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.columnSettingList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.columnSettingList.FormattingEnabled = true;
            this.columnSettingList.IntegralHeight = false;
            this.columnSettingList.ItemHeight = 26;
            this.columnSettingList.Items.AddRange(new object[] {
            "My Settings",
            "Default"});
            this.columnSettingList.Location = new System.Drawing.Point(8, 6);
            this.columnSettingList.Name = "columnSettingList";
            this.columnSettingList.Size = new System.Drawing.Size(104, 361);
            this.columnSettingList.TabIndex = 1;
            this.columnSettingList.SelectedIndexChanged += new System.EventHandler(this.OnChangeGridColumnType);
            // 
            // panelDefaultColumns
            // 
            this.panelDefaultColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDefaultColumns.Controls.Add(this.infotipDefaultColumns);
            this.panelDefaultColumns.Controls.Add(this.label2);
            this.panelDefaultColumns.Controls.Add(this.copyFromMySettings);
            this.panelDefaultColumns.Controls.Add(this.label1);
            this.panelDefaultColumns.Controls.Add(this.gridColumnsDefault);
            this.panelDefaultColumns.Location = new System.Drawing.Point(115, 6);
            this.panelDefaultColumns.Name = "panelDefaultColumns";
            this.panelDefaultColumns.Size = new System.Drawing.Size(359, 361);
            this.panelDefaultColumns.TabIndex = 3;
            this.panelDefaultColumns.Visible = false;
            // 
            // infotipDefaultColumns
            // 
            this.infotipDefaultColumns.Caption = "Changing the default columns will not affect the columns of users who have alread" +
    "y gotten a copy of the defaults.";
            this.infotipDefaultColumns.Location = new System.Drawing.Point(236, 28);
            this.infotipDefaultColumns.Name = "infotipDefaultColumns";
            this.infotipDefaultColumns.Size = new System.Drawing.Size(12, 12);
            this.infotipDefaultColumns.TabIndex = 23;
            this.infotipDefaultColumns.Title = "Default Columns";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(2, 319);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Use My Settings";
            // 
            // copyFromMySettings
            // 
            this.copyFromMySettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.copyFromMySettings.Location = new System.Drawing.Point(3, 334);
            this.copyFromMySettings.Name = "copyFromMySettings";
            this.copyFromMySettings.Size = new System.Drawing.Size(142, 23);
            this.copyFromMySettings.TabIndex = 21;
            this.copyFromMySettings.Text = "Copy from My Settings";
            this.copyFromMySettings.UseVisualStyleBackColor = true;
            this.copyFromMySettings.Click += new System.EventHandler(this.OnCopyColumnsFromMySettings);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(373, 48);
            this.label1.TabIndex = 5;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // gridColumnsDefault
            // 
            this.gridColumnsDefault.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gridColumnsDefault.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumnsDefault.Location = new System.Drawing.Point(-3, 45);
            this.gridColumnsDefault.MinimumSize = new System.Drawing.Size(344, 200);
            this.gridColumnsDefault.Name = "gridColumnsDefault";
            this.gridColumnsDefault.Size = new System.Drawing.Size(384, 274);
            this.gridColumnsDefault.TabIndex = 2;
            // 
            // panelPersonalColumns
            // 
            this.panelPersonalColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPersonalColumns.Controls.Add(this.label3);
            this.panelPersonalColumns.Controls.Add(this.resetToDefualt);
            this.panelPersonalColumns.Controls.Add(this.gridColumnsPersonal);
            this.panelPersonalColumns.Location = new System.Drawing.Point(116, 6);
            this.panelPersonalColumns.Name = "panelPersonalColumns";
            this.panelPersonalColumns.Size = new System.Drawing.Size(356, 361);
            this.panelPersonalColumns.TabIndex = 4;
            this.panelPersonalColumns.Visible = false;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1, 319);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Reset";
            // 
            // resetToDefualt
            // 
            this.resetToDefualt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.resetToDefualt.Location = new System.Drawing.Point(2, 334);
            this.resetToDefualt.Name = "resetToDefualt";
            this.resetToDefualt.Size = new System.Drawing.Size(142, 23);
            this.resetToDefualt.TabIndex = 23;
            this.resetToDefualt.Text = "Reset to Default";
            this.resetToDefualt.UseVisualStyleBackColor = true;
            this.resetToDefualt.Click += new System.EventHandler(this.OnResetMySettingsToDefault);
            // 
            // gridColumnsPersonal
            // 
            this.gridColumnsPersonal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gridColumnsPersonal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumnsPersonal.Location = new System.Drawing.Point(-4, 0);
            this.gridColumnsPersonal.MinimumSize = new System.Drawing.Size(344, 283);
            this.gridColumnsPersonal.Name = "gridColumnsPersonal";
            this.gridColumnsPersonal.Size = new System.Drawing.Size(344, 320);
            this.gridColumnsPersonal.TabIndex = 0;
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(360, 529);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 4;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(441, 529);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 5;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // appliesTo
            // 
            this.appliesTo.AutoSize = true;
            this.appliesTo.Location = new System.Drawing.Point(107, 64);
            this.appliesTo.Name = "appliesTo";
            this.appliesTo.Size = new System.Drawing.Size(40, 13);
            this.appliesTo.TabIndex = 3;
            this.appliesTo.Text = "Orders";
            // 
            // labelAppliesTo
            // 
            this.labelAppliesTo.Location = new System.Drawing.Point(16, 64);
            this.labelAppliesTo.Name = "labelAppliesTo";
            this.labelAppliesTo.Size = new System.Drawing.Size(60, 13);
            this.labelAppliesTo.TabIndex = 2;
            this.labelAppliesTo.Text = "Applies To:";
            this.labelAppliesTo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // appliesToImage
            // 
            this.appliesToImage.Image = global::ShipWorks.Properties.Resources.order16;
            this.appliesToImage.Location = new System.Drawing.Point(86, 63);
            this.appliesToImage.Name = "appliesToImage";
            this.appliesToImage.Size = new System.Drawing.Size(16, 16);
            this.appliesToImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.appliesToImage.TabIndex = 6;
            this.appliesToImage.TabStop = false;
            // 
            // enabled
            // 
            this.enabled.AutoSize = true;
            this.enabled.Location = new System.Drawing.Point(86, 40);
            this.enabled.Name = "enabled";
            this.enabled.Size = new System.Drawing.Size(15, 14);
            this.enabled.TabIndex = 7;
            this.enabled.UseVisualStyleBackColor = true;
            // 
            // labelEnabled
            // 
            this.labelEnabled.AutoSize = true;
            this.labelEnabled.Location = new System.Drawing.Point(27, 40);
            this.labelEnabled.Name = "labelEnabled";
            this.labelEnabled.Size = new System.Drawing.Size(49, 13);
            this.labelEnabled.TabIndex = 8;
            this.labelEnabled.Text = "Enabled:";
            // 
            // FilterEditorDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(528, 564);
            this.Controls.Add(this.labelEnabled);
            this.Controls.Add(this.enabled);
            this.Controls.Add(this.labelAppliesTo);
            this.Controls.Add(this.appliesTo);
            this.Controls.Add(this.appliesToImage);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.filterName);
            this.Controls.Add(this.labelFilterName);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(536, 540);
            this.Name = "FilterEditorDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Filter Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnLoad);
            this.tabControl.ResumeLayout(false);
            this.tabPageCondition.ResumeLayout(false);
            this.tabPageGridColumns.ResumeLayout(false);
            this.panelDefaultColumns.ResumeLayout(false);
            this.panelDefaultColumns.PerformLayout();
            this.panelPersonalColumns.ResumeLayout(false);
            this.panelPersonalColumns.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.appliesToImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelFilterName;
        private System.Windows.Forms.TextBox filterName;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageCondition;
        private System.Windows.Forms.TabPage tabPageGridColumns;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.PictureBox appliesToImage;
        private System.Windows.Forms.Label appliesTo;
        private System.Windows.Forms.Label labelAppliesTo;
        private ShipWorks.Filters.Controls.FilterNodeColumnEditor gridColumnsPersonal;
        private ShipWorks.UI.Controls.MenuList columnSettingList;
        private ShipWorks.Filters.Controls.FilterNodeColumnEditor gridColumnsDefault;
        private ShipWorks.Filters.Controls.FilterConditionControl conditionControl;
        private System.Windows.Forms.Panel panelDefaultColumns;
        private System.Windows.Forms.Panel panelPersonalColumns;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button copyFromMySettings;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button resetToDefualt;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private UI.Controls.InfoTip infotipDefaultColumns;
        private System.Windows.Forms.CheckBox enabled;
        private System.Windows.Forms.Label labelEnabled;
    }
}