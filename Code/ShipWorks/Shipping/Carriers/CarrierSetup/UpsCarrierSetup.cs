﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.OneBalance;
using ShipWorks.Shipping.CarrierSetup;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.ShipEngine;
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
        private readonly IShipEngineWebClient shipEngineWebClient;
        private readonly IMainForm mainForm;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public UpsCarrierSetup(IShipmentTypeSetupActivity shipmentTypeSetupActivity,
            ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> upsAccountRepository,
            IShippingSettings shippingSettings,
            IShipmentPrintHelper printHelper,
            IShipEngineWebClient shipEngineWebClient,
            IMainForm mainForm) :
            base(shipmentTypeSetupActivity, shippingSettings, printHelper, upsAccountRepository)
        {
            this.upsAccountRepository = upsAccountRepository;
            this.shippingSettings = shippingSettings;
            this.shipEngineWebClient = shipEngineWebClient;
            this.mainForm = mainForm;
        }

        /// <summary>
        /// Setup a UPS account from data imported from the hub
        /// </summary>
#pragma warning disable 1998
        public async Task Setup(CarrierConfiguration config, IUspsAccountEntity oneBalanceUspsAccount)
#pragma warning restore 1998
        {
            if (upsAccountRepository.AccountsReadOnly.Any(x => x.HubCarrierId == config.HubCarrierID && x.HubVersion >= config.HubVersion))
            {
                return;
            }

            var account = config.AdditionalData["ups"].ToObject<UpsAccountConfiguration>();

            var upsAccount = GetOrCreateAccountEntity(config.HubCarrierID);

            if (!config.IsOneBalance)
            {
                ShippingSettingsEntity settings = shippingSettings.Fetch();
                settings.UpsAccessKey = SecureText.Encrypt(account.CustomerAccessNumber, "UPS");
                shippingSettings.Save(settings);
            }

            GetAddress(config.Address).CopyTo(upsAccount, string.Empty);
            upsAccount.HubVersion = config.HubVersion;

            if (upsAccount.IsNew)
            {
                if(!config.IsOneBalance)
                {
                    upsAccount.AccountNumber = account.AccountNumber;
                    upsAccount.InvoiceAuth = account.InvoiceAuth;
                    upsAccount.UserID = account.UserId;
                    upsAccount.Password = account.Password;
                    upsAccount.RateType = (int) account.RateType;
                    upsAccount.PromoStatus = 0;
                    upsAccount.LocalRatingEnabled = false;

                    upsAccount.Description = UpsAccountManager.GetDefaultDescription(upsAccount);
                    upsAccount.InitializeNullsToDefault();
                }
                else
                {
                    upsAccount.Description = UpsAccountManager.GetDefaultDescription(upsAccount);
                    upsAccount.InitializeNullsToDefault();
                    var result = await shipEngineWebClient.RegisterUpsAccount(upsAccount.Address, await GetDeviceIdentity().ConfigureAwait(false)).ConfigureAwait(false);
                    if (result.Success)
                    {
                        upsAccount.ShipEngineCarrierId = result.Value;
                        return;
                    }
                }                
            }

            upsAccountRepository.Save(upsAccount);

            SetupDefaultsIfNeeded(ShipmentTypeCode.UpsOnLineTools, config.RequestedLabelFormat);
        }

        /// <summary>
        /// Get the device identity
        /// </summary>
        private async Task<string> GetDeviceIdentity()
        {
            for (int i = 0; i < 5; i++)
            {
                if (string.IsNullOrEmpty(mainForm.DeviceIdentity))
                {
                    await Task.Delay(2000);
                }
                else
                {
                    return mainForm.DeviceIdentity;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Get an existing account entity, or create a new one if none exist with the given account ID
        /// </summary>
        private UpsAccountEntity GetOrCreateAccountEntity(Guid carrierID) =>
            upsAccountRepository.Accounts.FirstOrDefault(x => x.HubCarrierId == carrierID) ?? new UpsAccountEntity { HubCarrierId = carrierID };
    }
}