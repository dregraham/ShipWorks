using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration
{
    /// <summary>
    /// An interface for validating a USPS registration.
    /// </summary>
    public interface IUspsRegistrationValidator
    {
        /// <summary>
        /// Validates the specified registration.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns>A List of RegistrationValidationError objects containing all of the items that
        /// failed to pass validation. An empty list indicates the registration passed validation.</returns>
        IEnumerable<RegistrationValidationError> Validate(UspsRegistration registration);
    }
}
