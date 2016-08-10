namespace ShipWorks.ApplicationCore.Options
{
    partial class OptionPageAdvanced
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
            this.addressCasing = new System.Windows.Forms.CheckBox();
            this.sectionLogIn = new ShipWorks.UI.Controls.SectionTitle();
            this.labelAddressCasing = new System.Windows.Forms.Label();
            this.labelLogOnMethod = new System.Windows.Forms.Label();
            this.sectionAddressCasing = new ShipWorks.UI.Controls.SectionTitle();
            this.logOnMethod = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.compareCustomerAddress = new System.Windows.Forms.CheckBox();
            this.compareCustomerEmail = new System.Windows.Forms.CheckBox();
            this.sectionCustomerUpdating = new ShipWorks.UI.Controls.SectionTitle();
            this.sectionCustomerMatching = new ShipWorks.UI.Controls.SectionTitle();
            this.updateCustomerShipping = new System.Windows.Forms.CheckBox();
            this.updateCustomerBilling = new System.Windows.Forms.CheckBox();
            this.labelCustomerUpdating = new System.Windows.Forms.Label();
            this.labelCustomerMatching2 = new System.Windows.Forms.Label();
            this.labelCustomerMatching1 = new System.Windows.Forms.Label();
            this.sectionAuditing = new ShipWorks.UI.Controls.SectionTitle();
            this.auditNewOrders = new System.Windows.Forms.CheckBox();
            this.infotipAudit = new ShipWorks.UI.Controls.InfoTip();
            this.auditDeletedOrders = new System.Windows.Forms.CheckBox();
            this.sectionShipSense = new ShipWorks.UI.Controls.SectionTitle();
            this.clearKnowledgebase = new System.Windows.Forms.Button();
            this.enableShipSense = new System.Windows.Forms.CheckBox();
            this.labelShipSenseInfo = new System.Windows.Forms.Label();
            this.editShipSenseSettings = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.autoCreateShipments = new System.Windows.Forms.CheckBox();
            this.sectionShipmentCreation = new ShipWorks.UI.Controls.SectionTitle();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.orderShippingAddressChanged = new System.Windows.Forms.ComboBox();
            this.orderBillingAddressChanged = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.shipmentEditLimit = new System.Windows.Forms.ComboBox();
            this.infoTip1 = new ShipWorks.UI.Controls.InfoTip();
            this.SuspendLayout();
            // 
            // addressCasing
            // 
            this.addressCasing.AutoSize = true;
            this.addressCasing.Location = new System.Drawing.Point(32, 150);
            this.addressCasing.Name = "addressCasing";
            this.addressCasing.Size = new System.Drawing.Size(270, 17);
            this.addressCasing.TabIndex = 5;
            this.addressCasing.Text = "Convert all names and addresses to proper casing.";
            this.addressCasing.UseVisualStyleBackColor = true;
            // 
            // sectionLogIn
            // 
            this.sectionLogIn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionLogIn.Location = new System.Drawing.Point(10, 10);
            this.sectionLogIn.Name = "sectionLogIn";
            this.sectionLogIn.Size = new System.Drawing.Size(481, 22);
            this.sectionLogIn.TabIndex = 0;
            this.sectionLogIn.Text = "User Log On";
            // 
            // labelAddressCasing
            // 
            this.labelAddressCasing.Location = new System.Drawing.Point(17, 106);
            this.labelAddressCasing.Name = "labelAddressCasing";
            this.labelAddressCasing.Size = new System.Drawing.Size(350, 41);
            this.labelAddressCasing.TabIndex = 4;
            this.labelAddressCasing.Text = "When downloading orders ShipWorks can cleanup names and addresses entered by the " +
    "customer.  For instance, \"123 elm street\" would become \"123 Elm Street\".";
            // 
            // labelLogOnMethod
            // 
            this.labelLogOnMethod.Location = new System.Drawing.Point(21, 41);
            this.labelLogOnMethod.Name = "labelLogOnMethod";
            this.labelLogOnMethod.Size = new System.Drawing.Size(84, 13);
            this.labelLogOnMethod.TabIndex = 1;
            this.labelLogOnMethod.Text = "Log On Method:";
            // 
            // sectionAddressCasing
            // 
            this.sectionAddressCasing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionAddressCasing.Location = new System.Drawing.Point(10, 73);
            this.sectionAddressCasing.Name = "sectionAddressCasing";
            this.sectionAddressCasing.Size = new System.Drawing.Size(481, 22);
            this.sectionAddressCasing.TabIndex = 3;
            this.sectionAddressCasing.Text = "Address Cleanup";
            // 
            // logOnMethod
            // 
            this.logOnMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.logOnMethod.FormattingEnabled = true;
            this.logOnMethod.Items.AddRange(new object[] {
            "Type username",
            "Select username from dropdown"});
            this.logOnMethod.Location = new System.Drawing.Point(111, 38);
            this.logOnMethod.Name = "logOnMethod";
            this.logOnMethod.Size = new System.Drawing.Size(191, 21);
            this.logOnMethod.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(17, 290);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(385, 30);
            this.label1.TabIndex = 9;
            this.label1.Text = "In these cases you can configure what else ShipWorks should check to determine if" +
    " its the same customer as another order.";
            // 
            // compareCustomerAddress
            // 
            this.compareCustomerAddress.AutoSize = true;
            this.compareCustomerAddress.Location = new System.Drawing.Point(34, 346);
            this.compareCustomerAddress.Name = "compareCustomerAddress";
            this.compareCustomerAddress.Size = new System.Drawing.Size(174, 17);
            this.compareCustomerAddress.TabIndex = 11;
            this.compareCustomerAddress.Text = "Compare billing mailing address";
            this.compareCustomerAddress.UseVisualStyleBackColor = true;
            // 
            // compareCustomerEmail
            // 
            this.compareCustomerEmail.AutoSize = true;
            this.compareCustomerEmail.Location = new System.Drawing.Point(34, 326);
            this.compareCustomerEmail.Name = "compareCustomerEmail";
            this.compareCustomerEmail.Size = new System.Drawing.Size(166, 17);
            this.compareCustomerEmail.TabIndex = 10;
            this.compareCustomerEmail.Text = "Compare billing email address";
            this.compareCustomerEmail.UseVisualStyleBackColor = true;
            // 
            // sectionCustomerUpdating
            // 
            this.sectionCustomerUpdating.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionCustomerUpdating.Location = new System.Drawing.Point(10, 374);
            this.sectionCustomerUpdating.Name = "sectionCustomerUpdating";
            this.sectionCustomerUpdating.Size = new System.Drawing.Size(481, 22);
            this.sectionCustomerUpdating.TabIndex = 12;
            this.sectionCustomerUpdating.Text = "Customer Updating";
            // 
            // sectionCustomerMatching
            // 
            this.sectionCustomerMatching.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionCustomerMatching.Location = new System.Drawing.Point(10, 178);
            this.sectionCustomerMatching.Name = "sectionCustomerMatching";
            this.sectionCustomerMatching.Size = new System.Drawing.Size(481, 22);
            this.sectionCustomerMatching.TabIndex = 6;
            this.sectionCustomerMatching.Text = "Customer Matching";
            // 
            // updateCustomerShipping
            // 
            this.updateCustomerShipping.AutoSize = true;
            this.updateCustomerShipping.Location = new System.Drawing.Point(34, 446);
            this.updateCustomerShipping.Name = "updateCustomerShipping";
            this.updateCustomerShipping.Size = new System.Drawing.Size(346, 17);
            this.updateCustomerShipping.TabIndex = 15;
            this.updateCustomerShipping.Text = "Update the shipping email and mailing address from the new order.";
            this.updateCustomerShipping.UseVisualStyleBackColor = true;
            // 
            // updateCustomerBilling
            // 
            this.updateCustomerBilling.AutoSize = true;
            this.updateCustomerBilling.Location = new System.Drawing.Point(34, 426);
            this.updateCustomerBilling.Name = "updateCustomerBilling";
            this.updateCustomerBilling.Size = new System.Drawing.Size(333, 17);
            this.updateCustomerBilling.TabIndex = 14;
            this.updateCustomerBilling.Text = "Update the billing email and mailing address from the new order.";
            this.updateCustomerBilling.UseVisualStyleBackColor = true;
            // 
            // labelCustomerUpdating
            // 
            this.labelCustomerUpdating.AutoSize = true;
            this.labelCustomerUpdating.Location = new System.Drawing.Point(17, 406);
            this.labelCustomerUpdating.Name = "labelCustomerUpdating";
            this.labelCustomerUpdating.Size = new System.Drawing.Size(388, 13);
            this.labelCustomerUpdating.TabIndex = 13;
            this.labelCustomerUpdating.Text = "When ShipWorks determines that an order was placed by an existing customer:";
            // 
            // labelCustomerMatching2
            // 
            this.labelCustomerMatching2.Location = new System.Drawing.Point(17, 256);
            this.labelCustomerMatching2.Name = "labelCustomerMatching2";
            this.labelCustomerMatching2.Size = new System.Drawing.Size(383, 34);
            this.labelCustomerMatching2.TabIndex = 8;
            this.labelCustomerMatching2.Text = "Some stores allow customers to checkout without a registered account and there is" +
    " no online ID for ShipWorks to check.";
            // 
            // labelCustomerMatching1
            // 
            this.labelCustomerMatching1.Location = new System.Drawing.Point(17, 209);
            this.labelCustomerMatching1.Name = "labelCustomerMatching1";
            this.labelCustomerMatching1.Size = new System.Drawing.Size(374, 43);
            this.labelCustomerMatching1.TabIndex = 7;
            this.labelCustomerMatching1.Text = "ShipWorks determines which orders belong to the same customer by checking the onl" +
    "ine ID of the customer.  For instance, for eBay this would be the user ID or for" +
    " osCommerce it would be the customer #.";
            // 
            // sectionAuditing
            // 
            this.sectionAuditing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionAuditing.Location = new System.Drawing.Point(10, 544);
            this.sectionAuditing.Name = "sectionAuditing";
            this.sectionAuditing.Size = new System.Drawing.Size(481, 22);
            this.sectionAuditing.TabIndex = 16;
            this.sectionAuditing.Text = "Auditing";
            // 
            // auditNewOrders
            // 
            this.auditNewOrders.AutoSize = true;
            this.auditNewOrders.Location = new System.Drawing.Point(35, 574);
            this.auditNewOrders.Name = "auditNewOrders";
            this.auditNewOrders.Size = new System.Drawing.Size(249, 17);
            this.auditNewOrders.TabIndex = 17;
            this.auditNewOrders.Text = "Audit full details of new customers and orders.";
            this.auditNewOrders.UseVisualStyleBackColor = true;
            // 
            // infotipAudit
            // 
            this.infotipAudit.Caption = "Auditing all new order data uses a significant amount of space in the database.\r\n" +
    "\r\nAny edits are still audited, including both the original and updated values.";
            this.infotipAudit.Location = new System.Drawing.Point(282, 575);
            this.infotipAudit.Name = "infotipAudit";
            this.infotipAudit.Size = new System.Drawing.Size(12, 12);
            this.infotipAudit.TabIndex = 33;
            this.infotipAudit.Title = "Audit New Orders";
            // 
            // auditDeletedOrders
            // 
            this.auditDeletedOrders.AutoSize = true;
            this.auditDeletedOrders.Location = new System.Drawing.Point(35, 597);
            this.auditDeletedOrders.Name = "auditDeletedOrders";
            this.auditDeletedOrders.Size = new System.Drawing.Size(265, 17);
            this.auditDeletedOrders.TabIndex = 34;
            this.auditDeletedOrders.Text = "Audit full details of deleted customers and orders.";
            this.auditDeletedOrders.UseVisualStyleBackColor = true;
            // 
            // sectionShipSense
            // 
            this.sectionShipSense.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionShipSense.Location = new System.Drawing.Point(10, 625);
            this.sectionShipSense.Name = "sectionShipSense";
            this.sectionShipSense.Size = new System.Drawing.Size(481, 22);
            this.sectionShipSense.TabIndex = 17;
            this.sectionShipSense.Text = "ShipSense";
            // 
            // clearKnowledgebase
            // 
            this.clearKnowledgebase.Image = global::ShipWorks.Properties.Resources.delete16;
            this.clearKnowledgebase.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.clearKnowledgebase.Location = new System.Drawing.Point(32, 819);
            this.clearKnowledgebase.Name = "clearKnowledgebase";
            this.clearKnowledgebase.Size = new System.Drawing.Size(168, 23);
            this.clearKnowledgebase.TabIndex = 37;
            this.clearKnowledgebase.Text = "Reset all learning";
            this.clearKnowledgebase.UseVisualStyleBackColor = true;
            this.clearKnowledgebase.Click += new System.EventHandler(this.OnClearKnowledgebase);
            // 
            // enableShipSense
            // 
            this.enableShipSense.AutoSize = true;
            this.enableShipSense.Location = new System.Drawing.Point(35, 692);
            this.enableShipSense.Name = "enableShipSense";
            this.enableShipSense.Size = new System.Drawing.Size(348, 17);
            this.enableShipSense.TabIndex = 35;
            this.enableShipSense.Text = "Automatically configure my shipments based on my shipping history";
            this.enableShipSense.UseVisualStyleBackColor = true;
            // 
            // labelShipSenseInfo
            // 
            this.labelShipSenseInfo.Location = new System.Drawing.Point(17, 657);
            this.labelShipSenseInfo.Name = "labelShipSenseInfo";
            this.labelShipSenseInfo.Size = new System.Drawing.Size(423, 40);
            this.labelShipSenseInfo.TabIndex = 36;
            this.labelShipSenseInfo.Text = "ShipWorks monitors your shipping, remember what you do, and then automatically se" +
    "t the weights, dimensions, and customs information of future shipments.";
            // 
            // editShipSenseSettings
            // 
            this.editShipSenseSettings.Image = global::ShipWorks.Properties.Resources.edit16;
            this.editShipSenseSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.editShipSenseSettings.Location = new System.Drawing.Point(33, 759);
            this.editShipSenseSettings.Name = "editShipSenseSettings";
            this.editShipSenseSettings.Size = new System.Drawing.Size(167, 23);
            this.editShipSenseSettings.TabIndex = 38;
            this.editShipSenseSettings.Text = "Configure ShipSense...";
            this.editShipSenseSettings.UseVisualStyleBackColor = true;
            this.editShipSenseSettings.Click += new System.EventHandler(this.OnEditShipSenseClick);
            // 
            // button1
            // 
            this.button1.Image = global::ShipWorks.Properties.Resources.arrows_green_static;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(32, 849);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(168, 23);
            this.button1.TabIndex = 39;
            this.button1.Text = "Relearn from history";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnReloadKnowledgebase);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(17, 723);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(352, 32);
            this.label2.TabIndex = 40;
            this.label2.Text = "ShipWorks learns by comparing similar orders.  You can control what order informa" +
    "tion ShipSense uses to consider to orders as similar.";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(17, 797);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(423, 20);
            this.label3.TabIndex = 41;
            this.label3.Text = "If you want to start over (we\'re not sure why you would), here you go.";
            // 
            // autoCreateShipments
            // 
            this.autoCreateShipments.AutoSize = true;
            this.autoCreateShipments.Location = new System.Drawing.Point(35, 919);
            this.autoCreateShipments.Name = "autoCreateShipments";
            this.autoCreateShipments.Size = new System.Drawing.Size(281, 17);
            this.autoCreateShipments.TabIndex = 43;
            this.autoCreateShipments.Text = "Automatically create shipment when order is selected";
            this.autoCreateShipments.UseVisualStyleBackColor = true;
            // 
            // sectionShipmentCreation
            // 
            this.sectionShipmentCreation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionShipmentCreation.Location = new System.Drawing.Point(10, 888);
            this.sectionShipmentCreation.Name = "sectionShipmentCreation";
            this.sectionShipmentCreation.Size = new System.Drawing.Size(481, 22);
            this.sectionShipmentCreation.TabIndex = 44;
            this.sectionShipmentCreation.Text = "Shipment Editing";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 466);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(290, 13);
            this.label4.TabIndex = 45;
            this.label4.Text = "When ShipWorks downloads an order with updated details:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 485);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(192, 13);
            this.label5.TabIndex = 48;
            this.label5.Text = "When the billing address has changed:";
            // 
            // orderShippingAddressChanged
            // 
            this.orderShippingAddressChanged.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.orderShippingAddressChanged.FormattingEnabled = true;
            this.orderShippingAddressChanged.Items.AddRange(new object[] {
            "Type username",
            "Select username from dropdown"});
            this.orderShippingAddressChanged.Location = new System.Drawing.Point(242, 509);
            this.orderShippingAddressChanged.Name = "orderShippingAddressChanged";
            this.orderShippingAddressChanged.Size = new System.Drawing.Size(249, 21);
            this.orderShippingAddressChanged.TabIndex = 49;
            // 
            // orderBillingAddressChanged
            // 
            this.orderBillingAddressChanged.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.orderBillingAddressChanged.FormattingEnabled = true;
            this.orderBillingAddressChanged.Items.AddRange(new object[] {
            "Type username",
            "Select username from dropdown"});
            this.orderBillingAddressChanged.Location = new System.Drawing.Point(242, 482);
            this.orderBillingAddressChanged.Name = "orderBillingAddressChanged";
            this.orderBillingAddressChanged.Size = new System.Drawing.Size(249, 21);
            this.orderBillingAddressChanged.TabIndex = 50;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 512);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(205, 13);
            this.label6.TabIndex = 51;
            this.label6.Text = "When the shipping address has changed:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(32, 945);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(299, 13);
            this.label7.TabIndex = 53;
            this.label7.Text = "Maximum number of shipments that can be edited in a batch:";
            // 
            // shipmentEditLimit
            // 
            this.shipmentEditLimit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.shipmentEditLimit.FormattingEnabled = true;
            this.shipmentEditLimit.Items.AddRange(new object[] {
            "Type username",
            "Select username from dropdown"});
            this.shipmentEditLimit.Location = new System.Drawing.Point(337, 942);
            this.shipmentEditLimit.Name = "shipmentEditLimit";
            this.shipmentEditLimit.Size = new System.Drawing.Size(87, 21);
            this.shipmentEditLimit.TabIndex = 52;
            // 
            // infoTip1
            // 
            this.infoTip1.Caption = "Larger batch sizes mean more shipments can be processed at once, but require more" +
    " system resources.";
            this.infoTip1.Location = new System.Drawing.Point(430, 946);
            this.infoTip1.Name = "infoTip1";
            this.infoTip1.Size = new System.Drawing.Size(12, 12);
            this.infoTip1.TabIndex = 54;
            this.infoTip1.Title = "Maximum Batch Size";
            // 
            // OptionPageAdvanced
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScrollMargin = new System.Drawing.Size(0, 8);
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.infoTip1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.shipmentEditLimit);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.orderBillingAddressChanged);
            this.Controls.Add(this.orderShippingAddressChanged);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.sectionShipmentCreation);
            this.Controls.Add(this.autoCreateShipments);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.editShipSenseSettings);
            this.Controls.Add(this.clearKnowledgebase);
            this.Controls.Add(this.enableShipSense);
            this.Controls.Add(this.labelShipSenseInfo);
            this.Controls.Add(this.sectionShipSense);
            this.Controls.Add(this.auditDeletedOrders);
            this.Controls.Add(this.infotipAudit);
            this.Controls.Add(this.auditNewOrders);
            this.Controls.Add(this.sectionAuditing);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.compareCustomerAddress);
            this.Controls.Add(this.compareCustomerEmail);
            this.Controls.Add(this.sectionCustomerUpdating);
            this.Controls.Add(this.sectionCustomerMatching);
            this.Controls.Add(this.updateCustomerShipping);
            this.Controls.Add(this.updateCustomerBilling);
            this.Controls.Add(this.labelCustomerUpdating);
            this.Controls.Add(this.labelCustomerMatching2);
            this.Controls.Add(this.labelCustomerMatching1);
            this.Controls.Add(this.addressCasing);
            this.Controls.Add(this.sectionLogIn);
            this.Controls.Add(this.labelAddressCasing);
            this.Controls.Add(this.labelLogOnMethod);
            this.Controls.Add(this.sectionAddressCasing);
            this.Controls.Add(this.logOnMethod);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "OptionPageAdvanced";
            this.Size = new System.Drawing.Size(505, 981);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox addressCasing;
        private ShipWorks.UI.Controls.SectionTitle sectionLogIn;
        private System.Windows.Forms.Label labelAddressCasing;
        private System.Windows.Forms.Label labelLogOnMethod;
        private ShipWorks.UI.Controls.SectionTitle sectionAddressCasing;
        private System.Windows.Forms.ComboBox logOnMethod;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox compareCustomerAddress;
        private System.Windows.Forms.CheckBox compareCustomerEmail;
        private ShipWorks.UI.Controls.SectionTitle sectionCustomerUpdating;
        private ShipWorks.UI.Controls.SectionTitle sectionCustomerMatching;
        private System.Windows.Forms.CheckBox updateCustomerShipping;
        private System.Windows.Forms.CheckBox updateCustomerBilling;
        private System.Windows.Forms.Label labelCustomerUpdating;
        private System.Windows.Forms.Label labelCustomerMatching2;
        private System.Windows.Forms.Label labelCustomerMatching1;
        private UI.Controls.SectionTitle sectionAuditing;
        private System.Windows.Forms.CheckBox auditNewOrders;
        private UI.Controls.InfoTip infotipAudit;
        private System.Windows.Forms.CheckBox auditDeletedOrders;
        private UI.Controls.SectionTitle sectionShipSense;
        private System.Windows.Forms.Button clearKnowledgebase;
        private System.Windows.Forms.CheckBox enableShipSense;
        private System.Windows.Forms.Label labelShipSenseInfo;
        private System.Windows.Forms.Button editShipSenseSettings;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox autoCreateShipments;
        private UI.Controls.SectionTitle sectionShipmentCreation;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox orderShippingAddressChanged;
        private System.Windows.Forms.ComboBox orderBillingAddressChanged;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox shipmentEditLimit;
        private UI.Controls.InfoTip infoTip1;
    }
}
