using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.Warehouse.DTO;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Result of an AddProduct request
    /// </summary>
    public class AddProductResult : IProductChangeResult
    {
        private readonly AddProductResponseData data;

        /// <summary>
        /// Constructor
        /// </summary
        public AddProductResult(AddProductResponseData data)
        {
            this.data = data;
        }

        /// <summary>
        /// Apply changes to the product variant
        /// </summary>
        public void ApplyTo(ProductVariantEntity productVariant)
        {
            productVariant.HubProductId = Guid.Parse(data.ProductId);
            productVariant.HubVersion = data.Version;
        }
    }
}
