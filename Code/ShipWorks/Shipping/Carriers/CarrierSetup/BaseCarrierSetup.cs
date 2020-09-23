using Interapptive.Shared.Business;
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
        /// Get the account address, using new values when possible
        /// </summary>
        protected PersonAdapter GetAddress(ICarrierAccount account, ConfigurationAddress address)
        {
            PersonAdapter newAddress = new PersonAdapter();
            newAddress.FirstName = string.IsNullOrEmpty(address.FirstName) ? account.Address.FirstName : address.FirstName;
            newAddress.MiddleName = string.IsNullOrEmpty(address.MiddleName) ? account.Address.MiddleName : address.MiddleName;
            newAddress.LastName = string.IsNullOrEmpty(address.LastName) ? account.Address.LastName : address.LastName;
            newAddress.Company = string.IsNullOrEmpty(address.Company) ? account.Address.Company : address.Company;
            newAddress.Street1 = string.IsNullOrEmpty(address.Street1) ? account.Address.Street1 : address.Street1;
            newAddress.Street2 = string.IsNullOrEmpty(address.Street2) ? account.Address.Street2 : address.Street2;
            newAddress.Street3 = string.IsNullOrEmpty(address.Street3) ? account.Address.Street3 : address.Street3;
            newAddress.City = string.IsNullOrEmpty(address.City) ? account.Address.City : address.City;
            newAddress.StateProvCode = string.IsNullOrEmpty(address.State) ? account.Address.StateProvCode : address.State;
            newAddress.PostalCode = string.IsNullOrEmpty(address.Zip) ? account.Address.PostalCode : address.Zip;
            newAddress.CountryCode = string.IsNullOrEmpty(address.Country) ? account.Address.CountryCode : address.Country;
            newAddress.Phone = string.IsNullOrEmpty(address.Phone) ? account.Address.Phone : address.Phone;
            newAddress.Email = string.IsNullOrEmpty(address.Email) ? account.Address.Email : address.Email;
            return newAddress;
        }
    }
}
