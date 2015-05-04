namespace ShipWorks.Stores.Platforms.Ebay
{
    partial class EbayStoreSettingsControl
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
            this.downloadDetailsCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.downloadPayPalCheckBox = new System.Windows.Forms.CheckBox();
            this.credentialsPanel = new System.Windows.Forms.Panel();
            this.configureButton = new System.Windows.Forms.Button();
            this.credentialDetailsTextBox = new System.Windows.Forms.TextBox();
            this.credentialTypeTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.internationalComboBox = new System.Windows.Forms.ComboBox();
            this.domesticComboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.acceptedPaymentsControl = new ShipWorks.Stores.Platforms.Ebay.OrderCombining.AcceptedPaymentsControl();
            this.sectionInvoices = new ShipWorks.UI.Controls.SectionTitle();
            this.sectionDownloadDetails = new ShipWorks.UI.Controls.SectionTitle();
            this.downloadOlderOrders = new System.Windows.Forms.CheckBox();
            this.credentialsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // downloadDetailsCheckBox
            // 
            this.downloadDetailsCheckBox.AutoSize = true;
            this.downloadDetailsCheckBox.Location = new System.Drawing.Point(23, 31);
            this.downloadDetailsCheckBox.Name = "downloadDetailsCheckBox";
            this.downloadDetailsCheckBox.Size = new System.Drawing.Size(285, 17);
            this.downloadDetailsCheckBox.TabIndex = 0;
            this.downloadDetailsCheckBox.Text = "Download image URLs (to display images in templates)";
            this.downloadDetailsCheckBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(43, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(295, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "(This will add significant amounts of time to each download.)";
            // 
            // downloadPayPalCheckBox
            // 
            this.downloadPayPalCheckBox.AutoSize = true;
            this.downloadPayPalCheckBox.Location = new System.Drawing.Point(23, 95);
            this.downloadPayPalCheckBox.Name = "downloadPayPalCheckBox";
            this.downloadPayPalCheckBox.Size = new System.Drawing.Size(268, 17);
            this.downloadPayPalCheckBox.TabIndex = 2;
            this.downloadPayPalCheckBox.Text = "Download PayPal details for items paid with PayPal";
            this.downloadPayPalCheckBox.UseVisualStyleBackColor = true;
            this.downloadPayPalCheckBox.CheckedChanged += new System.EventHandler(this.OnPayPalDetailsCheckedChanged);
            // 
            // credentialsPanel
            // 
            this.credentialsPanel.Controls.Add(this.configureButton);
            this.credentialsPanel.Controls.Add(this.credentialDetailsTextBox);
            this.credentialsPanel.Controls.Add(this.credentialTypeTextBox);
            this.credentialsPanel.Controls.Add(this.label3);
            this.credentialsPanel.Controls.Add(this.label2);
            this.credentialsPanel.Location = new System.Drawing.Point(53, 115);
            this.credentialsPanel.Name = "credentialsPanel";
            this.credentialsPanel.Size = new System.Drawing.Size(393, 57);
            this.credentialsPanel.TabIndex = 8;
            // 
            // configureButton
            // 
            this.configureButton.Location = new System.Drawing.Point(301, 3);
            this.configureButton.Name = "configureButton";
            this.configureButton.Size = new System.Drawing.Size(84, 23);
            this.configureButton.TabIndex = 12;
            this.configureButton.Text = "&Configure...";
            this.configureButton.UseVisualStyleBackColor = true;
            this.configureButton.Click += new System.EventHandler(this.OnConfigureClick);
            // 
            // credentialDetailsTextBox
            // 
            this.credentialDetailsTextBox.Location = new System.Drawing.Point(101, 30);
            this.credentialDetailsTextBox.Name = "credentialDetailsTextBox";
            this.credentialDetailsTextBox.ReadOnly = true;
            this.credentialDetailsTextBox.Size = new System.Drawing.Size(194, 21);
            this.credentialDetailsTextBox.TabIndex = 11;
            // 
            // credentialTypeTextBox
            // 
            this.credentialTypeTextBox.Location = new System.Drawing.Point(101, 3);
            this.credentialTypeTextBox.Name = "credentialTypeTextBox";
            this.credentialTypeTextBox.ReadOnly = true;
            this.credentialTypeTextBox.Size = new System.Drawing.Size(194, 21);
            this.credentialTypeTextBox.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(52, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Details:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Credential Type:";
            // 
            // internationalComboBox
            // 
            this.internationalComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.internationalComboBox.FormattingEnabled = true;
            this.internationalComboBox.Location = new System.Drawing.Point(181, 373);
            this.internationalComboBox.Name = "internationalComboBox";
            this.internationalComboBox.Size = new System.Drawing.Size(238, 21);
            this.internationalComboBox.TabIndex = 16;
            // 
            // domesticComboBox
            // 
            this.domesticComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.domesticComboBox.FormattingEnabled = true;
            this.domesticComboBox.Location = new System.Drawing.Point(181, 346);
            this.domesticComboBox.Name = "domesticComboBox";
            this.domesticComboBox.Size = new System.Drawing.Size(238, 21);
            this.domesticComboBox.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 377);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(154, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "International Shipping Service:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 350);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(135, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Domestic Shipping Service:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 211);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(155, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Your accepted payment types:";
            // 
            // acceptedPaymentsControl
            // 
            this.acceptedPaymentsControl.Location = new System.Drawing.Point(33, 230);
            this.acceptedPaymentsControl.Name = "acceptedPaymentsControl";
            this.acceptedPaymentsControl.Size = new System.Drawing.Size(385, 108);
            this.acceptedPaymentsControl.TabIndex = 11;
            // 
            // sectionInvoices
            // 
            this.sectionInvoices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionInvoices.Location = new System.Drawing.Point(0, 179);
            this.sectionInvoices.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.sectionInvoices.Name = "sectionInvoices";
            this.sectionInvoices.Size = new System.Drawing.Size(535, 22);
            this.sectionInvoices.TabIndex = 12;
            this.sectionInvoices.Text = "Creating eBay Invoices";
            // 
            // sectionDownloadDetails
            // 
            this.sectionDownloadDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.sectionDownloadDetails.Location = new System.Drawing.Point(0, 0);
            this.sectionDownloadDetails.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.sectionDownloadDetails.Name = "sectionDownloadDetails";
            this.sectionDownloadDetails.Size = new System.Drawing.Size(535, 22);
            this.sectionDownloadDetails.TabIndex = 17;
            this.sectionDownloadDetails.Text = "Download Details";
            // 
            // downloadOlderOrders
            // 
            this.downloadOlderOrders.AutoSize = true;
            this.downloadOlderOrders.Location = new System.Drawing.Point(23, 72);
            this.downloadOlderOrders.Name = "downloadOlderOrders";
            this.downloadOlderOrders.Size = new System.Drawing.Size(146, 17);
            this.downloadOlderOrders.TabIndex = 18;
            this.downloadOlderOrders.Text = "Redownload older orders";
            this.downloadOlderOrders.UseVisualStyleBackColor = true;
            // 
            // EbayStoreSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.downloadOlderOrders);
            this.Controls.Add(this.sectionDownloadDetails);
            this.Controls.Add(this.internationalComboBox);
            this.Controls.Add(this.sectionInvoices);
            this.Controls.Add(this.domesticComboBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.credentialsPanel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.downloadPayPalCheckBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.acceptedPaymentsControl);
            this.Controls.Add(this.downloadDetailsCheckBox);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.MinimumSize = new System.Drawing.Size(535, 262);
            this.Name = "EbayStoreSettingsControl";
            this.Size = new System.Drawing.Size(535, 409);
            this.Load += new System.EventHandler(this.OnLoad);
            this.credentialsPanel.ResumeLayout(false);
            this.credentialsPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox downloadDetailsCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox downloadPayPalCheckBox;
        private System.Windows.Forms.Panel credentialsPanel;
        private System.Windows.Forms.Button configureButton;
        private System.Windows.Forms.TextBox credentialDetailsTextBox;
        private System.Windows.Forms.TextBox credentialTypeTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private ShipWorks.Stores.Platforms.Ebay.OrderCombining.AcceptedPaymentsControl acceptedPaymentsControl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox internationalComboBox;
        private System.Windows.Forms.ComboBox domesticComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private ShipWorks.UI.Controls.SectionTitle sectionInvoices;
        private ShipWorks.UI.Controls.SectionTitle sectionDownloadDetails;
        private System.Windows.Forms.CheckBox downloadOlderOrders;
    }
}
