namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    partial class ChannelAdvisorAttributesSettingsControl
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
            this.attributes = new ShipWorks.UI.Controls.StringListControl();
            this.attributesTitle = new ShipWorks.UI.Controls.SectionTitle();
            this.labelAttributeInstructions = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // attributes
            // 
            this.attributes.AddButtonText = "Add Attribute";
            this.attributes.AutoSize = true;
            this.attributes.Location = new System.Drawing.Point(3, 58);
            this.attributes.Name = "attributes";
            this.attributes.Size = new System.Drawing.Size(530, 38);
            this.attributes.TabIndex = 15;
            // 
            // attributesTitle
            // 
            this.attributesTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.attributesTitle.Location = new System.Drawing.Point(0, 0);
            this.attributesTitle.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.attributesTitle.Name = "attributesTitle";
            this.attributesTitle.Size = new System.Drawing.Size(558, 22);
            this.attributesTitle.TabIndex = 16;
            this.attributesTitle.Text = "Attributes";
            // 
            // labelAttributeInstructions
            // 
            this.labelAttributeInstructions.Location = new System.Drawing.Point(3, 25);
            this.labelAttributeInstructions.Name = "labelAttributeInstructions";
            this.labelAttributeInstructions.Size = new System.Drawing.Size(474, 30);
            this.labelAttributeInstructions.TabIndex = 17;
            this.labelAttributeInstructions.Text = "An item attribute is something specific about the items you’re shipping. For exam" +
    "ple, a color. If you want ShipWorks to download the color of the product ordere" +
    "d, add an attribute.";
            // 
            // ChannelAdvisorStoreSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.labelAttributeInstructions);
            this.Controls.Add(this.attributesTitle);
            this.Controls.Add(this.attributes);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "ChannelAdvisorStoreSettingsControl";
            this.Size = new System.Drawing.Size(558, 99);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Controls.StringListControl attributes;
        private UI.Controls.SectionTitle attributesTitle;
        private System.Windows.Forms.Label labelAttributeInstructions;
    }
}
