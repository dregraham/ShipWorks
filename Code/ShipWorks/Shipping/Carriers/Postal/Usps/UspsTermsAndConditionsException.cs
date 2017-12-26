using System;
using System.Threading;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.UI;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Exception thrown when terms and conditions need to be accepted before processing
    /// </summary>
    public class UspsTermsAndConditionsException : UspsException, ITermsAndConditionsException
    {
        public IUspsTermsAndConditions TermsAndConditions { get; }
        private readonly UspsAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsTermsAndConditionsException(UspsAccountEntity account, string message, IUspsTermsAndConditions termsAndConditions) :
            base(message)
        {
            TermsAndConditions = termsAndConditions;
            this.account = account;
        }
    }
}
