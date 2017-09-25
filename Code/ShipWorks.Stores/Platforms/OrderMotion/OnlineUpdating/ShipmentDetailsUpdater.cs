using System;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using Interapptive.Shared.Utility;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.OrderMotion.OnlineUpdating
{
    /// <summary>
    /// Handles uploading data to OrderMotion
    /// </summary>
    [Component]
    public class ShipmentDetailsUpdater : IShipmentDetailsUpdater
    {
        private readonly ILog log;
        private readonly IOrderMotionWebClient webClient;
        private readonly IShippingManager shippingManager;
        private readonly IStoreManager storeManager;
        private readonly IDataAccess dataAccess;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentDetailsUpdater(IOrderMotionWebClient webClient,
            IDataAccess dataAccess,
            IShippingManager shippingManager,
            IStoreManager storeManager,
            Func<Type, ILog> createLogger)
        {
            this.dataAccess = dataAccess;
            this.storeManager = storeManager;
            this.shippingManager = shippingManager;
            this.webClient = webClient;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Uploads shipment details for the given shipment Id
        /// </summary>
        public async Task UploadShipmentDetails(long shipmentID)
        {
            var shipmentAdapter = await shippingManager.GetShipmentAsync(shipmentID).ConfigureAwait(false);
            if (shipmentAdapter?.Shipment == null)
            {
                log.InfoFormat("Not uploading tracking number for shipment {0}, shipment was deleted.", shipmentID);
                return;
            }

            await UploadShipmentDetails(shipmentAdapter.Shipment).ConfigureAwait(false);
        }

        /// <summary>
        /// Uploads shipment details for the given shipment entity
        /// </summary>
        public async Task UploadShipmentDetails(ShipmentEntity shipment)
        {
            if (!shipment.Processed || shipment.Voided)
            {
                log.InfoFormat("Not uploading shipment details for shipment {0}, either not processed or has been voided.", shipment.ShipmentID);
                return;
            }

            var store = storeManager.GetRelatedStore(shipment) as IOrderMotionStoreEntity;
            if (store == null)
            {
                log.WarnFormat("Not uploading tracking number since the store for order {0} has gone away.", shipment.OrderID);
                return;
            }

            await shippingManager.EnsureShipmentLoadedAsync(shipment).ConfigureAwait(false);

            var orderDetails = await dataAccess.GetOrderDetails(shipment.OrderID).ConfigureAwait(false);
            if (orderDetails.All(x => x.IsManual))
            {
                log.InfoFormat("Not uploading tracking number since order {0} is manual.", shipment.OrderID);
                return;
            }

            List<IResult> results = new List<IResult>();
            foreach (var order in orderDetails)
            {
                try
                {
                    await webClient.UploadShipmentDetails(store, shipment, order).ConfigureAwait(false);
                }
                catch (OrderMotionException ex)
                {
                    results.Add(Result.FromError(ex));
                }
            }

            if (results.Any())
            {
                throw results.First().Exception;
            }
        }
    }
}
