namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    partial class ChannelAdvisorConsolidatorSettingsControl
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
            this.consolidatorTitle = new ShipWorks.UI.Controls.SectionTitle();
            this.labelAttributeInstructions = new System.Windows.Forms.Label();
            this.consolidatorAsUsps = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // consolidatorTitle
            // 
            this.consolidatorTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.consolidatorTitle.Location = new System.Drawing.Point(0, 0);
            this.consolidatorTitle.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.consolidatorTitle.Name = "consolidatorTitle";
            this.consolidatorTitle.Size = new System.Drawing.Size(558, 22);
            this.consolidatorTitle.TabIndex = 17;
            this.consolidatorTitle.Text = "Consolidator Upload";
            // 
            // labelAttributeInstructions
            // 
            this.labelAttributeInstructions.Location = new System.Drawing.Point(2, 23);
            this.labelAttributeInstructions.Name = "labelAttributeInstructions";
            this.labelAttributeInstructions.Size = new System.Drawing.Size(553, 30);
            this.labelAttributeInstructions.TabIndex = 18;
            this.labelAttributeInstructions.Text = "Select the check box below if you want orders shipped using a consolidator to be " +
    "uploaded as USPS.";
            // 
            // consolidatorAsUsps
            // 
            this.consolidatorAsUsps.AutoSize = true;
            this.consolidatorAsUsps.Location = new System.Drawing.Point(22, 48);
            this.consolidatorAsUsps.Name = "consolidatorAsUsps";
            this.consolidatorAsUsps.Size = new System.Drawing.Size(276, 17);
            this.consolidatorAsUsps.TabIndex = 19;
            this.consolidatorAsUsps.Text = "Upload consolidator shipments as USPS IECONOMY";
            this.consolidatorAsUsps.UseVisualStyleBackColor = true;
            // 
            // ChannelAdvisorConsolidatorSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.consolidatorAsUsps);
            this.Controls.Add(this.labelAttributeInstructions);
            this.Controls.Add(this.consolidatorTitle);
            this.Name = "ChannelAdvisorConsolidatorSettingsControl";
            this.Size = new System.Drawing.Size(558, 69);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Controls.SectionTitle consolidatorTitle;
        private System.Windows.Forms.Label labelAttributeInstructions;
        private System.Windows.Forms.CheckBox consolidatorAsUsps;
    }
}
