using System.Collections.Generic;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    public interface ICarrierPostProcessingMessage
    {
        void Show(IEnumerable<IShipmentEntity> processedShipments);
    }
}