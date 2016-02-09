using System;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerLicenseActivationService"/> class.
        /// </summary>
        /// <param name="tangoWebClient">The tango web client.</param>
        /// <param name="uspsWebClient">The usps web client.</param>
        /// <param name="uspsAccountRepository">The usps account repository.</param>
        /// <param name="licenseFactory">The license factory.</param>
        public CustomerLicenseActivationService(ITangoWebClient tangoWebClient, IUspsWebClient uspsWebClient, ICarrierAccountRepository<UspsAccountEntity> uspsAccountRepository, Func<string, ICustomerLicense> licenseFactory)
        {
            this.tangoWebClient = tangoWebClient;
            this.uspsWebClient = uspsWebClient;
            this.uspsAccountRepository = uspsAccountRepository;
            this.licenseFactory = licenseFactory;
        }

        /// <summary>
        /// Activates a CustomerLicense
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ICustomerLicense Activate(string username, string password)
        {
            GenericResult<ActivationResponse> activateLicenseResponse = tangoWebClient.ActivateLicense(username, password);

            // Check to see if something went wrong and if so we throw
            if (!activateLicenseResponse.Success)
            {
                throw new ShipWorksLicenseException(activateLicenseResponse.Message);
            }

            if (!uspsAccountRepository.Accounts.Any())
            {
                UspsAccountEntity uspsAccount = new UspsAccountEntity()
                {
                    Username = activateLicenseResponse.Context.StampsUsername,
                    Password = password
                };

                uspsWebClient.PopulateUspsAccountEntity(uspsAccount);

                uspsAccountRepository.Save(uspsAccount);
            }

            ICustomerLicense license = licenseFactory(activateLicenseResponse.Context.Key);

            license.Save();

            return license;
        }
    }
}