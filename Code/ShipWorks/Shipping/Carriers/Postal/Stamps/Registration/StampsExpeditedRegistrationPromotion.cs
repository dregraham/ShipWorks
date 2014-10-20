using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.Postal.Endicia;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Registration
{
    /// <summary>
    /// An implementation of the IRegistrationPromotion that should be used for creating new Stamps.com accounts.
    /// </summary>
    public class StampsExpeditedRegistrationPromotion : IRegistrationPromotion
    {
        /// <summary>
        /// This will only return the Expedited type.
        /// </summary>
        public IEnumerable<PostalAccountRegistrationType> AvailableRegistrationTypes
        {
            get { return new List<PostalAccountRegistrationType> { PostalAccountRegistrationType.Expedited }; }
        }

        /// <summary>
        /// Gets a value indicating whether the promotion waives the monthly fee.
        /// </summary>
        public bool IsMonthlyFeeWaived
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the promo code to use when registering an account with Stamps.com based on the
        /// type of account being registered.
        /// </summary>
        /// <param name="registrationType">The type of account being registered.</param>
        /// <returns>The promotion code to be used during registration.</returns>
        public string GetPromoCode(PostalAccountRegistrationType registrationType)
        {
            if (!StampsAccountManager.StampsAccounts.Any() && !StampsAccountManager.Express1Accounts.Any() && !StampsAccountManager.StampsExpeditedAccounts.Any() && !EndiciaAccountManager.Express1Accounts.Any() && !EndiciaAccountManager.EndiciaAccounts.Any())
            {
                // There aren't any postal accounts in ShipWorks; this is considered to be a brand new customer, 
                // so use a promo code to indicate such to allow Stamps.com to track this information
                return "ShipWorks6";
            }
            
            // The regular promotion code for a new expedited account.
            return "shipworks3";
        }
    }
}
