using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Newegg.Enums;
using ShipWorks.Stores.Platforms.Newegg.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Newegg
{
    /// <summary>
    /// Class that the Newegg store and the ShipWorks application interact with to upload shipping details.
    /// </summary>
    [Component]
    public class NeweggOnlineUpdater : IShipmentDetailsUpdater
    {
        private readonly ILog log;
        private readonly INeweggWebClient webClient;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IDataAccess dataAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="NeweggOnlineUpdater"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        public NeweggOnlineUpdater(INeweggWebClient webClient, IDataAccess dataAccess, ISqlAdapterFactory sqlAdapterFactory, Func<Type, ILog> createLogger)
        {
            this.dataAccess = dataAccess;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.webClient = webClient;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Uploads the shipping details to Newegg.
        /// </summary>
        public async Task UploadShippingDetails(INeweggStoreEntity store, ShipmentEntity shipment)
        {
            ShipmentUploadDetails shipmentDetails = await dataAccess.LoadShipmentDetailsAsync(shipment).ConfigureAwait(false);

            if (!IsReadyForUpload(shipment, shipmentDetails))
            {
                return;
            }

            log.InfoFormat("Uploading shipping details to Newegg for order number {0}", shipmentDetails.Order.OrderNumber);

            var results = await webClient.UploadShippingDetails(store, shipment, shipmentDetails).ConfigureAwait(false);

            // An exception was not thrown from the web client, so the upload went through
            // successfully and we can change order status to shipped
            await UpdateOrderStatusToShipped(results, shipment.OrderID).ConfigureAwait(false);
        }

        /// <summary>
        /// Determines whether [the specified shipment entity] [is ready for shipping].
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <returns>
        ///   <c>true</c> if [the specified shipment entity] [is ready for shipping]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsReadyForUpload(IShipmentEntity shipment, ShipmentUploadDetails shipmentDetails)
        {
            if (!shipment.Processed || shipment.Voided)
            {
                log.InfoFormat("Not uploading tracking number since shipment ID {0} is not processed.", shipment.ShipmentID);
                return false;
            }

            if (shipmentDetails.Identifiers.None())
            {
                log.WarnFormat("Not uploading tracking number for shipment {0} since no orders were found.", shipment.OrderID);
                return false;
            }

            if (shipmentDetails.AllOrdersManual)
            {
                log.InfoFormat("Not uploading tracking number since order {0} is manual.", shipment.OrderID);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Updates the order status to shipped.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="order">The order.</param>
        private async Task UpdateOrderStatusToShipped(IEnumerable<string> results, long orderID)
        {
            var status = results.FirstOrDefault();
            if (results.IsCountGreaterThan(1))
            {
                log.InfoFormat("Multiple statuses returned from Newegg for combined order.  Using first: {0}", results.FirstOrDefault());
            }

            // The shipping result will always contain a successful result (i.e. shipped)
            // otherwise a NeweggException would have been thrown prior to invoking this method
            var order = new OrderEntity(orderID)
            {
                IsNew = false,
                OnlineStatus = status,
                OnlineStatusCode = NeweggOrderStatus.Shipped
            };

            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                await adapter.SaveEntityAsync(order).ConfigureAwait(false);
            }
        }
    }
}
