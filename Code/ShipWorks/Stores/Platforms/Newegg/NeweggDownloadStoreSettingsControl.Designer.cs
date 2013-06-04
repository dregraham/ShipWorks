namespace ShipWorks.Stores.Platforms.Newegg
{
    partial class NeweggDownloadStoreSettingsControl
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.sectionHeader = new ShipWorks.UI.Controls.SectionTitle();
            this.excludeFulfilledByNewegg = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // sectionHeader
            // 
            this.sectionHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.sectionHeader.Location = new System.Drawing.Point(0, 0);
            this.sectionHeader.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.sectionHeader.Name = "sectionHeader";
            this.sectionHeader.Size = new System.Drawing.Size(440, 22);
            this.sectionHeader.TabIndex = 14;
            this.sectionHeader.Text = "Download Criteria";
            // 
            // excludeFulfilledByNewegg
            // 
            this.excludeFulfilledByNewegg.AutoSize = true;
            this.excludeFulfilledByNewegg.Location = new System.Drawing.Point(16, 37);
            this.excludeFulfilledByNewegg.Name = "excludeFulfilledByNewegg";
            this.excludeFulfilledByNewegg.Size = new System.Drawing.Size(270, 17);
            this.excludeFulfilledByNewegg.TabIndex = 19;
            this.excludeFulfilledByNewegg.Text = "Do not download orders that are fulfilled by Newegg";
            this.excludeFulfilledByNewegg.UseVisualStyleBackColor = true;
            // 
            // NeweggDownloadStoreSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.excludeFulfilledByNewegg);
            this.Controls.Add(this.sectionHeader);
            this.Name = "NeweggDownloadStoreSettingsControl";
            this.Size = new System.Drawing.Size(440, 80);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Controls.SectionTitle sectionHeader;
        private System.Windows.Forms.CheckBox excludeFulfilledByNewegg;
    }
}
