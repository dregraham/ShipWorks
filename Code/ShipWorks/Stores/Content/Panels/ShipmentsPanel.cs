﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
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
using ShipWorks.Shipping.Services;
using ShipWorks.Stores.Content.Panels.Selectors;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using TD.SandDock;

namespace ShipWorks.Stores.Content.Panels
{
    /// <summary>
    /// Control for displaying shipments of an order
    /// </summary>
    public partial class ShipmentsPanel : SingleSelectPanelBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ShipmentsPanel));

        private LoadedOrderSelection loadedOrderSelection;
        private bool isThisPanelVisible;
        private bool isRatingPanelVisible;
        private IEnumerable<long> selectedShipments;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentsPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Should the show rates panel link be allowed
        /// </summary>
        public bool AllowRatesPanelLink { get; set; } = true;

        /// <summary>
        /// Extra space at the bottom that can be used for other controls
        /// </summary>
        protected override int ExtraBottomSpace =>
            rateMessagePanel.Visible ? 18 : 0;

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

            // Initialize the value for the show rates panel link.
            rateMessagePanel.Visible = AllowRatesPanelLink && !isRatingPanelVisible;

            // Load the copy menu
            menuCopy.DropDownItems.AddRange(entityGrid.CreateCopyMenuItems(false));

            ILifetimeScope globalLifetimeScope = IoC.UnsafeGlobalLifetimeScope;
            IMessenger messenger = globalLifetimeScope.Resolve<IMessenger>();
            ISchedulerProvider schedulerProvider = globalLifetimeScope.Resolve<ISchedulerProvider>();

            HandleShipmentChanges(messenger);
            HandleOrderSelectionChanges(messenger, schedulerProvider);
            HandleShippingDialogMessages(messenger, schedulerProvider);
            HandleRatingPanelToggle(messenger);
            HandleShipmentsPanelToggle(messenger);
        }

        /// <summary>
        /// Handle messages related to the Shipping Dialog
        /// </summary>
        private void HandleShippingDialogMessages(IMessenger messenger, ISchedulerProvider schedulerProvider)
        {
            messenger.Where(x => x.Sender is ShippingDlg).Subscribe(_ => ReloadContent());

            // So that we don't show UPS rates with other rates, we hide the rate control when opening the shipping dialog
            // (Scenario: Shipments panel could be showing FedEx rates, opening ship dlg, switch to UPS, move ship dlg and see rates
            //  for both UPS and FedEx at the same time)
            messenger.OfType<ShippingDialogOpeningMessage>()
                .ObserveOn(schedulerProvider.Dispatcher)
                .Subscribe(_ =>
                {
                    rateMessagePanel.Visible = false;
                    selectedShipments = entityGrid.Selection.Keys.ToReadOnly();
                });
        }

        /// <summary>
        /// Handle order selection changes
        /// </summary>
        private void HandleOrderSelectionChanges(IMessenger messenger, ISchedulerProvider schedulerProvider)
        {
            messenger.OfType<OrderSelectionChangingMessage>()
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Do(_ => entityGrid.SelectRows(Enumerable.Empty<long>()))
                .Subscribe();

            messenger.OfType<OrderSelectionChangedMessage>()
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Do(x => rateMessagePanel.Visible = AllowRatesPanelLink && !isRatingPanelVisible)
                .Do(LoadSelectedOrder)
                .Do(x => ReloadContent())
                .Do(x => SelectShipmentRows(isThisPanelVisible ?
                    x.ShipmentSelector ?? EntityGridRowSelector.SpecificEntities(selectedShipments) :
                    DefaultShipmentSelection))
                .Do(_ => selectedShipments = Enumerable.Empty<long>())
                .Subscribe();
        }

        /// <summary>
        /// Handle shipment changes
        /// </summary>
        private void HandleShipmentChanges(IMessenger messenger)
        {
            // Update the shipment when the provider changes. This keeps things in sync, updates the displayed rates
            // immediately. This means we no longer need to hide rates when the shipping pane is shown because they
            // should not get out of sync.
            messenger.OfType<ShipmentChangedMessage>()
                .Where(x => x.ChangedField == ShipmentFields.ShipmentType.Name)
                .Do(_ => Program.MainForm.ForceHeartbeat())
                .Subscribe(_ => ReloadContent());

            messenger.OfType<ShipmentChangedMessage>()
                .Where(x => x.Sender is GridProviderDisplayType)
                .Do(x => UpdateStoredShipment(x.ShipmentAdapter))
                .Subscribe();
        }

        /// <summary>
        /// Listen for when the shipments panel is added or removed
        /// </summary>
        private void HandleShipmentsPanelToggle(IMessenger messenger)
        {
            messenger.OfType<PanelShownMessage>()
                .Where(x => DockPanelIdentifiers.IsShipmentsPanel(x.Panel))
                .Do(_ => SelectShipmentRows(DefaultShipmentSelection))
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
                .Do(x => isRatingPanelVisible = true)
                .Subscribe(_ => rateMessagePanel.Visible = AllowRatesPanelLink && !isRatingPanelVisible);

            messenger.OfType<PanelHiddenMessage>()
                .Where(x => DockPanelIdentifiers.IsRatingPanel(x.Panel))
                .Do(x => isRatingPanelVisible = false)
                .Subscribe(_ =>
                {
                    rateMessagePanel.Visible = AllowRatesPanelLink && !isRatingPanelVisible;
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

                loadedOrderSelection = orderSelection;
            }
            else
            {
                // More than one or no order has been selected
                loadedOrderSelection = default(LoadedOrderSelection);
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

            rateMessagePanel.Top = addLink.Bottom + 5;
        }

        /// <summary>
        /// The current shipment grid selection has changed
        /// </summary>
        private void OnShipmentSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isThisPanelVisible)
            {
                IEnumerable<long> keys = entityGrid.Selection.Keys;
                ICarrierShipmentAdapter shipmentAdapter = null;

                if (keys.IsCountEqualTo(1))
                {
                    shipmentAdapter = loadedOrderSelection.ShipmentAdapters?.FirstOrDefault(x => keys.Contains(x.Shipment.ShipmentID));

                    // If the currently loaded order doesn't have this shipment it must
                    // have been an auto return shipment, so reload the current order
                    if (shipmentAdapter == null)
                    {
                        // When this message is received the shipping panel is reloaded, so we don't
                        // need to also send the ShipmentSelectionChangedMessage, and can just return
                        Messenger.Current.Send(new OrderSelectionChangingMessage(this, new List<long>() { loadedOrderSelection.OrderID }));
                        return;
                    }
                }

                Messenger.Current.Send(new ShipmentSelectionChangedMessage(this, keys, shipmentAdapter));
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
                using (var lifetimeScope = IoC.BeginLifetimeScope())
                {
                    var addressValidator = lifetimeScope.Resolve<IAddressValidator>();
                    await ValidatedAddressManager.ValidateShipmentAsync(shipment, addressValidator);
                }

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
                ICarrierShipmentAdapter shipmentAdapter = loadedOrderSelection.ShipmentAdapters
                    .FirstOrDefault(s => s.Shipment.ShipmentID == entityGrid.Selection.Keys.First());
                if (shipmentAdapter != null)
                {
                    try
                    {
                        if (shipmentAdapter.Shipment.TrackingNumber.Length > 0)
                        {
                            Clipboard.SetText(shipmentAdapter.Shipment.TrackingNumber);
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
        /// Delete the given shipment
        /// </summary>
        private void DeleteShipment(long shipmentID)
        {
            if (loadedOrderSelection.ShipmentAdapters == null || loadedOrderSelection.ShipmentAdapters.None())
            {
                return;
            }

            DialogResult result = MessageHelper.ShowQuestion(this, "Delete the selected shipment?");

            if (result == DialogResult.OK)
            {
                ICarrierShipmentAdapter shipmentAdapter = loadedOrderSelection.ShipmentAdapters.FirstOrDefault(s => s.Shipment.ShipmentID == shipmentID);

                if (shipmentAdapter == null)
                {
                    MessageHelper.ShowMessage(this, "The shipment has already been deleted.");
                }
                else
                {
                    ShippingManager.DeleteShipment(EntityUtility.CloneEntity(shipmentAdapter.Shipment, false));
                    Messenger.Current.Send(new OrderSelectionChangingMessage(this, new[] { loadedOrderSelection.OrderID }));
                }

                ReloadContent();
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
                    ICarrierShipmentAdapter shipmentAdapter = loadedOrderSelection.ShipmentAdapters
                        .FirstOrDefault(s => s.Shipment.ShipmentID == entityGrid.Selection.Keys.First());
                    if (shipmentAdapter != null)
                    {
                        if (shipmentAdapter.Shipment.Processed || shipmentAdapter.Shipment.Voided)
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

        /// <summary>
        /// Show the rating panel
        /// </summary>
        private void OnRatesLinkLabelClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DockControl ratingPanel = Program.MainForm.Panels.Where(DockPanelIdentifiers.IsRatingPanel).Single();
            Program.MainForm.ShowPanel(ratingPanel);
        }

        /// <summary>
        /// Update a stored shipment
        /// </summary>
        /// <remarks>
        /// If LoadShipment is called directly without going through LoadOrder, the LoadedShipmentResult could
        /// be out of sync.  So we find the requested shipment in the list of order selection shipment adapters
        /// and replace it with the requested shipment adapter.  Then update the LoadedShipmentResult so that
        /// panels update correctly.
        /// </remarks>
        public virtual void UpdateStoredShipment(ICarrierShipmentAdapter shipmentAdapter) =>
            loadedOrderSelection = loadedOrderSelection.CreateSelectionWithUpdatedShipment(shipmentAdapter);
    }
}
