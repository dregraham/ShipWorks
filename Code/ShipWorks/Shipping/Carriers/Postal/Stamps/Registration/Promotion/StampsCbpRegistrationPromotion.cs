using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Registration.Promotion
{
    /// <summary>
    /// An implementation of the IRegistrationPromotion intended to be used to create a 
    /// new standard account with CBP rates.
    /// </summary>
    public class StampsCbpRegistrationPromotion : IRegistrationPromotion
    {
        /// <summary>
        /// This will only return the Standard type.
        /// </summary>
        public IEnumerable<PostalAccountRegistrationType> AvailableRegistrationTypes
        {
            get { return new List<PostalAccountRegistrationType> { PostalAccountRegistrationType.Standard }; }
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
            // The promotion code for creating a standard Stamps.com account with CBP rates.
            return "ShipWorks4";
        }
    }
}
