using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Stores.Platforms.Newegg.Enums;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping.Response;

namespace ShipWorks.Stores.Platforms.Newegg
{
    /// <summary>
    /// Class that the Newegg store and the ShipWorks application interact with to upload shipping details.
    /// </summary>
    public class NeweggOnlineUpdater
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(NeweggOnlineUpdater));

        // The store instance
        private readonly NeweggStoreEntity store;

        /// <summary>
        /// Initializes a new instance of the <see cref="NeweggOnlineUpdater"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        public NeweggOnlineUpdater(NeweggStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Uploads the shipping details to Newegg.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        public void UploadShippingDetails(ShipmentEntity shipmentEntity)
        {
            if (IsReadyForUpload(shipmentEntity))
            {
                if (shipmentEntity.Order.OrderItems.Count == 0)
                {
                    // Ensure the order items are populated before attempting to upload the shipping details, so
                    // the web client has all the data it may need.
                    List<EntityBase2> orderItems = DataProvider.GetRelatedEntities(shipmentEntity.OrderID, Data.Model.EntityType.NeweggOrderItemEntity);
                    shipmentEntity.Order.OrderItems.AddRange(orderItems.OfType<NeweggOrderItemEntity>());
                }

                log.InfoFormat("Uploading shipping details to Newegg for order number {0}", shipmentEntity.Order.OrderNumber);

                NeweggWebClient webClient = new NeweggWebClient(store);
                ShippingResult result = webClient.UploadShippingDetails(shipmentEntity);

                // An exception was not thrown from the web client, so the upload went through
                // successfully and we can change order status to shipped
                UpdateOrderStatusToShipped(result, shipmentEntity.Order as NeweggOrderEntity);
            }
        }

        /// <summary>
        /// Determines whether [the specified shipment entity] [is ready for shipping].
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <returns>
        ///   <c>true</c> if [the specified shipment entity] [is ready for shipping]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsReadyForUpload(ShipmentEntity shipmentEntity)
        {
            bool isReady = true;

            if (!shipmentEntity.Processed || shipmentEntity.Voided)
            {
                log.InfoFormat("Not uploading tracking number since shipment ID {0} is not processed.", shipmentEntity.ShipmentID);
                isReady = false;
            }
            else if (shipmentEntity.Order.IsManual)
            {
                log.InfoFormat("Not uploading tracking number since order {0} is manual.", shipmentEntity.Order.OrderID);
                isReady = false;
            }

            return isReady;
        }

        /// <summary>
        /// Updates the order status to shipped.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="order">The order.</param>
        private static void UpdateOrderStatusToShipped(ShippingResult result, NeweggOrderEntity order)
        {
            if (order == null)
            {
                throw new ArgumentNullException("Attempted to update the order status of a null Newegg order entity.");
            }

            // The shipping result will always contain a successful result (i.e. shipped)
            // otherwise a NeweggException would have been thrown prior to invoking this method
            order.OnlineStatus = result.Detail.OrderStatus;
            order.OnlineStatusCode = NeweggOrderStatus.Shipped;

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                adapter.SaveAndRefetch(order);
                adapter.Commit();
            }
        }
    }
}
