using ShipWorks.Core.Messaging;

namespace ShipWorks.Shipping.Settings
{
    partial class ShippingProviderControl
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
            if (disposing)
            {
                Messenger.Current.Remove(carrierConfiguredToken);

                if (components != null)
                {
                    components.Dispose();   
                }
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
            this.toolStripFakeDelete = new System.Windows.Forms.ToolStrip();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripAddRule = new System.Windows.Forms.ToolStrip();
            this.addSettingsLine = new System.Windows.Forms.ToolStripButton();
            this.labelDefaultType = new System.Windows.Forms.Label();
            this.shipmentTypeCombo = new System.Windows.Forms.ComboBox();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.toolStripFakeDelete.SuspendLayout();
            this.toolStripAddRule.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripFakeDelete
            // 
            this.toolStripFakeDelete.BackColor = System.Drawing.Color.Transparent;
            this.toolStripFakeDelete.CanOverflow = false;
            this.toolStripFakeDelete.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripFakeDelete.Enabled = false;
            this.toolStripFakeDelete.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStripFakeDelete.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripFakeDelete.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton2});
            this.toolStripFakeDelete.Location = new System.Drawing.Point(0, 1);
            this.toolStripFakeDelete.Name = "toolStripFakeDelete";
            this.toolStripFakeDelete.Padding = new System.Windows.Forms.Padding(0);
            this.toolStripFakeDelete.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripFakeDelete.Size = new System.Drawing.Size(25, 25);
            this.toolStripFakeDelete.Stretch = true;
            this.toolStripFakeDelete.TabIndex = 40;
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::ShipWorks.Properties.Resources.delete16;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Remove Line";
            // 
            // toolStripAddRule
            // 
            this.toolStripAddRule.BackColor = System.Drawing.Color.Transparent;
            this.toolStripAddRule.CanOverflow = false;
            this.toolStripAddRule.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripAddRule.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStripAddRule.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripAddRule.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addSettingsLine});
            this.toolStripAddRule.Location = new System.Drawing.Point(2, 27);
            this.toolStripAddRule.Name = "toolStripAddRule";
            this.toolStripAddRule.Padding = new System.Windows.Forms.Padding(0);
            this.toolStripAddRule.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripAddRule.Size = new System.Drawing.Size(77, 25);
            this.toolStripAddRule.Stretch = true;
            this.toolStripAddRule.TabIndex = 37;
            // 
            // addSettingsLine
            // 
            this.addSettingsLine.Image = global::ShipWorks.Properties.Resources.add16;
            this.addSettingsLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addSettingsLine.Name = "addSettingsLine";
            this.addSettingsLine.Size = new System.Drawing.Size(75, 22);
            this.addSettingsLine.Text = "Add Rule";
            this.addSettingsLine.Click += new System.EventHandler(this.OnAddRule);
            // 
            // labelDefaultType
            // 
            this.labelDefaultType.AutoSize = true;
            this.labelDefaultType.Location = new System.Drawing.Point(27, 6);
            this.labelDefaultType.Name = "labelDefaultType";
            this.labelDefaultType.Size = new System.Drawing.Size(76, 13);
            this.labelDefaultType.TabIndex = 39;
            this.labelDefaultType.Text = "Otherwise use";
            // 
            // shipmentTypeCombo
            // 
            this.shipmentTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.shipmentTypeCombo.FormattingEnabled = true;
            this.shipmentTypeCombo.Location = new System.Drawing.Point(106, 3);
            this.shipmentTypeCombo.Name = "shipmentTypeCombo";
            this.shipmentTypeCombo.Size = new System.Drawing.Size(150, 21);
            this.shipmentTypeCombo.TabIndex = 38;
            // 
            // panelMain
            // 
            this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMain.Location = new System.Drawing.Point(3, 3);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(397, 47);
            this.panelMain.TabIndex = 0;
            // 
            // panelBottom
            // 
            this.panelBottom.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBottom.Controls.Add(this.labelDefaultType);
            this.panelBottom.Controls.Add(this.shipmentTypeCombo);
            this.panelBottom.Controls.Add(this.toolStripFakeDelete);
            this.panelBottom.Controls.Add(this.toolStripAddRule);
            this.panelBottom.Location = new System.Drawing.Point(3, 50);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(397, 63);
            this.panelBottom.TabIndex = 1;
            // 
            // ShippingProviderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelMain);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "ShippingProviderControl";
            this.Size = new System.Drawing.Size(403, 120);
            this.toolStripFakeDelete.ResumeLayout(false);
            this.toolStripFakeDelete.PerformLayout();
            this.toolStripAddRule.ResumeLayout(false);
            this.toolStripAddRule.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripFakeDelete;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStrip toolStripAddRule;
        private System.Windows.Forms.ToolStripButton addSettingsLine;
        private System.Windows.Forms.Label labelDefaultType;
        private System.Windows.Forms.ComboBox shipmentTypeCombo;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelBottom;
    }
}
