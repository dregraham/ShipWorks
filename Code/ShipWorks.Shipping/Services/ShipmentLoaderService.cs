using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
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
    public class ShipmentLoaderService : IShipmentLoaderService<OrderSelectionLoaded>, IInitializeForCurrentSession
    {
        private readonly IShipmentLoader shipmentLoader;
        private readonly IMessenger messenger;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentLoaderService(IShipmentLoader shipmentLoader, IMessenger messenger)
        {
            this.shipmentLoader = shipmentLoader;
            this.messenger = messenger;
            
            messenger.OfType<OrderSelectionChangingMessage>()
                //.ObserveOn(DispatcherScheduler.Current)
                .SubscribeOn(TaskPoolScheduler.Default)
                .Throttle(TimeSpan.FromMilliseconds(100))
                .Subscribe(x => LoadAndNotify(x.OrderIdList));
        }

        /// <summary>
        /// Loads the order and sends a message that it has been loaded.
        /// </summary>
        public Task LoadAndNotify(IEnumerable<long> entityIDs)
        {
            return TaskEx.Run(() =>
            {
                long entityID = entityIDs?.FirstOrDefault() ?? 0;

                OrderSelectionLoaded orderSelectionLoaded = shipmentLoader.Load(entityID);

                OrderSelectionChangedMessage orderSelectionChangedMessage = new OrderSelectionChangedMessage(null, new List<OrderSelectionLoaded> {orderSelectionLoaded});

                messenger.Send(orderSelectionChangedMessage);
            });
        }

        /// <summary>
        /// Used to let ShipWorks create an instance of this from ShipWorks.Core
        /// </summary>
        public void InitializeForCurrentSession()
        {
            // Do nothing, just needed this to get an instance in memory on ShipWorks load.
        }
    }
}
