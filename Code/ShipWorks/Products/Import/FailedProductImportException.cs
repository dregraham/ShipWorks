using System.Collections.Generic;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// Exception for a partially successful import
    /// </summary>
    public class FailedProductImportException : ProductImportException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FailedProductImportException(IImportProductsResult result) : base("Import failed")
        {
            SuccessCount = result.SuccessCount;
            NewCount = result.NewCount;
            ExistingCount = result.ExistingCount;
            FailedCount = result.FailedCount;
            FailedProducts = result.FailureResults ?? new Dictionary<string, string>();
        }

        /// <summary>
        /// Number of products successfully imported
        /// </summary>
        public int SuccessCount { get; }

        /// <summary>
        /// Number of new products
        /// </summary>
        public int NewCount { get; }

        /// <summary>
        /// Number of products that already existed
        /// </summary>
        public int ExistingCount { get; }

        /// <summary>
        /// Number of products that failed to import
        /// </summary>
        public int FailedCount { get; }

        /// <summary>
        /// Failed products and the reason for their failures
        /// </summary>
        public IDictionary<string, string> FailedProducts { get; }
    }
}
