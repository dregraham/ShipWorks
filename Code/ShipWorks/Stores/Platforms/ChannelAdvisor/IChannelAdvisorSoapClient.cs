using System;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Interface for ChannelAdvisorSoapSoapClient
    /// </summary>
    public interface IChannelAdvisorSoapClient
    {
        /// <summary>
        /// Uploads the shipment details for the specified order
        /// </summary>
        void UploadShipmentDetails(int caOrderID, DateTime dateShipped, string carrierCode, string classCode, string trackingNumber);
    }
}