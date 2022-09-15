using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Partial class extention of the LLBLGen BestRateShipmentEntity
    /// </summary>
    public partial class BestRateShipmentEntity
    {
        /// <summary>
        /// The set of carrier accounts that can be used for BestRate
        /// </summary>
        public List<long> AllowedCarrierAccounts
        {
            get
            {
                return ArrayUtility.ParseCommaSeparatedList<long>(InternalAllowedCarrierAccounts).ToList();
            }
            set
            {
                InternalAllowedCarrierAccounts = ArrayUtility.FormatCommaSeparatedList(value.ToArray());
            }
        }
    }
}
