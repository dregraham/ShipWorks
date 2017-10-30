using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response
{
    /// <summary>
    /// An implementation of the ICarrierResponseFactory for FedEx.
    /// </summary>
    public interface IFedExResponseFactory : ICarrierResponseFactory
    {
        /// <summary>
        /// Creates an ICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request to ship an order/create a label.
        /// </summary>
        /// <param name="nativeResponse">The native response (WSDL object, raw XML, etc.) that is received from the carrier.</param>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>An ICarrierResponse representing the response of a shipment request.</returns>
        FedExShipResponse CreateShipResponse(object nativeResponse, CarrierRequest request, ShipmentEntity shipmentEntity);

        /// <summary>
        /// Creates theICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request to get Pickup Location if supported by API
        /// </summary>
        /// <param name="nativeResponse">The native response object received from the carrier.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>An ICarrierResponse representing the response of a Pickup Location request. </returns>
        ICarrierResponse CreateGlobalShipAddressResponse(object nativeResponse, CarrierRequest request);

        /// <summary>
        /// Creates an ICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request when performing the end of day ground close.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>An ICarrierResponse representing the response of a ground close request.</returns>
        ICarrierResponse CreateGroundCloseResponse(object nativeResponse, CarrierRequest request);

        /// <summary>
        /// Creates an ICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request when performing the end of day SmartPost close.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>An ICarrierResponse representing the response of a Smartpost close request.</returns>
        ICarrierResponse CreateSmartPostCloseResponse(object nativeResponse, CarrierRequest request);

        /// <summary>
        /// Creates an ICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request when performing the void.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>An ICarrierResponse representing the response of a void request.</returns>
        ICarrierResponse CreateVoidResponse(object nativeResponse, CarrierRequest request);

        /// <summary>
        /// Creates an ICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request for shipping rates.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>An ICarrierResponse representing the response of a void request.</returns>
        ICarrierResponse CreateRateResponse(object nativeResponse, CarrierRequest request);

        /// <summary>
        /// Creates an ICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request when performing the void.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>An ICarrierResponse representing the response of a void request.</returns>
        ICarrierResponse CreateTrackResponse(object nativeResponse, CarrierRequest request);
    }
}