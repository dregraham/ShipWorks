using System;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Common.Net;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Editions;
using ShipWorks.Products.Warehouse.DTO;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Client for interacting with the warehouse in regard to stores
    /// </summary>
    [Component(RegistrationType.Self)]
    public class WarehouseProductClient : IWarehouseProductClient
    {
        private readonly IWarehouseRequestClient warehouseRequestClient;
        private readonly IConfigurationData configurationData;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseProductClient(IWarehouseRequestClient warehouseRequestClient, IConfigurationData configurationData)
        {
            this.warehouseRequestClient = warehouseRequestClient;
            this.configurationData = configurationData;
        }

        /// <summary>
        /// Update the stores credentials
        /// </summary>
        public async Task<string> AddProduct(IProductVariantEntity product)
        {
            string warehouseId = configurationData.FetchReadOnly().WarehouseID;
            IRestRequest request = new RestRequest(WarehouseEndpoints.AddProduct, Method.PUT)
            {
                JsonSerializer = RestSharpJsonNetSerializer.CreateHubDefault(),
                RequestFormat = DataFormat.Json
            };

            request.AddJsonBody(new AddProductRequestData(product, warehouseId));

            return await warehouseRequestClient
                .MakeRequest<AddProductResponseData>(request, "Upload Store")
                .Map(x => x.ProductId)
                .ConfigureAwait(true);
        }
    }
}
