using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Customers
{
    /// <summary>
    /// Condition based on the how much a customer has spent
    /// </summary>
    [ConditionElement("Amount Spent", "Customer.AmountSpent")]
    class AmountSpentCondition : MoneyCondition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmountSpentCondition()
        {
            // Can't have a a count lower than zero
            minimumValue = 0;
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
