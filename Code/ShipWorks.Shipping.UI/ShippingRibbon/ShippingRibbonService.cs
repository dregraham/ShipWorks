using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Messaging.Messages;
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
            shippingRibbonActions.Void.Enabled = false;
            shippingRibbonActions.Return.Enabled = false;
            shippingRibbonActions.Reprint.Enabled = false;
            shippingRibbonActions.ShipAgain.Enabled = false;

            subscription = messages.OfType<OrderSelectionChangedMessage>()
                .Subscribe(HandleOrderSelectionChanged);
        }

        /// <summary>
        /// Handle order selection changed message
        /// </summary>
        private void HandleOrderSelectionChanged(OrderSelectionChangedMessage message)
        {
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
