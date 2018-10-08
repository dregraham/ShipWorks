using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Process voids for the shipment history view
    /// </summary>
    public interface IPreviousShipmentVoidActionHandler
    {
        /// <summary>
        /// Void a processed shipment
        /// </summary>
        GenericResult<ProcessedShipmentEntity> Void(ProcessedShipmentEntity shipment);

        /// <summary>
        /// Void last processed shipment
        /// </summary>
        Task<Unit> VoidLast();
    }
}