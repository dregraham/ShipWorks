using Interapptive.Shared.Business;
using Interapptive.Shared.Enums;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Services;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Policy for determining if we should validate addresses using a given input
    /// </summary>
    public static class AddressValidationPolicy
    {
        /// <summary>
        /// Should the given status be validated
        /// </summary>
        public static bool ShouldValidate(AddressValidationStatusType status)
        {
            return status == AddressValidationStatusType.NotChecked ||
                   status == AddressValidationStatusType.Pending ||
                   status == AddressValidationStatusType.Error;
        }

        /// <summary>
        /// Should the given store setting validate
        /// </summary>
        public static bool ShouldAutoValidate(AddressValidationStoreSettingType storeSettingStatus)
        {
            return storeSettingStatus == AddressValidationStoreSettingType.ValidateAndApply ||
                storeSettingStatus == AddressValidationStoreSettingType.ValidateAndNotify;
        }

        /// <summary>
        /// Should the given store setting validate when the user manually chooses to
        /// </summary>
        private static bool ShouldManuallyValidate(AddressValidationStoreSettingType storeSettingStatus)
        {
            return storeSettingStatus != AddressValidationStoreSettingType.ValidationDisabled;
        }

        /// <summary>
        /// Check to see if the store and address can be validated
        /// </summary>
        private static bool ShouldValidate(StoreEntity store, AddressAdapter address)
        {
            if (store == null || address == null)
            {
                return false;
            }

            if (!ShouldValidate((AddressValidationStatusType) address.AddressValidationStatus))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Should addresses from the given store be auto validated 
        /// </summary>
        public static bool ShouldAutoValidate(StoreEntity store, AddressAdapter address)
        {
            if (ShouldValidate(store, address))
            {
                return address.IsDomesticCountry() ?
                    ShouldAutoValidate(store.DomesticAddressValidationSetting) :
                    ShouldAutoValidate(store.InternationalAddressValidationSetting);
            }

            return false;
        }

        /// <summary>
        /// Should addresses from the given store be validated 
        /// </summary>
        public static bool ShouldManuallyValidate(StoreEntity store, AddressAdapter address)
        {
            if (ShouldValidate(store, address))
            {
                return address.IsDomesticCountry() ?
                    ShouldManuallyValidate(store.DomesticAddressValidationSetting) :
                    ShouldManuallyValidate(store.InternationalAddressValidationSetting);
            }

            return false;
        }

        /// <summary>
        /// Is address validation enabled for the given store and shipment
        /// </summary>
        public static bool IsValidationEnabled(IStoreEntity store, ICarrierShipmentAdapter shipmentAdapter)
        {
            if (store == null || shipmentAdapter == null)
            {
                return false;
            }

            var validationSetting = shipmentAdapter.IsDomestic ?
                store.DomesticAddressValidationSetting :
                store.InternationalAddressValidationSetting;

            return validationSetting != AddressValidationStoreSettingType.ValidationDisabled;
        }
    }
}
