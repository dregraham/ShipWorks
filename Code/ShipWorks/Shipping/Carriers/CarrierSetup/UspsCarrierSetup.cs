using System;
using System.Linq;
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
        }

        /// <summary>
        /// Creates a new USPS account from data imported from the hub
        /// </summary>
        public void Setup(CarrierConfiguration config)
        {
            if (uspsAccountRepository.AccountsReadOnly.Any(x => x.HubCarrierId == config.HubCarrierID && x.HubVersion >= config.HubVersion))
            {
                return;
            }

            var account = config.AdditionalData["usps"].ToObject<UspsAccountConfiguration>();

            UspsAccountEntity uspsAccount = GetOrCreateAccountEntity(config.HubCarrierID);

            uspsAccount.Username = account.Username;
            uspsAccount.Password = SecureText.Encrypt(account.Password, account.Username);
            uspsAccount.HubVersion = config.HubVersion;
            uspsAccount.HubCarrierId = config.HubCarrierID;

            if (uspsAccount.IsNew)
            {
                webClient.PopulateUspsAccountEntity(uspsAccount);

                uspsAccount.PendingInitialAccount = (int) UspsPendingAccountType.None;
                uspsAccount.ShipEngineCarrierId = config.ShipEngineCarrierID;
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
