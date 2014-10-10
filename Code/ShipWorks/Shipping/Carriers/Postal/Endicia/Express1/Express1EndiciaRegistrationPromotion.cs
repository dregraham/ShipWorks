using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.Postal.Stamps;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// An IRegistrationPromotion to be used when signing up for a new USPS (Stamps.com Expedited)
    /// account from an Express1 for Endicia shipment (and there are not any existing Endicia accounts).
    /// </summary>
    public class Express1EndiciaRegistrationPromotion : IRegistrationPromotion
    {
        /// <summary>
        /// Gets the available account registration types. These values could differ across implementations
        /// based on factors such as whether the account is a brand new account, being migrated from another
        /// postal carrier, etc.
        /// </summary>
        public IEnumerable<PostalAccountRegistrationType> AvailableRegistrationTypes
        {
            get { return new List<PostalAccountRegistrationType> { PostalAccountRegistrationType.Expedited }; }
        }

        /// <summary>
        /// Gets the promo code to use when registering an account with Stamps.com based on the
        /// type of account being registered.
        /// </summary>
        /// <param name="registrationType">The type of account being registered.</param>
        /// <returns>The promotion code to be used during registration.</returns>
        public string GetPromoCode(PostalAccountRegistrationType registrationType)
        {
            if (EndiciaAccountManager.Express1Accounts.Any() || StampsAccountManager.Express1Accounts.Any())
            {
                // 
                if (!StampsAccountManager.StampsAccounts.Any() && !EndiciaAccountManager.EndiciaAccounts.Any())
                {
                    // Promo code for customers that have an Express1 account but do not have a regular Stamps.com nor
                    // Endicia account; this will allow these customers to have a free Stamps.com account since their
                    // Express1 account was free
                    return "ShipWorks7";
                }
            }

            // Use the standard promo code if they're paying for an account from a USPS provider
            return "ShipWorks3";
        }
    }
}
