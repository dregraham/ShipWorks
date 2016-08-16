using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Partial class extention of the LLBLGen ShippingProfileEntity
    /// </summary>
    public partial class ShippingProfileEntity
    {
        /// <summary>
        /// Type of shipment
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode
        {
            get { return (ShipmentTypeCode)ShipmentType; }
            set { ShipmentType = (int)value; }
        }
    }
}
