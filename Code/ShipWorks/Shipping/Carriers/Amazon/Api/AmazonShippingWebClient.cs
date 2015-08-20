using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Shipping.Carriers.Amazon.Api
{
    /// <summary>
    /// Amazon shipping api client
    /// </summary>
    public class AmazonShippingWebClient : IAmazonShippingWebClient
    {
        /// <summary>
        /// Validate the given credentials
        /// </summary>
        public AmazonValidateCredentialsResponse ValidateCredentials(string merchantId, string authToken)
        {
            // Create a fake store instance because the current
            // AmazonMwsClient requires a store to be passed
            // use US api 
            AmazonStoreEntity fakeStore = new AmazonStoreEntity() 
            {
                MerchantID = merchantId, 
                AuthToken = authToken,
                AmazonApiRegion = "US"
            };

            using (AmazonMwsClient client = new AmazonMwsClient(fakeStore))
            {
                try
                {
                    // Request a list of marketplaces to test credentials
                    client.GetMarketplaces();
                    return AmazonValidateCredentialsResponse.Succeeded();
                }
                catch (AmazonException ex)
                {
                    // Something must be wrong with the credentails 
                    return AmazonValidateCredentialsResponse.Failed(ex.Message);
                }
            }

        }

        /// <summary>
        /// Gets the rates.
        /// </summary>
        public GetEligibleShippingServices GetRates(AmazonAccountEntity account, AmazonShipmentEntity amazonShipment)
        {
            throw new System.NotImplementedException();
        }
    }
}