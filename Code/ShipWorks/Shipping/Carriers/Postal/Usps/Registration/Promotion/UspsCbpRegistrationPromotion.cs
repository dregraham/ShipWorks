using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion
{
    /// <summary>
    /// An implementation of the IRegistrationPromotion intended to be used to create a 
    /// new standard account with CBP rates.
    /// </summary>
    public class UspsCbpRegistrationPromotion : IRegistrationPromotion
    {
        /// <summary>
        /// Gets a value indicating whether the promotion waives the monthly fee.
        /// </summary>
        public bool IsMonthlyFeeWaived
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the promo code to use when registering an account with USPS based on the
        /// type of account being registered.
        /// </summary>
        /// <returns>The promotion code to be used during registration.</returns>
        public string GetPromoCode()
        {
            // The promotion code for creating a standard USPS account with CBP rates.
            return "ShipWorks2";
        }
    }
}
