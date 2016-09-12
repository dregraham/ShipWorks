﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Core.UI.SandRibbon;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Dialogs;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping.UI.ShippingRibbon
{
    /// <summary>
    /// Service that handles interaction with the shipping ribbon
    /// </summary>
    public class ShippingRibbonService : IShippingRibbonService, IDisposable
    {
        readonly IMessenger messages;
        IShippingRibbonActions shippingRibbonActions;
        IDisposable subscription;
        private ShipmentEntity currentShipment;
        private IEnumerable<long> currentOrderIDs = Enumerable.Empty<long>();
        private readonly Func<ISecurityContext> securityContextRetriever;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingRibbonService(IMessenger messages, Func<ISecurityContext> securityContextRetriever)
        {
            this.messages = messages;
            this.securityContextRetriever = securityContextRetriever;
        }

        /// <summary>
        /// Register a set of actions with the ribbon service
        /// </summary>
        public void Register(IShippingRibbonActions actions)
        {
            shippingRibbonActions = actions;

            shippingRibbonActions.CreateLabel.Enabled = false;
            shippingRibbonActions.CreateLabel.Activate += OnCreateLabel;

            shippingRibbonActions.Void.Enabled = false;
            shippingRibbonActions.Void.Activate += OnVoidLabel;

            shippingRibbonActions.Return.Enabled = false;
            shippingRibbonActions.Return.Activate += OnCreateReturn;

            shippingRibbonActions.Reprint.Enabled = false;
            shippingRibbonActions.Reprint.Activate += OnReprintLabel;

            shippingRibbonActions.ShipAgain.Enabled = false;
            shippingRibbonActions.ShipAgain.Activate += OnShipAgain;

            shippingRibbonActions.ApplyProfile.Enabled = false;
            shippingRibbonActions.ApplyProfile.Activate += OnApplyProfile;

            shippingRibbonActions.ManageProfiles.Activate += OnManageProfiles;

            subscription = new CompositeDisposable(
                messages.OfType<ShipmentChangedMessage>().Subscribe(HandleShipmentChanged),
                messages.OfType<OrderSelectionChangedMessage>().Subscribe(HandleOrderSelectionChanged),
                messages.OfType<ShipmentsProcessedMessage>().Subscribe(HandleShipmentsProcessed),
                messages.OfType<ShipmentsVoidedMessage>().Subscribe(HandleLabelsVoided)
            );
        }

        /// <summary>
        /// Manage profiles
        /// </summary>
        private void OnManageProfiles(object sender, EventArgs e)
        {
            messages.Send(new OpenProfileManagerDialogMessage(this));
        }

        /// <summary>
        /// Apply a profile to a shipment
        /// </summary>
        private void OnApplyProfile(object sender, EventArgs e)
        {
            ShippingProfileEntity profile = (sender as IRibbonButton)?.Tag as ShippingProfileEntity;

            if (currentShipment != null && !currentShipment.Processed && profile != null)
            {
                messages.Send(new ApplyProfileMessage(this, currentShipment.ShipmentID, profile));
            }
        }

        /// <summary>
        /// Ship again
        /// </summary>
        private void OnShipAgain(object sender, EventArgs e)
        {
            if (currentShipment != null && currentShipment.Processed)
            {
                messages.Send(new ShipAgainMessage(this, currentShipment));
            }
        }

        /// <summary>
        /// Reprint the label
        /// </summary>
        private void OnReprintLabel(object sender, EventArgs e)
        {
            if (currentShipment != null && currentShipment.Processed && !currentShipment.Voided)
            {
                messages.Send(new ReprintLabelsMessage(this, new[] { currentShipment }));
            }
        }

        /// <summary>
        /// Create return shipment
        /// </summary>
        private void OnCreateReturn(object sender, EventArgs e)
        {
            if (currentShipment != null && currentShipment.Processed && !currentShipment.Voided)
            {
                messages.Send(new CreateReturnShipmentMessage(this, currentShipment));
            }
        }

        /// <summary>
        /// Void label
        /// </summary>
        private void OnVoidLabel(object sender, EventArgs e)
        {
            if (currentShipment != null && currentShipment.Processed && !currentShipment.Voided)
            {
                messages.Send(new VoidLabelMessage(this, currentShipment.ShipmentID));
            }
            else if (currentOrderIDs.Count() > 1)
            {
                messages.Send(new OpenShippingDialogWithOrdersMessage(this, currentOrderIDs));
            }
        }

        /// <summary>
        /// Create label
        /// </summary>
        private void OnCreateLabel(object sender, EventArgs e)
        {
            if (currentShipment != null && !currentShipment.Processed)
            {
                messages.Send(new CreateLabelMessage(this, currentShipment.ShipmentID));
            }
            else if (currentOrderIDs.Count() > 1)
            {
                messages.Send(new OpenShippingDialogWithOrdersMessage(this, currentOrderIDs));
            }
        }

        /// <summary>
        /// Handle shipments processed message
        /// </summary>
        private void HandleShipmentsProcessed(ShipmentsProcessedMessage message)
        {
            ShipmentEntity processedShipment = message.Shipments.Select(x => x.Shipment).FirstOrDefault(x => x.ShipmentID == currentShipment.ShipmentID);
            if (processedShipment != null)
            {
                currentShipment = processedShipment;
            }

            SetEnabledOnButtons();
        }

        /// <summary>
        /// Handle shipments voided message
        /// </summary>
        private void HandleLabelsVoided(ShipmentsVoidedMessage message)
        {
            ShipmentEntity voidedShipment = message.VoidShipmentResults.
                Select(x => x.Shipment).
                FirstOrDefault(x => x.ShipmentID == currentShipment.ShipmentID);

            if (voidedShipment?.ShipmentID == currentShipment.ShipmentID)
            {
                currentShipment = voidedShipment;
            }

            SetEnabledOnButtons();
        }

        /// <summary>
        /// Handle order selection changed message
        /// </summary>
        private void HandleOrderSelectionChanged(OrderSelectionChangedMessage message)
        {
            List<IOrderSelection> orderSelections = message.LoadedOrderSelection.ToList();
            List<LoadedOrderSelection> loadedOrders = orderSelections.OfType<LoadedOrderSelection>().ToList();

            currentShipment = null;
            currentOrderIDs = orderSelections.Select(x => x.OrderID).ToList();

            if (loadedOrders.Count == 1)
            {
                currentShipment = loadedOrders[0].ShipmentAdapters.Count() == 1 ?
                    loadedOrders[0].ShipmentAdapters.Single().Shipment :
                    null;
            }

            SetEnabledOnButtons();
        }

        /// <summary>
        /// Handle order selection changed message
        /// </summary>
        private void HandleShipmentChanged(ShipmentChangedMessage message)
        {
            // We only want to set the current shipment if it's an updated version
            // We only handle changing the shipment when the order selection changes
            if (currentShipment?.ShipmentID == message.ShipmentAdapter?.Shipment?.ShipmentID)
            {
                currentShipment = message.ShipmentAdapter?.Shipment;
            }

            SetEnabledOnButtons();
        }

        /// <summary>
        /// Update which buttons are enabled and which are disabled
        /// </summary>
        private void SetEnabledOnButtons()
        {
            // If the shipment is null or it's a non-supported shipping panel carrier, disable buttons
            if (currentShipment == null ||   
                currentShipment.ShipmentTypeCode == ShipmentTypeCode.None ||
                currentShipment.ShipmentTypeCode == ShipmentTypeCode.Amazon)
            {
                    shippingRibbonActions.CreateLabel.Enabled = false;
                    shippingRibbonActions.Void.Enabled = false;
                    shippingRibbonActions.Return.Enabled = false;
                    shippingRibbonActions.Reprint.Enabled = false;
                    shippingRibbonActions.ShipAgain.Enabled = false;
                    shippingRibbonActions.ApplyProfile.Enabled = false;
            }
            else
            {
                // We have a shipment, set the buttons as needed
                SetEnabledOnButtonsWithSecurity();
            }

            // Update the current shipment type
            shippingRibbonActions.SetCurrentShipmentType(currentShipment?.ShipmentTypeCode);

            // Update manage profiles button state
            SetManageProfilesEnabled();
        }

        /// <summary>
        /// Set the enabled state for the manage profiles button.
        /// 
        /// If order with no shipment is selected, disable the button.
        /// If a single order is selected for which the user has shipment create/edit/process rights, enable the button.
        /// If a single order is selected for which the user does NOT shipment create/edit/process rights, disable the button.
        /// If multiple orders are selected and the user has permission for each, enable the button.
        /// If multiple orders are selected and the user does NOT have permission for at least one, disable the button.
        /// </summary>
        private void SetManageProfilesEnabled()
        {
            ISecurityContext securityContext = securityContextRetriever();

            // Get the list of current order ids.  If a single order is selected, this will be empty.
            List<long> orderIDs = currentOrderIDs.ToList();
            
            // If no orders selected or the user does not have permission for at least one order, return false.  Otherwise, true.
            bool manageProfilesAllowed = orderIDs.Any() && !orderIDs.Any(orderID => !securityContext.HasPermission(PermissionType.ShipmentsCreateEditProcess, orderID));

            shippingRibbonActions.ManageProfiles.Enabled = manageProfilesAllowed;
        }

        /// <summary>
        /// Set button enabled state for shipment that needs to check security
        /// </summary>
        private void SetEnabledOnButtonsWithSecurity()
        {
            ISecurityContext securityContext = securityContextRetriever();

            bool shipmentsCreateEditProcessAllowed = securityContext.HasPermission(PermissionType.ShipmentsCreateEditProcess, currentShipment.OrderID);
            bool shipmentsVoidDelete = securityContext.HasPermission(PermissionType.ShipmentsVoidDelete, currentShipment.OrderID);

            shippingRibbonActions.CreateLabel.Enabled = !currentShipment.Processed && shipmentsCreateEditProcessAllowed;
            shippingRibbonActions.Void.Enabled = currentShipment.Processed && !currentShipment.Voided && shipmentsVoidDelete;
            shippingRibbonActions.Return.Enabled = currentShipment.Processed && !currentShipment.Voided && shipmentsCreateEditProcessAllowed;
            shippingRibbonActions.Reprint.Enabled = currentShipment.Processed && !currentShipment.Voided;
            shippingRibbonActions.ShipAgain.Enabled = currentShipment.Processed && shipmentsCreateEditProcessAllowed;
            shippingRibbonActions.ApplyProfile.Enabled = !currentShipment.Processed && shipmentsCreateEditProcessAllowed;
        }

        /// <summary>
        /// Dispose held resources
        /// </summary>
        public void Dispose()
        {
            subscription?.Dispose();
        }
    }
}