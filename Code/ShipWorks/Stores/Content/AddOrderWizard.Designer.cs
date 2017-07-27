namespace ShipWorks.Stores.Content
{
    partial class AddOrderWizard
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
            this.components = new System.ComponentModel.Container();
            this.wizardPageStoreCustomer = new ShipWorks.UI.Wizard.WizardPage();
            this.panelCustomer = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelOrderPreview = new System.Windows.Forms.Label();
            this.radioOrderNumberGenerate = new System.Windows.Forms.RadioButton();
            this.labelOrder = new System.Windows.Forms.Label();
            this.radioOrderNumberSpecify = new System.Windows.Forms.RadioButton();
            this.orderNumber = new ShipWorks.UI.Controls.NumericTextBox();
            this.labelOrderPrefix = new System.Windows.Forms.Label();
            this.labelOrderNumberInfo = new System.Windows.Forms.Label();
            this.orderPrefix = new System.Windows.Forms.TextBox();
            this.labelOrderPostfixInfo = new System.Windows.Forms.Label();
            this.labelOrderPrefixInfo = new System.Windows.Forms.Label();
            this.orderPostfix = new System.Windows.Forms.TextBox();
            this.labelPostfix = new System.Windows.Forms.Label();
            this.labelOrderNumber = new System.Windows.Forms.Label();
            this.labelCustomer = new System.Windows.Forms.Label();
            this.labelAutomaticCustomer = new System.Windows.Forms.Label();
            this.radioAssignCustomer = new System.Windows.Forms.RadioButton();
            this.radioAutomaticCustomer = new System.Windows.Forms.RadioButton();
            this.customerName = new System.Windows.Forms.Label();
            this.linkSelectCustomer = new System.Windows.Forms.Label();
            this.labelOrdersPlaced = new System.Windows.Forms.Label();
            this.amountSpent = new System.Windows.Forms.Label();
            this.ordersPlaced = new System.Windows.Forms.Label();
            this.labelAmountSpent = new System.Windows.Forms.Label();
            this.comboStores = new System.Windows.Forms.ComboBox();
            this.labelStore = new System.Windows.Forms.Label();
            this.wizardPageAddress = new ShipWorks.UI.Wizard.WizardPage();
            this.shipBillControl = new ShipWorks.Stores.Content.Controls.ShipBillAddressControl();
            this.wizardPageDetails = new ShipWorks.UI.Wizard.WizardPage();
            this.panelDetailsBottom = new System.Windows.Forms.Panel();
            this.labelSettings = new System.Windows.Forms.Label();
            this.panelExtras = new ShipWorks.UI.Controls.ThemeBorderPanel();
            this.linkStatus = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelShipping = new System.Windows.Forms.Label();
            this.requestedShipping = new System.Windows.Forms.TextBox();
            this.splitContainerDetails = new System.Windows.Forms.SplitContainer();
            this.labelInvoice = new System.Windows.Forms.Label();
            this.panelOrderDetail = new ShipWorks.UI.Controls.ThemeBorderPanel();
            this.invoiceControl = new ShipWorks.Stores.Content.Controls.InvoiceControl();
            this.labelNotes = new System.Windows.Forms.Label();
            this.noteControl = new ShipWorks.Stores.Content.Panels.NotesPanel();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageStoreCustomer.SuspendLayout();
            this.panelCustomer.SuspendLayout();
            this.panel1.SuspendLayout();
            this.wizardPageAddress.SuspendLayout();
            this.wizardPageDetails.SuspendLayout();
            this.panelDetailsBottom.SuspendLayout();
            this.panelExtras.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.splitContainerDetails)).BeginInit();
            this.splitContainerDetails.Panel1.SuspendLayout();
            this.splitContainerDetails.Panel2.SuspendLayout();
            this.splitContainerDetails.SuspendLayout();
            this.panelOrderDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            //
            // next
            //
            this.next.Location = new System.Drawing.Point(445, 528);
            //
            // cancel
            //
            this.cancel.Location = new System.Drawing.Point(526, 528);
            //
            // back
            //
            this.back.Location = new System.Drawing.Point(364, 528);
            //
            // mainPanel
            //
            this.mainPanel.Controls.Add(this.wizardPageStoreCustomer);
            this.mainPanel.Size = new System.Drawing.Size(613, 456);
            //
            // etchBottom
            //
            this.etchBottom.Location = new System.Drawing.Point(0, 518);
            this.etchBottom.Size = new System.Drawing.Size(617, 2);
            //
            // pictureBox
            //
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.form_blue_add;
            this.pictureBox.Location = new System.Drawing.Point(560, 3);
            //
            // topPanel
            //
            this.topPanel.Size = new System.Drawing.Size(613, 56);
            //
            // wizardPageStoreCustomer
            //
            this.wizardPageStoreCustomer.Controls.Add(this.panelCustomer);
            this.wizardPageStoreCustomer.Controls.Add(this.comboStores);
            this.wizardPageStoreCustomer.Controls.Add(this.labelStore);
            this.wizardPageStoreCustomer.Description = "Create a new order in ShipWorks.";
            this.wizardPageStoreCustomer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageStoreCustomer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageStoreCustomer.Location = new System.Drawing.Point(0, 0);
            this.wizardPageStoreCustomer.Name = "wizardPageStoreCustomer";
            this.wizardPageStoreCustomer.Size = new System.Drawing.Size(613, 456);
            this.wizardPageStoreCustomer.TabIndex = 0;
            this.wizardPageStoreCustomer.Title = "New Order";
            this.wizardPageStoreCustomer.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnInitialPageStepNext);
            //
            // panelCustomer
            //
            this.panelCustomer.Controls.Add(this.panel1);
            this.panelCustomer.Controls.Add(this.labelOrderNumber);
            this.panelCustomer.Controls.Add(this.labelCustomer);
            this.panelCustomer.Controls.Add(this.labelAutomaticCustomer);
            this.panelCustomer.Controls.Add(this.radioAssignCustomer);
            this.panelCustomer.Controls.Add(this.radioAutomaticCustomer);
            this.panelCustomer.Controls.Add(this.customerName);
            this.panelCustomer.Controls.Add(this.linkSelectCustomer);
            this.panelCustomer.Controls.Add(this.labelOrdersPlaced);
            this.panelCustomer.Controls.Add(this.amountSpent);
            this.panelCustomer.Controls.Add(this.ordersPlaced);
            this.panelCustomer.Controls.Add(this.labelAmountSpent);
            this.panelCustomer.Location = new System.Drawing.Point(23, 61);
            this.panelCustomer.Name = "panelCustomer";
            this.panelCustomer.Size = new System.Drawing.Size(509, 293);
            this.panelCustomer.TabIndex = 12;
            //
            // panel1
            //
            this.panel1.Controls.Add(this.labelOrderPreview);
            this.panel1.Controls.Add(this.radioOrderNumberGenerate);
            this.panel1.Controls.Add(this.labelOrder);
            this.panel1.Controls.Add(this.radioOrderNumberSpecify);
            this.panel1.Controls.Add(this.orderNumber);
            this.panel1.Controls.Add(this.labelOrderPrefix);
            this.panel1.Controls.Add(this.labelOrderNumberInfo);
            this.panel1.Controls.Add(this.orderPrefix);
            this.panel1.Controls.Add(this.labelOrderPostfixInfo);
            this.panel1.Controls.Add(this.labelOrderPrefixInfo);
            this.panel1.Controls.Add(this.orderPostfix);
            this.panel1.Controls.Add(this.labelPostfix);
            this.panel1.Location = new System.Drawing.Point(13, 147);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(464, 132);
            this.panel1.TabIndex = 26;
            //
            // labelOrderPreview
            //
            this.labelOrderPreview.AutoSize = true;
            this.labelOrderPreview.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelOrderPreview.Location = new System.Drawing.Point(182, 27);
            this.labelOrderPreview.Name = "labelOrderPreview";
            this.labelOrderPreview.Size = new System.Drawing.Size(41, 13);
            this.labelOrderPreview.TabIndex = 26;
            this.labelOrderPreview.Text = "125-A";
            //
            // radioOrderNumberGenerate
            //
            this.radioOrderNumberGenerate.AutoSize = true;
            this.radioOrderNumberGenerate.Location = new System.Drawing.Point(0, 3);
            this.radioOrderNumberGenerate.Name = "radioOrderNumberGenerate";
            this.radioOrderNumberGenerate.Size = new System.Drawing.Size(350, 17);
            this.radioOrderNumberGenerate.TabIndex = 0;
            this.radioOrderNumberGenerate.TabStop = true;
            this.radioOrderNumberGenerate.Text = "Automatically generate using the manual order settings of the store";
            this.radioOrderNumberGenerate.UseVisualStyleBackColor = true;
            this.radioOrderNumberGenerate.CheckedChanged += new System.EventHandler(this.OnChangeOrderNumberSource);
            //
            // labelOrder
            //
            this.labelOrder.AutoSize = true;
            this.labelOrder.Location = new System.Drawing.Point(25, 74);
            this.labelOrder.Name = "labelOrder";
            this.labelOrder.Size = new System.Drawing.Size(48, 13);
            this.labelOrder.TabIndex = 25;
            this.labelOrder.Text = "Number:";
            //
            // radioOrderNumberSpecify
            //
            this.radioOrderNumberSpecify.AutoSize = true;
            this.radioOrderNumberSpecify.Location = new System.Drawing.Point(0, 25);
            this.radioOrderNumberSpecify.Name = "radioOrderNumberSpecify";
            this.radioOrderNumberSpecify.Size = new System.Drawing.Size(179, 17);
            this.radioOrderNumberSpecify.TabIndex = 1;
            this.radioOrderNumberSpecify.TabStop = true;
            this.radioOrderNumberSpecify.Text = "Use the following order number:";
            this.radioOrderNumberSpecify.UseVisualStyleBackColor = true;
            this.radioOrderNumberSpecify.CheckedChanged += new System.EventHandler(this.OnChangeOrderNumberSource);
            //
            // orderNumber
            //
            this.orderNumber.Location = new System.Drawing.Point(76, 72);
            this.orderNumber.Name = "orderNumber";
            this.orderNumber.Size = new System.Drawing.Size(100, 21);
            this.orderNumber.TabIndex = 3;
            this.orderNumber.TextChanged += new System.EventHandler(this.OnOrderNumberChanged);
            //
            // labelOrderPrefix
            //
            this.labelOrderPrefix.AutoSize = true;
            this.labelOrderPrefix.Location = new System.Drawing.Point(33, 51);
            this.labelOrderPrefix.Name = "labelOrderPrefix";
            this.labelOrderPrefix.Size = new System.Drawing.Size(39, 13);
            this.labelOrderPrefix.TabIndex = 15;
            this.labelOrderPrefix.Text = "Prefix:";
            //
            // labelOrderNumberInfo
            //
            this.labelOrderNumberInfo.AutoSize = true;
            this.labelOrderNumberInfo.ForeColor = System.Drawing.Color.DimGray;
            this.labelOrderNumberInfo.Location = new System.Drawing.Point(182, 74);
            this.labelOrderNumberInfo.Name = "labelOrderNumberInfo";
            this.labelOrderNumberInfo.Size = new System.Drawing.Size(99, 13);
            this.labelOrderNumberInfo.TabIndex = 22;
            this.labelOrderNumberInfo.Text = "(Number, required)";
            //
            // orderPrefix
            //
            this.orderPrefix.Location = new System.Drawing.Point(76, 48);
            this.orderPrefix.MaxLength = 15;
            this.orderPrefix.Name = "orderPrefix";
            this.orderPrefix.Size = new System.Drawing.Size(100, 21);
            this.orderPrefix.TabIndex = 2;
            this.orderPrefix.TextChanged += new System.EventHandler(this.OnOrderNumberChanged);
            //
            // labelOrderPostfixInfo
            //
            this.labelOrderPostfixInfo.AutoSize = true;
            this.labelOrderPostfixInfo.ForeColor = System.Drawing.Color.DimGray;
            this.labelOrderPostfixInfo.Location = new System.Drawing.Point(182, 99);
            this.labelOrderPostfixInfo.Name = "labelOrderPostfixInfo";
            this.labelOrderPostfixInfo.Size = new System.Drawing.Size(102, 13);
            this.labelOrderPostfixInfo.TabIndex = 20;
            this.labelOrderPostfixInfo.Text = "(Any text, optional)";
            //
            // labelOrderPrefixInfo
            //
            this.labelOrderPrefixInfo.AutoSize = true;
            this.labelOrderPrefixInfo.ForeColor = System.Drawing.Color.DimGray;
            this.labelOrderPrefixInfo.Location = new System.Drawing.Point(182, 51);
            this.labelOrderPrefixInfo.Name = "labelOrderPrefixInfo";
            this.labelOrderPrefixInfo.Size = new System.Drawing.Size(102, 13);
            this.labelOrderPrefixInfo.TabIndex = 17;
            this.labelOrderPrefixInfo.Text = "(Any text, optional)";
            //
            // orderPostfix
            //
            this.orderPostfix.Location = new System.Drawing.Point(76, 96);
            this.orderPostfix.MaxLength = 15;
            this.orderPostfix.Name = "orderPostfix";
            this.orderPostfix.Size = new System.Drawing.Size(100, 21);
            this.orderPostfix.TabIndex = 4;
            this.orderPostfix.TextChanged += new System.EventHandler(this.OnOrderNumberChanged);
            //
            // labelPostfix
            //
            this.labelPostfix.AutoSize = true;
            this.labelPostfix.Location = new System.Drawing.Point(30, 99);
            this.labelPostfix.Name = "labelPostfix";
            this.labelPostfix.Size = new System.Drawing.Size(44, 13);
            this.labelPostfix.TabIndex = 18;
            this.labelPostfix.Text = "Postfix:";
            //
            // labelOrderNumber
            //
            this.labelOrderNumber.AutoSize = true;
            this.labelOrderNumber.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelOrderNumber.Location = new System.Drawing.Point(-3, 132);
            this.labelOrderNumber.Name = "labelOrderNumber";
            this.labelOrderNumber.Size = new System.Drawing.Size(86, 13);
            this.labelOrderNumber.TabIndex = 12;
            this.labelOrderNumber.Text = "Order Number";
            //
            // labelCustomer
            //
            this.labelCustomer.AutoSize = true;
            this.labelCustomer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelCustomer.Location = new System.Drawing.Point(-3, 2);
            this.labelCustomer.Name = "labelCustomer";
            this.labelCustomer.Size = new System.Drawing.Size(62, 13);
            this.labelCustomer.TabIndex = 2;
            this.labelCustomer.Text = "Customer";
            //
            // labelAutomaticCustomer
            //
            this.labelAutomaticCustomer.ForeColor = System.Drawing.Color.DimGray;
            this.labelAutomaticCustomer.Location = new System.Drawing.Point(43, 94);
            this.labelAutomaticCustomer.Name = "labelAutomaticCustomer";
            this.labelAutomaticCustomer.Size = new System.Drawing.Size(413, 33);
            this.labelAutomaticCustomer.TabIndex = 11;
            this.labelAutomaticCustomer.Text = "ShipWorks will lookup an existing customer using the address matching configured " +
                "in Advanced Options, or create a new customer if no match is found.";
            //
            // radioAssignCustomer
            //
            this.radioAssignCustomer.AutoSize = true;
            this.radioAssignCustomer.Location = new System.Drawing.Point(13, 19);
            this.radioAssignCustomer.Name = "radioAssignCustomer";
            this.radioAssignCustomer.Size = new System.Drawing.Size(73, 17);
            this.radioAssignCustomer.TabIndex = 3;
            this.radioAssignCustomer.TabStop = true;
            this.radioAssignCustomer.Text = "Assign to:";
            this.radioAssignCustomer.UseVisualStyleBackColor = true;
            this.radioAssignCustomer.CheckedChanged += new System.EventHandler(this.OnChangeCustomerSource);
            //
            // radioAutomaticCustomer
            //
            this.radioAutomaticCustomer.AutoSize = true;
            this.radioAutomaticCustomer.Location = new System.Drawing.Point(13, 74);
            this.radioAutomaticCustomer.Name = "radioAutomaticCustomer";
            this.radioAutomaticCustomer.Size = new System.Drawing.Size(217, 17);
            this.radioAutomaticCustomer.TabIndex = 10;
            this.radioAutomaticCustomer.TabStop = true;
            this.radioAutomaticCustomer.Text = "Automatically lookup or create customer";
            this.radioAutomaticCustomer.UseVisualStyleBackColor = true;
            this.radioAutomaticCustomer.CheckedChanged += new System.EventHandler(this.OnChangeCustomerSource);
            //
            // customerName
            //
            this.customerName.AutoSize = true;
            this.customerName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.customerName.ForeColor = System.Drawing.Color.Black;
            this.customerName.Location = new System.Drawing.Point(83, 21);
            this.customerName.Name = "customerName";
            this.customerName.Size = new System.Drawing.Size(126, 13);
            this.customerName.TabIndex = 4;
            this.customerName.Text = "Roger Jorgenbaskins";
            //
            // linkSelectCustomer
            //
            this.linkSelectCustomer.AutoSize = true;
            this.linkSelectCustomer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkSelectCustomer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.linkSelectCustomer.ForeColor = System.Drawing.Color.Blue;
            this.linkSelectCustomer.Location = new System.Drawing.Point(209, 21);
            this.linkSelectCustomer.Name = "linkSelectCustomer";
            this.linkSelectCustomer.Size = new System.Drawing.Size(56, 13);
            this.linkSelectCustomer.TabIndex = 9;
            this.linkSelectCustomer.Text = "Change...";
            this.linkSelectCustomer.Click += new System.EventHandler(this.OnChangeCustomer);
            //
            // labelOrdersPlaced
            //
            this.labelOrdersPlaced.AutoSize = true;
            this.labelOrdersPlaced.ForeColor = System.Drawing.Color.DimGray;
            this.labelOrdersPlaced.Location = new System.Drawing.Point(43, 40);
            this.labelOrdersPlaced.Name = "labelOrdersPlaced";
            this.labelOrdersPlaced.Size = new System.Drawing.Size(78, 13);
            this.labelOrdersPlaced.TabIndex = 5;
            this.labelOrdersPlaced.Text = "Orders placed:";
            //
            // amountSpent
            //
            this.amountSpent.AutoSize = true;
            this.amountSpent.ForeColor = System.Drawing.Color.DimGray;
            this.amountSpent.Location = new System.Drawing.Point(122, 57);
            this.amountSpent.Name = "amountSpent";
            this.amountSpent.Size = new System.Drawing.Size(41, 13);
            this.amountSpent.TabIndex = 8;
            this.amountSpent.Text = "$46.95";
            //
            // ordersPlaced
            //
            this.ordersPlaced.AutoSize = true;
            this.ordersPlaced.ForeColor = System.Drawing.Color.DimGray;
            this.ordersPlaced.Location = new System.Drawing.Point(122, 40);
            this.ordersPlaced.Name = "ordersPlaced";
            this.ordersPlaced.Size = new System.Drawing.Size(13, 13);
            this.ordersPlaced.TabIndex = 6;
            this.ordersPlaced.Text = "2";
            //
            // labelAmountSpent
            //
            this.labelAmountSpent.AutoSize = true;
            this.labelAmountSpent.ForeColor = System.Drawing.Color.DimGray;
            this.labelAmountSpent.Location = new System.Drawing.Point(43, 57);
            this.labelAmountSpent.Name = "labelAmountSpent";
            this.labelAmountSpent.Size = new System.Drawing.Size(78, 13);
            this.labelAmountSpent.TabIndex = 7;
            this.labelAmountSpent.Text = "Amount spent:";
            //
            // comboStores
            //
            this.comboStores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStores.FormattingEnabled = true;
            this.comboStores.Location = new System.Drawing.Point(38, 28);
            this.comboStores.Name = "comboStores";
            this.comboStores.Size = new System.Drawing.Size(223, 21);
            this.comboStores.TabIndex = 1;
            //
            // labelStore
            //
            this.labelStore.AutoSize = true;
            this.labelStore.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelStore.Location = new System.Drawing.Point(22, 8);
            this.labelStore.Name = "labelStore";
            this.labelStore.Size = new System.Drawing.Size(38, 13);
            this.labelStore.TabIndex = 0;
            this.labelStore.Text = "Store";
            //
            // wizardPageAddress
            //
            this.wizardPageAddress.Controls.Add(this.shipBillControl);
            this.wizardPageAddress.Description = "Enter the shipping and billing address for the order.";
            this.wizardPageAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageAddress.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageAddress.Location = new System.Drawing.Point(0, 0);
            this.wizardPageAddress.Name = "wizardPageAddress";
            this.wizardPageAddress.Size = new System.Drawing.Size(613, 456);
            this.wizardPageAddress.TabIndex = 0;
            this.wizardPageAddress.Title = "Order Address";
            //
            // shipBillControl
            //
            this.shipBillControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.shipBillControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.shipBillControl.Location = new System.Drawing.Point(23, 9);
            this.shipBillControl.Name = "shipBillControl";
            this.shipBillControl.Size = new System.Drawing.Size(565, 444);
            this.shipBillControl.TabIndex = 0;
            //
            // wizardPageDetails
            //
            this.wizardPageDetails.Controls.Add(this.panelDetailsBottom);
            this.wizardPageDetails.Controls.Add(this.splitContainerDetails);
            this.wizardPageDetails.Description = "Enter the item and charge details of the order.";
            this.wizardPageDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageDetails.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageDetails.Location = new System.Drawing.Point(0, 0);
            this.wizardPageDetails.Name = "wizardPageDetails";
            this.wizardPageDetails.Size = new System.Drawing.Size(613, 456);
            this.wizardPageDetails.TabIndex = 0;
            this.wizardPageDetails.Title = "Order Details";
            //
            // panelDetailsBottom
            //
            this.panelDetailsBottom.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDetailsBottom.Controls.Add(this.labelSettings);
            this.panelDetailsBottom.Controls.Add(this.panelExtras);
            this.panelDetailsBottom.Location = new System.Drawing.Point(22, 367);
            this.panelDetailsBottom.Name = "panelDetailsBottom";
            this.panelDetailsBottom.Size = new System.Drawing.Size(579, 82);
            this.panelDetailsBottom.TabIndex = 27;
            //
            // labelSettings
            //
            this.labelSettings.AutoSize = true;
            this.labelSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelSettings.Location = new System.Drawing.Point(0, 0);
            this.labelSettings.Name = "labelSettings";
            this.labelSettings.Size = new System.Drawing.Size(54, 13);
            this.labelSettings.TabIndex = 26;
            this.labelSettings.Text = "Settings";
            //
            // panelExtras
            //
            this.panelExtras.BackColor = System.Drawing.Color.White;
            this.panelExtras.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelExtras.Controls.Add(this.linkStatus);
            this.panelExtras.Controls.Add(this.labelStatus);
            this.panelExtras.Controls.Add(this.labelShipping);
            this.panelExtras.Controls.Add(this.requestedShipping);
            this.panelExtras.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelExtras.Location = new System.Drawing.Point(0, 16);
            this.panelExtras.Name = "panelExtras";
            this.panelExtras.Size = new System.Drawing.Size(579, 66);
            this.panelExtras.TabIndex = 23;
            //
            // linkStatus
            //
            this.linkStatus.AutoSize = true;
            this.linkStatus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkStatus.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.linkStatus.ForeColor = System.Drawing.Color.Blue;
            this.linkStatus.Location = new System.Drawing.Point(109, 10);
            this.linkStatus.Name = "linkStatus";
            this.linkStatus.Size = new System.Drawing.Size(38, 13);
            this.linkStatus.TabIndex = 23;
            this.linkStatus.Text = "Status";
            this.linkStatus.UseMnemonic = false;
            this.linkStatus.Click += new System.EventHandler(this.OnLinkLocalStatus);
            //
            // labelStatus
            //
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(42, 10);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(68, 13);
            this.labelStatus.TabIndex = 19;
            this.labelStatus.Text = "Local status:";
            //
            // labelShipping
            //
            this.labelShipping.AutoSize = true;
            this.labelShipping.Location = new System.Drawing.Point(5, 37);
            this.labelShipping.Name = "labelShipping";
            this.labelShipping.Size = new System.Drawing.Size(105, 13);
            this.labelShipping.TabIndex = 18;
            this.labelShipping.Text = "Requested shipping:";
            //
            // requestedShipping
            //
            this.requestedShipping.Location = new System.Drawing.Point(112, 34);
            this.fieldLengthProvider.SetMaxLengthSource(this.requestedShipping, ShipWorks.Data.Utility.EntityFieldLengthSource.OrderRequestedShipping);
            this.requestedShipping.Name = "requestedShipping";
            this.requestedShipping.Size = new System.Drawing.Size(210, 21);
            this.requestedShipping.TabIndex = 21;
            //
            // splitContainerDetails
            //
            this.splitContainerDetails.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerDetails.Location = new System.Drawing.Point(22, 8);
            this.splitContainerDetails.Name = "splitContainerDetails";
            this.splitContainerDetails.Orientation = System.Windows.Forms.Orientation.Horizontal;
            //
            // splitContainerDetails.Panel1
            //
            this.splitContainerDetails.Panel1.Controls.Add(this.labelInvoice);
            this.splitContainerDetails.Panel1.Controls.Add(this.panelOrderDetail);
            this.splitContainerDetails.Panel1MinSize = 200;
            //
            // splitContainerDetails.Panel2
            //
            this.splitContainerDetails.Panel2.Controls.Add(this.labelNotes);
            this.splitContainerDetails.Panel2.Controls.Add(this.noteControl);
            this.splitContainerDetails.Panel2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.splitContainerDetails.Panel2MinSize = 125;
            this.splitContainerDetails.Size = new System.Drawing.Size(579, 361);
            this.splitContainerDetails.SplitterDistance = 225;
            this.splitContainerDetails.TabIndex = 26;
            //
            // labelInvoice
            //
            this.labelInvoice.AutoSize = true;
            this.labelInvoice.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelInvoice.Location = new System.Drawing.Point(0, 0);
            this.labelInvoice.Name = "labelInvoice";
            this.labelInvoice.Size = new System.Drawing.Size(49, 13);
            this.labelInvoice.TabIndex = 4;
            this.labelInvoice.Text = "Invoice";
            //
            // panelOrderDetail
            //
            this.panelOrderDetail.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelOrderDetail.BackColor = System.Drawing.Color.White;
            this.panelOrderDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelOrderDetail.Controls.Add(this.invoiceControl);
            this.panelOrderDetail.Location = new System.Drawing.Point(0, 16);
            this.panelOrderDetail.Name = "panelOrderDetail";
            this.panelOrderDetail.Size = new System.Drawing.Size(579, 209);
            this.panelOrderDetail.TabIndex = 3;
            //
            // invoiceControl
            //
            this.invoiceControl.BackColor = System.Drawing.Color.White;
            this.invoiceControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.invoiceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.invoiceControl.Location = new System.Drawing.Point(0, 0);
            this.invoiceControl.Name = "invoiceControl";
            this.invoiceControl.Size = new System.Drawing.Size(575, 205);
            this.invoiceControl.TabIndex = 0;
            //
            // labelNotes
            //
            this.labelNotes.AutoSize = true;
            this.labelNotes.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelNotes.Location = new System.Drawing.Point(0, 0);
            this.labelNotes.Name = "labelNotes";
            this.labelNotes.Size = new System.Drawing.Size(39, 13);
            this.labelNotes.TabIndex = 25;
            this.labelNotes.Text = "Notes";
            //
            // noteControl
            //
            this.noteControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.noteControl.BackColor = System.Drawing.Color.White;
            this.noteControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.noteControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.noteControl.Location = new System.Drawing.Point(0, 16);
            this.noteControl.Name = "noteControl";
            this.noteControl.Size = new System.Drawing.Size(579, 109);
            this.noteControl.TabIndex = 24;
            //
            // AddOrderWizard
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 563);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.LastPageCancelable = true;
            this.Name = "AddOrderWizard";
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageStoreCustomer,
            this.wizardPageAddress,
            this.wizardPageDetails});
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "New Order Wizard";
            this.Load += new System.EventHandler(this.OnLoad);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageStoreCustomer.ResumeLayout(false);
            this.wizardPageStoreCustomer.PerformLayout();
            this.panelCustomer.ResumeLayout(false);
            this.panelCustomer.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.wizardPageAddress.ResumeLayout(false);
            this.wizardPageDetails.ResumeLayout(false);
            this.panelDetailsBottom.ResumeLayout(false);
            this.panelDetailsBottom.PerformLayout();
            this.panelExtras.ResumeLayout(false);
            this.panelExtras.PerformLayout();
            this.splitContainerDetails.Panel1.ResumeLayout(false);
            this.splitContainerDetails.Panel1.PerformLayout();
            this.splitContainerDetails.Panel2.ResumeLayout(false);
            this.splitContainerDetails.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.splitContainerDetails)).EndInit();
            this.splitContainerDetails.ResumeLayout(false);
            this.panelOrderDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Wizard.WizardPage wizardPageStoreCustomer;
        private ShipWorks.UI.Wizard.WizardPage wizardPageAddress;
        private System.Windows.Forms.Label labelStore;
        private System.Windows.Forms.ComboBox comboStores;
        private System.Windows.Forms.Label labelCustomer;
        private System.Windows.Forms.RadioButton radioAssignCustomer;
        private System.Windows.Forms.Label amountSpent;
        private System.Windows.Forms.Label labelAmountSpent;
        private System.Windows.Forms.Label ordersPlaced;
        private System.Windows.Forms.Label labelOrdersPlaced;
        private System.Windows.Forms.Label customerName;
        private System.Windows.Forms.Label linkSelectCustomer;
        private System.Windows.Forms.RadioButton radioAutomaticCustomer;
        private System.Windows.Forms.Label labelAutomaticCustomer;
        private ShipWorks.Stores.Content.Controls.ShipBillAddressControl shipBillControl;
        private ShipWorks.UI.Wizard.WizardPage wizardPageDetails;
        private System.Windows.Forms.Panel panelCustomer;
        private System.Windows.Forms.SplitContainer splitContainerDetails;
        private System.Windows.Forms.Label labelInvoice;
        private ShipWorks.UI.Controls.ThemeBorderPanel panelOrderDetail;
        private ShipWorks.Stores.Content.Controls.InvoiceControl invoiceControl;
        private System.Windows.Forms.Label labelNotes;
        private ShipWorks.Stores.Content.Panels.NotesPanel noteControl;
        private System.Windows.Forms.RadioButton radioOrderNumberGenerate;
        private System.Windows.Forms.Label labelOrderNumber;
        private System.Windows.Forms.RadioButton radioOrderNumberSpecify;
        private System.Windows.Forms.Label labelOrderPostfixInfo;
        private System.Windows.Forms.TextBox orderPostfix;
        private System.Windows.Forms.Label labelPostfix;
        private System.Windows.Forms.Label labelOrderPrefixInfo;
        private System.Windows.Forms.TextBox orderPrefix;
        private System.Windows.Forms.Label labelOrderPrefix;
        private System.Windows.Forms.Label labelOrderNumberInfo;
        private System.Windows.Forms.Label labelOrder;
        private ShipWorks.UI.Controls.NumericTextBox orderNumber;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelOrderPreview;
        private System.Windows.Forms.Panel panelDetailsBottom;
        private System.Windows.Forms.Label labelSettings;
        private ShipWorks.UI.Controls.ThemeBorderPanel panelExtras;
        private System.Windows.Forms.Label linkStatus;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label labelShipping;
        private System.Windows.Forms.TextBox requestedShipping;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
    }
}