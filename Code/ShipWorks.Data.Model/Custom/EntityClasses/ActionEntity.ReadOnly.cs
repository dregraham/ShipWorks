using System.Collections.Generic;
using Interapptive.Shared.Business;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Custom action code
    /// </summary>
    public partial class ReadOnlyActionEntity
    {
        /// <summary>
        /// List of StoreID's that the action is limited to acting on
        /// </summary>
        public IEnumerable<long> StoreLimitedList { get; private set; }

        /// <summary>
        /// List of ComputerID's that the action is limited to acting on.
        /// </summary>
        public IEnumerable<long> ComputerLimitedList { get; private set; }

        /// <summary>
        /// If the action is store limited - and limited only to a single store - that single ID is returned. If it is not limited,
        /// or its limited to more than one store, this returns null.
        /// </summary>
        public long? StoreLimitedSingleStoreID { get; private set; }

        /// <summary>
        /// Copy extra data defined in the custom Action entity
        /// </summary>
        partial void CopyCustomActionData(IActionEntity source)
        {
            StoreLimitedList = source.StoreLimitedList.ToReadOnly();
            ComputerLimitedList = source.ComputerLimitedList.ToReadOnly();
            StoreLimitedSingleStoreID = source.StoreLimitedSingleStoreID;
        }
    }
}
