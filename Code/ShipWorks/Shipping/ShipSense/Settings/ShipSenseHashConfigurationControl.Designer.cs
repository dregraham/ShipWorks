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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShipSenseHashConfigurationControl));
            this.panelProperties = new System.Windows.Forms.Panel();
            this.itemProperties = new ShipWorks.Shipping.ShipSense.Settings.ShipSenseItemPropertyControl();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.toolStripAddRule = new System.Windows.Forms.ToolStrip();
            this.addItemAttributeLine = new System.Windows.Forms.ToolStripButton();
            this.labelAttributeInstructions = new System.Windows.Forms.Label();
            this.labelItemPropertiesInstructions = new System.Windows.Forms.Label();
            this.panelAttributes = new System.Windows.Forms.Panel();
            this.sectionAddressCasing = new ShipWorks.UI.Controls.SectionTitle();
            this.itemPropertiesSection = new ShipWorks.UI.Controls.SectionTitle();
            this.panelProperties.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.toolStripAddRule.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelProperties
            // 
            this.panelProperties.BackColor = System.Drawing.Color.White;
            this.panelProperties.Controls.Add(this.itemProperties);
            this.panelProperties.Location = new System.Drawing.Point(5, 47);
            this.panelProperties.Name = "panelProperties";
            this.panelProperties.Size = new System.Drawing.Size(479, 231);
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
            this.panelBottom.Location = new System.Drawing.Point(13, 373);
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
            // labelAttributeInstructions
            // 
            this.labelAttributeInstructions.Location = new System.Drawing.Point(16, 310);
            this.labelAttributeInstructions.Name = "labelAttributeInstructions";
            this.labelAttributeInstructions.Size = new System.Drawing.Size(474, 42);
            this.labelAttributeInstructions.TabIndex = 5;
            this.labelAttributeInstructions.Text = resources.GetString("labelAttributeInstructions.Text");
            // 
            // labelItemPropertiesInstructions
            // 
            this.labelItemPropertiesInstructions.AutoSize = true;
            this.labelItemPropertiesInstructions.Location = new System.Drawing.Point(18, 30);
            this.labelItemPropertiesInstructions.Name = "labelItemPropertiesInstructions";
            this.labelItemPropertiesInstructions.Size = new System.Drawing.Size(386, 13);
            this.labelItemPropertiesInstructions.TabIndex = 7;
            this.labelItemPropertiesInstructions.Text = "Select the properties of your items that ShipSense will use to match like orders." +
    "";
            // 
            // panelAttributes
            // 
            this.panelAttributes.Location = new System.Drawing.Point(19, 357);
            this.panelAttributes.Name = "panelAttributes";
            this.panelAttributes.Size = new System.Drawing.Size(466, 10);
            this.panelAttributes.TabIndex = 8;
            // 
            // sectionAddressCasing
            // 
            this.sectionAddressCasing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionAddressCasing.Location = new System.Drawing.Point(3, 282);
            this.sectionAddressCasing.Name = "sectionAddressCasing";
            this.sectionAddressCasing.Size = new System.Drawing.Size(487, 22);
            this.sectionAddressCasing.TabIndex = 10;
            this.sectionAddressCasing.Text = "Item Attributes";
            // 
            // itemPropertiesSection
            // 
            this.itemPropertiesSection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.itemPropertiesSection.Location = new System.Drawing.Point(3, 3);
            this.itemPropertiesSection.Name = "itemPropertiesSection";
            this.itemPropertiesSection.Size = new System.Drawing.Size(492, 22);
            this.itemPropertiesSection.TabIndex = 9;
            this.itemPropertiesSection.Text = "Item Properties";
            // 
            // ShipSenseHashConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.sectionAddressCasing);
            this.Controls.Add(this.itemPropertiesSection);
            this.Controls.Add(this.panelAttributes);
            this.Controls.Add(this.labelItemPropertiesInstructions);
            this.Controls.Add(this.labelAttributeInstructions);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelProperties);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "ShipSenseHashConfigurationControl";
            this.Size = new System.Drawing.Size(493, 420);
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
        private System.Windows.Forms.Label labelAttributeInstructions;
        private System.Windows.Forms.Label labelItemPropertiesInstructions;
        private System.Windows.Forms.Panel panelAttributes;
        private UI.Controls.SectionTitle itemPropertiesSection;
        private UI.Controls.SectionTitle sectionAddressCasing;
    }
}
