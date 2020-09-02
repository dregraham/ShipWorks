using System;
using System.Net;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Warehouse.DTO.Configuration;

namespace ShipWorks.Warehouse
{
    /// <summary>
    /// Web client for downloading configuration settings from the Hub
    /// </summary>
    [Component]
    public class HubConfigurationWebClient : IHubConfigurationWebClient
    {
        private readonly IWarehouseRequestClient warehouseRequestClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubConfigurationWebClient(IWarehouseRequestClient warehouseRequestClient)
        {
            this.warehouseRequestClient = warehouseRequestClient;
        }

        /// <summary>
        /// Get the configuration from Hub
        /// </summary>
        public async Task<HubConfiguration> GetConfig(string warehouseID)
        {
            try
            {
                IRestRequest request = new RestRequest(WarehouseEndpoints.GetConfig, Method.GET);
                request.AddQueryParameter("warehouse", warehouseID);

                GenericResult<IRestResponse> response = await warehouseRequestClient
                    .MakeRequest(request, "Get Config")
                    .ConfigureAwait(true);

                if (response.Failure)
                {
                    throw new WebException(response.Message, response.Exception);
                }

                var configuration = JsonConvert.DeserializeObject<HubConfiguration>(
                    response.Value.Content,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new CamelCaseNamingStrategy
                            {
                                OverrideSpecifiedNames = false
                            }
                        },
                    });

                return configuration;
            }
            catch (Exception ex)
            {
                throw new WebException($"An error occurred downloading the configuration for warehouse ID {warehouseID}", ex);
            }
        }
    }
}
