using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using log4net;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Handle when shipments have been processed
    /// </summary>
    public class ShipmentsProcessedPipeline : IShippingPanelTransientPipeline
    {
        private readonly IObservable<IShipWorksMessage> messageStream;
        private readonly ICarrierShipmentAdapterFactory shipmentAdapterFactory;
        private readonly ISchedulerProvider schedulerProvider;
        private readonly ILog log;
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentsProcessedPipeline(IObservable<IShipWorksMessage> messageStream,
            ICarrierShipmentAdapterFactory shipmentAdapterFactory,
            ISchedulerProvider schedulerProvider,
            Func<Type, ILog> logManager)
        {
            this.messageStream = messageStream;
            this.shipmentAdapterFactory = shipmentAdapterFactory;
            this.schedulerProvider = schedulerProvider;
            log = logManager(typeof(ShipmentsProcessedMessage));
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = messageStream.OfType<ShipmentsProcessedMessage>()
                .Select(x => x.Shipments.FirstOrDefault(r => r.Shipment.ShipmentID == viewModel.Shipment.ShipmentID))
                .Where(x => x.Shipment != null)
                .ObserveOn(schedulerProvider.Dispatcher)
                .CatchAndContinue((Exception ex) => log.Error("An error occurred while handling processed shipment", ex))
                .Subscribe(x => HandleShipmentsProcessed(viewModel, x));
        }

        /// <summary>
        /// Handle the shipment processed message
        /// </summary>
        public void HandleShipmentsProcessed(ShippingPanelViewModel viewModel, ProcessShipmentResult processResults)
        {
            ICarrierShipmentAdapter shipmentAdapter = shipmentAdapterFactory.Get(processResults.Shipment);
            viewModel.LoadShipment(shipmentAdapter);

            viewModel.AllowEditing = !processResults.Shipment?.Processed ?? true;
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
