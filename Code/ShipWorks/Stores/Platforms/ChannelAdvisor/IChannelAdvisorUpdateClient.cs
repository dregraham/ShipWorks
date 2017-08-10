using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.ChannelAdvisor.DTO;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Uses the soap or rest interface to update Channel Advisor shipments
    /// </summary>
    public interface IChannelAdvisorUpdateClient
    {
        /// <summary>
        /// Uses the soap or rest interface to update Channel Advisor shipments
        /// </summary>
        Task UploadShipmentDetails(ChannelAdvisorStoreEntity store, ChannelAdvisorShipment shipment, IOrderEntity order);
    }
}