﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.OneBalance;
using ShipWorks.Shipping.Settings;
using ShipWorks.Warehouse.Configuration.Carriers.DTO;

namespace ShipWorks.Warehouse.Configuration.Carriers
{
    /// <summary>
    /// Setup the UPS carrier configuration downloaded from the hub
    /// </summary>
    [KeyedComponent(typeof(ICarrierSetup), ShipmentTypeCode.UpsOnLineTools)]
    public class UpsCarrierSetup : BaseCarrierSetup<UpsAccountEntity, IUpsAccountEntity>, ICarrierSetup
    {
        private readonly ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> upsAccountRepository;
        private readonly IShippingSettings shippingSettings;
        private readonly IMainForm mainForm;
        private readonly ILog log;
        private readonly IOneBalanceUpsAccountRegistrationActivity oneBalanceUpsAccountRegistrationActivity;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public UpsCarrierSetup(IShipmentTypeSetupActivity shipmentTypeSetupActivity,
            ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> upsAccountRepository,
            IShippingSettings shippingSettings,
            IShipmentPrintHelper printHelper,
            IMainForm mainForm,
            Func<Type, ILog> createLog,
            IOneBalanceUpsAccountRegistrationActivity oneBalanceUpsAccountRegistrationActivity) :
            base(shipmentTypeSetupActivity, shippingSettings, printHelper, upsAccountRepository)
        {
            this.upsAccountRepository = upsAccountRepository;
            this.shippingSettings = shippingSettings;
            this.mainForm = mainForm;
            this.oneBalanceUpsAccountRegistrationActivity = oneBalanceUpsAccountRegistrationActivity;
            log = createLog(typeof(UpsCarrierSetup));
        }

        /// <summary>
        /// Setup a UPS account from data imported from the hub
        /// </summary>
#pragma warning disable 1998
        public async Task Setup(CarrierConfiguration config, UspsAccountEntity oneBalanceUspsAccount)
#pragma warning restore 1998
        {
            // skip if we already know about this account
            if (upsAccountRepository.AccountsReadOnly.Any(x => x.HubCarrierId == config.HubCarrierID && x.HubVersion >= config.HubVersion))
            {
                return;
            }

            // skip if there is already a one balance account and this config is for onebalance
            if (config.IsOneBalance &&
                upsAccountRepository.AccountsReadOnly.Any(x => !string.IsNullOrWhiteSpace(x.ShipEngineCarrierId)))
            {
                return;
            }

            var account = config.AdditionalData["ups"].ToObject<UpsAccountConfiguration>();

            var upsAccount = GetOrCreateAccountEntity(config.HubCarrierID);

            ShippingSettingsEntity settings = shippingSettings.Fetch();

            if (!config.IsOneBalance)
            {
                settings.UpsAccessKey = SecureText.Encrypt(account.CustomerAccessNumber, "UPS");
                shippingSettings.Save(settings);
            }

            GetAddress(config.Address).CopyTo(upsAccount, string.Empty);
            upsAccount.HubVersion = config.HubVersion;

            // If the customer has a ShipEngineAccountId, that means they have a single ShipEngine account and we can
            // import this from the hub
            if (!string.IsNullOrWhiteSpace(settings.ShipEngineAccountID))
            {
                upsAccount.ShipEngineCarrierId = config.ShipEngineCarrierID;
            }

            if (upsAccount.IsNew)
            {
                if (!config.IsOneBalance)
                {
                    upsAccount.AccountNumber = account.AccountNumber;
                    upsAccount.InvoiceAuth = account.InvoiceAuth;
                    upsAccount.UserID = account.UserId;
                    upsAccount.Password = account.Password;
                    upsAccount.RateType = (int) account.RateType;
                }
                else
                {
                    // If we don't have a SE Carrier ID, that means the ups account was not linked to ShipEngine in the hub.
                    // So link it here
                    if (string.IsNullOrWhiteSpace(upsAccount.ShipEngineCarrierId))
                    {
                        string deviceIdentity = await GetDeviceIdentity().ConfigureAwait(false);
                        var result =
                            await oneBalanceUpsAccountRegistrationActivity.Execute(
                                oneBalanceUspsAccount, upsAccount, deviceIdentity).ConfigureAwait(false);
                        if (result.Failure)
                        {
                            // if we failed to register the ups account, we don't want to save it, so just bail. We'll try again next time.
                            log.Warn("Error registering UPS account", result.Exception);
                            return;
                        }
                    }
                }

                upsAccount.PromoStatus = 0;
                upsAccount.LocalRatingEnabled = false;
                upsAccount.Description = UpsAccountManager.GetDefaultDescription(upsAccount);
                upsAccount.InitializeNullsToDefault();
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