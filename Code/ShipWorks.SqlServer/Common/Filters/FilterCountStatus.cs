using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Filters
{
    /// <summary>
    /// The possible states a filter count can be in
    /// </summary>
    public enum FilterCountStatus
    {
        NeedsInitialCount = 0,
        Ready = 1,
        RunningInitialCount = 2,
        RunningUpdateCount = 3
    }
}
