using System;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Base for all exceptions thrown by the Express1 Registration integration
    /// </summary>
    public class Express1RegistrationException : Exception
    {
        public Express1RegistrationException()
        {}

        public Express1RegistrationException(string message)
            : base(message)
        {}

        public Express1RegistrationException(string message, Exception innerException)
            : base(message, innerException)
        {}
    }
}