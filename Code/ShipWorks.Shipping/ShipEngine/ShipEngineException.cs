using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public ShipEngineException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipEngineException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
