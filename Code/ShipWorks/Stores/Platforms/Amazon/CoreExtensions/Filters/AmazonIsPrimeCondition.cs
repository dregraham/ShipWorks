using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.Amazon.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the amazon fulfillment channel field
    /// </summary>
    [ConditionElement("Amazon Prime", "Amazon.IsPrime")]
    [ConditionStoreType(StoreTypeCode.Amazon)]
    public class AmazonIsPrimeCondition : EnumCondition<AmazonIsPrime>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonIsPrimeCondition()
        {
            Value = AmazonIsPrime.Yes;
        }

        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            // We have to get from Order -> AmazonOrder
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, AmazonOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(AmazonOrderFields.IsPrime), context));
            }
        }
    }
}
