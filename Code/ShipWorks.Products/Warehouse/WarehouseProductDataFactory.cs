using System;
using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Products.Warehouse.DTO;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Data factory for making Warehouse product requests
    /// </summary>
    [Component]
    public class WarehouseProductDataFactory : IWarehouseProductDataFactory
    {
        /// <summary>
        /// Create an AddProductRequestData object
        /// </summary>
        public IWarehouseProductRequestData CreateAddProductRequest(IProductVariantEntity product) => 
            new AddProductRequestData(product);

        /// <summary>
        /// Create a change result from an AddProductDataResponse
        /// </summary>
        public IProductChangeResult CreateAddProductResult(AddProductResponseData response) =>
            new AddProductResult(response);

        /// <summary>
        /// Create a ChangeProductRequestData object
        /// </summary>
        public IWarehouseProductRequestData CreateChangeProductRequest(IProductVariantEntity product) => 
            new ChangeProductRequestData(product);

        /// <summary>
        /// Create an UploadProductRequestData object
        /// </summary>
        public IWarehouseProductRequestData CreateUploadProductsRequest(IEnumerable<IProductVariantEntity> products) => 
            new UploadProductsRequestData(products);

        /// <summary>
        /// Create a change result from a ChangeProductDataResponse
        /// </summary>
        public IProductChangeResult CreateChangeProductResult(ChangeProductResponseData response) => 
            new ChangeProductResult(response);

        /// <summary>
        /// Create a SetActivationBulkRequestData object
        /// </summary>
        public IWarehouseProductRequestData CreateSetActivationRequest(IEnumerable<Guid> productIdList, bool activated) =>
            new SetActivationBulkRequestData(productIdList, activated);

        /// <summary>
        /// Create a change result from a SetActivationBulkResponse
        /// </summary>
        public IProductsChangeResult CreateSetActivationResult(SetActivationBulkResponseData response) =>
            new SetActivationBulkResult(response);

        /// <summary>
        /// Create a change result from an UploadResponse
        /// </summary>
        public IProductsChangeResult CreateUploadResult(UploadResponseData response) =>
            new UploadProductsResult(response);
    }
}