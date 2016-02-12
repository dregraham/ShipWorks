using System;
using System.Linq;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.ApplicationCore.Licensing.Activation
{
    /// <summary>
    /// An implementation of the IUspsAccountSetupActivity interface that will populate an USPS account
    /// entity via the IUspsWebClient and saves the account to the data source.
    /// </summary>
    /// <seealso cref="ShipWorks.ApplicationCore.Licensing.Activation.IUspsAccountSetupActivity" />
    public class UspsAccountSetupActivity : IUspsAccountSetupActivity
    {
        private readonly Func<UspsResellerType, IUspsWebClient> uspsWebClientFactory;
        private readonly ICarrierAccountRepository<UspsAccountEntity> uspsAccountRepository;
        private readonly IShippingSettings shippingSettings;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsAccountSetupActivity"/> class.
        /// </summary>
        public UspsAccountSetupActivity(Func<UspsResellerType, IUspsWebClient> uspsWebClientFactory, 
            ICarrierAccountRepository<UspsAccountEntity> uspsAccountRepository, 
            IShippingSettings shippingSettings, 
            Func<Type, ILog> logFactory)
        {
            this.uspsWebClientFactory = uspsWebClientFactory;
            this.uspsAccountRepository = uspsAccountRepository;
            this.shippingSettings = shippingSettings;
            log = logFactory(typeof(UspsAccountSetupActivity));
        }

        /// <summary>
        /// Uses the user name and password provided to populate and save an USPS
        /// account entity.
        /// </summary>
        /// <param name="stampsUserName">The user name of the SDC account.</param>
        /// <param name="password">The password.</param>
        public void Execute(string stampsUserName, string password)
        {
            if (!uspsAccountRepository.Accounts.Any() && !string.IsNullOrWhiteSpace(stampsUserName))
            {
                UspsAccountEntity uspsAccount = new UspsAccountEntity
                {
                    Username = stampsUserName,
                    Password = SecureText.Encrypt(password, stampsUserName)
                };

                try
                {
                    log.Info($"Retreving account information for USPS account with username {uspsAccount.Username}...");
                    IUspsWebClient webClient = uspsWebClientFactory(UspsResellerType.None);
                    webClient.PopulateUspsAccountEntity(uspsAccount);

                    log.Info($"Saving USPS account with username {uspsAccount.Username}...");
                    uspsAccountRepository.Save(uspsAccount);

                    log.Info("The USPS account has been saved. Marking USPS carrier as configured...");
                    shippingSettings.MarkAsConfigured(ShipmentTypeCode.Usps);
                    log.Info("USPS has been configured.");
                }
                catch (Exception ex) when (ex is UspsApiException || ex is UspsException)
                {
                    // Populating the account information failed, due to an issue with USPS
                    // log the error and continue with the activation process
                    log.Error($"Error when populating USPS account information: {ex.Message}");
                }
            }
        }
    }
}
