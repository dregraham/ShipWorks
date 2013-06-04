namespace ShipWorks.ApplicationCore.Dashboard
{
    partial class DashboardBar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DashboardBar));
            this.kryptonGroup = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.linkDismiss = new System.Windows.Forms.Label();
            this.labelSecondary = new System.Windows.Forms.Label();
            this.labelPrimary = new System.Windows.Forms.Label();
            this.image = new System.Windows.Forms.PictureBox();
            this.panelActions = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize) (this.kryptonGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.kryptonGroup.Panel)).BeginInit();
            this.kryptonGroup.Panel.SuspendLayout();
            this.kryptonGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.image)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonGroup
            // 
            this.kryptonGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonGroup.GroupBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridHeaderRowList;
            this.kryptonGroup.GroupBorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.ControlRibbon;
            this.kryptonGroup.Location = new System.Drawing.Point(0, 0);
            this.kryptonGroup.Name = "kryptonGroup";
            // 
            // kryptonGroup.Panel
            // 
            this.kryptonGroup.Panel.Controls.Add(this.linkDismiss);
            this.kryptonGroup.Panel.Controls.Add(this.labelSecondary);
            this.kryptonGroup.Panel.Controls.Add(this.labelPrimary);
            this.kryptonGroup.Panel.Controls.Add(this.image);
            this.kryptonGroup.Panel.Controls.Add(this.panelActions);
            this.kryptonGroup.Size = new System.Drawing.Size(561, 24);
            this.kryptonGroup.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders) ((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonGroup.StateCommon.Border.Rounding = 9;
            this.kryptonGroup.TabIndex = 0;
            // 
            // linkDismiss
            // 
            this.linkDismiss.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkDismiss.BackColor = System.Drawing.Color.Transparent;
            this.linkDismiss.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkDismiss.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.linkDismiss.ForeColor = System.Drawing.Color.RoyalBlue;
            this.linkDismiss.Image = ((System.Drawing.Image) (resources.GetObject("linkDismiss.Image")));
            this.linkDismiss.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.linkDismiss.Location = new System.Drawing.Point(499, 1);
            this.linkDismiss.Name = "linkDismiss";
            this.linkDismiss.Size = new System.Drawing.Size(53, 13);
            this.linkDismiss.TabIndex = 4;
            this.linkDismiss.Text = "Dismiss";
            this.linkDismiss.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkDismiss.Click += new System.EventHandler(this.OnDismiss);
            // 
            // labelSecondary
            // 
            this.labelSecondary.AutoSize = true;
            this.labelSecondary.BackColor = System.Drawing.Color.Transparent;
            this.labelSecondary.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (50)))), ((int) (((byte) (50)))), ((int) (((byte) (50)))));
            this.labelSecondary.Location = new System.Drawing.Point(102, 2);
            this.labelSecondary.Name = "labelSecondary";
            this.labelSecondary.Size = new System.Drawing.Size(83, 13);
            this.labelSecondary.TabIndex = 2;
            this.labelSecondary.Text = "Secondary Text";
            // 
            // labelPrimary
            // 
            this.labelPrimary.AutoSize = true;
            this.labelPrimary.BackColor = System.Drawing.Color.Transparent;
            this.labelPrimary.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelPrimary.Location = new System.Drawing.Point(16, 2);
            this.labelPrimary.Name = "labelPrimary";
            this.labelPrimary.Size = new System.Drawing.Size(78, 13);
            this.labelPrimary.TabIndex = 1;
            this.labelPrimary.Text = "Prmary Text";
            // 
            // image
            // 
            this.image.BackColor = System.Drawing.Color.Transparent;
            this.image.Image = global::ShipWorks.Properties.Resources.box_software;
            this.image.Location = new System.Drawing.Point(1, 1);
            this.image.Name = "image";
            this.image.Size = new System.Drawing.Size(16, 16);
            this.image.TabIndex = 0;
            this.image.TabStop = false;
            // 
            // panelActions
            // 
            this.panelActions.AutoSize = true;
            this.panelActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelActions.BackColor = System.Drawing.Color.Transparent;
            this.panelActions.Location = new System.Drawing.Point(200, 0);
            this.panelActions.MinimumSize = new System.Drawing.Size(50, 16);
            this.panelActions.Name = "panelActions";
            this.panelActions.Size = new System.Drawing.Size(50, 16);
            this.panelActions.TabIndex = 3;
            // 
            // DashboardBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.kryptonGroup);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "DashboardBar";
            this.Size = new System.Drawing.Size(561, 26);
            ((System.ComponentModel.ISupportInitialize) (this.kryptonGroup.Panel)).EndInit();
            this.kryptonGroup.Panel.ResumeLayout(false);
            this.kryptonGroup.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.kryptonGroup)).EndInit();
            this.kryptonGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.image)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonGroup kryptonGroup;
        private System.Windows.Forms.Label linkDismiss;
        private System.Windows.Forms.Label labelSecondary;
        private System.Windows.Forms.Label labelPrimary;
        private System.Windows.Forms.PictureBox image;
        private System.Windows.Forms.Panel panelActions;
    }
}
