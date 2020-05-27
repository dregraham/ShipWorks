using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.Warehouse.DTO;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Result of an ChangeProduct request
    /// </summary>
    public class ChangeProductResult : IProductChangeResult
    {
        private readonly ChangeProductResponseData data;

        /// <summary>
        /// Constructor
        /// </summary
        public ChangeProductResult(ChangeProductResponseData data)
        {
            this.data = data;
        }

        /// <summary>
        /// Apply changes to the product variant
        /// </summary>
        public void ApplyTo(ProductVariantEntity productVariant)
        {
            productVariant.HubVersion = data.Version;
        }
    }
}
