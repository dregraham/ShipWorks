using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Shipping.Carriers.Amazon.Api
{
    /// <summary>
    /// Amazon shipping api client
    /// </summary>
    public interface IAmazonShippingWebClient
    {
        /// <summary>
        /// Validate the given credentials
        /// </summary>
        AmazonValidateCredentialsResponse ValidateCredentials(IAmazonMwsWebClientSettings mwsSettings);

        /// <summary>
        /// Gets the rates.
        /// </summary>
        GetEligibleShippingServicesResponse GetRates(ShipmentRequestDetails requestDetails, IAmazonMwsWebClientSettings mwsSettings);

        /// <summary>
        /// Create a shipment
        /// </summary>
        CreateShipmentResponse CreateShipment(ShipmentRequestDetails requestDetails, IAmazonMwsWebClientSettings mwsSettings, string shippingServiceId);

        /// <summary>
        /// Voids the shipment
        /// </summary>
        CancelShipmentResponse CancelShipment(IAmazonMwsWebClientSettings mwsSettings, string amazonShipmentId);

        /// <summary>
        /// Validates the CreateShipmentREsponse
        /// </summary>
        CreateShipmentResponse ValidateCreateShipmentResponse(CreateShipmentResponse createShipmentResponse);
    }
}
