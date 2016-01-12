using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.Shipping;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Label processor
    /// </summary>
    public class ShipmentProcessorService : IInitializeForCurrentSession, IDisposable
    {
        private readonly IMessenger messenger;
        private readonly IShipmentProcessor shipmentProcessor;
        private readonly Func<ICarrierConfigurationShipmentRefresher> shipmentRefresherFactory;
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentProcessorService(IShipmentProcessor shipmentProcessor, IMessenger messenger,
            Func<ICarrierConfigurationShipmentRefresher> shipmentRefresherFactory)
        {
            this.messenger = messenger;
            this.shipmentProcessor = shipmentProcessor;
            this.shipmentRefresherFactory = shipmentRefresherFactory;
        }

        /// <summary>
        /// Initialize the message handler
        /// </summary>
        public void InitializeForCurrentSession()
        {
            subscription?.Dispose();
            subscription = messenger.OfType<ProcessShipmentsMessage>()
                .Select(x => Observable.FromAsync(() => ProcessShipments(x)))
                .Concat()
                .SubscribeWithRetry(x => messenger.Send(x));
        }

        /// <summary>
        /// Process the given shipments
        /// </summary>
        private async Task<ShipmentsProcessedMessage> ProcessShipments(ProcessShipmentsMessage message)
        {
            IEnumerable<ProcessShipmentResult> shipments;

            using (ICarrierConfigurationShipmentRefresher refresher = shipmentRefresherFactory())
            {
                shipments = await shipmentProcessor.Process(message.Shipments, refresher, null, null);
            }

            return new ShipmentsProcessedMessage(message.Sender, shipments);
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose() => subscription?.Dispose();
    }
}
