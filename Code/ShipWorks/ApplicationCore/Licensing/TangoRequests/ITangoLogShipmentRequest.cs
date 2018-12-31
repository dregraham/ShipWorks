using System.Data.Common;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.ApplicationCore.Licensing.TangoRequests
{
    /// <summary>
    /// Log a shipment to Tango
    /// </summary>
    public interface ITangoLogShipmentRequest
    {
        /// <summary>
        /// Log the given processed shipment to Tango.
        /// </summary>
        Result LogShipment(DbConnection connection, StoreEntity store, ShipmentEntity shipment);
    }
}