using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared;
using ShipWorks.Stores.Communication;
using ShipWorks.UI.Wizard;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Data.Controls;
using ShipWorks.Filters;
using ShipWorks.UI;
using ShipWorks.Data.Connection;
using ShipWorks.Stores.Platforms;
using ShipWorks.Templates.Tokens;
using log4net;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Stores.Content.Panels;
using ShipWorks.Data.Model;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Grid.Paging;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Wizard for adding a new order to ShipWorks
    /// </summary>
    [NDependIgnoreLongTypes]
    public partial class AddOrderWizard : WizardForm
    {
        static readonly ILog log = LogManager.GetLogger(typeof(AddOrderWizard));

        OrderEntity order;
        long? customerID;

        long? initialStoreID;

        Guid itemsLayoutID = new Guid("{3B2EE2CB-7F3F-46c5-A858-3B219CBC70AA}");
        Guid chargesLayoutID = new Guid("{84F6F636-8261-425b-9F92-903890E1E07F}");
        static Guid noteSettingsKey = new Guid("{04DBAEA2-3EB9-49c9-A5B5-8BA5E035F692}");

        ContextMenuStrip menuOrderStatus;

        /// <summary>
        /// Constructor
        /// </summary>
        public AddOrderWizard(long? initialCustomerID, long? initialStoreID)
        {
            InitializeComponent();

            this.customerID = initialCustomerID;
            this.initialStoreID = initialStoreID;

            order = new OrderEntity(-6);
            order.InitializeNullsToDefault();

            order.BillCountryCode = "US";
            order.ShipCountryCode = "US";

            radioOrderNumberGenerate.Checked = true;

            labelOrderPreview.Text = "";

            UpdateOrderStatusLinkText();
        }

        /// <summary>
        /// The ID of the order that was created.  Only valid if DialogResult is OK.
        /// </summary>
        public long OrderID
        {
            get
            {
                if (DialogResult == DialogResult.OK)
                {
                    return order.OrderID;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        [NDependIgnoreLongMethod]
        private void OnLoad(object sender, EventArgs e)
        {
            customerName.Visible = false;

            linkSelectCustomer.Text = "Select...";
            linkSelectCustomer.Left = customerName.Left;

            ordersPlaced.Text = "";
            amountSpent.Text = "";

            var validStores = (from store in StoreManager.GetAllStores()
                 where UserSession.Security.HasPermission(PermissionType.OrdersModify, store.StoreID)
                 select new { Key = store.StoreName, Value = store })
                .ToList();

            comboStores.DisplayMember = "Key";
            comboStores.ValueMember = "Value";
            comboStores.DataSource = validStores;

            if (comboStores.Items.Count == 0)
            {
                throw new PermissionException(UserSession.User, PermissionType.OrdersModify);
            }

            if (initialStoreID != null)
            {
                int initialIndex = validStores.FindIndex(v => v.Value.StoreID == initialStoreID);
                if (initialIndex >= 0)
                {
                    comboStores.SelectedIndex = initialIndex;
                }
            }

            if (comboStores.Items.Count > 0 && comboStores.SelectedIndex < 0)
            {
                comboStores.SelectedIndex = 0;
            }

            if (StoreManager.GetAllStores().Count == 1)
            {
                labelStore.Visible = false;
                comboStores.Visible = false;

                panelCustomer.Top = labelStore.Top;
            }

            shipBillControl.LoadEntity(order);

            if (UpdateAssignedCustomerUI(customerID))
            {
                radioAssignCustomer.Checked = true;
            }
            else
            {
                radioAutomaticCustomer.Checked = true;
            }

            invoiceControl.Initialize(itemsLayoutID, chargesLayoutID, PanelDataMode.LocalPending);

            noteControl.Initialize(noteSettingsKey, GridColumnDefinitionSet.Notes, PanelDataMode.LocalPending, null);
            noteControl.LoadState();

            noteControl.ChangeContent(order.OrderID);
            invoiceControl.UpdateContent(order.OrderID);
        }

        /// <summary>
        /// Update the UI for the assigned customer for the give customerID
        /// </summary>
        private bool UpdateAssignedCustomerUI(long? customerID)
        {
            CustomerEntity customer = null;

            if (customerID != null)
            {
                customer = (CustomerEntity) DataProvider.GetEntity(customerID.Value);
            }

            if (customer != null)
            {
                customerName.Visible = true;
                customerName.Text = string.Format("{0} {1}", customer.BillFirstName, customer.BillLastName);

                linkSelectCustomer.Text = "Change...";
                linkSelectCustomer.Left = customerName.Right;

                ordersPlaced.Text = customer.RollupOrderCount.ToString();
                amountSpent.Text = customer.RollupOrderTotal.ToString("c");
            }

            this.customerID = customer != null ? customer.CustomerID : (long?) null;

            if (customer != null && radioAssignCustomer.Checked)
            {
                PersonAdapter.Copy(customer, order, "Bill");
                PersonAdapter.Copy(customer, order, "Ship");

                shipBillControl.LoadEntity(order);
            }
            else
            {
                OrderEntity blankAddress = new OrderEntity { ShipCountryCode = "US", BillCountryCode = "US" };
                blankAddress.InitializeNullsToDefault();

                PersonAdapter.Copy(blankAddress, order, "Bill");
                PersonAdapter.Copy(blankAddress, order, "Ship");

                shipBillControl.LoadEntity(order);
            }

            return customer != null;
        }

        /// <summary>
        /// Changing how to choose the customer
        /// </summary>
        private void OnChangeCustomerSource(object sender, EventArgs e)
        {
            linkSelectCustomer.Enabled = radioAssignCustomer.Checked;

            UpdateAssignedCustomerUI(customerID);
        }

        /// <summary>
        /// Change the customer selected
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
                    UpdateAssignedCustomerUI(dlg.Selection.Keys.First());
                }
            }
        }

        /// <summary>
        /// Change how the order number is provided
        /// </summary>
        private void OnChangeOrderNumberSource(object sender, EventArgs e)
        {
            orderPrefix.Enabled = radioOrderNumberSpecify.Checked;
            orderNumber.Enabled = radioOrderNumberSpecify.Checked;
            orderPostfix.Enabled = radioOrderNumberSpecify.Checked;
        }

        /// <summary>
        /// User has changed the manually entered order number
        /// </summary>
        private void OnOrderNumberChanged(object sender, EventArgs e)
        {
            labelOrderPreview.Text = string.Format("{0}{1}{2}", orderPrefix.Text, orderNumber.Text, orderPostfix.Text);
        }

        /// <summary>
        /// The StoreID selected that the order will be added to
        /// </summary>
        private StoreEntity SelectedStore
        {
            get { return (StoreEntity) comboStores.SelectedValue; }
        }

        /// <summary>
        /// Stepping next from the first page
        /// </summary>
        private void OnInitialPageStepNext(object sender, WizardStepEventArgs e)
        {
            if (radioAssignCustomer.Checked)
            {
                if (customerID == null)
                {
                    MessageHelper.ShowMessage(this, "Please select a customer.");
                    e.NextPage = CurrentPage;

                    return;
                }
            }

            if (radioOrderNumberSpecify.Checked)
            {
                if (orderNumber.Text.Trim().Length == 0)
                {
                    MessageHelper.ShowInformation(this, "Please enter an order number.");
                    e.NextPage = CurrentPage;

                    return;
                }

                long number;
                if (!long.TryParse(orderNumber.Text.Trim(), out number))
                {
                    MessageHelper.ShowInformation(this, "The number you entered is too large to be used as an order number.");
                    e.NextPage = CurrentPage;

                    return;
                }
            }

            invoiceControl.LocalStoreID = SelectedStore.StoreID;

            UpdateOrderStatusMenu();
        }

        /// <summary>
        /// Update the menu for allowing the user to choose an order status
        /// </summary>
        private void UpdateOrderStatusMenu()
        {
            menuOrderStatus = new ContextMenuStrip();
            menuOrderStatus.Items.AddRange(MenuCommandConverter.ToToolStripItems(
                StatusPresetCommands.CreateMenuCommands(StatusPresetTarget.Order, new List<long> { SelectedStore.StoreID }),
                OnSetOrderStatus));
        }

        /// <summary>
        /// Set the order status
        /// </summary>
        private void OnSetOrderStatus(object sender, EventArgs e)
        {
            MenuCommand command = MenuCommandConverter.ExtractMenuCommand(sender);

            StatusPresetEntity preset = (StatusPresetEntity) command.Tag;

            order.LocalStatus = preset.StatusText;

            UpdateOrderStatusLinkText();

            if (TemplateTokenProcessor.HasTokens(preset.StatusText))
            {
                MessageHelper.ShowInformation(this, "ShipWorks will process the tokens in the status text when the order is saved.");
            }
        }

        /// <summary>
        /// Open the local status menu so the user can select
        /// </summary>
        private void OnLinkLocalStatus(object sender, EventArgs e)
        {
            menuOrderStatus.Show(linkStatus.Parent.PointToScreen(new Point(linkStatus.Left, linkStatus.Bottom)));
        }

        /// <summary>
        /// Update the displayed text of the order status link
        /// </summary>
        private void UpdateOrderStatusLinkText()
        {
            linkStatus.Text = order.LocalStatus.Length == 0 ? "(None)" : order.LocalStatus;
        }

        /// <summary>
        /// Stepping next on the last page
        /// </summary>
        [NDependIgnoreLongMethod]
        private void OnFinish(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            StoreEntity store = SelectedStore;
            StoreType storeType = StoreTypeManager.GetType(store);

            string orderStatusText = StatusPresetManager.GetStoreDefault(store, StatusPresetTarget.Order).StatusText;
            string itemStatusText = StatusPresetManager.GetStoreDefault(store, StatusPresetTarget.OrderItem).StatusText;

            // Set some values
            order.StoreID = store.StoreID;
            order.OrderDate = DateTime.UtcNow;
            order.OrderTotal = OrderUtility.CalculateTotal(invoiceControl.LocalItems, invoiceControl.LocalCharges);
            order.OnlineLastModified = DateTime.UtcNow;
            order.IsManual = true;

            // Save the address info
            shipBillControl.SavePendingChanges();

            // Requested shipping
            order.RequestedShipping = requestedShipping.Text;

            // Generate and apply the order number
            if (radioOrderNumberGenerate.Checked)
            {
                try
                {
                    storeType.GenerateManualOrderNumber(order);
                }
                catch (NotSupportedException ex)
                {
                    MessageHelper.ShowError(this, "An order number could not be retrieved from Miva Merchant.\n\nDetails: " + ex.Message);

                    e.NextPage = CurrentPage;
                    return;
                }
            }
            else
            {
                order.OrderNumber = long.Parse(orderNumber.Text);
                order.ApplyOrderNumberPrefix(orderPrefix.Text.Trim());
                order.ApplyOrderNumberPostfix(orderPostfix.Text.Trim());

                if (OrderCollection.GetCount(SqlAdapter.Default, OrderFields.OrderNumberComplete == order.OrderNumberComplete & OrderFields.StoreID == order.StoreID) > 0)
                {
                    DialogResult result = MessageHelper.ShowQuestion(this, MessageBoxIcon.Warning,
                        string.Format("There is already an order '{0}' in store '{1}'.\n\n", order.OrderNumberComplete, store.StoreName) +
                        "Continue and create the order anyway?");

                    if (result != DialogResult.OK)
                    {
                        e.NextPage = wizardPageStoreCustomer;
                        return;
                    }
                }
            }

            // Apply default status to order.  If tokenized, it has to be processed after the save.
            if (string.IsNullOrEmpty(order.LocalStatus))
            {
                order.LocalStatus = orderStatusText;
            }

            // Apply default status to items.  If tokenized, it will be processed after the save.
            foreach (OrderItemEntity item in invoiceControl.LocalItems)
            {
                if (string.IsNullOrEmpty(item.LocalStatus))
                {
                    item.LocalStatus = itemStatusText;
                }
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                // Do in a transaction
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    // Get the customer
                    if (radioAssignCustomer.Checked)
                    {
                        order.CustomerID = customerID.Value;
                    }
                    else
                    {
                        try
                        {
                            order.CustomerID = CustomerProvider.AcquireCustomer(order, storeType, adapter);
                        }
                        catch (CustomerAcquisitionLockException)
                        {
                            MessageHelper.ShowError(this, string.Format("ShipWorks was unable to find the customer in the time allotted.  Please try saving again."));
                            e.NextPage = wizardPageDetails;
                            return;
                        }
                    }

                    // Save the order so we can get its OrderID
                    adapter.SaveAndRefetch(order);

                    // Apply default order status, if it contained tokens it has to be after the save.
                    if (TemplateTokenProcessor.HasTokens(order.LocalStatus))
                    {
                        order.LocalStatus = TemplateTokenProcessor.ProcessTokens(order.LocalStatus, order.OrderID);
                        adapter.SaveAndRefetch(order);
                    }

                    // Save the notes (use clones, in case the save fails, the original's are left as they were)
                    foreach (NoteEntity note in noteControl.LocalNotes.Select(n => EntityUtility.CloneEntity(n)))
                    {
                        note.ObjectID = order.OrderID;

                        NoteManager.SaveNote(note);
                    }

                    // Save items, and apply item status, if it contained token it has to be after the save
                    foreach (OrderItemEntity item in invoiceControl.LocalItems.Select(i => EntityUtility.CloneEntity(i)))
                    {
                        item.OrderID = order.OrderID;
                        adapter.SaveAndRefetch(item);

                        if (TemplateTokenProcessor.HasTokens(item.LocalStatus))
                        {
                            item.LocalStatus = TemplateTokenProcessor.ProcessTokens(item.LocalStatus, item.OrderItemID);
                            adapter.SaveAndRefetch(item);
                        }
                    }

                    // Save all charges
                    foreach (OrderChargeEntity charge in invoiceControl.LocalCharges.Select(c => EntityUtility.CloneEntity(c)))
                    {
                        charge.OrderID = order.OrderID;
                        adapter.SaveAndRefetch(charge);
                    }

                    // Everything has been set on the order, so calculate the hash key
                    OrderUtility.PopulateOrderDetails(order, adapter);
                    OrderUtility.UpdateShipSenseHashKey(order);

                    adapter.SaveAndRefetch(order);

                    adapter.Commit();
                }
            }
            catch (SqlForeignKeyException ex)
            {
                log.Error("Error adding order", ex);

                MessageHelper.ShowError(this, "The selected customer or store has been deleted.");

                e.NextPage = wizardPageStoreCustomer;
            }
        }
    }
}
