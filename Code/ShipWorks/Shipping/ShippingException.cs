using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Thrown when a known error occurrs during shipping
    /// </summary>
    public class ShippingException : Exception
    {
        public ShippingException()
        {

        }

        public ShippingException(string message)
            : base(message)
        {

        }

        public ShippingException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
