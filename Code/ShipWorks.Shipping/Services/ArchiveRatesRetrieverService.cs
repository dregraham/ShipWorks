using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using log4net;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Stores.Orders.Archive;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Rates retriever service that does nothing
    /// </summary>
    [Component(RegistrationType.Self)]
    public class ArchiveRatesRetrieverService : IRatesRetrieverService
    {
        private readonly IMessenger messenger;
        private readonly ISchedulerProvider schedulerProvider;
        private readonly ILog log;
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public ArchiveRatesRetrieverService(IMessenger messenger,
            ISchedulerProvider schedulerProvider, Func<Type, ILog> logFactory)
        {
            this.messenger = messenger;
            this.schedulerProvider = schedulerProvider;
            log = logFactory(typeof(RatesRetrieverService));
        }

        /// <summary>
        /// Initialize the service for the current session
        /// </summary>
        public void InitializeForCurrentSession()
        {
            EndSession();

            var changedOrders = messenger.OfType<OrderSelectionChangedMessage>().Select(x => Guid.NewGuid());
            var changedShipments = messenger.OfType<ShipmentChangedMessage>().Select(x => Guid.NewGuid());
            var selectedShipments = messenger.OfType<ShipmentSelectionChangedMessage>().Select(x => Guid.NewGuid());

            // Ignore shipment changes from the GridProvider. This means someone changed the carrier from the
            // shipments panel, and if it is for the current shipment, we'll get another request for rates from
            // the shipping panel
            subscription = changedShipments
                .Merge(changedOrders)
                .Merge(selectedShipments)
                .Select(x => x.ToString())
                .Do(x => messenger.Send(new RatesRetrievingMessage(this, x)))
                .CatchAndContinue((Exception ex) => log.Error("Error occurred while getting rates", ex))
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Do(_ => messenger.Send(new RatesNotSupportedMessage(this, ArchiveConstants.RatesNotRetrievedInArchiveMessage)))
                .Subscribe();

            // Clear the Rates UI after login
            messenger.Send(new RatesNotSupportedMessage(this, string.Empty));
        }

        /// <summary>
        /// End the current session
        /// </summary>
        public void EndSession() => subscription?.Dispose();

        /// <summary>
        /// Dispose of any resources
        /// </summary>
        public void Dispose() => EndSession();
    }
}
