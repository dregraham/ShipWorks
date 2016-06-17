using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Interface for rules
    /// </summary>
    public interface IRuleEntity
    {
        /// <summary>
        /// Id of the related filter node
        /// </summary>
        long FilterNodeID { get; }

        /// <summary>
        /// Shipment type to which the rule applies
        /// </summary>
        ShipmentTypeCode ShipmentTypeCode { get; set; }
    }
}