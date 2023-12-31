﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using log4net;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Shipping;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Service that retrieves rates when shipments change
    /// </summary>
    [Component(RegistrationType.Self)]
    public class RatesRetrieverService : IRatesRetrieverService
    {
        private const double ThrottleTime = 250;
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

            multipleSelectionSubscription = messenger.OfType<ShipmentSelectionChangedMessage>()
                .Where(x => x.SelectedShipmentIDs.IsCountGreaterThan(1))
                .Do(_ => messenger.Send(new RatesNotSupportedMessage(this, "Unable to get rates for multiple shipments.")))
                .Subscribe();

            ShipmentEntity selectedShipment = null;

            // Ignore shipment changes from the GridProvider. This means someone changed the carrier from the
            // shipments panel, and if it is for the current shipment, we'll get another request for rates from
            // the shipping panel
            subscription = ChangedShipments()
                .Merge(ChangedOrders())
                .Merge(SelectedShipment())
                .Where(x => x.ShipmentAdapter?.Shipment?.Processed == false)
                .Do(x => selectedShipment = x.ShipmentAdapter.Shipment)
                .Select(x => new
                {
                    ShipmentAdapter = CloneAdapter(x.ShipmentAdapter, x.OnAfterClone),
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
                .Do(x => selectedShipment.BestRateEvents |= x.ShipmentAdapter.Shipment.BestRateEvents)
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Do(x => messenger.Send(new RatesRetrievedMessage(this, x.RatingHash, x.Rates, x.ShipmentAdapter)))
                .CatchAndContinue((Exception ex) => log.Error("Error occurred while getting rates", ex))
                .Subscribe();

            // Clear the Rates UI after login
            messenger.Send(new RatesNotSupportedMessage(this, string.Empty));
        }

        /// <summary>
        /// Subscribe to shipment selection changes
        /// </summary>
        private IObservable<(IRateHashingService HashingService, ICarrierShipmentAdapter ShipmentAdapter, Action<ICarrierShipmentAdapter> OnAfterClone)> SelectedShipment()
        {
            return messenger.OfType<ShipmentSelectionChangedMessage>()
                .Where(x => x.SelectedShipment != null)
                .Select(x =>
                (
                    HashingService: rateHashingServiceLookup[x.SelectedShipment.ShipmentTypeCode],
                    ShipmentAdapter: x.SelectedShipment,
                    OnAfterClone: (Action<ICarrierShipmentAdapter>) null
                ));
        }

        /// <summary>
        /// Subscribe to shipment changed messages
        /// </summary>
        private IObservable<(IRateHashingService HashingService, ICarrierShipmentAdapter ShipmentAdapter, Action<ICarrierShipmentAdapter> OnAfterClone)> ChangedShipments()
        {
            return messenger.OfType<ShipmentChangedMessage>()
                .Where(x => x.ShipmentAdapter != null && !(x.Sender is GridProviderDisplayType))
                .Select(x => new
                {
                    Message = x,
                    HashingService = rateHashingServiceLookup[x.ShipmentAdapter.ShipmentTypeCode],
                })
                .Where(x => string.IsNullOrEmpty(x.Message.ChangedField) || x.HashingService.IsRatingField(x.Message.ChangedField))
                .Select(x =>
                (
                    HashingService: x.HashingService,
                    ShipmentAdapter: x.Message.ShipmentAdapter,
                    OnAfterClone: x.Message.OnAfterClone
                ));
        }

        /// <summary>
        /// Subscribe to ORderSelctionChanged messages
        /// </summary>
        private IObservable<(IRateHashingService HashingService, ICarrierShipmentAdapter ShipmentAdapter, Action<ICarrierShipmentAdapter> OnAfterClone)> ChangedOrders()
        {
            return messenger.OfType<OrderSelectionChangedMessage>()
                .SelectMany(x => x.LoadedOrderSelection)
                .OfType<LoadedOrderSelection>()
                .Select(x => x.ShipmentAdapters.FirstOrDefault())
                .Where(x => x != null)
                .Select(x => (
                    HashingService: rateHashingServiceLookup[x.ShipmentTypeCode],
                    ShipmentAdapter: x,
                    OnAfterClone: (Action<ICarrierShipmentAdapter>) null
                ));
        }

        /// <summary>
        /// Clone the shipment adapter
        /// </summary>
        private ICarrierShipmentAdapter CloneAdapter(ICarrierShipmentAdapter adapter, Action<ICarrierShipmentAdapter> onAfterClone)
        {
            var clone = adapter.Clone();
            onAfterClone?.Invoke(clone);
            clone.UpdateTotalWeight();
            return clone;
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
