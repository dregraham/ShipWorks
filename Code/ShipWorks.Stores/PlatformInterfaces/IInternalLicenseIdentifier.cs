using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.PlatforInterfaces
{
    public interface IInternalLicenseIdentifierFactory
    {
        string Create(StoreEntity store);
    }
}