using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Settings;
using ShipWorks.Warehouse.Configuration.Carriers.DTO;

namespace ShipWorks.Warehouse.Configuration.Carriers
{
    /// <summary>
    /// Setup the Express1 USPS Carrier configuration downloaded from the Hub
    /// </summary>
    [KeyedComponent(typeof(ICarrierSetup), ShipmentTypeCode.Express1Usps)]
    public class Express1UspsCarrierSetup : BaseCarrierSetup<UspsAccountEntity, IUspsAccountEntity>, ICarrierSetup
    {
        private readonly ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> express1AccountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1UspsCarrierSetup(IShipmentTypeSetupActivity shipmentTypeSetupActivity,
            IIndex<ShipmentTypeCode, ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> accountRepositoryFactory,
            IShippingSettings shippingSettings,
            IShipmentPrintHelper printHelper) :
            base(shipmentTypeSetupActivity, shippingSettings, printHelper, accountRepositoryFactory[ShipmentTypeCode.Express1Usps])
        {
            this.express1AccountRepository = accountRepositoryFactory[ShipmentTypeCode.Express1Usps];
        }

        /// <summary>
        /// Creates a new Express1 USPS account from data imported from the hub
        /// </summary>
#pragma warning disable 1998
        public async Task Setup(CarrierConfiguration config)
#pragma warning restore 1998
        {
            if (express1AccountRepository.AccountsReadOnly.Any(x => x.HubCarrierId == config.HubCarrierID && x.HubVersion >= config.HubVersion))
            {
                return;
            }

            var account = config.AdditionalData["express1"].ToObject<Express1AccountConfiguration>();

            UspsAccountEntity express1Account = GetOrCreateAccountEntity(config.HubCarrierID);

            express1Account.Password = SecureText.Encrypt(account.Password, account.Username);
            express1Account.HubVersion = config.HubVersion;

            GetAddress(config.Address).CopyTo(express1Account, string.Empty);

            if (express1Account.IsNew)
            {
                express1Account.Username = account.Username;
                express1Account.ContractType = (int) UspsAccountContractType.NotApplicable;
                express1Account.PendingInitialAccount = (int) UspsPendingAccountType.None;
                express1Account.ShipEngineCarrierId = config.ShipEngineCarrierID;
                express1Account.CreatedDate = DateTime.UtcNow;
                express1Account.Description = UspsAccountManager.GetDefaultDescription(express1Account);
                express1Account.InitializeNullsToDefault();
            }

            express1AccountRepository.Save(express1Account);

            SetupDefaultsIfNeeded(ShipmentTypeCode.Express1Usps, config.RequestedLabelFormat);
        }

        /// <summary>
        /// Get an existing account entity, or create a new one if none exist with the given account ID
        /// </summary>
        private UspsAccountEntity GetOrCreateAccountEntity(Guid carrierID) =>
            express1AccountRepository.Accounts.FirstOrDefault(x => x.HubCarrierId == carrierID) ?? new UspsAccountEntity { HubCarrierId = carrierID, UspsReseller = (int) UspsResellerType.Express1 };
    }
}
