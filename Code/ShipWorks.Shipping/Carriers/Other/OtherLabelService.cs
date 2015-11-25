using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Other
{
    public class OtherLabelService : ILabelService
    { 
        public void Create(ShipmentEntity shipment)
        {
            if (shipment.Other.Carrier.Trim().Length == 0)
            {
                throw new ShippingException("No carrier is specified.");
            }

            if (shipment.Other.Service.Trim().Length == 0)
            {
                throw new ShippingException("No service is specified.");
            }
        }

        public void Void(ShipmentEntity shipment)
        {
            throw new System.NotImplementedException();
        }
    }
}