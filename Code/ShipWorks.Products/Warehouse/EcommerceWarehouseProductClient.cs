using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;

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
        public Task<IProductChangeResult> AddProduct(IProductVariantEntity product) => Task.FromResult(NullProductResult.Default);

        /// <summary>
        /// Change a product on the Hub
        /// </summary>
        public Task<IProductChangeResult> ChangeProduct(IProductVariantEntity product) => Task.FromResult(NullProductResult.Default);
    }
}
