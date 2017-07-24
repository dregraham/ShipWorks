using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Interface for a class that upgrades a store
    /// </summary>
    public interface IStoreUpgrade
    {
        // Upgrades the store
        void Upgrade(StoreEntity store);
    }
}