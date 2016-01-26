using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Extensions on the LLBLgen entity
    /// </summary>
    public partial class ShippingProviderRuleEntity : IRuleEntity
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
