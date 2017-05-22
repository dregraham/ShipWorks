using System.Drawing;

namespace ShipWorks.Stores.UI.Platforms.BigCommerce
{
    partial class BigCommerceAllStoreSettingsControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private BigCommerceStoreSettingsControl bigCommerceStoreSettingsControl = null;

        private BigCommerceDownloadCriteriaControl downloadCriteriaControl = null;

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
            this.sectionTitle1 = new ShipWorks.UI.Controls.SectionTitle();
            this.bigCommerceStoreSettingsControl = new ShipWorks.Stores.UI.Platforms.BigCommerce.BigCommerceStoreSettingsControl();
            this.downloadCriteriaControl = new ShipWorks.Stores.UI.Platforms.BigCommerce.BigCommerceDownloadCriteriaControl();
            this.sectionTitleDownloadCriteria = new ShipWorks.UI.Controls.SectionTitle();
            this.SuspendLayout();
            // 
            // sectionTitle1
            // 
            this.sectionTitle1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitle1.Location = new System.Drawing.Point(0, 0);
            this.sectionTitle1.Name = "sectionTitle1";
            this.sectionTitle1.Size = new System.Drawing.Size(489, 23);
            this.sectionTitle1.TabIndex = 0;
            this.sectionTitle1.Text = "BigCommerce Store Settings";
            // 
            // bigCommerceStoreSettingsControl
            // 
            this.bigCommerceStoreSettingsControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bigCommerceStoreSettingsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bigCommerceStoreSettingsControl.Location = new System.Drawing.Point(0, 20);
            this.bigCommerceStoreSettingsControl.Name = "bigCommerceStoreSettingsControl";
            this.bigCommerceStoreSettingsControl.Size = new System.Drawing.Size(489, 45);
            this.bigCommerceStoreSettingsControl.TabIndex = 0;
            // 
            // downloadCriteriaControl
            // 
            this.downloadCriteriaControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadCriteriaControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadCriteriaControl.Location = new System.Drawing.Point(0, 105);
            this.downloadCriteriaControl.Name = "downloadCriteriaControl";
            this.downloadCriteriaControl.Size = new System.Drawing.Size(489, 99);
            this.downloadCriteriaControl.TabIndex = 0;
            // 
            // sectionTitleDownloadCriteria
            // 
            this.sectionTitleDownloadCriteria.Location = new System.Drawing.Point(0, 76);
            this.sectionTitleDownloadCriteria.Name = "sectionTitleDownloadCriteria";
            this.sectionTitleDownloadCriteria.Size = new System.Drawing.Size(489, 23);
            this.sectionTitleDownloadCriteria.TabIndex = 1;
            this.sectionTitleDownloadCriteria.Text = "Download Criteria";
            // 
            // BigCommerceAllStoreSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sectionTitleDownloadCriteria);
            this.Controls.Add(this.sectionTitle1);
            this.Controls.Add(this.bigCommerceStoreSettingsControl);
            this.Controls.Add(this.downloadCriteriaControl);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "BigCommerceAllStoreSettingsControl";
            this.Size = new System.Drawing.Size(489, 212);
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Controls.SectionTitle sectionTitle1;
        private ShipWorks.UI.Controls.SectionTitle sectionTitleDownloadCriteria;

    }
}
