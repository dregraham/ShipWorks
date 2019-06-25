using System;
using System.Net;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Common.Net;
using ShipWorks.Data;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Link this instance of ShipWorks with a warehouse
    /// </summary>
    [Component]
    public class WarehouseLink : IWarehouseLink
    {
        private readonly WarehouseRequestClient warehouseRequestClient;
        private readonly IDatabaseIdentifier databaseIdentifier;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseLink(WarehouseRequestClient warehouseRequestClient, IDatabaseIdentifier databaseIdentifier)
        {
            this.warehouseRequestClient = warehouseRequestClient;
            this.databaseIdentifier = databaseIdentifier;
        }

        /// <summary>
        /// Link this instance of ShipWorks with the given warehouse
        /// </summary>
        public async Task<Result> Link(string warehouseId)
        {
            try
            {
                IRestRequest restRequest = new RestRequest(WarehouseEndpoints.LinkWarehouse(warehouseId), Method.POST);
                restRequest.JsonSerializer = new RestSharpJsonNetSerializer();
                restRequest.RequestFormat = DataFormat.Json;
                restRequest.AddJsonBody(new LinkDatabaseDto { DatabaseId = databaseIdentifier.Get().ToString() });

                var response = await warehouseRequestClient.MakeRequest(restRequest, "LinkWarehouse").ConfigureAwait(false);
                return response
                    .Bind(x => x.StatusCode != HttpStatusCode.OK ? new Exception() : Result.FromSuccess());
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
