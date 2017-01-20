namespace ShipWorks.Filters.Search
{
    /// <summary>
    /// Prefix that identifies a scan result as a ShipWorks order
    /// </summary>
    public interface ISingleScanOrderPrefix
    {
        /// <summary>
        /// Get scan result that is displayed to user
        /// </summary>
        string GetDisplayText(string barcodeText);

        /// <summary>
        /// Whether or not the scan result starts with the ShipWorks order prefix
        /// </summary>
        bool Contains(string barcodeText);
    }
}