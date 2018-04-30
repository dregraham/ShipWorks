namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// Order Archive Filter Regeneration
    /// </summary>
    public interface IOrderArchiveFilterRegenerator
    {
        /// <summary>
        /// Regenerate filters for an archive database
        /// </summary>
        void Regenerate();
    }
}