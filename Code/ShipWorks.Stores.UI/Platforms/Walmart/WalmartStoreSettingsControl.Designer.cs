namespace ShipWorks.Stores.UI.Platforms.Walmart
{
    partial class WalmartStoreSettingsControl
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
            this.sectionTitleDownloadCriteria = new ShipWorks.UI.Controls.SectionTitle();
            this.checkBoxDownloadModifiedOrders = new System.Windows.Forms.CheckBox();
            this.downloadModifiedOrdersNumberOfDays = new System.Windows.Forms.ComboBox();
            this.daysLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // sectionTitleDownloadCriteria
            // 
            this.sectionTitleDownloadCriteria.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitleDownloadCriteria.Location = new System.Drawing.Point(0, 0);
            this.sectionTitleDownloadCriteria.Name = "sectionTitleDownloadCriteria";
            this.sectionTitleDownloadCriteria.Size = new System.Drawing.Size(600, 23);
            this.sectionTitleDownloadCriteria.TabIndex = 0;
            this.sectionTitleDownloadCriteria.Text = "Download Criteria";
            // 
            // checkBoxDownloadModifiedOrders
            // 
            this.checkBoxDownloadModifiedOrders.AutoSize = true;
            this.checkBoxDownloadModifiedOrders.Location = new System.Drawing.Point(11, 29);
            this.checkBoxDownloadModifiedOrders.Name = "checkBoxDownloadModifiedOrders";
            this.checkBoxDownloadModifiedOrders.Size = new System.Drawing.Size(196, 17);
            this.checkBoxDownloadModifiedOrders.TabIndex = 1;
            this.checkBoxDownloadModifiedOrders.Text = "Download orders modified in the last";
            this.checkBoxDownloadModifiedOrders.UseVisualStyleBackColor = true;
            this.checkBoxDownloadModifiedOrders.CheckedChanged += new System.EventHandler(this.OnCheckBoxDownloadModifiedOrdersCheckedChanged);
            // 
            // downloadModifiedOrdersNumberOfDays
            // 
            this.downloadModifiedOrdersNumberOfDays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.downloadModifiedOrdersNumberOfDays.FormattingEnabled = true;
            this.downloadModifiedOrdersNumberOfDays.Location = new System.Drawing.Point(213, 27);
            this.downloadModifiedOrdersNumberOfDays.Name = "downloadModifiedOrdersNumberOfDays";
            this.downloadModifiedOrdersNumberOfDays.Size = new System.Drawing.Size(40, 21);
            this.downloadModifiedOrdersNumberOfDays.TabIndex = 2;
            // 
            // daysLabel
            // 
            this.daysLabel.AutoSize = true;
            this.daysLabel.Location = new System.Drawing.Point(259, 30);
            this.daysLabel.Name = "daysLabel";
            this.daysLabel.Size = new System.Drawing.Size(32, 13);
            this.daysLabel.TabIndex = 0;
            this.daysLabel.Text = "days.";
            // 
            // WalmartStoreSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.daysLabel);
            this.Controls.Add(this.checkBoxDownloadModifiedOrders);
            this.Controls.Add(this.downloadModifiedOrdersNumberOfDays);
            this.Controls.Add(this.sectionTitleDownloadCriteria);
            this.Name = "WalmartStoreSettingsControl";
            this.Size = new System.Drawing.Size(600, 58);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.UI.Controls.SectionTitle sectionTitleDownloadCriteria;
        private System.Windows.Forms.CheckBox checkBoxDownloadModifiedOrders;
        private System.Windows.Forms.ComboBox downloadModifiedOrdersNumberOfDays;
        private System.Windows.Forms.Label daysLabel;
    }
}
