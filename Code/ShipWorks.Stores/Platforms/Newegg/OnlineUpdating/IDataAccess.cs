using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Newegg.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Newegg
{
    /// <summary>
    /// Data access necessary for Newegg data uploads
    /// </summary>
    public interface IDataAccess
    {
        /// <summary>
        /// Load shipment details
        /// </summary>
        Task<ShipmentUploadDetails> LoadShipmentDetailsAsync(IShipmentEntity shipmentEntity);
    }
}