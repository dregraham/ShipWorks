using System;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// A class encapsulating the logic to submit a claim to InsureShip via their API.
    /// </summary>
    public interface IInsureShipClaim
    {
        /// <summary>
        /// Checks the status of this claim by issuing a request to InsureShip.
        /// </summary>
        GenericResult<string> CheckStatus(IShipmentEntity shipment);

        /// <summary>
        /// Submits the claim to InsureShip and updates the shipment entity provided to the
        /// constructor with he updated claim data.
        /// </summary>
        Result Submit(InsureShipClaimType claimType, ShipmentEntity shipment, Action<InsurancePolicyEntity> updatePolicy);
    }
}