using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services
{
    public interface ISingleScanShipmentConfirmationService
    {
        Task<IEnumerable<ShipmentEntity>> GetShipments(long OrderId, string scannedBarcode);
    }
}