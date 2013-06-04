using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Registration
{
    /// <summary>
    /// An interface for validating a stamps.com registration.
    /// </summary>
    public interface IStampsRegistrationValidator
    {
        /// <summary>
        /// Validates the specified registration.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns>A List of RegistrationValidationError objects containing all of the items that
        /// failed to pass validation. An empty list indicates the registration passed validation.</returns>
        IEnumerable<RegistrationValidationError> Validate(StampsRegistration registration);
    }
}
