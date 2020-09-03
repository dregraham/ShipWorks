using System;
using System.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Warehouse.DTO.Configuration;
using ShipWorks.Warehouse.DTO.Configuration.ShippingSettings;

namespace ShipWorks.Shipping.CarrierSetup
{
    [KeyedComponent(typeof(ICarrierSetup), ShipmentTypeCode.Usps)]
    public class UspsCarrierSetup : ICarrierSetup
    {
        private readonly ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository;
        private readonly IShippingSettings shippingSettings;
        private readonly IShipmentTypeSetupActivity shipmentTypeSetupActivity;

        public UspsCarrierSetup(IShipmentTypeSetupActivity shipmentTypeSetupActivity,
            ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository,
            IShippingSettings shippingSettings)
        {
            this.uspsAccountRepository = uspsAccountRepository;
            this.shippingSettings = shippingSettings;
            this.shipmentTypeSetupActivity = shipmentTypeSetupActivity;
        }

        /// <summary>
        /// Creates a new usps account from data imported from the hub
        /// </summary>
        public void Setup(CarrierConfigurationPayload config)
        {
            var account = config.AdditionalData["account"].ToObject<UspsConfigurationAccount>();

            if (uspsAccountRepository.AccountsReadOnly.Any(x => x.UspsAccountID == account.AccountId && x.HubVersion >= config.HubVersion))
            {
                return;
            }

            bool shouldMarkAsConfigured = uspsAccountRepository.AccountsReadOnly.None();

            UspsAccountEntity uspsAccount = GetOrCreateAccountEntity(account.AccountId);

            uspsAccount.Username = account.Username;
            uspsAccount.Password = SecureText.Encrypt(account.Password, account.Username);
            uspsAccount.PendingInitialAccount = (int) UspsPendingAccountType.Existing;
            uspsAccount.CreatedDate = DateTime.UtcNow;

            uspsAccount.Description = UspsAccountManager.GetDefaultDescription(uspsAccount) ?? string.Empty;

            ConfigurationAddress accountAddress = config.Address;
            PersonName name = PersonName.Parse(accountAddress.FullName);

            uspsAccount.FirstName = name.First;
            uspsAccount.MiddleName = name.Middle;
            uspsAccount.LastName = name.Last;
            uspsAccount.Company = accountAddress.Company ?? string.Empty;
            uspsAccount.Street1 = accountAddress.Street1 ?? string.Empty;
            uspsAccount.Street2 = accountAddress.Street2 ?? string.Empty;
            uspsAccount.City = accountAddress.City ?? string.Empty;
            uspsAccount.StateProvCode = Geography.GetStateProvCode(accountAddress.State) ?? string.Empty;

            uspsAccount.PostalCode = accountAddress.Zip ?? string.Empty;
            uspsAccount.MailingPostalCode = accountAddress.Zip ?? string.Empty;

            uspsAccount.CountryCode = Geography.GetCountryCode(accountAddress.Country) ?? string.Empty;
            uspsAccount.Phone = accountAddress.Phone ?? string.Empty;
            uspsAccount.Email = account.Email ?? string.Empty;
            uspsAccount.Website = string.Empty;
            uspsAccount.UspsReseller = (int) UspsResellerType.None;
            uspsAccount.ContractType = account.ContractType;
            uspsAccount.CreatedDate = DateTime.UtcNow;
            uspsAccount.PendingInitialAccount = (int) UspsPendingAccountType.Existing;
            uspsAccount.GlobalPostAvailability = account.GlobalPost;

            uspsAccount.InitializeNullsToDefault();
            uspsAccountRepository.Save(uspsAccount);

            shipmentTypeSetupActivity.InitializeShipmentType(ShipmentTypeCode.Usps, ShipmentOriginSource.Account);

            if (shouldMarkAsConfigured)
            {
                shippingSettings.MarkAsConfigured(ShipmentTypeCode.Usps);
            }
        }

        /// <summary>
        /// Get an existing account entity, or create a new one if none exist with the given account ID
        /// </summary>
        private UspsAccountEntity GetOrCreateAccountEntity(long accountID) =>
            uspsAccountRepository.GetAccount(accountID) ?? new UspsAccountEntity { UspsAccountID = accountID };
    }
}
