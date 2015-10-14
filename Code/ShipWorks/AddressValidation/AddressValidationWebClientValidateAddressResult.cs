using System.Collections.Generic;

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
    }
}
