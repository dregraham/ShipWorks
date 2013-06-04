namespace ShipWorks.Shipping.Settings.Defaults
{
    partial class ShippingDefaultsRuleControl
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
            this.labelIndex = new System.Windows.Forms.Label();
            this.toolStipDelete = new System.Windows.Forms.ToolStrip();
            this.buttonDelete = new System.Windows.Forms.ToolStripButton();
            this.linkProfile = new System.Windows.Forms.Label();
            this.labelApply = new System.Windows.Forms.Label();
            this.labelOrder = new System.Windows.Forms.Label();
            this.filterCombo = new ShipWorks.Filters.Controls.FilterComboBox();
            this.toolStipDelete.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelIndex
            // 
            this.labelIndex.AutoSize = true;
            this.labelIndex.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelIndex.Location = new System.Drawing.Point(26, 6);
            this.labelIndex.Name = "labelIndex";
            this.labelIndex.Size = new System.Drawing.Size(17, 13);
            this.labelIndex.TabIndex = 1;
            this.labelIndex.Text = "1.";
            // 
            // toolStipDelete
            // 
            this.toolStipDelete.BackColor = System.Drawing.Color.Transparent;
            this.toolStipDelete.CanOverflow = false;
            this.toolStipDelete.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStipDelete.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStipDelete.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStipDelete.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonDelete});
            this.toolStipDelete.Location = new System.Drawing.Point(1, 1);
            this.toolStipDelete.Name = "toolStipDelete";
            this.toolStipDelete.Padding = new System.Windows.Forms.Padding(0);
            this.toolStipDelete.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStipDelete.Size = new System.Drawing.Size(25, 25);
            this.toolStipDelete.Stretch = true;
            this.toolStipDelete.TabIndex = 0;
            // 
            // buttonDelete
            // 
            this.buttonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonDelete.Image = global::ShipWorks.Properties.Resources.delete16;
            this.buttonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(23, 22);
            this.buttonDelete.Text = "Remove Line";
            this.buttonDelete.Click += new System.EventHandler(this.OnDelete);
            // 
            // linkProfile
            // 
            this.linkProfile.AutoSize = true;
            this.linkProfile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkProfile.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.linkProfile.ForeColor = System.Drawing.Color.Blue;
            this.linkProfile.Location = new System.Drawing.Point(360, 6);
            this.linkProfile.Name = "linkProfile";
            this.linkProfile.Size = new System.Drawing.Size(66, 13);
            this.linkProfile.TabIndex = 5;
            this.linkProfile.Text = "Some Profile";
            this.linkProfile.Click += new System.EventHandler(this.OnClickProfileLink);
            // 
            // labelApply
            // 
            this.labelApply.AutoSize = true;
            this.labelApply.Location = new System.Drawing.Point(295, 6);
            this.labelApply.Name = "labelApply";
            this.labelApply.Size = new System.Drawing.Size(66, 13);
            this.labelApply.TabIndex = 4;
            this.labelApply.Text = "apply profile";
            // 
            // labelOrder
            // 
            this.labelOrder.AutoSize = true;
            this.labelOrder.Location = new System.Drawing.Point(49, 6);
            this.labelOrder.Name = "labelOrder";
            this.labelOrder.Size = new System.Drawing.Size(84, 13);
            this.labelOrder.TabIndex = 2;
            this.labelOrder.Text = "If the order is in";
            // 
            // filterCombo
            // 
            this.filterCombo.AllowQuickFilter = true;
            this.filterCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterCombo.FormattingEnabled = true;
            this.filterCombo.Location = new System.Drawing.Point(134, 3);
            this.filterCombo.Name = "filterCombo";
            this.filterCombo.Size = new System.Drawing.Size(159, 21);
            this.filterCombo.TabIndex = 3;
            // 
            // ShippingDefaultsRuleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelIndex);
            this.Controls.Add(this.toolStipDelete);
            this.Controls.Add(this.linkProfile);
            this.Controls.Add(this.labelApply);
            this.Controls.Add(this.filterCombo);
            this.Controls.Add(this.labelOrder);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "ShippingDefaultsRuleControl";
            this.Size = new System.Drawing.Size(530, 28);
            this.toolStipDelete.ResumeLayout(false);
            this.toolStipDelete.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelIndex;
        private System.Windows.Forms.ToolStrip toolStipDelete;
        private System.Windows.Forms.ToolStripButton buttonDelete;
        private System.Windows.Forms.Label linkProfile;
        private System.Windows.Forms.Label labelApply;
        private ShipWorks.Filters.Controls.FilterComboBox filterCombo;
        private System.Windows.Forms.Label labelOrder;
    }
}
