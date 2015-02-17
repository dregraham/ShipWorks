using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion
{
    /// <summary>
    /// An implementation of the IRegistrationPromotion that should be used for creating new 
    /// Stamps.com accounts when migrating from an existing Endicia account.
    /// </summary>
    public class EndiciaIntuishipRegistrationPromotion : IRegistrationPromotion
    {

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
        /// <returns>The promotion code to be used during registration.</returns>
        public string GetPromoCode()
        {
            // The promotion code for a new expedited account.
            return "ShipWorks5";
        }
    }
}
