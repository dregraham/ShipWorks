using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Other
{
    /// <summary>
    /// Label Service for the Other carrier
    /// </summary>
    public class OtherLabelService : ILabelService
    { 
        /// <summary>
        /// Creates an Other label
        /// </summary>
        /// <param name="shipment"></param>
        public void Create(ShipmentEntity shipment)
        {
            if (string.IsNullOrWhiteSpace(shipment.Other.Carrier))
            {
                throw new ShippingException("No carrier is specified.");
            }

            if (string.IsNullOrWhiteSpace(shipment.Other.Service))
            {
                throw new ShippingException("No service is specified.");
            }
        }

        /// <summary>
        /// Voids the Other label
        /// </summary>
        /// <param name="shipment"></param>
        public void Void(ShipmentEntity shipment)
        {
        }
    }
}