using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Partial class extension of the LLBLGen ShippingProfileEntity
    /// </summary>
    public partial interface IShippingProfileEntity
    {
        /// <summary>
        /// Type of shipment
        /// </summary>
        ShipmentTypeCode ShipmentTypeCode { get; }
    }
}
