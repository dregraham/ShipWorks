using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal.Usps;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Stamps web client for DHL Express
    /// </summary>
    public interface IDhlExpressStampsWebClient
    {
        /// <summary>
        /// Create a label for the given shipment
        /// </summary>
        StampsLabelResponse CreateLabel(IShipmentEntity shipment);
    }
}
