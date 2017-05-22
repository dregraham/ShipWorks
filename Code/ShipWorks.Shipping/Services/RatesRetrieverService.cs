using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Shipping;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Service that retrieves rates when shipments change
    /// </summary>
    public class RatesRetrieverService : IInitializeForCurrentUISession
    {
        const double ThrottleTime = 250;
        private readonly IMessenger messenger;
        private readonly IRatesRetriever ratesRetriever;
        private readonly IIndex<ShipmentTypeCode, IRateHashingService> rateHashingServiceLookup;
        private readonly ISchedulerProvider schedulerProvider;
        private readonly ILog log;
        private IDisposable subscription;
        private IDisposable multipleSelectionSubscription;

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
            // We should never initialize an already initialized session. We'll re-subscribe in release but when
            // debugging, we should get alerted that this is happening
            Debug.Assert(subscription == null, "Subscription is already initialized");
            EndSession();

            var changedOrders = messenger.OfType<OrderSelectionChangedMessage>()
                .SelectMany(x => x.LoadedOrderSelection)
                .OfType<LoadedOrderSelection>()
                .Select(x => x.ShipmentAdapters.FirstOrDefault())
                .Where(x => x != null)
                .Select(x => new
                {
                    HashingService = rateHashingServiceLookup[x.ShipmentTypeCode],
                    ShipmentAdapter = x
                });

            var changedShipments = messenger.OfType<ShipmentChangedMessage>()
                .Where(x => x.ShipmentAdapter != null)
                .Select(x => new
                {
                    Message = x,
                    HashingService = rateHashingServiceLookup[x.ShipmentAdapter.ShipmentTypeCode]
                })
                .Where(x => string.IsNullOrEmpty(x.Message.ChangedField) || x.HashingService.IsRatingField(x.Message.ChangedField))
                .Select(x => new
                {
                    x.HashingService,
                    x.Message.ShipmentAdapter
                });

            var selectedShipments = messenger.OfType<ShipmentSelectionChangedMessage>()
                .Where(x => x.SelectedShipment != null)
                .Select(x => new
                {
                    HashingService = rateHashingServiceLookup[x.SelectedShipment.ShipmentTypeCode],
                    ShipmentAdapter = x.SelectedShipment
                });

            multipleSelectionSubscription = messenger.OfType<ShipmentSelectionChangedMessage>()
                .Where(x => x.SelectedShipmentIDs.IsCountGreaterThan(1))
                .Do(_ => messenger.Send(new RatesNotSupportedMessage(this, "Unable to get rates for multiple shipments.")))
                .Subscribe();

            // Ignore shipment changes from the GridProvider. This means someone changed the carrier from the
            // shipments panel, and if it is for the current shipment, we'll get another request for rates from
            // the shipping panel
            subscription = changedShipments
                .Merge(changedOrders)
                .Merge(selectedShipments)
                .Select(x => new
                {
                    ShipmentAdapter = x.ShipmentAdapter.Clone(),
                    RatingHash = x.HashingService.GetRatingHash(x.ShipmentAdapter.Shipment)
                })
                .Do(x => messenger.Send(new RatesRetrievingMessage(this, x.RatingHash)))
                .Throttle(TimeSpan.FromMilliseconds(ThrottleTime), schedulerProvider.Default)
                .ObserveOn(schedulerProvider.TaskPool)
                .Select(x => new
                {
                    x.ShipmentAdapter,
                    x.RatingHash,
                    Rates = ratesRetriever.GetRates(x.ShipmentAdapter.Shipment)
                })
                .CatchAndContinue((Exception ex) => log.Error("Error occurred while getting rates", ex))
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Subscribe(x => messenger.Send(new RatesRetrievedMessage(this, x.RatingHash, x.Rates, x.ShipmentAdapter)));
        }

        /// <summary>
        /// End the current session
        /// </summary>
        public void EndSession()
        {
            subscription?.Dispose();
            multipleSelectionSubscription?.Dispose();
        }

        /// <summary>
        /// Dispose of any resources
        /// </summary>
        public void Dispose() => EndSession();
    }
}
