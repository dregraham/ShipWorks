using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Claim
{
    /// <summary>
    /// InsureShip request class for checking the status of a claim
    /// </summary>
    public interface IInsureShipClaimStatusRequest
    {
        /// <summary>
        /// Submits this request to InsureShip
        /// </summary>
        GenericResult<string> GetClaimStatus(IShipmentEntity shipment);
    }
}