using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Registration.Promotion
{
    /// <summary>
    /// An implementation of the IRegistrationPromotion intended to be used to create a 
    /// new Intuiship/expedited account when a Stamps.com already exists in ShipWorks.
    /// </summary>
    public class StampsIntuishipRegistrationPromotion : IRegistrationPromotion
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
            // The regular promotion code for a new expedited account when a Stamps.com 
            // account already exists in ShipWorks.
            return "ShipWorks3";
        }
    }
}
