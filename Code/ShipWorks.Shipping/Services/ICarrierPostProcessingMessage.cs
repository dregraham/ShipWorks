using System.Collections.Generic;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Interface that represents a post processing message
    /// </summary>
    public interface ICarrierPostProcessingMessage
    {
        /// <summary>
        /// Show a message for the given shipments
        /// </summary>
        void Show(IEnumerable<IShipmentEntity> processedShipments);
    }
}