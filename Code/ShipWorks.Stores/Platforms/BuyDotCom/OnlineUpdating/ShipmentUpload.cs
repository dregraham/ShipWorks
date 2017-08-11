using System.Collections.Generic;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.BuyDotCom.OnlineUpdating
{
    /// <summary>
    /// Shipment details for status upload
    /// </summary>
    public class ShipmentUpload
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentUpload(IShipmentEntity shipment, IEnumerable<ShipmentUploadOrder> shipmentOrders)
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
        public IEnumerable<ShipmentUploadOrder> Orders { get; }
    }
}