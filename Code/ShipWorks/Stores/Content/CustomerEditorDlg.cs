using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Data.SqlClient;
using Interapptive.Shared;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.Data.Controls;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Window for editing customers
    /// </summary>
    public partial class CustomerEditorDlg : Form
    {
        long customerID;

        CustomerEntity customer;

        Guid ordersLayoutID = new Guid("{A6F203C4-D020-4c41-9C53-0069346D0371}");
        Guid notesLayoutID = new Guid("{37A3A239-03EB-4ab6-85AD-B6CE19D4B43B}");
        Guid emailLayoutID = new Guid("{916B21FA-7B54-42e0-92F0-7975E3A985D7}");
        Guid shipmentLayoutID = new Guid("{E2CA9188-6506-4e27-B9CE-D9E2FEF5B567}");
        Guid printLayoutID = new Guid("{4393B860-E191-40ed-8D2B-8729C3357378}");

        static List<long> openCustomers = new List<long>();

        /// <summary>
        /// Constructor
        /// </summary>
        private CustomerEditorDlg(long customerID)
        {
            InitializeComponent();

            this.customerID = customerID;

            WindowStateSaver wss = new WindowStateSaver(this);
            wss.ManageSplitter(splitContainerDetails, "Details");
            wss.ManageSplitter(splitContainerHistoryTop, "HistoryTop");
            wss.ManageSplitter(splitContainerHistoryBottom, "HistoryBottom");
        }

        /// <summary>
        /// Open the editor for the given customer id
        /// </summary>
        public static void Open(long customerID, IWin32Window owner)
        {
            if (openCustomers.Contains(customerID))
            {
                MessageHelper.ShowInformation(owner, "The customer is already open for editing.");
            }
            else
            {
                using (CustomerEditorDlg dlg = new CustomerEditorDlg(customerID))
                {
                    dlg.ShowDialog(owner);
                }
            }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        [NDependIgnoreLongMethod]
        private void OnLoad(object sender, EventArgs e)
        {
            ordersControl.Initialize(ordersLayoutID, GridColumnDefinitionSet.OrderPanel, (GridColumnLayout layout) =>
            {
                layout.AllColumns[OrderFields.RollupNoteCount].Visible = false;
                layout.AllColumns[OrderFields.ShipLastName].Visible = false;
                layout.AllColumns[OrderFields.ShipCountryCode].Visible = false;

                // Hide all the store specific columns
                foreach (GridColumnPosition column in layout.AllColumns.Where(c => c.Definition.StoreTypeCode != null))
                {
                    column.Visible = false;
                }
            });

            emailControl.Initialize(emailLayoutID, GridColumnDefinitionSet.EmailOutboundPanel, (GridColumnLayout layout) =>
            {
                layout.AllColumns[EmailOutboundFields.ToList].Width = 83;
                layout.AllColumns[EmailOutboundFields.Subject].Width = 93;

                layout.AllColumns[EmailOutboundFields.ContextID].Position = 1;
            });

            shipmentsControl.Initialize(shipmentLayoutID, GridColumnDefinitionSet.ShipmentPanel, (GridColumnLayout layout) =>
            {
                layout.AllColumns[ShipmentFields.OrderID].Visible = true;
                layout.AllColumns[ShipmentFields.OrderID].Width = 90;
                layout.AllColumns[ShipmentFields.ShipmentType].Width = 70;
                layout.AllColumns[ShipmentFields.ShipDate].Width = 90;
                layout.AllColumns[ShipmentFields.ShipDate].Visible = true;
                layout.AllColumns[ShipmentFields.ProcessedDate].Width = 90;
                layout.AllColumns[ShipmentFields.VoidedDate].Width = 90;
                layout.AllColumns[ShipmentFields.TrackingNumber].Width = 90;
            });

            printResultControl.Initialize(printLayoutID, GridColumnDefinitionSet.PrintResult, (GridColumnLayout layout) =>
            {

            });

            noteControl.Initialize(notesLayoutID, GridColumnDefinitionSet.Notes, null);

            ordersControl.LoadState();
            noteControl.LoadState();
            emailControl.LoadState();
            shipmentsControl.LoadState();
            printResultControl.LoadState();

            // See if the user can edit the order as a whole
            if (!UserSession.Security.HasPermission(PermissionType.CustomersCreateEdit, customerID))
            {
                shipBillControl.Dock = DockStyle.Fill;
                editAddress.Visible = false;
            }

            openCustomers.Add(customerID);

            Activated += this.OnFormActivated;
            OnFormActivated(this, EventArgs.Empty);
        }

        /// <summary>
        /// Activation
        /// </summary>
        private void OnFormActivated(object sender, EventArgs e)
        {
            if (!UpdateEditorContent())
            {
                panelHeader.Visible = false;
                optionControl.Visible = false;

                Activated -= new EventHandler(OnFormActivated);

                BeginInvoke((MethodInvoker) delegate
                {
                    MessageHelper.ShowInformation(this, "The customer has been deleted.");

                    Close();
                });
            }
        }

        /// <summary>
        /// Update the contents of the editor
        /// </summary>
        private bool UpdateEditorContent()
        {
            customer = (CustomerEntity) DataProvider.GetEntity(customerID);

            if (customer != null)
            {
                ordersControl.ChangeContent(customerID);
                noteControl.ChangeContent(customerID);
                emailControl.ChangeContent(customerID);
                shipmentsControl.ChangeContent(customerID);
                printResultControl.ChangeContent(customerID);

                shipBillControl.LoadEntity(customer);

                LoadHeader();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Load the header information for the customer
        /// </summary>
        private void LoadHeader()
        {
            labelCustomer.Text = string.Format("{0} {1}", customer.BillFirstName, customer.BillLastName);
            labelOrderCount.Text = customer.RollupOrderCount.ToString();

            DateTime? mostRecent = null;

            using (SqlAdapter adapter = new SqlAdapter())
            {
                object result = adapter.GetScalar(OrderFields.OrderDate, null, AggregateFunction.Max, OrderFields.CustomerID == customerID);

                if (result is DateTime)
                {
                    mostRecent = (DateTime) result;
                }
            }

            labelMostRecent.Text = string.Format("Most Recent Order: {0}", mostRecent != null ? StringUtility.FormatFriendlyDateTime(mostRecent.Value) : "Never");
            labelTotalSpent.Text = string.Format("Total Spent: {0}", customer.RollupOrderTotal.ToString("c"));

            labelMostRecent.Left = panelHeader.Width - labelMostRecent.Width;
            labelTotalSpent.Left = labelMostRecent.Left + 34;
        }

        /// <summary>
        /// Create a new order
        /// </summary>
        private void OnAddOrder(object sender, EventArgs e)
        {
            using (AddOrderWizard dlg = new AddOrderWizard(customerID, null))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    UpdateEditorContent();
                }
            }
        }
        /// <summary>
        /// When an order is deleted, we have to update the header summary info
        /// </summary>
        private void OnOrderDeleted(object sender, EventArgs e)
        {
            UpdateEditorContent();
        }

        /// <summary>
        /// Open the ship\bill address for editing
        /// </summary>
        private void OnEditAddress(object sender, EventArgs e)
        {
            using (ShipBillAddressEditorDlg dlg = new ShipBillAddressEditorDlg(customer))
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            if (customer != null)
            {
                shipBillControl.SavePendingChanges();
            }

            ordersControl.SaveState();
            noteControl.SaveState();
            emailControl.SaveState();
            shipmentsControl.SaveState();
            printResultControl.SaveState();

            openCustomers.Remove(customerID);
        }
    }
}
