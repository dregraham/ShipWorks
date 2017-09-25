using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.ThreeDCart.OnlineUpdating
{
    /// <summary>
    /// Details about a specific order to upload
    /// </summary>
    public class ThreeDCartOnlineUpdatingOrderDetail
    {
        /// <summary>
        /// Order number
        /// </summary>
        public long OrderNumber { get; set; }

        /// <summary>
        /// Order number complete
        /// </summary>
        public string OrderNumberComplete { get; set; }

        /// <summary>
        /// The 3D cart shipment ID
        /// </summary>
        public long ThreeDCartOrderID { get; set; }

        /// <summary>
        /// Is the order manual
        /// </summary>
        public bool IsManual { get; set; }

        ///// <summary>
        ///// The 3D cart shipment ID
        ///// </summary>
        //public long ThreeDCartShipmentID { get; set; }

        /// <summary>
        /// The original order ID
        /// </summary>
        public long OriginalOrderID { get; set; }
    }
}
