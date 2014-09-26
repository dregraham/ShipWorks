using System;
using System.Collections.Generic;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Interface for AddressValidationWebClient
    /// </summary>
    public interface IAddressValidationWebClient
    {
        /// <summary>
        /// Validates the address.
        /// </summary>
        AddressValidationWebClientValidateAddressResult ValidateAddress(string street1, string street2, string city, string state, String zip, string countryCode);
    }
}