using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
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
        private readonly IWarehouseProductRequestFactory requestFactory;
        private readonly IWarehouseProductDataFactory dataFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseProductClient(IWarehouseRequestClient warehouseRequestClient, IWarehouseProductRequestFactory requestFactory, IWarehouseProductDataFactory dataFactory)
        {
            this.warehouseRequestClient = warehouseRequestClient;
            this.requestFactory = requestFactory;
            this.dataFactory = dataFactory;
        }

        /// <summary>
        /// Add a product to the Hub
        /// </summary>
        public async Task<IProductChangeResult> AddProduct(IProductVariantEntity product)
        {
            var payload = dataFactory.CreateAddProductRequest(product);
            var request = requestFactory.Create(WarehouseEndpoints.AddProduct, Method.PUT, payload);

            return await warehouseRequestClient
                .MakeRequest<AddProductResponseData>(request, "Add Product")
                .Map(dataFactory.CreateAddProductResult)
                .ConfigureAwait(true);
        }

        /// <summary>
        /// Change a product on the Hub
        /// </summary>
        public async Task<IProductChangeResult> ChangeProduct(IProductVariantEntity product)
        {
            var payload = dataFactory.CreateChangeProductRequest(product);
            var request = requestFactory.Create(WarehouseEndpoints.ChangeProduct(product), Method.POST, payload);

            return await warehouseRequestClient
                .MakeRequest<ChangeProductResponseData>(request, "Change Product")
                .Map(dataFactory.CreateChangeProductResult)
                .ConfigureAwait(true);
        }

        /// <summary>
        /// Enable or disable the given products
        /// </summary>
        public async Task<IProductsChangeResult> SetActivation(IEnumerable<Guid?> productIDs, bool activation)
        {
            if (productIDs.Any(x => !x.HasValue))
            {
                throw new WarehouseProductException("Some of the products have not yet been saved to the Hub");
            }

            var payload = dataFactory.CreateSetActivationRequest(productIDs.Select(x => x.Value), activation);
            var request = requestFactory.Create(WarehouseEndpoints.SetActivationBulk, Method.POST, payload);

            return await warehouseRequestClient
                .MakeRequest<SetActivationBulkResponseData>(request, "Set Activation")
                .Map(dataFactory.CreateSetActivationResult)
                .ConfigureAwait(true);
        }

        /// <summary>
        /// Upload products to the Hub
        /// </summary>
        public async Task<IProductsChangeResult> Upload(IEnumerable<IProductVariantEntity> products)
        {
            if (products?.Any() != true)
            {
                return NullProductsResult.Default;
            }

            var payload = dataFactory.CreateUploadProductsRequest(products);
            var request = requestFactory.Create(WarehouseEndpoints.UploadProducts, Method.POST, payload);
            request.AddHeader("version", "2");

            return await warehouseRequestClient
                .MakeRequest<UploadResponseData>(request, "UploadSkusToWarehouse")
                .Map(dataFactory.CreateUploadResult)
                .ConfigureAwait(true);
        }
    }
}
