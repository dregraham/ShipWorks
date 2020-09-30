﻿using System;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.CarrierSetup;
using ShipWorks.Shipping.Settings;
using ShipWorks.Warehouse.Configuration.DTO.ShippingSettings;

namespace ShipWorks.Shipping.Carriers.CarrierSetup
{
    /// <summary>
    /// Setup the UPS carrier configuration downloaded from the hub
    /// </summary>
    [KeyedComponent(typeof(ICarrierSetup), ShipmentTypeCode.UpsOnLineTools)]
    public class UpsCarrierSetup : BaseCarrierSetup<UpsAccountEntity, IUpsAccountEntity>, ICarrierSetup
    {
        private readonly ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> upsAccountRepository;
        private readonly IShippingSettings shippingSettings;
        private readonly IEncryptionProvider encryptionProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsCarrierSetup(IShipmentTypeSetupActivity shipmentTypeSetupActivity,
            ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> upsAccountRepository,
            IShippingSettings shippingSettings,
            IShipmentPrintHelper printHelper,
            IEncryptionProviderFactory encryptionProviderFactory) :
            base(shipmentTypeSetupActivity, shippingSettings, printHelper, upsAccountRepository)
        {
            this.upsAccountRepository = upsAccountRepository;
            this.shippingSettings = shippingSettings;
            this.encryptionProvider = encryptionProviderFactory.CreateSecureTextEncryptionProvider("UPS");
        }

        /// <summary>
        /// Setup a UPS account from data imported from the hub
        /// </summary>
        public void Setup(CarrierConfiguration config)
        {
            if (upsAccountRepository.AccountsReadOnly.Any(x => x.HubCarrierId == config.HubCarrierID && x.HubVersion >= config.HubVersion))
            {
                return;
            }

            var account = config.AdditionalData["ups"].ToObject<UpsAccountConfiguration>();

            var upsAccount = GetOrCreateAccountEntity(config.HubCarrierID);

            ShippingSettingsEntity settings = shippingSettings.Fetch();
            settings.UpsAccessKey = encryptionProvider.Encrypt(account.CustomerAccessNumber);
            shippingSettings.Save(settings);

            GetAddress(config.Address).CopyTo(upsAccount, string.Empty);

            if (upsAccount.IsNew)
            {
                upsAccount.AccountNumber = account.AccountNumber;
                upsAccount.InvoiceAuth = false;
                upsAccount.RateType = (int) account.RateType;
                upsAccount.PromoStatus = 0;
                upsAccount.LocalRatingEnabled = false;
                upsAccount.Description = UpsAccountManager.GetDefaultDescription(upsAccount);
                upsAccount.InitializeNullsToDefault();
            }

            upsAccountRepository.Save(upsAccount);

            SetupDefaultsIfNeeded(ShipmentTypeCode.UpsOnLineTools, config.RequestedLabelFormat);
        }

        /// <summary>
        /// Get an existing account entity, or create a new one if none exist with the given account ID
        /// </summary>
        private UpsAccountEntity GetOrCreateAccountEntity(Guid carrierID) =>
            upsAccountRepository.Accounts.FirstOrDefault(x => x.HubCarrierId == carrierID) ?? new UpsAccountEntity { HubCarrierId = carrierID };
    }
}