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
using ShipWorks.Shipping.Services;

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
        IEnumerable<IOrderSelection> currentOrderSelection;

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
            shippingRibbonActions.Return.Enabled = false;
            shippingRibbonActions.Reprint.Enabled = false;
            shippingRibbonActions.ShipAgain.Enabled = false;

            subscription = new CompositeDisposable(
                messages.OfType<OrderSelectionChangedMessage>().Subscribe(HandleOrderSelectionChanged),
                messages.OfType<ShipmentsProcessedMessage>().Subscribe(HandleShipmentsProcessed)
            );
        }

        /// <summary>
        /// Create label
        /// </summary>
        private void OnCreateLabel(object sender, EventArgs e)
        {
            if (currentOrderSelection == null)
            {
                return;
            }

            List<ShipmentEntity> shipmentIDs = currentOrderSelection.OfType<LoadedOrderSelection>()
                .SelectMany(x => x.ShipmentAdapters)
                .Select(x => x.Shipment).ToList();

            if (shipmentIDs.Count == 1)
            {
                if (!shipmentIDs[0].Processed)
                {
                    messages.Send(new CreateLabelMessage(this, shipmentIDs[0].ShipmentID));
                }

                return;
            }

            messages.Send(new OpenShippingDialogWithOrdersMessage(this, currentOrderSelection.Select(x => x.OrderID)));
        }

        /// <summary>
        /// Handle shipments processed message
        /// </summary>
        private void HandleShipmentsProcessed(ShipmentsProcessedMessage message)
        {
            //TODO: Implement this
        }

        /// <summary>
        /// Handle order selection changed message
        /// </summary>
        private void HandleOrderSelectionChanged(OrderSelectionChangedMessage message)
        {
            currentOrderSelection = message.LoadedOrderSelection;

            if (message.LoadedOrderSelection.OfType<LoadedOrderSelection>().Any())
            {
                IEnumerable<ICarrierShipmentAdapter> shipments = message.LoadedOrderSelection
                    .OfType<LoadedOrderSelection>()
                    .SelectMany(y => y.ShipmentAdapters);

                shippingRibbonActions.CreateLabel.Enabled = shipments.Any(y => !y.Shipment.Processed);
                shippingRibbonActions.Void.Enabled = shipments.Any(y => y.Shipment.Processed && !y.Shipment.Voided);
                shippingRibbonActions.Return.Enabled = !shipments.Skip(1).Any() && shipments.Any(y => y.Shipment.Processed && !y.Shipment.Voided);
                shippingRibbonActions.Reprint.Enabled = !shipments.Skip(1).Any() && shipments.Any(y => y.Shipment.Processed && !y.Shipment.Voided);
                shippingRibbonActions.ShipAgain.Enabled = !shipments.Skip(1).Any() && shipments.Any(y => y.Shipment.Processed);
                return;
            }

            shippingRibbonActions.CreateLabel.Enabled = message.LoadedOrderSelection.Any();
            shippingRibbonActions.Void.Enabled = message.LoadedOrderSelection.Any();
            shippingRibbonActions.Return.Enabled = false;
            shippingRibbonActions.Reprint.Enabled = false;
            shippingRibbonActions.ShipAgain.Enabled = false;
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
