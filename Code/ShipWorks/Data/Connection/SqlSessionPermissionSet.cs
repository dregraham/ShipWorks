using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Defines various permission sets that ShipWorks needs to run various things
    /// </summary>
    public enum SqlSessionPermissionSet
    {
        Standard,
        Setup,
        Upgrade
    }
}
