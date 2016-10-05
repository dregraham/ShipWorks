using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// IDownloadedLabelData for shipment types that have no label data.
    /// </summary>
    class NullDownloadedLabelData : IDownloadedLabelData
    {
        /// <summary>
        /// Save label data to the database and/or disk
        /// </summary>
        public void Save()
        {
            // Nothing to do, just return.
        }
    }
}
