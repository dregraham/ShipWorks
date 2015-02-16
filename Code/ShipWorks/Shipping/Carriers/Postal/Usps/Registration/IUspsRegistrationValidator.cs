using System.Collections.Generic;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration
{
    /// <summary>
    /// An interface for validating a stamps.com registration.
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
