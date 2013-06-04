namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    partial class MarketplaceAdvisorStoreSettingsControl
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
            this.flagsControl = new ShipWorks.Stores.Platforms.MarketplaceAdvisor.MarketplaceAdvisorOmsFlagsControl();
            this.sectionHeader = new ShipWorks.UI.Controls.SectionTitle();
            this.SuspendLayout();
            // 
            // flagsControl
            // 
            this.flagsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.flagsControl.Location = new System.Drawing.Point(19, 28);
            this.flagsControl.Name = "flagsControl";
            this.flagsControl.Size = new System.Drawing.Size(486, 205);
            this.flagsControl.TabIndex = 0;
            // 
            // sectionHeader
            // 
            this.sectionHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.sectionHeader.Location = new System.Drawing.Point(0, 0);
            this.sectionHeader.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.sectionHeader.Name = "sectionHeader";
            this.sectionHeader.Size = new System.Drawing.Size(495, 22);
            this.sectionHeader.TabIndex = 18;
            this.sectionHeader.Text = "Download Flags";
            // 
            // MarketplaceAdvisorStoreSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sectionHeader);
            this.Controls.Add(this.flagsControl);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "MarketplaceAdvisorStoreSettingsControl";
            this.Size = new System.Drawing.Size(495, 243);
            this.ResumeLayout(false);

        }

        #endregion

        private MarketplaceAdvisorOmsFlagsControl flagsControl;
        private ShipWorks.UI.Controls.SectionTitle sectionHeader;
    }
}
