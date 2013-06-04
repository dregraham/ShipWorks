using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Simple data structure for describing a shipment's number within all the shipments of an order.
    /// </summary>
    public class ShipmentSiblingData
    {
        int position;
        int total;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentSiblingData(int position, int total)
        {
            this.position = position;
            this.total = total;
        }

        /// <summary>
        /// The 1-based position of the shipment
        /// </summary>
        public int ShipmentNumber
        {
            get
            {
                return position;
            }
        }

        /// <summary>
        /// The total number of shipments in the order
        /// </summary>
        public int TotalShipments
        {
            get
            {
                return total;
            }
        }
    }
}
