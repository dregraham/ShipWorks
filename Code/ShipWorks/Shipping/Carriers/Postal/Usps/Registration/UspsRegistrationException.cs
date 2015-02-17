using System;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration
{
    /// <summary>
    /// An exception that occus during registering an account with USPS 
    /// </summary>
    public class UspsRegistrationException : UspsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsRegistrationException"/> class.
        /// </summary>
        public UspsRegistrationException()
            : base()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsRegistrationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public UspsRegistrationException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsRegistrationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public UspsRegistrationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
