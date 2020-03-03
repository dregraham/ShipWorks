using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    /// <summary>
    /// Create a IUpsShipmentValidator
    /// </summary>
    public interface IUpsShipmentValidatorFactory
    {
        /// <summary>
        /// Create a IUpsShipmentValidator
        /// </summary>
        IUpsShipmentValidator Create(IShipmentEntity shipment);
    }
}
