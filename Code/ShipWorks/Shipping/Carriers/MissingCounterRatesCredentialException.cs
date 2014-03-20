using System;

namespace ShipWorks.Shipping.Carriers
{
    public class MissingCounterRatesCredentialException : ShippingException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingCounterRatesCredentialException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MissingCounterRatesCredentialException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingCounterRatesCredentialException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public MissingCounterRatesCredentialException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
