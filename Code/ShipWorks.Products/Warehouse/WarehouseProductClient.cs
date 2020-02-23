using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Common.Net;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityInterfaces;
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
        /// Add a product to the Hub
        /// </summary>
        public async Task<IProductChangeResult> AddProduct(IProductVariantEntity product)
        {
            string warehouseId = configurationData.FetchReadOnly().WarehouseID;
            IRestRequest request = new RestRequest(WarehouseEndpoints.AddProduct, Method.PUT)
            {
                JsonSerializer = RestSharpJsonNetSerializer.CreateHubDefault(),
                RequestFormat = DataFormat.Json
            };

            request.AddJsonBody(new AddProductRequestData(product, warehouseId));

            return await warehouseRequestClient
                .MakeRequest<AddProductResponseData>(request, "Add Product")
                .Map(x => new AddProductResult(x))
                .ConfigureAwait(true);
        }

        /// <summary>
        /// Change a product on the Hub
        /// </summary>
        public async Task<IProductChangeResult> ChangeProduct(IProductVariantEntity product)
        {
            string warehouseId = configurationData.FetchReadOnly().WarehouseID;
            IRestRequest request = new RestRequest(WarehouseEndpoints.ChangeProduct(product), Method.POST)
            {
                JsonSerializer = RestSharpJsonNetSerializer.CreateHubDefault(),
                RequestFormat = DataFormat.Json
            };

            request.AddJsonBody(new ChangeProductRequestData(product, warehouseId));

            return await warehouseRequestClient
                .MakeRequest<ChangeProductResponseData>(request, "Change Product")
                .Map(x => new ChangeProductResult(x))
                .ConfigureAwait(true);
        }
    }
}
