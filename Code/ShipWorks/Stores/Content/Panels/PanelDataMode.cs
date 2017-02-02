namespace ShipWorks.Stores.Content.Panels
{
    /// <summary>
    /// Controls how and where a panel gets and saves its data
    /// </summary>
    public enum PanelDataMode
    {
        /// <summary>
        /// Normal mode where results are paged in and saved live to the database
        /// </summary>
        LiveDatabase,

        /// <summary>
        /// Mode used for adding orders, so that all data is stored locally and not persisted
        /// to the database.
        /// </summary>
        LocalPending
    }
}
