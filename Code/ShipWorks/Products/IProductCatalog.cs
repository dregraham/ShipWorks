using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Products
{
    /// <summary>
    /// Product Catalog Interface
    /// </summary>
    public interface IProductCatalog
    {
        /// <summary>
        /// Fetch a product variant wrapper
        /// </summary>
        IProductVariant FetchProductVariant(ISqlAdapter sqlAdapter, string sku);

        /// <summary>
        /// Set given products activation to specified value
        /// </summary>
        Task SetActivation(IEnumerable<long> productIDs, bool activation);

        /// <summary>
        /// Fetch a product variant based on SKU
        /// </summary>
        ProductVariantEntity FetchProductVariantEntity(ISqlAdapter sqlAdapter, string sku);

        /// <summary>
        /// Fetch product variants based on a collection of skus
        /// </summary>
        Task<IEnumerable<ProductVariantEntity>> FetchProductVariantEntities(ISqlAdapter sqlAdapter, IEnumerable<string> skus);

        /// <summary>
        /// Fetch a products siblings
        /// </summary>
        Task<IEnumerable<IProductVariantEntity>> FetchSiblingVariants(IProductVariantEntity productVariant, ISqlAdapter sqlAdapter);

        /// <summary>
        /// Fetch a product variant based on ProductVariantID
        /// </summary>
        ProductVariantEntity FetchProductVariantEntity(ISqlAdapter sqlAdapter, long productVariantID);

        /// <summary>
        /// Save the given product
        /// </summary>
        Task<Result> Save(ProductVariantEntity product);

        /// Get a DataTable of products from the database
        /// </summary>
        Task<DataTable> GetProductDataForExport();

        /// <summary>
        /// Create a variant based on the given variant
        /// </summary>
        GenericResult<ProductVariantEntity> CloneVariant(ProductVariantEntity productVariant);

        /// <summary>
        /// Fetch a product attribute based on name
        /// </summary>
        ProductAttributeEntity FetchProductAttribute(ISqlAdapter sqlAdapter, string name, long productID);

        /// <summary>
        /// Get the available attributes for a variant
        /// </summary>
        Task<IEnumerable<IProductAttributeEntity>> GetAvailableAttributesFor(ISqlAdapter sqlAdapter, ProductVariantEntity variant);

        /// <summary>
        /// Reset the NeedsUploadToWarehouse flag for the given variants
        /// </summary>
        Task<int> ResetNeedsWarehouseUploadFlag(ISqlAdapter sqlAdapter, IEnumerable<IProductVariantEntity> variants);

        /// <summary>
        /// Fetch product variants to upload to the warehouse.
        /// </summary>
        Task<IEnumerable<IProductVariantEntity>> FetchProductVariantsForUploadToWarehouse(ISqlAdapter sqlAdapter, int pageSize);

        /// <summary>
        /// Fetch count of product variants to upload to the warehouse.
        /// </summary>
        Task<int> FetchProductVariantsForUploadToWarehouseCount(ISqlAdapter sqlAdapter);

        /// <summary>
        /// Fetch the newest sequence number of all the products
        /// </summary>
        Task<long> FetchNewestSequence(ISqlAdapter sqlAdapter);
    }
}