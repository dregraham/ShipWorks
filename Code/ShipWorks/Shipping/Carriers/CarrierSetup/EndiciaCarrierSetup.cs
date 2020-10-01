using System;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.CarrierSetup;
using ShipWorks.Shipping.Settings;
using ShipWorks.Warehouse.Configuration.DTO.ShippingSettings;

namespace ShipWorks.Shipping.Carriers.CarrierSetup
{
    /// <summary>
    /// Setup the Endicia carrier configuration downloaded from the hub
    /// </summary>
    [KeyedComponent(typeof(ICarrierSetup), ShipmentTypeCode.Endicia)]
    public class EndiciaCarrierSetup : BaseCarrierSetup<EndiciaAccountEntity, IEndiciaAccountEntity>, ICarrierSetup
    {
        private readonly ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity> endiciaAccountRepository;
        private readonly IShippingSettings shippingSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaCarrierSetup(IShipmentTypeSetupActivity shipmentTypeSetupActivity,
            ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity> endiciaAccountRepository,
            IShippingSettings shippingSettings,
            IShipmentPrintHelper printHelper,
            ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity> accountRepository)
            : base(shipmentTypeSetupActivity, shippingSettings, printHelper, accountRepository)
        {
            this.endiciaAccountRepository = endiciaAccountRepository;
            this.shippingSettings = shippingSettings;
        }

        /// <summary>
        /// Setup an Endicia account from data imported from the hub
        /// </summary>
        public void Setup(CarrierConfiguration config)
        {
            if (endiciaAccountRepository.AccountsReadOnly.Any(x =>
                x.HubCarrierId == config.HubCarrierID && x.HubVersion >= config.HubVersion))
            {
                return;
            }

            var additionalAccountInfo = config.AdditionalData["endicia"].ToObject<EndiciaAccountConfiguration>();

            var endiciaAccount = GetOrCreateAccountEntity(config.HubCarrierID);

            GetAddress(config.Address).CopyTo(endiciaAccount, string.Empty);
            endiciaAccount.HubVersion = config.HubVersion;

            if (endiciaAccount.IsNew)
            {
                if (!string.IsNullOrEmpty(additionalAccountInfo.Signature))
                {
                    ShippingSettingsEntity settings = shippingSettings.Fetch();
                    settings.EndiciaCustomsSigner = additionalAccountInfo.Signature;
                    settings.EndiciaCustomsCertify = true;
                    shippingSettings.Save(settings);
                }

                endiciaAccount.AccountNumber = additionalAccountInfo.AccountNumber;
                endiciaAccount.ApiUserPassword = additionalAccountInfo.Passphrase;
                endiciaAccount.CreatedByShipWorks = false;
                endiciaAccount.EndiciaReseller = (int) EndiciaReseller.None;
                endiciaAccount.WebPassword = string.Empty;
                endiciaAccount.ApiInitialPassword = string.Empty;
                endiciaAccount.SignupConfirmation = string.Empty;
                endiciaAccount.AccountType = -1;
                endiciaAccount.TestAccount = false;
                endiciaAccount.ScanFormAddressSource = (int) EndiciaScanFormAddressSource.Provider;
                endiciaAccount.AcceptedFCMILetterWarning = false;
                endiciaAccount.Description = EndiciaAccountManager.GetDefaultDescription(endiciaAccount);
                endiciaAccount.InitializeNullsToDefault();
            }

            endiciaAccountRepository.Save(endiciaAccount);

            SetupDefaultsIfNeeded(ShipmentTypeCode.Endicia, config.RequestedLabelFormat);
        }

        /// <summary>
        /// Get an existing Endicia account
        /// </summary>
        private EndiciaAccountEntity GetOrCreateAccountEntity(Guid carrierId) =>
            endiciaAccountRepository.Accounts.FirstOrDefault(x => x.HubCarrierId == carrierId)
            ?? new EndiciaAccountEntity { HubCarrierId = carrierId };
    }
}