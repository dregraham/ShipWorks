using System;
using System.Linq;
using System.Net;
using Interapptive.Shared.Security;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.Profiles;

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
        private readonly ILog log;
        private readonly IShipmentTypeSetupActivity shipmentTypeSetupActivity;
        private readonly IShippingProfileManager shippingProfileManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsAccountSetupActivity"/> class.
        /// </summary>
        public UspsAccountSetupActivity(Func<UspsResellerType, IUspsWebClient> uspsWebClientFactory,
            ICarrierAccountRepository<UspsAccountEntity> uspsAccountRepository,
            IShipmentTypeSetupActivity shipmentTypeSetupActivity,
            IShippingProfileManager shippingProfileManager,
            Func<Type, ILog> logFactory)
        {
            this.shipmentTypeSetupActivity = shipmentTypeSetupActivity;
            this.uspsWebClientFactory = uspsWebClientFactory;
            this.uspsAccountRepository = uspsAccountRepository;
            this.shippingProfileManager = shippingProfileManager;
            log = logFactory(typeof(UspsAccountSetupActivity));
        }

        /// <summary>
        /// Uses the user name and password provided to populate and save an USPS
        /// account entity.
        /// </summary>
        public void Execute(ICustomerLicense license, string password)
        {
            if (!uspsAccountRepository.Accounts.Any())
            {
                shippingProfileManager.InitializeForCurrentSession();

                if (!string.IsNullOrWhiteSpace(license?.AssociatedStampsUsername))
                {
                    CreateExistingAccount(license.AssociatedStampsUsername, password);
                }
                else if (!string.IsNullOrWhiteSpace(license?.StampsUsername))
                {
                    CreateNewAccount(license.StampsUsername, password);
                }
            }
        }

        /// <summary>
        /// Creates an existing Stamps account in ShipWorks by getting the
        /// required information from the UspsWebClient.
        /// </summary>
        private void CreateExistingAccount(string username, string password)
        {
            UspsAccountEntity uspsAccount = new UspsAccountEntity
            {
                Username = username,
                Password = SecureText.Encrypt(password, username),
                PendingInitialAccount = (int)UspsPendingAccountType.Existing
            };

            try
            {
                log.Info($"Retreving account information for USPS account with username {uspsAccount.Username}...");
                IUspsWebClient webClient = uspsWebClientFactory(UspsResellerType.None);
                webClient.PopulateUspsAccountEntity(uspsAccount);

                log.Info($"Saving USPS account with username {uspsAccount.Username}...");
                uspsAccountRepository.Save(uspsAccount);

                log.Info("The USPS account has been saved. Setting USPS as the default shipping provider.");
                shipmentTypeSetupActivity.InitializeShipmentType(ShipmentTypeCode.Usps, ShipmentOriginSource.Account);
            }
                catch (Exception ex) when (ex is UspsApiException || ex is UspsException || ex is WebException)
            {
                // Populating the account information failed, due to an issue with USPS
                // log the error and continue with the activation process
                log.Error($"Error when populating USPS account information: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a shell UspsAccount. The rest of the information will be entered when the user
        /// sets up a UspsAccount.
        /// </summary>
        private void CreateNewAccount(string username, string password)
        {
            UspsAccountEntity uspsAccount = new UspsAccountEntity
            {
                Username = username,
                Password = SecureText.Encrypt(password, username),
                PendingInitialAccount = (int)UspsPendingAccountType.Create,
                CreatedDate = DateTime.UtcNow
            };

            uspsAccount.InitializeNullsToDefault();
            uspsAccountRepository.Save(uspsAccount);

            shipmentTypeSetupActivity.InitializeShipmentType(ShipmentTypeCode.Usps, ShipmentOriginSource.Store);
        }
    }
}
