using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.ApplicationCore.Licensing.WebClientEnvironments;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Login to the warehouse
    /// </summary>
    [Component]
    public class WarehouseList : IWarehouseList
    {
        private readonly WebClientEnvironment webClientEnvironment;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseList(WebClientEnvironmentFactory webClientEnvironmentFactory)
        {
            webClientEnvironment = webClientEnvironmentFactory.SelectedEnvironment;
        }

        /// <summary>
        /// Get list of warehouses
        /// </summary>
        public GenericResult<WarehouseListDto> GetList(TokenResponse tokenResponse)
        {
            try
            {
                if (tokenResponse?.token.IsNullOrWhiteSpace() == true)
                {
                    return GenericResult.FromError<WarehouseListDto>("Unable to obtain a valid token to authenticate request.");
                }

                RestRequest restRequest = new RestRequest("api/warehouses", Method.GET);
                restRequest.RequestFormat = DataFormat.Json;
                restRequest.AddHeader("Authorization", $"Bearer {tokenResponse.token}");

                var restClient = new RestClient(webClientEnvironment.WarehouseUrl)
                {
                    //Authenticator = authenticatorFactory.Create(store)
                };

                IRestResponse restResponse = restClient.Execute(restRequest);

                // De-serialize the result
                WarehouseListDto requestResult = JsonConvert.DeserializeObject<WarehouseListDto>(restResponse.Content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                        NullValueHandling = NullValueHandling.Ignore
                    });

                return requestResult;
            }
            catch (Exception e)
            {
                return GenericResult.FromError<WarehouseListDto>(e.Message);
            }
        }
    }
}
