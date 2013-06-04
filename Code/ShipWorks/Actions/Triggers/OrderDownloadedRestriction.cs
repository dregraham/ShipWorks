using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Actions.Triggers
{
    /// <summary>
    /// Restrictions on the OrderDownloadedTrigger
    /// </summary>
    public enum OrderDownloadedRestriction
    {
        /// <summary>
        /// Anytime an order is downloaded.
        /// </summary>
        None = 0,

        /// <summary>
        /// Only trigger when an order is downloaded for the first time.
        /// </summary>
        OnlyInitial = 1,

        /// <summary>
        /// Only trigger if the order has already been downloaded
        /// </summary>
        NotInitial = 2
    }
}
