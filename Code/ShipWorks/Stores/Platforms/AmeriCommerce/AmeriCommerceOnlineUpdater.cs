using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Stores.Content;

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
        private readonly Func<AmeriCommerceStoreEntity, AmeriCommerceWebClient> createWebClient;
        
        // status code provider
        private AmeriCommerceStatusCodeProvider statusCodeProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmeriCommerceOnlineUpdater(IOrderManager orderManager, IShippingManager shippingManager,
            ISqlAdapterFactory sqlAdapterFactory, Func<AmeriCommerceStoreEntity, AmeriCommerceWebClient> createWebClient)
        {
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
            OrderEntity order = (OrderEntity)DataProvider.GetEntity(orderID);
            if (order != null)
            {
                if (!order.IsManual)
                {
                    // Create the client for connecting to the module
                    IAmeriCommerceWebClient webClient = createWebClient(store as AmeriCommerceStoreEntity);

                    await webClient.UpdateOrderStatus(order, statusCode).ConfigureAwait(false);

                    // Update the local database with the new status
                    OrderEntity basePrototype = new OrderEntity(orderID)
                    {
                        IsNew = false,
                        OnlineStatusCode = statusCode,
                        OnlineStatus = StatusCodes(store).GetCodeName(statusCode)
                    };

                    unitOfWork.AddForSave(basePrototype);
                }
                else
                {
                    log.InfoFormat("Not uploading order status since order {0} is manual.", order.OrderID);
                }
            }
            else
            {
                log.WarnFormat("Unable to update online status for order {0}: cannot find order", orderID);
            }
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

            OrderEntity order = shipment.Order;
            if (!order.IsManual || order.CombineSplitStatus == CombineSplitStatusType.Combined)
            {
                try
                {
                    // Create the client for connecting to the module
                    IAmeriCommerceWebClient webClient = createWebClient(store as AmeriCommerceStoreEntity);

                    // upload the details
                    await webClient.UploadShipmentDetails(shipment).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                log.InfoFormat("Not uploading tracking number since order {0} is manual.", order.OrderID);
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
