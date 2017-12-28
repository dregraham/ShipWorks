using System.Collections.Generic;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Services
{
    public interface ICarrierPostProcessingMessage
    {
        void Show(IEnumerable<IShipmentEntity> processedShipments);
    }
}