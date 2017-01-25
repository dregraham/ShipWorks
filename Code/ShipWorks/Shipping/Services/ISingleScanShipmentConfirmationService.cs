using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services
{
    [Service]
    public interface ISingleScanShipmentConfirmationService
    {
        Task<IEnumerable<ShipmentEntity>> GetShipments(long OrderId, string scannedBarcode);
    }
}