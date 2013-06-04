namespace ShipWorks.Stores.Content
{
    partial class OrderEditorDlg
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
            this.ok = new System.Windows.Forms.Button();
            this.labelOrder = new System.Windows.Forms.Label();
            this.imageUser = new System.Windows.Forms.PictureBox();
            this.labelOrderDate = new System.Windows.Forms.Label();
            this.labelCustomer = new System.Windows.Forms.Label();
            this.labelCustomerOrderCount = new System.Windows.Forms.Label();
            this.viewCustomer = new System.Windows.Forms.Label();
            this.labelStore = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.linkChangeCustomer = new System.Windows.Forms.Label();
            this.optionControl = new ShipWorks.UI.Controls.OptionControl();
            this.optionPageDetails = new ShipWorks.UI.Controls.OptionPage();
            this.splitContainerDetails = new System.Windows.Forms.SplitContainer();
            this.labelInvoice = new System.Windows.Forms.Label();
            this.panelOrderDetail = new ShipWorks.UI.Controls.ThemeBorderPanel();
            this.invoiceControl = new ShipWorks.Stores.Content.Controls.InvoiceControl();
            this.labelNotes = new System.Windows.Forms.Label();
            this.noteControl = new ShipWorks.Stores.Content.Panels.NotesPanel();
            this.panelDetailsBottom = new System.Windows.Forms.Panel();
            this.labelSettings = new System.Windows.Forms.Label();
            this.panelExtras = new ShipWorks.UI.Controls.ThemeBorderPanel();
            this.linkStatus = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelShipping = new System.Windows.Forms.Label();
            this.requestedShipping = new System.Windows.Forms.TextBox();
            this.optionPageHistory = new ShipWorks.UI.Controls.OptionPage();
            this.splitContainerHistoryBottom = new System.Windows.Forms.SplitContainer();
            this.splitContainerHistoryTop = new System.Windows.Forms.SplitContainer();
            this.labelShipments = new System.Windows.Forms.Label();
            this.shipmentsControl = new ShipWorks.Stores.Content.Panels.ShipmentsPanel();
            this.labelEmail = new System.Windows.Forms.Label();
            this.emailControl = new ShipWorks.Stores.Content.Panels.EmailOutboundPanel();
            this.printResultControl = new ShipWorks.Stores.Content.Panels.PrintResultsPanel();
            this.labelPrint = new System.Windows.Forms.Label();
            this.optionPageAddress = new ShipWorks.UI.Controls.OptionPage();
            this.editAddress = new System.Windows.Forms.Button();
            this.shipBillControl = new ShipWorks.Stores.Content.Controls.ShipBillAddressControl();
            this.optionPageAudit = new ShipWorks.UI.Controls.OptionPage();
            this.auditControl = new ShipWorks.Users.Audit.AuditControl();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            ((System.ComponentModel.ISupportInitialize) (this.imageUser)).BeginInit();
            this.panelHeader.SuspendLayout();
            this.optionControl.SuspendLayout();
            this.optionPageDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.splitContainerDetails)).BeginInit();
            this.splitContainerDetails.Panel1.SuspendLayout();
            this.splitContainerDetails.Panel2.SuspendLayout();
            this.splitContainerDetails.SuspendLayout();
            this.panelOrderDetail.SuspendLayout();
            this.panelDetailsBottom.SuspendLayout();
            this.panelExtras.SuspendLayout();
            this.optionPageHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.splitContainerHistoryBottom)).BeginInit();
            this.splitContainerHistoryBottom.Panel1.SuspendLayout();
            this.splitContainerHistoryBottom.Panel2.SuspendLayout();
            this.splitContainerHistoryBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.splitContainerHistoryTop)).BeginInit();
            this.splitContainerHistoryTop.Panel1.SuspendLayout();
            this.splitContainerHistoryTop.Panel2.SuspendLayout();
            this.splitContainerHistoryTop.SuspendLayout();
            this.optionPageAddress.SuspendLayout();
            this.optionPageAudit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok.Location = new System.Drawing.Point(681, 529);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 0;
            this.ok.Text = "Close";
            this.ok.UseVisualStyleBackColor = true;
            // 
            // labelOrder
            // 
            this.labelOrder.AutoSize = true;
            this.labelOrder.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelOrder.Location = new System.Drawing.Point(43, 5);
            this.labelOrder.Name = "labelOrder";
            this.labelOrder.Size = new System.Drawing.Size(77, 13);
            this.labelOrder.TabIndex = 11;
            this.labelOrder.Text = "Order 13458";
            // 
            // imageUser
            // 
            this.imageUser.Image = global::ShipWorks.Properties.Resources.order32;
            this.imageUser.Location = new System.Drawing.Point(4, 5);
            this.imageUser.Name = "imageUser";
            this.imageUser.Size = new System.Drawing.Size(32, 32);
            this.imageUser.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imageUser.TabIndex = 10;
            this.imageUser.TabStop = false;
            // 
            // labelOrderDate
            // 
            this.labelOrderDate.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelOrderDate.AutoSize = true;
            this.labelOrderDate.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (80)))), ((int) (((byte) (80)))), ((int) (((byte) (80)))));
            this.labelOrderDate.Location = new System.Drawing.Point(552, 6);
            this.labelOrderDate.Name = "labelOrderDate";
            this.labelOrderDate.Size = new System.Drawing.Size(173, 13);
            this.labelOrderDate.TabIndex = 13;
            this.labelOrderDate.Text = "Date Placed: Yesterday, 10:05 AM";
            // 
            // labelCustomer
            // 
            this.labelCustomer.AutoSize = true;
            this.labelCustomer.Location = new System.Drawing.Point(44, 22);
            this.labelCustomer.Name = "labelCustomer";
            this.labelCustomer.Size = new System.Drawing.Size(79, 13);
            this.labelCustomer.TabIndex = 14;
            this.labelCustomer.Text = "Arful Liznobber";
            // 
            // labelCustomerOrderCount
            // 
            this.labelCustomerOrderCount.AutoSize = true;
            this.labelCustomerOrderCount.ForeColor = System.Drawing.Color.DimGray;
            this.labelCustomerOrderCount.Location = new System.Drawing.Point(120, 22);
            this.labelCustomerOrderCount.Name = "labelCustomerOrderCount";
            this.labelCustomerOrderCount.Size = new System.Drawing.Size(84, 13);
            this.labelCustomerOrderCount.TabIndex = 15;
            this.labelCustomerOrderCount.Text = "(2 other orders)";
            // 
            // viewCustomer
            // 
            this.viewCustomer.AutoSize = true;
            this.viewCustomer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.viewCustomer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.viewCustomer.ForeColor = System.Drawing.Color.Blue;
            this.viewCustomer.Location = new System.Drawing.Point(202, 22);
            this.viewCustomer.Name = "viewCustomer";
            this.viewCustomer.Size = new System.Drawing.Size(78, 13);
            this.viewCustomer.TabIndex = 16;
            this.viewCustomer.Text = "View Customer";
            this.viewCustomer.Click += new System.EventHandler(this.OnViewCustomer);
            // 
            // labelStore
            // 
            this.labelStore.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStore.AutoSize = true;
            this.labelStore.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (80)))), ((int) (((byte) (80)))), ((int) (((byte) (80)))));
            this.labelStore.Location = new System.Drawing.Point(583, 21);
            this.labelStore.Name = "labelStore";
            this.labelStore.Size = new System.Drawing.Size(99, 13);
            this.labelStore.TabIndex = 17;
            this.labelStore.Text = "Store: Electro Gear";
            // 
            // panelHeader
            // 
            this.panelHeader.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHeader.Controls.Add(this.linkChangeCustomer);
            this.panelHeader.Controls.Add(this.labelOrder);
            this.panelHeader.Controls.Add(this.labelStore);
            this.panelHeader.Controls.Add(this.imageUser);
            this.panelHeader.Controls.Add(this.viewCustomer);
            this.panelHeader.Controls.Add(this.labelOrderDate);
            this.panelHeader.Controls.Add(this.labelCustomerOrderCount);
            this.panelHeader.Controls.Add(this.labelCustomer);
            this.panelHeader.Location = new System.Drawing.Point(4, 4);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(752, 41);
            this.panelHeader.TabIndex = 18;
            // 
            // linkChangeCustomer
            // 
            this.linkChangeCustomer.AutoSize = true;
            this.linkChangeCustomer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkChangeCustomer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.linkChangeCustomer.ForeColor = System.Drawing.Color.Blue;
            this.linkChangeCustomer.Location = new System.Drawing.Point(280, 22);
            this.linkChangeCustomer.Name = "linkChangeCustomer";
            this.linkChangeCustomer.Size = new System.Drawing.Size(56, 13);
            this.linkChangeCustomer.TabIndex = 18;
            this.linkChangeCustomer.Text = "Change...";
            this.linkChangeCustomer.Click += new System.EventHandler(this.OnChangeCustomer);
            // 
            // optionControl
            // 
            this.optionControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.optionControl.Controls.Add(this.optionPageDetails);
            this.optionControl.Controls.Add(this.optionPageHistory);
            this.optionControl.Controls.Add(this.optionPageAddress);
            this.optionControl.Controls.Add(this.optionPageAudit);
            this.optionControl.Location = new System.Drawing.Point(9, 50);
            this.optionControl.MenuListWidth = 90;
            this.optionControl.Name = "optionControl";
            this.optionControl.SelectedIndex = 0;
            this.optionControl.Size = new System.Drawing.Size(747, 473);
            this.optionControl.TabIndex = 12;
            this.optionControl.Text = "optionControl";
            // 
            // optionPageDetails
            // 
            this.optionPageDetails.Controls.Add(this.splitContainerDetails);
            this.optionPageDetails.Controls.Add(this.panelDetailsBottom);
            this.optionPageDetails.Location = new System.Drawing.Point(93, 0);
            this.optionPageDetails.Name = "optionPageDetails";
            this.optionPageDetails.Size = new System.Drawing.Size(654, 473);
            this.optionPageDetails.TabIndex = 1;
            this.optionPageDetails.Text = "Details";
            // 
            // splitContainerDetails
            // 
            this.splitContainerDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerDetails.Location = new System.Drawing.Point(0, 0);
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
            this.splitContainerDetails.Size = new System.Drawing.Size(654, 391);
            this.splitContainerDetails.SplitterDistance = 245;
            this.splitContainerDetails.TabIndex = 25;
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
            this.panelOrderDetail.Size = new System.Drawing.Size(654, 229);
            this.panelOrderDetail.TabIndex = 3;
            // 
            // invoiceControl
            // 
            this.invoiceControl.BackColor = System.Drawing.Color.White;
            this.invoiceControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.invoiceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.invoiceControl.Location = new System.Drawing.Point(0, 0);
            this.invoiceControl.Name = "invoiceControl";
            this.invoiceControl.Size = new System.Drawing.Size(650, 225);
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
            this.noteControl.Size = new System.Drawing.Size(654, 119);
            this.noteControl.TabIndex = 24;
            // 
            // panelDetailsBottom
            // 
            this.panelDetailsBottom.Controls.Add(this.labelSettings);
            this.panelDetailsBottom.Controls.Add(this.panelExtras);
            this.panelDetailsBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelDetailsBottom.Location = new System.Drawing.Point(0, 391);
            this.panelDetailsBottom.Name = "panelDetailsBottom";
            this.panelDetailsBottom.Size = new System.Drawing.Size(654, 82);
            this.panelDetailsBottom.TabIndex = 26;
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
            this.panelExtras.Size = new System.Drawing.Size(654, 66);
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
            // optionPageHistory
            // 
            this.optionPageHistory.Controls.Add(this.splitContainerHistoryBottom);
            this.optionPageHistory.Location = new System.Drawing.Point(93, 0);
            this.optionPageHistory.Name = "optionPageHistory";
            this.optionPageHistory.Size = new System.Drawing.Size(654, 473);
            this.optionPageHistory.TabIndex = 4;
            this.optionPageHistory.Text = "History";
            // 
            // splitContainerHistoryBottom
            // 
            this.splitContainerHistoryBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerHistoryBottom.Location = new System.Drawing.Point(0, 0);
            this.splitContainerHistoryBottom.Name = "splitContainerHistoryBottom";
            this.splitContainerHistoryBottom.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerHistoryBottom.Panel1
            // 
            this.splitContainerHistoryBottom.Panel1.Controls.Add(this.splitContainerHistoryTop);
            // 
            // splitContainerHistoryBottom.Panel2
            // 
            this.splitContainerHistoryBottom.Panel2.Controls.Add(this.printResultControl);
            this.splitContainerHistoryBottom.Panel2.Controls.Add(this.labelPrint);
            this.splitContainerHistoryBottom.Size = new System.Drawing.Size(654, 473);
            this.splitContainerHistoryBottom.SplitterDistance = 303;
            this.splitContainerHistoryBottom.TabIndex = 3;
            // 
            // splitContainerHistoryTop
            // 
            this.splitContainerHistoryTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerHistoryTop.Location = new System.Drawing.Point(0, 0);
            this.splitContainerHistoryTop.Name = "splitContainerHistoryTop";
            this.splitContainerHistoryTop.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerHistoryTop.Panel1
            // 
            this.splitContainerHistoryTop.Panel1.Controls.Add(this.labelShipments);
            this.splitContainerHistoryTop.Panel1.Controls.Add(this.shipmentsControl);
            // 
            // splitContainerHistoryTop.Panel2
            // 
            this.splitContainerHistoryTop.Panel2.Controls.Add(this.labelEmail);
            this.splitContainerHistoryTop.Panel2.Controls.Add(this.emailControl);
            this.splitContainerHistoryTop.Size = new System.Drawing.Size(654, 303);
            this.splitContainerHistoryTop.SplitterDistance = 149;
            this.splitContainerHistoryTop.TabIndex = 2;
            // 
            // labelShipments
            // 
            this.labelShipments.AutoSize = true;
            this.labelShipments.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelShipments.Location = new System.Drawing.Point(0, 0);
            this.labelShipments.Name = "labelShipments";
            this.labelShipments.Size = new System.Drawing.Size(67, 13);
            this.labelShipments.TabIndex = 1;
            this.labelShipments.Text = "Shipments";
            // 
            // shipmentsControl
            // 
            this.shipmentsControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.shipmentsControl.BackColor = System.Drawing.Color.White;
            this.shipmentsControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.shipmentsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.shipmentsControl.Location = new System.Drawing.Point(0, 16);
            this.shipmentsControl.Name = "shipmentsControl";
            this.shipmentsControl.Size = new System.Drawing.Size(654, 132);
            this.shipmentsControl.TabIndex = 0;
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelEmail.Location = new System.Drawing.Point(0, 0);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(37, 13);
            this.labelEmail.TabIndex = 2;
            this.labelEmail.Text = "Email";
            // 
            // emailControl
            // 
            this.emailControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.emailControl.BackColor = System.Drawing.Color.White;
            this.emailControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.emailControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.emailControl.Location = new System.Drawing.Point(0, 16);
            this.emailControl.Name = "emailControl";
            this.emailControl.Size = new System.Drawing.Size(654, 134);
            this.emailControl.TabIndex = 1;
            // 
            // printResultControl
            // 
            this.printResultControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.printResultControl.BackColor = System.Drawing.Color.White;
            this.printResultControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.printResultControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.printResultControl.Location = new System.Drawing.Point(0, 16);
            this.printResultControl.Name = "printResultControl";
            this.printResultControl.Size = new System.Drawing.Size(654, 150);
            this.printResultControl.TabIndex = 4;
            // 
            // labelPrint
            // 
            this.labelPrint.AutoSize = true;
            this.labelPrint.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelPrint.Location = new System.Drawing.Point(0, 0);
            this.labelPrint.Name = "labelPrint";
            this.labelPrint.Size = new System.Drawing.Size(48, 13);
            this.labelPrint.TabIndex = 3;
            this.labelPrint.Text = "Printed";
            // 
            // optionPageAddress
            // 
            this.optionPageAddress.Controls.Add(this.editAddress);
            this.optionPageAddress.Controls.Add(this.shipBillControl);
            this.optionPageAddress.Location = new System.Drawing.Point(93, 0);
            this.optionPageAddress.Name = "optionPageAddress";
            this.optionPageAddress.Size = new System.Drawing.Size(654, 473);
            this.optionPageAddress.TabIndex = 2;
            this.optionPageAddress.Text = "Address";
            // 
            // editAddress
            // 
            this.editAddress.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editAddress.Image = global::ShipWorks.Properties.Resources.edit16;
            this.editAddress.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.editAddress.Location = new System.Drawing.Point(579, 16);
            this.editAddress.Name = "editAddress";
            this.editAddress.Size = new System.Drawing.Size(75, 23);
            this.editAddress.TabIndex = 2;
            this.editAddress.Text = "   Edit";
            this.editAddress.UseVisualStyleBackColor = true;
            this.editAddress.Click += new System.EventHandler(this.OnEditAddress);
            // 
            // shipBillControl
            // 
            this.shipBillControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.shipBillControl.EditStyle = ShipWorks.Data.Controls.PersonEditStyle.ReadOnly;
            this.shipBillControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.shipBillControl.Location = new System.Drawing.Point(0, 0);
            this.shipBillControl.Name = "shipBillControl";
            this.shipBillControl.Size = new System.Drawing.Size(573, 473);
            this.shipBillControl.TabIndex = 0;
            // 
            // optionPageAudit
            // 
            this.optionPageAudit.Controls.Add(this.auditControl);
            this.optionPageAudit.Location = new System.Drawing.Point(93, 0);
            this.optionPageAudit.Name = "optionPageAudit";
            this.optionPageAudit.Size = new System.Drawing.Size(654, 473);
            this.optionPageAudit.TabIndex = 6;
            this.optionPageAudit.Text = "Audit";
            // 
            // auditControl
            // 
            this.auditControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.auditControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.auditControl.Location = new System.Drawing.Point(0, 0);
            this.auditControl.Name = "auditControl";
            this.auditControl.Size = new System.Drawing.Size(654, 473);
            this.auditControl.TabIndex = 0;
            // 
            // OrderEditorDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ok;
            this.ClientSize = new System.Drawing.Size(768, 564);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.optionControl);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(650, 585);
            this.Name = "OrderEditorDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Order Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize) (this.imageUser)).EndInit();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.optionControl.ResumeLayout(false);
            this.optionPageDetails.ResumeLayout(false);
            this.splitContainerDetails.Panel1.ResumeLayout(false);
            this.splitContainerDetails.Panel1.PerformLayout();
            this.splitContainerDetails.Panel2.ResumeLayout(false);
            this.splitContainerDetails.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.splitContainerDetails)).EndInit();
            this.splitContainerDetails.ResumeLayout(false);
            this.panelOrderDetail.ResumeLayout(false);
            this.panelDetailsBottom.ResumeLayout(false);
            this.panelDetailsBottom.PerformLayout();
            this.panelExtras.ResumeLayout(false);
            this.panelExtras.PerformLayout();
            this.optionPageHistory.ResumeLayout(false);
            this.splitContainerHistoryBottom.Panel1.ResumeLayout(false);
            this.splitContainerHistoryBottom.Panel2.ResumeLayout(false);
            this.splitContainerHistoryBottom.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.splitContainerHistoryBottom)).EndInit();
            this.splitContainerHistoryBottom.ResumeLayout(false);
            this.splitContainerHistoryTop.Panel1.ResumeLayout(false);
            this.splitContainerHistoryTop.Panel1.PerformLayout();
            this.splitContainerHistoryTop.Panel2.ResumeLayout(false);
            this.splitContainerHistoryTop.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.splitContainerHistoryTop)).EndInit();
            this.splitContainerHistoryTop.ResumeLayout(false);
            this.optionPageAddress.ResumeLayout(false);
            this.optionPageAudit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Label labelOrder;
        private System.Windows.Forms.PictureBox imageUser;
        private ShipWorks.UI.Controls.OptionControl optionControl;
        private ShipWorks.UI.Controls.OptionPage optionPageDetails;
        private ShipWorks.UI.Controls.OptionPage optionPageAddress;
        private ShipWorks.UI.Controls.OptionPage optionPageHistory;
        private ShipWorks.UI.Controls.ThemeBorderPanel panelOrderDetail;
        private System.Windows.Forms.Label labelOrderDate;
        private System.Windows.Forms.Label labelCustomer;
        private System.Windows.Forms.Label labelCustomerOrderCount;
        private System.Windows.Forms.Label viewCustomer;
        private System.Windows.Forms.Label labelStore;
        private System.Windows.Forms.TextBox requestedShipping;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label labelShipping;
        private ShipWorks.UI.Controls.ThemeBorderPanel panelExtras;
        private System.Windows.Forms.Panel panelHeader;
        private ShipWorks.Stores.Content.Panels.NotesPanel noteControl;
        private System.Windows.Forms.SplitContainer splitContainerDetails;
        private ShipWorks.Stores.Content.Panels.ShipmentsPanel shipmentsControl;
        private ShipWorks.Stores.Content.Panels.EmailOutboundPanel emailControl;
        private System.Windows.Forms.SplitContainer splitContainerHistoryTop;
        private System.Windows.Forms.Label labelShipments;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.SplitContainer splitContainerHistoryBottom;
        private System.Windows.Forms.Label labelPrint;
        private System.Windows.Forms.Label labelInvoice;
        private System.Windows.Forms.Label labelNotes;
        private System.Windows.Forms.Panel panelDetailsBottom;
        private System.Windows.Forms.Label labelSettings;
        private System.Windows.Forms.Label linkStatus;
        private ShipWorks.Stores.Content.Panels.PrintResultsPanel printResultControl;
        private ShipWorks.Stores.Content.Controls.ShipBillAddressControl shipBillControl;
        private ShipWorks.Stores.Content.Controls.InvoiceControl invoiceControl;
        private System.Windows.Forms.Label linkChangeCustomer;
        private ShipWorks.UI.Controls.OptionPage optionPageAudit;
        private ShipWorks.Users.Audit.AuditControl auditControl;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.Button editAddress;
    }
}