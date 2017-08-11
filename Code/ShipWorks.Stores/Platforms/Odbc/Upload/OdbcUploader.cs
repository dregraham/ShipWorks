using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Enums;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

namespace ShipWorks.Stores.Platforms.Odbc.Upload
{
    /// <summary>
    /// Uploader for ODBC stores
    /// </summary>
    public class OdbcUploader : IOdbcUploader
    {
        private readonly IShippingManager shippingManager;
        private readonly IOrderManager orderManager;
        private readonly IOdbcUploadCommandFactory uploadCommandFactory;
        private readonly ILog log;
        private readonly OdbcStoreType storeType;
        private readonly ICombineOrderSearchProvider<string> combineOrderSearchProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcUploader"/> class.
        /// </summary>
        public OdbcUploader(IShippingManager shippingManager,
            IOrderManager orderManager, 
            IOdbcUploadCommandFactory uploadCommandFactory,
            ICombineOrderSearchProvider<string> combineOrderSearchProvider,
            Func<Type, ILog> logFactory)
        {
            this.shippingManager = shippingManager;
            this.orderManager = orderManager;
            this.uploadCommandFactory = uploadCommandFactory;
            this.combineOrderSearchProvider = combineOrderSearchProvider;
            log = logFactory(typeof(OdbcUploader));
        }

        /// <summary>
        /// Uploads the shipments.
        /// </summary>
        /// <exception cref="ShipWorksOdbcException">Unable to update shipment.</exception>
        public async Task UploadShipments(OdbcStoreEntity store, IEnumerable<long> shipmentIds)
        {
            foreach (long shipmentId in shipmentIds)
            {
                ICarrierShipmentAdapter carrierShipmentAdapter = shippingManager.GetShipment(shipmentId);
                await Upload(store, carrierShipmentAdapter.Shipment).ConfigureAwait(false); ;
            }
        }

        /// <summary>
        /// Uploads the latest shipment.
        /// </summary>
        /// <exception cref="ShipWorksOdbcException">Unable to update shipment.</exception>
        public async Task UploadLatestShipment(OdbcStoreEntity store, long orderid)
        {
            ShipmentEntity shipment = orderManager.GetLatestActiveShipment(orderid);

            if (shipment == null || !shipment.Processed)
            {
                log.Error($"Unable to upload {orderid} as it is has not been processed.");
                return;
            }

            await Upload(store, shipment).ConfigureAwait(false);
        }


        /// <summary>
        /// Uploads the shipment details
        /// </summary>
        /// <exception cref="ShipWorksOdbcException">Unable to update shipment.</exception>
        private async Task Upload(OdbcStoreEntity store, ShipmentEntity shipment)
        {
            ShipmentEntity clonedShipment = EntityUtility.CloneEntity(shipment);
            IEnumerable<string> combinedOrderNumbers = await combineOrderSearchProvider.GetOrderIdentifiers(clonedShipment.Order).ConfigureAwait(false);
            int errorCount = 0;

            foreach (string combinedOrderNumber in combinedOrderNumbers)
            {
                clonedShipment.Order.ChangeOrderNumber(combinedOrderNumber);

                IOdbcUploadCommand odbcUploadCommand = uploadCommandFactory.CreateUploadCommand(store, clonedShipment);
                int rowsAffected = odbcUploadCommand.Execute();

                if (rowsAffected == 0)
                {
                    errorCount++;
                }
            }

            if (errorCount > 0)
            {
                throw new ShipWorksOdbcException("Unable to update shipment.");
            }
        }
    }
}
