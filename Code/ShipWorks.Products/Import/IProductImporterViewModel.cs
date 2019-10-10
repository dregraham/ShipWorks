using Interapptive.Shared.Utility;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// View model for importing products
    /// </summary>
    public interface IProductImporterViewModel
    {
        /// <summary>
        /// Import products
        /// </summary>
        Result ImportProducts();

        /// <summary>
        /// Import products
        /// </summary>
        Result ImportProducts(IProductImportState startingState);
    }
}