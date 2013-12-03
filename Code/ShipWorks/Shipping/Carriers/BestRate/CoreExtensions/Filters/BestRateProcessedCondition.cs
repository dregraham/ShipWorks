using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Shipping.Editing.Enums;

namespace ShipWorks.Shipping.Carriers.BestRate.CoreExtensions.Filters
{

    /// <summary>
    /// Filter for BestRateProcessedCondition
    /// </summary>
    [ConditionElement("Best rate used", "Shipment.BestRateProcessed")]
    class BestRateProcessedCondition : BooleanCondition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BestRateProcessedCondition"/> class.
        /// </summary>
        public BestRateProcessedCondition()
            : base("Yes", "No")
        { }

        /// <summary>
        /// Generate the SQL for the condition clement
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override string GenerateSql(SqlGenerationContext context)
        {

            return string.Format("BestRateEvents & {0} {1} {0}", (int)BestRateEventTypes.RateAutoSelectedAndProcessed,
                                 GetSqlOperator());
        }

        /// <summary>
        /// Gets the SQL operator.
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
