using System;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.CarrierSetup;
using ShipWorks.Shipping.Settings;
using ShipWorks.Warehouse.Configuration.DTO.ShippingSettings;

namespace ShipWorks.Shipping.Carriers.CarrierSetup
{
    /// <summary>
    /// Setup the DHL carrier configuration downloaded from the hub
    /// </summary>
    [KeyedComponent(typeof(ICarrierSetup), ShipmentTypeCode.DhlExpress)]
    public class DhlCarrierSetup : BaseCarrierSetup<DhlExpressAccountEntity, IDhlExpressAccountEntity>, ICarrierSetup
    {
        private readonly ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity> accountRepository;
        private readonly IShippingSettings shippingSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlCarrierSetup(IShipmentTypeSetupActivity shipmentTypeSetupActivity,
            IShippingSettings shippingSettings, 
            IShipmentPrintHelper printHelper, 
            ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity> accountRepository) 
            : base(shipmentTypeSetupActivity, shippingSettings, printHelper, accountRepository)
        {
            this.accountRepository = accountRepository;
            this.shippingSettings = shippingSettings;
        }

        /// <summary>
        /// Setup a DHL account from data imported from the hub
        /// </summary>
        public void Setup(CarrierConfiguration config)
        {
            if (accountRepository.AccountsReadOnly.Any(x => 
                x.HubCarrierId == config.HubCarrierID && x.HubVersion >= config.HubVersion))
            {
                return;
            }

            var additionalAccountInfo = config.AdditionalData["dhl"].ToObject<DhlAccountConfiguration>();
            var dhlAccount = GetOrCreateAccountEntity(config.HubCarrierID);

            GetAddress(config.Address).CopyTo(dhlAccount, string.Empty);
            dhlAccount.HubVersion = config.HubVersion;

            if (dhlAccount.IsNew)
            {
                dhlAccount.AccountNumber = additionalAccountInfo.AccountNumber;
            }

            accountRepository.Save(dhlAccount);
            SetupDefaultsIfNeeded(ShipmentTypeCode.DhlExpress, config.RequestedLabelFormat);
        }

        /// <summary>
        /// Get or Create a DHL account
        /// </summary>
        private DhlExpressAccountEntity GetOrCreateAccountEntity(Guid carrierId) =>
            accountRepository.Accounts.FirstOrDefault(x => x.HubCarrierId == carrierId)
            ?? new DhlExpressAccountEntity {HubCarrierId = carrierId};
    }
}