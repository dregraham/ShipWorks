using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Create Amazon labels
    /// </summary>
    public interface ILabelService
    {
        /// <summary>
        /// Create a label
        /// </summary>
        void Create(ShipmentEntity shipment);

        /// <summary>
        /// Voids the shipment
        /// </summary>
        void Void(ShipmentEntity shipment);
    }
}
