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

        /// <summary>
        /// Creates an instance of an IInsureShipResponse for submitting a claim to InsureShip.
        /// </summary>
        IInsureShipResponse CreateSubmitClaimResponse(InsureShipRequestBase insureShipRequestBase);

        /// <summary>
        /// Creates the void policy response.
        /// </summary>
        IInsureShipResponse CreateVoidPolicyResponse(InsureShipRequestBase insureShipRequestBase);
    }
}
