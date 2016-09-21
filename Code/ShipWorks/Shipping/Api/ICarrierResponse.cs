namespace ShipWorks.Shipping.Carriers.Api
{
    /// <summary>
    /// Interface for object that processes Carrier Responses
    /// </summary>
    public interface ICarrierResponse
    {
        /// <summary>
        /// Gets the request the was used to generate the response.
        /// </summary>
        /// <value>The CarrierRequest object.</value>
        CarrierRequest Request { get; }

        /// <summary>
        /// Gets the native response received from the carrier API.
        /// </summary>
        /// <value>The native response.</value>
        object NativeResponse { get; }

        /// <summary>
        /// Performs any processing required based on the response from the carrier.
        /// </summary>
        void Process();
    }
}
