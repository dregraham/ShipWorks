using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.SparkPay.DTO;

namespace ShipWorks.Stores.Platforms.SparkPay.Factories
{
    /// <summary>
    /// SparkPay shipment factory
    /// </summary>
    public interface ISparkPayShipmentFactory
    {
        /// <summary>
        /// Create a SparkPay shipment
        /// </summary>
        Shipment Create(ShipmentEntity shipment, long orderNumber);
    }
}