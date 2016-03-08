﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Divelements.SandGrid;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.AddressValidation;
using ShipWorks.Core.Messaging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Messaging.Messages;
using ShipWorks.Properties;
using ShipWorks.Shipping;
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

        // So we don't race condition on fast selection changes to auto-create more than one shipment for an order
        static HashSet<long> autoCreatingShipments = new HashSet<long>();

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentsPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public override void Initialize(Guid settingsKey, GridColumnDefinitionSet definitionSet, Action<GridColumnLayout> layoutInitializer)
        {
            base.Initialize(settingsKey, definitionSet, layoutInitializer);

            // Load the copy menu
            menuCopy.DropDownItems.AddRange(entityGrid.CreateCopyMenuItems(false));

            Messenger.Current.Where(x => x.Sender is ShippingDlg)
                .Subscribe(_ => ReloadContent());
        }

        /// <summary>
        /// EntityType displayed by this panel
        /// </summary>
        public override EntityType EntityType
        {
            get { return EntityType.ShipmentEntity; }
        }

        /// <summary>
        /// The targets this supports
        /// </summary>
        public override FilterTarget[] SupportedTargets
        {
            get { return new FilterTarget[] { FilterTarget.Orders, FilterTarget.Customers }; }
        }

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
        }

        /// <summary>
        /// Refreshes the selected shipments - Updates the rate control
        /// </summary>
        private void RefreshSelectedShipments()
        {
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
                Messenger.Current.Send(new OrderSelectionChangingMessage(this, new[] { shipment.OrderID }));
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
                EditShipments(new List<long> { entityID });
            }
        }

        /// <summary>
        /// An entity row has been double clicked
        /// </summary>
        protected override void OnEntityDoubleClicked(long entityID)
        {
            EditShipments(new long[] { entityID });
        }

        /// <summary>
        /// Edit the selected shipment
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            if (entityGrid.Selection.Count > 0)
            {
                EditShipments(entityGrid.Selection.Keys);
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
                ShipmentEntity shipment = ShippingManager.GetShipment(entityGrid.Selection.Keys.First());
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
                ShipmentEntity shipment = ShippingManager.GetShipment(entityGrid.Selection.Keys.First());
                if (shipment != null)
                {
                    Messenger.Current.Send(new OpenShippingDialogMessage(this, new[] { shipment }, initialTab));
                }
            }
        }

        /// <summary>
        /// Edit the shipment with the given ID
        /// </summary>
        private void EditShipments(IEnumerable<long> shipmentKeys)
        {
            Messenger.Current.Send(new OpenShippingDialogWithOrdersMessage(this, shipmentKeys));
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
            DialogResult result = MessageHelper.ShowQuestion(this, "Delete the selected shipment?");

            if (result == DialogResult.OK)
            {
                ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
                long orderID = shipment.OrderID;

                if (shipment == null)
                {
                    MessageHelper.ShowMessage(this, "The shipment has already been deleted.");
                }
                else
                {
                    ShippingManager.DeleteShipment(shipment);
                    Messenger.Current.Send(new OrderSelectionChangingMessage(this, new[] { orderID }));
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
                    ShipmentEntity shipment = (ShipmentEntity) DataProvider.GetEntity(entityGrid.Selection.Keys.First());
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
