using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.OrderLookup.ShipmentHistory
{
    /// <summary>
    /// Process voids for the shipment history view
    /// </summary>
    public interface IShipmentHistoryVoidProcessor
    {
        /// <summary>
        /// Void a processed shipment
        /// </summary>
        GenericResult<ProcessedShipmentEntity> Void(ProcessedShipmentEntity shipment);
    }
}