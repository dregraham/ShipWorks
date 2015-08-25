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
        AmazonValidateCredentialsResponse ValidateCredentials(string merchantId, string authToken);

        /// <summary>
        /// Gets the rates.
        /// </summary>
        GetEligibleShippingServices GetRates(ShipmentRequestDetails requestDetails, IAmazonMwsWebClientSettings mwsSettings);
    }
}
