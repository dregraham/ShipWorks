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
    }
}
