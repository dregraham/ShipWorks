using System;
using System.Collections.Generic;

namespace ShipWorks.AddressValidation
{
    public interface IAddressValidationWebClient
    {
        List<AddressValidationResult> ValidateAddress(string street1, string street2, string city, string state, String zip);
    }
}