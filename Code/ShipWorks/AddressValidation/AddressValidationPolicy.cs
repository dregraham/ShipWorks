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
        public static bool ShouldValidate(AddressValidationStoreSettingType storeSettingStatus)
        {
            return storeSettingStatus == AddressValidationStoreSettingType.ValidateAndApply || 
                storeSettingStatus == AddressValidationStoreSettingType.ValidateAndNotify;
        }

        /// <summary>
        /// Should addresses from the given store be validated 
        /// </summary>
        public static bool ShouldValidate(StoreEntity store, AddressAdapter address)
        {
            if (store == null || address == null)
            {
                return false;
            }

            if (!ShouldValidate((AddressValidationStatusType) address.AddressValidationStatus))
            {
                return false;
            }
            
            return address.IsDomesticCountry() ? 
                ShouldValidate(store.DomesticAddressValidationSetting) : 
                ShouldValidate(store.InternationalAddressValidationSetting);
        }
    }
}
