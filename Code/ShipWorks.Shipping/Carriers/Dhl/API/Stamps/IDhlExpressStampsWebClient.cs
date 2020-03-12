using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps;

namespace ShipWorks.Shipping.Carriers.Dhl.API.Stamps
{
    /// <summary>
    /// Stamps web client for DHL Express
    /// </summary>
    public interface IDhlExpressStampsWebClient
    {
        /// <summary>
        /// Create a label for the given shipment
        /// </summary>
        Task<TelemetricResult<StampsLabelResponse>> ProcessShipment(ShipmentEntity shipment);

        /// <summary>
        /// Void the given shipment
        /// </summary>
        void VoidShipment(ShipmentEntity shipment);
    }
}
