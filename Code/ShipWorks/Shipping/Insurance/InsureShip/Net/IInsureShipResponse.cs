namespace ShipWorks.Shipping.Insurance.InsureShip.Net
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
