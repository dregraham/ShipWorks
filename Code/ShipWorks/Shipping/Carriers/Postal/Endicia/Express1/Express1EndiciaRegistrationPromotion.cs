using System.Collections.Generic;

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
            return "ShipWorks3";
        }
    }
}
