using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging.TrackedObservable;
using Interapptive.Shared.Threading;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Implementation for ShipmentLoaderService
    /// </summary>
    public class OrderLoaderService : IInitializeForCurrentUISession
    {
        private readonly Func<Owned<IOrderLoader>> shipmentLoaderFactory;
        private readonly IMessenger messenger;
        private readonly ISchedulerProvider schedulerProvider;
        private readonly ICarrierShipmentAdapterFactory carrierShipmentAdapterFactory;
        private readonly IStoreTypeManager storeTypeManager;
        private IDisposable subscription;
        private readonly ILog log;
        private readonly IOrderManager orderManager;
        private readonly IShippingSettings shippingSettings;

#pragma warning disable S107 // Methods should not have too many parameters
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLoaderService(Func<Owned<IOrderLoader>> shipmentLoaderFactory, IMessenger messenger,
            ISchedulerProvider schedulerProvider, ICarrierShipmentAdapterFactory carrierShipmentAdapterFactory,
            IStoreTypeManager storeTypeManager, Func<Type, ILog> logFactory, IOrderManager orderManager, IShippingSettings shippingSettings)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            this.shipmentLoaderFactory = shipmentLoaderFactory;
            this.messenger = messenger;
            this.schedulerProvider = schedulerProvider;
            this.carrierShipmentAdapterFactory = carrierShipmentAdapterFactory;
            this.storeTypeManager = storeTypeManager;
            this.orderManager = orderManager;
            this.shippingSettings = shippingSettings;
            this.log = logFactory(this.GetType());
        }

        /// <summary>
        /// Loads the order
        /// </summary>
        public async Task<IEnumerable<IOrderSelection>> Load(IEnumerable<long> orderIDs, bool createIfNoShipments)
        {
            if (orderIDs.CompareCountTo(1) != ComparisonResult.Equal)
            {
                return orderIDs.Select(x => new BasicOrderSelection(x)).Cast<IOrderSelection>().ToArray();
            }

            IOrderLoader shipmentLoader = shipmentLoaderFactory().Value;
            ShipmentsLoadedEventArgs results = await shipmentLoader.LoadAsync(orderIDs, ProgressDisplayOptions.NeverShow, createIfNoShipments, TimeSpan.FromSeconds(10))
                .ConfigureAwait(true);

            // Only 1 order was requested.  If auto creation of shipments isn't enabled, the results.Shipments will be empty
            // and we can't get an order, but we need one for the shipping panel.  So if there are no shipments, load the
            // requested order.
            OrderEntity order = results.Shipments.FirstOrDefault()?.Order;
            if (order == null)
            {
                order = orderManager.FetchOrder(orderIDs.First());
            }

            List<ICarrierShipmentAdapter> adapters = results.Shipments.Select(carrierShipmentAdapterFactory.Get).ToList();

            LoadedOrderSelection orderSelectionLoaded = results.Error == null ?
                new LoadedOrderSelection(order, adapters, GetDestinationAddressEditable(order)) :
                new LoadedOrderSelection(results.Error, order, adapters, GetDestinationAddressEditable(order));

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
                .Gate(messenger.OfType<OrderSelectionChangedMessage>(), schedulerProvider.Default)
                .Select(x => x.Last())
                .SelectMany(x => Load(x.OrderIdList, shippingSettings.FetchReadOnly().AutoCreateShipments)
                    .ToObservable())
                .CatchAndContinue((Exception ex) => log.Error(ex))
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

        /// <summary>
        /// Get whether the destination address is editable
        /// </summary>
        private ShippingAddressEditStateType GetDestinationAddressEditable(OrderEntity order)
        {
            ShipmentEntity firstShipment = order?.Shipments.FirstOrDefault();

            if (firstShipment != null)
            {
                return storeTypeManager.GetType(order.Store)
                    .ShippingAddressEditableState(order, firstShipment);
            }

            return ShippingAddressEditStateType.Editable;
        }
    }
}
