using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.PlatforInterfaces
{
    public interface IStoreInstanceFactory
    {
        StoreEntity CreateStoreInstance();
    }
}