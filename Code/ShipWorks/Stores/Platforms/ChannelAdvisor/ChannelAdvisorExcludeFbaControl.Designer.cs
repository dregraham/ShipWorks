namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    partial class ChannelAdvisorExcludeFbaControl
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
            this.excludeFba = new System.Windows.Forms.CheckBox();
            this.sectionHeader = new ShipWorks.UI.Controls.SectionTitle();
            this.SuspendLayout();
            // 
            // excludeFba
            // 
            this.excludeFba.AutoSize = true;
            this.excludeFba.Location = new System.Drawing.Point(19, 31);
            this.excludeFba.Name = "excludeFba";
            this.excludeFba.Size = new System.Drawing.Size(308, 17);
            this.excludeFba.TabIndex = 16;
            this.excludeFba.Text = "Do not download orders that are Fulfilled By Amazon (FBA)";
            this.excludeFba.UseVisualStyleBackColor = true;
            // 
            // sectionHeader
            // 
            this.sectionHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.sectionHeader.Location = new System.Drawing.Point(0, 0);
            this.sectionHeader.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.sectionHeader.Name = "sectionHeader";
            this.sectionHeader.Size = new System.Drawing.Size(580, 22);
            this.sectionHeader.TabIndex = 15;
            this.sectionHeader.Text = "Download Criteria";
            // 
            // ChannelAdvisorExcludeFbaControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.excludeFba);
            this.Controls.Add(this.sectionHeader);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "ChannelAdvisorExcludeFbaControl";
            this.Size = new System.Drawing.Size(580, 60);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox excludeFba;
        private UI.Controls.SectionTitle sectionHeader;
    }
}
