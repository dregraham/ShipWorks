using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// DTO to hold the data needed for a Magneto Upload Action
    /// </summary>
    public struct MagentoUploadAction
    {
        /// <summary>
        /// Order number
        /// </summary>
        public long OrderNumber { get; set; }

        /// <summary>
        /// Action (complete, cancel, hold)
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Comments to add to the order
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Carrier used for shipping
        /// </summary>
        public string Carrier { get; set; }

        /// <summary>
        /// Tracking number of the shipment
        /// </summary>
        public string TrackingNumber { get; set; }

        /// <summary>
        /// Should we send an email to the user
        /// </summary>
        public bool SendEmail { get; set; }
    }
}
