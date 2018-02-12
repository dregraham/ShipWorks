using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Filter for CombineSplitStatus
    /// </summary>
    [ConditionElement("Combined", "Order.Combined")]
    public class OrderCombinedCondition : EnumCondition<CombineSplitStatusType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderCombinedCondition"/> class.
        /// </summary>
        public OrderCombinedCondition()
        {
            Value = CombineSplitStatusType.None;
        }

        /// <summary>
        /// Generate the SQL for the condition clement
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            MethodConditions.EnsureArgumentIsNotNull(context, nameof(context));

            return base.GenerateSql(context.GetColumnReference(OrderFields.CombineSplitStatus), context);
        }
    }
}
