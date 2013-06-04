namespace ShipWorks.Stores.Platforms.NetworkSolutions
{
    partial class NetworkSolutionsStoreSettingsControl
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
            this.sectionDownloadDetails = new ShipWorks.UI.Controls.SectionTitle();
            this.label1 = new System.Windows.Forms.Label();
            this.statuses = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // sectionDownloadDetails
            // 
            this.sectionDownloadDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.sectionDownloadDetails.Location = new System.Drawing.Point(0, 0);
            this.sectionDownloadDetails.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.sectionDownloadDetails.Name = "sectionDownloadDetails";
            this.sectionDownloadDetails.Size = new System.Drawing.Size(440, 22);
            this.sectionDownloadDetails.TabIndex = 18;
            this.sectionDownloadDetails.Text = "Order Status";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(222, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Download Network Solutions orders of status:";
            // 
            // statuses
            // 
            this.statuses.CheckOnClick = true;
            this.statuses.FormattingEnabled = true;
            this.statuses.Location = new System.Drawing.Point(40, 52);
            this.statuses.Name = "statuses";
            this.statuses.Size = new System.Drawing.Size(216, 124);
            this.statuses.TabIndex = 20;
            // 
            // NetworkSolutionsStoreSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statuses);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sectionDownloadDetails);
            this.Name = "NetworkSolutionsStoreSettingsControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.UI.Controls.SectionTitle sectionDownloadDetails;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox statuses;

    }
}
