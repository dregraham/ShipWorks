using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.UI.ShippingPanel.Loading
{
    /// <summary>
    /// Class representing the loading of an order's shipment(s).
    /// </summary>
    public class OrderSelectionLoaded
    {
        /// <summary>
        /// The shipments
        /// </summary>
        public List<ShipmentEntity> Shipments { get; set; }

        /// <summary>
        /// The result of the load
        /// </summary>
        public ShippingPanelLoadedShipmentResult Result { get; set; }

        /// <summary>
        /// Any exception that may have occured during loading.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Order that has been loaded
        /// </summary>
        public OrderEntity Order { get; set; }
    }
}
