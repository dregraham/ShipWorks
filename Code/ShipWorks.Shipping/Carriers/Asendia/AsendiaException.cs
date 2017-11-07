using System;

namespace ShipWorks.Shipping.Carriers.Asendia
{
    /// <summary>
    /// Base exception for all exceptions thrown by the Asendia integration.
    /// </summary>
    public class AsendiaException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DhlExpressException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AsendiaException(string message) : base(message)
        {
        }
    }
}