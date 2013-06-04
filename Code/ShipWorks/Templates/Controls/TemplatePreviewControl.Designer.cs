namespace ShipWorks.Templates.Controls
{
    partial class TemplatePreviewControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelTools = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.panelPreviewControls = new System.Windows.Forms.Panel();
            this.previewSource = new System.Windows.Forms.Label();
            this.panelPreviewFilter = new System.Windows.Forms.Panel();
            this.previewFilter = new ShipWorks.Filters.Controls.FilterComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.labelPreviewWith = new System.Windows.Forms.Label();
            this.panelZoomControls = new System.Windows.Forms.Panel();
            this.zoomCombo = new System.Windows.Forms.ComboBox();
            this.zoomInToolbar = new System.Windows.Forms.ToolStrip();
            this.zoomin = new System.Windows.Forms.ToolStripButton();
            this.zoomOutToolbar = new System.Windows.Forms.ToolStrip();
            this.zoomout = new System.Windows.Forms.ToolStripButton();
            this.zoomBar = new ShipWorks.UI.Controls.TransparentTrackBar();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelMessage = new System.Windows.Forms.Label();
            this.backgroundPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize) (this.panelTools)).BeginInit();
            this.panelTools.SuspendLayout();
            this.panelPreviewControls.SuspendLayout();
            this.panelPreviewFilter.SuspendLayout();
            this.panelZoomControls.SuspendLayout();
            this.zoomInToolbar.SuspendLayout();
            this.zoomOutToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.zoomBar)).BeginInit();
            this.backgroundPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTools
            // 
            this.panelTools.Controls.Add(this.panelPreviewControls);
            this.panelTools.Controls.Add(this.panelZoomControls);
            this.panelTools.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelTools.Location = new System.Drawing.Point(0, 498);
            this.panelTools.Name = "panelTools";
            this.panelTools.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridHeaderColumnSheet;
            this.panelTools.Size = new System.Drawing.Size(632, 30);
            this.panelTools.TabIndex = 0;
            // 
            // panelPreviewControls
            // 
            this.panelPreviewControls.BackColor = System.Drawing.Color.Transparent;
            this.panelPreviewControls.Controls.Add(this.previewSource);
            this.panelPreviewControls.Controls.Add(this.panelPreviewFilter);
            this.panelPreviewControls.Controls.Add(this.labelPreviewWith);
            this.panelPreviewControls.Location = new System.Drawing.Point(1, 5);
            this.panelPreviewControls.Name = "panelPreviewControls";
            this.panelPreviewControls.Size = new System.Drawing.Size(321, 25);
            this.panelPreviewControls.TabIndex = 9;
            this.panelPreviewControls.SizeChanged += new System.EventHandler(this.OnPreviewPanelSizeChanged);
            this.panelPreviewControls.VisibleChanged += new System.EventHandler(this.OnPreviewPanelVisibleChanged);
            // 
            // previewSource
            // 
            this.previewSource.AutoSize = true;
            this.previewSource.BackColor = System.Drawing.Color.Transparent;
            this.previewSource.Cursor = System.Windows.Forms.Cursors.Hand;
            this.previewSource.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.previewSource.ForeColor = System.Drawing.Color.Blue;
            this.previewSource.Location = new System.Drawing.Point(77, 6);
            this.previewSource.Name = "previewSource";
            this.previewSource.Size = new System.Drawing.Size(53, 13);
            this.previewSource.TabIndex = 7;
            this.previewSource.Text = "10 orders";
            this.previewSource.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnPreviewSourceMouseDown);
            // 
            // panelPreviewFilter
            // 
            this.panelPreviewFilter.Controls.Add(this.previewFilter);
            this.panelPreviewFilter.Controls.Add(this.label3);
            this.panelPreviewFilter.Location = new System.Drawing.Point(129, 1);
            this.panelPreviewFilter.Name = "panelPreviewFilter";
            this.panelPreviewFilter.Size = new System.Drawing.Size(189, 25);
            this.panelPreviewFilter.TabIndex = 2;
            // 
            // previewFilter
            // 
            this.previewFilter.AllowQuickFilter = true;
            this.previewFilter.DropDownHeight = 350;
            this.previewFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.previewFilter.DropDownWidth = 270;
            this.previewFilter.FormattingEnabled = true;
            this.previewFilter.IntegralHeight = false;
            this.previewFilter.Location = new System.Drawing.Point(42, 2);
            this.previewFilter.Name = "previewFilter";
            this.previewFilter.Size = new System.Drawing.Size(140, 21);
            this.previewFilter.SizeToContent = true;
            this.previewFilter.TabIndex = 9;
            this.previewFilter.SelectedFilterNodeChanged += new System.EventHandler(this.OnPreviewFilterChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (64)))), ((int) (((byte) (64)))), ((int) (((byte) (64)))));
            this.label3.Location = new System.Drawing.Point(3, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "in filter";
            // 
            // labelPreviewWith
            // 
            this.labelPreviewWith.AutoSize = true;
            this.labelPreviewWith.BackColor = System.Drawing.Color.Transparent;
            this.labelPreviewWith.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (64)))), ((int) (((byte) (64)))), ((int) (((byte) (64)))));
            this.labelPreviewWith.Location = new System.Drawing.Point(2, 6);
            this.labelPreviewWith.Name = "labelPreviewWith";
            this.labelPreviewWith.Size = new System.Drawing.Size(77, 13);
            this.labelPreviewWith.TabIndex = 4;
            this.labelPreviewWith.Text = "Preview using:";
            // 
            // panelZoomControls
            // 
            this.panelZoomControls.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelZoomControls.BackColor = System.Drawing.Color.Transparent;
            this.panelZoomControls.Controls.Add(this.zoomCombo);
            this.panelZoomControls.Controls.Add(this.zoomInToolbar);
            this.panelZoomControls.Controls.Add(this.zoomOutToolbar);
            this.panelZoomControls.Controls.Add(this.zoomBar);
            this.panelZoomControls.Location = new System.Drawing.Point(435, 2);
            this.panelZoomControls.Name = "panelZoomControls";
            this.panelZoomControls.Size = new System.Drawing.Size(196, 28);
            this.panelZoomControls.TabIndex = 8;
            // 
            // zoomCombo
            // 
            this.zoomCombo.FormattingEnabled = true;
            this.zoomCombo.Items.AddRange(new object[] {
            "Fit Width",
            "Fit All",
            "400%",
            "200%",
            "100%",
            "50%"});
            this.zoomCombo.Location = new System.Drawing.Point(119, 3);
            this.zoomCombo.Name = "zoomCombo";
            this.zoomCombo.Size = new System.Drawing.Size(72, 21);
            this.zoomCombo.TabIndex = 3;
            this.zoomCombo.Text = "100%";
            this.zoomCombo.SelectedIndexChanged += new System.EventHandler(this.OnZoomSpecific);
            this.zoomCombo.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidatingZoomSpecific);
            // 
            // zoomInToolbar
            // 
            this.zoomInToolbar.BackColor = System.Drawing.Color.Transparent;
            this.zoomInToolbar.CanOverflow = false;
            this.zoomInToolbar.Dock = System.Windows.Forms.DockStyle.None;
            this.zoomInToolbar.GripMargin = new System.Windows.Forms.Padding(0);
            this.zoomInToolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.zoomInToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomin});
            this.zoomInToolbar.Location = new System.Drawing.Point(94, 2);
            this.zoomInToolbar.Name = "zoomInToolbar";
            this.zoomInToolbar.Padding = new System.Windows.Forms.Padding(0);
            this.zoomInToolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.zoomInToolbar.Size = new System.Drawing.Size(25, 25);
            this.zoomInToolbar.Stretch = true;
            this.zoomInToolbar.TabIndex = 1;
            // 
            // zoomin
            // 
            this.zoomin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.zoomin.Image = global::ShipWorks.Properties.Resources.zoom_in;
            this.zoomin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zoomin.Name = "zoomin";
            this.zoomin.Size = new System.Drawing.Size(23, 22);
            this.zoomin.Text = "Zoom In";
            this.zoomin.Click += new System.EventHandler(this.OnZoomButton);
            // 
            // zoomOutToolbar
            // 
            this.zoomOutToolbar.BackColor = System.Drawing.Color.Transparent;
            this.zoomOutToolbar.CanOverflow = false;
            this.zoomOutToolbar.Dock = System.Windows.Forms.DockStyle.None;
            this.zoomOutToolbar.GripMargin = new System.Windows.Forms.Padding(0);
            this.zoomOutToolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.zoomOutToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomout});
            this.zoomOutToolbar.Location = new System.Drawing.Point(3, 2);
            this.zoomOutToolbar.Name = "zoomOutToolbar";
            this.zoomOutToolbar.Padding = new System.Windows.Forms.Padding(0);
            this.zoomOutToolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.zoomOutToolbar.Size = new System.Drawing.Size(25, 25);
            this.zoomOutToolbar.Stretch = true;
            this.zoomOutToolbar.TabIndex = 2;
            // 
            // zoomout
            // 
            this.zoomout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.zoomout.Image = global::ShipWorks.Properties.Resources.zoom_out;
            this.zoomout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zoomout.Name = "zoomout";
            this.zoomout.Size = new System.Drawing.Size(23, 22);
            this.zoomout.Text = "Zoom Out";
            this.zoomout.Click += new System.EventHandler(this.OnZoomButton);
            // 
            // zoomBar
            // 
            this.zoomBar.AutoSize = false;
            this.zoomBar.LargeChange = 20;
            this.zoomBar.Location = new System.Drawing.Point(22, 2);
            this.zoomBar.Maximum = 800;
            this.zoomBar.Minimum = 20;
            this.zoomBar.Name = "zoomBar";
            this.zoomBar.Size = new System.Drawing.Size(78, 27);
            this.zoomBar.SmallChange = 5;
            this.zoomBar.TabIndex = 0;
            this.zoomBar.TickFrequency = 5;
            this.zoomBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.zoomBar.Value = 20;
            this.zoomBar.Scroll += new System.EventHandler(this.OnZoomSlider);
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(0, 497);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(632, 1);
            this.kryptonBorderEdge.TabIndex = 1;
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            // 
            // labelMessage
            // 
            this.labelMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMessage.ForeColor = System.Drawing.Color.DimGray;
            this.labelMessage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelMessage.Location = new System.Drawing.Point(0, 0);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(632, 497);
            this.labelMessage.TabIndex = 0;
            this.labelMessage.Text = "No template selected.";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // backgroundPanel
            // 
            this.backgroundPanel.BackColor = System.Drawing.Color.White;
            this.backgroundPanel.Controls.Add(this.labelMessage);
            this.backgroundPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backgroundPanel.Location = new System.Drawing.Point(0, 0);
            this.backgroundPanel.Name = "backgroundPanel";
            this.backgroundPanel.Size = new System.Drawing.Size(632, 497);
            this.backgroundPanel.TabIndex = 2;
            // 
            // TemplatePreviewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.backgroundPanel);
            this.Controls.Add(this.kryptonBorderEdge);
            this.Controls.Add(this.panelTools);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "TemplatePreviewControl";
            this.Size = new System.Drawing.Size(632, 528);
            this.SizeChanged += new System.EventHandler(this.OnSizeChanged);
            ((System.ComponentModel.ISupportInitialize) (this.panelTools)).EndInit();
            this.panelTools.ResumeLayout(false);
            this.panelPreviewControls.ResumeLayout(false);
            this.panelPreviewControls.PerformLayout();
            this.panelPreviewFilter.ResumeLayout(false);
            this.panelPreviewFilter.PerformLayout();
            this.panelZoomControls.ResumeLayout(false);
            this.panelZoomControls.PerformLayout();
            this.zoomInToolbar.ResumeLayout(false);
            this.zoomInToolbar.PerformLayout();
            this.zoomOutToolbar.ResumeLayout(false);
            this.zoomOutToolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.zoomBar)).EndInit();
            this.backgroundPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonPanel panelTools;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        private ShipWorks.UI.Controls.TransparentTrackBar zoomBar;
        private System.Windows.Forms.ToolStrip zoomOutToolbar;
        private System.Windows.Forms.ToolStripButton zoomout;
        private System.Windows.Forms.ToolStrip zoomInToolbar;
        private System.Windows.Forms.ToolStripButton zoomin;
        private System.Windows.Forms.ComboBox zoomCombo;
        private System.Windows.Forms.Label labelPreviewWith;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Panel backgroundPanel;
        private System.Windows.Forms.Panel panelZoomControls;
        private System.Windows.Forms.Panel panelPreviewControls;
        private System.Windows.Forms.Panel panelPreviewFilter;
        private System.Windows.Forms.Label previewSource;
        private ShipWorks.Filters.Controls.FilterComboBox previewFilter;

    }
}
