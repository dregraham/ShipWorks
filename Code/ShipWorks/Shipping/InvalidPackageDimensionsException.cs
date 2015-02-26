using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Thrown when a package has invalid dimensions.
    /// </summary>
    public class InvalidPackageDimensionsException : ShippingException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InvalidPackageDimensionsException()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InvalidPackageDimensionsException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InvalidPackageDimensionsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
