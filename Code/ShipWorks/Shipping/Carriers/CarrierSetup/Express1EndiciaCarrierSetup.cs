using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Account;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.CarrierSetup;
using ShipWorks.Shipping.Settings;
using ShipWorks.Warehouse.Configuration.DTO.ShippingSettings;

namespace ShipWorks.Shipping.Carriers.CarrierSetup
{
    /// <summary>
    /// Setup the Express1 Endicia Carrier configuration downloaded from the Hub
    /// </summary>
    [KeyedComponent(typeof(ICarrierSetup), ShipmentTypeCode.Express1Endicia)]
    public class Express1EndiciaCarrierSetup : BaseCarrierSetup<EndiciaAccountEntity, IEndiciaAccountEntity>, ICarrierSetup
    {
        private readonly ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity> express1AccountRepository;
        private readonly IShippingSettings shippingSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1EndiciaCarrierSetup(IShipmentTypeSetupActivity shipmentTypeSetupActivity,
            IIndex<ShipmentTypeCode, ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity>> accountRepositoryFactory,
            IShippingSettings shippingSettings,
            IShipmentPrintHelper printHelper) :
            base(shipmentTypeSetupActivity, shippingSettings, printHelper, accountRepositoryFactory[ShipmentTypeCode.Express1Endicia])
        {
            this.express1AccountRepository = accountRepositoryFactory[ShipmentTypeCode.Express1Endicia];
            this.shippingSettings = shippingSettings;
        }

        /// <summary>
        /// Creates a new Express1 Endicia account from data imported from the hub
        /// </summary>
#pragma warning disable 1998
        public async Task Setup(CarrierConfiguration config, IUspsAccountEntity oneBalanceUspsAccount)
#pragma warning restore 1998
        {
            if (express1AccountRepository.AccountsReadOnly.Any(x => x.HubCarrierId == config.HubCarrierID && x.HubVersion >= config.HubVersion))
            {
                return;
            }

            var account = config.AdditionalData["express1"].ToObject<Express1AccountConfiguration>();

            EndiciaAccountEntity express1Account = GetOrCreateAccountEntity(config.HubCarrierID);

            express1Account.ApiUserPassword = SecureText.Encrypt(account.Password, "Endicia");
            express1Account.HubVersion = config.HubVersion;

            GetAddress(config.Address).CopyTo(express1Account, string.Empty);

            if (express1Account.IsNew)
            {
                if (!string.IsNullOrEmpty(account.Signature))
                {
                    ShippingSettingsEntity settings = shippingSettings.Fetch();
                    settings.Express1EndiciaCustomsSigner = account.Signature;
                    settings.Express1EndiciaCustomsCertify = true;
                    shippingSettings.Save(settings);
                }

                express1Account.AccountNumber = account.Username;
                express1Account.CreatedByShipWorks = false;
                express1Account.AccountType = (int) EndiciaAccountType.Standard;
                express1Account.ScanFormAddressSource = (int) EndiciaScanFormAddressSource.Provider;
                express1Account.TestAccount = Express1EndiciaUtility.UseTestServer;
                express1Account.AcceptedFCMILetterWarning = false;
                express1Account.Description = EndiciaAccountManager.GetDefaultDescription(express1Account);
                express1Account.InitializeNullsToDefault();
            }

            express1AccountRepository.Save(express1Account);

            SetupDefaultsIfNeeded(ShipmentTypeCode.Express1Endicia, config.RequestedLabelFormat);
        }

        /// <summary>
        /// Get an existing account entity, or create a new one if none exist with the given account ID
        /// </summary>
        private EndiciaAccountEntity GetOrCreateAccountEntity(Guid carrierID) =>
            express1AccountRepository.Accounts.FirstOrDefault(x => x.HubCarrierId == carrierID) ?? new EndiciaAccountEntity { HubCarrierId = carrierID, EndiciaReseller = (int) EndiciaReseller.Express1 };
    }
}
