using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Collections;

namespace ShipWorks.Data.Model.EntityClasses
{
    public partial class ActionFilterTriggerEntity
    {
        /// <summary>
        /// List of StoreID's that the action is limited to acting on
        /// </summary>
        public long[] ComputerLimitedList
        {
            get
            {
                return ArrayUtility.ParseCommaSeparatedList<long>(InternalComputerLimitedList);
            }
            set
            {
                InternalComputerLimitedList = ArrayUtility.FormatCommaSeparatedList(value);
            }
        }
    }
}
