using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// Interface for an InsureShip Response Factory
    /// </summary>
    public interface IInsureShipResponseFactory
    {
        /// <summary>
        /// Creates the insure shipment response.
        /// </summary>
        void CreateInsureShipmentResponse(InsureShipRequestBase insureShipRequestBase);
    }
}
