using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Void
{
    /// <summary>
    /// Validates whether a policy can be voided
    /// </summary>
    public interface IInsureShipVoidValidator
    {
        /// <summary>
        /// Is the policy voidable
        /// </summary>
        GenericResult<bool> IsVoidable(IShipmentEntity shipment);
    }
}