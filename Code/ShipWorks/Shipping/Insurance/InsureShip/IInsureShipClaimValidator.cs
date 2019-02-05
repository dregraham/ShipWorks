using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// Validator for making an InsureShip claim
    /// </summary>
    public interface IInsureShipClaimValidator
    {
        /// <summary>
        /// Determines whether a shipment is eligible for a claim to be submitted.
        /// </summary>
        Result IsShipmentEligibleToSubmitClaim(InsureShipClaimType claimType, IShipmentEntity shipment);
    }
}