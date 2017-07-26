using System.Collections.Generic;
using Interapptive.Shared.Business;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Custom action code
    /// </summary>
    public partial interface IActionEntity
    {
        /// <summary>
        /// List of StoreID's that the action is limited to acting on
        /// </summary>
        IEnumerable<long> StoreLimitedList { get; }

        /// <summary>
        /// List of ComputerID's that the action is limited to acting on.
        /// </summary>
        IEnumerable<long> ComputerLimitedList { get; }

        /// <summary>
        /// If the action is store limited - and limited only to a single store - that single ID is returned. If it is not limited,
        /// or its limited to more than one store, this returns null.
        /// </summary>
        long? StoreLimitedSingleStoreID { get; }
    }
}
