using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Handle when shipments have been processed
    /// </summary>
    public class ShipmentsProcessedPipeline : IShippingPanelObservableRegistration
    {
        private readonly IObservable<IShipWorksMessage> messageStream;
        private readonly ICarrierShipmentAdapterFactory shipmentAdapterFactory;
        private readonly ISchedulerProvider schedulerProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentsProcessedPipeline(IObservable<IShipWorksMessage> messageStream,
            ICarrierShipmentAdapterFactory shipmentAdapterFactory,
            ISchedulerProvider schedulerProvider)
        {
            this.messageStream = messageStream;
            this.shipmentAdapterFactory = shipmentAdapterFactory;
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public IDisposable Register(ShippingPanelViewModel viewModel)
        {
            return messageStream.OfType<ShipmentsProcessedMessage>()
                .Select(x => x.Shipments.FirstOrDefault(r => r.Shipment.ShipmentID == viewModel.Shipment.ShipmentID))
                .Where(x => x.Shipment != null)
                .ObserveOn(schedulerProvider.Dispatcher)
                .SubscribeWithRetry(x => HandleShipmentsProcessed(viewModel, x));
        }

        /// <summary>
        /// Handle the shipment processed message
        /// </summary>
        public void HandleShipmentsProcessed(ShippingPanelViewModel viewModel, ProcessShipmentResult processResults)
        {
            ICarrierShipmentAdapter shipmentAdapter = shipmentAdapterFactory.Get(processResults.Shipment);
            viewModel.Populate(shipmentAdapter);

            viewModel.AllowEditing = !processResults.Shipment?.Processed ?? true;
        }
    }
}
