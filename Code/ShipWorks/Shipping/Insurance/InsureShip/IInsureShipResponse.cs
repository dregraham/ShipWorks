using ShipWorks.Shipping.Insurance.InsureShip.Enums;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// Interface for working with InsureShip responses
    /// </summary>
    public interface IInsureShipResponse
    {
        /// <summary>
        /// Process a response from InsureShip
        /// </summary>
        InsureShipResponseCode Process();
    }
}
