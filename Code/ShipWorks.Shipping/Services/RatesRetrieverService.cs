using System;
using System.Linq;
using System.Reactive.Linq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Threading;
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
        private readonly IMessenger messenger;
        private readonly IRatesRetriever ratesRetriever;
        private readonly IIndex<ShipmentTypeCode, IRateHashingService> rateHashingServiceLookup;
        private readonly ISchedulerProvider schedulerProvider;
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public RatesRetrieverService(IMessenger messenger, IRatesRetriever ratesRetriever,
            IIndex<ShipmentTypeCode, IRateHashingService> rateHashingServiceLookup,
            ISchedulerProvider schedulerProvider)
        {
            this.messenger = messenger;
            this.ratesRetriever = ratesRetriever;
            this.rateHashingServiceLookup = rateHashingServiceLookup;
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Initialize the service for the current session
        /// </summary>
        public void InitializeForCurrentSession()
        {
            subscription = messenger.OfType<ShipmentChangedMessage>()
                .Select(x => new { Message = x, HashingService = rateHashingServiceLookup[x.ShipmentAdapter.ShipmentTypeCode] })
                .Where(x => x.HashingService.IsRatingField(x.Message.ChangedField))
                .Select(x => new { ShipmentAdapter = x.Message.ShipmentAdapter, RatingHash = x.HashingService.GetRatingHash(x.Message.ShipmentAdapter.Shipment) })
                .Do(x => messenger.Send(new RatesRetrievingMessage(this, x.RatingHash)))
                .Throttle(TimeSpan.FromMilliseconds(250))
                .Select(x => new { x.ShipmentAdapter, x.RatingHash, Rates = ratesRetriever.GetRates(x.ShipmentAdapter.Shipment) })
                .Subscribe(x => messenger.Send(new RatesRetrievedMessage(this, x.RatingHash, x.Rates, x.ShipmentAdapter)));
        }

        /// <summary>
        /// Dispose of any resources
        /// </summary>
        public void Dispose() => subscription?.Dispose();
    }
}
