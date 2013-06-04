namespace ShipWorks.Data.Controls
{
    partial class EntityPickerDlg
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.filterTree = new ShipWorks.Filters.Controls.FilterTree();
            this.gridControl = new ShipWorks.ApplicationCore.MainGridControl();
            this.panelStatus = new System.Windows.Forms.Panel();
            this.kryptonStatusPanel = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.labelSelected = new System.Windows.Forms.Label();
            this.labelTotal = new System.Windows.Forms.Label();
            this.labelStatusEtch = new System.Windows.Forms.Label();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.panelStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.kryptonStatusPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.kryptonStatusPanel.Panel)).BeginInit();
            this.kryptonStatusPanel.Panel.SuspendLayout();
            this.kryptonStatusPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(592, 501);
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
            this.cancel.Location = new System.Drawing.Point(673, 501);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(12, 12);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.filterTree);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.gridControl);
            this.splitContainer.Panel2.Controls.Add(this.panelStatus);
            this.splitContainer.Size = new System.Drawing.Size(736, 483);
            this.splitContainer.SplitterDistance = 150;
            this.splitContainer.TabIndex = 2;
            // 
            // filterTree
            // 
            this.filterTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterTree.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.filterTree.HotTrackNode = null;
            this.filterTree.Location = new System.Drawing.Point(0, 0);
            this.filterTree.Name = "filterTree";
            this.filterTree.Size = new System.Drawing.Size(150, 483);
            this.filterTree.TabIndex = 0;
            this.filterTree.SelectedFilterNodeChanged += new System.EventHandler(this.OnSelectedFilterChanged);
            // 
            // gridControl
            // 
            this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridControl.Location = new System.Drawing.Point(0, 0);
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(582, 463);
            this.gridControl.TabIndex = 0;
            this.gridControl.RowActivated += new Divelements.SandGrid.GridRowEventHandler(this.OnRowActivated);
            this.gridControl.SelectionChanged += new System.EventHandler(this.OnGridSelectionChanged);
            // 
            // panelStatus
            // 
            this.panelStatus.Controls.Add(this.kryptonStatusPanel);
            this.panelStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelStatus.Location = new System.Drawing.Point(0, 463);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(582, 20);
            this.panelStatus.TabIndex = 5;
            // 
            // kryptonStatusPanel
            // 
            this.kryptonStatusPanel.AutoSize = true;
            this.kryptonStatusPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonStatusPanel.GroupBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridHeaderColumnSheet;
            this.kryptonStatusPanel.GroupBorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.ButtonStandalone;
            this.kryptonStatusPanel.Location = new System.Drawing.Point(0, 0);
            this.kryptonStatusPanel.Name = "kryptonStatusPanel";
            // 
            // kryptonStatusPanel.Panel
            // 
            this.kryptonStatusPanel.Panel.Controls.Add(this.labelSelected);
            this.kryptonStatusPanel.Panel.Controls.Add(this.labelTotal);
            this.kryptonStatusPanel.Panel.Controls.Add(this.labelStatusEtch);
            this.kryptonStatusPanel.Size = new System.Drawing.Size(582, 20);
            this.kryptonStatusPanel.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders) (((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonStatusPanel.TabIndex = 1;
            // 
            // labelSelected
            // 
            this.labelSelected.AutoSize = true;
            this.labelSelected.BackColor = System.Drawing.Color.Transparent;
            this.labelSelected.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (64)))), ((int) (((byte) (64)))), ((int) (((byte) (64)))));
            this.labelSelected.Location = new System.Drawing.Point(80, 2);
            this.labelSelected.Name = "labelSelected";
            this.labelSelected.Size = new System.Drawing.Size(73, 13);
            this.labelSelected.TabIndex = 44;
            this.labelSelected.Text = "Selected: 345";
            // 
            // labelTotal
            // 
            this.labelTotal.AutoSize = true;
            this.labelTotal.BackColor = System.Drawing.Color.Transparent;
            this.labelTotal.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (64)))), ((int) (((byte) (64)))), ((int) (((byte) (64)))));
            this.labelTotal.Location = new System.Drawing.Point(4, 2);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(65, 13);
            this.labelTotal.TabIndex = 43;
            this.labelTotal.Text = "Orders: 345";
            // 
            // labelStatusEtch
            // 
            this.labelStatusEtch.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelStatusEtch.Location = new System.Drawing.Point(72, 3);
            this.labelStatusEtch.Name = "labelStatusEtch";
            this.labelStatusEtch.Size = new System.Drawing.Size(2, 14);
            this.labelStatusEtch.TabIndex = 42;
            this.labelStatusEtch.Text = "Etch";
            // 
            // EntityPickerDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(760, 536);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EntityPickerDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Orders";
            this.Load += new System.EventHandler(this.OnLoad);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.panelStatus.ResumeLayout(false);
            this.panelStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.kryptonStatusPanel.Panel)).EndInit();
            this.kryptonStatusPanel.Panel.ResumeLayout(false);
            this.kryptonStatusPanel.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.kryptonStatusPanel)).EndInit();
            this.kryptonStatusPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.SplitContainer splitContainer;
        private ShipWorks.Filters.Controls.FilterTree filterTree;
        private ShipWorks.ApplicationCore.MainGridControl gridControl;
        private System.Windows.Forms.Panel panelStatus;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup kryptonStatusPanel;
        private System.Windows.Forms.Label labelStatusEtch;
        private System.Windows.Forms.Label labelSelected;
        private System.Windows.Forms.Label labelTotal;
    }
}