using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using System;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc.Upload
{
    public class OdbcUploader : IOdbcUploader
    {
        private readonly IShippingManager shippingManager;

        public OdbcUploader(IShippingManager shippingManager)
        {
            this.shippingManager = shippingManager;
        }

        public void UploadShipments(OdbcStoreEntity store, IEnumerable<long> shipmentIds)
        {
            foreach (long shipmentId in shipmentIds)
            {
                ICarrierShipmentAdapter carrierShipmentAdapter = shippingManager.GetShipment(shipmentId);
                Upload(carrierShipmentAdapter.Shipment);
            }
        }

        public void UploadLatestShipment(OdbcStoreEntity store, long orderid)
        {
            ShipmentEntity shipment = shippingManager.GetLatestActiveShipment(orderid);
            Upload(shipment);
        }

        private void Upload(ShipmentEntity shipment)
        {
            throw new NotImplementedException();
        }
    }
}
