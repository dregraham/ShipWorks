using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.None
{
    public class NoneLabelService:ILabelService
    {
        public void Create(ShipmentEntity shipment)
        {
            throw new ShippingException("No carrier is selected for the shipment.");
        }

        public void Void(ShipmentEntity shipment)
        {
        }
    }
}