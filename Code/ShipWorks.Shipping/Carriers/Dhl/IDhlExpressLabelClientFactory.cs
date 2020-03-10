using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Factory for a IDhlExpressLabelClient
    /// </summary>
    public interface IDhlExpressLabelClientFactory
    {
        /// <summary>
        /// Create an IDhlExpressLabelClient
        /// </summary>
        IDhlExpressLabelClient Create(IShipmentEntity shipment);
    }
}
