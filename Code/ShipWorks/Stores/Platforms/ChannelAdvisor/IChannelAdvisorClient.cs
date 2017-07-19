using System;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Interface for ChannelAdvisorClient
    /// </summary>
    public interface IChannelAdvisorClient
    {
        /// <summary>
        /// Uploads the shipment details for the specified order
        /// </summary>
        void UploadShipmentDetails(int caOrderID, DateTime dateShipped, string carrierCode, string classCode, string trackingNumber);
    }
}