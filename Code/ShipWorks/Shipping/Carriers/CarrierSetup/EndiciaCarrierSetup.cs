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
    /// Setup the Endicia carrier configuration downloaded from the hub
    /// </summary>
    [KeyedComponent(typeof(ICarrierSetup), ShipmentTypeCode.Endicia)]
    public class EndiciaCarrierSetup : BaseCarrierSetup<EndiciaAccountEntity, IEndiciaAccountEntity>, ICarrierSetup
    {
        private readonly ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity> endiciaAccountRepository;

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

            var endiciaAccount = GetAccountEntity(config.HubCarrierID);

            GetAddress(config.Address).CopyTo(endiciaAccount, string.Empty);

            endiciaAccountRepository.Save(endiciaAccount);

            SetupDefaultsIfNeeded(ShipmentTypeCode.Endicia, config.RequestedLabelFormat);
        }

        /// <summary>
        /// Get an existing Endicia account
        /// </summary>
        private EndiciaAccountEntity GetAccountEntity(Guid carrierId) =>
            endiciaAccountRepository.Accounts.FirstOrDefault(x => x.HubCarrierId == carrierId);
    }
}