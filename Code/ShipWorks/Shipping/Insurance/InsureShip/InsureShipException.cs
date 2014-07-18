using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// An InsureShip specific exception
    /// </summary>
    public class InsureShipException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InsureShipException()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InsureShipException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InsureShipException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
