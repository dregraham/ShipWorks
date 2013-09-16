using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Validates Registration information prior to sending registration request to Express1.
    /// </summary>
    public interface IExpress1RegistrationValidator
    {
        /// <summary>
        /// Validates Registration information prior to sending registration request to Express1.
        /// </summary>
        List<Express1ValidationError> Validate(Express1Registration registration);
    }
}