namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    partial class ThreeDCartStoreSettingsControl
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
            this.timeZoneControl = new ShipWorks.Stores.Platforms.ThreeDCart.ThreeDCartTimeZoneControl();
            this.threeDCartDownloadCriteriaControl = new ShipWorks.Stores.Platforms.ThreeDCart.ThreeDCartDownloadCriteriaControl();
            this.sectionTitleDownloadCriteria = new ShipWorks.UI.Controls.SectionTitle();
            this.SuspendLayout();
            // 
            // sectionDownloadDetails
            // 
            this.sectionDownloadDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.sectionDownloadDetails.Location = new System.Drawing.Point(0, 0);
            this.sectionDownloadDetails.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.sectionDownloadDetails.Name = "sectionDownloadDetails";
            this.sectionDownloadDetails.Size = new System.Drawing.Size(489, 22);
            this.sectionDownloadDetails.TabIndex = 19;
            this.sectionDownloadDetails.Text = "Time Zone";
            // 
            // timeZoneControl
            // 
            this.timeZoneControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeZoneControl.Location = new System.Drawing.Point(3, 30);
            this.timeZoneControl.Name = "timeZoneControl";
            this.timeZoneControl.Size = new System.Drawing.Size(448, 88);
            this.timeZoneControl.TabIndex = 31;
            // 
            // threeDCartDownloadCriteriaControl
            // 
            this.threeDCartDownloadCriteriaControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.threeDCartDownloadCriteriaControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.threeDCartDownloadCriteriaControl.Location = new System.Drawing.Point(0, 155);
            this.threeDCartDownloadCriteriaControl.Name = "threeDCartDownloadCriteriaControl";
            this.threeDCartDownloadCriteriaControl.Size = new System.Drawing.Size(489, 134);
            this.threeDCartDownloadCriteriaControl.TabIndex = 32;
            // 
            // sectionTitleDownloadCriteria
            // 
            this.sectionTitleDownloadCriteria.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitleDownloadCriteria.Location = new System.Drawing.Point(0, 131);
            this.sectionTitleDownloadCriteria.Name = "sectionTitleDownloadCriteria";
            this.sectionTitleDownloadCriteria.Size = new System.Drawing.Size(489, 23);
            this.sectionTitleDownloadCriteria.TabIndex = 33;
            this.sectionTitleDownloadCriteria.Text = "Download Criteria";
            // 
            // ThreeDCartStoreSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sectionTitleDownloadCriteria);
            this.Controls.Add(this.timeZoneControl);
            this.Controls.Add(this.sectionDownloadDetails);
            this.Controls.Add(this.threeDCartDownloadCriteriaControl);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ThreeDCartStoreSettingsControl";
            this.Size = new System.Drawing.Size(489, 273);
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Controls.SectionTitle sectionDownloadDetails;
        private ThreeDCartTimeZoneControl timeZoneControl;
        private ThreeDCartDownloadCriteriaControl threeDCartDownloadCriteriaControl;
        private UI.Controls.SectionTitle sectionTitleDownloadCriteria;
    }
}
