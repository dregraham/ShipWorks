using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public class CounterRatesOriginAddressException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CounterRatesOriginAddressException" /> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="message">The message.</param>
        public CounterRatesOriginAddressException(ShipmentEntity shipment, string message)
            : base(message)
        {
            Shipment = shipment;
        }

        /// <summary>
        /// Gets or sets the shipment.
        /// </summary>
        /// <value>The shipment.</value>
        protected ShipmentEntity Shipment
        {
            get; 
            set;
        }
    }
}
