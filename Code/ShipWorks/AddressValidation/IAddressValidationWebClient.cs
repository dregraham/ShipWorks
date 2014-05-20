using System;
using System.Collections.Generic;

namespace ShipWorks.AddressValidation
{
    public interface IAddressValidationWebClient
    {
        AddressValidationWebClientValidateAddressResult ValidateAddress(string street1, string street2, string city, string state, String zip);
    }
}