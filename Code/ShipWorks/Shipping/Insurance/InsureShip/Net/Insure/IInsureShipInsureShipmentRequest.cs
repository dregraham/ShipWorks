using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Insure
{
    /// <summary>
    /// InsureShip request class for insuring a shipment
    /// </summary>
    public interface IInsureShipInsureShipmentRequest
    {
        /// <summary>
        /// Submits this request to InsureShip
        /// </summary>
        Result CreateInsurancePolicy(ShipmentEntity shipment);
    }
}