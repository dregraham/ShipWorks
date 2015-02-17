using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion
{
    /// <summary>
    /// An IRegistrationPromotion to be used when signing up for a new USPS
    /// account when Express1 is the only Postsal acount in the ShipWorks.
    /// </summary>
    public class Express1OnlyRegistrationPromotion : IRegistrationPromotion
    {
        /// <summary>
        /// Gets a value indicating whether the promotion waives the monthly fee.
        /// </summary>
        public bool IsMonthlyFeeWaived
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the promo code to use when registering an account with USPS based on the
        /// type of account being registered.
        /// </summary>
        /// <returns>The promotion code to be used during registration.</returns>
        public string GetPromoCode()
        {
            // Promo code for customers that have an Express1 account but do not have a regular USPS nor
            // Endicia account; this will allow these customers to have a free USPS account since their
            // Express1 account was free
            return "ShipWorks7";
        }
    }
}
