using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// EventArgs class for adding and deleting shipments from the ShipmentGridControl
    /// </summary>
    public class ShipmentGridShipmentsChangedEventArgs : EventArgs
    {
        public List<ShipmentEntity> ShipmentsAdded { get; private set; }
        public List<ShipmentEntity> ShipmentsRemoved { get; private set; }

        /// <summary>
        /// Constructor for creating this ShipmentGridShipmentsChangedEventArgs instance
        /// </summary>
        /// <param name="shipmentsAdded">List of shipments that were added to the grid</param>
        /// <param name="shipmentsRemoved">List of shipments that were removed from the grid</param>
        public ShipmentGridShipmentsChangedEventArgs(List<ShipmentEntity> shipmentsAdded, List<ShipmentEntity> shipmentsRemoved)
        {
            ShipmentsAdded = shipmentsAdded;
            ShipmentsRemoved = shipmentsRemoved;
        }
    }
}
