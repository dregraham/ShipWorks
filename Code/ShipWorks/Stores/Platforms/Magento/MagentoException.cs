using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// An exception that gets thrown when something goes wrong with Magento
    /// </summary>
    public class MagentoException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoException(string message):base(message)
        {}

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoException(Exception innerException) : base(innerException.Message, innerException)
        { }
    }
}
