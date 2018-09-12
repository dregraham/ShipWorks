namespace ShipWorks.Filters.Search
{
    /// <summary>
    /// Represents the SingleScanSearchDefinitionProvider
    /// </summary>
    public interface ISingleScanSearchDefinitionProvider
    {
        /// <summary>
        /// Generate the SQL for this search definition
        /// </summary>
        string GenerateSql(string scanMsgScannedText);
    }
}