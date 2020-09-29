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
        protected PersonAdapter GetAddress(ConfigurationAddress address)
        {
            PersonAdapter newAddress = new PersonAdapter();
            newAddress.FirstName = address.FirstName;
            newAddress.MiddleName = address.MiddleName;
            newAddress.LastName = address.LastName;
            newAddress.Company = address.Company;
            newAddress.Street1 = address.Street1;
            newAddress.Street2 = address.Street2;
            newAddress.Street3 = address.Street3;
            newAddress.City = address.City;
            newAddress.StateProvCode = address.StateProvCode;
            newAddress.PostalCode = address.PostalCode;
            newAddress.CountryCode = address.CountryCode;
            newAddress.Phone = address.Phone;
            newAddress.Email = address.Email;
            return newAddress;
        }
    }
}
