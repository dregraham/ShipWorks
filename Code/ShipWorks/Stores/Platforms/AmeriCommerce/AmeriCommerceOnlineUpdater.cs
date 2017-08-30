using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

namespace ShipWorks.Stores.Platforms.AmeriCommerce
{
    /// <summary>
    /// Handles uploading data to AmeriCommerce
    /// </summary>
    [Component]
    public class AmeriCommerceOnlineUpdater : IAmeriCommerceOnlineUpdater
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(AmeriCommerceOnlineUpdater));
        private readonly IOrderManager orderManager;
        private readonly IShippingManager shippingManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly ICombineOrderNumberSearchProvider cominedOrderSearchProvider;
        private readonly Func<AmeriCommerceStoreEntity, IAmeriCommerceWebClient> createWebClient;

        // status code provider
        private AmeriCommerceStatusCodeProvider statusCodeProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmeriCommerceOnlineUpdater(IOrderManager orderManager,
            IShippingManager shippingManager,
            ISqlAdapterFactory sqlAdapterFactory,
            ICombineOrderNumberSearchProvider cominedOrderSearchProvider,
            Func<AmeriCommerceStoreEntity, IAmeriCommerceWebClient> createWebClient)
        {
            this.cominedOrderSearchProvider = cominedOrderSearchProvider;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.shippingManager = shippingManager;
            this.orderManager = orderManager;
            this.createWebClient = createWebClient;
        }

        /// <summary>
        /// Gets the status code provider
        /// </summary>
        private AmeriCommerceStatusCodeProvider StatusCodes(IAmeriCommerceStoreEntity store)
        {
            return statusCodeProvider ?? (statusCodeProvider = new AmeriCommerceStatusCodeProvider(store as AmeriCommerceStoreEntity));
        }

        /// <summary>
        /// Changes the status of an AmeriCommerce order to that specified
        /// </summary>
        public async Task UpdateOrderStatus(IAmeriCommerceStoreEntity store, long orderID, int statusCode)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();
            await UpdateOrderStatus(store, orderID, statusCode, unitOfWork).ConfigureAwait(false);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Changes the status of an AmeriCommerce order to that specified
        /// </summary>
        public async Task UpdateOrderStatus(IAmeriCommerceStoreEntity store, long orderID, int statusCode, UnitOfWork2 unitOfWork)
        {
            OrderEntity order = (OrderEntity) DataProvider.GetEntity(orderID);
            if (order == null)
            {
                log.WarnFormat("Unable to update online status for order {0}: cannot find order", orderID);
                return;
            }

            IEnumerable<long> identifiers = await cominedOrderSearchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

            if (identifiers.Count() == 1 && order.IsManual)
            {
                log.InfoFormat("Not uploading order status since order {0} is manual.", order.OrderID);
                return;
            }

            // Create the client for connecting to the module
            IAmeriCommerceWebClient webClient = createWebClient(store as AmeriCommerceStoreEntity);

            foreach (var orderIdentifier in identifiers)
            {
                webClient.UpdateOrderStatus(orderIdentifier, statusCode);
            }

            // Update the local database with the new status
            OrderEntity basePrototype = new OrderEntity(orderID)
            {
                IsNew = false,
                OnlineStatusCode = statusCode,
                OnlineStatus = StatusCodes(store).GetCodeName(statusCode)
            };

            unitOfWork.AddForSave(basePrototype);
        }

        /// <summary>
        /// Upload shipment details for a list of orders
        /// </summary>
        public async Task UploadOrderShipmentDetails(IAmeriCommerceStoreEntity store, IEnumerable<long> orderIDs)
        {
            var orders = await LoadOrders(orderIDs).ConfigureAwait(false);

            foreach (var order in orders)
            {
                // upload tracking number for the most recent processed, not voided shipment
                ShipmentEntity shipment = await orderManager.GetLatestActiveShipmentAsync(order.OrderID).ConfigureAwait(false);
                if (shipment == null)
                {
                    log.InfoFormat("There were no Processed and not Voided shipments to upload for OrderID {0}.", order.OrderID);
                }
                else
                {
                    shipment.Order = order;
                    await UploadShipmentDetails(store, shipment).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Upload shipment details for a single shipment
        /// </summary>
        public async Task UploadOrderShipmentDetails(IAmeriCommerceStoreEntity store, long shipmentID)
        {
            var shipmentAdapter = await shippingManager.GetShipmentAsync(shipmentID).ConfigureAwait(false);
            ShipmentEntity shipment = shipmentAdapter?.Shipment;

            if (shipment == null)
            {
                log.InfoFormat("Not uploading shipment details, since the shipment {0} was deleted.", shipmentID);
            }
            else
            {
                await UploadShipmentDetails(store, shipment).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Uploads shipment details for the given shipment entity
        /// </summary>
        private async Task UploadShipmentDetails(IAmeriCommerceStoreEntity store, ShipmentEntity shipment)
        {
            if (!shipment.Processed || shipment.Voided)
            {
                log.InfoFormat("Not uploading shipment details for shipment {0}, either not processed or has been voided.", shipment.ShipmentID);
                return;
            }

            // Create the client for connecting to the module
            IAmeriCommerceWebClient webClient = createWebClient(store as AmeriCommerceStoreEntity);

            IEnumerable<long> identifiers = await cominedOrderSearchProvider.GetOrderIdentifiers(shipment.Order).ConfigureAwait(false);

            if (identifiers.Count() == 1 && shipment.Order.IsManual)
            {
                log.WarnFormat("Not uploading shipment details since order {0} is manual.", shipment.Order.OrderID);
                return;
            }

            foreach (var orderIdentifier in identifiers)
            {
                try
                {
                    // upload the details
                    webClient.UploadShipmentDetails(orderIdentifier, shipment);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        /// <summary>
        /// Load the orders for the given keys
        /// </summary>
        private async Task<IEnumerable<OrderEntity>> LoadOrders(IEnumerable<long> orderKeys)
        {
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                return await orderManager.LoadOrdersAsync(orderKeys, sqlAdapter).ConfigureAwait(false);
            }
        }
    }
}
