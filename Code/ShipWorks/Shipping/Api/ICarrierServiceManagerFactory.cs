using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.Shipping.Api
{
    /// <summary>
    /// A factory interface for creating ICarrierServiceManager objects.
    /// </summary>
    public interface ICarrierServiceManagerFactory
    {
        /// <summary>
        /// Creates the an ICarrierServiceManager appropriate for the given shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An ICarrierServiceManager object.</returns>
        IUpsServiceManager Create(ShipmentEntity shipment);
    }
}
