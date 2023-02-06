using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Etsy
{
    public interface IEtsyAccountSettingsControlFactory
    {
        AccountSettingsControlBase Create(IStoreEntity store);
    }
}
