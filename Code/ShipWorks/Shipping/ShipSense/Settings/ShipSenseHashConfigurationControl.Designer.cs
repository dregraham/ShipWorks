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
            this.labelAttributeInstructions = new System.Windows.Forms.Label();
            this.labelItemPropertiesInstructions = new System.Windows.Forms.Label();
            this.attributes = new ShipWorks.UI.Controls.StringList();
            this.sectionAddressCasing = new ShipWorks.UI.Controls.SectionTitle();
            this.itemPropertiesSection = new ShipWorks.UI.Controls.SectionTitle();
            this.itemProperties = new ShipWorks.Shipping.ShipSense.Settings.ShipSenseItemPropertyControl();
            this.panelProperties.SuspendLayout();
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
            // attributes
            // 
            this.attributes.AddButtonText = "Add Attributes";
            this.attributes.AutoSize = true;
            this.attributes.Location = new System.Drawing.Point(3, 355);
            this.attributes.Name = "attributes";
            this.attributes.Size = new System.Drawing.Size(475, 38);
            this.attributes.TabIndex = 11;
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
            this.itemPropertiesSection.Size = new System.Drawing.Size(487, 22);
            this.itemPropertiesSection.TabIndex = 9;
            this.itemPropertiesSection.Text = "Item Properties";
            // 
            // itemProperties
            // 
            this.itemProperties.Location = new System.Drawing.Point(16, 1);
            this.itemProperties.Name = "itemProperties";
            this.itemProperties.Size = new System.Drawing.Size(187, 230);
            this.itemProperties.TabIndex = 0;
            // 
            // ShipSenseHashConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.attributes);
            this.Controls.Add(this.sectionAddressCasing);
            this.Controls.Add(this.itemPropertiesSection);
            this.Controls.Add(this.labelItemPropertiesInstructions);
            this.Controls.Add(this.labelAttributeInstructions);
            this.Controls.Add(this.panelProperties);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "ShipSenseHashConfigurationControl";
            this.Size = new System.Drawing.Size(494, 420);
            this.panelProperties.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelProperties;
        private ShipSenseItemPropertyControl itemProperties;
        private System.Windows.Forms.Label labelAttributeInstructions;
        private System.Windows.Forms.Label labelItemPropertiesInstructions;
        private UI.Controls.SectionTitle itemPropertiesSection;
        private UI.Controls.SectionTitle sectionAddressCasing;
        private UI.Controls.StringList attributes;
    }
}
