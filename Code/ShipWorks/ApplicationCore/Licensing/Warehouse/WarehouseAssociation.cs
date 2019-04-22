using System;
using System.Net;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using RestSharp;
using ShipWorks.Data;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Associate this instance of ShipWorks with a warehouse
    /// </summary>
    [Component]
    public class WarehouseAssociation : IWarehouseAssociation
    {
        private readonly WarehouseRequestClient warehouseRequestClient;
        private readonly IDatabaseIdentifier databaseIdentifier;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseAssociation(WarehouseRequestClient warehouseRequestClient, IDatabaseIdentifier databaseIdentifier)
        {
            this.warehouseRequestClient = warehouseRequestClient;
            this.databaseIdentifier = databaseIdentifier;
        }

        /// <summary>
        /// Associate this instance of ShipWorks with the given warehouse
        /// </summary>
        public async Task<Result> Associate(string warehouseId)
        {
            try
            {
                RestRequest restRequest = new RestRequest(WarehouseEndpoints.AssociateWarehouse(warehouseId), Method.POST);
                restRequest.RequestFormat = DataFormat.Json;
                restRequest.AddJsonBody(new DatabaseDto { databaseId = databaseIdentifier.Get().ToString() });

                var response = await warehouseRequestClient.MakeRequest(restRequest).ConfigureAwait(false);
                return response
                    .Bind(x => x.StatusCode != HttpStatusCode.OK ? new Exception() : Result.FromSuccess());
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private class DatabaseDto
        {
            public string databaseId { get; set; }
        }
    }
}
