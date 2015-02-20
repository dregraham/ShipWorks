using Interapptive.Shared.Business;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

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

            UspsWebClient session = new UspsWebClient(UspsResellerType.None);

            try
            {
                UspsAddressValidationResults uspsResult = session.ValidateAddress(adapter);

                if (uspsResult.IsSuccessfulMatch)
                {
                    validationResult.AddressValidationResults.Add(CreateAddressValidationResult(uspsResult.MatchedAddress, true, uspsResult.IsPoBox, uspsResult.ResidentialIndicator));
                }
                else
                {
                    validationResult.AddressValidationError = uspsResult.IsCityStateZipOk ?
                        "City, State and ZIP Code are valid, but street address is not a match." :
                        "The address as submitted could not be found. Check for excessive abbreviations in the street address line or in the City name."; 
                }

                foreach (Address address in uspsResult.Candidates)
                {
                    validationResult.AddressValidationResults.Add(CreateAddressValidationResult(address, false, uspsResult.IsPoBox, uspsResult.ResidentialIndicator));
                }
            }
            catch (UspsException ex)
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
