using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    public interface IUpsOltShipmentValidator
    {
        void ValidateShipment(ShipmentEntity shipment);
    }
}