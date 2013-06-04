using System;
using System.Collections.Generic;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Customers
{
    /// <summary>
    /// Condition based on the how much a customer has spent
    /// </summary>
    [ConditionElement("Amount Spent", "Customer.AmountSpent")]
    class AmountSpentCondition : NumericCondition<decimal>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmountSpentCondition()
        {
            // Can't have a a count lower than zero
            minimumValue = 0;

            // Format as currency
            format = "C";
        }

        /// <summary>
        /// Generate the SQL for the condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(CustomerFields.RollupOrderTotal), context);
        }
    }
}
