using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Connection;
using Interapptive.Shared.Utility;
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
        Task SetActivation(IEnumerable<long> productIDs, bool activation, IProgressReporter progressReporter);

        /// <summary>
        /// Fetch a product variant based on SKU
        /// </summary>
        ProductVariantEntity FetchProductVariantEntity(ISqlAdapter sqlAdapter, string sku);

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
        Task<Result> Save(ProductVariantEntity product, ISqlAdapterFactory sqlAdapterFactory);

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
    }
}