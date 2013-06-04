using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Controls how the range of an initial download is restricted for a store
    /// </summary>
    public enum InitialDownloadRestrictionType
    {
        None,
        DaysBack,
        OrderNumber
    }
}
