using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    public class FedExAddressValidationException : ShippingException
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExAddressValidationException"/> class.
        /// </summary>
        public FedExAddressValidationException(string message, Exception ex) : base(message, ex)
        {}
    }
}
