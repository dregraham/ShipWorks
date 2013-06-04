namespace ShipWorks.UI.Controls
{
    partial class CollapsibleGroupControl
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
            this.collapseControl = new ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup();
            this.buttonSpecCollapse = new ComponentFactory.Krypton.Toolkit.ButtonSpecHeaderGroup();
            ((System.ComponentModel.ISupportInitialize) (this.collapseControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.collapseControl.Panel)).BeginInit();
            this.collapseControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // collapseControl
            // 
            this.collapseControl.AutoCollapseArrow = false;
            this.collapseControl.ButtonSpecs.AddRange(new ComponentFactory.Krypton.Toolkit.ButtonSpecHeaderGroup[] {
            this.buttonSpecCollapse});
            this.collapseControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.collapseControl.HeaderStylePrimary = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.collapseControl.HeaderVisibleSecondary = false;
            this.collapseControl.Location = new System.Drawing.Point(0, 0);
            this.collapseControl.Name = "collapseControl";
            this.collapseControl.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.collapseControl.Size = new System.Drawing.Size(364, 328);
            this.collapseControl.StateCommon.Border.Color1 = System.Drawing.Color.Gray;
            this.collapseControl.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders) ((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.collapseControl.StateCommon.Border.Rounding = 2;
            this.collapseControl.StateCommon.HeaderPrimary.Back.Color1 = System.Drawing.Color.White;
            this.collapseControl.StateCommon.HeaderPrimary.Back.Color2 = System.Drawing.Color.FromArgb(((int) (((byte) (224)))), ((int) (((byte) (224)))), ((int) (((byte) (224)))));
            this.collapseControl.StateCommon.HeaderPrimary.Border.Color1 = System.Drawing.Color.Gray;
            this.collapseControl.StateCommon.HeaderPrimary.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders) ((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.collapseControl.StateCommon.HeaderPrimary.Border.Rounding = 2;
            this.collapseControl.StateCommon.HeaderPrimary.Content.LongText.Color1 = System.Drawing.Color.Gray;
            this.collapseControl.StateCommon.HeaderPrimary.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.collapseControl.StateCommon.HeaderPrimary.Content.ShortText.Color1 = System.Drawing.Color.FromArgb(((int) (((byte) (64)))), ((int) (((byte) (64)))), ((int) (((byte) (64)))));
            this.collapseControl.TabIndex = 5;
            this.collapseControl.Text = "Section Name";
            this.collapseControl.ValuesPrimary.Description = "";
            this.collapseControl.ValuesPrimary.Heading = "Section Name";
            this.collapseControl.ValuesPrimary.Image = null;
            this.collapseControl.ValuesSecondary.Description = "";
            this.collapseControl.ValuesSecondary.Heading = "Description";
            this.collapseControl.ValuesSecondary.Image = null;
            this.collapseControl.Click += new System.EventHandler(this.OnClick);
            // 
            // buttonSpecCollapse
            // 
            this.buttonSpecCollapse.ExtraText = "";
            this.buttonSpecCollapse.Image = null;
            this.buttonSpecCollapse.Text = "";
            this.buttonSpecCollapse.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.ArrowUp;
            this.buttonSpecCollapse.UniqueName = "FE0A894F00794EDBFE0A894F00794EDB";
            // 
            // CollapsibleGroupControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.collapseControl);
            this.Name = "CollapsibleGroupControl";
            this.Size = new System.Drawing.Size(364, 328);
            ((System.ComponentModel.ISupportInitialize) (this.collapseControl.Panel)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.collapseControl)).EndInit();
            this.collapseControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.ButtonSpecHeaderGroup buttonSpecCollapse;
        protected ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup collapseControl;
    }
}
