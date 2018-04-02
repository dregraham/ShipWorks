namespace ShipWorks.Shipping.Settings.Printing
{
    partial class PrintOutputRuleControl
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
            this.toolStripDelete = new System.Windows.Forms.ToolStrip();
            this.delete = new System.Windows.Forms.ToolStripButton();
            this.labelPrint = new System.Windows.Forms.Label();
            this.labelShipment = new System.Windows.Forms.Label();
            this.linkAlwaysOption = new ShipWorks.UI.Controls.LinkControl();
            this.templateCombo = new ShipWorks.Templates.Controls.TemplateComboBox();
            this.filterCombo = new ShipWorks.Filters.Controls.FilterComboBox();
            this.toolStripDelete.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripDelete
            // 
            this.toolStripDelete.BackColor = System.Drawing.Color.Transparent;
            this.toolStripDelete.CanOverflow = false;
            this.toolStripDelete.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripDelete.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStripDelete.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripDelete.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.delete});
            this.toolStripDelete.Location = new System.Drawing.Point(0, 1);
            this.toolStripDelete.Name = "toolStripDelete";
            this.toolStripDelete.Padding = new System.Windows.Forms.Padding(0);
            this.toolStripDelete.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripDelete.Size = new System.Drawing.Size(25, 25);
            this.toolStripDelete.Stretch = true;
            this.toolStripDelete.TabIndex = 0;
            // 
            // delete
            // 
            this.delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.delete.Image = global::ShipWorks.Properties.Resources.delete16;
            this.delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(23, 22);
            this.delete.Text = "Remove Line";
            this.delete.Click += new System.EventHandler(this.OnDelete);
            // 
            // labelPrint
            // 
            this.labelPrint.AutoSize = true;
            this.labelPrint.Location = new System.Drawing.Point(354, 6);
            this.labelPrint.Name = "labelPrint";
            this.labelPrint.Size = new System.Drawing.Size(52, 13);
            this.labelPrint.TabIndex = 3;
            this.labelPrint.Text = "print with";
            // 
            // labelShipment
            // 
            this.labelShipment.AutoSize = true;
            this.labelShipment.Location = new System.Drawing.Point(26, 6);
            this.labelShipment.Name = "labelShipment";
            this.labelShipment.Size = new System.Drawing.Size(101, 13);
            this.labelShipment.TabIndex = 1;
            this.labelShipment.Text = "If the shipment is in";
            // 
            // linkAlwaysOption
            // 
            this.linkAlwaysOption.AutoSize = true;
            this.linkAlwaysOption.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkAlwaysOption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkAlwaysOption.ForeColor = System.Drawing.Color.Blue;
            this.linkAlwaysOption.Location = new System.Drawing.Point(133, 6);
            this.linkAlwaysOption.Name = "linkAlwaysOption";
            this.linkAlwaysOption.Size = new System.Drawing.Size(41, 13);
            this.linkAlwaysOption.TabIndex = 5;
            this.linkAlwaysOption.Text = "Always";
            this.linkAlwaysOption.Click += new System.EventHandler(this.OnClickAlwaysOption);
            // 
            // templateCombo
            // 
            this.templateCombo.FormattingEnabled = true;
            this.templateCombo.Location = new System.Drawing.Point(407, 3);
            this.templateCombo.Name = "templateCombo";
            this.templateCombo.Size = new System.Drawing.Size(144, 21);
            this.templateCombo.TabIndex = 4;
            // 
            // filterCombo
            // 
            this.filterCombo.AllowQuickFilter = true;
            this.filterCombo.FormattingEnabled = true;
            this.filterCombo.Location = new System.Drawing.Point(206, 3);
            this.filterCombo.Name = "filterCombo";
            this.filterCombo.Size = new System.Drawing.Size(140, 21);
            this.filterCombo.TabIndex = 2;
            this.filterCombo.SizeChanged += new System.EventHandler(this.OnFilterComboSizeChanged);
            // 
            // PrintOutputRuleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkAlwaysOption);
            this.Controls.Add(this.templateCombo);
            this.Controls.Add(this.toolStripDelete);
            this.Controls.Add(this.labelPrint);
            this.Controls.Add(this.filterCombo);
            this.Controls.Add(this.labelShipment);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PrintOutputRuleControl";
            this.Size = new System.Drawing.Size(556, 28);
            this.toolStripDelete.ResumeLayout(false);
            this.toolStripDelete.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.Templates.Controls.TemplateComboBox templateCombo;
        private System.Windows.Forms.ToolStrip toolStripDelete;
        private System.Windows.Forms.ToolStripButton delete;
        private System.Windows.Forms.Label labelPrint;
        private ShipWorks.Filters.Controls.FilterComboBox filterCombo;
        private System.Windows.Forms.Label labelShipment;
        private ShipWorks.UI.Controls.LinkControl linkAlwaysOption;
    }
}
