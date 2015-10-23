namespace ShipWorks.Stores.Services
{
    public class StoreTypeManagerWrapper : IStoreTypeManager
    {
        public StoreType GetType(StoreTypeCode typeCode) => StoreTypeManager.GetType(typeCode);
    }
}