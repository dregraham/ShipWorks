using System.Collections.Generic;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// Results from importing products
    /// </summary>
    public interface IImportProductsResult
    {
        /// <summary>
        /// Number of products updated
        /// </summary>
        int ExistingCount { get; }

        /// <summary>
        /// Number of products that failed importing.
        /// </summary>
        int FailedCount { get; }

        /// <summary>
        /// List of SKU and reason for failure.
        /// </summary>
        IDictionary<string, string> FailureResults { get; }

        /// <summary>
        /// Number of products added
        /// </summary>
        int NewCount { get; }

        /// <summary>
        /// Number of products successfully imported.
        /// </summary>
        int SuccessCount { get; }

        /// <summary>
        /// Total number of products attempted to import
        /// </summary>
        int TotalCount { get; }
    }
}