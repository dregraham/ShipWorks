namespace ShipWorks.Shipping.Insurance.InsureShip.Net
{
    /// <summary>
    /// Interface for an InsureShip Response Factory
    /// </summary>
    public interface IInsureShipResponseFactory
    {
        /// <summary>
        /// Creates the insure shipment response.
        /// </summary>
        IInsureShipResponse CreateInsureShipmentResponse(InsureShipRequestBase insureShipRequestBase);
    }
}
