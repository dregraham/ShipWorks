using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.Postal.Usps;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Registration
{
    /// <summary>
    /// An exception that occus during registering an account with Stamps.com
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
