using System.Security;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;


namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for the CustomerLicenseActivationViewModel
    /// </summary>
    public interface ICustomerLicenseActivationViewModel
    {
        /// <summary>
        /// The Password
        /// </summary>
        SecureString Password { get; set; }

        /// <summary>
        /// The Username
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// The decrypted password
        /// </summary>
       string DecryptedPassword { get; }

        /// <summary>
        /// Called to save the credentials
        /// </summary>
        GenericResult<ICustomerLicense> Save();
    }
}