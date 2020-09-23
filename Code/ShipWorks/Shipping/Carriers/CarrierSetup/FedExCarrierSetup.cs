using System;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.CarrierSetup;
using ShipWorks.Shipping.Settings;
using ShipWorks.Warehouse.Configuration.DTO.ShippingSettings;

namespace ShipWorks.Shipping.Carriers.CarrierSetup
{
    /// <summary>
    /// Setup the FedEx carrier configuration downloaded from the hub
    /// </summary>
    [KeyedComponent(typeof(ICarrierSetup), ShipmentTypeCode.FedEx)]
    public class FedExCarrierSetup : BaseCarrierSetup<FedExAccountEntity, IFedExAccountEntity>, ICarrierSetup
    {
        private readonly ICarrierAccountRepository<FedExAccountEntity, IFedExAccountEntity> fedExAccountRepository;
        private readonly IFedExShippingClerk shippingClerk;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExCarrierSetup(IShipmentTypeSetupActivity shipmentTypeSetupActivity,
            ICarrierAccountRepository<FedExAccountEntity, IFedExAccountEntity> fedExAccountRepository,
            IShippingSettings shippingSettings,
            IShipmentPrintHelper printHelper,
            IFedExShippingClerkFactory shippingClerkFactory) :
            base(shipmentTypeSetupActivity, shippingSettings, printHelper, fedExAccountRepository)
        {
            this.fedExAccountRepository = fedExAccountRepository;
            this.shippingClerk = shippingClerkFactory.Create();
        }

        /// <summary>
        /// Setup a FedEx account from data imported from the hub
        /// </summary>
        public void Setup(CarrierConfiguration config)
        {
            if (fedExAccountRepository.AccountsReadOnly.Any(x => x.HubCarrierId == config.HubCarrierID && x.HubVersion >= config.HubVersion))
            {
                return;
            }

            var account = config.AdditionalData["fedex"].ToObject<FedExAccountConfiguration>();

            var fedExAccount = GetOrCreateAccountEntity(config.HubCarrierID);

            fedExAccount.AccountNumber = account.AccountNumber;

            GetAddress(fedExAccount, config.Address).CopyTo(fedExAccount, string.Empty);

            if (fedExAccount.IsNew)
            {
                fedExAccount.SmartPostHubList = "<Root />";
                fedExAccount.SignatureRelease = string.Empty;
                fedExAccount.Description = FedExAccountManager.GetDefaultDescription(fedExAccount);
                shippingClerk.RegisterAccount(fedExAccount);
                fedExAccount.InitializeNullsToDefault();
            }

            fedExAccountRepository.Save(fedExAccount);

            SetupDefaultsIfNeeded(ShipmentTypeCode.FedEx, config.RequestedLabelFormat);
        }

        /// <summary>
        /// Get an existing account entity, or create a new one if none exist with the given account ID
        /// </summary>
        private FedExAccountEntity GetOrCreateAccountEntity(Guid carrierID) =>
            fedExAccountRepository.Accounts.FirstOrDefault(x => x.HubCarrierId == carrierID) ?? new FedExAccountEntity { HubCarrierId = carrierID };
    }
}
