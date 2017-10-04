using System;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// An exception thrown in a ShipEngine related process
    /// </summary>
    public class ShipEngineException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipEngineException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipEngineException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
