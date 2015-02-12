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
            // The regular promotion code for a new expedited account when a Stamps.com 
            // account already exists in ShipWorks.
            return "ShipWorks3";
        }
    }
}
