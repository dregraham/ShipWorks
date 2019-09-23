﻿using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// Service for interacting with InsureShip
    /// </summary>
    public interface IInsureShipService
    {
        /// <summary>
        /// Insures the shipment with InsureShip and sets the InsuredWith property of the shipment based
        /// on the response from InsureShip.
        /// </summary>
        Result Insure(ShipmentEntity shipment);

        /// <summary>
        /// Void the policy for the given shipment
        /// </summary>
        Result Void(ShipmentEntity shipment);

        /// <summary>
        /// Is the given shipment insured by InsureShip
        /// </summary>
        bool IsInsuredByInsureShip(ShipmentEntity shipment);
    }
}