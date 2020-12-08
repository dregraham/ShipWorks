using System;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.CarrierSetup;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Settings;
using ShipWorks.Warehouse.Configuration.DTO.ShippingSettings;

namespace ShipWorks.Shipping.CarrierSetup
{
    /// <summary>
    /// Setup the USPS Carrier configuration downloaded from the Hub
    /// </summary>
    [KeyedComponent(typeof(ICarrierSetup), ShipmentTypeCode.Usps)]
    public class UspsCarrierSetup : BaseCarrierSetup<UspsAccountEntity, IUspsAccountEntity>, ICarrierSetup
    {
        private readonly ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository;
        private readonly IUspsWebClient webClient;
        private readonly IShippingSettings shippingSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsCarrierSetup(IShipmentTypeSetupActivity shipmentTypeSetupActivity,
            ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository,
            IShippingSettings shippingSettings,
            IShipmentPrintHelper printHelper,
            Func<UspsResellerType, IUspsWebClient> uspsWebClientFactory) :
            base(shipmentTypeSetupActivity, shippingSettings, printHelper, uspsAccountRepository)
        {
            this.uspsAccountRepository = uspsAccountRepository;
            this.webClient = uspsWebClientFactory(UspsResellerType.None);
            this.shippingSettings = shippingSettings;
        }

        /// <summary>
        /// Creates a new USPS account from data imported from the hub
        /// </summary>
#pragma warning disable 1998
        public async Task Setup(CarrierConfiguration config, UspsAccountEntity oneBalanceUspsAccount)
#pragma warning restore 1998
        {
            // skip if we already know about this account
            if (uspsAccountRepository.AccountsReadOnly.Any(x => x.HubCarrierId == config.HubCarrierID && x.HubVersion >= config.HubVersion))
            {
                return;
            }

            // skip if there is already a one balance account and this config is for onebalance
            if (config.IsOneBalance &&
                uspsAccountRepository.AccountsReadOnly.Any(x => !string.IsNullOrWhiteSpace(x.ShipEngineCarrierId)))
            {
                return;
            }

            var account = config.AdditionalData["usps"].ToObject<UspsAccountConfiguration>();

            UspsAccountEntity uspsAccount = GetOrCreateAccountEntity(config.HubCarrierID);

            uspsAccount.Password = SecureText.Encrypt(account.Password, account.Username);
            uspsAccount.HubVersion = config.HubVersion;

            if (uspsAccount.IsNew)
            {
                ShippingSettingsEntity settings  = shippingSettings.Fetch();

                if (string.IsNullOrWhiteSpace(settings.ShipEngineAccountID))
                {
                    uspsAccount.ShipEngineCarrierId = config.ShipEngineCarrierID;
                }

                uspsAccount.Username = account.Username;
                webClient.PopulateUspsAccountEntity(uspsAccount);
                uspsAccount.PendingInitialAccount = (int) UspsPendingAccountType.None;
                uspsAccount.InitializeNullsToDefault();
            }

            GetAddress(config.Address).CopyTo(uspsAccount, string.Empty);

            uspsAccountRepository.Save(uspsAccount);

            SetupDefaultsIfNeeded(ShipmentTypeCode.Usps, config.RequestedLabelFormat);
        }

        /// <summary>
        /// Get an existing account entity, or create a new one if none exist with the given account ID
        /// </summary>
        private UspsAccountEntity GetOrCreateAccountEntity(Guid carrierID) =>
            uspsAccountRepository.Accounts.FirstOrDefault(x => x.HubCarrierId == carrierID) ?? new UspsAccountEntity { HubCarrierId = carrierID };
    }
}
