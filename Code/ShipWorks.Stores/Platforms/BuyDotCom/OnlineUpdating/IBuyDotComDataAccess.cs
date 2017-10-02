using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.BuyDotCom.OnlineUpdating
{
    /// <summary>
    /// Data access for uploading Buy.com shipment data
    /// </summary>
    public interface IBuyDotComDataAccess
    {
        /// <summary>
        /// Get shipment data needed for uploading
        /// </summary>
        Task<IEnumerable<BuyDotComShipmentUpload>> GetShipmentDataByOrderAsync(IEnumerable<long> orderKeys);

        /// <summary>
        /// Get shipment data needed for uploading
        /// </summary>
        Task<IEnumerable<BuyDotComShipmentUpload>> GetShipmentDataByShipmentAsync(IEnumerable<long> shipmentKeys);
    }
}