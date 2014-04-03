namespace ShipWorks.Shipping.ShipSense.Settings
{
    partial class ShipSenseHashConfigurationControl
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
            this.labelDescription = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.panelCustomization = new System.Windows.Forms.Panel();
            this.toolStripAddRule = new System.Windows.Forms.ToolStrip();
            this.addItemAttributeLine = new System.Windows.Forms.ToolStripButton();
            this.panelHeader.SuspendLayout();
            this.panelCustomization.SuspendLayout();
            this.toolStripAddRule.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(3, 9);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(460, 31);
            this.labelDescription.TabIndex = 0;
            this.labelDescription.Text = "You can customize how ShipSense learns about the way you ship by telling ShipSens" +
    "e which properties and attributes of items have an impact on how you ship orders" +
    ".";
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.labelDescription);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(493, 50);
            this.panelHeader.TabIndex = 1;
            // 
            // panelCustomization
            // 
            this.panelCustomization.Controls.Add(this.toolStripAddRule);
            this.panelCustomization.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCustomization.Location = new System.Drawing.Point(0, 50);
            this.panelCustomization.Name = "panelCustomization";
            this.panelCustomization.Size = new System.Drawing.Size(493, 283);
            this.panelCustomization.TabIndex = 2;
            // 
            // toolStripAddRule
            // 
            this.toolStripAddRule.BackColor = System.Drawing.Color.Transparent;
            this.toolStripAddRule.CanOverflow = false;
            this.toolStripAddRule.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripAddRule.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStripAddRule.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripAddRule.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addItemAttributeLine});
            this.toolStripAddRule.Location = new System.Drawing.Point(6, 33);
            this.toolStripAddRule.Name = "toolStripAddRule";
            this.toolStripAddRule.Padding = new System.Windows.Forms.Padding(0);
            this.toolStripAddRule.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripAddRule.Size = new System.Drawing.Size(82, 25);
            this.toolStripAddRule.Stretch = true;
            this.toolStripAddRule.TabIndex = 38;
            // 
            // addItemAttributeLine
            // 
            this.addItemAttributeLine.Image = global::ShipWorks.Properties.Resources.add16;
            this.addItemAttributeLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addItemAttributeLine.Name = "addItemAttributeLine";
            this.addItemAttributeLine.Size = new System.Drawing.Size(49, 22);
            this.addItemAttributeLine.Text = "Add";
            // 
            // ShipSenseHashConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelCustomization);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "ShipSenseHashConfigurationControl";
            this.Size = new System.Drawing.Size(493, 333);
            this.panelHeader.ResumeLayout(false);
            this.panelCustomization.ResumeLayout(false);
            this.panelCustomization.PerformLayout();
            this.toolStripAddRule.ResumeLayout(false);
            this.toolStripAddRule.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelCustomization;
        private System.Windows.Forms.ToolStrip toolStripAddRule;
        private System.Windows.Forms.ToolStripButton addItemAttributeLine;
    }
}
