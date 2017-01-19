using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Divelements.SandGrid;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.AddressValidation;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Dialogs;
using ShipWorks.Messaging.Messages.Panels;
using ShipWorks.Properties;
using ShipWorks.Shipping;
using ShipWorks.Stores.Content.Panels.Selectors;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Stores.Content.Panels
{
    /// <summary>
    /// Control for displaying shipments of an order
    /// </summary>
    public partial class ShipmentsPanel : SingleSelectPanelBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipmentsPanel));

        private OrderEntity loadedOrder;
        private bool isThisPanelVisible;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentsPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets all the entity keys from the grid
        /// </summary>
        public IEnumerable<long> EntityKeys =>
            entityGrid.EntityGateway?.GetOrderedKeys() ?? Enumerable.Empty<long>();

        /// <summary>
        /// Default shipment selection
        /// </summary>
        public IEntityGridRowSelector DefaultShipmentSelection =>
            EntityGridRowSelector.SpecificEntities(EntityKeys.OrderByDescending(x => x).Take(1));

        /// <summary>
        /// Initialization
        /// </summary>
        public override void Initialize(Guid settingsKey, GridColumnDefinitionSet definitionSet, Action<GridColumnLayout> layoutInitializer)
        {
            base.Initialize(settingsKey, definitionSet, layoutInitializer);

            // Load the copy menu
            menuCopy.DropDownItems.AddRange(entityGrid.CreateCopyMenuItems(false));

            ILifetimeScope globalLifetimeScope = IoC.UnsafeGlobalLifetimeScope;
            IMessenger messenger = globalLifetimeScope.Resolve<IMessenger>();
            ISchedulerProvider schedulerProvider = globalLifetimeScope.Resolve<ISchedulerProvider>();

            messenger.Where(x => x.Sender is ShippingDlg)
                .Subscribe(_ => ReloadContent());

            // Update the shipment when the provider changes. This keeps things in sync, updates the displayed rates
            // immediately. This means we no longer need to hide rates when the shipping pane is shown because they
            // should not get out of sync.
            messenger.OfType<ShipmentChangedMessage>()
                .Where(x => x.ChangedField == ShipmentFields.ShipmentType.Name)
                .Do(_ => Program.MainForm.ForceHeartbeat())
                .Subscribe(_ => ReloadContent());

            messenger.OfType<OrderSelectionChangedMessage>()
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Do(x => ratesControl.Visible = true)
                .Do(LoadSelectedOrder)
                .Do(x => ReloadContent())
                .Do(x => SelectShipmentRows(isThisPanelVisible ? x.ShipmentSelector : DefaultShipmentSelection))
                .Subscribe();

            // So that we don't show UPS rates with other rates, we hide the rate control when opening the shipping dialog
            // (Scenario: Shipments panel could be showing FedEx rates, opening ship dlg, switch to UPS, move ship dlg and see rates
            //  for both UPS and FedEx at the same time)
            messenger.OfType<OpenShippingDialogMessage>()
                .ObserveOn(schedulerProvider.Dispatcher)
                .Subscribe(_ =>
                {
                    ratesControl.Visible = false;
                });

            HandleRatingPanelToggle(messenger);

            HandleShipmentsPanelToggle(messenger);
        }

        /// <summary>
        /// Listen for when the shipments panel is added or removed
        /// </summary>
        private void HandleShipmentsPanelToggle(IMessenger messenger)
        {
            messenger.OfType<PanelShownMessage>()
                .Where(x => DockPanelIdentifiers.IsShipmentsPanel(x.Panel))
                .Subscribe(_ => isThisPanelVisible = true);

            messenger.OfType<PanelHiddenMessage>()
                .Where(x => DockPanelIdentifiers.IsShipmentsPanel(x.Panel))
                .Subscribe(_ =>
                {
                    if (entityGrid.Selection.Count > 1)
                    {
                        SelectShipmentRows(DefaultShipmentSelection);
                    }

                    isThisPanelVisible = false;
                });
        }

        /// <summary>
        /// Listen for when the rating panel is added or removed
        /// </summary>
        private void HandleRatingPanelToggle(IMessenger messenger)
        {
            messenger.OfType<PanelShownMessage>()
                .Where(x => DockPanelIdentifiers.IsRatingPanel(x.Panel))
                .Subscribe(_ => ratesControl.Visible = false);

            messenger.OfType<PanelHiddenMessage>()
                .Where(x => DockPanelIdentifiers.IsRatingPanel(x.Panel))
                .Subscribe(_ =>
                {
                    ratesControl.Visible = true;
                    RefreshSelectedShipments();
                });
        }

        /// <summary>
        /// Select the required shipment rows
        /// </summary>
        private void SelectShipmentRows(IEntityGridRowSelector shipmentsToSelect)
        {
            shipmentsToSelect.Select(entityGrid);
        }

        /// <summary>
        /// Loads the order that has been selected
        /// </summary>
        /// <remarks>if more than one order is selected, set loaded order to null</remarks>
        /// <param name="orderSelectionChangedMessage"></param>
        private void LoadSelectedOrder(OrderSelectionChangedMessage orderSelectionChangedMessage)
        {
            // If a single order is selected set loaded order to that order
            if (orderSelectionChangedMessage.LoadedOrderSelection.CompareCountTo(1) == ComparisonResult.Equal)
            {
                LoadedOrderSelection orderSelection =
                    orderSelectionChangedMessage.LoadedOrderSelection.OfType<LoadedOrderSelection>().FirstOrDefault();

                loadedOrder = orderSelection.Order;
            }
            else
            {
                // More than one or no order has been selected
                loadedOrder = null;
            }
        }

        /// <summary>
        /// EntityType displayed by this panel
        /// </summary>
        public override EntityType EntityType => EntityType.ShipmentEntity;

        /// <summary>
        /// The targets this supports
        /// </summary>
        public override FilterTarget[] SupportedTargets => new[] { FilterTarget.Orders, FilterTarget.Customers };

        /// <summary>
        /// Update the gateway query filter and reload the grid
        /// </summary>
        protected override IEntityGateway CreateGateway(long entityID)
        {
            EntityType type = EntityUtility.GetEntityType(entityID);

            RelatedKeysEntityGateway gateway = new RelatedKeysEntityGateway(entityID, EntityType.ShipmentEntity);

            if (type == EntityType.OrderEntity)
            {
                entityGrid.EmptyText = "The order has no shipments.";
            }
            else
            {
                entityGrid.EmptyText = "The customer has no shipments.";
            }

            // Can't add shipments directly to a customer
            addLink.Visible =
                (type == EntityType.OrderEntity) &&
                UserSession.Security.HasPermission(PermissionType.ShipmentsCreateEditProcess, entityID);

            return gateway;
        }

        /// <summary>
        /// Layout is updating
        /// </summary>
        protected override void UpdateLayout()
        {
            base.UpdateLayout();

            // Update the top of the rates panel to be just under the add link
            ratesControl.Top = addLink.Bottom + 5;
        }

        /// <summary>
        /// When the content is called to be updated, we need to make sure our rates are up to date as well
        /// </summary>
        public override Task UpdateContent()
        {
            Task task = base.UpdateContent();

            RefreshSelectedShipments();
            return task;
        }

        /// <summary>
        /// The shipment grid has finished loading.  Check to see if there are any shipments, and if there are not, we create one by default.
        /// </summary>
        private void OnShipmentGridLoaded(object sender, EventArgs e)
        {
            if (EntityID == null || entityGrid.Rows.Count == 0)
            {
                ratesControl.ChangeShipment(null);
            }
            else
            {
                RefreshSelectedShipments();
            }
        }

        /// <summary>
        /// The current shipment grid selection has changed
        /// </summary>
        private void OnShipmentSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshSelectedShipments();

            if (isThisPanelVisible)
            {
                Messenger.Current.Send(new ShipmentSelectionChangedMessage(this, entityGrid.Selection.Keys));
            }
        }

        /// <summary>
        /// Refreshes the selected shipments - Updates the rate control
        /// </summary>
        private void RefreshSelectedShipments()
        {
            if (!ratesControl.Visible)
            {
                return;
            }

            int shipmentSelectionCount = entityGrid.Selection.Count;

            if (entityGrid.Rows.Count == 1)
            {
                ratesControl.ChangeShipment(entityGrid.EntityGateway.GetKeyFromRow(0));
            }
            else if (shipmentSelectionCount == 1)
            {
                ratesControl.ChangeShipment(entityGrid.Selection.Keys.First());
            }
            else if (shipmentSelectionCount > 1)
            {
                ratesControl.ClearRates("Multiple shipments selected.");
            }
            else
            {
                ratesControl.ClearRates("No shipments are selected.");
            }
        }

        /// <summary>
        /// Add a new shipment
        /// </summary>
        private async void OnAddShipment(object sender, EventArgs e)
        {
            Debug.Assert(EntityID != null);
            if (EntityID == null)
            {
                return;
            }

            try
            {
                ShipmentEntity shipment = ShippingManager.CreateShipment(EntityID.Value);
                await ValidatedAddressManager.ValidateShipmentAsync(shipment, new AddressValidator());

                Messenger.Current.Send(new OpenShippingDialogMessage(this, new[] { shipment }));
            }
            catch (SqlForeignKeyException)
            {
                MessageHelper.ShowMessage(this, "The order of the shipment has been deleted.");
            }
        }

        /// <summary>
        /// An action has occurred in one of the cells, like a hyperlink click.
        /// </summary>
        private void OnGridCellLinkClicked(object sender, GridHyperlinkClickEventArgs e)
        {
            EntityGridRow row = e.Row;

            if (row.EntityID == null)
            {
                MessageHelper.ShowMessage(this, "The shipment data is not yet loaded.");
                return;
            }

            // If the action data is an event handler, execute it and stop processing
            var actionMethod = ((GridActionDisplayType) e.Column.DisplayType).ActionData as Action<object, GridHyperlinkClickEventArgs>;
            if (actionMethod != null)
            {
                actionMethod(sender, e);
                return;
            }

            // If the action data is not an event handler, just assume it's a GridLinkAction
            long entityID = row.EntityID.Value;

            GridLinkAction action = (GridLinkAction) ((GridActionDisplayType) e.Column.DisplayType).ActionData;

            if (action == GridLinkAction.Delete)
            {
                DeleteShipment(entityID);
            }

            if (action == GridLinkAction.Edit)
            {
                EditShipments(new List<long> { entityID }, InitialShippingTabDisplay.Shipping);
            }
        }

        /// <summary>
        /// An entity row has been double clicked
        /// </summary>
        protected override void OnEntityDoubleClicked(long entityID)
        {
            EditShipments(new[] { entityID }, InitialShippingTabDisplay.Shipping);
        }

        /// <summary>
        /// Edit the selected shipment
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            if (entityGrid.Selection.Count > 0)
            {
                EditShipments(entityGrid.Selection.Keys, InitialShippingTabDisplay.Shipping);
            }
        }

        /// <summary>
        /// Delete the selected shipment
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            if (entityGrid.Selection.Count == 1)
            {
                DeleteShipment(entityGrid.Selection.Keys.First());
            }
        }

        /// <summary>
        /// Copy the tracking number of the selected shipment
        /// </summary>
        private void OnCopyTracking(object sender, EventArgs e)
        {
            if (entityGrid.Selection.Count == 1)
            {
                ShipmentEntity shipment = loadedOrder?.Shipments.FirstOrDefault(s => s.ShipmentID == entityGrid.Selection.Keys.First());
                if (shipment != null)
                {
                    try
                    {
                        if (shipment.TrackingNumber.Length > 0)
                        {
                            Clipboard.SetText(shipment.TrackingNumber);
                        }
                        else
                        {
                            Clipboard.Clear();
                        }
                    }
                    catch (ExternalException ex)
                    {
                        log.Error("Copy tracking", ex);

                        MessageHelper.ShowError(this, "ShipWorks could not copy to the clipboard because it is in use by another application.");
                    }
                }
            }
        }

        /// <summary>
        /// Track the selected shipment
        /// </summary>
        private void OnTrackShipment(object sender, EventArgs e)
        {
            OpenSingleShipmentInDialog(InitialShippingTabDisplay.Tracking);
        }

        /// <summary>
        /// Track the selected shipment
        /// </summary>
        private void OnSubmitClaim(object sender, EventArgs e)
        {
            OpenSingleShipmentInDialog(InitialShippingTabDisplay.Insurance);
        }

        /// <summary>
        /// Open a single shipment in the dialog
        /// </summary>
        private void OpenSingleShipmentInDialog(InitialShippingTabDisplay initialTab)
        {
            if (entityGrid.Selection.Count == 1)
            {
                EditShipments(new[] { entityGrid.Selection.Keys.First() }, initialTab);
            }
        }

        /// <summary>
        /// Edit the shipment with the given ID
        /// </summary>
        private void EditShipments(IEnumerable<long> shipmentKeys, InitialShippingTabDisplay initialTab)
        {
            // Get the entities from the grid because the local loadedOrder
            // shipment entities could be out of sync with whats stored locally
            IEnumerable<ShipmentEntity> shipments = shipmentKeys.Select(entityGrid.EntityGateway.GetEntityFromKey).OfType<ShipmentEntity>();
            Messenger.Current.Send(new OpenShippingDialogMessage(this, shipments, initialTab));
        }

        /// <summary>
        /// Refresh the existing selected content by re-querying for the relevant keys to ensure an up-to-date related row
        /// list with up-to-date displayed entity content.
        /// </summary>
        public override Task ReloadContent()
        {
            Task task = base.ReloadContent();

            RefreshSelectedShipments();
            return task;
        }

        /// <summary>
        /// Delete the given shipment
        /// </summary>
        private void DeleteShipment(long shipmentID)
        {
            if (loadedOrder != null)
            {
                DialogResult result = MessageHelper.ShowQuestion(this, "Delete the selected shipment?");

                if (result == DialogResult.OK)
                {
                    ShipmentEntity shipment = loadedOrder?.Shipments.FirstOrDefault(s => s.ShipmentID == shipmentID);

                    if (shipment == null)
                    {
                        MessageHelper.ShowMessage(this, "The shipment has already been deleted.");
                    }
                    else
                    {
                        ShippingManager.DeleteShipment(EntityUtility.CloneEntity(shipment, false));
                        Messenger.Current.Send(new OrderSelectionChangingMessage(this, new[] { loadedOrder.OrderID }));
                    }

                    ReloadContent();
                }
            }
        }

        /// <summary>
        /// The context menu is opening
        /// </summary>
        private void OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            menuEdit.Enabled = entityGrid.Selection.Count >= 1;
            menuDelete.Enabled = entityGrid.Selection.Count == 1;
            menuTrack.Enabled = entityGrid.Selection.Count == 1;
            menuCopyTracking.Enabled = entityGrid.Selection.Count == 1;
            menuInsuranceClaim.Enabled = entityGrid.Selection.Count == 1;
            menuCopy.Enabled = entityGrid.Selection.Count >= 1;

            string editText = "Ship";
            Image editImage = Resources.edit16;

            if (EntityID != null && !UserSession.Security.HasPermission(PermissionType.ShipmentsCreateEditProcess, EntityID.Value))
            {
                editText = "View";
                editImage = Resources.view;
            }
            else
            {
                // If there's multiple, just show edit, even if there all processed.  Probably for no better
                // reason than I'm being lazy right now.
                if (entityGrid.Selection.Count == 1)
                {
                    ShipmentEntity shipment = loadedOrder?.Shipments.FirstOrDefault(s => s.ShipmentID == entityGrid.Selection.Keys.First());
                    if (shipment != null)
                    {
                        if (shipment.Processed || shipment.Voided)
                        {
                            editText = "View";
                            editImage = Resources.view;
                        }
                    }
                }
            }

            menuEdit.Text = editText;
            menuEdit.Image = editImage;

            menuDelete.Visible = EntityID == null || UserSession.Security.HasPermission(PermissionType.ShipmentsVoidDelete, EntityID.Value);
        }
    }
}
