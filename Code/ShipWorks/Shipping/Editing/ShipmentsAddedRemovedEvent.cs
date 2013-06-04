using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Delegate for the ShipmentsRemoved event
    /// </summary>
    public delegate void ShipmentsAddedRemovedEventHandler(object sender, ShipmentsAddedRemovedEventArgs e);

    /// <summary>
    /// EventArgs class for the ShipmentsRemoved event
    /// </summary>
    public class ShipmentsAddedRemovedEventArgs : EventArgs
    {
        List<ShipmentEntity> shipments;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentsAddedRemovedEventArgs(List<ShipmentEntity> shipments)
        {
            this.shipments = shipments;
        }

        /// <summary>
        /// The shipments that were removed
        /// </summary>
        public List<ShipmentEntity> Shipments
        {
            get { return shipments; }
        }
    }
}
