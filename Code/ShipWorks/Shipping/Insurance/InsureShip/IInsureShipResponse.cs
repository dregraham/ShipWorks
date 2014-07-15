using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// Interface for working with InsureShip responses
    /// </summary>
    public interface IInsureShipResponse
    {
        /// <summary>
        /// Process a response from InsureShip
        /// </summary>
        void Process();
    }
}
