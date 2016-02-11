using System;
using System.Linq;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;


namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Service for activating a customer license
    /// </summary>
    public class CustomerLicenseActivationService : ICustomerLicenseActivationService
    {
        private readonly ITangoWebClient tangoWebClient;
        private readonly IUspsWebClient uspsWebClient;
        private readonly ICarrierAccountRepository<UspsAccountEntity> uspsAccountRepository;
        private readonly Func<string, ICustomerLicense> licenseFactory;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerLicenseActivationService"/> class.
        /// </summary>
        /// <param name="tangoWebClient">The tango web client.</param>
        /// <param name="uspsWebClientFactory"></param>
        /// <param name="uspsAccountRepository">The usps account repository.</param>
        /// <param name="licenseFactory">The license factory.</param>
        /// <param name="logFactory">Logging factory</param>
        public CustomerLicenseActivationService(ITangoWebClient tangoWebClient,
            Func<UspsResellerType, IUspsWebClient> uspsWebClientFactory,
            ICarrierAccountRepository<UspsAccountEntity> uspsAccountRepository,
            Func<string, ICustomerLicense> licenseFactory, 
            Func<Type, ILog> logFactory)
        {
            this.tangoWebClient = tangoWebClient;
            uspsWebClient = uspsWebClientFactory(UspsResellerType.None);
            this.uspsAccountRepository = uspsAccountRepository;
            this.licenseFactory = licenseFactory;
            log = logFactory(typeof (CustomerLicenseActivationService));
        }

        /// <summary>
        /// Attempts to use the email address and password provided to activate a customer license with Tango.
        /// </summary>
        public ICustomerLicense Activate(string email, string password)
        {
            GenericResult<IActivationResponse> activateLicenseResponse = tangoWebClient.ActivateLicense(email, password);

            // Check to see if something went wrong and if so we throw
            if (!activateLicenseResponse.Success)
            {
                throw new ShipWorksLicenseException(activateLicenseResponse.Message);
            }

            string associatedStampsUserName = activateLicenseResponse.Context.AssociatedStampsUserName;
            if (!uspsAccountRepository.Accounts.Any() && !string.IsNullOrWhiteSpace(associatedStampsUserName))
            {
                UspsAccountEntity uspsAccount = new UspsAccountEntity
                {
                    Username = associatedStampsUserName,
                    Password = SecureText.Encrypt(password, associatedStampsUserName)
                };

                try
                {
                    uspsWebClient.PopulateUspsAccountEntity(uspsAccount);
                    uspsAccountRepository.Save(uspsAccount);
                }
                catch (Exception ex) when (ex is UspsApiException || ex is UspsException)
                {
                    // Populating the account information failed, due to an issue with USPS
                    // log the error and continue with the activation process
                    log.Error($"Error when populating USPS account information: {ex.Message}");
                }
            }

            ICustomerLicense license = licenseFactory(activateLicenseResponse.Context.Key);
            license.Save();

            return license;
        }
    }
}