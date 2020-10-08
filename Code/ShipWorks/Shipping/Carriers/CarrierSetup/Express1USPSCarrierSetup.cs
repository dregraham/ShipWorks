using System;
using System.Linq;
using Autofac.Features.Indexed;
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
    /// Setup the Express1 USPS Carrier configuration downloaded from the Hub
    /// </summary>
    [KeyedComponent(typeof(ICarrierSetup), ShipmentTypeCode.Express1Usps)]
    public class Express1UspsCarrierSetup : BaseCarrierSetup<UspsAccountEntity, IUspsAccountEntity>, ICarrierSetup
    {
        private readonly ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> express1AccountRepository;
        private readonly IUspsWebClient webClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1UspsCarrierSetup(IShipmentTypeSetupActivity shipmentTypeSetupActivity,
            IIndex<ShipmentTypeCode, ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> accountRepositoryFactory,
            IShippingSettings shippingSettings,
            IShipmentPrintHelper printHelper,
            Func<UspsResellerType, IUspsWebClient> uspsWebClientFactory) :
            base(shipmentTypeSetupActivity, shippingSettings, printHelper, accountRepositoryFactory[ShipmentTypeCode.Express1Usps])
        {
            this.express1AccountRepository = accountRepositoryFactory[ShipmentTypeCode.Express1Usps];
            this.webClient = uspsWebClientFactory(UspsResellerType.Express1);
        }

        /// <summary>
        /// Creates a new Express1 USPS account from data imported from the hub
        /// </summary>
        public void Setup(CarrierConfiguration config)
        {
            if (express1AccountRepository.AccountsReadOnly.Any(x => x.HubCarrierId == config.HubCarrierID && x.HubVersion >= config.HubVersion))
            {
                return;
            }

            var account = config.AdditionalData["express1"].ToObject<Express1UspsAccountConfiguration>();

            UspsAccountEntity express1Account = GetOrCreateAccountEntity(config.HubCarrierID);

            express1Account.Username = account.Username;
            express1Account.Password = SecureText.Encrypt(account.Password, account.Username);
            express1Account.HubVersion = config.HubVersion;

            if (express1Account.IsNew)
            {
                webClient.PopulateUspsAccountEntity(express1Account);

                express1Account.PendingInitialAccount = (int) UspsPendingAccountType.None;
                express1Account.ShipEngineCarrierId = config.ShipEngineCarrierID;
                express1Account.InitializeNullsToDefault();
            }

            GetAddress(config.Address).CopyTo(express1Account, string.Empty);

            express1AccountRepository.Save(express1Account);

            SetupDefaultsIfNeeded(ShipmentTypeCode.Usps, config.RequestedLabelFormat);
        }

        /// <summary>
        /// Get an existing account entity, or create a new one if none exist with the given account ID
        /// </summary>
        private UspsAccountEntity GetOrCreateAccountEntity(Guid carrierID) =>
            express1AccountRepository.Accounts.FirstOrDefault(x => x.HubCarrierId == carrierID) ?? new UspsAccountEntity { HubCarrierId = carrierID, UspsReseller = (int) UspsResellerType.Express1 };
    }
}
