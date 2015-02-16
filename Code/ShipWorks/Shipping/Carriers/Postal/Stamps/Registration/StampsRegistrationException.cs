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
    public class StampsRegistrationException : UspsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StampsRegistrationException"/> class.
        /// </summary>
        public StampsRegistrationException()
            : base()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StampsRegistrationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public StampsRegistrationException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StampsRegistrationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public StampsRegistrationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
