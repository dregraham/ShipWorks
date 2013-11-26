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
    [ConditionElement("BestRate Proccessed Status", "Shipment.BestRateProcessed")]
    class BestRateProcessedCondition : EnumCondition<BestRateEventTypes>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BestRateProcessedCondition"/> class.
        /// </summary>
        public BestRateProcessedCondition()
        {
            Value=BestRateEventTypes.None;
        }

        /// <summary>
        /// Generate the SQL for the condition clement
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string param = context.RegisterParameter(Value);

            return string.Format("BestRateEvents & {0} {1} {0}", param, GetSqlOperator());
        }
    }
}
