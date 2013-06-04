using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ShipWorks.SqlServer.Filters
{
    /// <summary>
    /// State of a checkpoint
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    enum FilterCountCheckpointState
    {
        Prepare = 1,

        UpdateCustomers = 20,
        UpdateOrders = 21,
        UpdateItems = 22,
        UpdateShipments = 23,

        Cleanup = 30
    }
}
