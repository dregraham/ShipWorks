using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Enum that specifies if a computer is allowed to download for a given store
    /// </summary>
    public enum ComputerDownloadAllowed
    {
        No = 0,
        Yes = 1,
        Default = 2
    }
}
