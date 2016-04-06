using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
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
    public class ShipmentLoaderService : IInitializeForCurrentSession
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
                .SubscribeOn(TaskPoolScheduler.Default)
                .Throttle(TimeSpan.FromMilliseconds(100))
                .Subscribe(async x => await LoadAndNotify(x.OrderIdList).ConfigureAwait(false));
        }

        /// <summary>
        /// Loads the order and sends a message that it has been loaded.
        /// </summary>
        public async Task LoadAndNotify(IEnumerable<long> entityIDs)
        {
            IEnumerable<IOrderSelection> orderSelection;

            if (entityIDs.CompareCountTo(1) != ComparisonResult.Equal)
            {
                orderSelection = entityIDs.Select(x => new BasicOrderSelection(x)).Cast<IOrderSelection>().ToArray();
            }
            else
            {
                LoadedOrderSelection orderSelectionLoaded = await shipmentLoader.Load(entityIDs.Single());
                orderSelection = new IOrderSelection[] { orderSelectionLoaded };
            }

            OrderSelectionChangedMessage orderSelectionChangedMessage = new OrderSelectionChangedMessage(this, orderSelection);

            messenger.Send(orderSelectionChangedMessage);
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
