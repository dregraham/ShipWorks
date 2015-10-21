using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI;
using ShipWorks.Data;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Connection;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.Data.Controls;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Filters;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Administration.Retry;
using Autofac;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Window for editing an order
    /// </summary>
    public partial class OrderEditorDlg : Form
    {
        long orderID;

        OrderEntity order;
        CustomerEntity customer;

        Guid itemsLayoutID = new Guid("{1FCDA8BC-40A4-449c-A1A2-83F737840DEF}");
        Guid chargesLayoutID = new Guid("{3A128D19-98CF-4d22-96D0-4F3432E56079}");
        Guid notesLayoutID = new Guid("{E512C8D1-F7A2-4a2c-9339-0ABB5D2DEFA4}");
        Guid shipmentLayoutID = new Guid("{BBA5C927-F1C1-428d-8FED-1610C53C5B3B}");
        Guid emailLayoutID = new Guid("{3BB5B706-9C72-4bc3-83F8-75AE44587997}");
        Guid printLayoutID = new Guid("{EB59D1CE-6AD7-47ef-8DC9-0FB7AA844D30}");
        Guid auditLayoutID = new Guid("{7279A34F-1607-4e4c-8D50-4502C05F4817}");

        static List<long> openOrders = new List<long>();

        /// <summary>
        /// Constructor
        /// </summary>
        private OrderEditorDlg(long orderID)
        {
            InitializeComponent();

            WindowStateSaver wss = new WindowStateSaver(this, WindowStateSaverOptions.Size);
            wss.ManageSplitter(splitContainerDetails, "Details");
            wss.ManageSplitter(splitContainerHistoryTop, "HistoryTop");
            wss.ManageSplitter(splitContainerHistoryBottom, "HistoryBottom");

            this.orderID = orderID;
        }

        /// <summary>
        /// Open the editor for the given order id
        /// </summary>
        public static void Open(long orderID, IWin32Window owner)
        {
            if (openOrders.Contains(orderID))
            {
                MessageHelper.ShowInformation(owner, "The order is already open for editing.");
            }
            else
            {
                using (OrderEditorDlg dlg = new OrderEditorDlg(orderID))
                {
                    dlg.ShowDialog(owner);
                }
            }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            invoiceControl.Initialize(itemsLayoutID, chargesLayoutID);

            emailControl.Initialize(emailLayoutID, GridColumnDefinitionSet.EmailOutboundPanel, (GridColumnLayout layout) =>
            {
                layout.AllColumns[EmailOutboundFields.ContextID].Visible = false;
                layout.AllColumns[EmailOutboundFields.ContextID].Position = 1;
            });

            printResultControl.Initialize(printLayoutID, GridColumnDefinitionSet.PrintResult, (GridColumnLayout layout) =>
            {

            });

            shipmentsControl.Initialize(shipmentLayoutID, GridColumnDefinitionSet.ShipmentPanel, (GridColumnLayout layout) =>
            {
                layout.AllColumns[ShipmentFields.ShipDate].Visible = true;
            });

            noteControl.Initialize(notesLayoutID, GridColumnDefinitionSet.Notes, null);

            noteControl.LoadState();
            shipmentsControl.LoadState();
            emailControl.LoadState();
            printResultControl.LoadState();

            openOrders.Add(orderID);

            // See if the user can edit the order as a whole
            if (!UserSession.Security.HasPermission(PermissionType.OrdersModify, orderID))
            {
                // Can't edit address
                shipBillControl.Dock = DockStyle.Fill;
                editAddress.Visible = false;

                // Can't change customer
                linkChangeCustomer.Visible = false;

                // Can't update requested shipping
                requestedShipping.ReadOnly = true;
            }
            
            // See if the user can edit the status
            if (!UserSession.Security.HasPermission(PermissionType.OrdersEditStatus, orderID))
            {
                linkStatus.Font = Font;
                linkStatus.ForeColor = SystemColors.WindowText;
                linkStatus.Cursor = Cursors.Default;
                linkStatus.Click -= this.OnLinkLocalStatus;
            }

            if (!UserSession.Security.HasPermission(PermissionType.ManageUsers))
            {
                optionControl.OptionPages.Remove(optionPageAudit);
            }
            else
            {
                // Load the audit
                auditControl.Initialize(auditLayoutID, null);

                // Lock the control into displaying stuff for this order
                auditControl.LockOrderSearchCriteria(orderID);
            }

            Activated += this.OnFormActivated;
            OnFormActivated(this, EventArgs.Empty);
        }

        /// <summary>
        /// The form has been activated.  This could be from coming back from a child modal window, or just switching from another application back to SW.
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
                    MessageHelper.ShowInformation(this, "The order has been deleted.");

                    Close();
                });
            }
        }
        
        /// <summary>
        /// Update the contents of the editor
        /// </summary>
        private bool UpdateEditorContent()
        {
            order = (OrderEntity) DataProvider.GetEntity(orderID);
            if (order != null)
            {
                customer = (CustomerEntity) DataProvider.GetEntity(order.CustomerID);
            }

            if (order != null && customer != null)
            {
                StoreEntity store = StoreManager.GetStore(order.StoreID);

                if (store != null)
                {
                    invoiceControl.UpdateContent(order.OrderID);
                    noteControl.ChangeContent(orderID);
                    shipmentsControl.ChangeContent(orderID);
                    emailControl.ChangeContent(orderID);
                    printResultControl.ChangeContent(orderID);
                    shipBillControl.LoadEntity(order);

                    linkStatus.Text = order.LocalStatus.Length == 0 ? "(None)" : order.LocalStatus;
                    requestedShipping.Text = order.RequestedShipping;

                    LoadHeader(store);

                    return true;
                }
            }

            order = null;
            customer = null;

            return false;
        }

        /// <summary>
        /// Load the header data
        /// </summary>
        private void LoadHeader(StoreEntity store)
        {
            labelOrder.Text = string.Format("Order {0}", order.OrderNumberComplete);
            labelCustomer.Text = string.Format("{0} {1}", customer.BillFirstName, customer.BillLastName);

            if (customer.RollupOrderCount > 1)
            {
                labelCustomerOrderCount.Visible = true;
                labelCustomerOrderCount.Text = string.Format("({0} other order{1})", customer.RollupOrderCount - 1, customer.RollupOrderCount > 2 ? "s" : "");
                labelCustomerOrderCount.Left = labelCustomer.Right - 3;
                viewCustomer.Left = labelCustomerOrderCount.Right;
            }
            else
            {
                labelCustomerOrderCount.Visible = false;
                viewCustomer.Left = labelCustomer.Right;
            }

            linkChangeCustomer.Left = viewCustomer.Right;

            labelStore.Text = string.Format("Store: {0}", store.StoreName);
            labelOrderDate.Text = string.Format("Date Placed: {0}", StringUtility.FormatFriendlyDateTime(order.OrderDate));

            int dateSpaceNeeded = labelOrderDate.Width;
            int storeSpaceNeeded = 31 + labelStore.Width;

            int leftStart = panelHeader.Width - Math.Max(dateSpaceNeeded, storeSpaceNeeded);
            labelOrderDate.Left = leftStart;
            labelStore.Left = leftStart + 31;
        }

        /// <summary>
        /// Open the local status menu so the user can select
        /// </summary>
        private void OnLinkLocalStatus(object sender, EventArgs e)
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            List<MenuCommand> commands = StatusPresetCommands.CreateMenuCommands(StatusPresetTarget.Order, new List<long> { order.StoreID });
            contextMenu.Items.AddRange(MenuCommandConverter.ToToolStripItems(commands, OnChangeLocalStatus));

            contextMenu.Show(linkStatus.Parent.PointToScreen(new Point(linkStatus.Left, linkStatus.Bottom)));
        }

        /// <summary>
        /// Changing the local status
        /// </summary>
        private void OnChangeLocalStatus(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            MenuCommand command = MenuCommandConverter.ExtractMenuCommand(sender);

            // Execute the command
            command.ExecuteAsync(this, new long[] { orderID }, OnAsyncSetStatusCompleted);
        }

        /// <summary>
        /// Called when an async entity operation has completed
        /// </summary>
        private void OnAsyncSetStatusCompleted(object sender, EventArgs e)
        {
            OnFormActivated(this, EventArgs.Empty);
        }

        /// <summary>
        /// Open the customer editor window
        /// </summary>
        private void OnViewCustomer(object sender, EventArgs e)
        {
            CustomerEditorDlg.Open(customer.CustomerID, this);
        }

        /// <summary>
        /// Change the customer that this order is attached to
        /// </summary>
        private void OnChangeCustomer(object sender, EventArgs e)
        {
            using (EntityPickerDlg dlg = new EntityPickerDlg(FilterTarget.Customers))
            {
                dlg.FormClosing += (o, args) =>
                {
                    if (dlg.DialogResult == DialogResult.OK)
                    {
                        if (dlg.Selection.Count != 1)
                        {
                            MessageHelper.ShowInformation(this, "Please select a single customer.");
                            args.Cancel = true;
                        }
                    }
                };

                if (dlg.ShowDialog(this) == DialogResult.OK && dlg.Selection.Count == 1)
                {
                    long customerID = dlg.Selection.Keys.First();

                    order.CustomerID = customerID;

                    SqlAdapter.Default.SaveAndRefetch(order);
                    UpdateEditorContent();
                }
            }
        }

        /// <summary>
        /// Edit the ship\bill address
        /// </summary>
        private void OnEditAddress(object sender, EventArgs e)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ShipBillAddressEditorDlg dlg = lifetimeScope.Resolve<ShipBillAddressEditorDlg>(
                    new TypedParameter(typeof(EntityBase2), order), 
                    new TypedParameter(typeof(bool), true));
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            if (order != null)
            {
                order.RequestedShipping = requestedShipping.Text;
                shipBillControl.SavePendingChanges();

                SqlAdapterRetry<SqlException> sqlDeadlockRetry = new SqlAdapterRetry<SqlException>(5, -6, string.Format("OrderEditorDlg.OnClosing for OrderID {0}", order.OrderID));
                sqlDeadlockRetry.ExecuteWithRetry(adapter =>
                {
                    try
                    {
                        adapter.SaveAndRefetch(order);

                        // Everything has been set on the order, so calculate the hash key
                        OrderUtility.PopulateOrderDetails(order, adapter);
                        OrderUtility.UpdateShipSenseHashKey(order);
                        adapter.SaveAndRefetch(order);
                    }
                    catch (SqlForeignKeyException)
                    {
                        MessageHelper.ShowWarning(this,
                                                  "This order has already been edited or deleted by other users.\n\n" +
                                                  "Your changes to this order were not saved.");
                    }
                    catch (ORMConcurrencyException)
                    {
                        MessageHelper.ShowWarning(this,
                                                  "This order has already been edited or deleted by other users.\n\n" +
                                                  "Your changes to this order were not saved.");
                    }
                });
            }

            invoiceControl.SaveState();
            noteControl.SaveState();
            shipmentsControl.SaveState();
            emailControl.SaveState();
            printResultControl.SaveState();

            openOrders.Remove(orderID);
        }
    }
}
