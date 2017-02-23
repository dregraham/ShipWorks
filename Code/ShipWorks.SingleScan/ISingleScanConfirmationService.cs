using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Metrics;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.SingleScan
{
    public interface ISingleScanConfirmationService
    {
        Task<bool> Apply(IEnumerable<ShipmentEntity> shipments, ITrackedDurationEvent trackedDurationEvent);
        bool ConfirmOrders(long orderId, int numberOfMatchedOrders, string scanText);
        Task<IEnumerable<ShipmentEntity>> GetShipments(long orderId, string scannedBarcode);
    }
}