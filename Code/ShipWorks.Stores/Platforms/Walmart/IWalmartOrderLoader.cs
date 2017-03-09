using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Walmart.DTO;

namespace ShipWorks.Stores.Platforms.Walmart
{
    public interface IWalmartOrderLoader
    {
        void LoadOrder(Order downloadedOrder, WalmartOrderEntity orderToSave);
    }
}