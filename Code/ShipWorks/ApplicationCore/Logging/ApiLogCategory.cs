using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore.Logging
{
    /// <summary>
    /// Possible categories for log files written from an ApiLogEntry
    /// </summary>
    public enum ApiLogCategory
    {
        Request,
        RequestSupplement,
        Response,
        ResponseSupplement
    }
}
