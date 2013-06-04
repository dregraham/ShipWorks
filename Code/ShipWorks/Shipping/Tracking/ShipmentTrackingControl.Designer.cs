namespace ShipWorks.Shipping.Tracking
{
    partial class ShipmentTrackingControl
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
            this.components = new System.ComponentModel.Container();
            this.copy = new System.Windows.Forms.Button();
            this.themeBorderPanel = new ShipWorks.UI.Controls.ThemeBorderPanel();
            this.htmlControl = new ShipWorks.UI.Controls.Html.HtmlControl();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themeBorderPanel.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // copy
            // 
            this.copy.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.copy.Location = new System.Drawing.Point(385, 252);
            this.copy.Name = "copy";
            this.copy.Size = new System.Drawing.Size(75, 23);
            this.copy.TabIndex = 236;
            this.copy.Text = "Copy";
            this.copy.UseVisualStyleBackColor = true;
            this.copy.Click += new System.EventHandler(this.OnCopy);
            // 
            // themeBorderPanel
            // 
            this.themeBorderPanel.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.themeBorderPanel.AutoScroll = true;
            this.themeBorderPanel.Controls.Add(this.htmlControl);
            this.themeBorderPanel.Location = new System.Drawing.Point(0, 0);
            this.themeBorderPanel.Name = "themeBorderPanel";
            this.themeBorderPanel.Size = new System.Drawing.Size(470, 248);
            this.themeBorderPanel.TabIndex = 235;
            // 
            // htmlControl
            // 
            this.htmlControl.AllowActiveContent = false;
            this.htmlControl.AllowContextMenu = false;
            this.htmlControl.AllowNavigation = false;
            this.htmlControl.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.htmlControl.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.htmlControl.ContextMenuStrip = this.contextMenu;
            this.htmlControl.Html = "";
            this.htmlControl.Location = new System.Drawing.Point(0, 0);
            this.htmlControl.Name = "htmlControl";
            this.htmlControl.OpenLinksInNewWindow = true;
            this.htmlControl.SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.htmlControl.SelectionBackColor = System.Drawing.Color.Empty;
            this.htmlControl.SelectionBold = false;
            this.htmlControl.SelectionBullets = false;
            this.htmlControl.SelectionFont = null;
            this.htmlControl.SelectionFontName = "Times New Roman";
            this.htmlControl.SelectionFontSize = 2;
            this.htmlControl.SelectionForeColor = System.Drawing.Color.Empty;
            this.htmlControl.SelectionItalic = false;
            this.htmlControl.SelectionNumbering = false;
            this.htmlControl.SelectionUnderline = false;
            this.htmlControl.ShowBorderGuides = true;
            this.htmlControl.ShowGlyphs = false;
            this.htmlControl.Size = new System.Drawing.Size(470, 88);
            this.htmlControl.TabIndex = 234;
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(103, 26);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = global::ShipWorks.Properties.Resources.copy;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.OnCopy);
            // 
            // ShipmentTrackingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.copy);
            this.Controls.Add(this.themeBorderPanel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "ShipmentTrackingControl";
            this.Size = new System.Drawing.Size(470, 280);
            this.Resize += new System.EventHandler(this.OnResize);
            this.themeBorderPanel.ResumeLayout(false);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Controls.Html.HtmlControl htmlControl;
        private ShipWorks.UI.Controls.ThemeBorderPanel themeBorderPanel;
        private System.Windows.Forms.Button copy;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
    }
}
