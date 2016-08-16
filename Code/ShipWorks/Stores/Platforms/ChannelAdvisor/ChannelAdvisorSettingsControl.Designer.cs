namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    partial class ChannelAdvisorSettingsControl
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
            this.consolidator = new ShipWorks.Stores.Platforms.ChannelAdvisor.ChannelAdvisorConsolidatorSettingsControl();
            this.attributes = new ShipWorks.Stores.Platforms.ChannelAdvisor.ChannelAdvisorAttributesSettingsControl();
            this.amazon = new ShipWorks.Stores.Platforms.ChannelAdvisor.ChannelAdvisorAmazonSettingsControl();
            this.SuspendLayout();
            // 
            // consolidator
            // 
            this.consolidator.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.consolidator.Location = new System.Drawing.Point(0, 242);
            this.consolidator.Name = "consolidator";
            this.consolidator.Size = new System.Drawing.Size(582, 69);
            this.consolidator.TabIndex = 1;
            // 
            // attributes
            // 
            this.attributes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.attributes.AutoSize = true;
            this.attributes.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.attributes.Location = new System.Drawing.Point(0, 0);
            this.attributes.Margin = new System.Windows.Forms.Padding(0);
            this.attributes.Name = "attributes";
            this.attributes.Size = new System.Drawing.Size(582, 99);
            this.attributes.TabIndex = 0;
            this.attributes.SizeChanged += new System.EventHandler(this.OnAttributesResize);
            // 
            // amazon
            // 
            this.amazon.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.amazon.Location = new System.Drawing.Point(0, 113);
            this.amazon.Name = "amazon";
            this.amazon.Size = new System.Drawing.Size(582, 129);
            this.amazon.TabIndex = 2;
            // 
            // ChannelAdvisorSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.amazon);
            this.Controls.Add(this.consolidator);
            this.Controls.Add(this.attributes);
            this.Name = "ChannelAdvisorSettingsControl";
            this.Size = new System.Drawing.Size(582, 311);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ChannelAdvisorAttributesSettingsControl attributes;
        private ChannelAdvisorConsolidatorSettingsControl consolidator;
        private ChannelAdvisorAmazonSettingsControl amazon;
    }
}
