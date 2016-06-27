using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Loading;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Implementation for ShipmentLoaderService
    /// </summary>
    public class ShipmentLoaderService : IInitializeForCurrentUISession
    {
        private readonly IShipmentLoader shipmentLoader;
        private readonly IMessenger messenger;
        private readonly ISchedulerProvider schedulerProvider;
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentLoaderService(IShipmentLoader shipmentLoader, IMessenger messenger, ISchedulerProvider schedulerProvider)
        {
            this.shipmentLoader = shipmentLoader;
            this.messenger = messenger;
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Loads the order and sends a message that it has been loaded.
        /// </summary>
        public async Task<IEnumerable<IOrderSelection>> LoadAndNotify(IEnumerable<long> entityIDs)
        {
            if (entityIDs.CompareCountTo(1) != ComparisonResult.Equal)
            {
                return entityIDs.Select(x => new BasicOrderSelection(x)).Cast<IOrderSelection>().ToArray();
            }

            LoadedOrderSelection orderSelectionLoaded = await shipmentLoader.Load(entityIDs.Single());
            return new IOrderSelection[] { orderSelectionLoaded };
        }

        /// <summary>
        /// Used to let ShipWorks create an instance of this from ShipWorks.Core
        /// </summary>
        public void InitializeForCurrentSession()
        {
            // We should never initialize an already initialized session. We'll re-subscribe in release but when
            // debugging, we should get alerted that this is happening
            Debug.Assert(subscription == null, "Subscription is already initialized");
            EndSession();

            subscription = messenger.OfType<OrderSelectionChangingMessage>()
                .Throttle(TimeSpan.FromMilliseconds(100), schedulerProvider.Default)
                .SelectMany(x => Observable.FromAsync(() => LoadAndNotify(x.OrderIdList)))
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Subscribe(x => messenger.Send(new OrderSelectionChangedMessage(this, x)));
        }

        /// <summary>
        /// Unload the shipment loader when the session ends
        /// </summary>
        public void EndSession()
        {
            subscription?.Dispose();
        }

        /// <summary>
        /// Dispose the object
        /// </summary>
        public void Dispose() => EndSession();
    }
}
