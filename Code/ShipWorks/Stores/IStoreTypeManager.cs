namespace ShipWorks.Stores
{
    public interface IStoreTypeManager
    {
        StoreType GetType(StoreTypeCode typeCode);
    }
}