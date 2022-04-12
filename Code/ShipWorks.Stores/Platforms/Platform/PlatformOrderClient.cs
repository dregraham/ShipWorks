using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Platform.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Platform
{
    /// <summary>
    /// Class to communicate with platform regarding orders
    /// </summary>
    [KeyedComponent(typeof(IUploadPlatformShipment), UploadPlatformShipmentType.DirectToPlatform)]
    public class PlatformOrderClient : IUploadPlatformShipment
    {
        /// <summary>
        /// Notify platform that an order has shipped
        /// </summary>
        public Task<Result> NotifyShipped(string salesOrderId, string trackingNumber, string carrier) => throw new NotImplementedException();
    }
}
