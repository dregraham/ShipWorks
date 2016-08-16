using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using System;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc.Upload
{
    /// <summary>
    /// Uploader for Odbc stores
    /// </summary>
    public class OdbcUploader : IOdbcUploader
    {
        private readonly IShippingManager shippingManager;
        private readonly IOrderManager orderManager;
        private readonly IOdbcUploadCommandFactory uploadCommandFactory;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcUploader"/> class.
        /// </summary>
        public OdbcUploader(IShippingManager shippingManager,
            IOrderManager orderManager,
            IOdbcUploadCommandFactory uploadCommandFactory,
            Func<Type, ILog> logFactory)
        {
            this.shippingManager = shippingManager;
            this.orderManager = orderManager;
            this.uploadCommandFactory = uploadCommandFactory;
            log = logFactory(typeof(OdbcUploader));
        }

        /// <summary>
        /// Uploads the shipments.
        /// </summary>
        /// <exception cref="ShipWorksOdbcException">Unable to update shipment.</exception>
        public void UploadShipments(OdbcStoreEntity store, IEnumerable<long> shipmentIds)
        {
            foreach (long shipmentId in shipmentIds)
            {
                ICarrierShipmentAdapter carrierShipmentAdapter = shippingManager.GetShipment(shipmentId);
                Upload(store, carrierShipmentAdapter.Shipment);
            }
        }

        /// <summary>
        /// Uploads the latest shipment.
        /// </summary>
        /// <exception cref="ShipWorksOdbcException">Unable to update shipment.</exception>
        public void UploadLatestShipment(OdbcStoreEntity store, long orderid)
        {
            ShipmentEntity shipment = orderManager.GetLatestActiveShipment(orderid);

            if (shipment == null || !shipment.Processed)
            {
                log.Error($"Unable to upload {orderid} as it is has not been processed.");
                return;
            }

            Upload(store, shipment);
        }

        /// <summary>
        /// Uploads the shipment details
        /// </summary>
        /// <exception cref="ShipWorksOdbcException">Unable to update shipment.</exception>
        private void Upload(OdbcStoreEntity store, ShipmentEntity shipment)
        {
            IOdbcUploadCommand odbcUploadCommand = uploadCommandFactory.CreateUploadCommand(store, shipment);
            int rowsAffected = odbcUploadCommand.Execute();

            if (rowsAffected == 0)
            {
                throw new ShipWorksOdbcException("Unable to update shipment.");
            }
        }
    }
}
