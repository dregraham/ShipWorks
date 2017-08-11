using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.BuyDotCom.OnlineUpdating
{
    /// <summary>
    /// Data access for uploading Buy.com shipment data
    /// </summary>
    public interface IDataAccess
    {
        /// <summary>
        /// Get shipment data needed for uploading
        /// </summary>
        Task<IEnumerable<ShipmentUpload>> GetShipmentDataAsync(IEnumerable<long> shipmentKeys);
    }
}