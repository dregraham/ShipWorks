using System;

namespace ShipWorks.Stores.Platforms.Newegg.Net
{
    /// <summary>
    /// Calculate page information
    /// </summary>
    public class PageCalculator
    {
        /// <summary>
        /// Calculates the page count required to capture all of the results.
        /// </summary>
        /// <param name="numberOfResults">The total number of results available.</param>
        /// <param name="maximumPageSize">Maximum number of results on a single page.</param>
        /// <returns>The number of pages required to capture all of the results.</returns>
        /// <exception cref="System.InvalidOperationException">Page size must be greater than zero.</exception>
        public int CalculatePageCount(int numberOfResults, int maximumPageSize)
        {
            if (maximumPageSize <= 0)
            {
                throw new InvalidOperationException("Page size must be greater than zero.");
            }

            // Calculate the total page count based on using the max page size since that is the
            // page size used in the actual order download: subtract 1 from the summation of the
            // max page size and the total orders before dividing by the max page size to handle
            // to account for integer division.
            // (e.g.: (100 + 42 - 1) / 100 = 1 page, (100 + 100 - 1) / 100 = 1 page)
            return (maximumPageSize + numberOfResults - 1) / maximumPageSize;
        }
    }
}
