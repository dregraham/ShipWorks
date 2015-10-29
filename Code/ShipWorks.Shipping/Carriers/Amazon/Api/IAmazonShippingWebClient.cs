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
        AmazonValidateCredentialsResponse ValidateCredentials(AmazonMwsWebClientSettings mwsSettings);

        /// <summary>
        /// Gets the rates.
        /// </summary>
        GetEligibleShippingServicesResponse GetRates(ShipmentRequestDetails requestDetails, AmazonMwsWebClientSettings mwsSettings);

        /// <summary>
        /// Create a shipment
        /// </summary>
        CreateShipmentResponse CreateShipment(ShipmentRequestDetails requestDetails, AmazonMwsWebClientSettings mwsSettings, string shippingServiceId);

        /// <summary>
        /// Voids the shipment
        /// </summary>
        CancelShipmentResponse CancelShipment(AmazonMwsWebClientSettings mwsSettings, string amazonShipmentId);

        /// <summary>
        /// Validates the CreateShipmentREsponse
        /// </summary>
        CreateShipmentResponse ValidateCreateShipmentResponse(CreateShipmentResponse createShipmentResponse);
    }
}
