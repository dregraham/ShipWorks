using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Void
{
    /// <summary>
    /// InsureShip request class for voiding a policy
    /// </summary>
    public interface IInsureShipVoidPolicyRequest
    {
        /// <summary>
        /// Submits this request to InsureShip
        /// </summary>
        Result VoidInsurancePolicy(ShipmentEntity shipment);
    }
}