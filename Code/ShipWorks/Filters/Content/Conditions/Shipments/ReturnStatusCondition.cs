using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Shipments
{
    [ConditionElement("Is Return", "Shipment.ReturnStatus")]
    public class ReturnStatusCondition : EnumCondition<ReturnStatusType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ReturnStatusCondition()
        {
            Value = ReturnStatusType.ReturnShipment;
        }

        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(ShipmentFields.ReturnShipment), context);
        }
    }
}
