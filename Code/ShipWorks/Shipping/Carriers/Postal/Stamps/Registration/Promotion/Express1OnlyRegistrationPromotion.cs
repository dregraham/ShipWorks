using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Registration.Promotion
{
    /// <summary>
    /// An IRegistrationPromotion to be used when signing up for a new USPS (Stamps.com Expedited)
    /// account when Express1 is the only Postsal acount in the ShipWorks.
    /// </summary>
    public class Express1OnlyRegistrationPromotion : IRegistrationPromotion
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
        /// Gets a value indicating whether the promotion waives the monthly fee.
        /// </summary>
        public bool IsMonthlyFeeWaived
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the promo code to use when registering an account with Stamps.com based on the
        /// type of account being registered.
        /// </summary>
        /// <param name="registrationType">The type of account being registered.</param>
        /// <returns>The promotion code to be used during registration.</returns>
        public string GetPromoCode(PostalAccountRegistrationType registrationType)
        {
            // Promo code for customers that have an Express1 account but do not have a regular Stamps.com nor
            // Endicia account; this will allow these customers to have a free Stamps.com account since their
            // Express1 account was free
            return "ShipWorks7";
        }
    }
}
