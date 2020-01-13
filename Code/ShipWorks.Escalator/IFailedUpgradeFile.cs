namespace ShipWorks.Escalator
{
    /// <summary>
    /// Keeps track if the upgrade failed or now using a file called "FailedAutoUpdate.txt"
    /// </summary>
    public interface IFailedUpgradeFile
    {
        /// <summary>
        /// Creates "FailedAutoUpdate.txt"
        /// </summary>
        void CreateFailedAutoUpdateFile();

        /// <summary>
        /// Deletes "FailedAutoUpdate.txt"
        /// </summary>
        void DeleteFailedAutoUpdateFile();
    }
}