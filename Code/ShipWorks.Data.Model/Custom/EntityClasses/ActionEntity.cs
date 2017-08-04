using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Partial class extension of the LLBLGen ActionEntity
    /// </summary>
    public partial class ActionEntity
    {
        /// <summary>
        /// List of StoreID's that the action is limited to acting on
        /// </summary>
        public IEnumerable<long> StoreLimitedList
        {
            get
            {
                return ArrayUtility.ParseCommaSeparatedList<long>(InternalStoreLimitedList);
            }
            set
            {
                InternalStoreLimitedList = ArrayUtility.FormatCommaSeparatedList(value.ToArray());
            }
        }

        /// <summary>
        /// List of ComputerID's that the action is limited to acting on.
        /// </summary>
        public IEnumerable<long> ComputerLimitedList
        {
            get
            {
                return ArrayUtility.ParseCommaSeparatedList<long>(InternalComputerLimitedList);
            }
            set
            {
                InternalComputerLimitedList = "";
                if (value != null)
                {
                    InternalComputerLimitedList = ArrayUtility.FormatCommaSeparatedList(value.ToArray());
                }
            }
        }

        /// <summary>
        /// If the action is store limited - and limited only to a single store - that single ID is returned. If it is not limited,
        /// or its limited to more than one store, this returns null.
        /// </summary>
        public long? StoreLimitedSingleStoreID
        {
            get
            {
                if (StoreLimited && StoreLimitedList.Count() == 1)
                {
                    return StoreLimitedList.First();
                }

                return null;
            }
        }
    }
}
