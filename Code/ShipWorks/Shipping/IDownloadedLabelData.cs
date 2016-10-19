namespace ShipWorks.Shipping
{
    /// <summary>
    /// Label data that has been downloaded from a carrier
    /// </summary>
    public interface IDownloadedLabelData
    {
        /// <summary>
        /// Save label data to the database and/or disk
        /// </summary>
        void Save();
    }
}
