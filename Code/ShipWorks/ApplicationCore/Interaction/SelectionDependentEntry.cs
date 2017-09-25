using System;
using System.Collections.Generic;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// Struct for creating a selection entry
    /// </summary>
    public struct SelectionDependentEntry
    {
        public SelectionDependentType SelectionDependentType;
        public Func<IEnumerable<long>, bool> Applies;
    }
}
