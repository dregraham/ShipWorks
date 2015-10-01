using System;
using ShipWorks.Core.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Class representing the loading of an order's shipment(s).
    /// </summary>
    public class ShippingPanelLoadedShipment
    {
        /// <summary>
        /// The shipment 
        /// </summary>
        public ShipmentEntity Shipment { get; set; }

        /// <summary>
        /// The shipment adapter
        /// </summary>
        public IShipmentAdapter ShipmentAdapter { get; set; }

        /// <summary>
        /// The result of the load
        /// </summary>
        public ShippingPanelLoadedShipmentResult Result { get; set; }

        /// <summary>
        /// Any exception that may have occured during loading.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Type of shpping requested by the customer
        /// </summary>
        public string RequestedShippingMode { get; internal set; }

        /// <summary>
        /// OrderID of the shipment.  0 if no shipments.
        /// </summary>
        public long OrderID { get; internal set; }
    }
}
