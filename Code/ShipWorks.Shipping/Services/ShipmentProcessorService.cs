﻿using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging.TrackedObservable;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ComponentRegistration.Ordering;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.Shipping;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Label processor
    /// </summary>
    [Order(typeof(IInitializeForCurrentSession), Order.Unordered)]
    public class ShipmentProcessorService : IInitializeForCurrentSession, IDisposable
    {
        private readonly IMessenger messenger;
        private readonly IShipmentProcessor shipmentProcessor;
        private readonly Func<ICarrierConfigurationShipmentRefresher> shipmentRefresherFactory;
        private IDisposable subscription;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentProcessorService(IShipmentProcessor shipmentProcessor, IMessenger messenger,
            Func<ICarrierConfigurationShipmentRefresher> shipmentRefresherFactory,
            Func<Type, ILog> logManager)
        {
            this.messenger = messenger;
            this.shipmentProcessor = shipmentProcessor;
            this.shipmentRefresherFactory = shipmentRefresherFactory;
            log = logManager(typeof(ShipmentProcessorService));
        }

        /// <summary>
        /// Initialize the message handler
        /// </summary>
        public void InitializeForCurrentSession()
        {
            subscription?.Dispose();
            subscription = messenger.OfType<ProcessShipmentsMessage>()
                .Trackable()
                .Select(this, x => Observable.FromAsync(() => ProcessShipments(x)).Trackable())
                .Concat(this)
                .CatchAndContinue((Exception ex) => log.Error("An error occurred while creating label", ex))
                .Subscribe();
        }

        /// <summary>
        /// Process the given shipments
        /// </summary>
        private async Task<ShipmentsProcessedMessage> ProcessShipments(ProcessShipmentsMessage message)
        {
            IEnumerable<ProcessShipmentResult> shipments;

            using (ICarrierConfigurationShipmentRefresher refresher = shipmentRefresherFactory())
            {
                refresher.RetrieveShipments = () => message.ShipmentsInContext;

                shipments = await shipmentProcessor.Process(message.Shipments, refresher, message.SelectedRate, null);
            }

            return new ShipmentsProcessedMessage(message.Sender, shipments);
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose() => subscription?.Dispose();
    }
}
