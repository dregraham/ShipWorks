using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Shipping.Carriers.Amazon.SFP.DTO;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Terms.DTO;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP.Terms
{
    /// <summary>
    /// Class for communicating with the Hub for Amazon Terms 
    /// </summary>
    [Component()]
    public class AmazonTermsWebClient : IAmazonTermsWebClient
    {
        private readonly IWarehouseRequestFactory requestFactory;
        private readonly IWarehouseRequestClient warehouseRequestClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonTermsWebClient(IWarehouseRequestClient warehouseRequestClient,
            IWarehouseRequestFactory requestFactory)
        {
            this.warehouseRequestClient = warehouseRequestClient;
            this.requestFactory = requestFactory;
        }

        /// <summary>
        /// Make a call to get the latest Amazon terms
        /// </summary>
        public async Task<AmazonTermsVersion> GetTerms()
        {
            var request = requestFactory.Create(WarehouseEndpoints.AmazonBuyShippingTerms, Method.GET, null);

            var response = await warehouseRequestClient.MakeRequest<AmazonGetTermsInfoResponse>(request, "AmazonGetTermsInfo").ConfigureAwait(true);

            var returnObject = new AmazonTermsVersion()
            {
                AvailableDate = response.AmazonTermsAvailableDate,
                DeadlineDate = response.AmazonTermsDeadlineDate,
                Url = response.AmazonTermsUrl,
                Version = response.AmazonTermsVersion
            };

            return returnObject;
        }

        /// <summary>
        /// Make a call to accept Amazon terms
        /// </summary>
        public async Task<bool> AcceptTerms(Version version)
        {
            var payload = new { amazonTermsVersion = version.ToString() };

            var request = requestFactory.Create(WarehouseEndpoints.AmazonBuyShippingTerms, Method.PUT, payload);

            var response = await warehouseRequestClient.MakeRequest(request, "AmazonGetTermsInfo").ConfigureAwait(true);

            return response.Success;
        }
    }
}
