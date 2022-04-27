using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net.RestSharp;
using Interapptive.Shared.Utility;
using Newtonsoft.Json.Linq;
using RestSharp;
using ShipWorks.Shipping.ShipEngine.DTOs;
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
        private readonly IWarehouseRequestFactory warehouseRequestFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseCreate(IWarehouseRequestClient warehouseRequestClient, IWarehouseRequestFactory warehouseRequestFactory)
        {
            this.warehouseRequestClient = warehouseRequestClient;
            this.warehouseRequestFactory = warehouseRequestFactory;
        }

        /// <summary>
        /// Creates a warehouse on the hub and returns the id of the new warehouse
        /// </summary>
        public async Task<GenericResult<string>> Create(Details warehouse)
        {
            try
            {
                IRestRequest request = warehouseRequestFactory.Create(WarehouseEndpoints.Warehouses, Method.POST, new ShipWorks.ApplicationCore.Licensing.Warehouse.DTO.Warehouse { details = warehouse });
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
