namespace ShipWorks.Templates.Tokens
{
    partial class TemplateTokenTextBox
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
            this.tokenTextBox = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.toeknOptionsDropdown = new ComponentFactory.Krypton.Toolkit.ButtonSpecAny();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToken = new ComponentFactory.Krypton.Toolkit.ButtonSpecAny();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tokenTextBox
            // 
            this.tokenTextBox.AllowButtonSpecToolTips = true;
            this.tokenTextBox.ButtonSpecs.AddRange(new ComponentFactory.Krypton.Toolkit.ButtonSpecAny[] {
            this.toeknOptionsDropdown,
            this.editToken});
            this.tokenTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tokenTextBox.Location = new System.Drawing.Point(0, 0);
            this.tokenTextBox.Name = "tokenTextBox";
            this.tokenTextBox.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.tokenTextBox.Size = new System.Drawing.Size(210, 21);
            this.tokenTextBox.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.tokenTextBox.StateCommon.Content.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tokenTextBox.TabIndex = 221;
            // 
            // toeknOptionsDropdown
            // 
            this.toeknOptionsDropdown.ContextMenuStrip = this.contextMenuStrip;
            this.toeknOptionsDropdown.ExtraText = "";
            this.toeknOptionsDropdown.Image = null;
            this.toeknOptionsDropdown.Style = ComponentFactory.Krypton.Toolkit.PaletteButtonStyle.Standalone;
            this.toeknOptionsDropdown.Text = "";
            this.toeknOptionsDropdown.ToolTipBody = "Inserts a token into the token text box.  If text is selected, \r\nthe selection wi" +
    "ll be replaced with the inserted token.";
            this.toeknOptionsDropdown.ToolTipStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.SuperTip;
            this.toeknOptionsDropdown.ToolTipTitle = "Insert Token";
            this.toeknOptionsDropdown.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.DropDown;
            this.toeknOptionsDropdown.UniqueName = "D87DA930376E4CAAD87DA930376E4CAA";
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(181, 26);
            // 
            // editToken
            // 
            this.editToken.ExtraText = "";
            this.editToken.Image = global::ShipWorks.Properties.Resources.pencil2_small;
            this.editToken.Style = ComponentFactory.Krypton.Toolkit.PaletteButtonStyle.Standalone;
            this.editToken.Text = "";
            this.editToken.ToolTipBody = "Edit the token content in the token XSL editor.";
            this.editToken.ToolTipStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.SuperTip;
            this.editToken.ToolTipTitle = "Token Editor";
            this.editToken.UniqueName = "34527D302837489A34527D302837489A";
            this.editToken.Click += new System.EventHandler(this.OnEditToken);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem1.Text = "toolStripMenuItem1";
            this.toolStripMenuItem1.ToolTipText = "53213";
            // 
            // TemplateTokenTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tokenTextBox);
            this.Name = "TemplateTokenTextBox";
            this.Size = new System.Drawing.Size(210, 21);
            this.Load += new System.EventHandler(this.OnLoad);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonTextBox tokenTextBox;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecAny toeknOptionsDropdown;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecAny editToken;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}
