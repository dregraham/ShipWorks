using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Shipping.ShipSense;

namespace ShipWorks.Filters.Content.Conditions.Shipments
{
    [ConditionElement("ShipSense", "Shipment.ShipSenseStatus")]
    public class ShipSenseStatusCondition : EnumCondition<ShipSenseStatus>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipSenseStatusCondition()
        {
            Value = ShipSenseStatus.NotApplied;
            SelectedValues = new[] { Value };
        }

        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(ShipmentFields.ShipSenseStatus), context);
        }
    }
}