using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.CarrierSetup;
using ShipWorks.Shipping.Settings;
using ShipWorks.Warehouse.Configuration.DTO;
using ShipWorks.Warehouse.Configuration.DTO.ShippingSettings;

namespace ShipWorks.Shipping.Carriers.CarrierSetup
{
    /// <summary>
    /// Setup the UPS carrier configuration downloaded from the hub
    /// </summary>
    [KeyedComponent(typeof(ICarrierSetup), ShipmentTypeCode.UpsOnLineTools)]
    public class UpsCarrierSetup
    {
        private readonly ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> upsAccountRepository;
        private readonly IShippingSettings shippingSettings;
        private readonly IShipmentPrintHelper printHelper;
        private readonly IShipmentTypeSetupActivity shipmentTypeSetupActivity;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsCarrierSetup(IShipmentTypeSetupActivity shipmentTypeSetupActivity,
            ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> upsAccountRepository,
            IShippingSettings shippingSettings,
            IShipmentPrintHelper printHelper)
        {
            this.shipmentTypeSetupActivity = shipmentTypeSetupActivity;
            this.upsAccountRepository = upsAccountRepository;
            this.shippingSettings = shippingSettings;
            this.printHelper = printHelper;
        }

        /// <summary>
        /// Setup a UPS account from data imported from the hub
        /// </summary>
        public void Setup(CarrierConfiguration config)
        {
            var account = config.AdditionalData["ups"].ToObject<UpsAccountConfiguration>();

        }

        /// <summary>
        /// Update the account address with the one we get from the hub
        /// </summary>
        private void UpdateAddress(UpsAccountEntity account, ConfigurationAddress address)
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