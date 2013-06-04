using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.EquaShip
{
    /// <summary>
    /// Exception interacting with the EquaShip API
    /// </summary>
    [Serializable]
    public class EquaShipException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EquaShipException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EquaShipException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EquaShipException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

    }
}
