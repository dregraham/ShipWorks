using System.Collections.Generic;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.BuyDotCom.OnlineUpdating
{
    /// <summary>
    /// Shipment details for status upload
    /// </summary>
    public class BuyDotComShipmentUpload
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BuyDotComShipmentUpload(IShipmentEntity shipment, IEnumerable<BuyDotComShipmentUploadOrder> shipmentOrders)
        {
            Shipment = shipment;
            Orders = shipmentOrders.ToReadOnly();
        }

        /// <summary>
        /// Shipment
        /// </summary>
        public IShipmentEntity Shipment { get; }

        /// <summary>
        /// Orders associated with the shipment
        /// </summary>
        public IEnumerable<BuyDotComShipmentUploadOrder> Orders { get; }
    }
}