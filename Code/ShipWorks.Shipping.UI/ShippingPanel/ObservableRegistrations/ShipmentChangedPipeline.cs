using System;
using System.Linq;
using System.Reactive.Linq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Pipeline for changing shipments
    /// </summary>
    public class ShipmentChangedPipeline : IShippingPanelObservableRegistration
    {
        readonly IObservable<IShipWorksMessage> messages;
        readonly Func<ShippingPanelViewModel, object>[] viewModelsToIgnore = {
            x => x,
            x => x?.ShipmentViewModel,
            x => x?.ShipmentViewModel?.InsuranceViewModel,
            x => x?.Origin,
            x => x?.Destination
        };

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
            if (IsSenderViewModelOrDescendant(shipmentChangedMessage, viewModel))
            {
                return;
            }

            ShipmentEntity shipment = shipmentChangedMessage?.ShipmentAdapter?.Shipment;
            if (shipment == null)
            {
                return;
            }

            if (viewModel.Shipment == null ||
                shipmentChangedMessage.ShipmentAdapter.Shipment.ShipmentID == viewModel.Shipment.ShipmentID)
            {
                viewModel.LoadShipment(shipmentChangedMessage.ShipmentAdapter);
            }
        }

        /// <summary>
        /// We don't want to reload the shipment if the message was sent by the view model or one of its descendants
        /// </summary>
        private bool IsSenderViewModelOrDescendant(ShipmentChangedMessage shipmentChangedMessage, ShippingPanelViewModel viewModel)
        {
            if (shipmentChangedMessage?.Sender == null)
            {
                return false;
            }

            return viewModelsToIgnore.Select(x => x(viewModel)).Any(vm =>
            {
                return vm?.Equals(shipmentChangedMessage?.Sender) ?? false;
            });
        }
    }
}
