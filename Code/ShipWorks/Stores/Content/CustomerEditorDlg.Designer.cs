namespace ShipWorks.Stores.Content
{
    partial class CustomerEditorDlg
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
            this.close = new System.Windows.Forms.Button();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.labelTotalSpent = new System.Windows.Forms.Label();
            this.labelMostRecent = new System.Windows.Forms.Label();
            this.labelCustomer = new System.Windows.Forms.Label();
            this.imageUser = new System.Windows.Forms.PictureBox();
            this.labelOrderCount = new System.Windows.Forms.Label();
            this.labelOrdersPlaced = new System.Windows.Forms.Label();
            this.optionControl = new ShipWorks.UI.Controls.OptionControl();
            this.optionPageDetails = new ShipWorks.UI.Controls.OptionPage();
            this.splitContainerDetails = new System.Windows.Forms.SplitContainer();
            this.ordersControl = new ShipWorks.Stores.Content.Panels.OrdersPanel();
            this.labelOrders = new System.Windows.Forms.Label();
            this.noteControl = new ShipWorks.Stores.Content.Panels.NotesPanel();
            this.labelNotes = new System.Windows.Forms.Label();
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
            this.shipBillControl = new ShipWorks.Stores.Content.Controls.ShipBillAddressControl();
            this.editAddress = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.imageUser)).BeginInit();
            this.optionControl.SuspendLayout();
            this.optionPageDetails.SuspendLayout();
            this.splitContainerDetails.Panel1.SuspendLayout();
            this.splitContainerDetails.Panel2.SuspendLayout();
            this.splitContainerDetails.SuspendLayout();
            this.optionPageHistory.SuspendLayout();
            this.splitContainerHistoryBottom.Panel1.SuspendLayout();
            this.splitContainerHistoryBottom.Panel2.SuspendLayout();
            this.splitContainerHistoryBottom.SuspendLayout();
            this.splitContainerHistoryTop.Panel1.SuspendLayout();
            this.splitContainerHistoryTop.Panel2.SuspendLayout();
            this.splitContainerHistoryTop.SuspendLayout();
            this.optionPageAddress.SuspendLayout();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.close.Location = new System.Drawing.Point(677, 514);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 0;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // panelHeader
            // 
            this.panelHeader.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHeader.Controls.Add(this.labelTotalSpent);
            this.panelHeader.Controls.Add(this.labelMostRecent);
            this.panelHeader.Controls.Add(this.labelCustomer);
            this.panelHeader.Controls.Add(this.imageUser);
            this.panelHeader.Controls.Add(this.labelOrderCount);
            this.panelHeader.Controls.Add(this.labelOrdersPlaced);
            this.panelHeader.Location = new System.Drawing.Point(4, 4);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(748, 41);
            this.panelHeader.TabIndex = 19;
            // 
            // labelTotalSpent
            // 
            this.labelTotalSpent.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTotalSpent.AutoSize = true;
            this.labelTotalSpent.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (80)))), ((int) (((byte) (80)))), ((int) (((byte) (80)))));
            this.labelTotalSpent.Location = new System.Drawing.Point(572, 22);
            this.labelTotalSpent.Name = "labelTotalSpent";
            this.labelTotalSpent.Size = new System.Drawing.Size(112, 13);
            this.labelTotalSpent.TabIndex = 19;
            this.labelTotalSpent.Text = " Total Spent: $105.83";
            // 
            // labelMostRecent
            // 
            this.labelMostRecent.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMostRecent.AutoSize = true;
            this.labelMostRecent.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (80)))), ((int) (((byte) (80)))), ((int) (((byte) (80)))));
            this.labelMostRecent.Location = new System.Drawing.Point(538, 5);
            this.labelMostRecent.Name = "labelMostRecent";
            this.labelMostRecent.Size = new System.Drawing.Size(207, 13);
            this.labelMostRecent.TabIndex = 18;
            this.labelMostRecent.Text = "Most Recent Order: Yesterday, 10:05 AM";
            // 
            // labelCustomer
            // 
            this.labelCustomer.AutoSize = true;
            this.labelCustomer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelCustomer.Location = new System.Drawing.Point(43, 5);
            this.labelCustomer.Name = "labelCustomer";
            this.labelCustomer.Size = new System.Drawing.Size(103, 13);
            this.labelCustomer.TabIndex = 11;
            this.labelCustomer.Text = "Johnny Backseat";
            // 
            // imageUser
            // 
            this.imageUser.Image = global::ShipWorks.Properties.Resources.customer32;
            this.imageUser.Location = new System.Drawing.Point(4, 5);
            this.imageUser.Name = "imageUser";
            this.imageUser.Size = new System.Drawing.Size(32, 32);
            this.imageUser.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imageUser.TabIndex = 10;
            this.imageUser.TabStop = false;
            // 
            // labelOrderCount
            // 
            this.labelOrderCount.AutoSize = true;
            this.labelOrderCount.Location = new System.Drawing.Point(120, 22);
            this.labelOrderCount.Name = "labelOrderCount";
            this.labelOrderCount.Size = new System.Drawing.Size(13, 13);
            this.labelOrderCount.TabIndex = 15;
            this.labelOrderCount.Text = "2";
            // 
            // labelOrdersPlaced
            // 
            this.labelOrdersPlaced.AutoSize = true;
            this.labelOrdersPlaced.Location = new System.Drawing.Point(44, 22);
            this.labelOrdersPlaced.Name = "labelOrdersPlaced";
            this.labelOrdersPlaced.Size = new System.Drawing.Size(78, 13);
            this.labelOrdersPlaced.TabIndex = 14;
            this.labelOrdersPlaced.Text = "Orders Placed:";
            // 
            // optionControl
            // 
            this.optionControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.optionControl.Controls.Add(this.optionPageDetails);
            this.optionControl.Controls.Add(this.optionPageHistory);
            this.optionControl.Controls.Add(this.optionPageAddress);
            this.optionControl.Location = new System.Drawing.Point(9, 50);
            this.optionControl.MenuListWidth = 90;
            this.optionControl.Name = "optionControl";
            this.optionControl.SelectedIndex = 0;
            this.optionControl.Size = new System.Drawing.Size(743, 458);
            this.optionControl.TabIndex = 20;
            this.optionControl.Text = "optionControl";
            // 
            // optionPageDetails
            // 
            this.optionPageDetails.Controls.Add(this.splitContainerDetails);
            this.optionPageDetails.Location = new System.Drawing.Point(93, 0);
            this.optionPageDetails.Name = "optionPageDetails";
            this.optionPageDetails.Size = new System.Drawing.Size(650, 458);
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
            this.splitContainerDetails.Panel1.Controls.Add(this.ordersControl);
            this.splitContainerDetails.Panel1.Controls.Add(this.labelOrders);
            // 
            // splitContainerDetails.Panel2
            // 
            this.splitContainerDetails.Panel2.Controls.Add(this.noteControl);
            this.splitContainerDetails.Panel2.Controls.Add(this.labelNotes);
            this.splitContainerDetails.Size = new System.Drawing.Size(650, 458);
            this.splitContainerDetails.SplitterDistance = 297;
            this.splitContainerDetails.TabIndex = 0;
            // 
            // ordersControl
            // 
            this.ordersControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ordersControl.BackColor = System.Drawing.Color.White;
            this.ordersControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ordersControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.ordersControl.Location = new System.Drawing.Point(0, 16);
            this.ordersControl.Name = "ordersControl";
            this.ordersControl.Size = new System.Drawing.Size(650, 280);
            this.ordersControl.TabIndex = 2;
            this.ordersControl.AddOrderClicked += new System.EventHandler(this.OnAddOrder);
            this.ordersControl.OrderDeleted += new System.EventHandler(this.OnOrderDeleted);
            // 
            // labelOrders
            // 
            this.labelOrders.AutoSize = true;
            this.labelOrders.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelOrders.Location = new System.Drawing.Point(0, 0);
            this.labelOrders.Name = "labelOrders";
            this.labelOrders.Size = new System.Drawing.Size(45, 13);
            this.labelOrders.TabIndex = 1;
            this.labelOrders.Text = "Orders";
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
            this.noteControl.Size = new System.Drawing.Size(650, 141);
            this.noteControl.TabIndex = 1;
            // 
            // labelNotes
            // 
            this.labelNotes.AutoSize = true;
            this.labelNotes.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelNotes.Location = new System.Drawing.Point(0, 0);
            this.labelNotes.Name = "labelNotes";
            this.labelNotes.Size = new System.Drawing.Size(39, 13);
            this.labelNotes.TabIndex = 0;
            this.labelNotes.Text = "Notes";
            // 
            // optionPageHistory
            // 
            this.optionPageHistory.Controls.Add(this.splitContainerHistoryBottom);
            this.optionPageHistory.Location = new System.Drawing.Point(93, 0);
            this.optionPageHistory.Name = "optionPageHistory";
            this.optionPageHistory.Size = new System.Drawing.Size(650, 458);
            this.optionPageHistory.TabIndex = 2;
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
            this.splitContainerHistoryBottom.Size = new System.Drawing.Size(650, 458);
            this.splitContainerHistoryBottom.SplitterDistance = 294;
            this.splitContainerHistoryBottom.TabIndex = 4;
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
            this.splitContainerHistoryTop.Size = new System.Drawing.Size(650, 294);
            this.splitContainerHistoryTop.SplitterDistance = 145;
            this.splitContainerHistoryTop.TabIndex = 2;
            // 
            // labelShipments
            // 
            this.labelShipments.AutoSize = true;
            this.labelShipments.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelShipments.Location = new System.Drawing.Point(0, 0);
            this.labelShipments.Name = "labelShipments";
            this.labelShipments.Size = new System.Drawing.Size(67, 13);
            this.labelShipments.TabIndex = 3;
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
            this.shipmentsControl.Size = new System.Drawing.Size(650, 128);
            this.shipmentsControl.TabIndex = 2;
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
            this.emailControl.Size = new System.Drawing.Size(650, 129);
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
            this.printResultControl.Size = new System.Drawing.Size(650, 143);
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
            this.optionPageAddress.Size = new System.Drawing.Size(650, 458);
            this.optionPageAddress.TabIndex = 3;
            this.optionPageAddress.Text = "Address";
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
            this.shipBillControl.Size = new System.Drawing.Size(570, 458);
            this.shipBillControl.TabIndex = 0;
            // 
            // editAddress
            // 
            this.editAddress.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editAddress.Image = global::ShipWorks.Properties.Resources.edit16;
            this.editAddress.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.editAddress.Location = new System.Drawing.Point(575, 17);
            this.editAddress.Name = "editAddress";
            this.editAddress.Size = new System.Drawing.Size(75, 23);
            this.editAddress.TabIndex = 3;
            this.editAddress.Text = "   Edit";
            this.editAddress.UseVisualStyleBackColor = true;
            this.editAddress.Click += new System.EventHandler(this.OnEditAddress);
            // 
            // CustomerEditorDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(764, 549);
            this.Controls.Add(this.optionControl);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.close);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(650, 585);
            this.Name = "CustomerEditorDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Customer Editor";
            this.Load += new System.EventHandler(this.OnLoad);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.imageUser)).EndInit();
            this.optionControl.ResumeLayout(false);
            this.optionPageDetails.ResumeLayout(false);
            this.splitContainerDetails.Panel1.ResumeLayout(false);
            this.splitContainerDetails.Panel1.PerformLayout();
            this.splitContainerDetails.Panel2.ResumeLayout(false);
            this.splitContainerDetails.Panel2.PerformLayout();
            this.splitContainerDetails.ResumeLayout(false);
            this.optionPageHistory.ResumeLayout(false);
            this.splitContainerHistoryBottom.Panel1.ResumeLayout(false);
            this.splitContainerHistoryBottom.Panel2.ResumeLayout(false);
            this.splitContainerHistoryBottom.Panel2.PerformLayout();
            this.splitContainerHistoryBottom.ResumeLayout(false);
            this.splitContainerHistoryTop.Panel1.ResumeLayout(false);
            this.splitContainerHistoryTop.Panel1.PerformLayout();
            this.splitContainerHistoryTop.Panel2.ResumeLayout(false);
            this.splitContainerHistoryTop.Panel2.PerformLayout();
            this.splitContainerHistoryTop.ResumeLayout(false);
            this.optionPageAddress.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button close;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label labelCustomer;
        private System.Windows.Forms.PictureBox imageUser;
        private System.Windows.Forms.Label labelOrderCount;
        private System.Windows.Forms.Label labelOrdersPlaced;
        private System.Windows.Forms.Label labelTotalSpent;
        private System.Windows.Forms.Label labelMostRecent;
        private ShipWorks.UI.Controls.OptionControl optionControl;
        private ShipWorks.UI.Controls.OptionPage optionPageDetails;
        private ShipWorks.UI.Controls.OptionPage optionPageHistory;
        private ShipWorks.UI.Controls.OptionPage optionPageAddress;
        private System.Windows.Forms.SplitContainer splitContainerDetails;
        private System.Windows.Forms.Label labelNotes;
        private ShipWorks.Stores.Content.Panels.NotesPanel noteControl;
        private ShipWorks.Stores.Content.Panels.OrdersPanel ordersControl;
        private System.Windows.Forms.Label labelOrders;
        private System.Windows.Forms.SplitContainer splitContainerHistoryBottom;
        private System.Windows.Forms.SplitContainer splitContainerHistoryTop;
        private System.Windows.Forms.Label labelEmail;
        private ShipWorks.Stores.Content.Panels.EmailOutboundPanel emailControl;
        private System.Windows.Forms.Label labelPrint;
        private System.Windows.Forms.Label labelShipments;
        private ShipWorks.Stores.Content.Panels.ShipmentsPanel shipmentsControl;
        private ShipWorks.Stores.Content.Panels.PrintResultsPanel printResultControl;
        private ShipWorks.Stores.Content.Controls.ShipBillAddressControl shipBillControl;
        private System.Windows.Forms.Button editAddress;
    }
}