using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Ebay.Requests.System
{
    /// <summary>
    /// An interface for sending requests to obtain the server time.
    /// </summary>
    public interface ITimeRequest
    {
        /// <summary>
        /// Gets the server time in UTC.
        /// </summary>
        /// <returns>A DateTime object in UTC.</returns>
        DateTime GetServerTimeInUtc();
    }
}
