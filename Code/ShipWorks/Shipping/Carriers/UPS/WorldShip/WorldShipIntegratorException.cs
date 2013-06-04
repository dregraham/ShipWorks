using System;
using System.Runtime.Serialization;

namespace ShipWorks.Shipping.Carriers.UPS.WorldShip
{
    /// <summary>
    /// WorldShipIntegrator specific exception
    /// </summary>
    [Serializable]
    public class WorldShipIntegratorException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WorldShipIntegratorException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public WorldShipIntegratorException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public WorldShipIntegratorException(string message, Exception inner)
            : base(message, inner)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected WorldShipIntegratorException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {

        }
    }
}
