namespace ShipWorks.Actions
{
    partial class ActionErrorDlg
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
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer1 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.close = new System.Windows.Forms.Button();
            this.panelGridArea = new System.Windows.Forms.Panel();
            this.entityGrid = new ShipWorks.Data.Grid.Paging.PagedEntityGrid();
            this.gridColumn1 = new Divelements.SandGrid.GridColumn();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.panelTools = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.labelGridSettings = new System.Windows.Forms.Label();
            this.showErrorMessages = new System.Windows.Forms.CheckBox();
            this.delete = new System.Windows.Forms.Button();
            this.retry = new System.Windows.Forms.Button();
            this.labelErrors = new System.Windows.Forms.Label();
            this.panelGridArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelTools)).BeginInit();
            this.panelTools.SuspendLayout();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.close.Location = new System.Drawing.Point(696, 407);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 5;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // panelGridArea
            // 
            this.panelGridArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelGridArea.BackColor = System.Drawing.Color.White;
            this.panelGridArea.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelGridArea.Controls.Add(this.entityGrid);
            this.panelGridArea.Controls.Add(this.kryptonBorderEdge);
            this.panelGridArea.Controls.Add(this.panelTools);
            this.panelGridArea.Location = new System.Drawing.Point(12, 12);
            this.panelGridArea.Name = "panelGridArea";
            this.panelGridArea.Size = new System.Drawing.Size(638, 383);
            this.panelGridArea.TabIndex = 0;
            // 
            // entityGrid
            // 
            this.entityGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.entityGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumn1});
            this.entityGrid.DetailViewSettings = null;
            this.entityGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entityGrid.EnableSearching = false;
            this.entityGrid.GridLines = Divelements.SandGrid.GridLinesDisplayType.Both;
            this.entityGrid.LiveResize = false;
            this.entityGrid.Location = new System.Drawing.Point(0, 0);
            this.entityGrid.Name = "entityGrid";
            this.entityGrid.NullRepresentation = "";
            this.entityGrid.Renderer = windowsXPRenderer1;
            this.entityGrid.RowHighlightType = Divelements.SandGrid.RowHighlightType.Full;
            this.entityGrid.ShadeAlternateRows = true;
            this.entityGrid.Size = new System.Drawing.Size(634, 352);
            this.entityGrid.StretchPrimaryGrid = false;
            this.entityGrid.TabIndex = 0;
            this.entityGrid.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnGridSelectionChanged);
            // 
            // gridColumn1
            // 
            this.gridColumn1.DataPropertyName = "Started";
            this.gridColumn1.HeaderText = "Started";
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(0, 352);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(634, 1);
            this.kryptonBorderEdge.TabIndex = 2;
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            // 
            // panelTools
            // 
            this.panelTools.Controls.Add(this.labelGridSettings);
            this.panelTools.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelTools.Location = new System.Drawing.Point(0, 353);
            this.panelTools.Name = "panelTools";
            this.panelTools.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridHeaderColumnSheet;
            this.panelTools.Size = new System.Drawing.Size(634, 26);
            this.panelTools.TabIndex = 1;
            // 
            // labelGridSettings
            // 
            this.labelGridSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelGridSettings.AutoSize = true;
            this.labelGridSettings.BackColor = System.Drawing.Color.Transparent;
            this.labelGridSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelGridSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGridSettings.ForeColor = System.Drawing.Color.Blue;
            this.labelGridSettings.Location = new System.Drawing.Point(561, 6);
            this.labelGridSettings.Name = "labelGridSettings";
            this.labelGridSettings.Size = new System.Drawing.Size(68, 13);
            this.labelGridSettings.TabIndex = 0;
            this.labelGridSettings.Text = "Grid Settings";
            this.labelGridSettings.Click += new System.EventHandler(this.OnGridSettings);
            // 
            // showErrorMessages
            // 
            this.showErrorMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.showErrorMessages.AutoSize = true;
            this.showErrorMessages.BackColor = System.Drawing.Color.Transparent;
            this.showErrorMessages.Location = new System.Drawing.Point(660, 91);
            this.showErrorMessages.Name = "showErrorMessages";
            this.showErrorMessages.Size = new System.Drawing.Size(81, 17);
            this.showErrorMessages.TabIndex = 4;
            this.showErrorMessages.Text = "Show detail";
            this.showErrorMessages.UseVisualStyleBackColor = false;
            this.showErrorMessages.CheckedChanged += new System.EventHandler(this.OnChangeShowErrorMessages);
            // 
            // delete
            // 
            this.delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.delete.Image = global::ShipWorks.Properties.Resources.delete16;
            this.delete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.delete.Location = new System.Drawing.Point(656, 41);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(115, 23);
            this.delete.TabIndex = 2;
            this.delete.Text = "Delete";
            this.delete.UseVisualStyleBackColor = true;
            this.delete.Click += new System.EventHandler(this.OnDelete);
            // 
            // retry
            // 
            this.retry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.retry.Image = global::ShipWorks.Properties.Resources.gear_run16;
            this.retry.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.retry.Location = new System.Drawing.Point(656, 12);
            this.retry.Name = "retry";
            this.retry.Size = new System.Drawing.Size(115, 23);
            this.retry.TabIndex = 1;
            this.retry.Text = "Retry";
            this.retry.UseVisualStyleBackColor = true;
            this.retry.Click += new System.EventHandler(this.OnRetry);
            // 
            // labelErrors
            // 
            this.labelErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelErrors.AutoSize = true;
            this.labelErrors.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelErrors.Location = new System.Drawing.Point(656, 74);
            this.labelErrors.Name = "labelErrors";
            this.labelErrors.Size = new System.Drawing.Size(94, 13);
            this.labelErrors.TabIndex = 3;
            this.labelErrors.Text = "Error Messages";
            // 
            // ActionErrorDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(783, 442);
            this.Controls.Add(this.showErrorMessages);
            this.Controls.Add(this.labelErrors);
            this.Controls.Add(this.retry);
            this.Controls.Add(this.delete);
            this.Controls.Add(this.panelGridArea);
            this.Controls.Add(this.close);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(584, 368);
            this.Name = "ActionErrorDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Action Errors";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnLoad);
            this.panelGridArea.ResumeLayout(false);
            this.panelGridArea.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelTools)).EndInit();
            this.panelTools.ResumeLayout(false);
            this.panelTools.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button close;
        private System.Windows.Forms.Panel panelGridArea;
        private ShipWorks.Data.Grid.Paging.PagedEntityGrid entityGrid;
        private Divelements.SandGrid.GridColumn gridColumn1;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel panelTools;
        private System.Windows.Forms.Label labelGridSettings;
        private System.Windows.Forms.Button delete;
        private System.Windows.Forms.Button retry;
        private System.Windows.Forms.CheckBox showErrorMessages;
        private System.Windows.Forms.Label labelErrors;
    }
}