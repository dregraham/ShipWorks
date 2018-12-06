using System.Collections.Generic;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// Results from importing products
    /// </summary>
    public class ImportProductsResult
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
        public int TotalCount { get; private set; }

        /// <summary>
        /// Number of products successfully imported.
        /// </summary>
        public int SuccessCount { get; private set; }

        /// <summary>
        /// Number of products that failed importing.
        /// </summary>
        public int FailedCount { get; private set; }

        /// <summary>
        /// Number of products added
        /// </summary>
        public int NewCount { get; private set; }

        /// <summary>
        /// Number of products updated
        /// </summary>
        public int ExistingCount { get; private set; }

        /// <summary>
        /// List of SKU and reason for failure.
        /// </summary>
        public Dictionary<string, string> FailureResults { get; }

        /// <summary>
        /// A product failed while importing
        /// </summary>
        public void ProductFailed(string sku, string message)
        {
            FailedCount += 1;
            FailureResults.Add(sku, message);
        }

        /// <summary>
        /// A product import succeeded
        /// </summary>
        public void ProductSucceeded(bool wasNew)
        {
            if (wasNew)
            {
                NewCount += 1;
            }
            else
            {
                ExistingCount += 1;
            }

            SuccessCount += 1;
        }
    }
}
