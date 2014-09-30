using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.Postal.Stamps;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    public class UspsSetupWizard : StampsSetupWizard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsSetupWizard"/> class.
        /// </summary>
        /// <param name="promotion">The promotion.</param>
        /// <param name="allowRegisteringExistingAccount">if set to <c>true</c> [allow registering existing account].</param>
        public UspsSetupWizard(IRegistrationPromotion promotion, bool allowRegisteringExistingAccount)
            : base(promotion, allowRegisteringExistingAccount)
        { }

        /// <summary>
        /// Prepares the stamps account for save. Just sets the reseller type to expedited.
        /// </summary>
        protected override void PrepareStampsAccountForSave()
        {
            base.PrepareStampsAccountForSave();
            StampsAccount.StampsReseller = (int) StampsResellerType.StampsExpedited;
        }
    }
}
