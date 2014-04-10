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
            this.panelProperties = new System.Windows.Forms.Panel();
            this.itemProperties = new ShipWorks.Shipping.ShipSense.Settings.ShipSenseItemPropertyControl();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.toolStripAddRule = new System.Windows.Forms.ToolStrip();
            this.addItemAttributeLine = new System.Windows.Forms.ToolStripButton();
            this.labelAttributesHeader = new System.Windows.Forms.Label();
            this.labelAttributeInstructions = new System.Windows.Forms.Label();
            this.labelItemPropertiesHeader = new System.Windows.Forms.Label();
            this.labelItemPropertiesInstructions = new System.Windows.Forms.Label();
            this.panelAttributes = new System.Windows.Forms.Panel();
            this.panelProperties.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.toolStripAddRule.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelProperties
            // 
            this.panelProperties.BackColor = System.Drawing.SystemColors.Control;
            this.panelProperties.Controls.Add(this.itemProperties);
            this.panelProperties.Location = new System.Drawing.Point(2, 36);
            this.panelProperties.Name = "panelProperties";
            this.panelProperties.Size = new System.Drawing.Size(487, 236);
            this.panelProperties.TabIndex = 2;
            // 
            // itemProperties
            // 
            this.itemProperties.Location = new System.Drawing.Point(16, 1);
            this.itemProperties.Name = "itemProperties";
            this.itemProperties.Size = new System.Drawing.Size(187, 230);
            this.itemProperties.TabIndex = 0;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.toolStripAddRule);
            this.panelBottom.Location = new System.Drawing.Point(12, 348);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(471, 35);
            this.panelBottom.TabIndex = 3;
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
            this.toolStripAddRule.Location = new System.Drawing.Point(6, 5);
            this.toolStripAddRule.Name = "toolStripAddRule";
            this.toolStripAddRule.Padding = new System.Windows.Forms.Padding(0);
            this.toolStripAddRule.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripAddRule.Size = new System.Drawing.Size(101, 25);
            this.toolStripAddRule.Stretch = true;
            this.toolStripAddRule.TabIndex = 38;
            // 
            // addItemAttributeLine
            // 
            this.addItemAttributeLine.Image = global::ShipWorks.Properties.Resources.add16;
            this.addItemAttributeLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addItemAttributeLine.Name = "addItemAttributeLine";
            this.addItemAttributeLine.Size = new System.Drawing.Size(99, 22);
            this.addItemAttributeLine.Text = "Add Attribute";
            this.addItemAttributeLine.Click += new System.EventHandler(this.OnAddAttribute);
            // 
            // labelAttributesHeader
            // 
            this.labelAttributesHeader.AutoSize = true;
            this.labelAttributesHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelAttributesHeader.Location = new System.Drawing.Point(-1, 282);
            this.labelAttributesHeader.Name = "labelAttributesHeader";
            this.labelAttributesHeader.Size = new System.Drawing.Size(96, 13);
            this.labelAttributesHeader.TabIndex = 4;
            this.labelAttributesHeader.Text = "Item Attributes";
            // 
            // labelAttributeInstructions
            // 
            this.labelAttributeInstructions.Location = new System.Drawing.Point(15, 296);
            this.labelAttributeInstructions.Name = "labelAttributeInstructions";
            this.labelAttributeInstructions.Size = new System.Drawing.Size(474, 32);
            this.labelAttributeInstructions.TabIndex = 5;
            this.labelAttributeInstructions.Text = "ShipSense only uses the item attributes that match the attributes below. Any attr" +
    "ibutes that are not applicable to an item will be ignored.";
            // 
            // labelItemPropertiesHeader
            // 
            this.labelItemPropertiesHeader.AutoSize = true;
            this.labelItemPropertiesHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelItemPropertiesHeader.Location = new System.Drawing.Point(-1, 3);
            this.labelItemPropertiesHeader.Name = "labelItemPropertiesHeader";
            this.labelItemPropertiesHeader.Size = new System.Drawing.Size(97, 13);
            this.labelItemPropertiesHeader.TabIndex = 6;
            this.labelItemPropertiesHeader.Text = "Item Properties";
            // 
            // labelItemPropertiesInstructions
            // 
            this.labelItemPropertiesInstructions.AutoSize = true;
            this.labelItemPropertiesInstructions.Location = new System.Drawing.Point(15, 18);
            this.labelItemPropertiesInstructions.Name = "labelItemPropertiesInstructions";
            this.labelItemPropertiesInstructions.Size = new System.Drawing.Size(441, 13);
            this.labelItemPropertiesInstructions.TabIndex = 7;
            this.labelItemPropertiesInstructions.Text = "Select the properties of an order item that ShipSense should use as it learns how" +
    " you ship.";
            // 
            // panelAttributes
            // 
            this.panelAttributes.Location = new System.Drawing.Point(18, 332);
            this.panelAttributes.Name = "panelAttributes";
            this.panelAttributes.Size = new System.Drawing.Size(471, 10);
            this.panelAttributes.TabIndex = 8;
            // 
            // ShipSenseHashConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelAttributes);
            this.Controls.Add(this.labelItemPropertiesInstructions);
            this.Controls.Add(this.labelItemPropertiesHeader);
            this.Controls.Add(this.labelAttributeInstructions);
            this.Controls.Add(this.labelAttributesHeader);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelProperties);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "ShipSenseHashConfigurationControl";
            this.Size = new System.Drawing.Size(493, 400);
            this.panelProperties.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.toolStripAddRule.ResumeLayout(false);
            this.toolStripAddRule.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelProperties;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.ToolStrip toolStripAddRule;
        private System.Windows.Forms.ToolStripButton addItemAttributeLine;
        private ShipSenseItemPropertyControl itemProperties;
        private System.Windows.Forms.Label labelAttributesHeader;
        private System.Windows.Forms.Label labelAttributeInstructions;
        private System.Windows.Forms.Label labelItemPropertiesHeader;
        private System.Windows.Forms.Label labelItemPropertiesInstructions;
        private System.Windows.Forms.Panel panelAttributes;
    }
}
