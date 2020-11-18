using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Warehouse.Configuration.Carriers.DTO;

namespace ShipWorks.Warehouse.Configuration.Carriers
{
    /// <summary>
    /// Setup the DHL carrier configuration downloaded from the hub
    /// </summary>
    [KeyedComponent(typeof(ICarrierSetup), ShipmentTypeCode.DhlExpress)]
    public class DhlCarrierSetup : BaseCarrierSetup<DhlExpressAccountEntity, IDhlExpressAccountEntity>, ICarrierSetup
    {
        private readonly ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity> accountRepository;
        private readonly IShipEngineWebClient webClient;
        private readonly ICarrierAccountDescription accountDescription;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public DhlCarrierSetup(IShipmentTypeSetupActivity shipmentTypeSetupActivity,
            IShippingSettings shippingSettings,
            IShipmentPrintHelper printHelper,
            ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity> accountRepository,
            IIndex<ShipmentTypeCode, ICarrierAccountDescription> accountDescriptionFactory,
            IShipEngineWebClient webClient)
            : base(shipmentTypeSetupActivity, shippingSettings, printHelper, accountRepository)
        {
            this.accountRepository = accountRepository;
            this.webClient = webClient;
            accountDescription = accountDescriptionFactory[ShipmentTypeCode.DhlExpress];
        }

        /// <summary>
        /// Setup a DHL account from data imported from the hub
        /// </summary>
        public async Task Setup(CarrierConfiguration config)
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
                GenericResult<string> connectAccountResult = await webClient.ConnectDhlAccount(additionalAccountInfo.AccountNumber);
                if (connectAccountResult.Success)
                {
                    dhlAccount.AccountNumber = long.Parse(additionalAccountInfo.AccountNumber);
                    dhlAccount.ShipEngineCarrierId = connectAccountResult.Value;
                    dhlAccount.Description = accountDescription.GetDefaultAccountDescription(dhlAccount);
                }
                else
                {
                    throw connectAccountResult.Exception;
                }
            }

            accountRepository.Save(dhlAccount);
            SetupDefaultsIfNeeded(ShipmentTypeCode.DhlExpress, config.RequestedLabelFormat);
        }

        /// <summary>
        /// Get or Create a DHL account
        /// </summary>
        private DhlExpressAccountEntity GetOrCreateAccountEntity(Guid carrierId) =>
            accountRepository.Accounts.FirstOrDefault(x => x.HubCarrierId == carrierId)
            ?? new DhlExpressAccountEntity { HubCarrierId = carrierId };
    }
}
