using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Products.Warehouse.DTO;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Web client for product related interactions with the ShipWorks Warehouse app
    /// </summary>
    public interface IWarehouseProductClient
    {
        /// <summary>
        /// Add a product to the Hub
        /// </summary>
        Task<IProductChangeResult> AddProduct(IProductVariantEntity product);

        /// <summary>
        /// Change a product on the Hub
        /// </summary>
        Task<IProductChangeResult> ChangeProduct(IProductVariantEntity product);

        /// <summary>
        /// Enable or disable the given products
        /// </summary>
        Task<IProductsChangeResult> SetActivation(IEnumerable<Guid?> productIDs, bool activation);

        /// <summary>
        /// Upload products to the Hub
        /// </summary>
        Task<IProductsChangeResult> Upload(IEnumerable<IProductVariantEntity> products);

        /// <summary>
        /// Get a product from the Hub
        /// </summary>
        Task<WarehouseProduct> GetProduct(string hubProductId, CancellationToken cancellationToken);

        /// <summary>
        /// Get products from the Hub for this warehouse after the given sequence
        /// </summary>
        Task<IGetProductsAfterSequenceResult> GetProductsAfterSequence(long sequence, CancellationToken cancellationToken);
    }
}
