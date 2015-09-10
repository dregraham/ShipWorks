using ShipWorks.Data.Model.EntityClasses;
using System.Threading.Tasks;

namespace ShipWorks.Shipping
{
    public interface IShipmentLoader
    {
        Task<ShippingPanelLoadedShipment> LoadAsync(OrderEntity order);
    }
}
