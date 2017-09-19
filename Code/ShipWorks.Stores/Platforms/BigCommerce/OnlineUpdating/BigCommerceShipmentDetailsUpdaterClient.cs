using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating
{
    /// <summary>
    /// Web client for updating shipment details online
    /// </summary>
    [Component]
    public class BigCommerceShipmentDetailsUpdaterClient : IBigCommerceShipmentDetailsUpdaterClient
    {
        private readonly IBigCommerceWebClientFactory webClientFactory;
        private readonly IBigCommerceItemLoader productLoader;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceShipmentDetailsUpdaterClient(IBigCommerceWebClientFactory webClientFactory, IBigCommerceItemLoader productLoader,
            Func<Type, ILog> createLogger)
        {
            this.productLoader = productLoader;
            this.webClientFactory = webClientFactory;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Update shipment details for a single order
        /// </summary>
        public async Task UpdateOnline(IBigCommerceStoreEntity store,
            BigCommerceOnlineOrderDetails orderDetail,
            string orderNumberComplete,
            ShipmentEntity shipment,
            IDictionary<long, IEnumerable<IBigCommerceOrderItemEntity>> allItems)
        {
            var webClient = webClientFactory.Create(store);

            IEnumerable<IBigCommerceOrderItemEntity> orderItems = allItems.ContainsKey(orderDetail.OrderID) ?
                    allItems[orderDetail.OrderID] :
                    Enumerable.Empty<IBigCommerceOrderItemEntity>();

            if (orderItems.None())
            {
                log.InfoFormat("There are no items for order {0}, not uploading", orderDetail.OrderNumberComplete);
                return;
            }

            // If the order is only digital, update the order status to completed, then return
            // We don't want to actually "ship" the order because there is no order address id, and BigCommerce
            // will return an error.
            if (IsOrderAllDigital(orderItems))
            {
                await webClient.UpdateOrderStatus(Convert.ToInt32(orderDetail.OrderNumber), BigCommerceConstants.OrderStatusCompleted).ConfigureAwait(false);
                return;
            }

            GenericResult<BigCommerceOnlineItems> updateDetailsResult = await productLoader
                .LoadItems(orderItems, orderNumberComplete, orderDetail.OrderNumber, webClient)
                .ConfigureAwait(false);
            if (updateDetailsResult.Failure)
            {
                log.Warn(updateDetailsResult.Message);
                return;
            }

            var updateDetails = updateDetailsResult.Value;
            Tuple<string, string> shippingMethod = productLoader.GetShippingMethod(shipment);

            await webClient.UploadOrderShipmentDetails(
                    orderDetail.OrderNumber,
                    updateDetails.OrderAddressID,
                    shipment.TrackingNumber,
                    shippingMethod,
                    updateDetails.Items.ToList()).ConfigureAwait(false);
        }

        /// <summary>
        /// Determines if an orders' items are all digital
        /// </summary>
        /// <param name="order">The order to check</param>
        /// <returns>True if all items are digital, otherwise false</returns>
        [SuppressMessage("SonarLint", "S1944",
            Justification = "This seems to be a false positive. IBigCommerceOrderItemEntity implements IOrderItemEntity so the cast is valid")]
        public bool IsOrderAllDigital(IEnumerable<IOrderItemEntity> orderItems) =>
            orderItems.All(oi => (oi as IBigCommerceOrderItemEntity)?.IsDigitalItem == true);
    }
}
