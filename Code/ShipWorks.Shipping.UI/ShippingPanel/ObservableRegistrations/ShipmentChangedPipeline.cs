using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Pipeline for changing shipments
    /// </summary>
    public class ShipmentChangedPipeline : IShippingPanelTransientPipeline
    {
        readonly IObservable<IShipWorksMessage> messages;
        readonly Func<ShippingPanelViewModel, object>[] viewModelsToIgnore = {
            x => x,
            x => x?.ShipmentViewModel,
            x => x?.ShipmentViewModel?.InsuranceViewModel,
            x => x?.Origin,
            x => x?.Destination
        };
        private IDisposable subscription;
        private readonly ISchedulerProvider schedulerProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentChangedPipeline(IObservable<IShipWorksMessage> messages, ISchedulerProvider schedulerProvider)
        {
            this.messages = messages;
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = messages.OfType<ShipmentChangedMessage>()
                .ObserveOn(schedulerProvider.Dispatcher)
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

        /// <summary>
        /// Dispose the subscription
        /// </summary>
        public void Dispose()
        {
            subscription?.Dispose();
        }
    }
}
