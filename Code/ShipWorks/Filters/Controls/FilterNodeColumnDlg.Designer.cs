namespace ShipWorks.Filters.Controls
{
    partial class FilterNodeColumnDlg
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
            this.labelDirections = new System.Windows.Forms.Label();
            this.labelFilter = new System.Windows.Forms.Label();
            this.filterComboBox = new ShipWorks.Filters.Controls.FilterComboBox();
            this.gridColumnLayoutEditor = new ShipWorks.Filters.Controls.FilterNodeColumnEditor();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(190, 451);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 4;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(271, 451);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 5;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // labelDirections
            // 
            this.labelDirections.AutoSize = true;
            this.labelDirections.Location = new System.Drawing.Point(21, 25);
            this.labelDirections.Name = "labelDirections";
            this.labelDirections.Size = new System.Drawing.Size(294, 13);
            this.labelDirections.TabIndex = 1;
            this.labelDirections.Text = "Configure the columns to display when this filter is selected:";
            // 
            // labelFilter
            // 
            this.labelFilter.AutoSize = true;
            this.labelFilter.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelFilter.Location = new System.Drawing.Point(12, 9);
            this.labelFilter.Name = "labelFilter";
            this.labelFilter.Size = new System.Drawing.Size(36, 13);
            this.labelFilter.TabIndex = 0;
            this.labelFilter.Text = "Filter";
            // 
            // filterComboBox
            // 
            this.filterComboBox.DropDownHeight = 350;
            this.filterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterComboBox.FormattingEnabled = true;
            this.filterComboBox.IntegralHeight = false;
            this.filterComboBox.Location = new System.Drawing.Point(24, 42);
            this.filterComboBox.Name = "filterComboBox";
            this.filterComboBox.Size = new System.Drawing.Size(259, 21);
            this.filterComboBox.TabIndex = 2;
            this.filterComboBox.SelectedFilterNodeChanged += new System.EventHandler(this.OnSelectedFilterNodeChanged);
            // 
            // gridColumnLayoutEditor
            // 
            this.gridColumnLayoutEditor.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.gridColumnLayoutEditor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnLayoutEditor.Location = new System.Drawing.Point(6, 70);
            this.gridColumnLayoutEditor.MinimumSize = new System.Drawing.Size(344, 340);
            this.gridColumnLayoutEditor.Name = "gridColumnLayoutEditor";
            this.gridColumnLayoutEditor.Size = new System.Drawing.Size(344, 375);
            this.gridColumnLayoutEditor.TabIndex = 3;
            // 
            // FilterNodeColumnDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(350, 486);
            this.Controls.Add(this.filterComboBox);
            this.Controls.Add(this.labelFilter);
            this.Controls.Add(this.labelDirections);
            this.Controls.Add(this.gridColumnLayoutEditor);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(366, 1080);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(366, 480);
            this.Name = "FilterNodeColumnDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Grid Columns";
            this.Load += new System.EventHandler(this.OnLoad);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private ShipWorks.Filters.Controls.FilterNodeColumnEditor gridColumnLayoutEditor;
        private System.Windows.Forms.Label labelDirections;
        private System.Windows.Forms.Label labelFilter;
        private ShipWorks.Filters.Controls.FilterComboBox filterComboBox;
    }
}