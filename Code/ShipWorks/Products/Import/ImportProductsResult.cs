using System.Collections.Generic;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// Results from importing products
    /// </summary>
    public struct ImportProductsResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ImportProductsResult(int totalCount, int successCount, int failedCount)
        {
            TotalCount = totalCount;
            SuccessCount = successCount;
            FailedCount = failedCount;
            FailureResults = new Dictionary<string, string>();
        }

        /// <summary>
        /// Total number of products attempted to import
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Number of products successfully imported.
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// Number of products that failed importing.
        /// </summary>
        public int FailedCount { get; set; }

        /// <summary>
        /// List of SKU and reason for failure.
        /// </summary>
        public Dictionary<string, string> FailureResults { get; set; }
    }
}
