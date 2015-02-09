using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// An interface intended to obtain the promotion code that should be used when registering
    /// an account with Stamps.com. The promotion code could differ based on the context that the 
    /// account is being registered within. For example, the promo code could differ based on the
    /// account being registered (Expedited vs Standard), whether a customer is migrating from 
    /// another carrier, etc.
    /// </summary>
    public interface IRegistrationPromotion
    {
        /// <summary>
        /// Gets a value indicating whether the promotion waives the monthly fee.
        /// </summary>
        bool IsMonthlyFeeWaived { get; }

        /// <summary>
        /// Gets the promo code to use when registering an account with Stamps.com based on the 
        /// type of account being registered.
        /// </summary>
        /// <returns>The promotion code to be used during registration.</returns>
        string GetPromoCode();
    }
}
