using System;
using System.Reflection;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Products.Warehouse.DTO
{
    /// <summary>
    /// A DTO that represents a bundled product
    /// </summary>
    [Obfuscation]
    public class BundledProduct
    {
        /// <summary>
        /// Create a new BundledProduct
        /// </summary>
        public static BundledProduct Create(IProductBundleEntity bundledProduct) => 
            new BundledProduct(bundledProduct);

        /// <summary>
        /// Constructor
        /// </summary>
        private BundledProduct(IProductBundleEntity bundledProduct)
        {
            Quantity = bundledProduct.Quantity;
            ProductId = bundledProduct.ChildVariant.HubProductId?.ToString("D");
        }

        /// <summary>
        /// Quantity of the items in the bundle
        /// </summary>
        public int Quantity { get; private set; }

        /// <summary>
        /// Id of the bundled product
        /// </summary>
        public string ProductId { get; private set; }
    }
}