namespace ShipWorks.Shipping.Settings
{
    partial class ShippingProviderRuleControl
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
            this.labelUse = new System.Windows.Forms.Label();
            this.toolStipDelete = new System.Windows.Forms.ToolStrip();
            this.buttonDelete = new System.Windows.Forms.ToolStripButton();
            this.labelOrder = new System.Windows.Forms.Label();
            this.shipmentTypeCombo = new System.Windows.Forms.ComboBox();
            this.filterCombo = new ShipWorks.Filters.Controls.FilterComboBox();
            this.toolStipDelete.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelUse
            // 
            this.labelUse.AutoSize = true;
            this.labelUse.Location = new System.Drawing.Point(266, 6);
            this.labelUse.Name = "labelUse";
            this.labelUse.Size = new System.Drawing.Size(24, 13);
            this.labelUse.TabIndex = 3;
            this.labelUse.Text = "use";
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
            // labelOrder
            // 
            this.labelOrder.AutoSize = true;
            this.labelOrder.Location = new System.Drawing.Point(27, 6);
            this.labelOrder.Name = "labelOrder";
            this.labelOrder.Size = new System.Drawing.Size(84, 13);
            this.labelOrder.TabIndex = 1;
            this.labelOrder.Text = "If the order is in";
            // 
            // shipmentTypeCombo
            // 
            this.shipmentTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.shipmentTypeCombo.FormattingEnabled = true;
            this.shipmentTypeCombo.Location = new System.Drawing.Point(291, 3);
            this.shipmentTypeCombo.Name = "shipmentTypeCombo";
            this.shipmentTypeCombo.Size = new System.Drawing.Size(159, 21);
            this.shipmentTypeCombo.TabIndex = 4;
            // 
            // filterCombo
            // 
            this.filterCombo.AllowQuickFilter = true;
            this.filterCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterCombo.FormattingEnabled = true;
            this.filterCombo.Location = new System.Drawing.Point(112, 3);
            this.filterCombo.Name = "filterCombo";
            this.filterCombo.Size = new System.Drawing.Size(140, 21);
            this.filterCombo.SizeToContent = true;
            this.filterCombo.TabIndex = 2;
            this.filterCombo.SizeChanged += new System.EventHandler(this.OnFilterComboSizeChanged);
            // 
            // ShippingProviderRuleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelUse);
            this.Controls.Add(this.toolStipDelete);
            this.Controls.Add(this.filterCombo);
            this.Controls.Add(this.labelOrder);
            this.Controls.Add(this.shipmentTypeCombo);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "ShippingProviderRuleControl";
            this.Size = new System.Drawing.Size(485, 28);
            this.toolStipDelete.ResumeLayout(false);
            this.toolStipDelete.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelUse;
        private System.Windows.Forms.ToolStrip toolStipDelete;
        private System.Windows.Forms.ToolStripButton buttonDelete;
        private ShipWorks.Filters.Controls.FilterComboBox filterCombo;
        private System.Windows.Forms.Label labelOrder;
        private System.Windows.Forms.ComboBox shipmentTypeCombo;
    }
}
