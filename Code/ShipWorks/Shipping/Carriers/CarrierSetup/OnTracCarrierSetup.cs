using System;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.CarrierSetup;
using ShipWorks.Shipping.Settings;
using ShipWorks.Warehouse.Configuration.DTO.ShippingSettings;

namespace ShipWorks.Shipping.Carriers.CarrierSetup
{
    /// <summary>
    /// Setup the OnTrac carrier configuration downloaded from the hub
    /// </summary>
    [KeyedComponent(typeof(ICarrierSetup), ShipmentTypeCode.OnTrac)]
    public class OnTracCarrierSetup : BaseCarrierSetup<OnTracAccountEntity, IOnTracAccountEntity>, ICarrierSetup
    {
        private readonly ICarrierAccountRepository<OnTracAccountEntity, IOnTracAccountEntity> accountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracCarrierSetup(IShipmentTypeSetupActivity shipmentTypeSetupActivity,
            IShippingSettings shippingSettings,
            IShipmentPrintHelper printHelper,
            ICarrierAccountRepository<OnTracAccountEntity, IOnTracAccountEntity> accountRepository)
            : base(shipmentTypeSetupActivity, shippingSettings, printHelper, accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        /// <summary>
        /// Setup an OnTrac account from data imported from the hub
        /// </summary>
        public void Setup(CarrierConfiguration config)
        {
            if (accountRepository.AccountsReadOnly.Any(x =>
                x.HubCarrierId == config.HubCarrierID && x.HubVersion >= config.HubVersion))
            {
                return;
            }

            var additionalAccountInfo = config.AdditionalData["ontrac"].ToObject<OnTracAccountConfiguration>();

            var ontracAccount = GetOrCreateAccountEntity(config.HubCarrierID);

            ontracAccount.AccountNumber = additionalAccountInfo.AccountNumber;
            ontracAccount.Password = SecureText.Encrypt(additionalAccountInfo.Password, additionalAccountInfo.AccountNumber.ToString());

            GetAddress(config.Address).CopyTo(ontracAccount, string.Empty);

            if (ontracAccount.IsNew)
            {
                ontracAccount.Description = OnTracAccountManager.GetDefaultDescription(ontracAccount);
                ontracAccount.InitializeNullsToDefault();
            }

            accountRepository.Save(ontracAccount);

            SetupDefaultsIfNeeded(ShipmentTypeCode.OnTrac, config.RequestedLabelFormat);
        }


        /// <summary>
        /// Get or create an OnTrac Account
        /// </summary>
        private OnTracAccountEntity GetOrCreateAccountEntity(Guid carrierId) =>
            accountRepository.Accounts.FirstOrDefault(x => x.HubCarrierId == carrierId)
            ?? new OnTracAccountEntity { HubCarrierId = carrierId };
    }
}