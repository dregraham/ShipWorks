namespace ShipWorks.ApplicationCore.WindowsServices
{
    partial class ServiceStatusDialog
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
            System.Windows.Forms.Button closeButton;
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer1 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.panelGridArea = new System.Windows.Forms.Panel();
            this.entityGrid = new ShipWorks.Data.Grid.Paging.PagedEntityGrid();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.panelTools = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.labelGridSettings = new System.Windows.Forms.Label();
            closeButton = new System.Windows.Forms.Button();
            this.panelGridArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelTools)).BeginInit();
            this.panelTools.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            closeButton.Location = new System.Drawing.Point(464, 316);
            closeButton.Name = "closeButton";
            closeButton.Size = new System.Drawing.Size(75, 23);
            closeButton.TabIndex = 6;
            closeButton.Text = "Close";
            closeButton.UseVisualStyleBackColor = true;
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
            this.panelGridArea.Size = new System.Drawing.Size(527, 292);
            this.panelGridArea.TabIndex = 7;
            // 
            // entityGrid
            // 
            this.entityGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
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
            this.entityGrid.Size = new System.Drawing.Size(523, 261);
            this.entityGrid.StretchPrimaryGrid = false;
            this.entityGrid.TabIndex = 0;
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(0, 261);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(523, 1);
            this.kryptonBorderEdge.TabIndex = 2;
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            // 
            // panelTools
            // 
            this.panelTools.Controls.Add(this.labelGridSettings);
            this.panelTools.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelTools.Location = new System.Drawing.Point(0, 262);
            this.panelTools.Name = "panelTools";
            this.panelTools.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridHeaderColumnSheet;
            this.panelTools.Size = new System.Drawing.Size(523, 26);
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
            this.labelGridSettings.Location = new System.Drawing.Point(450, 6);
            this.labelGridSettings.Name = "labelGridSettings";
            this.labelGridSettings.Size = new System.Drawing.Size(68, 13);
            this.labelGridSettings.TabIndex = 0;
            this.labelGridSettings.Text = "Grid Settings";
            this.labelGridSettings.Click += new System.EventHandler(this.OnEditGridSettings);
            // 
            // ServiceStatusDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = closeButton;
            this.ClientSize = new System.Drawing.Size(551, 351);
            this.Controls.Add(this.panelGridArea);
            this.Controls.Add(closeButton);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ServiceStatusDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Service Status";
            this.panelGridArea.ResumeLayout(false);
            this.panelGridArea.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelTools)).EndInit();
            this.panelTools.ResumeLayout(false);
            this.panelTools.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelGridArea;
        private Data.Grid.Paging.PagedEntityGrid entityGrid;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel panelTools;
        private System.Windows.Forms.Label labelGridSettings;
    }
}