using System;

namespace ShipWorks.Shipping.Carriers.Amazon.SWA
{
    /// <summary>
    /// Base exception for all exceptions thrown by the AmazonSWA integration.
    /// </summary>
    public class AmazonSWAException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonSWAException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AmazonSWAException(string message) : base(message)
        {
        }
    }
}