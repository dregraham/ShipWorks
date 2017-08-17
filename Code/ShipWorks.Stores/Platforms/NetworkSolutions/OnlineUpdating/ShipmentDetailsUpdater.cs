using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;

namespace ShipWorks.Stores.Platforms.NetworkSolutions.OnlineUpdating
{
    /// <summary>
    /// Update NetworkSolutions shipment details online
    /// </summary>
    [Component]
    public class ShipmentDetailsUpdater : IShipmentDetailsUpdater
    {
        private readonly INetworkSolutionsCombineOrderSearchProvider orderSearchProvider;
        private readonly IShippingManager shippingManager;
        private readonly ILog log;
        private readonly INetworkSolutionsWebClient webClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentDetailsUpdater(
            INetworkSolutionsCombineOrderSearchProvider orderSearchProvider,
            INetworkSolutionsWebClient webClient,
            IShippingManager shippingManager,
            Func<Type, ILog> createLogger)
        {
            this.webClient = webClient;
            this.orderSearchProvider = orderSearchProvider;
            this.shippingManager = shippingManager;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Uploads shipment details for the given shipment Id
        /// </summary>
        public async Task UploadShipmentDetails(INetworkSolutionsStoreEntity store, long shipmentID)
        {
            var shipmentAdapter = await shippingManager.GetShipmentAsync(shipmentID).ConfigureAwait(false);
            if (shipmentAdapter?.Shipment == null)
            {
                log.InfoFormat("Not uploading tracking number for shipment {0}, shipment was deleted.", shipmentID);
                return;
            }

            await UploadShipmentDetails(store, shipmentAdapter.Shipment).ConfigureAwait(false);
        }

        /// <summary>
        /// Uploads shipment details for the given shipment entity
        /// </summary>
        public async Task UploadShipmentDetails(INetworkSolutionsStoreEntity store, ShipmentEntity shipment)
        {
            if (!shipment.Processed || shipment.Voided)
            {
                log.InfoFormat("Not uploading shipment details for shipment {0}, either not processed or has been voided.", shipment.ShipmentID);
                return;
            }

            OrderEntity order = shipment.Order;
            if (order.IsManual && order.CombineSplitStatus != CombineSplitStatusType.Combined)
            {
                log.InfoFormat("Not uploading tracking number since order {0} is manual.", order.OrderID);
                return;
            }

            var exceptions = new List<Exception>();
            var identifiers = await orderSearchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

            foreach (var identifier in identifiers)
            {
                try
                {
                    await Task.Run(() => webClient.UploadShipmentDetails(store, identifier, shipment));
                }
                catch (NetworkSolutionsException ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw exceptions.First();
            }
        }
    }
}
