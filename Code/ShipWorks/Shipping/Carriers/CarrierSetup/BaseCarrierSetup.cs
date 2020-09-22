using Interapptive.Shared.Collections;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Warehouse.Configuration.DTO;

namespace ShipWorks.Shipping.Carriers.CarrierSetup
{
    /// <summary>
    /// An abstract class for common carrier setup functionality
    /// </summary>
    public abstract class BaseCarrierSetup<TAccount, TAccountInterface>
        where TAccount : TAccountInterface
        where TAccountInterface : ICarrierAccount
    {
        protected bool isFirstAccountOfType;

        private readonly IShipmentTypeSetupActivity shipmentTypeSetupActivity;
        private readonly IShippingSettings shippingSettings;
        private readonly IShipmentPrintHelper printHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public BaseCarrierSetup(IShipmentTypeSetupActivity shipmentTypeSetupActivity,
            IShippingSettings shippingSettings,
            IShipmentPrintHelper printHelper,
            ICarrierAccountRepository<TAccount, TAccountInterface> accountRepository)
        {
            this.shipmentTypeSetupActivity = shipmentTypeSetupActivity;
            this.shippingSettings = shippingSettings;
            this.printHelper = printHelper;

            this.isFirstAccountOfType = accountRepository.AccountsReadOnly.None();
        }

        /// <summary>
        /// Initializes a shipment type and sets up default rules if this is the first account of that type
        /// </summary>
        protected void SetupDefaultsIfNeeded(ShipmentTypeCode shipmentTypeCode, ThermalLanguage requestedLabelFormat)
        {
            if (isFirstAccountOfType)
            {
                shipmentTypeSetupActivity.InitializeShipmentType(shipmentTypeCode, ShipmentOriginSource.Account, false, requestedLabelFormat);
                shippingSettings.MarkAsConfigured(shipmentTypeCode);
                printHelper.InstallDefaultRules(shipmentTypeCode);
            }
        }

        /// <summary>
        /// Update the account address with the one we get from the hub
        /// </summary>
        protected void UpdateAddress(ICarrierAccount account, ConfigurationAddress address)
        {
            account.Address.FirstName = string.IsNullOrEmpty(address.FirstName) ? account.Address.FirstName : address.FirstName;
            account.Address.MiddleName = string.IsNullOrEmpty(address.MiddleName) ? account.Address.MiddleName : address.MiddleName;
            account.Address.LastName = string.IsNullOrEmpty(address.LastName) ? account.Address.LastName : address.LastName;
            account.Address.Company = string.IsNullOrEmpty(address.Company) ? account.Address.Company : address.Company;
            account.Address.Street1 = string.IsNullOrEmpty(address.Street1) ? account.Address.Street1 : address.Street1;
            account.Address.Street2 = string.IsNullOrEmpty(address.Street2) ? account.Address.Street2 : address.Street2;
            account.Address.Street3 = string.IsNullOrEmpty(address.Street3) ? account.Address.Street3 : address.Street3;
            account.Address.City = string.IsNullOrEmpty(address.City) ? account.Address.City : address.City;
            account.Address.StateProvCode = string.IsNullOrEmpty(address.State) ? account.Address.StateProvCode : address.State;
            account.Address.PostalCode = string.IsNullOrEmpty(address.Zip) ? account.Address.PostalCode : address.Zip;
            account.Address.CountryCode = string.IsNullOrEmpty(address.Country) ? account.Address.CountryCode : address.Country;
            account.Address.Phone = string.IsNullOrEmpty(address.Phone) ? account.Address.Phone : address.Phone;
            account.Address.Email = string.IsNullOrEmpty(address.Email) ? account.Address.Email : address.Email;
        }
    }
}
