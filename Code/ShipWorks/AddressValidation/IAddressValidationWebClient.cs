using System;
using System.Collections.Generic;
using Interapptive.Shared.Business;

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
        AddressValidationWebClientValidateAddressResult ValidateAddress(AddressAdapter addressAdapter);
    }
}