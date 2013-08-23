namespace ShipWorks.ApplicationCore.Services.UI
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
            System.Windows.Forms.Label labelHeader1;
            System.Windows.Forms.Label labelHeader2;
            System.Windows.Forms.Label labelHeader3;
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer1 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.panelGridArea = new System.Windows.Forms.Panel();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.panelTools = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.startingServicePanel = new System.Windows.Forms.Panel();
            this.startingServiceImage = new System.Windows.Forms.PictureBox();
            this.startingServiceLabel = new System.Windows.Forms.Label();
            this.entityGrid = new ShipWorks.Data.Grid.Paging.PagedEntityGrid();
            closeButton = new System.Windows.Forms.Button();
            labelHeader1 = new System.Windows.Forms.Label();
            labelHeader2 = new System.Windows.Forms.Label();
            labelHeader3 = new System.Windows.Forms.Label();
            this.panelGridArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelTools)).BeginInit();
            this.startingServicePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startingServiceImage)).BeginInit();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            closeButton.Location = new System.Drawing.Point(640, 344);
            closeButton.Name = "closeButton";
            closeButton.Size = new System.Drawing.Size(75, 23);
            closeButton.TabIndex = 6;
            closeButton.Text = "Close";
            closeButton.UseVisualStyleBackColor = true;
            // 
            // labelHeader1
            // 
            labelHeader1.AutoSize = true;
            labelHeader1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            labelHeader1.Location = new System.Drawing.Point(12, 14);
            labelHeader1.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            labelHeader1.Name = "labelHeader1";
            labelHeader1.Size = new System.Drawing.Size(211, 13);
            labelHeader1.TabIndex = 8;
            labelHeader1.Text = "Required services that are not running are";
            // 
            // labelHeader2
            // 
            labelHeader2.AutoSize = true;
            labelHeader2.BackColor = System.Drawing.Color.Pink;
            labelHeader2.Location = new System.Drawing.Point(219, 14);
            labelHeader2.Margin = new System.Windows.Forms.Padding(0);
            labelHeader2.Name = "labelHeader2";
            labelHeader2.Size = new System.Drawing.Size(59, 13);
            labelHeader2.TabIndex = 9;
            labelHeader2.Text = "highlighted";
            // 
            // labelHeader3
            // 
            labelHeader3.AutoSize = true;
            labelHeader3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            labelHeader3.Location = new System.Drawing.Point(275, 14);
            labelHeader3.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            labelHeader3.Name = "labelHeader3";
            labelHeader3.Size = new System.Drawing.Size(316, 13);
            labelHeader3.TabIndex = 10;
            labelHeader3.Text = ".  These services must be started for ShipWorks to run properly.";
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
            this.panelGridArea.Location = new System.Drawing.Point(12, 42);
            this.panelGridArea.Name = "panelGridArea";
            this.panelGridArea.Size = new System.Drawing.Size(703, 290);
            this.panelGridArea.TabIndex = 7;
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(0, 259);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(699, 1);
            this.kryptonBorderEdge.TabIndex = 2;
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            // 
            // panelTools
            // 
            this.panelTools.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelTools.Location = new System.Drawing.Point(0, 260);
            this.panelTools.Name = "panelTools";
            this.panelTools.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridHeaderColumnSheet;
            this.panelTools.Size = new System.Drawing.Size(699, 26);
            this.panelTools.TabIndex = 1;
            // 
            // startingServicePanel
            // 
            this.startingServicePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.startingServicePanel.Controls.Add(this.startingServiceImage);
            this.startingServicePanel.Controls.Add(this.startingServiceLabel);
            this.startingServicePanel.Location = new System.Drawing.Point(3, 336);
            this.startingServicePanel.Name = "startingServicePanel";
            this.startingServicePanel.Size = new System.Drawing.Size(220, 39);
            this.startingServicePanel.TabIndex = 11;
            // 
            // startingServiceImage
            // 
            this.startingServiceImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startingServiceImage.Cursor = System.Windows.Forms.Cursors.Default;
            this.startingServiceImage.Image = global::ShipWorks.Properties.Resources.squares_circle_green;
            this.startingServiceImage.Location = new System.Drawing.Point(12, 4);
            this.startingServiceImage.Name = "startingServiceImage";
            this.startingServiceImage.Size = new System.Drawing.Size(31, 31);
            this.startingServiceImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.startingServiceImage.TabIndex = 6;
            this.startingServiceImage.TabStop = false;
            // 
            // startingServiceLabel
            // 
            this.startingServiceLabel.AutoSize = true;
            this.startingServiceLabel.Location = new System.Drawing.Point(46, 12);
            this.startingServiceLabel.Name = "startingServiceLabel";
            this.startingServiceLabel.Size = new System.Drawing.Size(155, 13);
            this.startingServiceLabel.TabIndex = 0;
            this.startingServiceLabel.Text = "Starting service, please wait...";
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
            this.entityGrid.Size = new System.Drawing.Size(699, 259);
            this.entityGrid.StretchPrimaryGrid = false;
            this.entityGrid.TabIndex = 0;
            // 
            // ServiceStatusDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = closeButton;
            this.ClientSize = new System.Drawing.Size(727, 379);
            this.Controls.Add(this.startingServicePanel);
            this.Controls.Add(labelHeader3);
            this.Controls.Add(labelHeader2);
            this.Controls.Add(labelHeader1);
            this.Controls.Add(this.panelGridArea);
            this.Controls.Add(closeButton);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "ServiceStatusDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Service Status Monitor";
            this.panelGridArea.ResumeLayout(false);
            this.panelGridArea.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelTools)).EndInit();
            this.startingServicePanel.ResumeLayout(false);
            this.startingServicePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startingServiceImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelGridArea;
        private Data.Grid.Paging.PagedEntityGrid entityGrid;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel panelTools;
        private System.Windows.Forms.Panel startingServicePanel;
        private System.Windows.Forms.Label startingServiceLabel;
        private System.Windows.Forms.PictureBox startingServiceImage;
    }
}