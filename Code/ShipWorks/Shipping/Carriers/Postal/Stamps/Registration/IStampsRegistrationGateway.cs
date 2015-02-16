using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Registration
{
    /// <summary>
    /// An interface for registering an account with Stamps.com.
    /// </summary>
    public interface IStampsRegistrationGateway
    {
        /// <summary>
        /// Registers an account with Stamps.com.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns>A StampsRegistrationResult object.</returns>
        UspsRegistrationResult Register(StampsRegistration registration);
    }
}
