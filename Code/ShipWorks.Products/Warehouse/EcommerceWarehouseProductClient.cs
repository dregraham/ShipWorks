using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Products.Warehouse.DTO;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Warehouse product client for ecommerce users
    /// </summary>
    /// <remarks>This is basically a null object</remarks>
    [Component(RegistrationType.Self)]
    public class EcommerceWarehouseProductClient : IWarehouseProductClient
    {
        /// <summary>
        /// Add a product to the Hub
        /// </summary>
        public Task<IProductChangeResult> AddProduct(IProductVariantEntity product) => 
            Task.FromResult(NullProductResult.Default);

        /// <summary>
        /// Change a product on the Hub
        /// </summary>
        public Task<IProductChangeResult> ChangeProduct(IProductVariantEntity product) => 
            Task.FromResult(NullProductResult.Default);

        /// <summary>
        /// Get a product from the Hub
        /// </summary>
        public Task<WarehouseProduct> GetProduct(string hubProductId, CancellationToken cancellationToken) =>
            Task.FromResult<WarehouseProduct>(null);

        /// <summary>
        /// Get products from the Hub for this warehouse after the given sequence
        /// </summary>
        public Task<IGetProductsAfterSequenceResult> GetProductsAfterSequence(long sequence, CancellationToken cancellationToken) =>
            Task.FromResult(NullGetProductsAfterSequenceResult.Default);

        /// <summary>
        /// Enable or disable the given products
        /// </summary>
        public Task<IProductsChangeResult> SetActivation(IEnumerable<Guid?> productIDs, bool activation) =>
            Task.FromResult(NullProductsResult.Default);

        /// <summary>
        /// Upload products to the Hub
        /// </summary>
        public Task<IProductsChangeResult> Upload(IEnumerable<IProductVariantEntity> products) =>
            Task.FromResult(NullProductsResult.Default);

        /// <summary>
        /// Get a product from the Hub
        /// </summary>
        public Task<WarehouseProduct> GetProduct(string hubProductId, CancellationToken cancellationToken) =>
            Task.FromResult(new WarehouseProduct());

        /// <summary>
        /// Get products from the Hub for this warehouse after the given sequence
        /// </summary>
        public Task<IGetProductsAfterSequenceResult> GetProductsAfterSequence(long sequence, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
