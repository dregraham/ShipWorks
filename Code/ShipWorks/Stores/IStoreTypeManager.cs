namespace ShipWorks.Stores
{
    /// <summary>
    /// Interface for StoreTypeManager
    /// </summary>
    public interface IStoreTypeManager
    {
        /// <summary>
        /// Gets the StoreType from a StoreTypeCode
        /// </summary>
        /// <param name="typeCode">The type code.</param>
        /// <returns></returns>
        StoreType GetType(StoreTypeCode typeCode);
    }
}