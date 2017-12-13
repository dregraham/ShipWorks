using Interapptive.Shared.Business;
using Interapptive.Shared.Enums;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private static bool ShouldManualValidate(AddressValidationStoreSettingType storeSettingStatus)
        {
            return storeSettingStatus != AddressValidationStoreSettingType.ValidationDisabled;
        }
        
        /// <summary>
        /// Check to see if the store and address can be validated
        /// </summary>
        private static bool ShouldValidate(StoreEntity store, AddressAdapter address, Func<bool> settingFunc)
        {
            if (store == null || address == null)
            {
                return false;
            }

            if (!ShouldValidate((AddressValidationStatusType)address.AddressValidationStatus))
            {
                return false;
            }

            return settingFunc();
        }

        /// <summary>
        /// Should addresses from the given store be auto Validated validated 
        /// </summary>
        public static bool ShouldAutoValidate(StoreEntity store, AddressAdapter address)
        {
            return ShouldValidate(store, address, () =>
            {
                return address.IsDomesticCountry() ?
                    ShouldAutoValidate(store.DomesticAddressValidationSetting) :
                    ShouldAutoValidate(store.InternationalAddressValidationSetting);
            });
        }

        /// <summary>
        /// Should addresses from the given store be validated 
        /// </summary>
        public static bool ShouldManualValidate(StoreEntity store, AddressAdapter address)
        {
            return ShouldValidate(store, address, () =>
            {
                return address.IsDomesticCountry() ?
                    ShouldManualValidate(store.DomesticAddressValidationSetting) :
                    ShouldManualValidate(store.InternationalAddressValidationSetting);
            });
        }
    }
}
