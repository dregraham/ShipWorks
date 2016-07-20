using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc.Upload
{
    public class OdbcUploader : IOdbcUploader
    {
        private readonly IShippingManager shippingManager;
        private readonly IOdbcFieldMap fieldMap;
        private readonly IOdbcCommandFactory commandFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcUploader"/> class.
        /// </summary>
        public OdbcUploader(IShippingManager shippingManager, IOdbcFieldMap fieldMap, IOdbcCommandFactory commandFactory)
        {
            this.shippingManager = shippingManager;
            this.fieldMap = fieldMap;
            this.commandFactory = commandFactory;
        }

        /// <summary>
        /// Uploads the shipments.
        /// </summary>
        /// <exception cref="ShipWorksOdbcException">Unable to update shipment.</exception>
        public void UploadShipments(OdbcStoreEntity store, IEnumerable<long> shipmentIds)
        {
            fieldMap.Load(store.UploadMap);

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
            fieldMap.Load(store.UploadMap);

            ShipmentEntity shipment = shippingManager.GetLatestActiveShipment(orderid);
            Upload(store, shipment);
        }

        /// <summary>
        /// Uploads the specified store.
        /// </summary>
        /// <exception cref="ShipWorksOdbcException">Unable to update shipment.</exception>
        private void Upload(OdbcStoreEntity store, ShipmentEntity shipment)
        {
            fieldMap.ApplyValues(new IEntity2[] {shipment, shipment.Order});

            IOdbcUploadCommand odbcUploadCommand = commandFactory.CreateUploadCommand(store, fieldMap);
            int rowsAffected = odbcUploadCommand.Execute();

            if (rowsAffected == 0)
            {
                throw new ShipWorksOdbcException("Unable to update shipment.");
            }
        }
    }
}
