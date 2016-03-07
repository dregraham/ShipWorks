using System;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing.Activation
{
    /// <summary>
    /// An implementation of ICustomerLicenseActivationActivity that will carry out the work for
    /// activating a customer license.
    /// </summary>
    /// <seealso cref="ShipWorks.ApplicationCore.Licensing.Activation.ICustomerLicenseActivationActivity" />
    public class CustomerLicenseActivationActivity : ICustomerLicenseActivationActivity
    {
        private readonly ITangoWebClient tangoWebClient;
        private readonly Func<string, ICustomerLicense> licenseFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerLicenseActivationActivity"/> class.
        /// </summary>
        public CustomerLicenseActivationActivity(ITangoWebClient tangoWebClient, Func<string, ICustomerLicense> licenseFactory)
        {
            this.tangoWebClient = tangoWebClient;
            this.licenseFactory = licenseFactory;
        }

        /// <summary>
        /// Uses the credentials provided to activate a customer license with the Tango web 
        /// client. Once verified with Tango, the license key is saved.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns>An instance of an activated ICustomerLicense.</returns>
        /// <exception cref="ShipWorksLicenseException">A ShipWorksLicenseException is thrown when 
        /// the license could not be activated with Tango.</exception>
        public ICustomerLicense Execute(string email, string password)
        {
            GenericResult<IActivationResponse> activateLicenseResponse = tangoWebClient.ActivateLicense(email, password);

            // Check to see if something went wrong and if so we throw
            if (!activateLicenseResponse.Success)
            {
                throw new ShipWorksLicenseException(activateLicenseResponse.Message);
            }
            
            ICustomerLicense license = licenseFactory(activateLicenseResponse.Context.Key);
            license.AssociatedStampsUsername = activateLicenseResponse.Context.AssociatedStampsUsername;
            license.Save();

            return license;
        }
    }
}
