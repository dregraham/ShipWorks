using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Create Amazon labels
    /// </summary>
    public interface IAmazonLabelService
    {
        /// <summary>
        /// Create a label
        /// </summary>
        void Create(ShipmentEntity shipment);
    }
}
