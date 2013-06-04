using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.Postal.Endicia.WebServices.LabelService;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Data structure to capture what is returned from the GetAccountStatus endicia call
    /// </summary>
    public class EndiciaAccountStatus
    {
        decimal postageBalance;
        decimal runningBalance;

        /// <summary>
        /// Create a new instance based on the response from an API call
        /// </summary>
        public EndiciaAccountStatus(AccountStatusResponse response)
        {
            postageBalance = (decimal) response.CertifiedIntermediary.PostageBalance;
            runningBalance = (decimal) response.CertifiedIntermediary.AscendingBalance;
        }

        /// <summary>
        /// The remaining postage balance
        /// </summary>
        public decimal PostageBalance
        {
            get { return postageBalance; }
        }

        /// <summary>
        /// The total amount of postage printed so far.
        /// </summary>
        public decimal RunningBalance
        {
            get { return runningBalance; }
        }
    }
}
