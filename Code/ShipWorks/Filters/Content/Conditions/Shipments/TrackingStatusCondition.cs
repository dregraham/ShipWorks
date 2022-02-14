using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Shipments
{
    /// <summary>
    /// Condition that compares against the military address field of a shipment's ShipTo address
    /// </summary>
    [ConditionElement("Delivery Status", "Shipment.TrackingStatus")]
    public class TrackingStatusCondition : EnumCondition<TrackingStatus>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TrackingStatusCondition()
        {
            Value = TrackingStatus.Unknown;
        }

        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(ShipmentFields.TrackingStatus), context);
        }
    }
}