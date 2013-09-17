using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Validates registration information prior to sending registration request to Express1.
    /// </summary>
    public interface IExpress1RegistrationValidator
    {
        /// <summary>
        /// Validates Registration information prior to sending registration request to Express1.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns>A List of Express1ValidationError objects containing all of the items that
        /// failed to pass validation. An empty list indicates the registration passed validation.</returns>
        List<Express1ValidationError> Validate(Express1Registration registration);
    }
}