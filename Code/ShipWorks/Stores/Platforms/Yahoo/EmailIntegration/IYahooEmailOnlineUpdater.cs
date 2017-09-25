using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Yahoo.EmailIntegration
{
    /// <summary>
    /// Responsible for updating the online status of Yahoo! orders
    /// </summary>
    public interface IYahooEmailOnlineUpdater
    {
        /// <summary>
        /// Upload the shipment details of the most recent shipment of the given order
        /// </summary>
        Task<IEnumerable<EmailOutboundEntity>> GenerateOrderShipmentUpdateEmail(long orderID);

        /// <summary>
        /// Upload the shipment details of the given shipment
        /// </summary>
        Task<IEnumerable<EmailOutboundEntity>> GenerateShipmentUpdateEmail(long shipmentID);
    }
}