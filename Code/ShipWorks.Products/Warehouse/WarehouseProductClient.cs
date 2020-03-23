using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
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
        private readonly IWarehouseProductRequestFactory requestFactory;
        private readonly IWarehouseProductDataFactory dataFactory;
        private readonly IConfigurationData configurationData;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseProductClient(
            IWarehouseRequestClient warehouseRequestClient, 
            IWarehouseProductRequestFactory requestFactory, 
            IWarehouseProductDataFactory dataFactory,
            IConfigurationData configurationData)
        {
            this.warehouseRequestClient = warehouseRequestClient;
            this.requestFactory = requestFactory;
            this.dataFactory = dataFactory;
            this.configurationData = configurationData;
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
        /// Get a product from the Hub
        /// </summary>
        public async Task<WarehouseProduct> GetProduct(string hubProductId, CancellationToken cancellationToken)
        {
            var payload = dataFactory.CreateGetProductRequest(hubProductId);
            var request = requestFactory.Create(WarehouseEndpoints.GetProduct(hubProductId), Method.GET, payload);

            return await warehouseRequestClient
                .MakeRequest<WarehouseProduct>(request, "Get Product")
                .ConfigureAwait(true);
        }

        /// <summary>
        /// Get products from the Hub for this warehouse after the given sequence
        /// </summary>
        public async Task<IGetProductsAfterSequenceResult> GetProductsAfterSequence(long sequence, CancellationToken cancellationToken)
        {
            var warehouseId = configurationData.FetchReadOnly().WarehouseID;
            var request = requestFactory.Create(WarehouseEndpoints.GetProductsAfterSequence(warehouseId, sequence), Method.GET);

            return await warehouseRequestClient
                .MakeRequest<GetProductsAfterSequenceResponseData>(request, "Get Products After Sequence", cancellationToken)
                .Map(dataFactory.CreateGetProductsAfterSequenceResult)
                .ConfigureAwait(false);
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
            if (products?.WhereNotNull().Any() != true)
            {
                return NullProductsResult.Default;
            }

            var payload = dataFactory.CreateUploadRequest(products);
            var request = requestFactory.Create(WarehouseEndpoints.UploadProducts, Method.POST, payload);
            request.AddHeader("version", "2");

            return await warehouseRequestClient
                .MakeRequest<UploadResponseData>(request, "UploadSkusToWarehouse")
                .Map(dataFactory.CreateUploadResult)
                .ConfigureAwait(true);
        }
    }
}
