using ShipWorks.AddressValidation.Enums;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Interface that represents an AddressValidationResult Factory
    /// </summary>
    public interface IAddressValidationResultFactory
    {
        /// <summary>
        /// Create an AddressValidationResult
        /// </summary>
        AddressValidationResult CreateAddressValidationResult(Address address, bool isValid, UspsAddressValidationResults uspsResult, int addressType);
    }
}