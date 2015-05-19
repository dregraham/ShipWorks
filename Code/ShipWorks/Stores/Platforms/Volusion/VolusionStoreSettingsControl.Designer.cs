namespace ShipWorks.Stores.Platforms.Volusion
{
    partial class VolusionStoreSettingsControl
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
            this.sectionTitle1 = new ShipWorks.UI.Controls.SectionTitle();
            this.label2 = new System.Windows.Forms.Label();
            this.shippingMethodsStatus = new System.Windows.Forms.Label();
            this.paymentMethodsStatus = new System.Windows.Forms.Label();
            this.refreshShippingMethodsLink = new ShipWorks.UI.Controls.LinkControl();
            this.refreshPaymentMethodsLink = new ShipWorks.UI.Controls.LinkControl();
            this.timeZoneControl = new ShipWorks.Stores.Platforms.Volusion.VolusionTimeZoneControl();
            this.sectionTitle2 = new ShipWorks.UI.Controls.SectionTitle();
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
            this.sectionDownloadDetails.Size = new System.Drawing.Size(489, 22);
            this.sectionDownloadDetails.TabIndex = 19;
            this.sectionDownloadDetails.Text = "Time Zone";
            // 
            // sectionTitle1
            // 
            this.sectionTitle1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitle1.Location = new System.Drawing.Point(0, 133);
            this.sectionTitle1.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.sectionTitle1.Name = "sectionTitle1";
            this.sectionTitle1.Size = new System.Drawing.Size(489, 22);
            this.sectionTitle1.TabIndex = 20;
            this.sectionTitle1.Text = "Shipping and Payment Methods";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(19, 167);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(467, 19);
            this.label2.TabIndex = 25;
            this.label2.Text = "ShipWorks needs the lists of shipping and payment methods from your online store." +
    "";
            // 
            // shippingMethodsStatus
            // 
            this.shippingMethodsStatus.AutoSize = true;
            this.shippingMethodsStatus.Location = new System.Drawing.Point(41, 189);
            this.shippingMethodsStatus.Name = "shippingMethodsStatus";
            this.shippingMethodsStatus.Size = new System.Drawing.Size(168, 13);
            this.shippingMethodsStatus.TabIndex = 27;
            this.shippingMethodsStatus.Text = "{0} Shipping methods are loaded.";
            // 
            // paymentMethodsStatus
            // 
            this.paymentMethodsStatus.AutoSize = true;
            this.paymentMethodsStatus.Location = new System.Drawing.Point(41, 210);
            this.paymentMethodsStatus.Name = "paymentMethodsStatus";
            this.paymentMethodsStatus.Size = new System.Drawing.Size(170, 13);
            this.paymentMethodsStatus.TabIndex = 28;
            this.paymentMethodsStatus.Text = "{0} Payment methods are loaded.";
            // 
            // refreshShippingMethodsLink
            // 
            this.refreshShippingMethodsLink.AutoSize = true;
            this.refreshShippingMethodsLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.refreshShippingMethodsLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.refreshShippingMethodsLink.ForeColor = System.Drawing.Color.Blue;
            this.refreshShippingMethodsLink.Location = new System.Drawing.Point(216, 189);
            this.refreshShippingMethodsLink.Name = "refreshShippingMethodsLink";
            this.refreshShippingMethodsLink.Size = new System.Drawing.Size(129, 13);
            this.refreshShippingMethodsLink.TabIndex = 29;
            this.refreshShippingMethodsLink.Text = "Update Shipping Methods";
            this.refreshShippingMethodsLink.Click += new System.EventHandler(this.OnUpdateShippingMethodsClick);
            // 
            // refreshPaymentMethodsLink
            // 
            this.refreshPaymentMethodsLink.AutoSize = true;
            this.refreshPaymentMethodsLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.refreshPaymentMethodsLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.refreshPaymentMethodsLink.ForeColor = System.Drawing.Color.Blue;
            this.refreshPaymentMethodsLink.Location = new System.Drawing.Point(216, 210);
            this.refreshPaymentMethodsLink.Name = "refreshPaymentMethodsLink";
            this.refreshPaymentMethodsLink.Size = new System.Drawing.Size(131, 13);
            this.refreshPaymentMethodsLink.TabIndex = 30;
            this.refreshPaymentMethodsLink.Text = "Update Payment Methods";
            this.refreshPaymentMethodsLink.Click += new System.EventHandler(this.OnUpdatePaymentMethodsClick);
            // 
            // timeZoneControl
            // 
            this.timeZoneControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeZoneControl.Location = new System.Drawing.Point(19, 30);
            this.timeZoneControl.Name = "timeZoneControl";
            this.timeZoneControl.Size = new System.Drawing.Size(448, 88);
            this.timeZoneControl.TabIndex = 31;
            // 
            // sectionTitle2
            // 
            this.sectionTitle2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitle2.Location = new System.Drawing.Point(0, 240);
            this.sectionTitle2.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.sectionTitle2.Name = "sectionTitle2";
            this.sectionTitle2.Size = new System.Drawing.Size(489, 22);
            this.sectionTitle2.TabIndex = 32;
            this.sectionTitle2.Text = "Order Status";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 275);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 13);
            this.label1.TabIndex = 33;
            this.label1.Text = "Volusion orders of status:";
            // 
            // statuses
            // 
            this.statuses.CheckOnClick = true;
            this.statuses.FormattingEnabled = true;
            this.statuses.Location = new System.Drawing.Point(44, 291);
            this.statuses.Name = "statuses";
            this.statuses.Size = new System.Drawing.Size(216, 116);
            this.statuses.TabIndex = 34;
            // 
            // VolusionStoreSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statuses);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sectionTitle2);
            this.Controls.Add(this.timeZoneControl);
            this.Controls.Add(this.refreshPaymentMethodsLink);
            this.Controls.Add(this.refreshShippingMethodsLink);
            this.Controls.Add(this.paymentMethodsStatus);
            this.Controls.Add(this.shippingMethodsStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.sectionTitle1);
            this.Controls.Add(this.sectionDownloadDetails);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "VolusionStoreSettingsControl";
            this.Size = new System.Drawing.Size(489, 429);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.UI.Controls.SectionTitle sectionDownloadDetails;
        private ShipWorks.UI.Controls.SectionTitle sectionTitle1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label shippingMethodsStatus;
        private System.Windows.Forms.Label paymentMethodsStatus;
        private ShipWorks.UI.Controls.LinkControl refreshShippingMethodsLink;
        private ShipWorks.UI.Controls.LinkControl refreshPaymentMethodsLink;
        private VolusionTimeZoneControl timeZoneControl;
        private UI.Controls.SectionTitle sectionTitle2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox statuses;
    }
}
