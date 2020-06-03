using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using Newtonsoft.Json.Linq;
using RestSharp;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Common.Net;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Create warehouse
    /// </summary>
    [Component]
    public class WarehouseCreate : IWarehouseCreate
    {
        private readonly IWarehouseRequestClient warehouseRequestClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseCreate(IWarehouseRequestClient warehouseRequestClient)
        {
            this.warehouseRequestClient = warehouseRequestClient;
        }

        /// <summary>
        /// Creates a warehouse on the hub and returns the id of the new warehouse
        /// </summary>
        public async Task<GenericResult<string>> Create(Details warehouse)
        {
            try
            {
                IRestRequest request = new RestRequest(WarehouseEndpoints.Warehouses, Method.POST);
                request.JsonSerializer = new RestSharpJsonNetSerializer();
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(new ShipWorks.ApplicationCore.Licensing.Warehouse.DTO.Warehouse { details = warehouse });

                var response = await warehouseRequestClient.MakeRequest(request, "CreateDefaultWarehouse").ConfigureAwait(false);

                if (response.Success)
                {
                    return JObject.Parse(response.Value.Content).GetValue("id").Value<string>();
                }
   
                return GenericResult.FromError<string>(response.Message);

            }
            catch (Exception ex)
            {
                return GenericResult.FromError<string>(ex);
            }
        }
    }
}
