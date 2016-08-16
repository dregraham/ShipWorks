using System.Collections.Generic;
using ShipWorks.AddressValidation.Enums;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Holds the result of ValidateAddress
    /// </summary>
    public class AddressValidationWebClientValidateAddressResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AddressValidationWebClientValidateAddressResult()
        {
            AddressValidationResults = new List<AddressValidationResult>();
            AddressValidationError = string.Empty;
        }

        /// <summary>
        /// Gets or sets the address validation results.
        /// </summary>
        public List<AddressValidationResult> AddressValidationResults { get; set; }

        /// <summary>
        /// Gets or sets the address validation error.
        /// </summary>
        public string AddressValidationError { get; set; }

        /// <summary>
        /// Gets or sets the type of the address.
        /// </summary>
        public AddressType AddressType { get; set; }
    }
}
