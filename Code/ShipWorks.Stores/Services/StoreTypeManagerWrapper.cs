namespace ShipWorks.Stores.Services
{
    /// <summary>
    /// Wrapper for static StoreTypeManager
    /// </summary>
    public class StoreTypeManagerWrapper : IStoreTypeManager
    {
        /// <summary>
        /// Gets the StoreType from a StoreTypeCode
        /// </summary>
        /// <param name="typeCode">The store type code.</param>
        public StoreType GetType(StoreTypeCode typeCode) => StoreTypeManager.GetType(typeCode);
    }
}