namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// Manage the archive notification
    /// </summary>
    public interface IArchiveNotificationManager
    {
        /// <summary>
        /// Show the archive notification banner, if necessary
        /// </summary>
        void ShowIfNecessary();

        /// <summary>
        /// Refresh the UI
        /// </summary>
        void RefreshUI();

        /// <summary>
        /// Reset the notification
        /// </summary>
        void Reset();
    }
}