using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.GenericModule
{
    /// <summary>
    /// Configuration option for how orders are to be retrieved from a ShipWorks module.
    /// </summary>
    public enum GenericStoreDownloadStrategy
    {
        ByModifiedTime = 0,
        ByOrderNumber = 1,
    }
}
