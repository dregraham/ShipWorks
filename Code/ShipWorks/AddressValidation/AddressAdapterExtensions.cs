using Interapptive.Shared.Business;
using Interapptive.Shared.Enums;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Address Adapter extensions
    /// </summary>
    public static class AddressAdapterExtensions
    {
        /// <summary>
        /// Validate that the address is syntactically correct
        /// </summary>
        private static bool PreValidate(this AddressAdapter address)
        {        
            if (string.IsNullOrEmpty(address.CountryCode))
            {
                address.AddressValidationError = "ShipWorks cannot validate an address without a country.";
                address.AddressValidationStatus = (int)AddressValidationStatusType.BadAddress;
                address.AddressType = (int)AddressType.WillNotValidate;

                return false;
            }

            if (string.IsNullOrEmpty(address.Street1))
            {
                address.AddressValidationError = "ShipWorks cannot validate an address without a first line.";
                address.AddressValidationStatus = (int)AddressValidationStatusType.BadAddress;
                address.AddressType = (int)AddressType.PrimaryNotFound;

                return false;
            }

            return true;
        }

        /// <summary>
        /// Set the address validation status based on if the order has a country and first line
        /// </summary>
        public static void UpdateValidationStatus(this AddressAdapter address)
        {
            if (PreValidate(address))
            {
                address.AddressValidationStatus = (int)AddressValidationStatusType.NotChecked;
                address.AddressType = (int)AddressType.NotChecked;
            }
        }

        /// <summary>
        /// Set the address validation status based on the Store setting if the order has a country and first line
        /// </summary>
        public static void UpdateValidationStatus(this AddressAdapter address, StoreEntity store)
        {
            if (PreValidate(address))
            {
                AddressValidationStoreSettingType setting = address.IsDomesticCountry() ?
                    store.DomesticAddressValidationSetting :
                    store.InternationalAddressValidationSetting;

                if (setting == AddressValidationStoreSettingType.ValidateAndApply ||
                     setting == AddressValidationStoreSettingType.ValidateAndNotify)
                {
                    address.AddressValidationStatus = (int)AddressValidationStatusType.Pending;
                }
                else
                {
                    address.AddressValidationStatus = (int)AddressValidationStatusType.NotChecked;
                }
            }
        }
    }
}
