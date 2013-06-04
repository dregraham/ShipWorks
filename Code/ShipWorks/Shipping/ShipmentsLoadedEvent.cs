using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Delegate for the ShipmentsLoaded event
    /// </summary>
    public delegate void ShipmentsLoadedEventHandler(object sender, ShipmentsLoadedEventArgs e);

    /// <summary>
    /// EventArgs for the ShipmentsLoaded event handler
    /// </summary>
    public class ShipmentsLoadedEventArgs : AsyncCompletedEventArgs
    {
        List<ShipmentEntity> shipments;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentsLoadedEventArgs(Exception error, bool canceled, object userState, List<ShipmentEntity> shipments) :
            base(error, canceled, userState)
        {
            if (shipments == null)
            {
                throw new ArgumentNullException("shipments");
            }

            this.shipments = shipments;
        }

        /// <summary>
        /// The shipments that were loaded
        /// </summary>
        public List<ShipmentEntity> Shipments
        {
            get { return shipments; }
        }
    }
}
