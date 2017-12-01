﻿using Interapptive.Shared.Business;
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
        /// Should addresses from the given store be validated 
        /// </summary>
        public static bool ShouldValidate(StoreEntity store)
        {
            if (store == null)
            {
                return false;
            }

            AddressValidationStoreSettingType setting = (AddressValidationStoreSettingType)store.DomesticAddressValidationSetting;
            return setting == AddressValidationStoreSettingType.ValidateAndApply ||
                   setting == AddressValidationStoreSettingType.ValidateAndNotify;
        }
        
        /// <summary>
        /// Should the specified address should be validated
        /// </summary>
        public static bool ShouldValidate(AddressAdapter currentShippingAddress)
        {
            if (string.IsNullOrEmpty(currentShippingAddress.CountryCode))
            {
                currentShippingAddress.AddressValidationError = "ShipWorks cannot validate an address without a country.";
                currentShippingAddress.AddressValidationStatus = (int)AddressValidationStatusType.BadAddress;
                currentShippingAddress.AddressType = (int)AddressType.WillNotValidate;

                return false;
            }

            if (string.IsNullOrEmpty(currentShippingAddress.Street1))
            {
                currentShippingAddress.AddressValidationError = "ShipWorks cannot validate an address without a first line.";
                currentShippingAddress.AddressValidationStatus = (int)AddressValidationStatusType.BadAddress;
                currentShippingAddress.AddressType = (int)AddressType.PrimaryNotFound;

                return false;
            }

            return true;
        }
    }
}
