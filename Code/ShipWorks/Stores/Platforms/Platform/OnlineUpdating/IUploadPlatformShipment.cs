using System.Threading.Tasks;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Platform.OnlineUpdating
{
    /// <summary>
    /// Interface for classes that can upload shipments for platform stores
    /// </summary>
    public interface IUploadPlatformShipment
    {
        /// <summary>
        /// Notify platform that an order has shipped
        /// </summary>
        Task<Result> NotifyShipped(string salesOrderId, string trackingNumber, string carrier);
    }
}
