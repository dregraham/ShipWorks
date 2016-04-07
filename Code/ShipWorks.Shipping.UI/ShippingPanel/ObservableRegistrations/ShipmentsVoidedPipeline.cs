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
    /// Handle when shipments have been voided
    /// </summary>
    public class ShipmentsVoidedPipeline : IShippingPanelTransientPipeline
    {
        private readonly IObservable<IShipWorksMessage> messageStream;
        private readonly ICarrierShipmentAdapterFactory shipmentAdapterFactory;
        private readonly ISchedulerProvider schedulerProvider;
        private readonly ILog log;
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentsVoidedPipeline(IObservable<IShipWorksMessage> messageStream,
            ICarrierShipmentAdapterFactory shipmentAdapterFactory,
            ISchedulerProvider schedulerProvider,
            Func<Type, ILog> logManager)
        {
            this.messageStream = messageStream;
            this.shipmentAdapterFactory = shipmentAdapterFactory;
            this.schedulerProvider = schedulerProvider;
            log = logManager(typeof(ShipmentsVoidedMessage));
        }

        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        public void Register(ShippingPanelViewModel viewModel)
        {
            subscription = messageStream.OfType<ShipmentsVoidedMessage>()
                .Select(x => x.VoidShipmentResults.FirstOrDefault(r => r.Shipment.ShipmentID == viewModel.Shipment.ShipmentID))
                .Where(x => x.Shipment != null)
                .ObserveOn(schedulerProvider.Dispatcher)
                .CatchAndContinue((Exception ex) => log.Error("An error occurred while handling voiding shipment.", ex))
                .Subscribe(x => HandleShipmentsVoided(viewModel, x));
        }

        /// <summary>
        /// Handle the shipment voided message
        /// </summary>
        public void HandleShipmentsVoided(ShippingPanelViewModel viewModel, VoidShipmentResult voidedShipmentResult)
        {
            ICarrierShipmentAdapter voidedShipmentAdapter = shipmentAdapterFactory.Get(voidedShipmentResult.Shipment);

            viewModel.LoadShipment(voidedShipmentAdapter);

            viewModel.AllowEditing = !voidedShipmentAdapter.Shipment?.Processed ?? true;
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
