using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.EntityClasses
{
    public partial class ShippingDefaultsRuleEntity : IRuleEntity
    {
        /// <summary>
        /// Strongly typed shipment type
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode
        {
            get { return (ShipmentTypeCode) ShipmentType; }
            set { ShipmentType = (int) value; }
        }
    }
}
