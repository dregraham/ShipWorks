﻿using Interapptive.Shared.Business;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Address validator that uses Stamps.com for lookups
    /// </summary>
    public class StampsAddressValidationWebClient : IAddressValidationWebClient
    {
        /// <summary>
        /// Validate the address
        /// </summary>
        public AddressValidationWebClientValidateAddressResult ValidateAddress(string street1, string street2, string city, string state, string zip, string countryCode)
        {
            PersonAdapter adapter = new PersonAdapter
            {
                FirstName = "Sample",
                LastName = "Name",
                Street1 = street1,
                Street2 = street2,
                City = city,
                StateProvCode = state,
                PostalCode = zip,
                CountryCode = countryCode
            };

            AddressValidationWebClientValidateAddressResult validationResult = new AddressValidationWebClientValidateAddressResult();

            StampsApiSession session = new StampsApiSession();

            try
            {
                StampsAddressValidationResults stampsResult = session.ValidateAddress(adapter);

                if (stampsResult.IsSuccessfulMatch)
                {
                    validationResult.AddressValidationResults.Add(CreateAddressValidationResult(stampsResult.MatchedAddress, true, stampsResult.IsPoBox, stampsResult.ResidentialIndicator));
                }
                else
                {
                    validationResult.AddressValidationError = stampsResult.IsCityStateZipOk ?
                        "City, State and ZIP Code are valid, but street address is not a match." :
                        "The address as submitted could not be found. Check for excessive abbreviations in the street address line or in the City name."; 
                }

                foreach (Address address in stampsResult.Candidates)
                {
                    validationResult.AddressValidationResults.Add(CreateAddressValidationResult(address, false, stampsResult.IsPoBox, stampsResult.ResidentialIndicator));
                }
            }
            catch (StampsException ex)
            {
                validationResult.AddressValidationError = ex.Message;
            }

            return validationResult;
        }

        /// <summary>
        /// Create an AddressValidationResult from a Stamps.com address
        /// </summary>
        private static AddressValidationResult CreateAddressValidationResult(Address address, bool isValid, bool? isPoBox, ResidentialDeliveryIndicatorType residentialStatus)
        {
            AddressValidationResult addressValidationResult = new AddressValidationResult
            {
                Street1 = address.Address1,
                Street2 = address.Address2 ?? string.Empty,
                Street3 = address.Address3 ?? string.Empty, 
                City = address.City, 
                StateProvCode = address.State, 
                PostalCode = GetPostalCode(address),
                CountryCode = address.Country,
                IsValid = isValid,
                POBox = ConvertPoBox(isPoBox),
                ResidentialStatus = ConvertResidentialStatus(residentialStatus)
            };

            addressValidationResult.ParseStreet1();
            addressValidationResult.ApplyAddressCasing();

            return addressValidationResult;
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
        /// Get a full postal code from an address
        /// </summary>
        private static string GetPostalCode(Address address)
        {
            if (!string.IsNullOrEmpty(address.ZIPCodeAddOn) && address.ZIPCodeAddOn != "0000")
            {
                return address.ZIPCode + "-" + address.ZIPCodeAddOn;
            }

            return address.ZIPCode;
        }
    }
}
