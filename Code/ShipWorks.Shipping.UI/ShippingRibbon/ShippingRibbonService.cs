using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Shipping;

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
        private IEnumerable<long> currentOrderIDs;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingRibbonService(IMessenger messages)
        {
            this.messages = messages;
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

            subscription = new CompositeDisposable(
                messages.OfType<OrderSelectionChangedMessage>().Subscribe(HandleOrderSelectionChanged),
                messages.OfType<ShipmentsProcessedMessage>().Subscribe(HandleShipmentsProcessed),
                messages.OfType<ShipmentsVoidedMessage>().Subscribe(HandleLabelsVoided)
            );
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
            else if (currentOrderIDs?.Any() == true)
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
            else if (currentOrderIDs?.Any() == true)
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

            if (loadedOrders.Count == 1)
            {
                currentShipment = loadedOrders[0].ShipmentAdapters.Count() == 1 ?
                    loadedOrders[0].ShipmentAdapters.Single().Shipment :
                    null;
                currentOrderIDs = Enumerable.Empty<long>();
            }
            else
            {
                currentShipment = null;
                currentOrderIDs = orderSelections.Select(x => x.OrderID).ToList();
            }

            SetEnabledOnButtons();
        }

        /// <summary>
        /// Update which buttons are enabled and which are disabled
        /// </summary>
        private void SetEnabledOnButtons()
        {
            if (currentShipment != null)
            {
                shippingRibbonActions.CreateLabel.Enabled = !currentShipment.Processed;
                shippingRibbonActions.Void.Enabled = currentShipment.Processed && !currentShipment.Voided;
                shippingRibbonActions.Return.Enabled = currentShipment.Processed && !currentShipment.Voided;
                shippingRibbonActions.Reprint.Enabled = currentShipment.Processed && !currentShipment.Voided;
                shippingRibbonActions.ShipAgain.Enabled = currentShipment.Processed;
            }
            else
            {
                shippingRibbonActions.CreateLabel.Enabled = currentOrderIDs.Any();
                shippingRibbonActions.Void.Enabled = currentOrderIDs.Any();
                shippingRibbonActions.Return.Enabled = false;
                shippingRibbonActions.Reprint.Enabled = false;
                shippingRibbonActions.ShipAgain.Enabled = false;
            }
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
