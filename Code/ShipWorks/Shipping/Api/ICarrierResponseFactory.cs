namespace ShipWorks.Shipping.Carriers.Api
{
    /// <summary>
    /// An abstract factory interface for creating carrier-specific response objects.
    /// </summary>
    public interface ICarrierResponseFactory
    {
        /// <summary>
        /// Creates an ICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request when registering a new user.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>An ICarrierResponse representing the response of a void request.</returns>
        ICarrierResponse CreateRegisterUserResponse(object nativeResponse, CarrierRequest request);

        /// <summary>
        /// Creates an ICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request when subscribing a shipper to the Carrier using the carrier services.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>An ICarrierResponse representing the response of a void request.</returns>
        ICarrierResponse CreateSubscriptionResponse(object nativeResponse, CarrierRequest request);
    }
}
