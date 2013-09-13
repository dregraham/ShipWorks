using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Helper methods for Express1
    /// </summary>
    public static class Express1Utilities
    {
        /// <summary>
        /// Simple validation that just checks that the string is not empty.
        /// </summary>
        /// <param name="registrationField">The value to check.</param>
        /// <param name="validationMessage">The validation error message.</param>
        /// <returns>A List of Express1ValidationError objects.</returns>
        public static IEnumerable<Express1ValidationError> ValidateDataIsProvided(string registrationField, string validationMessage)
        {
            List<Express1ValidationError> errors = new List<Express1ValidationError>();

            if (string.IsNullOrWhiteSpace(registrationField))
            {
                errors.Add(new Express1ValidationError(validationMessage));
            }

            return errors;
        }
    }
}
