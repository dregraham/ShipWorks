namespace ShipWorks.Data.Grid.Columns
{
    partial class GridColumnSettingsDlg
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
            this.labelDataView = new System.Windows.Forms.Label();
            this.labelTemplate = new System.Windows.Forms.Label();
            this.groupBoxDetailsView = new System.Windows.Forms.GroupBox();
            this.detailViewTemplate = new ShipWorks.Templates.Controls.TemplateComboBox();
            this.detailViewHeight = new System.Windows.Forms.ComboBox();
            this.labelHeight = new System.Windows.Forms.Label();
            this.detailViewMode = new System.Windows.Forms.ComboBox();
            this.labelInitialSort = new System.Windows.Forms.Label();
            this.panelDetailView = new System.Windows.Forms.Panel();
            this.labelInitialSortInfo = new System.Windows.Forms.Label();
            this.initialSort = new System.Windows.Forms.ComboBox();
            this.gridColumnEditor = new ShipWorks.Data.Grid.Columns.GridColumnLayoutEditor();
            this.groupBoxDetailsView.SuspendLayout();
            this.panelDetailView.SuspendLayout();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(180, 458);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 5;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(261, 458);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 6;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // labelDataView
            // 
            this.labelDataView.AutoSize = true;
            this.labelDataView.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelDataView.Location = new System.Drawing.Point(3, 3);
            this.labelDataView.Name = "labelDataView";
            this.labelDataView.Size = new System.Drawing.Size(63, 13);
            this.labelDataView.TabIndex = 0;
            this.labelDataView.Text = "Data View";
            // 
            // labelTemplate
            // 
            this.labelTemplate.AutoSize = true;
            this.labelTemplate.Location = new System.Drawing.Point(10, 21);
            this.labelTemplate.Name = "labelTemplate";
            this.labelTemplate.Size = new System.Drawing.Size(55, 13);
            this.labelTemplate.TabIndex = 0;
            this.labelTemplate.Text = "Template:";
            // 
            // groupBoxDetailsView
            // 
            this.groupBoxDetailsView.Controls.Add(this.detailViewTemplate);
            this.groupBoxDetailsView.Controls.Add(this.detailViewHeight);
            this.groupBoxDetailsView.Controls.Add(this.labelHeight);
            this.groupBoxDetailsView.Controls.Add(this.labelTemplate);
            this.groupBoxDetailsView.Location = new System.Drawing.Point(18, 49);
            this.groupBoxDetailsView.Name = "groupBoxDetailsView";
            this.groupBoxDetailsView.Size = new System.Drawing.Size(311, 79);
            this.groupBoxDetailsView.TabIndex = 2;
            this.groupBoxDetailsView.TabStop = false;
            this.groupBoxDetailsView.Text = "Details View";
            // 
            // detailViewTemplate
            // 
            this.detailViewTemplate.DropDownHeight = 300;
            this.detailViewTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.detailViewTemplate.FormattingEnabled = true;
            this.detailViewTemplate.IntegralHeight = false;
            this.detailViewTemplate.Location = new System.Drawing.Point(71, 18);
            this.detailViewTemplate.Name = "detailViewTemplate";
            this.detailViewTemplate.Size = new System.Drawing.Size(234, 21);
            this.detailViewTemplate.TabIndex = 1;
            // 
            // detailViewHeight
            // 
            this.detailViewHeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.detailViewHeight.FormattingEnabled = true;
            this.detailViewHeight.Location = new System.Drawing.Point(71, 46);
            this.detailViewHeight.Name = "detailViewHeight";
            this.detailViewHeight.Size = new System.Drawing.Size(76, 21);
            this.detailViewHeight.TabIndex = 3;
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Location = new System.Drawing.Point(23, 49);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(42, 13);
            this.labelHeight.TabIndex = 2;
            this.labelHeight.Text = "Height:";
            // 
            // detailViewMode
            // 
            this.detailViewMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.detailViewMode.FormattingEnabled = true;
            this.detailViewMode.Location = new System.Drawing.Point(6, 20);
            this.detailViewMode.Name = "detailViewMode";
            this.detailViewMode.Size = new System.Drawing.Size(143, 21);
            this.detailViewMode.TabIndex = 1;
            this.detailViewMode.SelectedIndexChanged += new System.EventHandler(this.OnChangeDetailViewMode);
            // 
            // labelInitialSort
            // 
            this.labelInitialSort.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelInitialSort.AutoSize = true;
            this.labelInitialSort.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelInitialSort.Location = new System.Drawing.Point(9, 406);
            this.labelInitialSort.Name = "labelInitialSort";
            this.labelInitialSort.Size = new System.Drawing.Size(67, 13);
            this.labelInitialSort.TabIndex = 2;
            this.labelInitialSort.Text = "Initial Sort";
            // 
            // panelDetailView
            // 
            this.panelDetailView.Controls.Add(this.labelDataView);
            this.panelDetailView.Controls.Add(this.groupBoxDetailsView);
            this.panelDetailView.Controls.Add(this.detailViewMode);
            this.panelDetailView.Location = new System.Drawing.Point(5, 5);
            this.panelDetailView.Name = "panelDetailView";
            this.panelDetailView.Size = new System.Drawing.Size(339, 141);
            this.panelDetailView.TabIndex = 0;
            // 
            // labelInitialSortInfo
            // 
            this.labelInitialSortInfo.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelInitialSortInfo.AutoSize = true;
            this.labelInitialSortInfo.Location = new System.Drawing.Point(24, 424);
            this.labelInitialSortInfo.Name = "labelInitialSortInfo";
            this.labelInitialSortInfo.Size = new System.Drawing.Size(183, 13);
            this.labelInitialSortInfo.TabIndex = 3;
            this.labelInitialSortInfo.Text = "When the grid first loads, sort using:";
            // 
            // initialSort
            // 
            this.initialSort.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.initialSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.initialSort.FormattingEnabled = true;
            this.initialSort.Items.AddRange(new object[] {
            "The default sort",
            "The sort I last used"});
            this.initialSort.Location = new System.Drawing.Point(213, 421);
            this.initialSort.Name = "initialSort";
            this.initialSort.Size = new System.Drawing.Size(121, 21);
            this.initialSort.TabIndex = 4;
            // 
            // gridColumnEditor
            // 
            this.gridColumnEditor.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.gridColumnEditor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnEditor.Location = new System.Drawing.Point(4, 147);
            this.gridColumnEditor.MinimumSize = new System.Drawing.Size(344, 200);
            this.gridColumnEditor.Name = "gridColumnEditor";
            this.gridColumnEditor.Size = new System.Drawing.Size(352, 250);
            this.gridColumnEditor.TabIndex = 1;
            // 
            // GridColumnSettingsDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(340, 493);
            this.Controls.Add(this.initialSort);
            this.Controls.Add(this.labelInitialSortInfo);
            this.Controls.Add(this.panelDetailView);
            this.Controls.Add(this.labelInitialSort);
            this.Controls.Add(this.gridColumnEditor);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(356, 2425);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(356, 512);
            this.Name = "GridColumnSettingsDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Grid Settings";
            this.Load += new System.EventHandler(this.OnLoad);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.groupBoxDetailsView.ResumeLayout(false);
            this.groupBoxDetailsView.PerformLayout();
            this.panelDetailView.ResumeLayout(false);
            this.panelDetailView.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private ShipWorks.Data.Grid.Columns.GridColumnLayoutEditor gridColumnEditor;
        private System.Windows.Forms.Label labelDataView;
        private System.Windows.Forms.Label labelTemplate;
        private System.Windows.Forms.GroupBox groupBoxDetailsView;
        private System.Windows.Forms.ComboBox detailViewHeight;
        private System.Windows.Forms.Label labelHeight;
        private ShipWorks.Templates.Controls.TemplateComboBox detailViewTemplate;
        private System.Windows.Forms.ComboBox detailViewMode;
        private System.Windows.Forms.Label labelInitialSort;
        private System.Windows.Forms.Panel panelDetailView;
        private System.Windows.Forms.Label labelInitialSortInfo;
        private System.Windows.Forms.ComboBox initialSort;
    }
}