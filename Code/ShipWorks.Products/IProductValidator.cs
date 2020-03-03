using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Products
{
    /// <summary>
    /// Validator for Products
    /// </summary>
    public interface IProductValidator
    {
        /// <summary>
        /// Validate the given product variant
        /// </summary>
        Task<Result> Validate(ProductVariantEntity productVariant, IProductCatalog productCatalog);
    }
}