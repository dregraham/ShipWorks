using ShipWorks.AddressValidation.Enums;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using System;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Factory for Creating AddressValidationResult from Address
    /// </summary>
    public class StampsAddressValidationResultFactory : IAddressValidationResultFactory
    {
        /// <summary>
        /// Create the AddressValidationResult
        /// </summary>
        public AddressValidationResult CreateAddressValidationResult(Address address, bool isValid, UspsAddressValidationResults uspsResult, int addressType)
        {
            AddressValidationResult addressValidationResult = new AddressValidationResult
            {
                Street1 = address.Address1 ?? string.Empty,
                Street2 = address.Address2 ?? string.Empty,
                Street3 = address.Address3 ?? string.Empty,
                City = address.City ?? string.Empty,
                StateProvCode = GetStateProvCode(address),
                PostalCode = GetPostalCode(address) ?? string.Empty,
                CountryCode = address.Country ?? string.Empty,
                IsValid = isValid,
                POBox = ConvertPoBox(uspsResult.IsPoBox),
                ResidentialStatus = ConvertResidentialStatus(uspsResult.ResidentialIndicator),
                AddressType = addressType
            };

            addressValidationResult.ParseStreet1();
            addressValidationResult.ApplyAddressCasing();

            return addressValidationResult;
        }

        /// <summary>
        /// Get the State/Provice code
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private static string GetStateProvCode(Address address)
        {
            if (address.Country.Equals("GB", StringComparison.InvariantCultureIgnoreCase) ||
                address.Country.Equals("DE", StringComparison.InvariantCultureIgnoreCase))
            {
                return string.Empty;
            }

            return address.State ?? address.Province ?? string.Empty;
        }

        /// <summary>
        /// Get a full postal code from an address
        /// </summary>
        private static string GetPostalCode(Address address)
        {
            if (!string.IsNullOrEmpty(address.PostalCode))
            {
                return address.PostalCode;
            }

            if (!string.IsNullOrEmpty(address.ZIPCodeAddOn) && address.ZIPCodeAddOn != "0000")
            {
                return address.ZIPCode + "-" + address.ZIPCodeAddOn;
            }

            return address.ZIPCode;
        }
        
        /// <summary>
        /// Convert Stamps.com residential status into ShipWorks residential status
        /// </summary>
        private static ValidationDetailStatusType ConvertResidentialStatus(ResidentialDeliveryIndicatorType residentialStatus)
        {
            switch (residentialStatus)
            {
                case ResidentialDeliveryIndicatorType.No:
                    return ValidationDetailStatusType.No;
                case ResidentialDeliveryIndicatorType.Yes:
                    return ValidationDetailStatusType.Yes;
                default:
                    return ValidationDetailStatusType.Unknown;
            }
        }

        /// <summary>
        /// Converts the po box indicator into a ShipWorks ValidationDetailStatus
        /// </summary>
        private static ValidationDetailStatusType ConvertPoBox(bool? isPoBox)
        {
            if (!isPoBox.HasValue)
            {
                return ValidationDetailStatusType.Unknown;
            }

            return isPoBox.Value ? ValidationDetailStatusType.Yes : ValidationDetailStatusType.No;
        }
    }
}
