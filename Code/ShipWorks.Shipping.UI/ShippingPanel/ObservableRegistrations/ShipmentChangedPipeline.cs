using System;
using System.Linq;
using System.Reactive.Linq;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Pipeline for changing shipments
    /// </summary>
    public class ShipmentChangedPipeline : IShippingPanelObservableRegistration
    {
        readonly IObservable<IShipWorksMessage> messages;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentChangedPipeline(IObservable<IShipWorksMessage> messages)
        {
            this.messages = messages;
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public IDisposable Register(ShippingPanelViewModel viewModel)
        {
            return messages.OfType<ShipmentChangedMessage>()
                .Subscribe(msg => OnShipmentChanged(msg, viewModel));
        }

        /// <summary>
        /// Event for updating the view model when a shipment has changed.
        /// </summary>
        private void OnShipmentChanged(ShipmentChangedMessage shipmentChangedMessage, ShippingPanelViewModel viewModel)
        {
            // Don't handle shipment changed messages from ourselves
            if (viewModel.Equals(shipmentChangedMessage.Sender))
            {
                return;
            }

            if (shipmentChangedMessage?.ShipmentAdapter?.Shipment == null ||
                viewModel.Shipment == null)
            {
                return;
            }

            if (shipmentChangedMessage.ShipmentAdapter.Shipment.ShipmentID == viewModel.Shipment.ShipmentID)
            {
                viewModel.Populate(shipmentChangedMessage.ShipmentAdapter);
            }
        }
    }
}
