namespace ShipWorks.Filters.Search
{
    /// <summary>
    /// Prefix that identifies a scan result as a ShipWorks order
    /// </summary>
    public interface ISingleScanOrderPrefix
    {
        /// <summary>
        /// Scan result that is displayed to user
        /// </summary>
        string DisplayText { get; set; }

        /// <summary>
        /// Actual scan result
        /// </summary>
        string OriginalSearchText { get; set; }

        /// <summary>
        /// Whether or not the scan result starts with the ShipWorks order prefix
        /// </summary>
        bool Contains();
    }
}