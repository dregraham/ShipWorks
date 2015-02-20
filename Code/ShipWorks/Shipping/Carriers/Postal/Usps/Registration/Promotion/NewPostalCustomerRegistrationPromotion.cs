using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion
{
    /// <summary>
    /// This is intended to be used when there aren't any postal accounts in ShipWorks; this is 
    /// considered to be a brand new customer, so use a promo code to indicate such to allow 
    /// USPS to track this information
    /// </summary>
    public class NewPostalCustomerRegistrationPromotion : IRegistrationPromotion
    {
        /// <summary>
        /// Gets a value indicating whether the promotion waives the monthly fee.
        /// </summary>
        public bool IsMonthlyFeeWaived
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the promo code to use when registering an account with USPS
        /// </summary>
        /// <returns>The promotion code to be used during registration.</returns>
        public string GetPromoCode()
        {            
            return "ShipWorks6";
        }
    }
}
