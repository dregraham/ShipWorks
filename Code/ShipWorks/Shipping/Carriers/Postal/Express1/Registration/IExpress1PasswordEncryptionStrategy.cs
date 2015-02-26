
namespace ShipWorks.Shipping.Carriers.Postal.Express1.Registration
{
    /// <summary>
    /// An interface that abstracts the password encryption strategy used on the password field
    /// when registering an Express1 account. This allows for the encryption strategy to be independent
    /// of the underlying carrier behind the Express1 account (i.e. Endicia or USPS).  
    /// </summary>
    public interface IExpress1PasswordEncryptionStrategy
    {
        /// <summary>
        /// Encrypts the plain text password of the Express1 registration.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns>An encrypted password.</returns>
        string EncryptPassword(Express1Registration registration);
    }
}
