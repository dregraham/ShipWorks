using System;

namespace ShipWorks.Shipping.ShipSense
{
    public class ShipSenseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShipSenseException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ShipSenseException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipSenseException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="exception">Exception to be made as the Inner Exception</param>
        public ShipSenseException(string message, Exception exception)
            : base(message, exception)
        { }
    }
}
