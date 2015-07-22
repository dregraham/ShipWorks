using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Fims
{
    /// <summary>
    /// Interface for working with a Fims Web Client
    /// </summary>
    public interface IFimsWebClient
    {
        /// <summary>
        /// Ships a FIMS shipment
        /// </summary>
        FimsShipResponse Ship(FimsShipRequest fimsShipRequest);
    }
}
