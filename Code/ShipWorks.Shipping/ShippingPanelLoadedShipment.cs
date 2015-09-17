using System;
using ShipWorks.Data.Model.EntityClasses;

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
        /// The result of the load
        /// </summary>
        public ShippingPanelLoadedShipmentResult Result { get; set; }

        /// <summary>
        /// Any exception that may have occured during loading.
        /// </summary>
        public Exception Exception { get; set; }
    }
}
