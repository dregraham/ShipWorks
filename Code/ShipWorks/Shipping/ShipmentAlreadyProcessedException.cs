using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Thrown when a shipment has already been processed and was tried to process the
    /// </summary>
    class ShipmentAlreadyProcessedException : ShippingException
    {
        public ShipmentAlreadyProcessedException()
        {

        }

        public ShipmentAlreadyProcessedException(string message)
            : base(message)
        {

        }

        public ShipmentAlreadyProcessedException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
