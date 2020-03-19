using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Products.Warehouse.DTO;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Data factory for making Warehouse product requests
    /// </summary>
    public interface IWarehouseProductDataFactory
    {
        /// <summary>
        /// Create an AddProductRequestData object
        /// </summary>
        IWarehouseProductRequestData CreateAddProductRequest(IProductVariantEntity product);

        /// <summary>
        /// Create a change result from an AddProductDataResponse
        /// </summary>
        IProductChangeResult CreateAddProductResult(AddProductResponseData response);

        /// <summary>
        /// Create a ChangeProductRequestData object
        /// </summary>
        IWarehouseProductRequestData CreateChangeProductRequest(IProductVariantEntity product);

        /// <summary>
        /// Create an UploadProductRequestData object
        /// </summary>
        IWarehouseProductRequestData CreateUploadProductsRequest(IEnumerable<IProductVariantEntity> products);

        /// <summary>
        /// Create a change result from a ChangeProductDataResponse
        /// </summary>
        IProductChangeResult CreateChangeProductResult(ChangeProductResponseData response);

        /// <summary>
        /// Create a SetActivationBulkRequestData object
        /// </summary>
        IWarehouseProductRequestData CreateSetActivationRequest(IEnumerable<Guid> productIdList, bool activated);

        /// <summary>
        /// Create a change result from a SetActivationBulkResponse
        /// </summary>
        IProductsChangeResult CreateSetActivationResult(SetActivationBulkResponseData response);

        /// <summary>
        /// Create a change result from an UploadResponse
        /// </summary>
        IProductsChangeResult CreateUploadResult(UploadResponseData response);

        /// <summary>
        /// Create a get product request for the given product id
        /// </summary>
        IWarehouseProductRequestData CreateGetProductRequest(string hubProductId);

        /// <summary>
        /// Create request to get products after the given sequence
        /// </summary>
        /// <param name="sequence">Newest sequence in the db</param>
        /// <returns></returns>
        IWarehouseProductRequestData CreateGetProductsAfterSequenceRequest(long sequence);

        /// <summary>
        /// Create result for given response data
        /// </summary>
        /// <param name="data"></param>
        IGetProductsAfterSequenceResult CreateGetProductsAfterSequenceResult(GetProductsAfterSequenceResponseData data);
    }
}