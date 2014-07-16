using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// Implementation of an InsureShip Insure Shipment Request Factory
    /// </summary>
    public class InsureShipRequestFactory : IInsureShipRequestFactory
    {
        /// <summary>
        /// Creates the insure shipment request.
        /// </summary>
        public InsureShipRequestBase CreateInsureShipmentRequest(ShipmentEntity shipmentEntity, InsureShipAffiliate insureShipAffiliate)
        {
            throw new NotImplementedException();
        }
    }
}
