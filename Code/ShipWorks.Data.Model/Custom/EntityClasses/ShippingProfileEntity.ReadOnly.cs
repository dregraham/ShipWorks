using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Partial class extension of the LLBLGen ShippingProfileEntity
    /// </summary>
    public partial class ReadOnlyShippingProfileEntity
    {
        /// <summary>
        /// Type of shipment
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode => (ShipmentTypeCode) ShipmentType;
    }
}
