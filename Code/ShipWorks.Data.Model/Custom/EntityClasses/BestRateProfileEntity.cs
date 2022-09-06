using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Partial class extention of the LLBLGen BestRateProfileEntity
    /// </summary>
    public partial class BestRateProfileEntity
    {
        /// <summary>
        /// The set of carrier accounts that can be used for BestRate
        /// </summary>
        public HashSet<long> AllowedCarrierAccounts
        {
            get
            {
                return new HashSet<long>(ArrayUtility.ParseCommaSeparatedList<long>(InternalAllowedCarrierAccounts));
            }
            set
            {
                InternalAllowedCarrierAccounts = ArrayUtility.FormatCommaSeparatedList(value.ToArray());
            }
        }
    }
}
