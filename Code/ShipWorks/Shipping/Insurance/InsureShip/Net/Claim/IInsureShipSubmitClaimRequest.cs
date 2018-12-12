using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Claim
{
    /// <summary>
    /// InsureShip request class for submitting a claim
    /// </summary>
    public interface IInsureShipSubmitClaimRequest
    {
        /// <summary>
        /// Submits this request to InsureShip
        /// </summary>
        Result CreateInsuranceClaim(ShipmentEntity shipment);
    }
}