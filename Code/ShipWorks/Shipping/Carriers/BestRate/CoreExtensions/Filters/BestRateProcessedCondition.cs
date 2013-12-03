using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Shipping.Carriers.BestRate.CoreExtensions.Filters
{
    /// <summary>
    ///     Filter for BestRateProcessedCondition
    /// </summary>
    [ConditionElement("Best rate used", "Shipment.BestRateProcessed")]
    internal class BestRateProcessedCondition : BooleanCondition
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BestRateProcessedCondition" /> class.
        /// </summary>
        public BestRateProcessedCondition()
            : base("Yes", "No")
        {}

        /// <summary>
        ///     Generate the SQL for the condition clement
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return string.Format("{0} & {1} {2} {1}",
                                 context.GetColumnReference(ShipmentFields.BestRateEvents),
                                 (int)BestRateEventTypes.RateAutoSelectedAndProcessed,
                                 GetSqlOperator());
        }

        /// <summary>
        ///     Gets the SQL operator.
        /// </summary>
        protected override string GetSqlOperator()
        {
            // When user selects Equals, True and False should be = and != respectively.
            // When user selects not equals, that is flip-flopped.
            return Operator == EqualityOperator.Equals ?
                       (Value ? "=" : "!=") : (Value ? "!=" : "=");
        }
    }
}