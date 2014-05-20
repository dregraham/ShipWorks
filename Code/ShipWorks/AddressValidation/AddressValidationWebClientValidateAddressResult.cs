using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Holds the result of ValidateAddress
    /// </summary>
    public class AddressValidationWebClientValidateAddressResult
    {
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
