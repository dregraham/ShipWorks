using System;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Warehouse.Configuration.DTO;
using ShipWorks.Warehouse.Configuration.DTO.ShippingSettings;

namespace ShipWorks.Shipping.CarrierSetup
{
    /// <summary>
    /// Setup the USPS Carrier configuration downloaded from the Hub
    /// </summary>
    [KeyedComponent(typeof(ICarrierSetup), ShipmentTypeCode.Usps)]
    public class UspsCarrierSetup : ICarrierSetup
    {
        private readonly ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository;
        private readonly IShippingSettings shippingSettings;
        private readonly IShipmentPrintHelper printHelper;
        private readonly IUspsWebClient webClient;
        private readonly IShipmentTypeSetupActivity shipmentTypeSetupActivity;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsCarrierSetup(IShipmentTypeSetupActivity shipmentTypeSetupActivity,
            ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository,
            IShippingSettings shippingSettings,
            IShipmentPrintHelper printHelper,
            Func<UspsResellerType, IUspsWebClient> uspsWebClientFactory)
        {
            this.uspsAccountRepository = uspsAccountRepository;
            this.shippingSettings = shippingSettings;
            this.printHelper = printHelper;
            this.webClient = uspsWebClientFactory(UspsResellerType.None);
            this.shipmentTypeSetupActivity = shipmentTypeSetupActivity;
        }

        /// <summary>
        /// Creates a new USPS account from data imported from the hub
        /// </summary>
        public void Setup(CarrierConfiguration config)
        {
            var account = config.AdditionalData["usps"].ToObject<UspsAccountConfiguration>();

            if (uspsAccountRepository.AccountsReadOnly.Any(x => x.HubCarrierId == config.HubCarrierID && x.HubVersion >= config.HubVersion))
            {
                return;
            }

            bool isFirstAccount = uspsAccountRepository.AccountsReadOnly.None();

            UspsAccountEntity uspsAccount = GetOrCreateAccountEntity(config.HubCarrierID);

            uspsAccount.Username = account.Username;
            uspsAccount.Password = SecureText.Encrypt(account.Password, account.Username);

            if (uspsAccount.IsNew)
            {
                webClient.PopulateUspsAccountEntity(uspsAccount);

                uspsAccount.PendingInitialAccount = (int) UspsPendingAccountType.None;
                uspsAccount.ShipEngineCarrierId = config.ShipEngineCarrierID;
            }

            UpdateAddress(uspsAccount, config.Address);

            uspsAccount.HubVersion = config.HubVersion;
            uspsAccount.HubCarrierId = config.HubCarrierID;

            uspsAccountRepository.Save(uspsAccount);

            if (isFirstAccount)
            {
                shipmentTypeSetupActivity.InitializeShipmentType(ShipmentTypeCode.Usps, ShipmentOriginSource.Account, false, config.RequestedLabelFormat);
                shippingSettings.MarkAsConfigured(ShipmentTypeCode.Usps);
                printHelper.InstallDefaultRules(ShipmentTypeCode.Usps);
            }
        }

        /// <summary>
        /// Get an existing account entity, or create a new one if none exist with the given account ID
        /// </summary>
        private UspsAccountEntity GetOrCreateAccountEntity(Guid carrierID) =>
            uspsAccountRepository.Accounts.FirstOrDefault(x => x.HubCarrierId == carrierID) ?? new UspsAccountEntity { HubCarrierId = carrierID };

        /// <summary>
        /// Update the account address with the one we got from the config
        /// </summary>
        private void UpdateAddress(UspsAccountEntity account, ConfigurationAddress address)
        {
            account.FirstName = string.IsNullOrEmpty(address.FirstName) ? account.FirstName : address.FirstName;
            account.MiddleName = string.IsNullOrEmpty(address.MiddleName) ? account.MiddleName : address.MiddleName;
            account.LastName = string.IsNullOrEmpty(address.LastName) ? account.LastName : address.LastName;
            account.Company = string.IsNullOrEmpty(address.Company) ? account.Company : address.Company;
            account.Street1 = string.IsNullOrEmpty(address.Street1) ? account.Street1 : address.Street1;
            account.Street2 = string.IsNullOrEmpty(address.Street2) ? account.Street2 : address.Street2;
            account.Street3 = string.IsNullOrEmpty(address.Street3) ? account.Street3 : address.Street3;
            account.City = string.IsNullOrEmpty(address.City) ? account.City : address.City;
            account.StateProvCode = string.IsNullOrEmpty(address.State) ? account.StateProvCode : address.State;
            account.PostalCode = string.IsNullOrEmpty(address.Zip) ? account.PostalCode : address.Zip;
            account.CountryCode = string.IsNullOrEmpty(address.Country) ? account.CountryCode : address.Country;
            account.Phone = string.IsNullOrEmpty(address.Phone) ? account.Phone : address.Phone;
            account.Email = string.IsNullOrEmpty(address.Email) ? account.Email : address.Email;

        }
    }
}
