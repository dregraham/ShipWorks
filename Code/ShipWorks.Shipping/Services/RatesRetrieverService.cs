﻿using System;
using System.Linq;
using System.Reactive.Linq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Shipping;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Service that retrieves rates when shipments change
    /// </summary>
    public class RatesRetrieverService : IInitializeForCurrentSession, IDisposable
    {
        const double ThrottleTime = 250;
        private readonly IMessenger messenger;
        private readonly IRatesRetriever ratesRetriever;
        private readonly IIndex<ShipmentTypeCode, IRateHashingService> rateHashingServiceLookup;
        private readonly ISchedulerProvider schedulerProvider;
        private readonly ILog log;
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public RatesRetrieverService(IMessenger messenger, IRatesRetriever ratesRetriever,
            IIndex<ShipmentTypeCode, IRateHashingService> rateHashingServiceLookup,
            ISchedulerProvider schedulerProvider, Func<Type, ILog> logFactory)
        {
            this.messenger = messenger;
            this.ratesRetriever = ratesRetriever;
            this.rateHashingServiceLookup = rateHashingServiceLookup;
            this.schedulerProvider = schedulerProvider;
            log = logFactory(typeof(RatesRetrieverService));
        }

        /// <summary>
        /// Initialize the service for the current session
        /// </summary>
        public void InitializeForCurrentSession()
        {
            subscription = messenger.OfType<ShipmentChangedMessage>()
                .Where(x => x.ShipmentAdapter != null)
                .Select(x => new { Message = x, HashingService = rateHashingServiceLookup[x.ShipmentAdapter.ShipmentTypeCode] })
                .Where(x => string.IsNullOrEmpty(x.Message.ChangedField) || x.HashingService.IsRatingField(x.Message.ChangedField))
                .Select(x => new
                {
                    //TODO: Clone shipment and adapter because I got a concurrency error
                    x.Message.ShipmentAdapter,
                    RatingHash = x.HashingService.GetRatingHash(x.Message.ShipmentAdapter.Shipment)
                })
                .Do(x => messenger.Send(new RatesRetrievingMessage(this, x.RatingHash)))
                .Throttle(TimeSpan.FromMilliseconds(ThrottleTime), schedulerProvider.Default)
                .ObserveOn(schedulerProvider.TaskPool)
                .Select(x => new { x.ShipmentAdapter, x.RatingHash, Rates = ratesRetriever.GetRates(x.ShipmentAdapter.Shipment) })
                .SubscribeWithRetry(x => messenger.Send(new RatesRetrievedMessage(this, x.RatingHash, x.Rates, x.ShipmentAdapter)), HandleException);
        }

        /// <summary>
        /// Handle exceptions generated while getting rates
        /// </summary>
        private bool HandleException(Exception ex)
        {
            log.Warn(ex);

            return true;
        }

        /// <summary>
        /// Dispose of any resources
        /// </summary>
        public void Dispose() => subscription?.Dispose();
    }
}
