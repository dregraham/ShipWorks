using System;

namespace ShipWorks.Stores.Platforms.Platform
{
    /// <summary>
    /// API Store exception
    /// </summary>
    public class PlatformStoreException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PlatformStoreException(string message, Exception ex) : base(message, ex)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PlatformStoreException(string message) : base(message)
        {
        }
    }
}
