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
        public AddressValidationWebClientValidateAddressResult ValidateAddress(AddressAdapter addressAdapter)
        {
            // The underlying web client expects a PersonAdapter, so convert the address adapter to a person adapter
            PersonAdapter personAdapter = new PersonAdapter()
            {
                FirstName = "Sample",
                LastName = "Name",
                Street1 = addressAdapter.Street1,
                Street2 = addressAdapter.Street2,
                City = addressAdapter.City,
                StateProvCode = addressAdapter.StateProvCode,
                PostalCode = addressAdapter.PostalCode,
                CountryCode = addressAdapter.CountryCode
            };

            AddressValidationWebClientValidateAddressResult validationResult = new AddressValidationWebClientValidateAddressResult();

            UspsWebClient session = new UspsWebClient(UspsResellerType.None);

            try
            {
                UspsAddressValidationResults uspsResult = session.ValidateAddress(personAdapter);

                if (uspsResult.IsSuccessfulMatch)
                {
                    validationResult.AddressValidationResults.Add(CreateAddressValidationResult(uspsResult.MatchedAddress, true, uspsResult.IsPoBox, uspsResult.ResidentialIndicator));
                }
                else
                {
                    validationResult.AddressValidationError = uspsResult.BadAddressMessage;
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
                Street1 = address.Address1 ?? string.Empty,
                Street2 = address.Address2 ?? string.Empty,
                Street3 = address.Address3 ?? string.Empty,
                City = address.City ?? string.Empty,
                StateProvCode = address.State ?? string.Empty,
                PostalCode = GetPostalCode(address) ?? string.Empty,
                CountryCode = address.Country ?? string.Empty,
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
