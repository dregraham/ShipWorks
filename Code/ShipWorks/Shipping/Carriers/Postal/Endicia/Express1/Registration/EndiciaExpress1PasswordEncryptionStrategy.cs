using System;
using Interapptive.Shared.Security;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1.Registration
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
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }

            return SecureText.Encrypt(registration.PlainTextPassword, "Endicia");
        }
    }
}
