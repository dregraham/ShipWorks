using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Registration
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
        /// Gets the available account registration types. These values could differ across implementations
        /// based on factors such as whether the account is a brand new account, being migrated from another
        /// postal carrier, etc.
        /// </summary>
        IEnumerable<PostalAccountRegistrationType> AvailableAccountTypes { get; }

        /// <summary>
        /// Gets the promo code to use when registering an account with Stamps.com based on the 
        /// type of account being registered.
        /// </summary>
        /// <param name="registrationType">The type of account being registered.</param>
        /// <returns>The promotion code to be used during registration.</returns>
        string GetPromoCode(PostalAccountRegistrationType registrationType);
    }
}
