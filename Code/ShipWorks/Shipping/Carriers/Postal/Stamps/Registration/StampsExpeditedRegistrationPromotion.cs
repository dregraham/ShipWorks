using System.Collections.Generic;

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
        /// Gets the promo code to use when registering an account with Stamps.com based on the
        /// type of account being registered.
        /// </summary>
        /// <param name="registrationType">The type of account being registered.</param>
        /// <returns>The promotion code to be used during registration.</returns>
        public string GetPromoCode(PostalAccountRegistrationType registrationType)
        {
            // The promotion code for a new expedited account.
            return "shipworks3";
        }
    }
}
