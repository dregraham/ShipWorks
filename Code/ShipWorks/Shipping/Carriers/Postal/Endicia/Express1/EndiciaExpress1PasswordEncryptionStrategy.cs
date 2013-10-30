using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// An implementation of the IExpress1PasswordEncryptionStrategy interface that is specific to the 
    /// registration of an Express1 for Endicia account.
    /// </summary>
    public class EndiciaExpress1PasswordEncryptionStrategy : IExpress1PasswordEncryptionStrategy
    {
        /// <summary>
        /// Encrypts the plain text password of the Express1 registration by using the value "Endicia"
        /// as the salt.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns>The encrypted password.</returns>
        public string EncryptPassword(Express1Registration registration)
        {
            return SecureText.Encrypt(registration.PlainTextPassword, "Endicia");
        }
    }
}
