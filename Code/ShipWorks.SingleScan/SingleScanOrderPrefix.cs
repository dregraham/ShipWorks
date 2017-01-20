using ShipWorks.Filters.Search;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Prefix that identifies a scan result as a ShipWorks order
    /// </summary>
    /// <seealso cref="ShipWorks.Filters.Search.ISingleScanOrderPrefix" />
    public class SingleScanOrderPrefix : ISingleScanOrderPrefix
    {
        public const string Prefix = "SW-O-";

        /// <summary>
        /// Scan result that is displayed to user
        /// </summary>
        public string DisplayText { get; set; }

        /// <summary>
        /// Actual scan result
        /// </summary>
        public string OriginalSearchText { get; set; }

        /// <summary>
        /// Whether or not the scan result begins with the ShipWorks order prefix
        /// </summary>
        public bool Contains()
        {
            return OriginalSearchText.StartsWith(Prefix);
        }
    }
}