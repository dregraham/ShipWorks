using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for logging shipments to tango.  Recovers from failures as well.
    /// </summary>
    public interface ITangoLogShipmentProcessor
    {
        /// <summary>
        /// Add a log shipment task to the queue to process
        /// </summary>
        void Add(StoreEntity store, ShipmentEntity shipment);
    }
}
